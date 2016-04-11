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
	public class GetSellerTransactionsCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetSellerTransactionsCall()
		{
			ApiRequest = new GetSellerTransactionsRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetSellerTransactionsCall(ApiContext ApiContext)
		{
			ApiRequest = new GetSellerTransactionsRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Retrieves order line item (transaction) information for the user for which the
		/// call is made, and not for any other user.&nbsp;<b>Also for
		/// Half.com</b>. (To retrieve order line items for another seller's listings, use
		/// GetItemTransactions.)
		/// </summary>
		/// 
		/// <param name="ModTimeFrom">
		/// The ModTimeFrom and ModTimeTo fields specify a date range for retrieving
		/// order line items associated with the seller. The ModTimeFrom
		/// field is the starting date range. All of the seller's order line items that were
		/// last modified within this date range are returned in the output. The
		/// maximum date range that may be specified is 30 days. This field is not
		/// applicable if the NumberOfDays date filter is used.
		/// 
		/// If you don't specify a ModTimeFrom/ModTimeTo filter, the NumberOfDays
		/// time filter is used and it defaults to 30 (days). 
		/// </param>
		///
		/// <param name="ModTimeTo">
		/// The ModTimeFrom and ModTimeTo fields specify a date range for retrieving
		/// order line items associated with the seller. The ModTimeTo
		/// field is the ending date range. All of the seller's order line items that were last
		/// modified within this date range are returned in the output. The maximum
		/// date range that may be specified is 30 days. If the ModTimeFrom field is
		/// used and the ModTimeTo field is omitted, the ModTimeTo value defaults to
		/// the present time or to 30 days past the ModTimeFrom value (if
		/// ModTimeFrom value is more than 30 days in the past). This field is not
		/// applicable if the NumberOfDays date filter is used.
		/// 
		/// If you don't specify a ModTimeFrom/ModTimeTo filter, the NumberOfDays
		/// time filter is used and it defaults to 30 (days).
		/// </param>
		///
		/// <param name="Pagination">
		/// Child elements control pagination of the output. Use EntriesPerPage property to
		/// control the number of transactions to return per call and PageNumber property to
		/// specify the page of data to return.
		/// </param>
		///
		/// <param name="IncludeFinalValueFee">
		/// Indicates whether to include Final Value Fee (FVF) in the response. For most
		/// listing types, the Final Value Fee is returned in Transaction.FinalValueFee.
		/// The Final Value Fee is returned for each order line item.
		/// 
		/// </param>
		///
		/// <param name="IncludeContainingOrder">
		/// Include this field and set it to True if you want the ContainingOrder
		/// container to be returned in the response under each Transaction node.
		/// For single line item orders, the ContainingOrder.OrderID value takes the
		/// value of the OrderLineItemID value for the order line item. For Combined
		/// Payment orders, the ContainingOrder.OrderID value will be shared by at
		/// least two order line items (transactions) that are part of the same
		/// order.
		/// </param>
		///
		/// <param name="SKUArrayList">
		/// Container for a set of SKUs.
		/// Filters (reduces) the response to only include order line items
		/// for listings that include any of the specified SKUs.
		/// If multiple listings include the same SKU, order line items for
		/// all of them are returned (assuming they also match the other
		/// criteria in the GetSellerTransactions request).
		/// 
		/// You can combine SKUArray with InventoryTrackingMethod.
		/// For example, if you also pass in InventoryTrackingMethod=SKU,
		/// the response only includes order line items for listings that
		/// include InventoryTrackingMethod=SKU and one of the
		/// requested SKUs.
		/// </param>
		///
		/// <param name="Platform">
		/// Name of the eBay co-branded site upon which the order line item was made.
		/// This will serve as a filter for the order line items to get emitted in the response.
		/// </param>
		///
		/// <param name="NumberOfDays">
		/// NumberOfDays enables you to specify the number of days' worth of new and modified
		/// order line items that you want to retrieve. The call response contains the
		/// order line items whose status was modified within the specified number of days since
		/// the API call was made. NumberOfDays is often preferable to using the ModTimeFrom
		/// and ModTimeTo filters because you only need to specify one value. If you use
		/// NumberOfDays, then ModTimeFrom and ModTimeTo are ignored. For this field, one day
		/// is defined as 24 hours.
		/// </param>
		///
		/// <param name="InventoryTrackingMethod">
		/// Filters the response to only include order line items for listings
		/// that match this InventoryTrackingMethod setting. 
		/// 
		/// To track items by seller-defined SKU values instead of by Item IDs, the 
		/// <b>InventoryTrackingMethod</b> must be included and set to 'SKU' 
		/// in the <b>AddFixedPriceItem</b> (or <b>ReviseFixedPriceItem</b> 
		/// or <b>RelistFixedPriceItem</b>) call.
		/// 
		/// 
		/// You can combine SKUArray with InventoryTrackingMethod.
		/// For example, if you set this to SKU and you also pass in
		/// SKUArray, the response only includes order line items for listings
		/// that include InventoryTrackingMethod=SKU and one of the
		/// requested SKUs.
		/// </param>
		///
		/// <param name="IncludeCodiceFiscale">
		/// If this flag is included in the request and set to 'true', the buyer's Codice Fiscale 
		/// number is returned in the response (if provided by the buyer).
		/// <br/><br/>
		/// This field is only applicable to Italian sellers. The Codice Fiscale number is unique 
		/// for each Italian citizen and is used for tax purposes.
		/// </param>
		///
		public TransactionTypeCollection GetSellerTransactions(DateTime ModTimeFrom, DateTime ModTimeTo, PaginationType Pagination, bool IncludeFinalValueFee, bool IncludeContainingOrder, StringCollection SKUArrayList, TransactionPlatformCodeType Platform, int NumberOfDays, InventoryTrackingMethodCodeType InventoryTrackingMethod, bool IncludeCodiceFiscale)
		{
			this.ModTimeFrom = ModTimeFrom;
			this.ModTimeTo = ModTimeTo;
			this.Pagination = Pagination;
			this.IncludeFinalValueFee = IncludeFinalValueFee;
			this.IncludeContainingOrder = IncludeContainingOrder;
			this.SKUArrayList = SKUArrayList;
			this.Platform = Platform;
			this.NumberOfDays = NumberOfDays;
			this.InventoryTrackingMethod = InventoryTrackingMethod;
			this.IncludeCodiceFiscale = IncludeCodiceFiscale;

			Execute();
			return ApiResponse.TransactionArray;
		}


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public TransactionTypeCollection GetSellerTransactions(TimeFilter ModTimeFilter)
		{
			this.ModTimeFilter = ModTimeFilter;
			Execute();
			return TransactionList;
		}
		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public TransactionTypeCollection GetSellerTransactions(DateTime TimeFrom, DateTime TimeTo)
		{
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
		/// Gets or sets the <see cref="GetSellerTransactionsRequestType"/> for this API call.
		/// </summary>
		public GetSellerTransactionsRequestType ApiRequest
		{ 
			get { return (GetSellerTransactionsRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetSellerTransactionsResponseType"/> for this API call.
		/// </summary>
		public GetSellerTransactionsResponseType ApiResponse
		{ 
			get { return (GetSellerTransactionsResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellerTransactionsRequestType.ModTimeFrom"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime ModTimeFrom
		{ 
			get { return ApiRequest.ModTimeFrom; }
			set { ApiRequest.ModTimeFrom = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellerTransactionsRequestType.ModTimeTo"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime ModTimeTo
		{ 
			get { return ApiRequest.ModTimeTo; }
			set { ApiRequest.ModTimeTo = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellerTransactionsRequestType.Pagination"/> of type <see cref="PaginationType"/>.
		/// </summary>
		public PaginationType Pagination
		{ 
			get { return ApiRequest.Pagination; }
			set { ApiRequest.Pagination = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellerTransactionsRequestType.IncludeFinalValueFee"/> of type <see cref="bool"/>.
		/// </summary>
		public bool IncludeFinalValueFee
		{ 
			get { return ApiRequest.IncludeFinalValueFee; }
			set { ApiRequest.IncludeFinalValueFee = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellerTransactionsRequestType.IncludeContainingOrder"/> of type <see cref="bool"/>.
		/// </summary>
		public bool IncludeContainingOrder
		{ 
			get { return ApiRequest.IncludeContainingOrder; }
			set { ApiRequest.IncludeContainingOrder = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellerTransactionsRequestType.SKUArray"/> of type <see cref="StringCollection"/>.
		/// </summary>
		public StringCollection SKUArrayList
		{ 
			get { return ApiRequest.SKUArray; }
			set { ApiRequest.SKUArray = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellerTransactionsRequestType.Platform"/> of type <see cref="TransactionPlatformCodeType"/>.
		/// </summary>
		public TransactionPlatformCodeType Platform
		{ 
			get { return ApiRequest.Platform; }
			set { ApiRequest.Platform = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellerTransactionsRequestType.NumberOfDays"/> of type <see cref="int"/>.
		/// </summary>
		public int NumberOfDays
		{ 
			get { return ApiRequest.NumberOfDays; }
			set { ApiRequest.NumberOfDays = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellerTransactionsRequestType.InventoryTrackingMethod"/> of type <see cref="InventoryTrackingMethodCodeType"/>.
		/// </summary>
		public InventoryTrackingMethodCodeType InventoryTrackingMethod
		{ 
			get { return ApiRequest.InventoryTrackingMethod; }
			set { ApiRequest.InventoryTrackingMethod = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellerTransactionsRequestType.IncludeCodiceFiscale"/> of type <see cref="bool"/>.
		/// </summary>
		public bool IncludeCodiceFiscale
		{ 
			get { return ApiRequest.IncludeCodiceFiscale; }
			set { ApiRequest.IncludeCodiceFiscale = value; }
		}
				/// <summary>
		/// Gets or sets the <see cref="GetSellerTransactionsRequestType.ModTimeFrom"/> and <see cref="GetSellerTransactionsRequestType.ModTimeTo"/> of type <see cref="TimeFilter"/>.
		/// </summary>
		public TimeFilter ModTimeFilter
		{ 
			get { return new TimeFilter(ApiRequest.ModTimeFrom, ApiRequest.ModTimeTo); }
			set 
			{
				if (value.TimeFrom > DateTime.MinValue)
					ApiRequest.ModTimeFrom = value.TimeFrom;
				if (value.TimeTo > DateTime.MinValue)
					ApiRequest.ModTimeTo = value.TimeTo;
			}
		}

		
 		/// <summary>
		/// Gets the returned <see cref="GetSellerTransactionsResponseType.PaginationResult"/> of type <see cref="PaginationResultType"/>.
		/// </summary>
		public PaginationResultType PaginationResult
		{ 
			get { return ApiResponse.PaginationResult; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellerTransactionsResponseType.HasMoreTransactions"/> of type <see cref="bool"/>.
		/// </summary>
		public bool HasMoreTransactions
		{ 
			get { return ApiResponse.HasMoreTransactions; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellerTransactionsResponseType.TransactionsPerPage"/> of type <see cref="int"/>.
		/// </summary>
		public int TransactionsPerPage
		{ 
			get { return ApiResponse.TransactionsPerPage; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellerTransactionsResponseType.PageNumber"/> of type <see cref="int"/>.
		/// </summary>
		public int PageNumber
		{ 
			get { return ApiResponse.PageNumber; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellerTransactionsResponseType.ReturnedTransactionCountActual"/> of type <see cref="int"/>.
		/// </summary>
		public int ReturnedTransactionCountActual
		{ 
			get { return ApiResponse.ReturnedTransactionCountActual; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellerTransactionsResponseType.Seller"/> of type <see cref="UserType"/>.
		/// </summary>
		public UserType Seller
		{ 
			get { return ApiResponse.Seller; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellerTransactionsResponseType.TransactionArray"/> of type <see cref="TransactionTypeCollection"/>.
		/// </summary>
		public TransactionTypeCollection TransactionList
		{ 
			get { return ApiResponse.TransactionArray; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellerTransactionsResponseType.PayPalPreferred"/> of type <see cref="bool"/>.
		/// </summary>
		public bool PayPalPreferred
		{ 
			get { return ApiResponse.PayPalPreferred; }
		}
		

		#endregion

		
	}
}
