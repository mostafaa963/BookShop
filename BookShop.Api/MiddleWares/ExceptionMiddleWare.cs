using BockShop.BLL.Common;
using FluentValidation;

namespace BookShop.Api.MiddleWares
{
    public class ExceptionMiddleWare
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleWare(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException failed)
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "Application/json";
                var response = new GenResponse<List<string>>
                {
                    Success = false,
                    StatusCode = 400,
                    Data = failed.Errors.Select(e => e.ErrorMessage).ToList()
                };
                await context.Response.WriteAsJsonAsync(response);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "Application/json";
                var response = new GenResponse<string>
                {
                    Success = false,
                    StatusCode = 500,
                    Data = ex.Message,
                };
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
