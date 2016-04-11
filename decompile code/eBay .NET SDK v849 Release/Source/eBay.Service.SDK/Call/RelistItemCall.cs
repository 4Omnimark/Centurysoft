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
	public class RelistItemCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public RelistItemCall()
		{
			ApiRequest = new RelistItemRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public RelistItemCall(ApiContext ApiContext)
		{
			ApiRequest = new RelistItemRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Enables a seller to take a single item (or a single multi-item
		/// listing) and re-list it on a specified eBay site.
		/// </summary>
		/// 
		/// <param name="Item">
		/// Child elements hold the values for item properties that change for the
		/// item re-list. Item is a required input. At a minimum, the Item.ItemID
		/// property must be set to the ID of the listing being re-listed (a
		/// listing that ended in the past 90 days). By default, the new listing's
		/// Item object properties are the same as those of the original (ended)
		/// listing. By setting a new value in the Item object, the new listing
		/// uses the new value rather than the corresponding value from the old
		/// listing.
		/// </param>
		///
		/// <param name="DeletedFieldList">
		/// Specifies the name of the field to delete from a listing.
		/// See the eBay Features Guide for rules on deleting values when relisting items.
		/// Also see the relevant field descriptions to determine when to use DeletedField (and potential consequences).
		/// The request can contain zero, one, or many instances of DeletedField (one for each field to be deleted).
		/// DeletedField accepts the following path names, which delete the corresponding nodes:
		/// Item.ApplicationData
		/// Item.AttributeSetArray
		/// Item.BuyItNowPrice
		/// Item.Charity
		/// Item.ConditionID
		/// Item.ItemSpecifics
		/// Item.ListingCheckoutRedirectPreference.ProStoresStoreName
		/// Item.ListingCheckoutRedirectPreference.SellerThirdPartyUsername
		/// Item.ListingDesigner.LayoutID
		/// Item.ListingDesigner.ThemeID
		/// Item.ListingDetails.LocalListingDistance
		/// Item.ListingDetails.MinimumBestOfferMessage
		/// Item.ListingDetails.MinimumBestOfferPrice
		/// Item.ListingEnhancement[Value]
		/// Item.PayPalEmailAddress
		/// Item.PictureDetails.GalleryURL
		/// Item.PictureDetails.PictureURL
		/// Item.PostalCode
		/// Item.ProductListingDetails
		/// Item.SecondaryCategory
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
		/// Item.SKU
		/// Item.SubTitle
		/// These values are case-sensitive. Use values that match the case of the schema element names
		/// (Item.PictureDetails.GalleryURL) or make the initial letter of each field name lowercase (item.pictureDetails.galleryURL).
		/// However, do not change the case of letters in the middle of a field name.
		/// For example, item.picturedetails.galleryUrl is not allowed.
		/// To delete a listing enhancement like Featured, specify the value you are deleting;
		/// for example, Item.ListingEnhancement[Featured].
		/// </param>
		///
		public FeeTypeCollection RelistItem(ItemType Item, StringCollection DeletedFieldList)
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
			if (Item != null)
			{

				if ((Item.UUID == null || Item.UUID.Length == 0) && AutoSetItemUUID)
				{
					Item.UUID = NewUUID();
				}
				if (ApiContext.EPSServerUrl != null && PictureFileList != null && PictureFileList.Count > 0)
				{
					eBayPictureService eps = new eBayPictureService(ApiContext);
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

			}
			base.Execute();

			string origid = Item.ItemID;
			Item.ItemID = ApiResponse.ItemID;

			if (Item.ListingDetails == null)
				Item.ListingDetails = new ListingDetailsType();
			Item.ListingDetails.StartTime = ApiResponse.StartTime;
			Item.ListingDetails.EndTime = ApiResponse.EndTime;
			Item.ListingDetails.RelistedItemID = origid;

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


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public FeeTypeCollection RelistItem(ItemType Item)
		{
			this.Item = Item;
			this.Execute();
			return FeeList;
		}

		#endregion



		#region Static Methods
		/// <summary>
		/// Generates a universal unique identifier.
		/// </summary>
		/// <returns>A universal unique identifier of type <see cref="string"/></returns>
		public static string NewUUID()
		{
			return System.Guid.NewGuid().ToString().Replace("-", "").ToString();
		}

		/// <summary>
		/// Sets or overwrites the <see cref="ItemType.UUID"/>.
		/// </summary>
		/// <param name="Item">The item to assign a universal unique identifier to.</param>
		public static void ResetItemUUID(ItemType Item)
		{
			Item.UUID = NewUUID();
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
		/// Gets or sets the <see cref="RelistItemRequestType"/> for this API call.
		/// </summary>
		public RelistItemRequestType ApiRequest
		{ 
			get { return (RelistItemRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="RelistItemResponseType"/> for this API call.
		/// </summary>
		public RelistItemResponseType ApiResponse
		{ 
			get { return (RelistItemResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="RelistItemRequestType.Item"/> of type <see cref="ItemType"/>.
		/// </summary>
		public ItemType Item
		{ 
			get { return ApiRequest.Item; }
			set { ApiRequest.Item = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="RelistItemRequestType.DeletedField"/> of type <see cref="StringCollection"/>.
		/// </summary>
		public StringCollection DeletedFieldList
		{ 
			get { return ApiRequest.DeletedField; }
			set { ApiRequest.DeletedField = value; }
		}
		/// <summary>
		///
		/// </summary>
										public bool AutoSetItemUUID
		{ 
			get { return mAutoSetItemUUID; }
			set { mAutoSetItemUUID = value; }
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
		/// Gets the returned <see cref="RelistItemResponseType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemID
		{ 
			get { return ApiResponse.ItemID; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="RelistItemResponseType.Fees"/> of type <see cref="FeeTypeCollection"/>.
		/// </summary>
		public FeeTypeCollection FeeList
		{ 
			get { return ApiResponse.Fees; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="RelistItemResponseType.StartTime"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime StartTime
		{ 
			get { return ApiResponse.StartTime; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="RelistItemResponseType.EndTime"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime EndTime
		{ 
			get { return ApiResponse.EndTime; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="RelistItemResponseType.CategoryID"/> of type <see cref="string"/>.
		/// </summary>
		public string CategoryID
		{ 
			get { return ApiResponse.CategoryID; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="RelistItemResponseType.Category2ID"/> of type <see cref="string"/>.
		/// </summary>
		public string Category2ID
		{ 
			get { return ApiResponse.Category2ID; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="RelistItemResponseType.DiscountReason"/> of type <see cref="DiscountReasonCodeTypeCollection"/>.
		/// </summary>
		public DiscountReasonCodeTypeCollection DiscountReasonList
		{ 
			get { return ApiResponse.DiscountReason; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="RelistItemResponseType.ProductSuggestions"/> of type <see cref="ProductSuggestionsType"/>.
		/// </summary>
		public ProductSuggestionsType ProductSuggestions
		{ 
			get { return ApiResponse.ProductSuggestions; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="RelistItemResponseType.ListingRecommendations"/> of type <see cref="ListingRecommendationsType"/>.
		/// </summary>
		public ListingRecommendationsType ListingRecommendations
		{ 
			get { return ApiResponse.ListingRecommendations; }
		}
		

		#endregion

		#region Private Fields
		private bool mAutoSetItemUUID = false;
		private StringCollection mPictureFileList = new StringCollection();
		#endregion
		
	}
}
