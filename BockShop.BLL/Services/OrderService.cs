using Azure.Core;
using BockShop.BLL.DTOs.Request;
using BockShop.BLL.DTOs.Response;
using BockShop.BLL.Exceptions;
using BockShop.BLL.Interfaces;
using BockShop.BLL.Specifications;
using BockShop.DAL.Interfaces;
using BookShop.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BockShop.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateOrderRequest> _createOrderRequestValidator;
        private readonly IValidator<UpdateOrderStatusRequest> _updateOrderStatusRequest;

        public OrderService(IValidator<UpdateOrderStatusRequest> updateOrderStatusRequest, IUnitOfWork unitOfWork, IValidator<CreateOrderRequest> createOrderRequestValidator)
        {
            _unitOfWork = unitOfWork;
            _createOrderRequestValidator = createOrderRequestValidator;
            _updateOrderStatusRequest = updateOrderStatusRequest;
        }
        public async Task CreateOrderAsync(CreateOrderRequest dto, string userId)
        {
            var validator = await _createOrderRequestValidator.ValidateAsync(dto);
            if (!validator.IsValid)
                throw new ValidationException(validator.Errors);


            var cart = await _unitOfWork.Cart.GetFirstOneAsync(e => e.UserId == userId);
            if (cart == null)
                throw new NotFoundException("Cart is not found.");

            if (!cart.cartItems.Any())
                throw new ValidationException(new[]
                {
                     new ValidationFailure(nameof(cart), "Cart is empty.")
                 });
            var cartSpec = new CartItemWithBookSpecification(cart.Id);
            var cartItem = await _unitOfWork.CartItem.GetAll(cartSpec);
            foreach (var item in cartItem)
            {
                if (item.Quantity > item.Book.Stock)
                    throw new ValidationException(new[]
                    {
                         new ValidationFailure(nameof(item.Quantity),
                       $"'{item.Book.Title}' has only {item.Book.Stock} items available.")
                    });

            }



            //6.Validate Coupon
            var coupon = await _unitOfWork.Coupon.GetFirstOneAsync(e => e.Code.ToLower() == dto.CouponCode!.ToLower().Trim());
            if (coupon == null)
                throw new Exception("InValid Coupon");
            if (!coupon.IsActive || coupon.CountUsed >= coupon.UsageLimit || coupon.ExpiredAt <= DateTime.UtcNow)
                throw new Exception("Coupon is not Active");
            var order = new Order
            {
                UserId = userId,
                CouponCode = coupon.Code,
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatus.Pending,
            };

            foreach (var item in cartItem)
            {
                order.orderItems!.Add(new OrderItem
                {
                    BookId = item.Id,
                    BookTitle = item.Book.Title,
                    UnitPrice = (decimal)item.Book.Price,
                    TotalPrice = item.Quantity * (decimal)item.Book.Price,
                    Quantity = item.Quantity,
                });
                item.Book.Stock -= item.Quantity;
                order.SubTotal += (decimal)item.TotalPrice;
                _unitOfWork.Book.Update(item.Book);
            }
            var couponUsage = await _unitOfWork.CouponUsage.GetFirstOneAsync(e => e.userId == userId
            && e.Code == coupon.Code);
            if (couponUsage != null)
                throw new Exception("Coupon is Used");

            order.Discount = (coupon.DiscountValue / 100) * order.SubTotal;
            order.TotalPrice = order.SubTotal - order.Discount;

            await _unitOfWork.Order.AddAsync(order);
            await _unitOfWork.CouponUsage.AddAsync(new CouponUsage
            {
                Code = coupon.Code,
                CouponId = coupon.Id,
                userId = userId,
                UsedAt = DateTime.UtcNow,
            });
            coupon.CountUsed++;
            foreach (var item in cartItem)
                _unitOfWork.CartItem.Delete(item);
            await _unitOfWork.SaveChangeAsync();


        }
        public async Task<IEnumerable<OrdersResponse>> GetAllOrdersAsync(OrderQueryParameters dto, string userId)
        {
            var orderSpec = new OrderSpecification(dto, userId);

            var orders = await _unitOfWork.Order.GetAll(orderSpec);
            return orders.Select(e => new OrdersResponse
            {
                Id = e.Id,
                CreateAt = DateTime.UtcNow,
                status = e.Status,
                TotalPrice = e.TotalPrice,
            });

        }
        public async Task<OrderDetailsResponse> GetOrderById(int Id, string userId)
        {
            if (Id <= 0)
                throw new ValidationException(new[] {
                    new ValidationFailure(nameof(Id),"Id Must Be Greater Than 0 "),
                });

            var orderSpec = new OrderWithItemSpecification(Id, userId);
            var order = await _unitOfWork.Order.GetOne(orderSpec);
            if (order == null)
                throw new NotFoundException("Order Not Found");
            return new OrderDetailsResponse
            {
                CouponCode = order.CouponCode,
                CreateAt = order.CreatedAt,
                SubTotal = order.SubTotal,
                Discount = order.Discount,
                TotalPrice = order.TotalPrice,
                Status = order.Status,
                Items = order.orderItems!.Select(e => new OrderItemResponse
                {
                    BookTitle = e.BookTitle,
                    Quantity = e.Quantity,
                    TotalPrice = e.TotalPrice,
                    UnitPrice = e.UnitPrice,
                    ASIN = e.Book.ASIN,

                })
            };
        }
        public async Task CancelOrder(int orderId, string userId)
        {
            if (orderId <= 0)
                throw new ValidationException(new[] {
                    new ValidationFailure("OrderId","OrderId Must be Greater than  0")
                });

            var orderSpec = new OrderWithItemSpecification(orderId, userId);
            var order = await _unitOfWork.Order.GetOne(orderSpec);
            if (order == null)
                throw new NotFoundException("Not Found Order ");
            if (order.Status == OrderStatus.Cancelled)
                throw new ValidationException(new[]
                {
                    new ValidationFailure("OrderStatus" , "Order is Already Canceled"),
                });

            if (order.Status == OrderStatus.Shipped || order.Status == OrderStatus.Delivered)
                throw new ValidationException(new[]
                  {
                    new ValidationFailure("OrderStatus" , "Cant be Canceled "),
                });

            if (!string.IsNullOrEmpty(order.CouponCode))
            {
                var coupon = await _unitOfWork.Coupon.GetFirstOneAsync(e => e.Code == order.CouponCode);
                if (coupon != null)
                {
                    var couponUsage = await _unitOfWork.CouponUsage.GetFirstOneAsync(e => e.Code == order.CouponCode && e.userId == userId);
                    if (couponUsage != null)
                    {
                        _unitOfWork.CouponUsage.Delete(couponUsage);
                    }

                    coupon.CountUsed--;
                }
            }
            foreach (var item in order.orderItems!)
            {
                item.Book.Stock += item.Quantity;
            }
            order.Status = OrderStatus.Cancelled;
            await _unitOfWork.SaveChangeAsync();

        }
        public async Task UpdateOrderStatus(UpdateOrderStatusRequest dto)
        {
            var validator = await _updateOrderStatusRequest.ValidateAsync(dto);
            if (!validator.IsValid)
                throw new ValidationException(validator.Errors);

            var order = await _unitOfWork.Order.GetByIdAsync(dto.OrderId);
            if (order == null)
                throw new NotFoundException("Not Found Order ");

            order.Status = dto.Status;
            await _unitOfWork.SaveChangeAsync();
        }

    }
}
