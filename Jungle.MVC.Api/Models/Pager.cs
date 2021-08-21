using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jungle.MVC.Api.Models
{
    public class Pager
    {
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }

        public Pager()
        {

        }

        public Pager(int totalItems, int page, int pagesize = 10)
        {
            int totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pagesize);
            int currentpage = page;
            int startPage = currentpage - 5;
            int endPage = currentpage + 4;

            if (startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }
            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }

            TotalPages = totalPages;
            CurrentPage = currentpage;
            TotalItems = totalItems;
            PageSize = pagesize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
        }
    }
}
