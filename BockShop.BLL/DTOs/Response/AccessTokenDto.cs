using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.DTOs.Response
{
    public class AccessTokenDto
    {
        public string AccessToken { get; set; }
        public DateTime ExpiredAccessTokenOn { get; set; }
    }
}
