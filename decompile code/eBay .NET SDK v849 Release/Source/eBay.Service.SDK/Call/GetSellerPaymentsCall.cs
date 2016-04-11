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
	public class GetSellerPaymentsCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetSellerPaymentsCall()
		{
			ApiRequest = new GetSellerPaymentsRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetSellerPaymentsCall(ApiContext ApiContext)
		{
			ApiRequest = new GetSellerPaymentsRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// <b>Half.com only.</b>&nbsp;Retrieves a summary of pending or paid payments that Half.com created for the
		/// seller identified by the authentication token in the request. Only retrieves
		/// payments that occurred within a particular pay period. Each payment is for one
		/// order line item in one order. An order can contain order line items for
		/// multiple items from multiple sellers, but this call only retrieves payments that
		/// are relevant to one seller. The financial value of a payment is typically based on
		/// an amount that a buyer paid to Half.com for an order line item, with adjustments for
		/// shipping costs and Half.com's commission. For most sellers, each month contains
		/// two pay periods: One from the 1st to the 15th of the month, and one from the 16th
		/// to the last day of the month. Sellers can refer to their account information on
		/// the Half.com site to determine their pay periods. (You cannot retrieve a seller's
		/// pay periods by using eBay API.) When a buyer makes a purchase and an
		/// order is created, Half.com creates a payment for the seller and marks it as
		/// Pending in the seller's Half.com account. Within a certain number of days after
		/// the pay period ends, Half.com settles payments for that period and marks each
		/// completed payment as Paid. See the Half.com Web site online help for more
		/// information about how payments are managed.
		/// </summary>
		/// 
		/// <param name="PaymentStatus">
		/// Filter to retrieve only items with the specified payment status (Paid or
		/// Pending). "Pending payments" are payments that Half.com has created but
		/// that have not yet been sent to the seller's financial institution. Pending
		/// payments are typically available once a buyer pays for an order. As
		/// Half.com processes payments by using periodic batch jobs, the
		/// GetSellerPayments response might not include an order line item's payment for
		/// up to 20 minutes after the buyer has paid. You can retrieve pending
		/// payments for the current pay period. Pending payments that have not been
		/// settled yet can also be retrieved for previous pay periods. "Paid
		/// payments" are payments that Half.com processed during previous pay
		/// periods. Paid payments might not appear in the seller's financial
		/// institution account balance until a certain number of days after the
		/// current pay period ends (see the Half.com online help for details). You
		/// can only retrieve paid payments for one previous pay period at a time.
		/// </param>
		///
		/// <param name="PaymentTimeFrom">
		/// Time range filter that retrieves Half.com payments that were created within
		/// a single pay period. Sellers can refer to the Half.com site to determine
		/// their pay periods. PaymentTimeFrom is the earliest (oldest) time and
		/// PaymentTimeTo is the latest (most recent) time in the range. Half.com pay
		/// periods start and end at midnight Pacific time, but the time values are
		/// stored in the database in GMT (not Pacific time). See "Time Values" in the
		/// eBay Features Guide for information about converting between GMT and
		/// Pacific time. 
		/// 
		/// If you specify a PaymentStatus of Pending, add a buffer of one hour (or one
		/// day) to both ends of the time range to retrieve more data than you need, and
		/// then filter the results on the client side as needed. If any pending
		/// payments match the request, the response may include all payments since the
		/// beginning of the period. 
		/// 
		/// If you specify a PaymentStatus of Paid, the time range must contain one
		/// full pay period. That is, PaymentTimeFrom must be earlier or equal the
		/// start time of the pay period, and PaymentTimeTo must be later than or
		/// equal to the end time of the pay period. Otherwise, no paid payments are
		/// returned. For example, if the pay period starts on 2005-09-16 and ends on
		/// 2005-09-30, you could specify an earlier PaymentTimeFrom value of
		/// 2005-09-16T00:00:00.000Z and a later PaymentTimeTo value of
		/// 2005-10-01T12:00:00.000Z. 
		/// 
		/// If you specify a time range that covers two pay periods, only the payments
		/// from the most recent pay period are returned. The earliest time you can
		/// specify is 18 months ago.
		/// </param>
		///
		/// <param name="PaymentTimeTo">
		/// Time range filter that retrieves Half.com payments for a single pay
		/// period. See the description of PaymentTimeTo for details about using this
		/// time range filter. For paid payments, this value should be equal to or
		/// later than the end of the last day of the pay period, where the time is
		/// converted to GMT. For example, if the period ends on 2005-09-30, you could
		/// specify 2005-10-01T09:00:00.000Z, which is later than the end of the last
		/// day.
		/// </param>
		///
		/// <param name="Pagination">
		/// If many payments are available, you may need to call GetSellerPayments
		/// multiple times to retrieve all the data. Each result set is returned as a
		/// page of entries. Use this Pagination information to indicate the maximum
		/// number of entries to retrieve per page (i.e., per call), the page number
		/// to retrieve, and other data.
		/// </param>
		///
		public SellerPaymentTypeCollection GetSellerPayments(RCSPaymentStatusCodeType PaymentStatus, DateTime PaymentTimeFrom, DateTime PaymentTimeTo, PaginationType Pagination)
		{
			this.PaymentStatus = PaymentStatus;
			this.PaymentTimeFrom = PaymentTimeFrom;
			this.PaymentTimeTo = PaymentTimeTo;
			this.Pagination = Pagination;

			Execute();
			return ApiResponse.SellerPayment;
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
		/// Gets or sets the <see cref="GetSellerPaymentsRequestType"/> for this API call.
		/// </summary>
		public GetSellerPaymentsRequestType ApiRequest
		{ 
			get { return (GetSellerPaymentsRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetSellerPaymentsResponseType"/> for this API call.
		/// </summary>
		public GetSellerPaymentsResponseType ApiResponse
		{ 
			get { return (GetSellerPaymentsResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellerPaymentsRequestType.PaymentStatus"/> of type <see cref="RCSPaymentStatusCodeType"/>.
		/// </summary>
		public RCSPaymentStatusCodeType PaymentStatus
		{ 
			get { return ApiRequest.PaymentStatus; }
			set { ApiRequest.PaymentStatus = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellerPaymentsRequestType.PaymentTimeFrom"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime PaymentTimeFrom
		{ 
			get { return ApiRequest.PaymentTimeFrom; }
			set { ApiRequest.PaymentTimeFrom = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellerPaymentsRequestType.PaymentTimeTo"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime PaymentTimeTo
		{ 
			get { return ApiRequest.PaymentTimeTo; }
			set { ApiRequest.PaymentTimeTo = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellerPaymentsRequestType.Pagination"/> of type <see cref="PaginationType"/>.
		/// </summary>
		public PaginationType Pagination
		{ 
			get { return ApiRequest.Pagination; }
			set { ApiRequest.Pagination = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellerPaymentsResponseType.PaginationResult"/> of type <see cref="PaginationResultType"/>.
		/// </summary>
		public PaginationResultType PaginationResult
		{ 
			get { return ApiResponse.PaginationResult; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellerPaymentsResponseType.HasMorePayments"/> of type <see cref="bool"/>.
		/// </summary>
		public bool HasMorePayments
		{ 
			get { return ApiResponse.HasMorePayments; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellerPaymentsResponseType.SellerPayment"/> of type <see cref="SellerPaymentTypeCollection"/>.
		/// </summary>
		public SellerPaymentTypeCollection SellerPaymentList
		{ 
			get { return ApiResponse.SellerPayment; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellerPaymentsResponseType.PaymentsPerPage"/> of type <see cref="int"/>.
		/// </summary>
		public int PaymentsPerPage
		{ 
			get { return ApiResponse.PaymentsPerPage; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellerPaymentsResponseType.PageNumber"/> of type <see cref="int"/>.
		/// </summary>
		public int PageNumber
		{ 
			get { return ApiResponse.PageNumber; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellerPaymentsResponseType.ReturnedPaymentCountActual"/> of type <see cref="int"/>.
		/// </summary>
		public int ReturnedPaymentCountActual
		{ 
			get { return ApiResponse.ReturnedPaymentCountActual; }
		}
		

		#endregion

		
	}
}
