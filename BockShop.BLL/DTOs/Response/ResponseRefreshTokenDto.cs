using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.DTOs.Response
{
    public class ResponseRefreshTokenDto
    {
        public string? RefreshToken { get; set; }
        public DateTime? ExpiredRefreshToken { get; set; }
        public string AccessToken { get; set; }
        public DateTime ExpiredAccessToken { get; set; }
    }
}
