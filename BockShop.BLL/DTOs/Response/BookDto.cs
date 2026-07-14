using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.DTOs.Response
{
    public  class BookDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public  double Rate { get; set; }
        public double Price { get; set; }
        public string? UrlImage { get; set; }
        public DateTime?  PublicationData    { get; set; }
        public string? AuthorName { get; set; }

    }
}
