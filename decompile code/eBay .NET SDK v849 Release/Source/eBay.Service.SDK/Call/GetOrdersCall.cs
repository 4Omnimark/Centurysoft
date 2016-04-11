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
	public class GetOrdersCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetOrdersCall()
		{
			ApiRequest = new GetOrdersRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetOrdersCall(ApiContext ApiContext)
		{
			ApiRequest = new GetOrdersRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Retrieves the orders for which the authenticated user is a participant, either as the buyer
		/// or the seller.&nbsp;<b>Also for Half.com</b>. The call returns all the
		/// orders that meet the request specifications.
		/// </summary>
		/// 
		/// <param name="OrderIDList">
		/// A container for eBay order IDs. If one or more order IDs are specified in this
		/// container, no other call-specific input fields are applicable.
		/// 
		/// Not applicable to Half.com.
		/// </param>
		///
		/// <param name="CreateTimeFrom">
		/// The <b>CreateTimeFrom</b> and <b>CreateTimeTo</b> fields
		/// specify a date range for retrieving orders. The <b>CreateTimeFrom</b> field is the
		/// starting date range. All eBay orders that were created within this date range are
		/// returned in the output. The maximum date range that may be specified with the
		/// <b>CreateTimeFrom</b> and <b>CreateTimeTo</b> fields is 90 
		/// days. <b>CreateTimeFrom</b>/<b>CreateTimeTo</b> date
		/// filters are ignored if the <b>NumberOfDays</b> date filter is used in the request, or if
		/// one or more order IDs are passed in the request.
		/// 
		/// Applicable to Half.com.
		/// </param>
		///
		/// <param name="CreateTimeTo">
		/// The <b>CreateTimeFrom</b> and <b>CreateTimeTo</b> fields specify a date range for retrieving
		/// orders. The <b>CreateTimeTo</b> field is the ending date range. All eBay orders that were
		/// created within this date range are returned in the output. The maximum
		/// date range that may be specified with the <b>CreateTimeFrom</b> and <b>CreateTimeTo</b> fields
		/// is 90 days. If the <b>CreateTimeFrom</b> field is used and the
		/// <b>CreateTimeTo</b> field is omitted, the "TimeTo" value defaults to the present time or
		/// to 90 days past the <b>CreateTimeFrom</b> value (if <b>CreateTimeFrom</b> value is more than 90
		/// days in the past). <b>CreateTimeFrom</b>/<b>CreateTimeTo</b> date filters are ignored if the
		/// <b>NumberOfDays</b> date filter is used in the request, or if one or more order IDs are
		/// passed in the request.
		/// 
		/// <span class="tablenote"><strong>Note:</strong>
		/// If a GetOrders call is made within a few seconds after the creation of a multiple
		/// line item order, the caller runs the risk of retrieving orders that are in an
		/// inconsistent state, since the order consolidation involved in a multi-line item order
		/// may not have been completed. For
		/// this reason, it is recommended that sellers include the
		/// <b>CreateTimeTo</b> field in the call, and set its value to: <i>
		/// Current Time</i> - 2 minutes.
		/// 
		/// Applicable to Half.com.
		/// </param>
		///
		/// <param name="OrderRole">
		/// Filters the returned orders based on the role of the user. The user's role is
		/// either buyer or seller. If this field is used with a date filter, returned orders
		/// must satisfy both the date range and the OrderRole value.
		/// 
		/// Applicable to eBay.com and Half.com.
		/// </param>
		///
		/// <param name="OrderStatus">
		/// The field is used to retrieve orders that are in a specific state. If this field is used with a date filter, only orders that satisfy both the date range and the <b>OrderStatus</b> value are retrieved. 
		/// 
		/// For eBay orders, this field's value can be set to 'Active', 'Completed', 'Cancelled' or 'Inactive' to retrieve orders in these states. The 'Shipped' value is only applicable for Half.com orders. 
		/// 
		/// To retrieve Half.com orders, this field's value should be set to 'Shipped', and the the <b>ListingType</b> field should be included and set to 'Half'. 
		/// 
		/// If one or more <b>OrderID</b> values are specified through the <b>OrderIDArray</b> container, the <b>OrderStatus</b> field should not be used, and it is ignored if it is used. 
		/// </param>
		///
		/// <param name="ListingType">
		/// Specify Half to retrieve Half.com orders.
		/// 
		/// <span class="tablenote"><strong>Note:</strong>
		/// Do not use this field if you are retrieving eBay orders.
		/// 
		/// This field cannot be used as a listing type filter on eBay.com. If not
		/// provided, or if you specify any value other than Half, this field has
		/// no useful effect and the call retrieves eBay orders of all types. Also,
		/// you can't retrieve both eBay and Half.com orders in the same response.
		/// </span>
		/// </param>
		///
		/// <param name="Pagination">
		/// If many orders are
		/// available to retrieve, you may need to call GetOrders multiple times to retrieve all
		/// the data. Each result set is returned as a page of entries. Use the
		/// Pagination filters to control the maximum number of entries to
		/// retrieve per page (i.e., per call), the page number to retrieve, and
		/// other data.
		/// </param>
		///
		/// <param name="ModTimeFrom">
		/// The ModTimeFrom and ModTimeTo fields specify a date range for retrieving
		/// existing orders that have been modified within this time window (for example,
		/// 'Incomplete' status to 'Pending' status or 'Pending' status to 'Complete' status). The
		/// ModTimeFrom field is the starting date range. All eBay orders that were last
		/// modified within this date range are returned in the output. The maximum date
		/// range that may be specified with the ModTimeFrom and ModTimeTo fields is 30
		/// days. ModTimeFrom/ModTimeTo date filters are ignored if the
		/// CreateTimeFrom/CreateTimeTo or NumberOfDays date filters are used in the
		/// request, or if one or more order IDs are passed in the request.
		/// 
		/// Applicable to Half.com.
		/// </param>
		///
		/// <param name="ModTimeTo">
		/// The ModTimeFrom and ModTimeTo fields specify a date range for retrieving
		/// existing orders that have been modified within this time window (for example,
		/// 'Incomplete' status to 'Pending' status or 'Pending' status to 'Complete'
		/// status). The ModTimeTo field is the ending date range. All eBay orders that were
		/// last modified within this date range are returned in the output. The maximum
		/// date range that may be specified with the ModTimeFrom and ModTimeTo fields is 30
		/// days. If the ModTimeFrom field is used and the ModTimeTo field is omitted, the
		/// "TimeTo" value defaults to the present time (if ModTimeFrom value is less than
		/// 30 days in the past) or to 30 days past the ModTimeFrom value.
		/// ModTimeFrom/ModTimeTo date filters are ignored if the
		/// CreateTimeFrom/CreateTimeTo or NumberOfDays date filters are used in the
		/// request, or if one or more order IDs are passed in the request.
		/// 
		/// Applicable to Half.com.
		/// </param>
		///
		/// <param name="NumberOfDays">
		/// This filter specifies the number of days (24-hour periods) in the past to search
		/// for orders. All eBay orders that were either created or modified within this
		/// period are returned in the output. This field cannot be used in conjunction with
		/// the CreateTimeFrom/CreateTimeTo or ModTimeFrom/ModTimeTo date filters. This date 
		/// filter is ignored if one or more order IDs are passed in the request.
		/// 
		/// Applicable to Half.com.
		/// </param>
		///
		/// <param name="IncludeFinalValueFee">
		/// Indicates whether to include the Final Value Fee (FVF) for all Transaction objects in the
		/// response. The Final Value Fee is returned in Transaction.FinalValueFee. The Final
		/// Value Fee is assessed right after the creation of an eBay order line item.
		/// 
		/// </param>
		///
		/// <param name="SortingOrder">
		/// Specifies how orders returned by this call should be sorted (using <strong>LastModifiedTime</strong> as the sort key). A value of <code>Ascending</code> returns the earliest modified orders first, and a value of <code>Descending</code> returns the latest modified orders first.
		/// <br/><br/>
		/// Default: <code>Ascending</code>
		/// </param>
		///
		public OrderTypeCollection GetOrders(StringCollection OrderIDList, DateTime CreateTimeFrom, DateTime CreateTimeTo, TradingRoleCodeType OrderRole, OrderStatusCodeType OrderStatus, ListingTypeCodeType ListingType, PaginationType Pagination, DateTime ModTimeFrom, DateTime ModTimeTo, int NumberOfDays, bool IncludeFinalValueFee, SortOrderCodeType SortingOrder)
		{
			this.OrderIDList = OrderIDList;
			this.CreateTimeFrom = CreateTimeFrom;
			this.CreateTimeTo = CreateTimeTo;
			this.OrderRole = OrderRole;
			this.OrderStatus = OrderStatus;
			this.ListingType = ListingType;
			this.Pagination = Pagination;
			this.ModTimeFrom = ModTimeFrom;
			this.ModTimeTo = ModTimeTo;
			this.NumberOfDays = NumberOfDays;
			this.IncludeFinalValueFee = IncludeFinalValueFee;
			this.SortingOrder = SortingOrder;

			Execute();
			return ApiResponse.OrderArray;
		}


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public OrderTypeCollection GetOrders(StringCollection OrderIDList)
		{
			this.OrderIDList = OrderIDList;
			Execute();
			return OrderList;
		}
		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public OrderTypeCollection GetOrders(TimeFilter CreateTimeFilter, TradingRoleCodeType OrderRole, OrderStatusCodeType OrderStatus)
		{
			this.OrderRole = OrderRole;
			this.OrderStatus = OrderStatus;
			this.CreateTimeFilter = CreateTimeFilter;
			this.OrderIDList = OrderIDList;
			Execute();
			return OrderList;
		}
		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public OrderTypeCollection GetOrders(DateTime CreateTimeFrom, DateTime CreateTimeTo, TradingRoleCodeType OrderRole, OrderStatusCodeType OrderStatus)
		{
			this.OrderRole = OrderRole;
			this.OrderStatus = OrderStatus;
			this.CreateTimeFilter = new TimeFilter(CreateTimeFrom, CreateTimeTo);
			this.OrderIDList = OrderIDList;
			Execute();
			return OrderList;
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
		/// Gets or sets the <see cref="GetOrdersRequestType"/> for this API call.
		/// </summary>
		public GetOrdersRequestType ApiRequest
		{ 
			get { return (GetOrdersRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetOrdersResponseType"/> for this API call.
		/// </summary>
		public GetOrdersResponseType ApiResponse
		{ 
			get { return (GetOrdersResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetOrdersRequestType.OrderIDArray"/> of type <see cref="StringCollection"/>.
		/// </summary>
		public StringCollection OrderIDList
		{ 
			get { return ApiRequest.OrderIDArray; }
			set { ApiRequest.OrderIDArray = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetOrdersRequestType.CreateTimeFrom"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime CreateTimeFrom
		{ 
			get { return ApiRequest.CreateTimeFrom; }
			set { ApiRequest.CreateTimeFrom = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetOrdersRequestType.CreateTimeTo"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime CreateTimeTo
		{ 
			get { return ApiRequest.CreateTimeTo; }
			set { ApiRequest.CreateTimeTo = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetOrdersRequestType.OrderRole"/> of type <see cref="TradingRoleCodeType"/>.
		/// </summary>
		public TradingRoleCodeType OrderRole
		{ 
			get { return ApiRequest.OrderRole; }
			set { ApiRequest.OrderRole = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetOrdersRequestType.OrderStatus"/> of type <see cref="OrderStatusCodeType"/>.
		/// </summary>
		public OrderStatusCodeType OrderStatus
		{ 
			get { return ApiRequest.OrderStatus; }
			set { ApiRequest.OrderStatus = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetOrdersRequestType.ListingType"/> of type <see cref="ListingTypeCodeType"/>.
		/// </summary>
		public ListingTypeCodeType ListingType
		{ 
			get { return ApiRequest.ListingType; }
			set { ApiRequest.ListingType = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetOrdersRequestType.Pagination"/> of type <see cref="PaginationType"/>.
		/// </summary>
		public PaginationType Pagination
		{ 
			get { return ApiRequest.Pagination; }
			set { ApiRequest.Pagination = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetOrdersRequestType.ModTimeFrom"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime ModTimeFrom
		{ 
			get { return ApiRequest.ModTimeFrom; }
			set { ApiRequest.ModTimeFrom = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetOrdersRequestType.ModTimeTo"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime ModTimeTo
		{ 
			get { return ApiRequest.ModTimeTo; }
			set { ApiRequest.ModTimeTo = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetOrdersRequestType.NumberOfDays"/> of type <see cref="int"/>.
		/// </summary>
		public int NumberOfDays
		{ 
			get { return ApiRequest.NumberOfDays; }
			set { ApiRequest.NumberOfDays = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetOrdersRequestType.IncludeFinalValueFee"/> of type <see cref="bool"/>.
		/// </summary>
		public bool IncludeFinalValueFee
		{ 
			get { return ApiRequest.IncludeFinalValueFee; }
			set { ApiRequest.IncludeFinalValueFee = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetOrdersRequestType.SortingOrder"/> of type <see cref="SortOrderCodeType"/>.
		/// </summary>
		public SortOrderCodeType SortingOrder
		{ 
			get { return ApiRequest.SortingOrder; }
			set { ApiRequest.SortingOrder = value; }
		}
				/// <summary>
		/// Gets or sets the <see cref="GetOrdersRequestType.CreateTimeFrom"/> and <see cref="GetOrdersRequestType.CreateTimeTo"/> of type <see cref="TimeFilter"/>.
		/// </summary>
		public TimeFilter CreateTimeFilter
		{ 
			get { return new TimeFilter(ApiRequest.CreateTimeFrom, ApiRequest.CreateTimeTo); }
			set { 
				if (value.TimeFrom > DateTime.MinValue)
					ApiRequest.CreateTimeFrom = value.TimeFrom;
				if (value.TimeTo > DateTime.MinValue)
					ApiRequest.CreateTimeTo = value.TimeTo;
			}
		}

		
 		/// <summary>
		/// Gets the returned <see cref="GetOrdersResponseType.PaginationResult"/> of type <see cref="PaginationResultType"/>.
		/// </summary>
		public PaginationResultType PaginationResult
		{ 
			get { return ApiResponse.PaginationResult; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetOrdersResponseType.HasMoreOrders"/> of type <see cref="bool"/>.
		/// </summary>
		public bool HasMoreOrders
		{ 
			get { return ApiResponse.HasMoreOrders; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetOrdersResponseType.OrderArray"/> of type <see cref="OrderTypeCollection"/>.
		/// </summary>
		public OrderTypeCollection OrderList
		{ 
			get { return ApiResponse.OrderArray; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetOrdersResponseType.OrdersPerPage"/> of type <see cref="int"/>.
		/// </summary>
		public int OrdersPerPage
		{ 
			get { return ApiResponse.OrdersPerPage; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetOrdersResponseType.PageNumber"/> of type <see cref="int"/>.
		/// </summary>
		public int PageNumber
		{ 
			get { return ApiResponse.PageNumber; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetOrdersResponseType.ReturnedOrderCountActual"/> of type <see cref="int"/>.
		/// </summary>
		public int ReturnedOrderCountActual
		{ 
			get { return ApiResponse.ReturnedOrderCountActual; }
		}
		

		#endregion

		
	}
}
