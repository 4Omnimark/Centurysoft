using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;
using System.Web.Mvc.Html;

namespace MvcPaging
{
    public static class PagingExtensions
    {
        #region AjaxHelper extensions

        public static HtmlString Pager(this AjaxHelper ajaxHelper, int pageSize, int currentPage, int totalItemCount, AjaxOptions ajaxOptions)
        {
            return Pager(ajaxHelper, pageSize, currentPage, totalItemCount, null, null, ajaxOptions);
        }

        public static HtmlString Pager(this AjaxHelper ajaxHelper, int pageSize, int currentPage, int totalItemCount,string sortBy, bool? sortDescending,string actionName, AjaxOptions ajaxOptions)
        {
            return Pager(ajaxHelper, pageSize, currentPage, totalItemCount, sortBy, sortDescending,actionName, null, ajaxOptions);
        }

        public static HtmlString Pager(this AjaxHelper ajaxHelper, int pageSize, int currentPage, int totalItemCount, string actionName, AjaxOptions ajaxOptions)
        {
            return Pager(ajaxHelper, pageSize, currentPage, totalItemCount, actionName, null, ajaxOptions);
        }

        public static HtmlString Pager(this AjaxHelper ajaxHelper, int pageSize, int currentPage, int totalItemCount, object values, AjaxOptions ajaxOptions)
        {
            return Pager(ajaxHelper, pageSize, currentPage, totalItemCount, null, new RouteValueDictionary(values), ajaxOptions);
        }

        public static HtmlString Pager(this AjaxHelper ajaxHelper, int pageSize, int currentPage, int totalItemCount, string actionName, object values, AjaxOptions ajaxOptions)
        {
            return Pager(ajaxHelper, pageSize, currentPage, totalItemCount,null,null, actionName, new RouteValueDictionary(values), ajaxOptions);
        }

        public static HtmlString Pager(this AjaxHelper ajaxHelper, int pageSize, int currentPage, int totalItemCount, RouteValueDictionary valuesDictionary, AjaxOptions ajaxOptions)
        {
            return Pager(ajaxHelper, pageSize, currentPage, totalItemCount, null, valuesDictionary, ajaxOptions);
        }

        public static HtmlString Pager(this AjaxHelper ajaxHelper, int pageSize, int currentPage, int totalItemCount, string sortBy, bool? sortDescending, string actionName, RouteValueDictionary valuesDictionary, AjaxOptions ajaxOptions)
        {
            if (valuesDictionary == null)
            {
                valuesDictionary = new RouteValueDictionary();
            }
            if (actionName != null)
            {
                if (valuesDictionary.ContainsKey("action"))
                {
                    throw new ArgumentException("The valuesDictionary already contains an action.", "actionName");
                }
                valuesDictionary.Add("action", actionName);
            }
            var pager = new Pager(ajaxHelper.ViewContext, pageSize, currentPage, totalItemCount,sortBy, sortDescending, valuesDictionary, ajaxOptions);
            return pager.RenderHtml();
        }

        #endregion      

        #region HtmlHelper extensions

        public static HtmlString Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount)
        {
            return Pager(htmlHelper, pageSize, currentPage, totalItemCount, null, null);
        }

