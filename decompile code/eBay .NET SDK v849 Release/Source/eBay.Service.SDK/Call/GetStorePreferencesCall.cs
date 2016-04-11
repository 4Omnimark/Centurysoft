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
	public class GetStorePreferencesCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetStorePreferencesCall()
		{
			ApiRequest = new GetStorePreferencesRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetStorePreferencesCall(ApiContext ApiContext)
		{
			ApiRequest = new GetStorePreferencesRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Retrieves a user's Store preferences.
		/// </summary>
		/// 
		public StorePreferencesType GetStorePreferences()
		{

			Execute();
			return ApiResponse.StorePreferences;
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
		/// Gets or sets the <see cref="GetStorePreferencesRequestType"/> for this API call.
		/// </summary>
		public GetStorePreferencesRequestType ApiRequest
		{ 
			get { return (GetStorePreferencesRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetStorePreferencesResponseType"/> for this API call.
		/// </summary>
		public GetStorePreferencesResponseType ApiResponse
		{ 
			get { return (GetStorePreferencesResponseType) AbstractResponse; }
		}

		
		
 		/// <summary>
		/// Gets the returned <see cref="GetStorePreferencesResponseType.StorePreferences"/> of type <see cref="StorePreferencesType"/>.
		/// </summary>
		public StorePreferencesType StorePreferences
		{ 
			get { return ApiResponse.StorePreferences; }
		}
		

		#endregion

		
	}
}
