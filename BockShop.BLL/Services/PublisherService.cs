using BockShop.BLL.Interfaces;
using BockShop.DAL.Interfaces;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.Services
{
    public  class PublisherService:IPublisherService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PublisherService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
