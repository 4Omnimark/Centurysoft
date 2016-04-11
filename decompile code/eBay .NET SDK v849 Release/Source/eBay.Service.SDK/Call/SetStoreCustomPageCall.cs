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
	public class SetStoreCustomPageCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public SetStoreCustomPageCall()
		{
			ApiRequest = new SetStoreCustomPageRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public SetStoreCustomPageCall(ApiContext ApiContext)
		{
			ApiRequest = new SetStoreCustomPageRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Creates or updates a custom page on a user's eBay Store.
		/// </summary>
		/// 
		/// <param name="CustomPage">
		/// The custom page being submitted.
		/// </param>
		///
		public StoreCustomPageType SetStoreCustomPage(StoreCustomPageType CustomPage)
		{
			this.CustomPage = CustomPage;

			Execute();
			return ApiResponse.CustomPage;
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
		/// Gets or sets the <see cref="SetStoreCustomPageRequestType"/> for this API call.
		/// </summary>
		public SetStoreCustomPageRequestType ApiRequest
		{ 
			get { return (SetStoreCustomPageRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="SetStoreCustomPageResponseType"/> for this API call.
		/// </summary>
		public SetStoreCustomPageResponseType ApiResponse
		{ 
			get { return (SetStoreCustomPageResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="SetStoreCustomPageRequestType.CustomPage"/> of type <see cref="StoreCustomPageType"/>.
		/// </summary>
		public StoreCustomPageType CustomPage
		{ 
			get { return ApiRequest.CustomPage; }
			set { ApiRequest.CustomPage = value; }
		}
		
		
		/// <summary>
		/// 
		/// </summary>
		public StoreCustomPageType CustomPageResult
		{ 
			get { return ApiResponse.CustomPage; }
		}

		#endregion

		
	}
}
