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
	public class AddSellingManagerTemplateCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public AddSellingManagerTemplateCall()
		{
			ApiRequest = new AddSellingManagerTemplateRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public AddSellingManagerTemplateCall(ApiContext ApiContext)
		{
			ApiRequest = new AddSellingManagerTemplateRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Adds a Selling Manager template.
		/// One product can have up to 20 templates associated with it.
		/// </summary>
		/// 
		/// <param name="Item">
		/// Contains the data in your template, i.e. the data needed to
		/// list an item. The item data you specify will be used
		/// when you list items from the template you are adding.
		/// </param>
		///
		/// <param name="SaleTemplateName">
		/// The name you provide for the template. If you don't specify
		/// a name, then <b>Item.Title</b> is used as the name.
		/// </param>
		///
		/// <param name="ProductID">
		/// The ID of the product with which the template will be associated.
		/// Before calling <b>AddSellingManagerTemplate</b>, you can obtain a product ID
		/// from the response of an <b>AddSellingManagerProduct</b> call.
		/// That is, after you add a product using <b>AddSellingManagerProduct</b>, a product ID is
		/// returned in the <b>SellingManagerProductDetails.ProductID</b> field.
		/// The <b>GetSellingManagerTemplates</b>
		/// call also returns product IDs.
		/// Alternatively, you can obtain information about a user's existing
		/// products, including the product IDs, by calling <b>GetSellingManagerInventory</b>.
		/// </param>
		///
		public long AddSellingManagerTemplate(ItemType Item, string SaleTemplateName, long ProductID)
		{
			this.Item = Item;
			this.SaleTemplateName = SaleTemplateName;
			this.ProductID = ProductID;

			Execute();
			return ApiResponse.CategoryID;
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
		/// Gets or sets the <see cref="AddSellingManagerTemplateRequestType"/> for this API call.
		/// </summary>
		public AddSellingManagerTemplateRequestType ApiRequest
		{ 
			get { return (AddSellingManagerTemplateRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="AddSellingManagerTemplateResponseType"/> for this API call.
		/// </summary>
		public AddSellingManagerTemplateResponseType ApiResponse
		{ 
			get { return (AddSellingManagerTemplateResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="AddSellingManagerTemplateRequestType.Item"/> of type <see cref="ItemType"/>.
		/// </summary>
		public ItemType Item
		{ 
			get { return ApiRequest.Item; }
			set { ApiRequest.Item = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="AddSellingManagerTemplateRequestType.SaleTemplateName"/> of type <see cref="string"/>.
		/// </summary>
		public string SaleTemplateName
		{ 
			get { return ApiRequest.SaleTemplateName; }
			set { ApiRequest.SaleTemplateName = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="AddSellingManagerTemplateRequestType.ProductID"/> of type <see cref="long"/>.
		/// </summary>
		public long ProductID
		{ 
			get { return ApiRequest.ProductID; }
			set { ApiRequest.ProductID = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="AddSellingManagerTemplateResponseType.CategoryID"/> of type <see cref="long"/>.
		/// </summary>
		public long CategoryID
		{ 
			get { return ApiResponse.CategoryID; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="AddSellingManagerTemplateResponseType.Category2ID"/> of type <see cref="long"/>.
		/// </summary>
		public long Category2ID
		{ 
			get { return ApiResponse.Category2ID; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="AddSellingManagerTemplateResponseType.SaleTemplateID"/> of type <see cref="long"/>.
		/// </summary>
		public long SaleTemplateID
		{ 
			get { return ApiResponse.SaleTemplateID; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="AddSellingManagerTemplateResponseType.SaleTemplateGroupID"/> of type <see cref="long"/>.
		/// </summary>
		public long SaleTemplateGroupID
		{ 
			get { return ApiResponse.SaleTemplateGroupID; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="AddSellingManagerTemplateResponseType.SaleTemplateName"/> of type <see cref="string"/>.
		/// </summary>
		public string SaleTemplateNameReturn
		{ 
			get { return ApiResponse.SaleTemplateName; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="AddSellingManagerTemplateResponseType.SellingManagerProductDetails"/> of type <see cref="SellingManagerProductDetailsType"/>.
		/// </summary>
		public SellingManagerProductDetailsType SellingManagerProductDetails
		{ 
			get { return ApiResponse.SellingManagerProductDetails; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="AddSellingManagerTemplateResponseType.Fees"/> of type <see cref="FeeTypeCollection"/>.
		/// </summary>
		public FeeTypeCollection FeeList
		{ 
			get { return ApiResponse.Fees; }
		}
		

		#endregion

		
	}
}
