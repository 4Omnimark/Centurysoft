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
	public class RespondToBestOfferCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public RespondToBestOfferCall()
		{
			ApiRequest = new RespondToBestOfferRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public RespondToBestOfferCall(ApiContext ApiContext)
		{
			ApiRequest = new RespondToBestOfferRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Enables the seller of a Best Offer item to accept, decline, or counter offers
		/// made by bidders. Best offers can be declined in bulk, using the same message
		/// from the seller to the bidders of all rejected offers.
		/// </summary>
		/// 
		/// <param name="ItemID">
		/// Specifies the item for which the BestOffer data is to be returned.
		/// </param>
		///
		/// <param name="BestOfferIDList">
		/// The ID of a Best Offer for the item.
		/// </param>
		///
		/// <param name="Action">
		/// The action taken on the Best Offer by the seller (e.g.,
		/// Accept, Decline, or Counter). Bulk Accept and Bulk
		/// Counter are not supported. That is, you cannot accept or
		/// counter multiple offers in a single call. You can,
		/// however, decline multiple offers in a single call.
		/// </param>
		///
		/// <param name="SellerResponse">
		/// A comment from the seller to the buyer.
		/// </param>
		///
		/// <param name="CounterOfferPrice">
		/// The counter offer price. When Action is set to Counter,
		/// you must specify the amount for the counter offer with
		/// CounterOfferPrice. The value of CounterOfferPrice cannot
		/// exceed the Buy It Now price for a single quantity item.
		/// The value of CounterOfferPrice may exceed the Buy It Now
		/// price if the value for CounterOfferQuantity is greater
		/// than 1.
		/// </param>
		///
		/// <param name="CounterOfferQuantity">
		/// The quantity of items in the counter offer. When Action is set to
		/// Counter you must specify the quantity of items for the
		/// counter offer with CounterOfferQuantity.
		/// </param>
		///
		public BestOfferTypeCollection RespondToBestOffer(string ItemID, StringCollection BestOfferIDList, BestOfferActionCodeType Action, string SellerResponse, AmountType CounterOfferPrice, int CounterOfferQuantity)
		{
			this.ItemID = ItemID;
			this.BestOfferIDList = BestOfferIDList;
			this.Action = Action;
			this.SellerResponse = SellerResponse;
			this.CounterOfferPrice = CounterOfferPrice;
			this.CounterOfferQuantity = CounterOfferQuantity;

			Execute();
			return ApiResponse.RespondToBestOffer;
		}


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public BestOfferTypeCollection RespondToBestOffer(string ItemID, StringCollection BestOfferIDList, BestOfferActionCodeType Action, string SellerResponse)
		{
			this.ItemID = ItemID;
			this.BestOfferIDList = BestOfferIDList;
			this.Action = Action;
			this.SellerResponse = SellerResponse;
			Execute();
			return this.RespondToBestOfferList;
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
		/// Gets or sets the <see cref="RespondToBestOfferRequestType"/> for this API call.
		/// </summary>
		public RespondToBestOfferRequestType ApiRequest
		{ 
			get { return (RespondToBestOfferRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="RespondToBestOfferResponseType"/> for this API call.
		/// </summary>
		public RespondToBestOfferResponseType ApiResponse
		{ 
			get { return (RespondToBestOfferResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="RespondToBestOfferRequestType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemID
		{ 
			get { return ApiRequest.ItemID; }
			set { ApiRequest.ItemID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="RespondToBestOfferRequestType.BestOfferID"/> of type <see cref="StringCollection"/>.
		/// </summary>
		public StringCollection BestOfferIDList
		{ 
			get { return ApiRequest.BestOfferID; }
			set { ApiRequest.BestOfferID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="RespondToBestOfferRequestType.Action"/> of type <see cref="BestOfferActionCodeType"/>.
		/// </summary>
		public BestOfferActionCodeType Action
		{ 
			get { return ApiRequest.Action; }
			set { ApiRequest.Action = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="RespondToBestOfferRequestType.SellerResponse"/> of type <see cref="string"/>.
		/// </summary>
		public string SellerResponse
		{ 
			get { return ApiRequest.SellerResponse; }
			set { ApiRequest.SellerResponse = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="RespondToBestOfferRequestType.CounterOfferPrice"/> of type <see cref="AmountType"/>.
		/// </summary>
		public AmountType CounterOfferPrice
		{ 
			get { return ApiRequest.CounterOfferPrice; }
			set { ApiRequest.CounterOfferPrice = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="RespondToBestOfferRequestType.CounterOfferQuantity"/> of type <see cref="int"/>.
		/// </summary>
		public int CounterOfferQuantity
		{ 
			get { return ApiRequest.CounterOfferQuantity; }
			set { ApiRequest.CounterOfferQuantity = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="RespondToBestOfferResponseType.RespondToBestOffer"/> of type <see cref="BestOfferTypeCollection"/>.
		/// </summary>
		public BestOfferTypeCollection RespondToBestOfferList
		{ 
			get { return ApiResponse.RespondToBestOffer; }
		}
		

		#endregion

		
	}
}
