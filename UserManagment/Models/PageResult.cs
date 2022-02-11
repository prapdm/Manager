using System;
using System.Collections.Generic;

namespace Manager.Models
{
    public class PageResult<T>  
    {
        public List<T> Items { get; set; }
        public int Totalpages { get; set; }
        public int ItemFrom { get; set; }
        public int ItemsTo { get; set; }
        public int TotalItemsCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public PageResult(List<T> items, int totalCount, int pageSize, int pageNumber)
        {
            Items = items;
            TotalItemsCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
            ItemFrom = pageSize * (pageNumber - 1) + 1;
            ItemsTo = ItemFrom + pageSize - 1;
            Totalpages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}
