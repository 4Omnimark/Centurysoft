using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcPaging
{
    public interface IPagingOption
    {
        int Page { get; set; }
        int PageSize { get; set; }
        int? TotalCount { get; set; }

        string SortBy { get; set; }
        bool? SortDescending { get; set; }
        string OrderByExpression { get; }
    }
}