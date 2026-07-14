using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.Interfaces
{
    public  interface IFileService
    {
        Task<string> UploadFile(IFormFile file, string entityFile);
    }
}
