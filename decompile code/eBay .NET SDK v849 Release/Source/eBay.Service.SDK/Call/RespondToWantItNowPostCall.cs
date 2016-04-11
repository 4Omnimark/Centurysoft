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
	public class RespondToWantItNowPostCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public RespondToWantItNowPostCall()
		{
			ApiRequest = new RespondToWantItNowPostRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public RespondToWantItNowPostCall(ApiContext ApiContext)
		{
			ApiRequest = new RespondToWantItNowPostRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Enables a seller to respond to a Want It Now post with an item listed on the eBay
		/// site. Responses appear on the Want It Now post page, with the item title, the
		/// price of the item, the number of bids on the item, and the time left before the
		/// listing ends. If the item has a picture, the picture is also included on the Want
		/// It Now post page.
		/// </summary>
		/// 
		/// <param name="ItemID">
		/// The unique identifier of an item listed on the eBay site.
		/// </param>
		///
		/// <param name="PostID">
		/// The unique identifier of a Want It Now post on the eBay site.
		/// </param>
		///
		public void RespondToWantItNowPost(string ItemID, string PostID)
		{
			this.ItemID = ItemID;
			this.PostID = PostID;

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
		/// Gets or sets the <see cref="RespondToWantItNowPostRequestType"/> for this API call.
		/// </summary>
		public RespondToWantItNowPostRequestType ApiRequest
		{ 
			get { return (RespondToWantItNowPostRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="RespondToWantItNowPostResponseType"/> for this API call.
		/// </summary>
		public RespondToWantItNowPostResponseType ApiResponse
		{ 
			get { return (RespondToWantItNowPostResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="RespondToWantItNowPostRequestType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemID
		{ 
			get { return ApiRequest.ItemID; }
			set { ApiRequest.ItemID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="RespondToWantItNowPostRequestType.PostID"/> of type <see cref="string"/>.
		/// </summary>
		public string PostID
		{ 
			get { return ApiRequest.PostID; }
			set { ApiRequest.PostID = value; }
		}
		
		

		#endregion

		
	}
}
