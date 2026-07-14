using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace BockShop.BLL.DTOs.Request
{
    public class RequestLoginDto
    {
        public  string UserName   { get; set; }

        public string Password { get; set; }
    }
}
