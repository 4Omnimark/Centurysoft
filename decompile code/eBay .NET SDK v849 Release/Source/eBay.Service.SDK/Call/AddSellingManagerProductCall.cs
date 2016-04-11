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
	public class AddSellingManagerProductCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public AddSellingManagerProductCall()
		{
			ApiRequest = new AddSellingManagerProductRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public AddSellingManagerProductCall(ApiContext ApiContext)
		{
			ApiRequest = new AddSellingManagerProductRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Creates a Selling Manager product. Sellers use Selling Manager products to store SYI forms for use
		/// as listing templates.
		/// </summary>
		/// 
		/// <param name="SellingManagerProductDetails">
		/// Contains product information that the seller has recorded, such as a product
		/// description and inventory and restocking details.
		/// </param>
		///
		/// <param name="FolderID">
		/// Unique identifier of the folder. This ID is created when the folder is added and is returned by the
		/// <b>AddSellingManagerInventoryFolder</b> call.
		/// </param>
		///
		/// <param name="SellingManagerProductSpecifics">
		/// Specifies an eBay category associated with the product,
		/// defines Item Specifics that are relevant to the product,
		/// and defines variations available for the product
		/// (which may be used to create mult-variation listings).
		/// </param>
		///
		public SellingManagerProductDetailsType AddSellingManagerProduct(SellingManagerProductDetailsType SellingManagerProductDetails, long FolderID, SellingManagerProductSpecificsType SellingManagerProductSpecifics)
		{
			this.SellingManagerProductDetails = SellingManagerProductDetails;
			this.FolderID = FolderID;
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
		/// Gets or sets the <see cref="AddSellingManagerProductRequestType"/> for this API call.
		/// </summary>
		public AddSellingManagerProductRequestType ApiRequest
		{ 
			get { return (AddSellingManagerProductRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="AddSellingManagerProductResponseType"/> for this API call.
		/// </summary>
		public AddSellingManagerProductResponseType ApiResponse
		{ 
			get { return (AddSellingManagerProductResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="AddSellingManagerProductRequestType.SellingManagerProductDetails"/> of type <see cref="SellingManagerProductDetailsType"/>.
		/// </summary>
		public SellingManagerProductDetailsType SellingManagerProductDetails
		{ 
			get { return ApiRequest.SellingManagerProductDetails; }
			set { ApiRequest.SellingManagerProductDetails = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="AddSellingManagerProductRequestType.FolderID"/> of type <see cref="long"/>.
		/// </summary>
		public long FolderID
		{ 
			get { return ApiRequest.FolderID; }
			set { ApiRequest.FolderID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="AddSellingManagerProductRequestType.SellingManagerProductSpecifics"/> of type <see cref="SellingManagerProductSpecificsType"/>.
		/// </summary>
		public SellingManagerProductSpecificsType SellingManagerProductSpecifics
		{ 
			get { return ApiRequest.SellingManagerProductSpecifics; }
			set { ApiRequest.SellingManagerProductSpecifics = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="AddSellingManagerProductResponseType.SellingManagerProductDetails"/> of type <see cref="SellingManagerProductDetailsType"/>.
		/// </summary>
		public SellingManagerProductDetailsType SellingManagerProductDetailsReturn
		{ 
			get { return ApiResponse.SellingManagerProductDetails; }
		}
		

		#endregion

		
	}
}
