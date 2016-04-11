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
	public class GetProductFinderCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetProductFinderCall()
		{
			ApiRequest = new GetProductFinderRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetProductFinderCall(ApiContext ApiContext)
		{
			ApiRequest = new GetProductFinderRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// This type is deprecated as the call is no longer available.
		/// </summary>
		/// 
		/// <param name="AttributeVersion">
		/// A version of the product finder attribute definitions for the site.
		/// Typically, an application passes the version value that was returned the last
		/// time the application executed this call.
		/// Filter that causes the call to return only the Product Finders
		/// for which the attribute meta-data has changed since the specified version.
		/// The latest version value is not necessarily greater than the previous
		/// value that was returned. Therefore, when comparing versions, only
		/// compare whether the value has changed.
		/// </param>
		///
		/// <param name="ProductFinderIDList">
		/// A number that uniquely identifies a product finder. 
		/// 
		/// For sell-side searches, you can determine the
		/// product finder IDs for a category by calling GetCategory2CS.
		/// 
		/// For buy-side searches, you can't use GetCategory2CS to reliably determine
		/// the product finder IDs for an eBay category.
		/// GetProductFinder may work for some attributes. Alternatively, you can
		/// use the pfid parameter from the URL of the Product Finder search page
		/// on the site you're interested in.
		/// See the Knowledge Base article referenced below for more details.
		/// 
		/// Multiple categories can be mapped to the same product finder ID.
		/// You can pass zero or multiple IDs in the request. When IDs are specified, the call
		/// only returns product finder meta-data for the specified product finders.
		/// When no IDs are specified, the call returns all the current product finder
		/// meta-data available on the site.
		/// </param>
		///
		public string GetProductFinder(string AttributeVersion, Int32Collection ProductFinderIDList)
		{
			this.AttributeVersion = AttributeVersion;
			this.ProductFinderIDList = ProductFinderIDList;

			Execute();
			return ApiResponse.AttributeSystemVersion;
		}


 		/// <summary>
 		/// Call GetProductFinderVersion to get the current version of the product finder.
 		/// </summary>
 		/// <returns>The <see cref="GetProductFinderResponseType.AttributeSystemVersion"/>.</returns>
 		public string GetProductFinderVersion()
 		{
 			this.DetailLevelOverride = true;
 			Execute();
 			this.DetailLevelOverride = false;
 			return AttributeSystemVersionReturn;
 		}

		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public string GetProductFinder()
		{
			Execute();
			return ProductFinderData;
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
		/// Gets or sets the <see cref="GetProductFinderRequestType"/> for this API call.
		/// </summary>
		public GetProductFinderRequestType ApiRequest
		{ 
			get { return (GetProductFinderRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetProductFinderResponseType"/> for this API call.
		/// </summary>
		public GetProductFinderResponseType ApiResponse
		{ 
			get { return (GetProductFinderResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetProductFinderRequestType.AttributeSystemVersion"/> of type <see cref="string"/>.
		/// </summary>
		public string AttributeVersion
		{ 
			get { return ApiRequest.AttributeSystemVersion; }
			set { ApiRequest.AttributeSystemVersion = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetProductFinderRequestType.ProductFinderID"/> of type <see cref="Int32Collection"/>.
		/// </summary>
		public Int32Collection ProductFinderIDList
		{ 
			get { return ApiRequest.ProductFinderID; }
			set { ApiRequest.ProductFinderID = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetProductFinderResponseType.ProductFinderData"/> of type <see cref="string"/>.
		/// </summary>
		public string ProductFinderData
		{ 
			get { return ApiResponse.ProductFinderData; }
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
