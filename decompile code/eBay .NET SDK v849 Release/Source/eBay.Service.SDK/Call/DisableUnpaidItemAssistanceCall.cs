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
	public class DisableUnpaidItemAssistanceCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public DisableUnpaidItemAssistanceCall()
		{
			ApiRequest = new DisableUnpaidItemAssistanceRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public DisableUnpaidItemAssistanceCall(ApiContext ApiContext)
		{
			ApiRequest = new DisableUnpaidItemAssistanceRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Enables a seller who has opted into the automated Unpaid Item Assistant
		/// mechanism to disable the Unpaid Item Assistant at the order line item
		/// level. This call can be made whether or not a Unpaid Item dispute
		/// exists for the order line item. If a dispute has already been created by the
		/// Unpaid Item Assistant, it is converted to a "manual" dispute for the seller to
		/// manage like any other manually-created dispute.
		/// </summary>
		/// 
		/// <param name="ItemID">
		/// Unique identifier for an eBay item listing. A listing can have multiple
		/// order line items (transactions), but only one <b>ItemID</b>. To identify a
		/// specific order line item for which to disable the Unpaid Item Assistant
		/// mechanism, either an <b>ItemID</b>/<b>TransactionID</b> pair, an <b>OrderLineItemID</b>, or a
		/// <b>DisputeID</b> (if a dispute record already exists) is required in the request. 
		/// </param>
		///
		/// <param name="TransactionID">
		/// The unique identifier of an order line item (transaction). An order line
		/// item is created once there is a commitment from a buyer to
		/// purchase an item. To identify a specific order line item for which to
		/// disable the Unpaid Item Assistant mechanism, either an <b>ItemID</b>/<b>TransactionID</b>
		/// pair, an <b>OrderLineItemID</b>, or a <b>DisputeID</b> (if a dispute record already
		/// exists) is required in the request.
		/// </param>
		///
		/// <param name="DisputeID">
		/// A unique identifier for an Unpaid Item dispute. Any order line item can
		/// only have one dispute record, so <b>DisputeID</b> can be used in the request to
		/// identify a specific order line item. If an <b>ItemID</b>/<b>TransactionID</b> pair or an
		/// <b>OrderLineItemID</b> is used to identify an order line item, <b>DisputeID</b> cannot be used and will be ignored if it is included in the request.
		/// </param>
		///
		/// <param name="OrderLineItemID">
		/// <b>OrderLineItemID</b> is a unique identifier for an eBay order line item and is
		/// based upon the concatenation of <b>ItemID</b> and <b>TransactionID</b>, with a hyphen in
		/// between these two IDs. To identify a specific order line item for which to
		/// disable the Unpaid Item Assistant mechanism, either an <b>ItemID</b>/<b>TransactionID</b>
		/// pair, an <b>OrderLineItemID</b>, or a <b>DisputeID</b> is required in the request.
		/// </param>
		///
		public void DisableUnpaidItemAssistance(string ItemID, string TransactionID, string DisputeID, string OrderLineItemID)
		{
			this.ItemID = ItemID;
			this.TransactionID = TransactionID;
			this.DisputeID = DisputeID;
			this.OrderLineItemID = OrderLineItemID;

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
		/// Gets or sets the <see cref="DisableUnpaidItemAssistanceRequestType"/> for this API call.
		/// </summary>
		public DisableUnpaidItemAssistanceRequestType ApiRequest
		{ 
			get { return (DisableUnpaidItemAssistanceRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="DisableUnpaidItemAssistanceResponseType"/> for this API call.
		/// </summary>
		public DisableUnpaidItemAssistanceResponseType ApiResponse
		{ 
			get { return (DisableUnpaidItemAssistanceResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="DisableUnpaidItemAssistanceRequestType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemID
		{ 
			get { return ApiRequest.ItemID; }
			set { ApiRequest.ItemID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="DisableUnpaidItemAssistanceRequestType.TransactionID"/> of type <see cref="string"/>.
		/// </summary>
		public string TransactionID
		{ 
			get { return ApiRequest.TransactionID; }
			set { ApiRequest.TransactionID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="DisableUnpaidItemAssistanceRequestType.DisputeID"/> of type <see cref="string"/>.
		/// </summary>
		public string DisputeID
		{ 
			get { return ApiRequest.DisputeID; }
			set { ApiRequest.DisputeID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="DisableUnpaidItemAssistanceRequestType.OrderLineItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string OrderLineItemID
		{ 
			get { return ApiRequest.OrderLineItemID; }
			set { ApiRequest.OrderLineItemID = value; }
		}
		
		

		#endregion

		
	}
}
