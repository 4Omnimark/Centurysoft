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
	public class DeleteSellingManagerProductCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public DeleteSellingManagerProductCall()
		{
			ApiRequest = new DeleteSellingManagerProductRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public DeleteSellingManagerProductCall(ApiContext ApiContext)
		{
			ApiRequest = new DeleteSellingManagerProductRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Deletes a Selling Manager product.
		/// This call is subject to change without notice; the
		/// deprecation process is inapplicable to this call.
		/// </summary>
		/// 
		/// <param name="ProductID">
		/// ID of the product to be deleted.
		/// </param>
		///
		public SellingManagerProductDetailsType DeleteSellingManagerProduct(long ProductID)
		{
			this.ProductID = ProductID;

			Execute();
			return ApiResponse.DeletedSellingManagerProductDetails;
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
		/// Gets or sets the <see cref="DeleteSellingManagerProductRequestType"/> for this API call.
		/// </summary>
		public DeleteSellingManagerProductRequestType ApiRequest
		{ 
			get { return (DeleteSellingManagerProductRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="DeleteSellingManagerProductResponseType"/> for this API call.
		/// </summary>
		public DeleteSellingManagerProductResponseType ApiResponse
		{ 
			get { return (DeleteSellingManagerProductResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="DeleteSellingManagerProductRequestType.ProductID"/> of type <see cref="long"/>.
		/// </summary>
		public long ProductID
		{ 
			get { return ApiRequest.ProductID; }
			set { ApiRequest.ProductID = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="DeleteSellingManagerProductResponseType.DeletedSellingManagerProductDetails"/> of type <see cref="SellingManagerProductDetailsType"/>.
		/// </summary>
		public SellingManagerProductDetailsType DeletedSellingManagerProductDetails
		{ 
			get { return ApiResponse.DeletedSellingManagerProductDetails; }
		}
		

		#endregion

		
	}
}
