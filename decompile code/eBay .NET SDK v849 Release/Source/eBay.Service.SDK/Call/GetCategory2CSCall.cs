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
	public class GetCategory2CSCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetCategory2CSCall()
		{
			ApiRequest = new GetCategory2CSRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetCategory2CSCall(ApiContext ApiContext)
		{
			ApiRequest = new GetCategory2CSRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// No longer recommended in general, although this call may still be used to
		/// determine whether a category is catalog-enabled. All other features of this call
		/// are no longer functional.
		/// </summary>
		/// 
		/// <param name="CategoryID">
		/// ID of a category for which to retrieve mappings.
		/// If not specified, the call
		/// retrieves a map for all categories.
		/// </param>
		///
		/// <param name="AttributeSystemVersion">
		/// A version of the mappings for the site.
		/// Typically, an application passes the version value that was returned the last
		/// time the application executed this call.
		/// Filter that causes the call to return only the categories
		/// for which the mappings have changed since the specified version.
		/// If not specified, all category-to-characteristics set mappings are returned.
		/// This value changes each time changes are made to the mappings.
		/// The current version value is not necessarily greater than the previous
		/// value. Therefore, when comparing versions, only compare whether the
		/// value has changed.
		/// </param>
		///
		public CategoryTypeCollection GetCategory2CS(string CategoryID, string AttributeSystemVersion)
		{
			this.CategoryID = CategoryID;
			this.AttributeSystemVersion = AttributeSystemVersion;

			Execute();
			return ApiResponse.MappedCategoryArray;
		}


 		/// <summary>
 		/// Call GetCategory2CSVersion to retrieve the Category2CS version for a site.
 		/// </summary>
 		/// <returns>The <see cref="GetCategory2CSResponseType.AttributeSystemVersion"/>.</returns>
 		public string GetCategory2CSVersion()
 		{
 			this.DetailLevelOverride = true;
 			Execute();
 			this.DetailLevelOverride = false;
 			return AttributeSystemVersionResponse;
 		}	

		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public CategoryTypeCollection GetCategory2CS()
		{
			Execute();
			return MappedCategoryList;
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
		/// Gets or sets the <see cref="GetCategory2CSRequestType"/> for this API call.
		/// </summary>
		public GetCategory2CSRequestType ApiRequest
		{ 
			get { return (GetCategory2CSRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetCategory2CSResponseType"/> for this API call.
		/// </summary>
		public GetCategory2CSResponseType ApiResponse
		{ 
			get { return (GetCategory2CSResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetCategory2CSRequestType.CategoryID"/> of type <see cref="string"/>.
		/// </summary>
		public string CategoryID
		{ 
			get { return ApiRequest.CategoryID; }
			set { ApiRequest.CategoryID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetCategory2CSRequestType.AttributeSystemVersion"/> of type <see cref="string"/>.
		/// </summary>
		public string AttributeSystemVersion
		{ 
			get { return ApiRequest.AttributeSystemVersion; }
			set { ApiRequest.AttributeSystemVersion = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetCategory2CSResponseType.MappedCategoryArray"/> of type <see cref="CategoryTypeCollection"/>.
		/// </summary>
		public CategoryTypeCollection MappedCategoryList
		{ 
			get { return ApiResponse.MappedCategoryArray; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetCategory2CSResponseType.UnmappedCategoryArray"/> of type <see cref="CategoryTypeCollection"/>.
		/// </summary>
		public CategoryTypeCollection UnMappedCategoryList
		{ 
			get { return ApiResponse.UnmappedCategoryArray; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetCategory2CSResponseType.SiteWideCharacteristicSets"/> of type <see cref="SiteWideCharacteristicsTypeCollection"/>.
		/// </summary>
		public SiteWideCharacteristicsTypeCollection SiteWideCharacteristicList
		{ 
			get { return ApiResponse.SiteWideCharacteristicSets; }
		}
		
		/// <summary>
		/// 
		/// </summary>

		public string AttributeSystemVersionResponse
		{ 
			get {return ApiResponse.AttributeSystemVersion; }
		}

		#endregion

		
	}
}
