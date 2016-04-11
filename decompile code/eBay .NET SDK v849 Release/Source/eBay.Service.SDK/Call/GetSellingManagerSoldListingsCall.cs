#region Copyright
//	Copyright (c) 2013 eBay, Inc.
//	
//	This program is licensed under the terms of the eBay Common Development and
//	Distribution License (CDDL) Version 1.0 (the "License") and any subsequent  
//	version thereof released by eBay.  The then-current version of the License can be 
//	found at http://www.opensource.org/licenses/cddl1.php and in the eBaySDKLicense 
//	file that is under the eBay SDK ../docs directory
#endregion

#region Namespaces
using System;
using System.Runtime.InteropServices;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using eBay.Service.EPS;
using eBay.Service.Util;

#endregion

namespace eBay.Service.Call
{

	/// <summary>
	/// 
	/// </summary>
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public class GetSellingManagerSoldListingsCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetSellingManagerSoldListingsCall()
		{
			ApiRequest = new GetSellingManagerSoldListingsRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetSellingManagerSoldListingsCall(ApiContext ApiContext)
		{
			ApiRequest = new GetSellingManagerSoldListingsRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Returns a Selling Manager user's sold listings.
		/// 
		/// This call is subject to change without notice; the deprecation process is
		/// inapplicable to this call.
		/// </summary>
		/// 
		/// <param name="Search">
		/// Search filters for sold listings.
		/// </param>
		///
		/// <param name="StoreCategoryID">
		/// Listings with this store category ID will be listed.
		/// </param>
		///
		/// <param name="FilterList">
		/// This holds the list of filters that can be applicable for sold listings.
		/// </param>
		///
		/// <param name="Archived">
		/// Requests listing records that are more than 90 days old. Records are archived between 90
		/// and 120 days after being created, and thereafter can only be retrieved using this tag.
		/// </param>
		///
		/// <param name="Sort">
		/// Field to be used to sort the response.
		/// </param>
		///
		/// <param name="SortOrder">
		/// Order to be used for sorting the requested listings.
		/// </param>
		///
		/// <param name="Pagination">
		/// Details about how many listings to return per page and which page to view.
		/// </param>
		///
		/// <param name="SaleDateRange">
		/// Specifies the earliest (oldest) and latest (most recent) dates to use in a date
		/// range filter based on item start time. A time range can be up to 120
		/// days.
		/// </param>
		///
		public SellingManagerSoldOrderTypeCollection GetSellingManagerSoldListings(SellingManagerSearchType Search, long StoreCategoryID, SellingManagerSoldListingsPropertyTypeCodeTypeCollection FilterList, bool Archived, SellingManagerSoldListingsSortTypeCodeType Sort, SortOrderCodeType SortOrder, PaginationType Pagination, TimeRangeType SaleDateRange)
		{
			this.Search = Search;
			this.StoreCategoryID = StoreCategoryID;
			this.FilterList = FilterList;
			this.Archived = Archived;
			this.Sort = Sort;
			this.SortOrder = SortOrder;
			this.Pagination = Pagination;
			this.SaleDateRange = SaleDateRange;

			Execute();
			return ApiResponse.SaleRecord;
		}



		#endregion




		#region Properties
		/// <summary>
		/// The base interface object.
		/// </summary>
		/// <remarks>This property is reserved for users who have difficulty querying multiple interfaces.</remarks>
		public ApiCall ApiCallBase
		{
			get { return this; }
		}

		/// <summary>
		/// Gets or sets the <see cref="GetSellingManagerSoldListingsRequestType"/> for this API call.
		/// </summary>
		public GetSellingManagerSoldListingsRequestType ApiRequest
		{ 
			get { return (GetSellingManagerSoldListingsRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetSellingManagerSoldListingsResponseType"/> for this API call.
		/// </summary>
		public GetSellingManagerSoldListingsResponseType ApiResponse
		{ 
			get { return (GetSellingManagerSoldListingsResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellingManagerSoldListingsRequestType.Search"/> of type <see cref="SellingManagerSearchType"/>.
		/// </summary>
		public SellingManagerSearchType Search
		{ 
			get { return ApiRequest.Search; }
			set { ApiRequest.Search = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellingManagerSoldListingsRequestType.StoreCategoryID"/> of type <see cref="long"/>.
		/// </summary>
		public long StoreCategoryID
		{ 
			get { return ApiRequest.StoreCategoryID; }
			set { ApiRequest.StoreCategoryID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellingManagerSoldListingsRequestType.Filter"/> of type <see cref="SellingManagerSoldListingsPropertyTypeCodeTypeCollection"/>.
		/// </summary>
		public SellingManagerSoldListingsPropertyTypeCodeTypeCollection FilterList
		{ 
			get { return ApiRequest.Filter; }
			set { ApiRequest.Filter = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellingManagerSoldListingsRequestType.Archived"/> of type <see cref="bool"/>.
		/// </summary>
		public bool Archived
		{ 
			get { return ApiRequest.Archived; }
			set { ApiRequest.Archived = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellingManagerSoldListingsRequestType.Sort"/> of type <see cref="SellingManagerSoldListingsSortTypeCodeType"/>.
		/// </summary>
		public SellingManagerSoldListingsSortTypeCodeType Sort
		{ 
			get { return ApiRequest.Sort; }
			set { ApiRequest.Sort = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellingManagerSoldListingsRequestType.SortOrder"/> of type <see cref="SortOrderCodeType"/>.
		/// </summary>
		public SortOrderCodeType SortOrder
		{ 
			get { return ApiRequest.SortOrder; }
			set { ApiRequest.SortOrder = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellingManagerSoldListingsRequestType.Pagination"/> of type <see cref="PaginationType"/>.
		/// </summary>
		public PaginationType Pagination
		{ 
			get { return ApiRequest.Pagination; }
			set { ApiRequest.Pagination = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellingManagerSoldListingsRequestType.SaleDateRange"/> of type <see cref="TimeRangeType"/>.
		/// </summary>
		public TimeRangeType SaleDateRange
		{ 
			get { return ApiRequest.SaleDateRange; }
			set { ApiRequest.SaleDateRange = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellingManagerSoldListingsResponseType.SaleRecord"/> of type <see cref="SellingManagerSoldOrderTypeCollection"/>.
		/// </summary>
		public SellingManagerSoldOrderTypeCollection SaleRecordList
		{ 
			get { return ApiResponse.SaleRecord; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellingManagerSoldListingsResponseType.PaginationResult"/> of type <see cref="PaginationResultType"/>.
		/// </summary>
		public PaginationResultType PaginationResult
		{ 
			get { return ApiResponse.PaginationResult; }
		}
		

		#endregion

		
	}
}
