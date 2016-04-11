using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagingSorting.Web.Models;
using System.Text;
using MvcPaging;

namespace PagingSorting.Web.Controllers
{
    public class xHomeController : BaseController
    {

        [HttpGet]
        public ActionResult Index(int? Page, string SortBy, bool? SortDescending)
        {
            int currentPageIndex = Page.HasValue ? Page.Value - 1 : 0;
            var option = new PagingOption { Page = currentPageIndex, PageSize = PageSize, SortBy = SortBy, SortDescending = SortDescending };

            var items = PeopleList.ToPagedList(option);
            return View(items);
        }
  
        

    }
}
