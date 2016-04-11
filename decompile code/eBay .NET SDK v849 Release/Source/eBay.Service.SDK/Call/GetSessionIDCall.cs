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
	public class GetSessionIDCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetSessionIDCall()
		{
			ApiRequest = new GetSessionIDRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetSessionIDCall(ApiContext ApiContext)
		{
			ApiRequest = new GetSessionIDRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Retrieves a session ID that identifies a user and your application when you make a FetchToken request.
		/// </summary>
		/// 
		/// <param name="RuName">
		/// The runame provided must match the one that will be used for validation
		/// during the creation of a user token.
		/// </param>
		///
		public string GetSessionID(string RuName)
		{
			this.RuName = RuName;

			Execute();
			return ApiResponse.SessionID;
		}


		/// <summary>
		/// Request for a SessionID, which is a unique identifier for authenticating data entry during the process that creates a user token.
		/// </summary>
		/// 
		public string GetSessionID()
		{
			Execute();
			return ApiResponse.SessionID;
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Constructs a security header from values in <see cref="ApiCall.ApiContext"/>.
		/// </summary>
		/// <returns>Security information of type <see cref="CustomSecurityHeaderType"/>.</returns>
		protected override CustomSecurityHeaderType GetSecurityHeader()
		{
			CustomSecurityHeaderType sechdr = new CustomSecurityHeaderType();

			if (ApiContext.ApiCredential.ApiAccount != null)
			{
				sechdr.Credentials = new UserIdPasswordType();
				sechdr.Credentials.AppId = ApiContext.ApiCredential.ApiAccount.Application;
				sechdr.Credentials.DevId = ApiContext.ApiCredential.ApiAccount.Developer;
				sechdr.Credentials.AuthCert = ApiContext.ApiCredential.ApiAccount.Certificate;
			}
			else
			{
			        throw new SdkException("GetSessionID needs Api Account to be called!");			
			}
			
			return (sechdr);
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
		/// Gets or sets the <see cref="GetSessionIDRequestType"/> for this API call.
		/// </summary>
		public GetSessionIDRequestType ApiRequest
		{ 
			get { return (GetSessionIDRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetSessionIDResponseType"/> for this API call.
		/// </summary>
		public GetSessionIDResponseType ApiResponse
		{ 
			get { return (GetSessionIDResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetSessionIDRequestType.RuName"/> of type <see cref="string"/>.
		/// </summary>
		public string RuName
		{ 
			get { return ApiRequest.RuName; }
			set { ApiRequest.RuName = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetSessionIDResponseType.SessionID"/> of type <see cref="string"/>.
		/// </summary>
		public string SessionID
		{ 
			get { return ApiResponse.SessionID; }
		}
		

		#endregion

		
	}
}
