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
	public class GetItemRecommendationsCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetItemRecommendationsCall()
		{
			ApiRequest = new GetItemRecommendationsRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetItemRecommendationsCall(ApiContext ApiContext)
		{
			ApiRequest = new GetItemRecommendationsRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// No longer recommended.
		/// </summary>
		/// 
		/// <param name="GetRecommendationsRequestContainerList">
		/// Specifies the data for a single item and configures the recommendation engines to use
		/// when processing the item.
		/// To retrieve recommendations for multiple items, pass a separate
		/// GetRecommendationsRequestContainer for each item. In this case,
		/// pass a user-defined correlation ID for each item to identify the matching response.
		/// </param>
		///
		public GetRecommendationsResponseContainerTypeCollection GetItemRecommendations(GetRecommendationsRequestContainerTypeCollection GetRecommendationsRequestContainerList)
		{
			this.GetRecommendationsRequestContainerList = GetRecommendationsRequestContainerList;

			Execute();
			return ApiResponse.GetRecommendationsResponseContainer;
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
		/// Gets or sets the <see cref="GetItemRecommendationsRequestType"/> for this API call.
		/// </summary>
		public GetItemRecommendationsRequestType ApiRequest
		{ 
			get { return (GetItemRecommendationsRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetItemRecommendationsResponseType"/> for this API call.
		/// </summary>
		public GetItemRecommendationsResponseType ApiResponse
		{ 
			get { return (GetItemRecommendationsResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetItemRecommendationsRequestType.GetRecommendationsRequestContainer"/> of type <see cref="GetRecommendationsRequestContainerTypeCollection"/>.
		/// </summary>
		public GetRecommendationsRequestContainerTypeCollection GetRecommendationsRequestContainerList
		{ 
			get { return ApiRequest.GetRecommendationsRequestContainer; }
			set { ApiRequest.GetRecommendationsRequestContainer = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetItemRecommendationsResponseType.GetRecommendationsResponseContainer"/> of type <see cref="GetRecommendationsResponseContainerTypeCollection"/>.
		/// </summary>
		public GetRecommendationsResponseContainerTypeCollection GetRecommendationsResponseContainerList
		{ 
			get { return ApiResponse.GetRecommendationsResponseContainer; }
		}
		

		#endregion

		
	}
}
