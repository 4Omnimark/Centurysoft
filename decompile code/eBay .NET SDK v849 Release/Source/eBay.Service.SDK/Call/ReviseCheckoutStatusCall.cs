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
	public class ReviseCheckoutStatusCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public ReviseCheckoutStatusCall()
		{
			ApiRequest = new ReviseCheckoutStatusRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public ReviseCheckoutStatusCall(ApiContext ApiContext)
		{
			ApiRequest = new ReviseCheckoutStatusRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// A seller can use this call to update the payment details, the shipping details,
		/// and the status of an order.
		/// </summary>
		/// 
		/// <param name="ItemID">
		/// Unique identifier for an eBay item listing. A listing can have multiple
		/// order line items (transactions), but only one <b>ItemID</b>. An <b>ItemID</b> can be
		/// paired up with a corresponding <b>TransactionID</b> and used as an input filter
		/// for <b>ReviseCheckoutStatus</b>.
		/// 
		/// Unless an <b>OrderLineItemID</b> is used to identify a single line item order,
		/// or the <b>OrderID</b> is used to identify a single or multiple line item
		/// (Combined Payment) order, the <b>ItemID</b>/<b>TransactionID</b> pair must be
		/// specified. For a multiple line item (Combined Payment) order, <b>OrderID</b>
		/// should be used. If <b>OrderID</b> or <b>OrderLineItemID</b> are specified, the
		/// <b>ItemID</b>/<b>TransactionID</b> pair is ignored if present in the same request.
		/// <br />
		/// <br />
		/// It is also possible to identify a single line item order with a
		/// <b>ItemID</b>/<b>BuyerID</b> combination, but this is not the most ideal
		/// approach since an error is returned if there are multiple
		/// order line items for that combination.
		/// </param>
		///
		/// <param name="TransactionID">
		/// Unique identifier for an eBay order line item (transaction). An order
		/// line item is created once there is a commitment from a buyer to purchase
		/// an item. Since an auction listing can only have one order line item
		/// during the duration of the listing, the <b>TransactionID</b> for
		/// auction listings is always 0. Along with its corresponding <b>ItemID</b>, a
		/// <b>TransactionID</b> is used and referenced during an order checkout flow and
		/// after checkout has been completed. The <b>ItemID</b>/<b>TransactionID</b> pair can be
		/// used as an input filter for <b>ReviseCheckoutStatus</b>.
		/// 
		/// Unless an <b>OrderLineItemID</b> is used to identify a single line item order,
		/// or the <b>OrderID</b> is used to identify a single or multiple line item
		/// (Combined Payment) order, the <b>ItemID</b>/<b>TransactionID</b> pair must be
		/// specified. For a multiple line item (Combined Payment) order, <b>OrderID</b>
		/// must be used. If <b>OrderID</b> or <b>OrderLineItemID</b> are specified, the
		/// <b>ItemID</b>/<b>TransactionID</b> pair is ignored if present in the same request.
		/// </param>
		///
		/// <param name="OrderID">
		/// A unique identifier that identifies a single line item or multiple line
		/// item (Combined Payment) order.
		/// 
		/// For a single line item order, the <b>OrderID</b> value is identical to the
		/// <b>OrderLineItemID</b> value that is generated upon creation of the order line
		/// item. For a Combined Payment order, the <b>OrderID</b> value is created by eBay
		/// when the buyer or seller (sharing multiple, common order line items)
		/// combines multiple order line items into a Combined Payment order through
		/// the eBay site. A Combined Payment order can also be created by the
		/// seller through the <b>AddOrder</b> call. The <b>OrderID</b> can be used as an input
		/// filter for <b>ReviseCheckoutStatus</b>.
		/// 
		/// <b>OrderID</b> overrides an <b>OrderLineItemID</b> or <b>ItemID</b>/<b>TransactionID</b> pair if
		/// these fields are also specified in the same request.
		/// </param>
		///
		/// <param name="AmountPaid">
		/// The total amount paid by the buyer. For a motor vehicle purchased on eBay Motors,
		/// <b>AmountPaid</b> is the total amount paid by the buyer for the deposit.
		/// <b>AmountPaid</b> is optional if <b>CheckoutStatus</b> is Incomplete and required if it
		/// is Complete.
		/// </param>
		///
		/// <param name="PaymentMethodUsed">
		/// Payment method used by the buyer. This field is required if <b>
		/// CheckoutStatus</b> is Complete and the payment method is a trusted
		/// payment method other than PayPal. See eBay's
		/// <a href="http://pages.ebay.com/help/policies/accepted-payments-policy.html">Accepted Payments Policy</a>.
		/// If the payment method is PayPal, this field should not be used since only PayPal can set this field's
		/// value to "PayPal". ReviseCheckoutStatus cannot be used for a non-trusted
		/// payment method.
		/// <b>Note:</b>Required or allowed payment methods vary by site and category.
		/// </param>
		///
		/// <param name="CheckoutStatus">
		/// The current checkout status of the order. Often, the seller or
		/// application will mark this value as 'Complete' if payment has been made. The <b>CheckoutStatus</b>
		/// value cannot be updated by DE and AT sellers who are subject to the new payment
		/// process, and an attempt to do so in a <b>ReviseCheckoutStatus</b> call
		/// will result in a call error.
		/// 
		/// 
		/// <span class="tablenote"><b>Note:</b>
		/// The introduction of the new eBay payment process for the entire German and
		/// Austrian eBay marketplace has been delayed until further notice.</span>
		/// </param>
		///
		/// <param name="ShippingService">
		/// The shipping service selected by the buyer from among the shipping services
		/// offered by the seller (such as UPS Ground). For a list of valid values, call
		/// GeteBayDetails with DetailName set to ShippingServiceDetails. The
		/// ShippingServiceDetails.ValidForSellingFlow flag must also be present.
		/// Otherwise, that particular shipping service option is no longer valid and
		/// cannot be offered to buyers through a listing.
		/// <br/><br/>
		/// <span class="tablenote">
		/// <strong>Note:</strong> <strong>ReviseCheckoutStatus</strong> is not available for the Global Shipping program; specifying InternationalPriorityShipping as a value for this field will produce an error.
		/// </span>
		/// </param>
		///
		/// <param name="ShippingIncludedInTax">
		/// An indicator of whether shipping costs were included in the
		/// taxable amount. .
		/// </param>
		///
		/// <param name="CheckoutMethod">
		/// </param>
		///
		/// <param name="InsuranceType">
		/// Enumeration value that indicates whether shipping insurance was offered to and
		/// selected by the buyer.
		/// </param>
		///
		/// <param name="PaymentStatus">
		/// Marks the order as paid or awaiting payment in My eBay. If you specify
		/// Paid, My eBay displays an icon for each item in the order to indicate
		/// that the order status is Paid. If you specify Pending, this indicates
		/// that the order is awaiting payment. (Some applications may use Pending
		/// when the buyer has paid but the funds have not yet been sent to the
		/// seller's financial institution.)
		/// 
		/// 
		/// <b>ReviseCheckoutStatus</b> cannot be used to update payment and checkout
		/// status for a non-trusted payment method. See eBay's <a href="
		/// http://pages.ebay.com/help/policies/accepted-payments-policy.html">
		/// Accepted Payments Policy</a> for more information on trusted
		/// payment methods. If the payment method is PayPal, this field should not
		/// be used since PayPal automatically set this field's value to "Paid" upon
		/// receiving the buyer's payment.
		/// 
		/// 
		/// The <b>PaymentStatus</b> value cannot be updated by DE and AT sellers
		/// who are subject to the new eBay payment process, and an attempt to do so in a
		/// <b>ReviseCheckoutStatus</b> call will result in a call error.
		/// 
		/// 
		/// <span class="tablenote"><b>Note:</b>
		/// The introduction of the new eBay payment process for the entire German and
		/// Austrian eBay marketplace has been delayed until further notice.
		/// </param>
		///
		/// <param name="AdjustmentAmount">
		/// Discount or charge agreed to by the buyer and seller. A positive value
		/// indicates that the amount is an extra charge being paid to the seller by
		/// the buyer. A negative value indicates that the amount is a discount given
		/// to the buyer by the seller.
		/// </param>
		///
		/// <param name="ShippingAddress">
		/// For internal use.
		/// </param>
		///
		/// <param name="BuyerID">
		/// eBay user ID for the order's buyer. A single line item order can
		/// actually be identified by a <b>BuyerID</b>/<b>ItemID</b> pair, but this approach is
		/// not recommended since an error is returned if there are multiple
		/// order line items for that combination. <b>BuyerID</b> is ignored if any other valid
		/// filter or filter combination is used in the same request.
		/// </param>
		///
		/// <param name="ShippingInsuranceCost">
		/// The amount of money paid for shipping insurance.
		/// </param>
		///
		/// <param name="SalesTax">
		/// The sales tax amount for the order. This field should be used if sales tax
		/// was applied to the order.
		/// </param>
		///
		/// <param name="ShippingCost">
		/// The amount of money paid for shipping.
		/// </param>
		///
		/// <param name="EncryptedID">
		/// Not supported.
		/// </param>
		///
		/// <param name="ExternalTransaction">
		/// Container consisting of a unique identifier and timestamp for the electronic
		/// payment of an order. An <b>ExternalTransactionID</b> is not exposed
		/// to a new DE or AT seller who is subject to the new eBay payment process.
		/// 
		/// 
		/// <span class="tablenote"><b>Note:</b>
		/// The introduction of the new eBay payment process for the entire German and
		/// Austrian eBay marketplace has been delayed until further notice.
		/// </param>
		///
		/// <param name="MultipleSellerPaymentID">
		/// Not supported.
		/// </param>
		///
		/// <param name="CODCost">
		/// This dollar value indicates the money due from the buyer upon delivery of the item.
		/// 
		/// This field should only be specified in the <b>ReviseCheckoutStatus</b>
		/// request if 'COD' (cash-on-delivery) was the payment method selected by the buyer
		/// and it is included as the <b>PaymentMethodUsed</b> value in the same
		/// request.
		/// </param>
		///
		/// <param name="OrderLineItemID">
		/// <b>OrderLineItemID</b> is a unique identifier for an eBay order line item and
		/// is based upon the concatenation of <b>ItemID</b> and <b>TransactionID</b>, with a
		/// hyphen in between these two IDs. For a single line item order, the
		/// <b>OrderLineItemID</b> value can be passed into the <b>OrderID</b> field to revise the
		/// checkout status of the order.
		/// 
		/// Unless an <b>ItemID</b>/<b>TransactionID</b> pair is used to identify a single line
		/// item order, or the <b>OrderID</b> is used to identify a single or multiple line
		/// item (Combined Payment) order, the <b>OrderLineItemID</b> must be specified.
		/// For a multiple line item (Combined Payment) order, <b>OrderID</b> should be
		/// used. If <b>OrderLineItemID</b> is specified, the <b>ItemID</b>/<b>TransactionID</b> pair are
		/// ignored if present in the same request.
		/// </param>
		///
		public void ReviseCheckoutStatus(string ItemID, string TransactionID, string OrderID, AmountType AmountPaid, BuyerPaymentMethodCodeType PaymentMethodUsed, CompleteStatusCodeType CheckoutStatus, string ShippingService, bool ShippingIncludedInTax, CheckoutMethodCodeType CheckoutMethod, InsuranceSelectedCodeType InsuranceType, RCSPaymentStatusCodeType PaymentStatus, AmountType AdjustmentAmount, AddressType ShippingAddress, string BuyerID, AmountType ShippingInsuranceCost, AmountType SalesTax, AmountType ShippingCost, string EncryptedID, ExternalTransactionType ExternalTransaction, string MultipleSellerPaymentID, AmountType CODCost, string OrderLineItemID)
		{
			this.ItemID = ItemID;
			this.TransactionID = TransactionID;
			this.OrderID = OrderID;
			this.AmountPaid = AmountPaid;
			this.PaymentMethodUsed = PaymentMethodUsed;
			this.CheckoutStatus = CheckoutStatus;
			this.ShippingService = ShippingService;
			this.ShippingIncludedInTax = ShippingIncludedInTax;
			this.CheckoutMethod = CheckoutMethod;
			this.InsuranceType = InsuranceType;
			this.PaymentStatus = PaymentStatus;
			this.AdjustmentAmount = AdjustmentAmount;
			this.ShippingAddress = ShippingAddress;
			this.BuyerID = BuyerID;
			this.ShippingInsuranceCost = ShippingInsuranceCost;
			this.SalesTax = SalesTax;
			this.ShippingCost = ShippingCost;
			this.EncryptedID = EncryptedID;
			this.ExternalTransaction = ExternalTransaction;
			this.MultipleSellerPaymentID = MultipleSellerPaymentID;
			this.CODCost = CODCost;
			this.OrderLineItemID = OrderLineItemID;

			Execute();
			
		}


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public void ReviseCheckoutStatus(string ItemID, string TransactionID, CompleteStatusCodeType CheckoutStatus)
		{
			this.ItemID = ItemID;
			this.TransactionID = TransactionID;
			this.CheckoutStatus = CheckoutStatus;
			Execute();
		}
		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public void ReviseCheckoutStatus(string OrderID, CompleteStatusCodeType CheckoutStatus)
		{
			this.OrderID = OrderID;
			this.TransactionID = TransactionID;
			this.CheckoutStatus = CheckoutStatus;
			Execute();
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
		/// Gets or sets the <see cref="ReviseCheckoutStatusRequestType"/> for this API call.
		/// </summary>
		public ReviseCheckoutStatusRequestType ApiRequest
		{ 
			get { return (ReviseCheckoutStatusRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="ReviseCheckoutStatusResponseType"/> for this API call.
		/// </summary>
		public ReviseCheckoutStatusResponseType ApiResponse
		{ 
			get { return (ReviseCheckoutStatusResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseCheckoutStatusRequestType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemID
		{ 
			get { return ApiRequest.ItemID; }
			set { ApiRequest.ItemID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseCheckoutStatusRequestType.TransactionID"/> of type <see cref="string"/>.
		/// </summary>
		public string TransactionID
		{ 
			get { return ApiRequest.TransactionID; }
			set { ApiRequest.TransactionID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseCheckoutStatusRequestType.OrderID"/> of type <see cref="string"/>.
		/// </summary>
		public string OrderID
		{ 
			get { return ApiRequest.OrderID; }
			set { ApiRequest.OrderID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseCheckoutStatusRequestType.AmountPaid"/> of type <see cref="AmountType"/>.
		/// </summary>
		public AmountType AmountPaid
		{ 
			get { return ApiRequest.AmountPaid; }
			set { ApiRequest.AmountPaid = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseCheckoutStatusRequestType.PaymentMethodUsed"/> of type <see cref="BuyerPaymentMethodCodeType"/>.
		/// </summary>
		public BuyerPaymentMethodCodeType PaymentMethodUsed
		{ 
			get { return ApiRequest.PaymentMethodUsed; }
			set { ApiRequest.PaymentMethodUsed = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseCheckoutStatusRequestType.CheckoutStatus"/> of type <see cref="CompleteStatusCodeType"/>.
		/// </summary>
		public CompleteStatusCodeType CheckoutStatus
		{ 
			get { return ApiRequest.CheckoutStatus; }
			set { ApiRequest.CheckoutStatus = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseCheckoutStatusRequestType.ShippingService"/> of type <see cref="string"/>.
		/// </summary>
		public string ShippingService
		{ 
			get { return ApiRequest.ShippingService; }
			set { ApiRequest.ShippingService = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseCheckoutStatusRequestType.ShippingIncludedInTax"/> of type <see cref="bool"/>.
		/// </summary>
		public bool ShippingIncludedInTax
		{ 
			get { return ApiRequest.ShippingIncludedInTax; }
			set { ApiRequest.ShippingIncludedInTax = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseCheckoutStatusRequestType.CheckoutMethod"/> of type <see cref="CheckoutMethodCodeType"/>.
		/// </summary>
		public CheckoutMethodCodeType CheckoutMethod
		{ 
			get { return ApiRequest.CheckoutMethod; }
			set { ApiRequest.CheckoutMethod = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseCheckoutStatusRequestType.InsuranceType"/> of type <see cref="InsuranceSelectedCodeType"/>.
		/// </summary>
		public InsuranceSelectedCodeType InsuranceType
		{ 
			get { return ApiRequest.InsuranceType; }
			set { ApiRequest.InsuranceType = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseCheckoutStatusRequestType.PaymentStatus"/> of type <see cref="RCSPaymentStatusCodeType"/>.
		/// </summary>
		public RCSPaymentStatusCodeType PaymentStatus
		{ 
			get { return ApiRequest.PaymentStatus; }
			set { ApiRequest.PaymentStatus = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseCheckoutStatusRequestType.AdjustmentAmount"/> of type <see cref="AmountType"/>.
		/// </summary>
		public AmountType AdjustmentAmount
		{ 
			get { return ApiRequest.AdjustmentAmount; }
			set { ApiRequest.AdjustmentAmount = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseCheckoutStatusRequestType.ShippingAddress"/> of type <see cref="AddressType"/>.
		/// </summary>
		public AddressType ShippingAddress
		{ 
			get { return ApiRequest.ShippingAddress; }
			set { ApiRequest.ShippingAddress = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseCheckoutStatusRequestType.BuyerID"/> of type <see cref="string"/>.
		/// </summary>
		public string BuyerID
		{ 
			get { return ApiRequest.BuyerID; }
			set { ApiRequest.BuyerID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseCheckoutStatusRequestType.ShippingInsuranceCost"/> of type <see cref="AmountType"/>.
		/// </summary>
		public AmountType ShippingInsuranceCost
		{ 
			get { return ApiRequest.ShippingInsuranceCost; }
			set { ApiRequest.ShippingInsuranceCost = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseCheckoutStatusRequestType.SalesTax"/> of type <see cref="AmountType"/>.
		/// </summary>
		public AmountType SalesTax
		{ 
			get { return ApiRequest.SalesTax; }
			set { ApiRequest.SalesTax = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseCheckoutStatusRequestType.ShippingCost"/> of type <see cref="AmountType"/>.
		/// </summary>
		public AmountType ShippingCost
		{ 
			get { return ApiRequest.ShippingCost; }
			set { ApiRequest.ShippingCost = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseCheckoutStatusRequestType.EncryptedID"/> of type <see cref="string"/>.
		/// </summary>
		public string EncryptedID
		{ 
			get { return ApiRequest.EncryptedID; }
			set { ApiRequest.EncryptedID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseCheckoutStatusRequestType.ExternalTransaction"/> of type <see cref="ExternalTransactionType"/>.
		/// </summary>
		public ExternalTransactionType ExternalTransaction
		{ 
			get { return ApiRequest.ExternalTransaction; }
			set { ApiRequest.ExternalTransaction = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseCheckoutStatusRequestType.MultipleSellerPaymentID"/> of type <see cref="string"/>.
		/// </summary>
		public string MultipleSellerPaymentID
		{ 
			get { return ApiRequest.MultipleSellerPaymentID; }
			set { ApiRequest.MultipleSellerPaymentID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseCheckoutStatusRequestType.CODCost"/> of type <see cref="AmountType"/>.
		/// </summary>
		public AmountType CODCost
		{ 
			get { return ApiRequest.CODCost; }
			set { ApiRequest.CODCost = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseCheckoutStatusRequestType.OrderLineItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string OrderLineItemID
		{ 
			get { return ApiRequest.OrderLineItemID; }
			set { ApiRequest.OrderLineItemID = value; }
		}
		
		

		#endregion

		
	}
}
