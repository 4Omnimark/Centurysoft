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
	public class GetAccountCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetAccountCall()
		{
			ApiRequest = new GetAccountRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetAccountCall(ApiContext ApiContext)
		{
			ApiRequest = new GetAccountRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Returns a seller's invoice data for their eBay account, including the account's
		/// summary data.
		/// </summary>
		/// 
		/// <param name="AccountHistorySelection">
		/// Specifies the report format in which to return account entries.
		/// </param>
		///
		/// <param name="InvoiceDate">
		/// Specifies the month and year of the invoice requested. The report includes
		/// only the entries that appear on the seller's invoice in the specified month
		/// and year. An entry can occur in one month and appear on the next month's
		/// invoice. Used with SpecifiedInvoice reports.
		/// </param>
		///
		/// <param name="BeginDate">
		/// Specifies the beginning of a date range during which a credit or debit
		/// occurred. Used when AccountHistorySelection is BetweenSpecifiedDates. Value
		/// must be less than or equal to the value specified in EndDate. The allowed
		/// date formats are YYYY-MM-DD and YYYY-MM-DD HH:mm:ss. You can retrieve
		/// information that is up to 4 months old.
		/// </param>
		///
		/// <param name="EndDate">
		/// Specifies the end of a date range during which a credit or debit occurred.
		/// Used when AccountHistorySelection is BetweenSpecifiedDates. Value must be
		/// greater than or equal to the value specified in BeginDate. The allowed date
		/// formats are YYYY-MM-DD and YYYY-MM-DD HH:mm:ss.
		/// </param>
		///
		/// <param name="Pagination">
		/// Controls pagination of the response. For this request, the valid values of
		/// Pagination.EntriesPerPage are 0 to 2000, with a default of 500.
		/// </param>
		///
		/// <param name="ExcludeBalance">
		/// Specifies whether to calculate balances. Default is false, which calculates
		/// balances. The value true means do not calculate balances. If true,
		/// AccountEntry.Balance and AccountSummary.CurrentBalance are never returned
		/// in the response.
		/// </param>
		///
		/// <param name="ExcludeSummary">
		/// Specifies whether to return account summary information in an
		/// AccountSummary node. Default is true, to return AccountSummary.
		/// </param>
		///
		/// <param name="IncludeConversionRate">
		/// Specifies whether to retrieve the rate used for the currency conversion for usage transactions.
		/// </param>
		///
		/// <param name="AccountEntrySortType">
		/// Specifies how account entries should be sorted in the response, by an
		/// element and then in ascending or descending order.
		/// </param>
		///
		/// <param name="Currency">
		/// Specifies the currency used in the account report. Do not specify Currency
		/// in the request unless the following conditions are met. First, the user has
		/// or had multiple accounts under the same UserID. Second, the account
		/// identified in the request uses the currency you specify in the request. An
		/// error is returned if no account is found that uses the currency you specify
		/// in the request.
		/// </param>
		///
		/// <param name="ItemID">
		/// Specifies the item ID for which to return account entries. If ItemID is
		/// used, all other filters in the request are ignored. If the specified item
		/// does not exist or if the requesting user is not the seller of the item, an
		/// error is returned.
		/// </param>
		///
		public AccountEntryTypeCollection GetAccount(AccountHistorySelectionCodeType AccountHistorySelection, DateTime InvoiceDate, DateTime BeginDate, DateTime EndDate, PaginationType Pagination, bool ExcludeBalance, bool ExcludeSummary, bool IncludeConversionRate, AccountEntrySortTypeCodeType AccountEntrySortType, CurrencyCodeType Currency, string ItemID)
		{
			this.AccountHistorySelection = AccountHistorySelection;
			this.InvoiceDate = InvoiceDate;
			this.BeginDate = BeginDate;
			this.EndDate = EndDate;
			this.Pagination = Pagination;
			this.ExcludeBalance = ExcludeBalance;
			this.ExcludeSummary = ExcludeSummary;
			this.IncludeConversionRate = IncludeConversionRate;
			this.AccountEntrySortType = AccountEntrySortType;
			this.Currency = Currency;
			this.ItemID = ItemID;

			Execute();
			return ApiResponse.AccountEntries.AccountEntry;
		}


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public AccountEntryTypeCollection GetAccount(AccountHistorySelectionCodeType AccountHistorySelection)
		{
			this.AccountHistorySelection = AccountHistorySelection;
			Execute();
			return AccountEntryList;
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
		/// Gets or sets the <see cref="GetAccountRequestType"/> for this API call.
		/// </summary>
		public GetAccountRequestType ApiRequest
		{ 
			get { return (GetAccountRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetAccountResponseType"/> for this API call.
		/// </summary>
		public GetAccountResponseType ApiResponse
		{ 
			get { return (GetAccountResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetAccountRequestType.AccountHistorySelection"/> of type <see cref="AccountHistorySelectionCodeType"/>.
		/// </summary>
		public AccountHistorySelectionCodeType AccountHistorySelection
		{ 
			get { return ApiRequest.AccountHistorySelection; }
			set { ApiRequest.AccountHistorySelection = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetAccountRequestType.InvoiceDate"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime InvoiceDate
		{ 
			get { return ApiRequest.InvoiceDate; }
			set { ApiRequest.InvoiceDate = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetAccountRequestType.BeginDate"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime BeginDate
		{ 
			get { return ApiRequest.BeginDate; }
			set { ApiRequest.BeginDate = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetAccountRequestType.EndDate"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime EndDate
		{ 
			get { return ApiRequest.EndDate; }
			set { ApiRequest.EndDate = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetAccountRequestType.Pagination"/> of type <see cref="PaginationType"/>.
		/// </summary>
		public PaginationType Pagination
		{ 
			get { return ApiRequest.Pagination; }
			set { ApiRequest.Pagination = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetAccountRequestType.ExcludeBalance"/> of type <see cref="bool"/>.
		/// </summary>
		public bool ExcludeBalance
		{ 
			get { return ApiRequest.ExcludeBalance; }
			set { ApiRequest.ExcludeBalance = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetAccountRequestType.ExcludeSummary"/> of type <see cref="bool"/>.
		/// </summary>
		public bool ExcludeSummary
		{ 
			get { return ApiRequest.ExcludeSummary; }
			set { ApiRequest.ExcludeSummary = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetAccountRequestType.IncludeConversionRate"/> of type <see cref="bool"/>.
		/// </summary>
		public bool IncludeConversionRate
		{ 
			get { return ApiRequest.IncludeConversionRate; }
			set { ApiRequest.IncludeConversionRate = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetAccountRequestType.AccountEntrySortType"/> of type <see cref="AccountEntrySortTypeCodeType"/>.
		/// </summary>
		public AccountEntrySortTypeCodeType AccountEntrySortType
		{ 
			get { return ApiRequest.AccountEntrySortType; }
			set { ApiRequest.AccountEntrySortType = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetAccountRequestType.Currency"/> of type <see cref="CurrencyCodeType"/>.
		/// </summary>
		public CurrencyCodeType Currency
		{ 
			get { return ApiRequest.Currency; }
			set { ApiRequest.Currency = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetAccountRequestType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemID
		{ 
			get { return ApiRequest.ItemID; }
			set { ApiRequest.ItemID = value; }
		}
				/// <summary>
		/// Gets or sets the <see cref="GetAccountRequestType.BeginDate"/> and <see cref="GetAccountRequestType.EndDate"/> of type <see cref="TimeFilter"/>.
		/// </summary>
		public TimeFilter StartTimeFilter
		{ 
			get 
			{ 
				return new TimeFilter(ApiRequest.BeginDate, ApiRequest.EndDate); 
			}
			set 
			{ 
				if (value.TimeFrom > DateTime.MinValue)
					ApiRequest.BeginDate = value.TimeFrom;

				if (value.TimeTo > DateTime.MinValue)
					ApiRequest.EndDate = value.TimeTo;
			}
		}
		
		/// <summary>
		/// Gets the <see cref="GetAccountResponseType.AccountEntries.AccountEntry"/> of type <see cref="AccountEntryList"/>.
		/// </summary>
		public AccountEntryTypeCollection AccountEntryList
		{ 
			get 
			{
				if (ApiResponse.AccountEntries != null)
					return ApiResponse.AccountEntries.AccountEntry; 
				else
					return null;
			}
		}


		
 		/// <summary>
		/// Gets the returned <see cref="GetAccountResponseType.AccountID"/> of type <see cref="string"/>.
		/// </summary>
		public string AccountID
		{ 
			get { return ApiResponse.AccountID; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetAccountResponseType.AccountSummary"/> of type <see cref="AccountSummaryType"/>.
		/// </summary>
		public AccountSummaryType AccountSummary
		{ 
			get { return ApiResponse.AccountSummary; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetAccountResponseType.AccountEntries"/> of type <see cref="AccountEntriesType"/>.
		/// </summary>
		public AccountEntriesType AccountEntries
		{ 
			get { return ApiResponse.AccountEntries; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetAccountResponseType.PaginationResult"/> of type <see cref="PaginationResultType"/>.
		/// </summary>
		public PaginationResultType PaginationResult
		{ 
			get { return ApiResponse.PaginationResult; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetAccountResponseType.HasMoreEntries"/> of type <see cref="bool"/>.
		/// </summary>
		public bool HasMoreEntries
		{ 
			get { return ApiResponse.HasMoreEntries; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetAccountResponseType.EntriesPerPage"/> of type <see cref="int"/>.
		/// </summary>
		public int EntriesPerPage
		{ 
			get { return ApiResponse.EntriesPerPage; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetAccountResponseType.PageNumber"/> of type <see cref="int"/>.
		/// </summary>
		public int PageNumber
		{ 
			get { return ApiResponse.PageNumber; }
		}
		

		#endregion

		
	}
}
