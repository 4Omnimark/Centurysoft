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
	public class GetItemTransactionsCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetItemTransactionsCall()
		{
			ApiRequest = new GetItemTransactionsRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetItemTransactionsCall(ApiContext ApiContext)
		{
			ApiRequest = new GetItemTransactionsRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Retrieves order line item (transaction) information for a specified <b>ItemID</b>. &
		/// nbsp;<b>Also for Half.com</b>. The call returns zero, one, or
		/// multiple order line items, depending on the number of items sold from the listing.
		/// 
		/// You can retrieve order line item data for a specific time range or
		/// number of days. If you don't specify a range or number of days, order line item
		/// data will be returned for the past 30 days.
		/// </summary>
		/// 
		/// <param name="ItemID">
		/// Unique identifier for an eBay item listing. A listing can have multiple
		/// order line items (transactions), but only one <b>ItemID</b>. When you use
		/// <b>ItemID</b> alone, eBay returns all order line items that are associated with
		/// the <b>ItemID</b> (listing). If you pair <b>ItemID</b> with a specific <b>TransactionID</b>,
		/// data on a specific order line item is returned. If <b>OrderLineItemID</b> is
		/// specified in the request, any <b>ItemID</b>/<b>TransactionID</b> pair specified in the
		/// same request will be ignored.
		/// 
		/// 
		/// <span class="tablenote"><b>Note:</b>
		/// <b>GetItemTransactions</b> doesn't support SKU as an input because this
		/// call requires an identifier that is unique across your active
		/// and ended listings. Even when <b>InventoryTrackingMethod</b> is set to
		/// <b>SKU</b> in a listing, the SKU is only unique across your active
		/// listings (not your ended listings). To retrieve order line items
		/// by SKU, use <b>GetSellerTransactions</b> or <b>GetOrderTransactions</b> instead.
		/// </span>
		/// </param>
		///
		/// <param name="ModTimeFrom">
		/// The <b>ModTimeFrom</b> and <b>ModTimeTo</b> fields specify a date range for retrieving
		/// order line items associated with the specified <b>ItemID</b>. The <b>ModTimeFrom</b>
		/// field is the starting date range. All eBay order line items that were
		/// last modified within this date range are returned in the output. The
		/// maximum date range that may be specified is 30 days. This field is not
		/// applicable if a specific <b>TransactionID</b> or <b>OrderLineItemID</b> is included in
		/// the request or if the <b>NumberOfDays</b> date filter is used.
		/// 
		/// If you don't specify a <b>ModTimeFrom</b>/<b>ModTimeTo</b> filter, the <b>NumberOfDays</b>
		/// time filter is used and it defaults to 30 (days). 
		/// </param>
		///
		/// <param name="ModTimeTo">
		/// The <b>ModTimeFrom</b> and <b>ModTimeTo</b> fields specify a date range for retrieving
		/// order line items associated with the specified <b>ItemID</b>. The <b>ModTimeTo</b>
		/// field is the ending date range. All eBay order line items that were last
		/// modified within this date range are returned in the output. The maximum
		/// date range that may be specified is 30 days. If the <b>ModTimeFrom</b> field is
		/// used and the <b>ModTimeTo</b> field is omitted, the <b>ModTimeTo</b> value defaults to
		/// the present time or to 30 days past the <b>ModTimeFrom</b> value (if
		/// <b>ModTimeFrom</b> value is more than 30 days in the past). This field is not
		/// applicable if a specific <b>TransactionID</b> or <b>OrderLineItemID</b> is included in
		/// the request or if the <b>NumberOfDays</b> date filter is used.
		/// 
		/// If you don't specify a <b>ModTimeFrom</b>/<b>ModTimeTo</b> filter, the <b>NumberOfDays</b>
		/// time filter is used and it defaults to 30 (days).
		/// </param>
		///
		/// <param name="TransactionID">
		/// Include a <b>TransactionID</b> field in the request if you want to retrieve the
		/// data for a specific order line item (transaction). If a <b>TransactionID</b> is
		/// provided, any specified time filter is ignored. 
		/// </param>
		///
		/// <param name="Pagination">
		/// Child elements control pagination of the output. Use the <b>EntriesPerPage</b>
		/// property to control the number of order line items to
		/// return per call and the <b>PageNumber</b> property to specify the specific page
		/// of data to return. If multiple pages of order line items are returned
		/// based on input criteria and Pagination properties, <b>GetItemTransactions</b>
		/// will need to be called multiple times (with the <b>PageNumber</b> value being
		/// increased by 1 each time) to scroll through all results.
		/// </param>
		///
		/// <param name="IncludeFinalValueFee">
		/// Indicates whether to include the Final Value Fee (FVF) for all order
		/// line items in the response. The Final Value Fee is
		/// returned in the <b>Transaction.FinalValueFee</b> field. The Final Value Fee is
		/// assessed right after the creation of an order line item.
		/// 
		/// </param>
		///
		/// <param name="IncludeContainingOrder">
		/// Include this field and set it to True if you want the <b>ContainingOrder</b>
		/// container to be returned in the response under each <b>Transaction</b> node.
		/// For single line item orders, the <b>ContainingOrder.OrderID</b> value takes the
		/// value of the <b>OrderLineItemID</b> value for the order line item. For Combined
		/// Payment orders, the <b>ContainingOrder.OrderID</b> value will be shared by at
		/// least two order line items (transactions) that are part of the same order.
		/// </param>
		///
		/// <param name="Platform">
		/// Name of the eBay co-branded site upon which the order line item
		/// was created. This filter can be used to limit the number
		/// of results that are returned.
		/// </param>
		///
		/// <param name="NumberOfDays">
		/// This time filter specifies the number of days (24-hour periods) in the
		/// past to search for order line items. All eBay order line items that were
		/// either created or modified within this period are returned in the
		/// response. If specified, <b>NumberOfDays</b> will override any date range
		/// specified with the <b>ModTimeFrom</b>/<b>ModTimeTo</b> time filters. This field is not
		/// applicable if a specific <b>TransactionID</b> or <b>OrderLineItemID</b> is included in
		/// the request.
		/// </param>
		///
		/// <param name="IncludeVariations">
		/// If included in the request and set to True, all variations defined for
		/// the item are returned at the root level, including variations
		/// that have no sales. If not included in the request or set to false, the
		/// variations with sales are still returned in separate <b>Transaction</b> nodes. This information is intended to help sellers to reconcile their
		/// local inventory with eBay's records, while processing order line items
		/// (without requiring a separate call to <b>GetItem</b>).
		/// </param>
		///
		/// <param name="OrderLineItemID">
		/// A unique identifier for an eBay order line item. This field is created
		/// as soon as there is a commitment to buy from the seller, and its value
		/// is based upon the concatenation of <b>ItemID</b> and <b>TransactionID</b>, with a
		/// hyphen in between these two IDs. If you want to retrieve data on a
		/// specific order line item, you can use an <b>OrderLineItemID</b> value in the
		/// request instead of the <b>ItemID</b>/<b>TransactionID</b> pair. If an <b>OrderLineItemID</b> is
		/// provided, any specified time filter is ignored. 
		/// </param>
		///
		public TransactionTypeCollection GetItemTransactions(string ItemID, DateTime ModTimeFrom, DateTime ModTimeTo, string TransactionID, PaginationType Pagination, bool IncludeFinalValueFee, bool IncludeContainingOrder, TransactionPlatformCodeType Platform, int NumberOfDays, bool IncludeVariations, string OrderLineItemID)
		{
			this.ItemID = ItemID;
			this.ModTimeFrom = ModTimeFrom;
			this.ModTimeTo = ModTimeTo;
			this.TransactionID = TransactionID;
			this.Pagination = Pagination;
			this.IncludeFinalValueFee = IncludeFinalValueFee;
			this.IncludeContainingOrder = IncludeContainingOrder;
			this.Platform = Platform;
			this.NumberOfDays = NumberOfDays;
			this.IncludeVariations = IncludeVariations;
			this.OrderLineItemID = OrderLineItemID;

			Execute();
			return ApiResponse.TransactionArray;
		}


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public TransactionTypeCollection GetItemTransactions(string ItemID, TimeFilter ModTimeFilter)
		{
			this.ItemID = ItemID;
			this.ModTimeFilter = ModTimeFilter;
			Execute();
			return TransactionList;
		}
		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public TransactionTypeCollection GetItemTransactions(string ItemID, DateTime TimeFrom, DateTime TimeTo)
		{
			this.ItemID = ItemID;
			this.ModTimeFilter = new TimeFilter(TimeFrom, TimeTo);
			Execute();
			return TransactionList;
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
		/// Gets or sets the <see cref="GetItemTransactionsRequestType"/> for this API call.
		/// </summary>
		public GetItemTransactionsRequestType ApiRequest
		{ 
			get { return (GetItemTransactionsRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetItemTransactionsResponseType"/> for this API call.
		/// </summary>
		public GetItemTransactionsResponseType ApiResponse
		{ 
			get { return (GetItemTransactionsResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetItemTransactionsRequestType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemID
		{ 
			get { return ApiRequest.ItemID; }
			set { ApiRequest.ItemID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetItemTransactionsRequestType.ModTimeFrom"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime ModTimeFrom
		{ 
			get { return ApiRequest.ModTimeFrom; }
			set { ApiRequest.ModTimeFrom = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetItemTransactionsRequestType.ModTimeTo"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime ModTimeTo
		{ 
			get { return ApiRequest.ModTimeTo; }
			set { ApiRequest.ModTimeTo = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetItemTransactionsRequestType.TransactionID"/> of type <see cref="string"/>.
		/// </summary>
		public string TransactionID
		{ 
			get { return ApiRequest.TransactionID; }
			set { ApiRequest.TransactionID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetItemTransactionsRequestType.Pagination"/> of type <see cref="PaginationType"/>.
		/// </summary>
		public PaginationType Pagination
		{ 
			get { return ApiRequest.Pagination; }
			set { ApiRequest.Pagination = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetItemTransactionsRequestType.IncludeFinalValueFee"/> of type <see cref="bool"/>.
		/// </summary>
		public bool IncludeFinalValueFee
		{ 
			get { return ApiRequest.IncludeFinalValueFee; }
			set { ApiRequest.IncludeFinalValueFee = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetItemTransactionsRequestType.IncludeContainingOrder"/> of type <see cref="bool"/>.
		/// </summary>
		public bool IncludeContainingOrder
		{ 
			get { return ApiRequest.IncludeContainingOrder; }
			set { ApiRequest.IncludeContainingOrder = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetItemTransactionsRequestType.Platform"/> of type <see cref="TransactionPlatformCodeType"/>.
		/// </summary>
		public TransactionPlatformCodeType Platform
		{ 
			get { return ApiRequest.Platform; }
			set { ApiRequest.Platform = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetItemTransactionsRequestType.NumberOfDays"/> of type <see cref="int"/>.
		/// </summary>
		public int NumberOfDays
		{ 
			get { return ApiRequest.NumberOfDays; }
			set { ApiRequest.NumberOfDays = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetItemTransactionsRequestType.IncludeVariations"/> of type <see cref="bool"/>.
		/// </summary>
		public bool IncludeVariations
		{ 
			get { return ApiRequest.IncludeVariations; }
			set { ApiRequest.IncludeVariations = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetItemTransactionsRequestType.OrderLineItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string OrderLineItemID
		{ 
			get { return ApiRequest.OrderLineItemID; }
			set { ApiRequest.OrderLineItemID = value; }
		}
				/// <summary>
		/// Gets or sets the <see cref="GetItemTransactionsRequestType.ModTimeFrom"/> and <see cref="GetItemTransactionsRequestType.ModTimeTo"/> of type <see cref="ModTimeFilter"/>.
		/// </summary>
		public TimeFilter ModTimeFilter
		{ 
			get 
			{ 
				return new TimeFilter(ApiRequest.ModTimeFrom, ApiRequest.ModTimeTo); 
			}
			set 
			{
				ApiRequest.ModTimeFrom = value.TimeFrom;
				ApiRequest.ModTimeTo = value.TimeTo; 
			}
		}

		
 		/// <summary>
		/// Gets the returned <see cref="GetItemTransactionsResponseType.PaginationResult"/> of type <see cref="PaginationResultType"/>.
		/// </summary>
		public PaginationResultType PaginationResult
		{ 
			get { return ApiResponse.PaginationResult; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetItemTransactionsResponseType.HasMoreTransactions"/> of type <see cref="bool"/>.
		/// </summary>
		public bool HasMoreTransactions
		{ 
			get { return ApiResponse.HasMoreTransactions; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetItemTransactionsResponseType.TransactionsPerPage"/> of type <see cref="int"/>.
		/// </summary>
		public int TransactionsPerPage
		{ 
			get { return ApiResponse.TransactionsPerPage; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetItemTransactionsResponseType.PageNumber"/> of type <see cref="int"/>.
		/// </summary>
		public int PageNumber
		{ 
			get { return ApiResponse.PageNumber; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetItemTransactionsResponseType.ReturnedTransactionCountActual"/> of type <see cref="int"/>.
		/// </summary>
		public int ReturnedTransactionCountActual
		{ 
			get { return ApiResponse.ReturnedTransactionCountActual; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetItemTransactionsResponseType.Item"/> of type <see cref="ItemType"/>.
		/// </summary>
		public ItemType Item
		{ 
			get { return ApiResponse.Item; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetItemTransactionsResponseType.TransactionArray"/> of type <see cref="TransactionTypeCollection"/>.
		/// </summary>
		public TransactionTypeCollection TransactionList
		{ 
			get { return ApiResponse.TransactionArray; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetItemTransactionsResponseType.PayPalPreferred"/> of type <see cref="bool"/>.
		/// </summary>
		public bool PayPalPreferred
		{ 
			get { return ApiResponse.PayPalPreferred; }
		}
		

		#endregion

		
	}
}
