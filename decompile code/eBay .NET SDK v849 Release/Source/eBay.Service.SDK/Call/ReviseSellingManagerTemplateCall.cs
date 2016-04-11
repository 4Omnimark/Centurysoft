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
	public class ReviseSellingManagerTemplateCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public ReviseSellingManagerTemplateCall()
		{
			ApiRequest = new ReviseSellingManagerTemplateRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public ReviseSellingManagerTemplateCall(ApiContext ApiContext)
		{
			ApiRequest = new ReviseSellingManagerTemplateRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Revises a Selling Manager template.
		/// This call is subject to change without notice; the
		/// deprecation process is inapplicable to this call.
		/// </summary>
		/// 
		/// <param name="SaleTemplateID">
		/// The ID of the sale template. You can obtain a
		/// SaleTemplateID by calling GetSellingManagerInventory or AddSellingManagerTemplate.
		/// </param>
		///
		/// <param name="ProductID">
		/// Reserved for future use.
		/// </param>
		///
		/// <param name="SaleTemplateName">
		/// The name of the sale template.
		/// </param>
		///
		/// <param name="Item">
		/// Required. In Item.ItemID, specify the same value as the
		/// value you specified in SaleTemplateID.
		/// Other child elements hold the values for properties that are being changed.
		/// Set values in the Item object only for those properties that are
		/// changing. Use DeletedField to remove a property.
		/// </param>
		///
		/// <param name="DeletedFieldList">
		/// Specifies the name of a field to remove from a template.
		/// See the eBay Features Guide for rules on removing values when revising items.
		/// Also see the relevant field descriptions to determine when to use DeletedField (and potential consequences).
		/// The request can contain zero, one, or many instances of DeletedField (one for each field to be removed).
		/// DeletedField accepts the following path names, which remove the corresponding fields:
		/// 
		/// Item.ApplicationData
		/// Item.AttributeSetArray
		/// Item.ConditionID
		/// Item.ItemSpecifics
		/// Item.ListingCheckoutRedirectPreference.ProStoresStoreName
		/// Item.ListingCheckoutRedirectPreference.SellerThirdPartyUsername
		/// Item.ListingDesigner.LayoutID
		/// Item.ListingDesigner.ThemeID
		/// Item.ListingEnhancement[Value]
		/// Item.PayPalEmailAddress
		/// Item.PictureDetails.GalleryURL
		/// Item.PictureDetails.PictureURL
		/// Item.PostalCode
		/// Item.ProductListingDetails
		/// item.ShippingDetails.PaymentInstructions
		/// item.SKU
		/// Item.SubTitle
		/// These values are case-sensitive. Use values that match the case of the schema element names
		/// (Item.PictureDetails.GalleryURL) or make the initial letter of each field name lowercase (item.pictureDetails.galleryURL).
		/// However, do not change the case of letters in the middle of a field name (e.g., item.picturedetails.galleryUrl is not allowed).
		/// 
		/// Depending on how you have configured your pictures, you cannot necessarily delete
		/// both GalleryURL and PictureURL from an existing listing.
		/// If GalleryType was already set for the item you are revising, you cannot remove it.
		/// This means you still need to include either a first picture
		/// or a gallery URL in your revised listing.
		/// </param>
		///
		/// <param name="VerifyOnly">
		/// Use this field to verify the template instead of revising it.
		/// </param>
		///
		public long ReviseSellingManagerTemplate(long SaleTemplateID, long ProductID, string SaleTemplateName, ItemType Item, StringCollection DeletedFieldList, bool VerifyOnly)
		{
			this.SaleTemplateID = SaleTemplateID;
			this.ProductID = ProductID;
			this.SaleTemplateName = SaleTemplateName;
			this.Item = Item;
			this.DeletedFieldList = DeletedFieldList;
			this.VerifyOnly = VerifyOnly;

			Execute();
			return ApiResponse.SaleTemplateID;
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
		/// Gets or sets the <see cref="ReviseSellingManagerTemplateRequestType"/> for this API call.
		/// </summary>
		public ReviseSellingManagerTemplateRequestType ApiRequest
		{ 
			get { return (ReviseSellingManagerTemplateRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="ReviseSellingManagerTemplateResponseType"/> for this API call.
		/// </summary>
		public ReviseSellingManagerTemplateResponseType ApiResponse
		{ 
			get { return (ReviseSellingManagerTemplateResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseSellingManagerTemplateRequestType.SaleTemplateID"/> of type <see cref="long"/>.
		/// </summary>
		public long SaleTemplateID
		{ 
			get { return ApiRequest.SaleTemplateID; }
			set { ApiRequest.SaleTemplateID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseSellingManagerTemplateRequestType.ProductID"/> of type <see cref="long"/>.
		/// </summary>
		public long ProductID
		{ 
			get { return ApiRequest.ProductID; }
			set { ApiRequest.ProductID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseSellingManagerTemplateRequestType.SaleTemplateName"/> of type <see cref="string"/>.
		/// </summary>
		public string SaleTemplateName
		{ 
			get { return ApiRequest.SaleTemplateName; }
			set { ApiRequest.SaleTemplateName = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseSellingManagerTemplateRequestType.Item"/> of type <see cref="ItemType"/>.
		/// </summary>
		public ItemType Item
		{ 
			get { return ApiRequest.Item; }
			set { ApiRequest.Item = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseSellingManagerTemplateRequestType.DeletedField"/> of type <see cref="StringCollection"/>.
		/// </summary>
		public StringCollection DeletedFieldList
		{ 
			get { return ApiRequest.DeletedField; }
			set { ApiRequest.DeletedField = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseSellingManagerTemplateRequestType.VerifyOnly"/> of type <see cref="bool"/>.
		/// </summary>
		public bool VerifyOnly
		{ 
			get { return ApiRequest.VerifyOnly; }
			set { ApiRequest.VerifyOnly = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="ReviseSellingManagerTemplateResponseType.SaleTemplateID"/> of type <see cref="long"/>.
		/// </summary>
		public long SaleTemplateIDReturn
		{ 
			get { return ApiResponse.SaleTemplateID; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="ReviseSellingManagerTemplateResponseType.Fees"/> of type <see cref="FeeTypeCollection"/>.
		/// </summary>
		public FeeTypeCollection FeeList
		{ 
			get { return ApiResponse.Fees; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="ReviseSellingManagerTemplateResponseType.CategoryID"/> of type <see cref="string"/>.
		/// </summary>
		public string CategoryID
		{ 
			get { return ApiResponse.CategoryID; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="ReviseSellingManagerTemplateResponseType.Category2ID"/> of type <see cref="string"/>.
		/// </summary>
		public string Category2ID
		{ 
			get { return ApiResponse.Category2ID; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="ReviseSellingManagerTemplateResponseType.VerifyOnly"/> of type <see cref="bool"/>.
		/// </summary>
		public bool VerifyOnlyReturn
		{ 
			get { return ApiResponse.VerifyOnly; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="ReviseSellingManagerTemplateResponseType.SaleTemplateName"/> of type <see cref="string"/>.
		/// </summary>
		public string SaleTemplateNameReturn
		{ 
			get { return ApiResponse.SaleTemplateName; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="ReviseSellingManagerTemplateResponseType.SellingManagerProductDetails"/> of type <see cref="SellingManagerProductDetailsType"/>.
		/// </summary>
		public SellingManagerProductDetailsType SellingManagerProductDetails
		{ 
			get { return ApiResponse.SellingManagerProductDetails; }
		}
		

		#endregion

		
	}
}
