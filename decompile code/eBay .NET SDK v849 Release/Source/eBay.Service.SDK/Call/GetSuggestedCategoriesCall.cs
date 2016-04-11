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
	public class GetSuggestedCategoriesCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetSuggestedCategoriesCall()
		{
			ApiRequest = new GetSuggestedCategoriesRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetSuggestedCategoriesCall(ApiContext ApiContext)
		{
			ApiRequest = new GetSuggestedCategoriesRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Returns a list of up to 10 categories that have the highest percentage of listings
		/// whose titles or descriptions contain the keywords you specify.
		/// </summary>
		/// 
		/// <param name="Query">
		/// Specifies the search string, consisting of one or
		/// more words to search for in the listing title.
		/// The words "and" and "or" are treated like any other
		/// word.
		/// </param>
		///
		public SuggestedCategoryTypeCollection GetSuggestedCategories(string Query)
		{
			this.Query = Query;

			Execute();
			return (ApiResponse.SuggestedCategoryArray==null?null:ApiResponse.SuggestedCategoryArray.SuggestedCategory);
		}


		/// <summary>
		/// Gets the <see cref="SuggestedCategoryArrayType.SuggestedCategory"/> of type <see cref="SuggestedCategoryTypeCollection"/>.
		/// </summary>
		public SuggestedCategoryTypeCollection SuggestedCategoryList
		{ 
			get {
				if (ApiResponse.SuggestedCategoryArray == null)
					return null;
				return ApiResponse.SuggestedCategoryArray.SuggestedCategory; }
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
		/// Gets or sets the <see cref="GetSuggestedCategoriesRequestType"/> for this API call.
		/// </summary>
		public GetSuggestedCategoriesRequestType ApiRequest
		{ 
			get { return (GetSuggestedCategoriesRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetSuggestedCategoriesResponseType"/> for this API call.
		/// </summary>
		public GetSuggestedCategoriesResponseType ApiResponse
		{ 
			get { return (GetSuggestedCategoriesResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetSuggestedCategoriesRequestType.Query"/> of type <see cref="string"/>.
		/// </summary>
		public string Query
		{ 
			get { return ApiRequest.Query; }
			set { ApiRequest.Query = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetSuggestedCategoriesResponseType.SuggestedCategoryArray"/> of type <see cref="SuggestedCategoryArrayType"/>.
		/// </summary>
		public SuggestedCategoryArrayType SuggestedCategoryArray
		{ 
			get { return ApiResponse.SuggestedCategoryArray; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetSuggestedCategoriesResponseType.CategoryCount"/> of type <see cref="int"/>.
		/// </summary>
		public int CategoryCount
		{ 
			get { return ApiResponse.CategoryCount; }
		}
		

		#endregion

		
	}
}
