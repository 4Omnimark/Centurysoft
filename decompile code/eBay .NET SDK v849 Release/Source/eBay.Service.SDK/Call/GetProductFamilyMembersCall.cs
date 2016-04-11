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
	public class GetProductFamilyMembersCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetProductFamilyMembersCall()
		{
			ApiRequest = new GetProductFamilyMembersRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetProductFamilyMembersCall(ApiContext ApiContext)
		{
			ApiRequest = new GetProductFamilyMembersRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// This type is deprecated as the call is no longer available.
		/// </summary>
		/// 
		/// <param name="ProductSearchList">
		/// Specifies the ID of a product in the family to be retrieved,
		/// along with pagination and sorting instructions.
		/// ProductSearch is a required input.
		/// </param>
		///
		public ProductSearchResultTypeCollection GetProductFamilyMembers(ProductSearchTypeCollection ProductSearchList)
		{
			this.ProductSearchList = ProductSearchList;

			Execute();
			return ApiResponse.ProductSearchResult;
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
		/// Gets or sets the <see cref="GetProductFamilyMembersRequestType"/> for this API call.
		/// </summary>
		public GetProductFamilyMembersRequestType ApiRequest
		{ 
			get { return (GetProductFamilyMembersRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetProductFamilyMembersResponseType"/> for this API call.
		/// </summary>
		public GetProductFamilyMembersResponseType ApiResponse
		{ 
			get { return (GetProductFamilyMembersResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetProductFamilyMembersRequestType.ProductSearch"/> of type <see cref="ProductSearchTypeCollection"/>.
		/// </summary>
		public ProductSearchTypeCollection ProductSearchList
		{ 
			get { return ApiRequest.ProductSearch; }
			set { ApiRequest.ProductSearch = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetProductFamilyMembersResponseType.DataElementSets"/> of type <see cref="DataElementSetTypeCollection"/>.
		/// </summary>
		public DataElementSetTypeCollection DataElementSetList
		{ 
			get { return ApiResponse.DataElementSets; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetProductFamilyMembersResponseType.ProductSearchResult"/> of type <see cref="ProductSearchResultTypeCollection"/>.
		/// </summary>
		public ProductSearchResultTypeCollection ProductSearchResultList
		{ 
			get { return ApiResponse.ProductSearchResult; }
		}
		

		#endregion

		
	}
}
