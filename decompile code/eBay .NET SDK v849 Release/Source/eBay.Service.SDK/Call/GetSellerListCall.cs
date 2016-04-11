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
	public class GetSellerListCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetSellerListCall()
		{
			ApiRequest = new GetSellerListRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetSellerListCall(ApiContext ApiContext)
		{
			ApiRequest = new GetSellerListRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Returns a list of the items posted by the authenticated user, including
		/// the related item data.
		/// </summary>
		/// 
		/// <param name="UserID">
		/// Specifies the seller whose items will be returned. UserID is an optional
		/// input. If not specified, retrieves listings for the user identified by the
		/// authentication token passed in the request. Note that since user
		/// information is anonymous to everyone except the bidder and the seller
		/// (during an active auction), only sellers looking for information about
		/// their own listings and bidders who know the user IDs of their sellers will
		/// be able to make this API call successfully.
		/// </param>
		///
		/// <param name="MotorsDealerUserList">
		/// Specifies the list of Motors Dealer sellers for which a special set of
		/// metrics can be requested. Applies to eBay Motors Pro applications only.
		/// </param>
		///
		/// <param name="EndTimeFrom">
		/// Specifies the earliest (oldest) date to use in a date range filter based on
		/// item end time. Specify either an end-time range or a start-time range
		/// filter in every call request. Each of the time ranges must be a value less than
		/// 120 days.
		/// </param>
		///
		/// <param name="EndTimeTo">
		/// Specifies the latest (most recent) date to use in a date range filter based
		/// on item end time. Must be specified if EndTimeFrom is specified.
		/// </param>
		///
		/// <param name="Sort">
		/// Specifies the order in which returned items are sorted (based on the end
		/// dates of the item listings). Valid values:
		/// 
		/// 0 = No sorting
		/// 1 = Sort in descending order
		/// 2 = Sort in ascending order
		/// </param>
		///
		/// <param name="StartTimeFrom">
		/// Specifies the earliest (oldest) date to use in a date range filter based on
		/// item start time. Each of the time ranges must be a value less than
		/// 120 days. In all calls, at least one date-range filter must be specified
		/// (i.e., you must specify either the end time range or start time range
		/// in every request).
		/// </param>
		///
		/// <param name="StartTimeTo">
		/// Specifies the latest (most recent) date to use in a date range filter based
		/// on item start time. Must be specified if StartTimeFrom is specified.
		/// </param>
		///
		/// <param name="Pagination">
		/// Contains the data controlling the pagination of the returned values.
		/// If you set a DetailLevel in this call, you must set pagination values.
		/// The Pagination field contains
		/// the number of items to be returned per page of data (per call),
		/// and the page number to return with the current call.
		/// </param>
		///
		/// <param name="GranularityLevel">
		/// Specifies the subset of item and user fields to return. See GetSellerList
		/// in the eBay Features Guide for a list of the fields that are returned
		/// for each granularity level. For GetSellerList, use DetailLevel or
		/// GranularityLevel in a request, but not both. For GetSellerList, if
		/// GranularityLevel is specified, DetailLevel is ignored.
		/// </param>
		///
		/// <param name="SKUArrayList">
		/// Container for a set of SKUs.
		/// Filters (reduces) the response to only include active listings
		/// that the seller listed with any of the specified SKUs.
		/// If multiple listings include the same SKU, they are
		/// all returned (assuming they also match the other criteria
		/// in the GetSellerList request).
		/// 
		/// SKUArray can be used to retrieve items listed by the user
		/// identified in AuthToken or in UserID.
		/// 
		/// <span class="tablenote"><b>Note:</b>
		/// Listings with matching SKUs are returned regardless of their
		/// Item.InventoryTrackingMethod settings.
		/// </span>
		/// </param>
		///
		/// <param name="IncludeWatchCount">
		/// Specifies whether to include WatchCount in Item nodes returned.
		/// WatchCount is only returned with DetailLevel ReturnAll.
		/// </param>
		///
		/// <param name="AdminEndedItemsOnly">
		/// Specifies whether to return only items that were administratively ended
		/// based on a policy violation.
		/// </param>
		///
		/// <param name="CategoryID">
		/// The category ID for the items retrieved.
		/// If you specify CategoryID in a GetSellerList call,
		/// the response contains only items in the category you specify.
		/// </param>
		///
		/// <param name="IncludeVariations">
		/// If true, the Variations node is returned for all multi-variation
		/// listings in the response.
		/// 
		/// Please note that if the seller includes a large number of
		/// variations in many listings, using this flag may degrade the
		/// call's performance. Therefore, when you use this flag, you
		/// may need to reduce the total number of items you're requesting
		/// at once.
		/// For example, you may need to use shorter time ranges in the
		/// EndTime or StartTime filters, fewer entries per page in
		/// Pagination, and/or SKUArray.
		/// </param>
		///
		public ItemTypeCollection GetSellerList(string UserID, UserIDArrayType MotorsDealerUserList, DateTime EndTimeFrom, DateTime EndTimeTo, int Sort, DateTime StartTimeFrom, DateTime StartTimeTo, PaginationType Pagination, GranularityLevelCodeType GranularityLevel, StringCollection SKUArrayList, bool IncludeWatchCount, bool AdminEndedItemsOnly, int CategoryID, bool IncludeVariations)
		{
			this.UserID = UserID;
			this.MotorsDealerUserList = MotorsDealerUserList;
			this.EndTimeFrom = EndTimeFrom;
			this.EndTimeTo = EndTimeTo;
			this.Sort = Sort;
			this.StartTimeFrom = StartTimeFrom;
			this.StartTimeTo = StartTimeTo;
			this.Pagination = Pagination;
			this.GranularityLevel = GranularityLevel;
			this.SKUArrayList = SKUArrayList;
			this.IncludeWatchCount = IncludeWatchCount;
			this.AdminEndedItemsOnly = AdminEndedItemsOnly;
			this.CategoryID = CategoryID;
			this.IncludeVariations = IncludeVariations;

			Execute();
			return ApiResponse.ItemArray;
		}


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public ItemTypeCollection GetSellerList()
		{
			Execute();
			return ItemList;
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
		/// Gets or sets the <see cref="GetSellerListRequestType"/> for this API call.
		/// </summary>
		public GetSellerListRequestType ApiRequest
		{ 
			get { return (GetSellerListRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetSellerListResponseType"/> for this API call.
		/// </summary>
		public GetSellerListResponseType ApiResponse
		{ 
			get { return (GetSellerListResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellerListRequestType.UserID"/> of type <see cref="string"/>.
		/// </summary>
		public string UserID
		{ 
			get { return ApiRequest.UserID; }
			set { ApiRequest.UserID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellerListRequestType.MotorsDealerUsers"/> of type <see cref="UserIDArrayType"/>.
		/// </summary>
		public UserIDArrayType MotorsDealerUserList
		{ 
			get { return ApiRequest.MotorsDealerUsers; }
			set { ApiRequest.MotorsDealerUsers = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellerListRequestType.EndTimeFrom"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime EndTimeFrom
		{ 
			get { return ApiRequest.EndTimeFrom; }
			set { ApiRequest.EndTimeFrom = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellerListRequestType.EndTimeTo"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime EndTimeTo
		{ 
			get { return ApiRequest.EndTimeTo; }
			set { ApiRequest.EndTimeTo = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellerListRequestType.Sort"/> of type <see cref="int"/>.
		/// </summary>
		public int Sort
		{ 
			get { return ApiRequest.Sort; }
			set { ApiRequest.Sort = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellerListRequestType.StartTimeFrom"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime StartTimeFrom
		{ 
			get { return ApiRequest.StartTimeFrom; }
			set { ApiRequest.StartTimeFrom = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellerListRequestType.StartTimeTo"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime StartTimeTo
		{ 
			get { return ApiRequest.StartTimeTo; }
			set { ApiRequest.StartTimeTo = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellerListRequestType.Pagination"/> of type <see cref="PaginationType"/>.
		/// </summary>
		public PaginationType Pagination
		{ 
			get { return ApiRequest.Pagination; }
			set { ApiRequest.Pagination = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellerListRequestType.GranularityLevel"/> of type <see cref="GranularityLevelCodeType"/>.
		/// </summary>
		public GranularityLevelCodeType GranularityLevel
		{ 
			get { return ApiRequest.GranularityLevel; }
			set { ApiRequest.GranularityLevel = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellerListRequestType.SKUArray"/> of type <see cref="StringCollection"/>.
		/// </summary>
		public StringCollection SKUArrayList
		{ 
			get { return ApiRequest.SKUArray; }
			set { ApiRequest.SKUArray = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellerListRequestType.IncludeWatchCount"/> of type <see cref="bool"/>.
		/// </summary>
		public bool IncludeWatchCount
		{ 
			get { return ApiRequest.IncludeWatchCount; }
			set { ApiRequest.IncludeWatchCount = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellerListRequestType.AdminEndedItemsOnly"/> of type <see cref="bool"/>.
		/// </summary>
		public bool AdminEndedItemsOnly
		{ 
			get { return ApiRequest.AdminEndedItemsOnly; }
			set { ApiRequest.AdminEndedItemsOnly = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellerListRequestType.CategoryID"/> of type <see cref="int"/>.
		/// </summary>
		public int CategoryID
		{ 
			get { return ApiRequest.CategoryID; }
			set { ApiRequest.CategoryID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellerListRequestType.IncludeVariations"/> of type <see cref="bool"/>.
		/// </summary>
		public bool IncludeVariations
		{ 
			get { return ApiRequest.IncludeVariations; }
			set { ApiRequest.IncludeVariations = value; }
		}
				/// <summary>
		/// Gets or sets the <see cref="GetSellerListRequestType.EndTimeFrom"/> and <see cref="GetSellerListRequestType.EndTimeTo"/> of type <see cref="TimeFilter"/>.
		/// </summary>
		public TimeFilter EndTimeFilter
		{ 
			get { return new TimeFilter(ApiRequest.EndTimeFrom, ApiRequest.EndTimeTo); }
			set 
			{
				if (value.TimeFrom > DateTime.MinValue)
					ApiRequest.EndTimeFrom = value.TimeFrom;
				if (value.TimeTo > DateTime.MinValue)
					ApiRequest.EndTimeTo = value.TimeTo;
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="GetSellerListRequestType.StartTimeFrom"/> and <see cref="GetSellerListRequestType.StartTimeTo"/> of type <see cref="TimeFilter"/>.
		/// </summary>
		public TimeFilter StartTimeFilter
		{ 
			get { return new TimeFilter(ApiRequest.StartTimeFrom, ApiRequest.StartTimeTo); }
			set 
			{
				if (value.TimeFrom > DateTime.MinValue)
					ApiRequest.StartTimeFrom = value.TimeFrom;
				if (value.TimeTo > DateTime.MinValue)
					ApiRequest.StartTimeTo = value.TimeTo;
			}
		}


		
 		/// <summary>
		/// Gets the returned <see cref="GetSellerListResponseType.PaginationResult"/> of type <see cref="PaginationResultType"/>.
		/// </summary>
		public PaginationResultType PaginationResult
		{ 
			get { return ApiResponse.PaginationResult; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellerListResponseType.HasMoreItems"/> of type <see cref="bool"/>.
		/// </summary>
		public bool HasMoreItems
		{ 
			get { return ApiResponse.HasMoreItems; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellerListResponseType.ItemArray"/> of type <see cref="ItemTypeCollection"/>.
		/// </summary>
		public ItemTypeCollection ItemList
		{ 
			get { return ApiResponse.ItemArray; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellerListResponseType.ItemsPerPage"/> of type <see cref="int"/>.
		/// </summary>
		public int ItemsPerPage
		{ 
			get { return ApiResponse.ItemsPerPage; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellerListResponseType.PageNumber"/> of type <see cref="int"/>.
		/// </summary>
		public int PageNumber
		{ 
			get { return ApiResponse.PageNumber; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellerListResponseType.ReturnedItemCountActual"/> of type <see cref="int"/>.
		/// </summary>
		public int ReturnedItemCountActual
		{ 
			get { return ApiResponse.ReturnedItemCountActual; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellerListResponseType.Seller"/> of type <see cref="UserType"/>.
		/// </summary>
		public UserType Seller
		{ 
			get { return ApiResponse.Seller; }
		}
		

		#endregion

		
	}
}
