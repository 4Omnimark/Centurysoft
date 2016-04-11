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
	public class GetAllBiddersCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetAllBiddersCall()
		{
			ApiRequest = new GetAllBiddersRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetAllBiddersCall(ApiContext ApiContext)
		{
			ApiRequest = new GetAllBiddersRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Provides three modes for retrieving a list of the users that bid on
		/// a listing.
		/// </summary>
		/// 
		/// <param name="ItemID">
		/// The ID of the item. The bidders who bid on this item are returned.
		/// </param>
		///
		/// <param name="CallMode">
		/// Specifies which bidder information to return.
		/// </param>
		///
		/// <param name="IncludeBiddingSummary">
		/// Specifies whether return BiddingSummary container for each offer.
		/// </param>
		///
		public OfferTypeCollection GetAllBidders(string ItemID, GetAllBiddersModeCodeType CallMode, bool IncludeBiddingSummary)
		{
			this.ItemID = ItemID;
			this.CallMode = CallMode;
			this.IncludeBiddingSummary = IncludeBiddingSummary;

			Execute();
			return ApiResponse.BidArray;
		}


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public OfferTypeCollection GetAllBidders(string ItemID, GetAllBiddersModeCodeType CallMode)
		{
			this.ItemID = ItemID;
			this.CallMode = CallMode;

			Execute();
			return ApiResponse.BidArray;
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
		/// Gets or sets the <see cref="GetAllBiddersRequestType"/> for this API call.
		/// </summary>
		public GetAllBiddersRequestType ApiRequest
		{ 
			get { return (GetAllBiddersRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetAllBiddersResponseType"/> for this API call.
		/// </summary>
		public GetAllBiddersResponseType ApiResponse
		{ 
			get { return (GetAllBiddersResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetAllBiddersRequestType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemID
		{ 
			get { return ApiRequest.ItemID; }
			set { ApiRequest.ItemID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetAllBiddersRequestType.CallMode"/> of type <see cref="GetAllBiddersModeCodeType"/>.
		/// </summary>
		public GetAllBiddersModeCodeType CallMode
		{ 
			get { return ApiRequest.CallMode; }
			set { ApiRequest.CallMode = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetAllBiddersRequestType.IncludeBiddingSummary"/> of type <see cref="bool"/>.
		/// </summary>
		public bool IncludeBiddingSummary
		{ 
			get { return ApiRequest.IncludeBiddingSummary; }
			set { ApiRequest.IncludeBiddingSummary = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetAllBiddersResponseType.BidArray"/> of type <see cref="OfferTypeCollection"/>.
		/// </summary>
		public OfferTypeCollection BidList
		{ 
			get { return ApiResponse.BidArray; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetAllBiddersResponseType.HighBidder"/> of type <see cref="string"/>.
		/// </summary>
		public string HighBidder
		{ 
			get { return ApiResponse.HighBidder; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetAllBiddersResponseType.HighestBid"/> of type <see cref="AmountType"/>.
		/// </summary>
		public AmountType HighestBid
		{ 
			get { return ApiResponse.HighestBid; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetAllBiddersResponseType.ListingStatus"/> of type <see cref="ListingStatusCodeType"/>.
		/// </summary>
		public ListingStatusCodeType ListingStatus
		{ 
			get { return ApiResponse.ListingStatus; }
		}
		

		#endregion

		
	}
}
