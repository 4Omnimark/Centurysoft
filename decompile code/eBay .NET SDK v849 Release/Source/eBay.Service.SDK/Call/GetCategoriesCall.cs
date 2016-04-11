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
	public class GetCategoriesCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetCategoriesCall()
		{
			ApiRequest = new GetCategoriesRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetCategoriesCall(ApiContext ApiContext)
		{
			ApiRequest = new GetCategoriesRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Retrieves the latest eBay category hierarchy for a given eBay site.
		/// Information returned for each category includes the category name
		/// and the unique ID for the category (unique within the eBay site for which
		/// categories are retrieved). A category ID is a required input when you list most items.
		/// </summary>
		/// 
		/// <param name="CategorySiteID">
		/// Specifies the eBay site for which to retrieve the category
		/// hierarchy.
		/// Use the numeric site code (e.g., 77 for eBay Germany).
		/// Only necessary if you want to retrieve category data
		/// for a site other than the site from which you are
		/// submitting the request.
		/// 
		/// 
		/// NOTE: If you are using the GetCategories call with eBay Motors, you
		/// must set the Site ID in the Request Header to 0, and then set
		/// the CategorySiteID to 100. These are both required fields when
		/// using GetCategories with eBay Motors.
		/// </param>
		///
		/// <param name="CategoryParent">
		/// Specifies the ID of the highest-level category to return,
		/// along with its subcategories.
		/// If no parent category is specified, all categories are
		/// returned for the specified site. (Please do not pass a value of 0; zero (0) is an invalid value for CategoryParent.)
		/// To determine available category IDs, call GetCategories with
		/// no filters and use a DetailLevel value of ReturnAll.
		/// If you specify multiple parent categories, the hierarchy for
		/// each one is returned.
		/// </param>
		///
		/// <param name="LevelLimit">
		/// Specifies the maximum depth of the category hierarchy to retrieve,
		/// where the top-level categories (meta-categories) are at level 1.
		/// Retrieves all category nodes with a category level less than or
		/// equal to this value.
		/// If not specified, retrieves categories at all applicable levels.
		/// As with all calls, the actual data returned will vary depending
		/// on how you configure other fields in the request
		/// (including DetailLevel).
		/// </param>
		///
		/// <param name="ViewAllNodes">
		/// This flag controls whether all eBay categories (that satisfy input filters) are
		/// returned, or only leaf categories (you can only list items in leaf categories)
		/// are returned. The default value is 'true', so if this field is omitted, all eBay
		/// categories will be returned. If you only want to retrieve leaf categories,
		/// include this flag and set it to 'false'. The actual data returned will vary
		/// depending on how you configure other fields in the request.
		/// </param>
		///
		public CategoryTypeCollection GetCategories(string CategorySiteID, StringCollection CategoryParent, int LevelLimit, bool ViewAllNodes)
		{
			this.CategorySiteID = CategorySiteID;
			this.CategoryParent = CategoryParent;
			this.LevelLimit = LevelLimit;
			this.ViewAllNodes = ViewAllNodes;

			Execute();
			return ApiResponse.CategoryArray;
		}


 		/// <summary>
 		/// Call GetCategoriesVersion to retrieve the Category version for a site.
 		/// </summary>
 		/// <returns>The <see cref="GetCategoriesResponseType.CategoryVersion"/>.</returns>
 		public string GetCategoriesVersion()
 		{
 			this.DetailLevelOverride = true;
 			Execute();
 			this.DetailLevelOverride = false;
 			return CategoryVersionResponse;
 		}	

		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public CategoryTypeCollection GetCategories()
		{
			Execute();
			return CategoryList;
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
		/// Gets or sets the <see cref="GetCategoriesRequestType"/> for this API call.
		/// </summary>
		public GetCategoriesRequestType ApiRequest
		{ 
			get { return (GetCategoriesRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetCategoriesResponseType"/> for this API call.
		/// </summary>
		public GetCategoriesResponseType ApiResponse
		{ 
			get { return (GetCategoriesResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetCategoriesRequestType.CategorySiteID"/> of type <see cref="string"/>.
		/// </summary>
		public string CategorySiteID
		{ 
			get { return ApiRequest.CategorySiteID; }
			set { ApiRequest.CategorySiteID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetCategoriesRequestType.CategoryParent"/> of type <see cref="StringCollection"/>.
		/// </summary>
		public StringCollection CategoryParent
		{ 
			get { return ApiRequest.CategoryParent; }
			set { ApiRequest.CategoryParent = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetCategoriesRequestType.LevelLimit"/> of type <see cref="int"/>.
		/// </summary>
		public int LevelLimit
		{ 
			get { return ApiRequest.LevelLimit; }
			set { ApiRequest.LevelLimit = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetCategoriesRequestType.ViewAllNodes"/> of type <see cref="bool"/>.
		/// </summary>
		public bool ViewAllNodes
		{ 
			get { return ApiRequest.ViewAllNodes; }
			set { ApiRequest.ViewAllNodes = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetCategoriesResponseType.CategoryArray"/> of type <see cref="CategoryTypeCollection"/>.
		/// </summary>
		public CategoryTypeCollection CategoryList
		{ 
			get { return ApiResponse.CategoryArray; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetCategoriesResponseType.CategoryCount"/> of type <see cref="int"/>.
		/// </summary>
		public int CategoryCount
		{ 
			get { return ApiResponse.CategoryCount; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetCategoriesResponseType.UpdateTime"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime UpdateTime
		{ 
			get { return ApiResponse.UpdateTime; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetCategoriesResponseType.CategoryVersion"/> of type <see cref="string"/>.
		/// </summary>
		public string CategoryVersionResponse
		{ 
			get { return ApiResponse.CategoryVersion; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetCategoriesResponseType.ReservePriceAllowed"/> of type <see cref="bool"/>.
		/// </summary>
		public bool ReservePriceAllowed
		{ 
			get { return ApiResponse.ReservePriceAllowed; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetCategoriesResponseType.MinimumReservePrice"/> of type <see cref="double"/>.
		/// </summary>
		public double MinimumReservePrice
		{ 
			get { return ApiResponse.MinimumReservePrice; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetCategoriesResponseType.ReduceReserveAllowed"/> of type <see cref="bool"/>.
		/// </summary>
		public bool ReduceReserveAllowed
		{ 
			get { return ApiResponse.ReduceReserveAllowed; }
		}
		

		#endregion

		
	}
}
