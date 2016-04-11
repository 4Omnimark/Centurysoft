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
	public class GetClientAlertsAuthTokenCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetClientAlertsAuthTokenCall()
		{
			ApiRequest = new GetClientAlertsAuthTokenRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetClientAlertsAuthTokenCall(ApiContext ApiContext)
		{
			ApiRequest = new GetClientAlertsAuthTokenRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Retrieves a token required for the GetUserAlerts call in the Client Alerts API.
		/// </summary>
		/// 
		public string GetClientAlertsAuthToken()
		{

			Execute();
			return ApiResponse.ClientAlertsAuthToken;
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
		/// Gets or sets the <see cref="GetClientAlertsAuthTokenRequestType"/> for this API call.
		/// </summary>
		public GetClientAlertsAuthTokenRequestType ApiRequest
		{ 
			get { return (GetClientAlertsAuthTokenRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetClientAlertsAuthTokenResponseType"/> for this API call.
		/// </summary>
		public GetClientAlertsAuthTokenResponseType ApiResponse
		{ 
			get { return (GetClientAlertsAuthTokenResponseType) AbstractResponse; }
		}

		
		
 		/// <summary>
		/// Gets the returned <see cref="GetClientAlertsAuthTokenResponseType.ClientAlertsAuthToken"/> of type <see cref="string"/>.
		/// </summary>
		public string ClientAlertsAuthToken
		{ 
			get { return ApiResponse.ClientAlertsAuthToken; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetClientAlertsAuthTokenResponseType.HardExpirationTime"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime HardExpirationTime
		{ 
			get { return ApiResponse.HardExpirationTime; }
		}
		

		#endregion

		
	}
}
