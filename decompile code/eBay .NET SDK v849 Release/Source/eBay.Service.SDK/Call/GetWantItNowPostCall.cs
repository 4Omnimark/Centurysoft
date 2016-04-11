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
	public class GetWantItNowPostCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetWantItNowPostCall()
		{
			ApiRequest = new GetWantItNowPostRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetWantItNowPostCall(ApiContext ApiContext)
		{
			ApiRequest = new GetWantItNowPostRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Retrieves data for a specific, active Want It Now post identified by a post ID.
		/// The response includes the following fields: CategoryID, Description, PostID,
		/// Site, StartTime, ResponseCount, and Title. Although GetWantItNowSearchResults
		/// returns most of this information, only GetWantItNowPost returns Description for
		/// a post.
		/// </summary>
		/// 
		/// <param name="PostID">
		/// Specifies the post ID that uniquely identifies the Want It Now post for
		/// which to retrieve the data. PostID is a required input. PostID is unique
		/// across all eBay sites.
		/// </param>
		///
		public WantItNowPostType GetWantItNowPost(string PostID)
		{
			this.PostID = PostID;

			Execute();
			return ApiResponse.WantItNowPost;
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
		/// Gets or sets the <see cref="GetWantItNowPostRequestType"/> for this API call.
		/// </summary>
		public GetWantItNowPostRequestType ApiRequest
		{ 
			get { return (GetWantItNowPostRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetWantItNowPostResponseType"/> for this API call.
		/// </summary>
		public GetWantItNowPostResponseType ApiResponse
		{ 
			get { return (GetWantItNowPostResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetWantItNowPostRequestType.PostID"/> of type <see cref="string"/>.
		/// </summary>
		public string PostID
		{ 
			get { return ApiRequest.PostID; }
			set { ApiRequest.PostID = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetWantItNowPostResponseType.WantItNowPost"/> of type <see cref="WantItNowPostType"/>.
		/// </summary>
		public WantItNowPostType WantItNowPost
		{ 
			get { return ApiResponse.WantItNowPost; }
		}
		

		#endregion

		
	}
}
