using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BockShop.BLL.DTOs.Response
{
    public class AddCouponResponse
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}
