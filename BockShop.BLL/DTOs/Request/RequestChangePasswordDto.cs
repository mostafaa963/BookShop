using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.DTOs.Request
{
    public class RequestChangePasswordDto
    {
        public string  EmailOrUserName  { get; set; }
        public string  Password  { get; set; }
        public string  NewPassword  { get; set; }
        public string  ConfirmPassword  { get; set; }
    }
}
