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
	public class GetContextualKeywordsCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetContextualKeywordsCall()
		{
			ApiRequest = new GetContextualKeywordsRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetContextualKeywordsCall(ApiContext ApiContext)
		{
			ApiRequest = new GetContextualKeywordsRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Retrieves top-ranked contextual eBay keywords and categories
		/// for a specified web page.
		/// </summary>
		/// 
		/// <param name="URL">
		/// The URL of the web page from which eBay is to extract keywords.
		/// </param>
		///
		/// <param name="Encoding">
		/// Web page encoding by which the URL is to be handled, such as ISO-8859-1.
		/// </param>
		///
		/// <param name="CategoryIDList">
		/// The ID of the category to which keywords are to be limited.
		/// Zero or more category IDs can be specified.
		/// </param>
		///
		public ContextSearchAssetTypeCollection GetContextualKeywords(string URL, string Encoding, StringCollection CategoryIDList)
		{
			this.URL = URL;
			this.Encoding = Encoding;
			this.CategoryIDList = CategoryIDList;

			Execute();
			return ApiResponse.ContextSearchAsset;
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
		/// Gets or sets the <see cref="GetContextualKeywordsRequestType"/> for this API call.
		/// </summary>
		public GetContextualKeywordsRequestType ApiRequest
		{ 
			get { return (GetContextualKeywordsRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetContextualKeywordsResponseType"/> for this API call.
		/// </summary>
		public GetContextualKeywordsResponseType ApiResponse
		{ 
			get { return (GetContextualKeywordsResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetContextualKeywordsRequestType.URL"/> of type <see cref="string"/>.
		/// </summary>
		public string URL
		{ 
			get { return ApiRequest.URL; }
			set { ApiRequest.URL = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetContextualKeywordsRequestType.Encoding"/> of type <see cref="string"/>.
		/// </summary>
		public string Encoding
		{ 
			get { return ApiRequest.Encoding; }
			set { ApiRequest.Encoding = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetContextualKeywordsRequestType.CategoryID"/> of type <see cref="StringCollection"/>.
		/// </summary>
		public StringCollection CategoryIDList
		{ 
			get { return ApiRequest.CategoryID; }
			set { ApiRequest.CategoryID = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetContextualKeywordsResponseType.ContextSearchAsset"/> of type <see cref="ContextSearchAssetTypeCollection"/>.
		/// </summary>
		public ContextSearchAssetTypeCollection ContextSearchAssetList
		{ 
			get { return ApiResponse.ContextSearchAsset; }
		}
		

		#endregion

		
	}
}
