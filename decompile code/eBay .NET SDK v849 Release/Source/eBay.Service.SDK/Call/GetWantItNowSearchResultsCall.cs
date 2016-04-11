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
	public class GetWantItNowSearchResultsCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetWantItNowSearchResultsCall()
		{
			ApiRequest = new GetWantItNowSearchResultsRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetWantItNowSearchResultsCall(ApiContext ApiContext)
		{
			ApiRequest = new GetWantItNowSearchResultsRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Retrieves a list of active Want It Now posts that match specified keywords
		/// and/or a specific category ID. The response contains the following data:
		/// CategoryID, PostID, StartTime, ResponseCount, Site, and Title. To get the post
		/// description (Description), you must use GetWantItNowPost to retrieve individual
		/// posts.
		/// </summary>
		/// 
		/// <param name="CategoryID">
		/// Limits the result set to just those Want It Now posts listed in the
		/// specified category. Defaults to all categories if no category ID is
		/// specified. If the specified category ID does not match an existing
		/// category for the site, an invalid-category error message is returned.
		/// Controls the set of listings to return (not the details to return for each
		/// listing).
		/// You must specify a Query and/or a CategoryID in the request.
		/// </param>
		///
		/// <param name="Query">
		/// Specifies a search string. The search string consists of one or more
		/// keywords to search for in the listing title. Note that the post
		/// description will also be searched if SearchInDescription is enabled.
		/// By default, requests return a list of Want It Now posts that include all
		/// of the keywords specified in the Query. All words used in the query,
		/// including "and," "or," and "the,"  will be treated as keywords. You can,
		/// however, use modifiers and wildcards (e.g., +, -, and *) in the Query
		/// field to create more complex searches. Be careful when using spaces before
		/// or after modifiers and wildcards (+, -, or *), as the spaces can affect
		/// the query logic.
		/// See the eBay Web Services Guide for a list of valid search keyword query
		/// operators and examples.
		/// </param>
		///
		/// <param name="SearchInDescription">
		/// If true, include the description field of Want It Now posts in the keyword search. Want
		/// It Now posts returned are those where specified search keywords appear in
		/// either the description or the title. This is the default behavior if SearchInDescription
		/// is not specified. If false, only the title will be searched. SearchInDescription is an
		/// optional input.
		/// </param>
		///
		/// <param name="SearchWorldwide">
		/// If true, the search applies to all eBay sites. If false, the search is
		/// limited to the site specified in the URL query string when the call is
		/// made.
		/// </param>
		///
		/// <param name="Pagination">
		/// Controls the pagination of the result set. Child elements specify the
		/// maximum number of item listings to return per call and which page of data
		/// to return.
		/// </param>
		///
		public WantItNowPostTypeCollection GetWantItNowSearchResults(string CategoryID, string Query, bool SearchInDescription, bool SearchWorldwide, PaginationType Pagination)
		{
			this.CategoryID = CategoryID;
			this.Query = Query;
			this.SearchInDescription = SearchInDescription;
			this.SearchWorldwide = SearchWorldwide;
			this.Pagination = Pagination;

			Execute();
			return ApiResponse.WantItNowPostArray;
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
		/// Gets or sets the <see cref="GetWantItNowSearchResultsRequestType"/> for this API call.
		/// </summary>
		public GetWantItNowSearchResultsRequestType ApiRequest
		{ 
			get { return (GetWantItNowSearchResultsRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetWantItNowSearchResultsResponseType"/> for this API call.
		/// </summary>
		public GetWantItNowSearchResultsResponseType ApiResponse
		{ 
			get { return (GetWantItNowSearchResultsResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetWantItNowSearchResultsRequestType.CategoryID"/> of type <see cref="string"/>.
		/// </summary>
		public string CategoryID
		{ 
			get { return ApiRequest.CategoryID; }
			set { ApiRequest.CategoryID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetWantItNowSearchResultsRequestType.Query"/> of type <see cref="string"/>.
		/// </summary>
		public string Query
		{ 
			get { return ApiRequest.Query; }
			set { ApiRequest.Query = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetWantItNowSearchResultsRequestType.SearchInDescription"/> of type <see cref="bool"/>.
		/// </summary>
		public bool SearchInDescription
		{ 
			get { return ApiRequest.SearchInDescription; }
			set { ApiRequest.SearchInDescription = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetWantItNowSearchResultsRequestType.SearchWorldwide"/> of type <see cref="bool"/>.
		/// </summary>
		public bool SearchWorldwide
		{ 
			get { return ApiRequest.SearchWorldwide; }
			set { ApiRequest.SearchWorldwide = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetWantItNowSearchResultsRequestType.Pagination"/> of type <see cref="PaginationType"/>.
		/// </summary>
		public PaginationType Pagination
		{ 
			get { return ApiRequest.Pagination; }
			set { ApiRequest.Pagination = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetWantItNowSearchResultsResponseType.WantItNowPostArray"/> of type <see cref="WantItNowPostTypeCollection"/>.
		/// </summary>
		public WantItNowPostTypeCollection WantItNowPostList
		{ 
			get { return ApiResponse.WantItNowPostArray; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetWantItNowSearchResultsResponseType.HasMoreItems"/> of type <see cref="bool"/>.
		/// </summary>
		public bool HasMoreItems
		{ 
			get { return ApiResponse.HasMoreItems; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetWantItNowSearchResultsResponseType.ItemsPerPage"/> of type <see cref="int"/>.
		/// </summary>
		public int ItemsPerPage
		{ 
			get { return ApiResponse.ItemsPerPage; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetWantItNowSearchResultsResponseType.PageNumber"/> of type <see cref="int"/>.
		/// </summary>
		public int PageNumber
		{ 
			get { return ApiResponse.PageNumber; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetWantItNowSearchResultsResponseType.PaginationResult"/> of type <see cref="PaginationResultType"/>.
		/// </summary>
		public PaginationResultType PaginationResult
		{ 
			get { return ApiResponse.PaginationResult; }
		}
		

		#endregion

		
	}
}
