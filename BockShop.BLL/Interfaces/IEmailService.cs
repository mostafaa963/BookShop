using BockShop.BLL.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.Interfaces
{
    public  interface IEmailService
    {
        Task SendEmailAsync(string email, string link, EmailType subjectOfEmail = EmailType.ConfirmEmail);
    }
}
