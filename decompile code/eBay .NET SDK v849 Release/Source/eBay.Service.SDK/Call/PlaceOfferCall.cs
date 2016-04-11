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
	public class PlaceOfferCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public PlaceOfferCall()
		{
			ApiRequest = new PlaceOfferRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public PlaceOfferCall(ApiContext ApiContext)
		{
			ApiRequest = new PlaceOfferRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Enables the authenticated user to to make a bid, a best offer, or
		/// a purchase on the item specified by the ItemID input field.
		/// </summary>
		/// 
		/// <param name="Offer">
		/// Specifies the type of offer being made. If the item is a
		/// competitive-bidding listing, the offer is a bid. If the item is a
		/// fixed-price listing, then the offer purchases the item. If the item is a
		/// competitive-bidding listing and the offer is of type with an active,
		/// unexercised Buy It Now option, then the offer can either purchase the
		/// item or be a bid. See the schema documentation for OfferType for details
		/// on its properties and their meanings.
		/// </param>
		///
		/// <param name="ItemID">
		/// Unique item ID that identifies the item listing for which the action is being
		/// submitted.
		/// 
		/// If the item was listed with Variations, you must also specify
		/// VariationSpecifics in the request to uniquely identify the
		/// variant being purchased.
		/// </param>
		///
		/// <param name="BlockOnWarning">
		/// If a warning message exists and BlockOnWarning is true,
		/// the warning message is returned and the bid is blocked. If no warning message
		/// exists and BlockOnWarning is true, the bid is placed. If BlockOnWarning
		/// is false, the bid is placed, regardless of warning.
		/// </param>
		///
		/// <param name="AffiliateTrackingDetails">
		/// Container for affiliate-related tags, which enable the tracking of user
		/// activity. If you include AffiliateTrackingDetails in your PlaceOffer call, then
		/// it is possible to receive affiliate commissions based on calls made by your
		/// application. (See the <a href=
		/// "http://www.ebaypartnernetwork.com/" target="_blank">eBay Partner Network</a>
		/// for information about commissions.) Please note that affiliate tracking is not
		/// available in the Sandbox environment, and that affiliate tracking is not
		/// available when you make a best offer.
		/// </param>
		///
		/// <param name="VariationSpecificList">
		/// Name-value pairs that identify a single variation within the
		/// listing identified by ItemID. Required when the seller
		/// listed the item with Item Variations.
		/// 
		/// If you want to buy items from multiple variations in the same
		/// listing, use a separate PlaceOffer request for each variation.
		/// For example, if you want to buy 3 red shirts and 2 black shirts
		/// from the same listing, use one PlaceOffer request for the
		/// 3 red shirts, and another PlaceOffer request for the 2
		/// black shirts.
		/// </param>
		///
		public SellingStatusType PlaceOffer(OfferType Offer, string ItemID, bool BlockOnWarning, AffiliateTrackingDetailsType AffiliateTrackingDetails, NameValueListTypeCollection VariationSpecificList)
		{
			this.Offer = Offer;
			this.ItemID = ItemID;
			this.BlockOnWarning = BlockOnWarning;
			this.AffiliateTrackingDetails = AffiliateTrackingDetails;
			this.VariationSpecificList = VariationSpecificList;

			Execute();
			return ApiResponse.SellingStatus;
		}


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public SellingStatusType PlaceOffer(OfferType Offer, string ItemID)
		{
			this.Offer = Offer;
			this.ItemID = ItemID;

			Execute();
			return ApiResponse.SellingStatus;
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
		/// Gets or sets the <see cref="PlaceOfferRequestType"/> for this API call.
		/// </summary>
		public PlaceOfferRequestType ApiRequest
		{ 
			get { return (PlaceOfferRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="PlaceOfferResponseType"/> for this API call.
		/// </summary>
		public PlaceOfferResponseType ApiResponse
		{ 
			get { return (PlaceOfferResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="PlaceOfferRequestType.Offer"/> of type <see cref="OfferType"/>.
		/// </summary>
		public OfferType Offer
		{ 
			get { return ApiRequest.Offer; }
			set { ApiRequest.Offer = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="PlaceOfferRequestType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemID
		{ 
			get { return ApiRequest.ItemID; }
			set { ApiRequest.ItemID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="PlaceOfferRequestType.BlockOnWarning"/> of type <see cref="bool"/>.
		/// </summary>
		public bool BlockOnWarning
		{ 
			get { return ApiRequest.BlockOnWarning; }
			set { ApiRequest.BlockOnWarning = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="PlaceOfferRequestType.AffiliateTrackingDetails"/> of type <see cref="AffiliateTrackingDetailsType"/>.
		/// </summary>
		public AffiliateTrackingDetailsType AffiliateTrackingDetails
		{ 
			get { return ApiRequest.AffiliateTrackingDetails; }
			set { ApiRequest.AffiliateTrackingDetails = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="PlaceOfferRequestType.VariationSpecifics"/> of type <see cref="NameValueListTypeCollection"/>.
		/// </summary>
		public NameValueListTypeCollection VariationSpecificList
		{ 
			get { return ApiRequest.VariationSpecifics; }
			set { ApiRequest.VariationSpecifics = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="PlaceOfferResponseType.SellingStatus"/> of type <see cref="SellingStatusType"/>.
		/// </summary>
		public SellingStatusType SellingStatus
		{ 
			get { return ApiResponse.SellingStatus; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="PlaceOfferResponseType.TransactionID"/> of type <see cref="string"/>.
		/// </summary>
		public string TransactionID
		{ 
			get { return ApiResponse.TransactionID; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="PlaceOfferResponseType.BestOffer"/> of type <see cref="BestOfferType"/>.
		/// </summary>
		public BestOfferType BestOffer
		{ 
			get { return ApiResponse.BestOffer; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="PlaceOfferResponseType.OrderLineItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string OrderLineItemID
		{ 
			get { return ApiResponse.OrderLineItemID; }
		}
		

		#endregion

		
	}
}
