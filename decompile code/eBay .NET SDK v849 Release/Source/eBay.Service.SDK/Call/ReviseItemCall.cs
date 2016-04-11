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
	public class ReviseItemCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public ReviseItemCall()
		{
			ApiRequest = new ReviseItemRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public ReviseItemCall(ApiContext ApiContext)
		{
			ApiRequest = new ReviseItemRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Enables a seller to change the properties of a currently active listing.&nbsp;<b>Also for Half.com</b>.
		/// 
		/// 
		/// After one item in a multi-quantity fixed-price listing has been sold, you can not
		/// the values in the Title, Primary Category, Secondary Category, Listing Duration,
		/// and Listing Type fields for that listing. However, all other fields in the
		/// multi-quantity, fixed-price listing are editable.
		/// 
		/// 
		/// Inputs are the Item ID of the listing you are revising, and the field or fields
		/// that you are updating.
		/// </summary>
		/// 
		/// <param name="Item">
		/// Child elements hold the values for properties that are changing. The
		/// Item.ItemID property must always be set to the ID of the item listing being
		/// changed. Set values in the Item object only for those properties that are
		/// changing. Use DeletedField to remove a property.
		/// 
		/// Applicable to Half.com.
		/// </param>
		///
		/// <param name="DeletedFieldList">
		/// Specifies the name of a field to delete from a listing. See the eBay
		/// Trading API guide for rules on deleting values when revising items. Also
		/// see the relevant field descriptions to determine when to use DeletedField
		/// (and potential consequences). The request can contain zero, one, or many
		/// instances of DeletedField (one for each field to be deleted).
		/// 
		/// You cannot delete required fields from a listing.
		/// 
		/// Some fields are optional when you first list an item (e.g.,
		/// SecondaryCategory), but once they are set they cannot be deleted when you
		/// revise an item. Some optional fields cannot be deleted from auction
		/// listings if the item has bids and/or ends within 12 hours. Some optional
		/// fields cannot be deleted if other fields depend on them.
		/// 
		/// Some data (such as Variation nodes within Variations) can't be deleted by
		/// using DeletedFields. See the relevant field descriptions for how to delete
		/// such data. See the eBay Features Guide for rules on removing values
		/// when revising items. Also see the relevant field descriptions for details
		/// on when to use DeletedField (and potential consequences).
		/// 
		/// DeletedField accepts the following path names, which delete the
		/// corresponding nodes:
		/// 
		/// Item.ApplicationData
		/// Item.AttributeSetArray
		/// Item.BuyItNowPrice
		/// Item.ConditionID
		/// Item.ExtendedSellerContactDetails
		/// Item.ClassifiedAdContactByEmailEnabled
		/// Item.ItemSpecifics
		/// Item.ListingCheckoutRedirectPreference.ProStoresStoreName
		/// Item.ListingCheckoutRedirectPreference.SellerThirdPartyUsername
		/// Item.ListingDesigner.LayoutID
		/// Item.ListingDesigner.ThemeID
		/// Item.ListingDetails.MinimumBestOfferMessage
		/// Item.ListingDetails.MinimumBestOfferPrice
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
		/// <param name="VerifyOnly">
		/// When the VerifyOnly boolean tag value is supplied as 'true', the response will include the calculated
		/// listing price change if there is an increase in the BIN/Start price, but does not persist the values in DB.
		/// This call is similar to VerifyAddItem in revise mode.
		/// </param>
		///
		public DateTime ReviseItem(ItemType Item, StringCollection DeletedFieldList, bool VerifyOnly)
		{
			this.Item = Item;
			this.DeletedFieldList = DeletedFieldList;
			this.VerifyOnly = VerifyOnly;

			Execute();
			return ApiResponse.StartTime;
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
		/// Gets or sets the <see cref="ReviseItemRequestType"/> for this API call.
		/// </summary>
		public ReviseItemRequestType ApiRequest
		{ 
			get { return (ReviseItemRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="ReviseItemResponseType"/> for this API call.
		/// </summary>
		public ReviseItemResponseType ApiResponse
		{ 
			get { return (ReviseItemResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseItemRequestType.Item"/> of type <see cref="ItemType"/>.
		/// </summary>
		public ItemType Item
		{ 
			get { return ApiRequest.Item; }
			set { ApiRequest.Item = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseItemRequestType.DeletedField"/> of type <see cref="StringCollection"/>.
		/// </summary>
		public StringCollection DeletedFieldList
		{ 
			get { return ApiRequest.DeletedField; }
			set { ApiRequest.DeletedField = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseItemRequestType.VerifyOnly"/> of type <see cref="bool"/>.
		/// </summary>
		public bool VerifyOnly
		{ 
			get { return ApiRequest.VerifyOnly; }
			set { ApiRequest.VerifyOnly = value; }
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
		/// Gets the returned <see cref="ReviseItemResponseType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemID
		{ 
			get { return ApiResponse.ItemID; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="ReviseItemResponseType.StartTime"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime StartTime
		{ 
			get { return ApiResponse.StartTime; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="ReviseItemResponseType.EndTime"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime EndTime
		{ 
			get { return ApiResponse.EndTime; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="ReviseItemResponseType.Fees"/> of type <see cref="FeeTypeCollection"/>.
		/// </summary>
		public FeeTypeCollection FeeList
		{ 
			get { return ApiResponse.Fees; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="ReviseItemResponseType.CategoryID"/> of type <see cref="string"/>.
		/// </summary>
		public string CategoryID
		{ 
			get { return ApiResponse.CategoryID; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="ReviseItemResponseType.Category2ID"/> of type <see cref="string"/>.
		/// </summary>
		public string Category2ID
		{ 
			get { return ApiResponse.Category2ID; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="ReviseItemResponseType.VerifyOnly"/> of type <see cref="bool"/>.
		/// </summary>
		public bool VerifyOnlyReturn
		{ 
			get { return ApiResponse.VerifyOnly; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="ReviseItemResponseType.DiscountReason"/> of type <see cref="DiscountReasonCodeTypeCollection"/>.
		/// </summary>
		public DiscountReasonCodeTypeCollection DiscountReasonList
		{ 
			get { return ApiResponse.DiscountReason; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="ReviseItemResponseType.ProductSuggestions"/> of type <see cref="ProductSuggestionsType"/>.
		/// </summary>
		public ProductSuggestionsType ProductSuggestions
		{ 
			get { return ApiResponse.ProductSuggestions; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="ReviseItemResponseType.ListingRecommendations"/> of type <see cref="ListingRecommendationsType"/>.
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
