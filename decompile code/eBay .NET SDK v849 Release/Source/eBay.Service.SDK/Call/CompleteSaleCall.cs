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
	public class CompleteSaleCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public CompleteSaleCall()
		{
			ApiRequest = new CompleteSaleRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public CompleteSaleCall(ApiContext ApiContext)
		{
			ApiRequest = new CompleteSaleRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Enables a seller to do various tasks after the creation of a single line item or
		/// multiple line item order. Typically, this call is used after the
		/// buyer has paid for the order, but it can be called by the
		/// seller beforehand. Typical post-payment tasks available to this call include
		/// marking the order as paid, marking the order as shipped, providing shipment tracking
		/// details, and leaving feedback for the buyer.
		/// </summary>
		/// 
		/// <param name="ItemID">
		/// Unique identifier for an eBay item listing. An <b>ItemID</b> can be paired up
		/// with a corresponding <b>TransactionID</b> and used in the <b>CompleteSale</b> request
		/// to identify a single line item order.
		/// 
		/// Unless an <b>OrderLineItemID</b> is used to identify a single line item order,
		/// or the <b>OrderID</b> is used to identify a single or multiple line item
		/// order, the <b>ItemID</b>/<b>TransactionID</b> pair must be
		/// specified. For a multiple line item order, <b>OrderID</b>
		/// must be used. If <b>OrderID</b> or <b>OrderLineItemID</b> are specified, the
		/// <b>ItemID</b>/<b>TransactionID</b> pair is ignored if present in the same request.
		/// </param>
		///
		/// <param name="TransactionID">
		/// Unique identifier for an eBay order line item (transaction). The
		/// <b>TransactionID</b> can be paired up with the corresponding <b>ItemID</b> and used in
		/// the <b>CompleteSale</b> request to identify a single line item order.
		/// 
		/// Unless an <b>OrderLineItemID</b> is used to identify a single line item order,
		/// or the <b>OrderID</b> is used to identify a single or multiple line item
		/// order, the <b>ItemID</b>/<b>TransactionID</b> pair must be
		/// specified. For a multiple line item order, <b>OrderID</b>
		/// must be used. If <b>OrderID</b> or <b>OrderLineItemID</b> are specified, the
		/// <b>ItemID</b>/<b>TransactionID</b> pair is ignored if present in the same request.
		/// </param>
		///
		/// <param name="FeedbackInfo">
		/// This container is used by the seller to leave feedback for the buyer for the
		/// order line item identified in the call request. The seller must include and
		/// specify all fields of this type, including the buyer's eBay User ID, the
		/// Feedback rating (a seller can only leave a buyer a "Positive" rating), and a
		/// comment, which helps justify the Feedback rating. The eBay User ID must match 
		/// the buyer who bought the order line item, or an error will occur. An error will 
		/// also occur if Feedback has already been left for the buyer (either through API 
		/// or the Web flow).
		/// 
		/// To determine if Feedback has already been left for an order line item, you can 
		/// call <b class="con">GetFeedback</b>, passing in the 
		/// <b class="con">OrderLineItemID</b> value in the call request.
		/// </param>
		///
		/// <param name="Shipped">
		/// The seller includes and sets this field to true if the order has been
		/// shipped. If the call is successful, the order line item(s) are marked as
		/// Shipped in My eBay.
		/// 
		/// If the seller includes and sets this field to false, the order line item(s)
		/// are marked (or remain) as Not Shipped in My eBay.
		/// 
		/// If this field is not included, the shipped status of the order line
		/// item(s) remain unchanged in My eBay.
		/// 
		/// If shipment tracking information is provided for an order line item through
		/// the Shipment container in the same request, the order line item is marked as
		/// shipped automatically and the <b>Shipped</b> field is not
		/// required.
		/// </param>
		///
		/// <param name="Paid">
		/// The seller includes and sets this field to true if the buyer has paid for
		/// the order. If the call is successful, the
		/// order line item(s) are marked as Paid in My eBay.
		/// 
		/// If the seller includes and sets this field to false, the order line item(s)
		/// are marked (or remain) as Not Paid in My eBay.
		/// 
		/// If this field is not included, the paid status of the order line
		/// item(s) remain unchanged in My eBay.
		/// </param>
		///
		/// <param name="ListingType">
		/// This field is required if <b>CompleteSale</b> is being used for a Half.com
		/// order. The value should be set to <i>Half</i>, which is the only applicable
		/// <b>ListingType</b> value for this call.
		/// </param>
		///
		/// <param name="Shipment">
		/// Container consisting of shipment tracking information, shipped time, and an
		/// optional text field to provide additional details to the buyer. Setting the
		/// tracking number and shipping carrier automatically marks the item as shipped
		/// and the <b>Shipped</b> field is not required.
		/// 
		/// (If you supply <b>ShipmentTrackingNumber</b> you must also supply 
		/// <b>ShippingCarrierUsed</b>, otherwise you will get an error.
		/// 
		/// To modify the shipping tracking number and/or carrier, supply the new number
		/// in <b>ShipmentTrackingNumber</b> or supply the value for
		/// <b>ShippingCarrierUsed</b> or both. The old number and carrier
		/// are deleted and the new ones are added.
		/// 
		/// To simply delete the current tracking details altogether, supply empty
		/// <b>Shipment</b> tags.
		///  
		/// 
		/// <span class="tablenote"><b>Note:</b>
		/// Top-Rated sellers must have a record of uploading shipment tracking
		/// information (through site or through API) for at least 90 percent of their order line
		/// items (purchased by U.S. buyers) to keep their status as Top-Rated sellers. For more
		/// information on the requirements to becoming a Top-Rated Seller, see the 
		/// <a href="http://pages.ebay.com/help/sell/top-rated.html">Becoming a Top-Rated Seller and qualifying for Top-Rated Plus</a> 
		/// customer support page.
		/// </span>
		/// 
		/// </param>
		///
		/// <param name="OrderID">
		/// A unique identifier that identifies a single line item or multiple line
		/// item order. 
		///  
		/// For a single line item order, the <b>OrderID</b> value is identical to the
		/// <b>OrderLineItemID</b> value that is generated upon creation of the order line
		/// item. For multiple line item orders, the <b>OrderID</b> value is created by eBay
		/// when the buyer is purchasing multiple order line items from the same seller at the same time.
		/// For multiple line item orders not going through the eBay Cart flow, a Combined Payment order can be created by the seller
		/// through the <b>AddOrder</b> call. The <b>OrderID</b> can be used in the <b>CompleteSale</b>
		/// request to identify a single or multiple line item order.
		/// 
		/// <b>OrderID</b> overrides an <b>OrderLineItemID</b> or <b>ItemID</b>/<b>TransactionID</b> pair if
		/// these fields are also specified in the same request.
		/// </param>
		///
		/// <param name="OrderLineItemID">
		/// <b>OrderLineItemID</b> is a unique identifier for an eBay order line item and
		/// is based upon the concatenation of <b>ItemID</b> and <b>TransactionID</b>, with a
		/// hyphen in between these two IDs. For a single line item order, the
		/// <b>OrderLineItemID</b> value can be passed into the <b>OrderID</b> field in the
		/// <b>CompleteSale</b> request.
		/// 
		/// Unless an <b>ItemID</b>/<b>TransactionID</b> pair is used to identify a single line
		/// item order, or the <b>OrderID</b> is used to identify a single or multiple line
		/// item order, the <b>OrderLineItemID</b> must be specified.
		/// For a multiple line item order, <b>OrderID</b> must be
		/// used. If <b>OrderLineItemID</b> is specified, the <b>ItemID</b>/<b>TransactionID</b> pair are
		/// ignored if present in the same request.
		/// </param>
		///
		public void CompleteSale(string ItemID, string TransactionID, FeedbackInfoType FeedbackInfo, bool Shipped, bool Paid, ListingTypeCodeType ListingType, ShipmentType Shipment, string OrderID, string OrderLineItemID)
		{
			this.ItemID = ItemID;
			this.TransactionID = TransactionID;
			this.FeedbackInfo = FeedbackInfo;
			this.Shipped = Shipped;
			this.Paid = Paid;
			this.ListingType = ListingType;
			this.Shipment = Shipment;
			this.OrderID = OrderID;
			this.OrderLineItemID = OrderLineItemID;

			Execute();
			
		}


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public void CompleteSale(string ItemID, string TransactionID, bool Paid, bool Shipped, FeedbackInfoType FeedbackInfo)
		{
			this.ItemID = ItemID;
			this.TransactionID = TransactionID;
			this.Paid = Paid;
			this.Shipped = Shipped;
			this.FeedbackInfo = FeedbackInfo;
			Execute();
		}
		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public void CompleteSale(string ItemID, string TransactionID, bool Paid, bool Shipped)
		{
			this.ItemID = ItemID;
			this.TransactionID = TransactionID;
			this.Paid = Paid;
			this.Shipped = Shipped;
			Execute();
		}
		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>

		public void CompleteSale(string ItemID, string TransactionID, FeedbackInfoType FeedbackInfo, bool Shipped, bool Paid)
		{
			this.ItemID = ItemID;
			this.TransactionID = TransactionID;
			this.FeedbackInfo = FeedbackInfo;
			this.Shipped = Shipped;
			this.Paid = Paid;

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
		/// Gets or sets the <see cref="CompleteSaleRequestType"/> for this API call.
		/// </summary>
		public CompleteSaleRequestType ApiRequest
		{ 
			get { return (CompleteSaleRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="CompleteSaleResponseType"/> for this API call.
		/// </summary>
		public CompleteSaleResponseType ApiResponse
		{ 
			get { return (CompleteSaleResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="CompleteSaleRequestType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemID
		{ 
			get { return ApiRequest.ItemID; }
			set { ApiRequest.ItemID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="CompleteSaleRequestType.TransactionID"/> of type <see cref="string"/>.
		/// </summary>
		public string TransactionID
		{ 
			get { return ApiRequest.TransactionID; }
			set { ApiRequest.TransactionID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="CompleteSaleRequestType.FeedbackInfo"/> of type <see cref="FeedbackInfoType"/>.
		/// </summary>
		public FeedbackInfoType FeedbackInfo
		{ 
			get { return ApiRequest.FeedbackInfo; }
			set { ApiRequest.FeedbackInfo = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="CompleteSaleRequestType.Shipped"/> of type <see cref="bool"/>.
		/// </summary>
		public bool Shipped
		{ 
			get { return ApiRequest.Shipped; }
			set { ApiRequest.Shipped = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="CompleteSaleRequestType.Paid"/> of type <see cref="bool"/>.
		/// </summary>
		public bool Paid
		{ 
			get { return ApiRequest.Paid; }
			set { ApiRequest.Paid = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="CompleteSaleRequestType.ListingType"/> of type <see cref="ListingTypeCodeType"/>.
		/// </summary>
		public ListingTypeCodeType ListingType
		{ 
			get { return ApiRequest.ListingType; }
			set { ApiRequest.ListingType = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="CompleteSaleRequestType.Shipment"/> of type <see cref="ShipmentType"/>.
		/// </summary>
		public ShipmentType Shipment
		{ 
			get { return ApiRequest.Shipment; }
			set { ApiRequest.Shipment = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="CompleteSaleRequestType.OrderID"/> of type <see cref="string"/>.
		/// </summary>
		public string OrderID
		{ 
			get { return ApiRequest.OrderID; }
			set { ApiRequest.OrderID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="CompleteSaleRequestType.OrderLineItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string OrderLineItemID
		{ 
			get { return ApiRequest.OrderLineItemID; }
			set { ApiRequest.OrderLineItemID = value; }
		}
		
		

		#endregion

		
	}
}