        public static HtmlString Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, string sortBy, bool? sortDescending)
        {
            return Pager(htmlHelper, pageSize, currentPage, totalItemCount, sortBy, sortDescending, null, null);
        }

        public static HtmlString Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, string actionName)
        {
            return Pager(htmlHelper, pageSize, currentPage, totalItemCount, actionName, null);
        }

        public static HtmlString Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, object values)
        {
            return Pager(htmlHelper, pageSize, currentPage, totalItemCount, null, new RouteValueDictionary(values));
        }

        public static HtmlString Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, string actionName, object values)
        {
            return Pager(htmlHelper, pageSize, currentPage, totalItemCount, null,null,actionName, new RouteValueDictionary(values));
        }

        public static HtmlString Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, RouteValueDictionary valuesDictionary)
        {
            return Pager(htmlHelper, pageSize, currentPage, totalItemCount, null, valuesDictionary);
        }

        public static HtmlString Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, string sortBy, bool? sortDescending, string actionName)
        {
            return Pager(htmlHelper, pageSize, currentPage, totalItemCount, sortBy, sortDescending, actionName, null);
        }

        public static HtmlString Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, string sortBy, bool? sortDescending, string actionName, RouteValueDictionary valuesDictionary)
        {
            if (valuesDictionary == null)
            {
                valuesDictionary = new RouteValueDictionary();
            }
            if (actionName != null)
            {
                if (valuesDictionary.ContainsKey("action"))
                {
                    throw new ArgumentException("The valuesDictionary already contains an action.", "actionName");
                }
                valuesDictionary.Add("action", actionName);
            }
            var pager = new Pager(htmlHelper.ViewContext, pageSize, currentPage, totalItemCount,sortBy, sortDescending, valuesDictionary, null);
            return pager.RenderHtml();
        }

        #endregion

        #region ActionLink extensions

        public static HtmlString ActionLink<T>(this AjaxHelper ajaxHelper, string name, string actionName, IPagedList<T> model, AjaxOptions ajaxOptions)
        {
            var sortDescending = model.SortBy != null && model.SortBy.Equals(name) && model.SortDescending.HasValue && model.SortDescending.Value ? false : true;
            var routeValues = new { SortBy = name, SortDescending = sortDescending };

            var css = "";
            if (!string.IsNullOrEmpty(model.SortBy) && model.SortBy.Equals(name))
            {
                if (model.SortDescending.HasValue && model.SortDescending.Value)
                    css = "sort-desc";
                else
                    css = "sort-asc";
            }
            else
                css = "sort-off";
            var htmlAttributes = new { @class = css };
            return ajaxHelper.ActionLink(name, actionName, routeValues, ajaxOptions, htmlAttributes);
        }

        public static HtmlString ActionLink<T>(this HtmlHelper htmlHelper, string name, string actionName, IPagedList<T> model)
        {
            return htmlHelper.ActionLink<T>(name, actionName, model, name, false);
        }

        public static HtmlString ActionLink<T>(this HtmlHelper htmlHelper, string name, string actionName, IPagedList<T> model, bool IncludeCurrentQueryString)
        {
            return htmlHelper.ActionLink<T>(name, actionName, model, name, IncludeCurrentQueryString);
        }

        public static HtmlString ActionLink<T>(this HtmlHelper htmlHelper, string name, string actionName, IPagedList<T> model, string SortFieldName)
        {
            return htmlHelper.ActionLink<T>(name, actionName, model, SortFieldName, false);
        }

        public static HtmlString ActionLink<T>(this HtmlHelper htmlHelper, string name, string actionName, IPagedList<T> model, string SortFieldName, bool IncludeCurrentQueryString)
        {
            bool sortDescending = false;

            if (model.SortBy != null && model.SortBy.Equals(SortFieldName))
                sortDescending = !model.SortDescending.Value;

            RouteValueDictionary routeValues = new RouteValueDictionary(new { SortBy = SortFieldName, SortDescending = sortDescending });

            if (IncludeCurrentQueryString)
            {
                foreach (string key in HttpContext.Current.Request.QueryString.Keys)
                {
                    if (key != "SortBy" && key != "SortDescending")
                        routeValues.Add(key, HttpContext.Current.Request.QueryString[key]);
                }
            }

            var css = "";
            if (!string.IsNullOrEmpty(model.SortBy) && model.SortBy.Equals(SortFieldName))
            {
                if (model.SortDescending.HasValue && model.SortDescending.Value)
                    css = "sort-desc";
                else
                    css = "sort-asc";
            }
            else
                css = "sort-off";
            
            Dictionary<String, Object> htmlAttributes = new Dictionary<string, object>();
            htmlAttributes["class"] = css;

            return htmlHelper.ActionLink(name, actionName, routeValues, htmlAttributes);
        }

        #endregion

        #region IQueryable<T> extensions

        public static IPagedList<T> ToPagedList<T>(this IQueryable<T> source, IPagingOption option)
        {
            return new PagedList<T>(source, option);
        }

        #endregion

        #region IEnumerable<T> extensions

        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> source, IPagingOption option)
        {
            return new PagedList<T>(source, option);
        }

        #endregion
    }
}