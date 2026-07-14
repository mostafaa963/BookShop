using BockShop.BLL.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace BockShop.BLL.Services
{
    public class LocalFileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LocalFileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> UploadFile(IFormFile file, string entityFile)
        {
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Image", entityFile);
            if(!File.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            var fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(file.FileName)}";
            var filePath= Path.Combine(folderPath, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
           await file.CopyToAsync(stream);
            return fileName;
        }
    }
}
