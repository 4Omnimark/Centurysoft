using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;


namespace MvcPaging
{
	public class PagedList<T> : List<T>, IPagedList<T>
	{
		public PagedList(IEnumerable<T> source, IPagingOption option)
            : this(source.AsQueryable(), option)
		{
		}

        public PagedList(IQueryable<T> source, IPagingOption option)
		{
            if(option==null)
                throw new ArgumentOutOfRangeException("PagingOption", "Value can not be null.");
			if (option.Page < 0)
				throw new ArgumentOutOfRangeException("index", "Value must be greater than 0.");
			if (option.PageSize < 1)
				throw new ArgumentOutOfRangeException("pageSize", "Value must be greater than 1.");

			if (source == null)
				source = new List<T>().AsQueryable();

			var realTotalCount = source.Count();

			PageSize = option.PageSize;
			PageIndex = option.Page;
			TotalItemCount = option.TotalCount.HasValue ? option.TotalCount.Value : realTotalCount;
			PageCount = TotalItemCount > 0 ? (int)Math.Ceiling(TotalItemCount / (double)PageSize) : 0;

            SortBy = option.SortBy;
            SortDescending = option.SortDescending;
            if(!string.IsNullOrEmpty(option.SortBy))
                source = source.OrderBy(option.OrderByExpression);
            
			HasPreviousPage = (PageIndex > 0);
			HasNextPage = (PageIndex < (PageCount - 1));
			IsFirstPage = (PageIndex <= 0);
			IsLastPage = (PageIndex >= (PageCount - 1));

			if (TotalItemCount <= 0)
				return;

			var realTotalPages = (int)Math.Ceiling(realTotalCount / (double)PageSize);

			if (realTotalCount < TotalItemCount && realTotalPages <= PageIndex)
				AddRange(source.Skip((realTotalPages - 1) * PageSize).Take(PageSize));
			else
				AddRange(source.Skip(PageIndex * PageSize).Take(PageSize));
		}

		#region IPagedList Members

		public int PageCount { get; private set; }
		public int TotalItemCount { get; private set; }
		public int PageIndex { get; private set; }
		public int PageNumber { get { return PageIndex + 1; } }
		public int PageSize { get; private set; }
		public bool HasPreviousPage { get; private set; }
		public bool HasNextPage { get; private set; }
		public bool IsFirstPage { get; private set; }
		public bool IsLastPage { get; private set; }
        public string SortBy{ get; private set; }
        public bool? SortDescending { get; private set; }
		#endregion
    }
}