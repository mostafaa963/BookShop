using Azure.Core;
using BockShop.BLL.DTOs.Request;
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

        public OrderService(IUnitOfWork unitOfWork, IValidator<CreateOrderRequest> createOrderRequestValidator)
        {
            _createOrderRequestValidator = createOrderRequestValidator;
            _unitOfWork = unitOfWork;
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
    }
}
