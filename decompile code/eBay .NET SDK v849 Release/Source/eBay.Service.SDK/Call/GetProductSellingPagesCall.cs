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
	public class GetProductSellingPagesCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetProductSellingPagesCall()
		{
			ApiRequest = new GetProductSellingPagesRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetProductSellingPagesCall(ApiContext ApiContext)
		{
			ApiRequest = new GetProductSellingPagesRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// his type is deprecated as the call is no longer available.
		/// </summary>
		/// 
		/// <param name="UseCase">
		/// Specifies the context in which the call is being executed, which will imply
		/// certain validation rules. Use this property to make sure you retrieve the
		/// appropriate version of product information and attribute meta-data
		/// when you are listing, revising, or relisting an item with Pre-filled Item Information.
		/// </param>
		///
		/// <param name="ProductList">
		/// A catalog product identifies a prototype description
		/// of a well-known type of item, such as a popular book.
		/// As this call supports batch requests, you can pass in an array of products
		/// to retrieve data for several products at the same time.
		/// </param>
		///
		public string GetProductSellingPages(ProductUseCaseCodeType UseCase, ProductTypeCollection ProductList)
		{
			this.UseCase = UseCase;
			this.ProductList = ProductList;

			Execute();
			return ApiResponse.ProductSellingPagesData;
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
		/// Gets or sets the <see cref="GetProductSellingPagesRequestType"/> for this API call.
		/// </summary>
		public GetProductSellingPagesRequestType ApiRequest
		{ 
			get { return (GetProductSellingPagesRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetProductSellingPagesResponseType"/> for this API call.
		/// </summary>
		public GetProductSellingPagesResponseType ApiResponse
		{ 
			get { return (GetProductSellingPagesResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetProductSellingPagesRequestType.UseCase"/> of type <see cref="ProductUseCaseCodeType"/>.
		/// </summary>
		public ProductUseCaseCodeType UseCase
		{ 
			get { return ApiRequest.UseCase; }
			set { ApiRequest.UseCase = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetProductSellingPagesRequestType.Product"/> of type <see cref="ProductTypeCollection"/>.
		/// </summary>
		public ProductTypeCollection ProductList
		{ 
			get { return ApiRequest.Product; }
			set { ApiRequest.Product = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetProductSellingPagesResponseType.ProductSellingPagesData"/> of type <see cref="string"/>.
		/// </summary>
		public string ProductSellingPagesData
		{ 
			get { return ApiResponse.ProductSellingPagesData; }
		}
		

		#endregion

		
	}
}
