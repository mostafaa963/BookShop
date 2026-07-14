using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace BookShop.Domain.Entities
{
    public  class Favorite
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
