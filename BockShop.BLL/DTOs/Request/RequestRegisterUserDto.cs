using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.DTOs.Request
{
    public class RequestRegisterUserDto
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
