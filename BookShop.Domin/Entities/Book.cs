using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookShop.Domain.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string ASIN { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; }
        public string Descripation { get; set; }
        public string ImageUrl { get; set; }
        public double Price { get; set; }
        public double Rate { get; set; }
        [ForeignKey(nameof(Author))]
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        [ForeignKey(nameof(Publisher))]
        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public IEnumerable<Favorite> favorites { get; set; }
        public IEnumerable<CartItem> cartItems { get; set; }
    }
}
