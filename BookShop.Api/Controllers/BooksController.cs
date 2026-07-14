using BockShop.BLL.Common;
using BockShop.BLL.DTOs.Request;
using BockShop.BLL.DTOs.Response;
using BockShop.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }
        [Authorize]
        [HttpGet("GetAllBook")]
        public async Task<ActionResult<GenResponse<List<ResponseBookDto>>>> GetAllBook([FromQuery] BookQueryParameters bookQueryParameters)
        {
            var response = await _bookService.GetAllBookAsync(bookQueryParameters);
            return Ok(new GenResponse<List<ResponseBookDto>>
            {
                Success = true,
                StatusCode = 200,
                Message = "Get All Books Successfully..!",
                Data = response
            });

        }
        [HttpGet("GetBookByName")]
        public async Task<ActionResult<GenResponse<List<BookDto>>>> GetBookByName(string bookName)
        {
            var book = await _bookService.SearchByNameAsync(bookName);
            return Ok(new GenResponse<List<BookDto>>
            {
                Message = "Get Book Successfully",
                Success = true,
                StatusCode = 200,
                Data = book
            });
        }
       
    }
}
