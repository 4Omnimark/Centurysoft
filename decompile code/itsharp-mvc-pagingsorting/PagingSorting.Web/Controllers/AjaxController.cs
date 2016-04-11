using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPaging;

namespace PagingSorting.Web.Controllers
{
    public class AjaxController : BaseController
    {
    
        [HttpGet]
        public ActionResult Index()
        {
            var option = new PagingOption { Page = 0, PageSize = PageSize };
            var items = PeopleList.ToPagedList(option);
            return View(items);
        }

        [HttpGet]
        public ActionResult AjaxPeople(int? Page, string SortBy, bool? SortDescending)
        {
            int currentPageIndex = Page.HasValue ? Page.Value - 1 : 0;
            var option = new PagingOption { Page = currentPageIndex, PageSize = PageSize, SortBy = SortBy, SortDescending = SortDescending };
            var items = PeopleList.ToPagedList(option);

            System.Threading.Thread.Sleep(1000);

            return PartialView("PeopleGridPartial", items);
        }
    }
}
