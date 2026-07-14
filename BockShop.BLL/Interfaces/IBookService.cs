using BockShop.BLL.DTOs.Request;
using BockShop.BLL.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.Interfaces
{
    public interface IBookService
    {
        Task<List<ResponseBookDto>> GetAllBookAsync(BookQueryParameters bookQueryParameters);
        Task<List<BookDto>> SearchByNameAsync(string name);
    }
}
