using BockShop.BLL.DTOs.Request;
using BockShop.BLL.DTOs.Response;
using BockShop.BLL.Exceptions;
using BockShop.BLL.Interfaces;
using BockShop.DAL.Interfaces;
using BookShop.Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Text;

namespace BockShop.BLL.Services
{
    public class FavoriteItemService : IFavoriteItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<RequestFavoriteItemDto> _requestFavoriteItemValidator;

        public FavoriteItemService(IValidator<RequestFavoriteItemDto> requestFavoriteItemValidator, IUnitOfWork unitOfWork)
        {
            _requestFavoriteItemValidator = requestFavoriteItemValidator;
            _unitOfWork = unitOfWork;
        }
        public async Task<FavoriteItemResponse> AddAsync(RequestFavoriteItemDto requestFavoriteItemDto, string UserId)
        {
            var validator = await _requestFavoriteItemValidator.ValidateAsync(requestFavoriteItemDto);
            if (!validator.IsValid)
                throw new ValidationException(validator.Errors);
            var book = await _unitOfWork.Book.GetByIdAsync(requestFavoriteItemDto.BookId);
            if (book == null)
                throw new NotFoundException("Book Is not Found");
            var favoriteItem = await _unitOfWork.Favorite.GetFirstOneAsync(e => e.UserId == UserId &&
            e.BookId == book.Id);
            if (favoriteItem != null)
                throw new Exception("Book is already in favorites.");
            var favoriteIteItem = new Favorite
            {
                UserId = UserId,
                BookId = book.Id
            };
            await _unitOfWork.Favorite.AddAsync(favoriteIteItem);
            await _unitOfWork.SaveChangeAsync();
            return new FavoriteItemResponse
            {
                Id = favoriteIteItem.Id,
                BookTitle = book.Title,
            };

        }
        public async Task<AllFavoriteItemResponse> GetAllFavoriteItemAsync(FavoriteQueryParameters favoriteQueryParameters, string userId)
        {
            var favoriteItem = _unitOfWork.Favorite.GetAll();
            favoriteItem = favoriteItem.Where(e => e.UserId == userId);
            favoriteItem = favoriteItem.Include(e => e.Book);
            if (favoriteQueryParameters.Search is not null)
                favoriteItem = favoriteItem.Where(e => e.Book.Title.ToLower().Contains(favoriteQueryParameters.Search.ToLower().Trim()));

            favoriteItem = favoriteQueryParameters.SortedBy?.ToLower() switch
            {
                "title" => favoriteQueryParameters.Descending ?
                favoriteItem.OrderByDescending(e => e.Book.Title) : favoriteItem.OrderBy(e => e.Book.Title),
                _ => favoriteItem.OrderBy(e => e.Id)

            };
            favoriteItem = favoriteItem.Skip((favoriteQueryParameters.PageNumber - 1) * favoriteQueryParameters.PageSize)
               .Take(favoriteQueryParameters.PageSize);
            var response = await favoriteItem.Select(e => new FavoriteItemResponse
            {
                BookTitle = e.Book.Title,
                Id = e.Id,
            }).ToListAsync();
            return new AllFavoriteItemResponse
            {
                Data = response,
            };
        }
        public async Task DeleteAsync(int bookId,string userId)
        {
            var favoriteItem =await  _unitOfWork.Favorite.GetFirstOneAsync(e=>e.UserId==userId&& e.BookId ==bookId);
          
            if (favoriteItem == null)
                throw new NotFoundException("Book is not in your favorites");

            _unitOfWork.Favorite.Delete(favoriteItem);
            await _unitOfWork.SaveChangeAsync();
        }
    }
}
