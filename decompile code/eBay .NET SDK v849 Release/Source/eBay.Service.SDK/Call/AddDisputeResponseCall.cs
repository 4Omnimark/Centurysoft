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
	public class AddDisputeResponseCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public AddDisputeResponseCall()
		{
			ApiRequest = new AddDisputeResponseRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public AddDisputeResponseCall(ApiContext ApiContext)
		{
			ApiRequest = new AddDisputeResponseRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// This call allows a seller to respond to or close an Item Not Received dispute.
		/// </summary>
		/// 
		/// <param name="DisputeID">
		/// The unique identifier of the dispute,
		/// returned when the dispute was created.
		/// </param>
		///
		/// <param name="MessageText">
		/// The text of a comment or response being posted to the dispute. Required
		/// when DisputeActivity is SellerAddInformation, SellerComment, or
		/// SellerPaymentNotReceived; otherwise, optional.
		/// </param>
		///
		/// <param name="DisputeActivity">
		/// The type of activity the seller is taking on the dispute. The allowed value is
		/// determined by the current value of DisputeState, returned by GetDispute or
		/// GetUserDisputes. Some values relate to Unpaid Item disputes and other values relate
		/// to buyer-created disputes known as Item Not Received or Significantly Not As
		/// Described disputes.
		/// </param>
		///
		/// <param name="ShippingCarrierUsed">
		/// The shipping carrier used to ship the item in dispute. Non-alphanumeric
		/// characters are not allowed. This field (along with ShipmentTrackNumber) is
		/// required if DisputeActivity is SellerShippedItem.
		/// </param>
		///
		/// <param name="ShipmentTrackNumber">
		/// The tracking number associated with one package of a shipment. The seller is
		/// responsible for the accuracy of the shipment tracking number, as eBay only
		/// verifies that the tracking number is consistent with the numbering scheme
		/// used by the specified shipping carrier, but cannot verify the accuracy of
		/// the tracking number. This field (along with ShippingCarrierUsed) is required
		/// if DisputeActivity is SellerShippedItem.
		/// </param>
		///
		/// <param name="ShippingTime">
		/// This timestamp indicates the date and time when the item under dispute was
		/// shipped. This field is required if DisputeActivity is SellerShippedItem.
		/// </param>
		///
		public void AddDisputeResponse(string DisputeID, string MessageText, DisputeActivityCodeType DisputeActivity, string ShippingCarrierUsed, string ShipmentTrackNumber, DateTime ShippingTime)
		{
			this.DisputeID = DisputeID;
			this.MessageText = MessageText;
			this.DisputeActivity = DisputeActivity;
			this.ShippingCarrierUsed = ShippingCarrierUsed;
			this.ShipmentTrackNumber = ShipmentTrackNumber;
			this.ShippingTime = ShippingTime;

			Execute();
			
		}


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public void AddDisputeResponse(string DisputeID, string MessageText, DisputeActivityCodeType DisputeActivity)
		{
			this.DisputeID = DisputeID;
			this.MessageText = MessageText;
			this.DisputeActivity = DisputeActivity;
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
		/// Gets or sets the <see cref="AddDisputeResponseRequestType"/> for this API call.
		/// </summary>
		public AddDisputeResponseRequestType ApiRequest
		{ 
			get { return (AddDisputeResponseRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="AddDisputeResponseResponseType"/> for this API call.
		/// </summary>
		public AddDisputeResponseResponseType ApiResponse
		{ 
			get { return (AddDisputeResponseResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="AddDisputeResponseRequestType.DisputeID"/> of type <see cref="string"/>.
		/// </summary>
		public string DisputeID
		{ 
			get { return ApiRequest.DisputeID; }
			set { ApiRequest.DisputeID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="AddDisputeResponseRequestType.MessageText"/> of type <see cref="string"/>.
		/// </summary>
		public string MessageText
		{ 
			get { return ApiRequest.MessageText; }
			set { ApiRequest.MessageText = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="AddDisputeResponseRequestType.DisputeActivity"/> of type <see cref="DisputeActivityCodeType"/>.
		/// </summary>
		public DisputeActivityCodeType DisputeActivity
		{ 
			get { return ApiRequest.DisputeActivity; }
			set { ApiRequest.DisputeActivity = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="AddDisputeResponseRequestType.ShippingCarrierUsed"/> of type <see cref="string"/>.
		/// </summary>
		public string ShippingCarrierUsed
		{ 
			get { return ApiRequest.ShippingCarrierUsed; }
			set { ApiRequest.ShippingCarrierUsed = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="AddDisputeResponseRequestType.ShipmentTrackNumber"/> of type <see cref="string"/>.
		/// </summary>
		public string ShipmentTrackNumber
		{ 
			get { return ApiRequest.ShipmentTrackNumber; }
			set { ApiRequest.ShipmentTrackNumber = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="AddDisputeResponseRequestType.ShippingTime"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime ShippingTime
		{ 
			get { return ApiRequest.ShippingTime; }
			set { ApiRequest.ShippingTime = value; }
		}
		
		

		#endregion

		
	}
}
