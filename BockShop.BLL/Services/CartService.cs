using BockShop.BLL.DTOs.Request;
using BockShop.BLL.DTOs.Response;
using BockShop.BLL.Exceptions;
using BockShop.BLL.Specifications;
using BockShop.DAL.Interfaces;
using BookShop.Domain.Entities;
using FluentValidation;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

namespace BockShop.BLL.Services
{
    public class CartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddCartItemRequest> _addCartItemRequestValidator;

        public CartService(IValidator<AddCartItemRequest> addCartItemRequestValidator, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _addCartItemRequestValidator = addCartItemRequestValidator;
        }

        public async Task<CartResponse> GetCartItems(string userId)
        {
            var cart = await _unitOfWork.Cart.GetFirstOneAsync(e => e.UserId == userId);
            if (cart == null)
                throw new NotFoundException("User Dose not has a Cart ");
            //return IQueryable 
            //var cartItem =  _unitOfWork.CartItem.GetAll(tracking: false, criteria: e => e.CartId == cart.Id,
            //    includes: [e => e.Book, x => x.Book.Author]);
            // used specification Pattern 
            var cartItemSpec = new CartItemSpecification(cart.Id);
            var cartItem = await _unitOfWork.CartItem.GetAll(cartItemSpec);
            var itemsCart = cartItem.Select(x => new CartItemResponse
            {
                BookTitle = x.Book.Title,
                ASIN = x.Book.ASIN,
                Description = x.Book.Descripation,
                Quantity = x.Quantity,
                UnitPrice = x.Book.Price,
                AuthorName = x.Book.Author.Name,
            });
            return new CartResponse
            {
                cartItems = itemsCart,
                CreatedAt = cart.CreatedAt,
                TotalItems = cartItem.Count(),
                TotalPrice = itemsCart.Sum(e => e.TotalPrice)

            };
        }
        public async Task AddToCart(AddCartItemRequest dto, string userId)
        {
            var validator = await _addCartItemRequestValidator.ValidateAsync(dto);
            if (!validator.IsValid)
                throw new ValidationException(validator.Errors);
            var book = await _unitOfWork.Book.GetFirstOneAsync(e => e.Id == dto.BookId);
            if (book == null)
                throw new NotFoundException("Book is Not Found");

            var cartSpec = new AddCartSpecification(userId);
            var cart = await _unitOfWork.Cart.GetOne(cartSpec);
            if (cart == null)
            {
                var newCart = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };
                await _unitOfWork.Cart.AddAsync(newCart);
                await _unitOfWork.SaveChangeAsync();
                await CreateCartItem(newCart.Id, book, dto);
            }
            else
            {
                var cartItem = cart.cartItems.FirstOrDefault(e => e.BookId == dto.BookId);
                if (cartItem == null)
                {
                    await CreateCartItem(cart.Id, book, dto);
                }
                else
                {

                    cartItem.Quantity += dto.Quantity;
                    cartItem.TotalPrice = cartItem.Quantity * dto.Quantity;
                    _unitOfWork.CartItem.Update(cartItem);
                    await _unitOfWork.SaveChangeAsync();
                }
            }
        }
        public async Task InCerement(int itemId, string userId)
        {
            if (itemId <= 0)
                throw new ValidationException("Invalid item id.");
            var item = await GetCartItemAsync(itemId, userId);
            var book = await _unitOfWork.Book.GetByIdAsync(item.BookId);
            if (book is null)
                throw new NotFoundException("Book is Not Found");
            item!.Quantity++;
            item.TotalPrice = item.Quantity * book.Price;
            _unitOfWork.CartItem.Update(item);
            await _unitOfWork.SaveChangeAsync();

        }
        public async Task DeCerement(int itemId, string userId)
        {
            if (itemId <= 0)
                throw new ValidationException("Invalid item id.");
            var item = await GetCartItemAsync(itemId, userId);
            var book = await _unitOfWork.Book.GetByIdAsync(item.BookId);
            if (book is null)
                throw new NotFoundException("Book is Not Found");
            if (item.Quantity == 1)
            {
                _unitOfWork.CartItem.Delete(item);
                await _unitOfWork.SaveChangeAsync();
            }
            else
            {
                item!.Quantity--;
                item.TotalPrice = item.Quantity * book.Price;
                _unitOfWork.CartItem.Update(item);
                await _unitOfWork.SaveChangeAsync();
            }
        }
        public async Task RemoveCart(string userId)
        {
            var cart = await _unitOfWork.Cart.GetFirstOneAsync(e => e.UserId == userId);
            if (cart == null)
                throw new NotFoundException("Cart not found.");
            _unitOfWork.Cart.Delete(cart);
            await _unitOfWork.SaveChangeAsync();
        }
        public async Task RemoveCartItem(int itemId, string userId)
        {
            if (itemId <= 0)
                throw new ValidationException("Invalid item id.");
            var item = await GetCartItemAsync(itemId, userId);
            _unitOfWork.CartItem.Delete(item);
            await _unitOfWork.SaveChangeAsync();
        }
        private async Task<CartItem> GetCartItemAsync(int itemId, string userId)
        {
            var spec = new AddCartSpecification(userId);

            var cart = await _unitOfWork.Cart.GetOne(spec);

            if (cart == null)
                throw new NotFoundException("Cart not found.");

            var item = cart.cartItems.FirstOrDefault(x => x.Id == itemId);

            if (item == null)
                throw new NotFoundException("Cart item not found.");

            return item;
        }

        private async Task<Cart> CreateCartAsync(string userId)
        {
            var cart = new Cart
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
            };
            await _unitOfWork.Cart.AddAsync(cart);
            await _unitOfWork.SaveChangeAsync();
            return cart;
        }
        private async Task CreateCartItem(int cartId, Book book, AddCartItemRequest dto)
        {
            await _unitOfWork.CartItem.AddAsync(new CartItem
            {
                CartId = cartId,
                BookId = dto.BookId,
                Quantity = dto.Quantity,
                TotalPrice = dto.Quantity * book.Price,
            });
            await _unitOfWork.SaveChangeAsync();

        }
    }
}
