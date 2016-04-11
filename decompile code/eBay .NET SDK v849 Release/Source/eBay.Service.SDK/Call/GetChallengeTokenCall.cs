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
	public class GetChallengeTokenCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetChallengeTokenCall()
		{
			ApiRequest = new GetChallengeTokenRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetChallengeTokenCall(ApiContext ApiContext)
		{
			ApiRequest = new GetChallengeTokenRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Retrieves a botblock token and URLs for an image or audio clip that the user is to
		/// match.
		/// </summary>
		/// 
		public string GetChallengeToken()
		{

			Execute();
			return ApiResponse.ChallengeToken;
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
		/// Gets or sets the <see cref="GetChallengeTokenRequestType"/> for this API call.
		/// </summary>
		public GetChallengeTokenRequestType ApiRequest
		{ 
			get { return (GetChallengeTokenRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetChallengeTokenResponseType"/> for this API call.
		/// </summary>
		public GetChallengeTokenResponseType ApiResponse
		{ 
			get { return (GetChallengeTokenResponseType) AbstractResponse; }
		}

		
		
 		/// <summary>
		/// Gets the returned <see cref="GetChallengeTokenResponseType.ChallengeToken"/> of type <see cref="string"/>.
		/// </summary>
		public string ChallengeToken
		{ 
			get { return ApiResponse.ChallengeToken; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetChallengeTokenResponseType.ImageChallengeURL"/> of type <see cref="string"/>.
		/// </summary>
		public string ImageChallengeURL
		{ 
			get { return ApiResponse.ImageChallengeURL; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetChallengeTokenResponseType.AudioChallengeURL"/> of type <see cref="string"/>.
		/// </summary>
		public string AudioChallengeURL
		{ 
			get { return ApiResponse.AudioChallengeURL; }
		}
		

		#endregion

		
	}
}
