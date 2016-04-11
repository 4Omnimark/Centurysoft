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
	public class ReviseFixedPriceItemCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public ReviseFixedPriceItemCall()
		{
			ApiRequest = new ReviseFixedPriceItemRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public ReviseFixedPriceItemCall(ApiContext ApiContext)
		{
			ApiRequest = new ReviseFixedPriceItemRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Enables a seller to change the properties of a currently active
		/// fixed-price listing (including multi-variation listings).
		/// </summary>
		/// 
		/// <param name="Item">
		/// Child elements hold the values for item details that you want to change.
		/// The Item.ItemID must always be set to the ID of the listing being changed.
		/// Only specify Item fields for the details that are changing. Unless
		/// otherwise specified in the field descriptions below, the listing retains
		/// its existing values for fields that you don't pass in the
		/// ReviseFixedPriceItem request. Use DeletedField to remove a field from the
		/// listing.
		/// </param>
		///
		/// <param name="DeletedFieldList">
		/// Specifies the name of a field to delete from a listing. The request can
		/// contain zero, one, or many instances of DeletedField (one for each field
		/// to be deleted). See the relevant field descriptions to determine when to
		/// use DeletedField (and potential consequences).
		/// 
		/// You cannot delete required fields from a listing.
		/// 
		/// Some fields are optional when you first list an item (e.g.,
		/// SecondaryCategory), but once they are set they cannot be deleted when you
		/// revise an item. Some optional fields cannot be deleted if the item has
		/// bids and/or ends within 12 hours. Some optional fields cannot be deleted
		/// if other fields depend on them. See the eBay Trading API guide for rules
		/// on removing values when revising items.
		/// 
		/// Some data (such as Variation nodes within Variations) can't be deleted by
		/// using DeletedFields. See the relevant field descriptions for how to delete
		/// such data.
		/// 
		/// DeletedField accepts the following path names, which delete the
		/// corresponding nodes:
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
		/// Item.SellerContactDetails
		/// Item.SellerContactDetails.CompanyName
		/// Item.SellerContactDetails.County
		/// Item.SellerContactDetails.InternationalStreet
		/// Item.SellerContactDetails.Phone2AreaOrCityCode
		/// Item.SellerContactDetails.Phone2CountryCode
		/// Item.SellerContactDetails.Phone2CountryPrefix
		/// Item.SellerContactDetails.Phone2LocalNumber
		/// Item.SellerContactDetails.PhoneAreaOrCityCode
		/// Item.SellerContactDetails.PhoneCountryCode
		/// Item.SellerContactDetails.PhoneCountryPrefix
		/// Item.SellerContactDetails.PhoneLocalNumber
		/// Item.SellerContactDetails.Street
		/// Item.SellerContactDetails.Street2
		/// Item.ShippingDetails.PaymentInstructions
		/// Item.SKU (unless InventoryTrackingMethod is SKU)
		/// Item.SubTitle
		/// 
		/// These values are case-sensitive. Use values that match the case of the
		/// schema element names (Item.PictureDetails.GalleryURL) or make the initial
		/// letter of each field name lowercase (item.pictureDetails.galleryURL).
		/// However, do not change the case of letters in the middle of a field name.
		/// For example, item.picturedetails.galleryUrl is not allowed.
		/// 
		/// To delete a listing enhancement like BoldTitle, specify the value you are
		/// deleting in square brackets ("[ ]"); for example,
		/// Item.ListingEnhancement[BoldTitle].
		/// </param>
		///
		public FeeTypeCollection ReviseFixedPriceItem(ItemType Item, StringCollection DeletedFieldList)
		{
			this.Item = Item;
			this.DeletedFieldList = DeletedFieldList;

			Execute();
			return ApiResponse.Fees;
		}

		/// <summary>
		/// 
		/// </summary>
		public override void Execute()
		{
			if (ApiContext.EPSServerUrl != null && PictureFileList != null && PictureFileList.Count > 0)
			{
				eBayPictureService eps = new eBayPictureService(this.ApiContext);
				if (Item.PictureDetails == null)
				{
					Item.PictureDetails = new PictureDetailsType();
					Item.PictureDetails.PhotoDisplay = PhotoDisplayCodeType.None;
				} 
				else if (!Item.PictureDetails.PhotoDisplaySpecified || Item.PictureDetails.PhotoDisplay == PhotoDisplayCodeType.CustomCode)
				{
					Item.PictureDetails.PhotoDisplay = PhotoDisplayCodeType.None;
				}

				try
				{
					Item.PictureDetails.PictureURL = new StringCollection();
					Item.PictureDetails.PictureURL.AddRange(eps.UpLoadPictureFiles(Item.PictureDetails.PhotoDisplay, PictureFileList.ToArray()));
				} 
				catch (Exception ex)
				{
					LogMessage(ex.Message, MessageType.Exception, MessageSeverity.Error);
					throw new SdkException(ex.Message, ex);
				}
			}				
			base.Execute();

			if (ApiResponse.CategoryID != null && ApiResponse.CategoryID.Length > 0)
			{
				if (Item.PrimaryCategory == null)
					Item.PrimaryCategory = new CategoryType();

				Item.PrimaryCategory.CategoryID = ApiResponse.CategoryID;
			}
			if (ApiResponse.Category2ID != null && ApiResponse.Category2ID.Length > 0)
			{
				if (Item.SecondaryCategory == null)
					Item.SecondaryCategory = new CategoryType();

				Item.SecondaryCategory.CategoryID = ApiResponse.Category2ID;
			}
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
		/// Gets or sets the <see cref="ReviseFixedPriceItemRequestType"/> for this API call.
		/// </summary>
		public ReviseFixedPriceItemRequestType ApiRequest
		{ 
			get { return (ReviseFixedPriceItemRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="ReviseFixedPriceItemResponseType"/> for this API call.
		/// </summary>
		public ReviseFixedPriceItemResponseType ApiResponse
		{ 
			get { return (ReviseFixedPriceItemResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseFixedPriceItemRequestType.Item"/> of type <see cref="ItemType"/>.
		/// </summary>
		public ItemType Item
		{ 
			get { return ApiRequest.Item; }
			set { ApiRequest.Item = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseFixedPriceItemRequestType.DeletedField"/> of type <see cref="StringCollection"/>.
		/// </summary>
		public StringCollection DeletedFieldList
		{ 
			get { return ApiRequest.DeletedField; }
			set { ApiRequest.DeletedField = value; }
		}
		/// <summary>
		///
		/// </summary>
										public StringCollection PictureFileList
		{ 
			get { return mPictureFileList; }
			set { mPictureFileList = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="ReviseFixedPriceItemResponseType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemID
		{ 
			get { return ApiResponse.ItemID; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="ReviseFixedPriceItemResponseType.SKU"/> of type <see cref="string"/>.
		/// </summary>
		public string SKU
		{ 
			get { return ApiResponse.SKU; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="ReviseFixedPriceItemResponseType.StartTime"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime StartTime
		{ 
			get { return ApiResponse.StartTime; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="ReviseFixedPriceItemResponseType.EndTime"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime EndTime
		{ 
			get { return ApiResponse.EndTime; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="ReviseFixedPriceItemResponseType.Fees"/> of type <see cref="FeeTypeCollection"/>.
		/// </summary>
		public FeeTypeCollection FeeList
		{ 
			get { return ApiResponse.Fees; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="ReviseFixedPriceItemResponseType.CategoryID"/> of type <see cref="string"/>.
		/// </summary>
		public string CategoryID
		{ 
			get { return ApiResponse.CategoryID; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="ReviseFixedPriceItemResponseType.Category2ID"/> of type <see cref="string"/>.
		/// </summary>
		public string Category2ID
		{ 
			get { return ApiResponse.Category2ID; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="ReviseFixedPriceItemResponseType.DiscountReason"/> of type <see cref="DiscountReasonCodeTypeCollection"/>.
		/// </summary>
		public DiscountReasonCodeTypeCollection DiscountReasonList
		{ 
			get { return ApiResponse.DiscountReason; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="ReviseFixedPriceItemResponseType.ProductSuggestions"/> of type <see cref="ProductSuggestionsType"/>.
		/// </summary>
		public ProductSuggestionsType ProductSuggestions
		{ 
			get { return ApiResponse.ProductSuggestions; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="ReviseFixedPriceItemResponseType.ListingRecommendations"/> of type <see cref="ListingRecommendationsType"/>.
		/// </summary>
		public ListingRecommendationsType ListingRecommendations
		{ 
			get { return ApiResponse.ListingRecommendations; }
		}
		

		#endregion

		#region Private Fields
		private StringCollection mPictureFileList = new StringCollection();
		#endregion
		
	}
}
