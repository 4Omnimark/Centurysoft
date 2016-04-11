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
	public class ReviseSellingManagerProductCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public ReviseSellingManagerProductCall()
		{
			ApiRequest = new ReviseSellingManagerProductRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public ReviseSellingManagerProductCall(ApiContext ApiContext)
		{
			ApiRequest = new ReviseSellingManagerProductRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Revises a Selling Manager product.
		/// 
		/// This call is subject to change without notice; the
		/// deprecation process is inapplicable to this call.
		/// </summary>
		/// 
		/// <param name="SellingManagerProductDetails">
		/// The details of the product that is being revised.
		/// </param>
		///
		/// <param name="SellingManagerFolderDetails">
		/// The details of the folder for this product.
		/// </param>
		///
		/// <param name="DeletedFieldList">
		/// Specifies the name of a field to remove from a Selling Manager product.
		/// The request can contain zero, one, or many instances of DeletedField (one for each field to be removed).
		/// DeletedField accepts the following path names, which remove the corresponding fields:
		/// SellingManagerProductDetails.CustomLabel
		/// SellingManagerProductDetails.QuantityAvailable
		/// SellingManagerProductDetails.UnitCost
		/// These values are case-sensitive. Use values that match the case of the schema element names.
		/// </param>
		///
		/// <param name="SellingManagerProductSpecifics">
		/// Specifies an eBay category associated with the product,
		/// defines Item Specifics that are relevant to the product,
		/// and defines variations available for the product
		/// (which may be used to create mult-variation listings).
		/// </param>
		///
		public SellingManagerProductDetailsType ReviseSellingManagerProduct(SellingManagerProductDetailsType SellingManagerProductDetails, SellingManagerFolderDetailsType SellingManagerFolderDetails, StringCollection DeletedFieldList, SellingManagerProductSpecificsType SellingManagerProductSpecifics)
		{
			this.SellingManagerProductDetails = SellingManagerProductDetails;
			this.SellingManagerFolderDetails = SellingManagerFolderDetails;
			this.DeletedFieldList = DeletedFieldList;
			this.SellingManagerProductSpecifics = SellingManagerProductSpecifics;

			Execute();
			return ApiResponse.SellingManagerProductDetails;
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
		/// Gets or sets the <see cref="ReviseSellingManagerProductRequestType"/> for this API call.
		/// </summary>
		public ReviseSellingManagerProductRequestType ApiRequest
		{ 
			get { return (ReviseSellingManagerProductRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="ReviseSellingManagerProductResponseType"/> for this API call.
		/// </summary>
		public ReviseSellingManagerProductResponseType ApiResponse
		{ 
			get { return (ReviseSellingManagerProductResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseSellingManagerProductRequestType.SellingManagerProductDetails"/> of type <see cref="SellingManagerProductDetailsType"/>.
		/// </summary>
		public SellingManagerProductDetailsType SellingManagerProductDetails
		{ 
			get { return ApiRequest.SellingManagerProductDetails; }
			set { ApiRequest.SellingManagerProductDetails = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseSellingManagerProductRequestType.SellingManagerFolderDetails"/> of type <see cref="SellingManagerFolderDetailsType"/>.
		/// </summary>
		public SellingManagerFolderDetailsType SellingManagerFolderDetails
		{ 
			get { return ApiRequest.SellingManagerFolderDetails; }
			set { ApiRequest.SellingManagerFolderDetails = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseSellingManagerProductRequestType.DeletedField"/> of type <see cref="StringCollection"/>.
		/// </summary>
		public StringCollection DeletedFieldList
		{ 
			get { return ApiRequest.DeletedField; }
			set { ApiRequest.DeletedField = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseSellingManagerProductRequestType.SellingManagerProductSpecifics"/> of type <see cref="SellingManagerProductSpecificsType"/>.
		/// </summary>
		public SellingManagerProductSpecificsType SellingManagerProductSpecifics
		{ 
			get { return ApiRequest.SellingManagerProductSpecifics; }
			set { ApiRequest.SellingManagerProductSpecifics = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="ReviseSellingManagerProductResponseType.SellingManagerProductDetails"/> of type <see cref="SellingManagerProductDetailsType"/>.
		/// </summary>
		public SellingManagerProductDetailsType SellingManagerProductDetailsReturn
		{ 
			get { return ApiResponse.SellingManagerProductDetails; }
		}
		

		#endregion

		
	}
}
