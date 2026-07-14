using BockShop.BLL.DTOs.Request;
using BockShop.BLL.DTOs.Response;
using BockShop.BLL.Interfaces;
using BockShop.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<ResponseBookDto>> GetAllBookAsync(BookQueryParameters bookQueryParameters)
        {
            var books = _unitOfWork.Book.GetAll();

            books = books.Include(e => e.Author).Include(e => e.Publisher);

            if (!string.IsNullOrEmpty(bookQueryParameters.Search))
                books = books.Where(e => e.Title.ToLower().Contains(bookQueryParameters.Search.ToLower().Trim()));
            books = bookQueryParameters.SortBy?.ToLower() switch
            {
                "title" => bookQueryParameters.DescendingOrder ?
                books.OrderByDescending(e => e.Title) : books.OrderBy(e => e.Title),
                _ => books.OrderBy(e => e.Id)
            };
            books.Skip((bookQueryParameters.PageNumber - 1) * bookQueryParameters.PageSize)
                .Take(bookQueryParameters.PageSize);

            var response = await books.Select(e => new ResponseBookDto
            {
                Name = e.Title,
                Rate = e.Rate,
                Price = e.Price,
                Author = e.Author.Name,
                PublicationPublish = e.Publisher.PublicationDate,
                Description = e.Descripation,
                UrlImage = e.ImageUrl
            }).ToListAsync();
            return response;
        }
        public async Task<List<BookDto>> SearchByNameAsync(string name)
        {
            var books = _unitOfWork.Book.GetAll();
            if (string.IsNullOrEmpty(name))
                throw new Exception("Name is Required");
            books = books.Where(e => e.Title.ToLower().Contains(name.ToLower().Trim()));
            books = books.Include(e => e.Author).Include(e => e.Publisher);
            var response = books.Select(e => new BookDto
            {
                Title = e.Title,
                Description = e.Descripation,
                Rate = e.Rate,
                Price = e.Price,
                UrlImage = e.ImageUrl,
                AuthorName = e.Author.Name,
                PublicationData = e.Publisher.PublicationDate,
            });

            return await response.ToListAsync();
        }
    }
}
