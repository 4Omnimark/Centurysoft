using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcPaging
{
    public class PagingOption : IPagingOption
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int? TotalCount { get; set; }
        public string SortBy { get; set; }
        public bool? SortDescending { get; set; }

        public string OrderByExpression
        {
            get{
                return this.SortDescending.HasValue && this.SortDescending.Value ? this.SortBy + " desc" : this.SortBy + " asc";
            }
        }
    }
}