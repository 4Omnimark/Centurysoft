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
	public class GetBidderListCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetBidderListCall()
		{
			ApiRequest = new GetBidderListRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetBidderListCall(ApiContext ApiContext)
		{
			ApiRequest = new GetBidderListRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Retrieves all items the user is currently bidding on, and the ones they have won
		/// or purchased.
		/// </summary>
		/// 
		/// <param name="ActiveItemsOnly">
		/// Indicates whether or not to limit the result set to active items. If true, only
		/// active items are returned and the EndTimeFrom and EndTimeTo filters are
		/// ignored. If false (or not sent), both active and ended items are returned.
		/// </param>
		///
		/// <param name="EndTimeFrom">
		/// Used in conjunction with EndTimeTo. Limits returned items to only those for
		/// which the item's end date is on or after the date-time specified. Specify an
		/// end date within 30 days prior to today. Items that ended more than 30 days
		/// ago are omitted from the results. If specified, EndTimeTo must also be
		/// specified. Express date-time in the format YYYY-MM-DD HH:MM:SS, and in GMT.
		/// (For information on how to convert between your local time zone and GMT, see
		/// Time Values Note.) This field is ignored if ActiveItemsOnly is true.
		/// </param>
		///
		/// <param name="EndTimeTo">
		/// Used in conjunction with EndTimeFrom. Limits returned items to only those for
		/// which the item's end date is on or before the date-time specified. If
		/// specified, EndTimeFrom must also be specified. Express date-time in the format
		/// YYYY-MM-DD HH:MM:SS, and in GMT. This field is ignored if ActiveItemsOnly is
		/// true. Note that for GTC items, whose end times automatically increment by 30
		/// days every 30 days, an EndTimeTo within in the first 30 days of a listing will
		/// refer to the listing's initial end time.
		/// </param>
		///
		/// <param name="UserID">
		/// The user for whom information should be returned. If
		/// provided, overrides user defined via RequesterCredentials
		/// in header.
		/// </param>
		///
		/// <param name="GranularityLevel">
		/// You can control some of the fields returned in the response by specifying one
		/// of two values in the GranularityLevel field: Fine or Medium. Fine returns
		/// more fields than the default, while setting this field to Medium returns an
		/// abbreviated set of results.
		/// </param>
		///
		public ItemTypeCollection GetBidderList(bool ActiveItemsOnly, DateTime EndTimeFrom, DateTime EndTimeTo, string UserID, GranularityLevelCodeType GranularityLevel)
		{
			this.ActiveItemsOnly = ActiveItemsOnly;
			this.EndTimeFrom = EndTimeFrom;
			this.EndTimeTo = EndTimeTo;
			this.UserID = UserID;
			this.GranularityLevel = GranularityLevel;

			Execute();
			return ApiResponse.BidItemArray;
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
		/// Gets or sets the <see cref="GetBidderListRequestType"/> for this API call.
		/// </summary>
		public GetBidderListRequestType ApiRequest
		{ 
			get { return (GetBidderListRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetBidderListResponseType"/> for this API call.
		/// </summary>
		public GetBidderListResponseType ApiResponse
		{ 
			get { return (GetBidderListResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetBidderListRequestType.ActiveItemsOnly"/> of type <see cref="bool"/>.
		/// </summary>
		public bool ActiveItemsOnly
		{ 
			get { return ApiRequest.ActiveItemsOnly; }
			set { ApiRequest.ActiveItemsOnly = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetBidderListRequestType.EndTimeFrom"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime EndTimeFrom
		{ 
			get { return ApiRequest.EndTimeFrom; }
			set { ApiRequest.EndTimeFrom = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetBidderListRequestType.EndTimeTo"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime EndTimeTo
		{ 
			get { return ApiRequest.EndTimeTo; }
			set { ApiRequest.EndTimeTo = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetBidderListRequestType.UserID"/> of type <see cref="string"/>.
		/// </summary>
		public string UserID
		{ 
			get { return ApiRequest.UserID; }
			set { ApiRequest.UserID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetBidderListRequestType.GranularityLevel"/> of type <see cref="GranularityLevelCodeType"/>.
		/// </summary>
		public GranularityLevelCodeType GranularityLevel
		{ 
			get { return ApiRequest.GranularityLevel; }
			set { ApiRequest.GranularityLevel = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetBidderListResponseType.Bidder"/> of type <see cref="UserType"/>.
		/// </summary>
		public UserType Bidder
		{ 
			get { return ApiResponse.Bidder; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetBidderListResponseType.BidItemArray"/> of type <see cref="ItemTypeCollection"/>.
		/// </summary>
		public ItemTypeCollection BidItemList
		{ 
			get { return ApiResponse.BidItemArray; }
		}
		

		#endregion

		
	}
}
