using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.Common
{
    public  class PaginationMetadata
    {
        public PaginationMetadata(int currentPage, int pageSize, int totalCount, int totalPages)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = totalPages;
        }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;

        public bool HasNextPage => CurrentPage < TotalPages;

    }
}
