using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Domain.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public IEnumerable<CartItem> cartItems { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.UtcNow;
        public double TotalPrice { get; set; }
    }
}
