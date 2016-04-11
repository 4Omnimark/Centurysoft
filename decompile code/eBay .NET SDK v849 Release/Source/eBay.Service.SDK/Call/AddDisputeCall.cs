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
	public class AddDisputeCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public AddDisputeCall()
		{
			ApiRequest = new AddDisputeRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public AddDisputeCall(ApiContext ApiContext)
		{
			ApiRequest = new AddDisputeRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// This call enables a seller to create an Unpaid Item case against a buyer, or to cancel a
		/// single line item order.
		/// <br/><br/>
		/// <span class="tablenote"><b>Note:</b>
		/// This call is only used by sellers to create an Unpaid Item case or to mutually cancel a
		/// single line item order. Buyers must use the eBay Resolution Center or PayPal Resolution
		/// Center (for orders that satisfy requirements) to create an Item Not Received or an Item
		/// Significantly Not as Described case.
		/// </span>
		/// </summary>
		/// 
		/// <param name="DisputeExplanation">
		/// This enumerated value gives the explanation of why the buyer or seller opened a
		/// case (or why seller canceled an order line item). Not all values contained in 
		/// <b>DisputeExplanationCodeType</b> are allowed in the 
		/// <b>AddDispute</b> call, and the values that are allowed must match 
		/// the <b>DisputeReason</b> value.
		/// </param>
		///
		/// <param name="DisputeReason">
		/// The type of dispute being created. <b>BuyerHasNotPaid</b> and
		/// <b>TransactionMutuallyCanceled</b> are the only values that may
		/// be used with the <b>AddDispute</b> call. 
		/// </param>
		///
		/// <param name="ItemID">
		/// Unique identifier for an eBay item listing. A listing can have multiple
		/// orders, but only one <b>ItemID</b>. To
		/// identify a specific order line item, either an
		/// <b>ItemID</b>/<b>TransactionID</b> pair or an
		/// <b>OrderLineItemID</b> value must be passed in the request. So,
		/// unless <b>OrderLineItemID</b> is used, this field is required.
		/// 
		/// </param>
		///
		/// <param name="TransactionID">
		/// The unique identifier of an order line item. An order line item is created once
		/// a buyer purchases the item through a fixed-priced listing or by using the Buy It
		/// Now feature in an auction listing, or when an auction listing ends with a
		/// winning bidder. To identify a specific order line item, either an 
		/// <b>ItemID</b>/<b>TransactionID</b> pair or an 
		/// <b>OrderLineItemID</b> value must be passed in the request. So,
		/// unless <b>OrderLineItemID</b> is used, this field is required.
		/// 
		/// </param>
		///
		/// <param name="OrderLineItemID">
		/// <b>OrderLineItemID</b> is a unique identifier of an order line item,
		/// and is based upon the concatenation of <b>ItemID</b> and 
		/// <b>TransactionID</b>, with a hyphen in between these two IDs. To 
		/// identify a specific order line item, either an 
		/// <b>ItemID</b>/<b>TransactionID</b> pair or an 
		/// <b>OrderLineItemID</b> value must be passed in the request. So,
		/// unless <b>ItemID</b>/<b>TransactionID</b> pair is used,
		/// this field is required.
		/// 
		/// </param>
		///
		public string AddDispute(DisputeExplanationCodeType DisputeExplanation, DisputeReasonCodeType DisputeReason, string ItemID, string TransactionID, string OrderLineItemID)
		{
			this.DisputeExplanation = DisputeExplanation;
			this.DisputeReason = DisputeReason;
			this.ItemID = ItemID;
			this.TransactionID = TransactionID;
			this.OrderLineItemID = OrderLineItemID;

			Execute();
			return ApiResponse.DisputeID;
		}


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public string AddDispute(string ItemID, string TransactionID, DisputeReasonCodeType DisputeReason, DisputeExplanationCodeType DisputeExplanation)
		{
			this.ItemID = ItemID;
			this.TransactionID = TransactionID;
			this.DisputeReason = DisputeReason;
			this.DisputeExplanation = DisputeExplanation;
			Execute();
			return this.DisputeID;
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
		/// Gets or sets the <see cref="AddDisputeRequestType"/> for this API call.
		/// </summary>
		public AddDisputeRequestType ApiRequest
		{ 
			get { return (AddDisputeRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="AddDisputeResponseType"/> for this API call.
		/// </summary>
		public AddDisputeResponseType ApiResponse
		{ 
			get { return (AddDisputeResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="AddDisputeRequestType.DisputeExplanation"/> of type <see cref="DisputeExplanationCodeType"/>.
		/// </summary>
		public DisputeExplanationCodeType DisputeExplanation
		{ 
			get { return ApiRequest.DisputeExplanation; }
			set { ApiRequest.DisputeExplanation = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="AddDisputeRequestType.DisputeReason"/> of type <see cref="DisputeReasonCodeType"/>.
		/// </summary>
		public DisputeReasonCodeType DisputeReason
		{ 
			get { return ApiRequest.DisputeReason; }
			set { ApiRequest.DisputeReason = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="AddDisputeRequestType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemID
		{ 
			get { return ApiRequest.ItemID; }
			set { ApiRequest.ItemID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="AddDisputeRequestType.TransactionID"/> of type <see cref="string"/>.
		/// </summary>
		public string TransactionID
		{ 
			get { return ApiRequest.TransactionID; }
			set { ApiRequest.TransactionID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="AddDisputeRequestType.OrderLineItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string OrderLineItemID
		{ 
			get { return ApiRequest.OrderLineItemID; }
			set { ApiRequest.OrderLineItemID = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="AddDisputeResponseType.DisputeID"/> of type <see cref="string"/>.
		/// </summary>
		public string DisputeID
		{ 
			get { return ApiResponse.DisputeID; }
		}
		

		#endregion

		
	}
}
