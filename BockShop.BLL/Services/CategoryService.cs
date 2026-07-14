using BockShop.BLL.DTOs.Request;
using BockShop.BLL.DTOs.Response;
using BockShop.BLL.Interfaces;
using BockShop.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<CategoryIndexDto> GetCountAllCategoryAsync()
        {
            var category = _unitOfWork.Category.GetAll();
            var groupBy =await category.GroupBy(e => e.Name).Select(e=>new GroupByDto
            {
                Name=e.Key,
                Count = e.Count(),
            }).ToListAsync();
            #region
            //if (categoryQueryParameters.Search is not null)
            //    category = category.Where(e => e.Name.ToLower().Contains(categoryQueryParameters.Search.ToLower().Trim()));
            //category = categoryQueryParameters.SortedBy!.ToLower() switch
            //{
            //    "name" => categoryQueryParameters.DescendingOrder ?
            //      category.OrderByDescending(e => e.Name) : category.OrderBy(e => e.Name),

            //    _ => category.OrderBy(e => e.Id),
            //};

            //category.Skip((categoryQueryParameters.PageNumber - 1) * categoryQueryParameters.PageSize)
            //    .Take(categoryQueryParameters.PageSize);

            //var query = category.Select(e => new CategoryIndexDto
            //{
            //    Name = e.Name,
            //    Description = e.Description,
            //});
            #endregion
            return new CategoryIndexDto
            {
                Groups = groupBy,
                CountCategory = category.Count()
            };
        }
    }
}
