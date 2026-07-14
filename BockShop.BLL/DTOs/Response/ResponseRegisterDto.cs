using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BockShop.BLL.DTOs.Response
{
    public  class ResponseRegisterDto
    {
        [JsonIgnore]
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
