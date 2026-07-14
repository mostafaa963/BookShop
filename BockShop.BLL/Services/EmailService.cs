using BockShop.BLL.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.Services
{
    public enum EmailType
    {
        ResetPassword = 1,
        ConfirmEmail = 2,
    }
    public class EmailService : IEmailService
    {
        private readonly IEmailSender _emailSender;

        public EmailService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }
        public async Task SendEmailAsync(string email, string link, EmailType subjectOfEmail = EmailType.ConfirmEmail)
        {
            string htmlBody = $"<h1><a href={link} >By Click Here</a><h2>";
            string subject = subjectOfEmail switch
            {
                EmailType.ResetPassword => "Reset Password For Email ",
                _ => "Confirm Your Email"
            };
            await _emailSender.SendEmailAsync(email, subject, htmlBody);
        }
    }
}
