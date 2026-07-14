using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Domain.Entities
{
    public  class Publisher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime PublicationDate { get; set; }
        public IEnumerable<Book>? books { get; set; }
    }
}
