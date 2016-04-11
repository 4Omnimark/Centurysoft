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
	public class ConfirmIdentityCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public ConfirmIdentityCall()
		{
			ApiRequest = new ConfirmIdentityRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public ConfirmIdentityCall(ApiContext ApiContext)
		{
			ApiRequest = new ConfirmIdentityRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Returns the ID of a user who has gone through an application's consent flow
		/// process for obtaining an authorization token.
		/// </summary>
		/// 
		/// <param name="SessionID">
		/// A string obtained by making a GetSessionID call, passed in redirect
		/// URL to the eBay signin page.
		/// </param>
		///
		public string ConfirmIdentity(string SessionID)
		{
			this.SessionID = SessionID;

			Execute();
			return ApiResponse.UserID;
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
		/// Gets or sets the <see cref="ConfirmIdentityRequestType"/> for this API call.
		/// </summary>
		public ConfirmIdentityRequestType ApiRequest
		{ 
			get { return (ConfirmIdentityRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="ConfirmIdentityResponseType"/> for this API call.
		/// </summary>
		public ConfirmIdentityResponseType ApiResponse
		{ 
			get { return (ConfirmIdentityResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="ConfirmIdentityRequestType.SessionID"/> of type <see cref="string"/>.
		/// </summary>
		public string SessionID
		{ 
			get { return ApiRequest.SessionID; }
			set { ApiRequest.SessionID = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="ConfirmIdentityResponseType.UserID"/> of type <see cref="string"/>.
		/// </summary>
		public string UserID
		{ 
			get { return ApiResponse.UserID; }
		}
		

		#endregion

		
	}
}
