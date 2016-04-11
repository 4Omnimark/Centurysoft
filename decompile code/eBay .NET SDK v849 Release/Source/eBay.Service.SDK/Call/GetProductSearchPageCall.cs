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
	public class GetProductSearchPageCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetProductSearchPageCall()
		{
			ApiRequest = new GetProductSearchPageRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetProductSearchPageCall(ApiContext ApiContext)
		{
			ApiRequest = new GetProductSearchPageRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// This type is deprecated as the call is no longer available.
		/// </summary>
		/// 
		/// <param name="AttributeVersion">
		/// A version of the search page definitions for the site. Typically, an
		/// application passes the version value that was returned the last time the
		/// application executed this call. Filter that causes the call to return only
		/// the search pages for which the attribute meta-data has changed since the
		/// specified version. The latest version value is not necessarily greater
		/// than the previous value that was returned. Therefore, when comparing
		/// versions, only compare whether the value has changed.
		/// </param>
		///
		/// <param name="AttributeSetIDList">
		/// A characteristic set ID that is associated with a
		/// catalog-enabled category that supports product search pages.
		/// You can pass an array of these IDs in the request.
		/// Each characteristic set corresponds to a level in the
		/// eBay category hierarchy at which all items share common characteristics.
		/// Multiple categories can be mapped to the same characteristic set.
		/// Each ID is used as a filter to limit the response content to fewer
		/// characteristic sets. When IDs are specified, the call only returns
		/// search page data for the corresponding characteristic sets.
		/// When no IDs are specified, the call returns all the current
		/// search page data in the system.
		/// </param>
		///
		public ProductSearchPageTypeCollection GetProductSearchPage(string AttributeVersion, Int32Collection AttributeSetIDList)
		{
			this.AttributeVersion = AttributeVersion;
			this.AttributeSetIDList = AttributeSetIDList;

			Execute();
			return ApiResponse.ProductSearchPage;
		}


 		/// <summary>
 		/// Call GetProductSearchPageVersion to retrieve the latest version of the Product Search Page.
 		/// </summary>
 		/// <returns>The <see cref="GetProductSearchPageResponseType.AttributeSystemVersion"/>.</returns>
 		public string GetProductSearchPageVersion()
 		{
 			this.DetailLevelOverride = true;
 			Execute();
 			this.DetailLevelOverride = false;
 			return AttributeSystemVersionReturn;
 		}

		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public ProductSearchPageTypeCollection GetProductSearchPage()
		{
			Execute();
			return ProductSearchPageList;
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
		/// Gets or sets the <see cref="GetProductSearchPageRequestType"/> for this API call.
		/// </summary>
		public GetProductSearchPageRequestType ApiRequest
		{ 
			get { return (GetProductSearchPageRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetProductSearchPageResponseType"/> for this API call.
		/// </summary>
		public GetProductSearchPageResponseType ApiResponse
		{ 
			get { return (GetProductSearchPageResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetProductSearchPageRequestType.AttributeSystemVersion"/> of type <see cref="string"/>.
		/// </summary>
		public string AttributeVersion
		{ 
			get { return ApiRequest.AttributeSystemVersion; }
			set { ApiRequest.AttributeSystemVersion = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetProductSearchPageRequestType.AttributeSetID"/> of type <see cref="Int32Collection"/>.
		/// </summary>
		public Int32Collection AttributeSetIDList
		{ 
			get { return ApiRequest.AttributeSetID; }
			set { ApiRequest.AttributeSetID = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetProductSearchPageResponseType.ProductSearchPage"/> of type <see cref="ProductSearchPageTypeCollection"/>.
		/// </summary>
		public ProductSearchPageTypeCollection ProductSearchPageList
		{ 
			get { return ApiResponse.ProductSearchPage; }
		}
		
		/// <summary>
		/// 
		/// </summary>

		public string AttributeSystemVersionReturn
		{ 
			get { return ApiResponse.AttributeSystemVersion; }
		}

		#endregion

		
	}
}
