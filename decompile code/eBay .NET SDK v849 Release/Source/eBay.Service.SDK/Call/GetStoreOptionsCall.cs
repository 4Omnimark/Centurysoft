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
	public class GetStoreOptionsCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetStoreOptionsCall()
		{
			ApiRequest = new GetStoreOptionsRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetStoreOptionsCall(ApiContext ApiContext)
		{
			ApiRequest = new GetStoreOptionsRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Retrieves the current list of eBay store configuration settings.
		/// </summary>
		/// 
		public StoreThemeTypeCollection GetStoreOptions()
		{

			Execute();
			return ApiResponse.BasicThemeArray.Theme;
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
		/// Gets or sets the <see cref="GetStoreOptionsRequestType"/> for this API call.
		/// </summary>
		public GetStoreOptionsRequestType ApiRequest
		{ 
			get { return (GetStoreOptionsRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetStoreOptionsResponseType"/> for this API call.
		/// </summary>
		public GetStoreOptionsResponseType ApiResponse
		{ 
			get { return (GetStoreOptionsResponseType) AbstractResponse; }
		}

		
		
 		/// <summary>
		/// Gets the returned <see cref="GetStoreOptionsResponseType.BasicThemeArray"/> of type <see cref="StoreThemeArrayType"/>.
		/// </summary>
		public StoreThemeArrayType BasicThemeArray
		{ 
			get { return ApiResponse.BasicThemeArray; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetStoreOptionsResponseType.AdvancedThemeArray"/> of type <see cref="StoreThemeArrayType"/>.
		/// </summary>
		public StoreThemeArrayType AdvancedThemeArray
		{ 
			get { return ApiResponse.AdvancedThemeArray; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetStoreOptionsResponseType.LogoArray"/> of type <see cref="StoreLogoTypeCollection"/>.
		/// </summary>
		public StoreLogoTypeCollection LogoList
		{ 
			get { return ApiResponse.LogoArray; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetStoreOptionsResponseType.SubscriptionArray"/> of type <see cref="StoreSubscriptionTypeCollection"/>.
		/// </summary>
		public StoreSubscriptionTypeCollection SubscriptionList
		{ 
			get { return ApiResponse.SubscriptionArray; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetStoreOptionsResponseType.MaxCategories"/> of type <see cref="int"/>.
		/// </summary>
		public int MaxCategories
		{ 
			get { return ApiResponse.MaxCategories; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetStoreOptionsResponseType.MaxCategoryLevels"/> of type <see cref="int"/>.
		/// </summary>
		public int MaxCategoryLevels
		{ 
			get { return ApiResponse.MaxCategoryLevels; }
		}
		

		#endregion

		
	}
}
