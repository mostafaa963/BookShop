using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.DTOs.Response
{
    public class EmailConfirmationTokenResponse
    {
        public string userId { get; set; }
        public string EmailConfirmationToken { get; set; }
    }
}
