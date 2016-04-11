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
	public class SetStorePreferencesCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public SetStorePreferencesCall()
		{
			ApiRequest = new SetStorePreferencesRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public SetStorePreferencesCall(ApiContext ApiContext)
		{
			ApiRequest = new SetStorePreferencesRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Sets the preferences for a user's eBay Store.
		/// </summary>
		/// 
		/// <param name="StorePreferences">
		/// Specifies the store preferences.
		/// </param>
		///
		public void SetStorePreferences(StorePreferencesType StorePreferences)
		{
			this.StorePreferences = StorePreferences;

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
		/// Gets or sets the <see cref="SetStorePreferencesRequestType"/> for this API call.
		/// </summary>
		public SetStorePreferencesRequestType ApiRequest
		{ 
			get { return (SetStorePreferencesRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="SetStorePreferencesResponseType"/> for this API call.
		/// </summary>
		public SetStorePreferencesResponseType ApiResponse
		{ 
			get { return (SetStorePreferencesResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="SetStorePreferencesRequestType.StorePreferences"/> of type <see cref="StorePreferencesType"/>.
		/// </summary>
		public StorePreferencesType StorePreferences
		{ 
			get { return ApiRequest.StorePreferences; }
			set { ApiRequest.StorePreferences = value; }
		}
		
		

		#endregion

		
	}
}
