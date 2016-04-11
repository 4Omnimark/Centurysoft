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
	public class SetUserPreferencesCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public SetUserPreferencesCall()
		{
			ApiRequest = new SetUserPreferencesRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public SetUserPreferencesCall(ApiContext ApiContext)
		{
			ApiRequest = new SetUserPreferencesRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Defines the <b>SetUserPreferences</b> request container.
		/// </summary>
		/// 
		/// <param name="BidderNoticePreferences">
		/// Container consisting of the seller's preference for receiving contact
		/// information for unsuccessful bidders. This preference is only applicable for
		/// auction listings.
		/// </param>
		///
		/// <param name="CombinedPaymentPreferences">
		/// Container consisting of the seller's preference for allowing combined payment
		/// on multiple orders shared between the same seller and buyer.
		/// 
		/// <span class="tablenote"><strong>Note:</strong>
		/// Calculated and flat-rate shipping preferences are no longer set using this
		/// call. Instead, use the <b>SetShippingDiscountProfiles</b> call to
		/// set the shipping discounts for combined payment orders.
		/// </span>
		/// 
		/// <span class="tablenote"><strong>Note:</strong>
		/// A seller's combined payment preferences can take up to 7 days to
		/// have any logical or functional effect on eBay.
		/// </span>
		/// </param>
		///
		/// <param name="CrossPromotionPreferences">
		/// This container should no longer be used as eBay Store Cross Promotions are no
		/// longer supported in the Trading API. This container will be removed from the
		/// Trading WSDL and API Call Reference docs in a future release.
		/// 
		/// Container consisting of the seller's cross-promotion preferences. These
		/// preferences are only applicable for eBay Store owners. One or more
		/// preferences may be set or modified under this field.
		/// </param>
		///
		/// <param name="SellerPaymentPreferences">
		/// Container consisting of the seller's payment preferences. One or more
		/// preferences may be set or modified under this field. Payment preferences
		/// specified in a <b>SetUserPreferences</b> call override the settings
		/// in My eBay payment preferences.
		/// </param>
		///
		/// <param name="SellerFavoriteItemPreferences">
		/// Container consisting of the seller's preferences for displaying items on a
		/// buyer's Favorite Sellers' Items page or Favorite Sellers' Items digest. One
		/// or more preferences may be set or modified under this field.
		/// </param>
		///
		/// <param name="EndOfAuctionEmailPreferences">
		/// Container consisting of the seller's preferences for the end-of-auction
		/// email sent to the winning bidder. These preferences allow the seller to
		/// customize the Email that is sent to buyer at the end of the auction. One or
		/// more preferences may be set or modified under this field. These preferences
		/// are only applicable for auction listings.
		/// </param>
		///
		/// <param name="EmailShipmentTrackingNumberPreference">
		/// Flag that controls whether the shipment's tracking number is sent by Email
		/// from the seller to the buyer.
		/// </param>
		///
		/// <param name="RequiredShipPhoneNumberPreference">
		/// Flag that controls whether the buyer is required to provide a shipping phone
		/// number upon checkout. Some shipping carriers require the receiver's phone
		/// number.
		/// </param>
		///
		/// <param name="UnpaidItemAssistancePreferences">
		/// Container consisting of a seller's Unpaid Item Assistant preferences. The
		/// Unpaid Item Assistant automatically opens an Unpaid Item dispute on the
		/// behalf of the seller. One or more preferences may be set or modified under
		/// this field.
		/// </param>
		///
		/// <param name="PurchaseReminderEmailPreferences">
		/// Container consisting of a seller's preference for sending a purchase
		/// reminder email to buyers.
		/// </param>
		///
		/// <param name="SellerThirdPartyCheckoutDisabled">
		/// A flag used to disable the use of a third-party application to handle the
		/// checkout flow for a seller. If set to true, Third-Party Checkout is disabled
		/// and any checkout flow initiated on the seller's application is redirected to
		/// the eBay checkout flow.
		/// </param>
		///
		/// <param name="DispatchCutoffTimePreference">
		/// Contains information about a seller's order cut off time preferences for same day shipping. If the seller specifies a value of <code>0</code> in <strong>Item.DispatchTimeMax</strong> to offer same day handling when listing an item, the seller's shipping time commitment depends on the order cut off time set for the listing site, as indicated by <strong>DispatchCutoffTimePreference.CutoffTime</strong>.
		/// </param>
		///
		/// <param name="GlobalShippingProgramListingPreference">
		/// If this flag is included and set to <code>true</code>, the seller's new listings will enable the Global Shipping Program by default.
		/// <br/><br/>
		/// <span class="tablenote">
		/// <strong>Note:</strong> This field is ignored for sellers who are not opted in to the Global Shipping Program (when GetUserPreferences returns <strong>OfferGlobalShippingProgramPreference</strong> with a value of <code>false</code>).
		/// </span>
		/// </param>
		///
		/// <param name="OverrideGSPserviceWithIntlService">
		/// If this flag is included and set to <code>true</code>, and the seller specifies an international shipping service to a particular country for a given listing, the specified service will take precedence and be the listing's default international shipping option for buyers in that country, rather than the Global Shipping Program. The Global Shipping Program will still be the listing's default option for shipping to any Global Shipping-eligible country for which the seller does <em>not</em> specify an international shipping service.
		/// <br/><br/>
		/// If this flag is set to <code>false</code>, the Global Shipping Program will be each Global Shipping-eligible listing's default option for shipping to any Global Shipping-eligible country, regardless of any international shipping service that the seller specifies for the listing.
		/// </param>
		///
		public void SetUserPreferences(BidderNoticePreferencesType BidderNoticePreferences, CombinedPaymentPreferencesType CombinedPaymentPreferences, CrossPromotionPreferencesType CrossPromotionPreferences, SellerPaymentPreferencesType SellerPaymentPreferences, SellerFavoriteItemPreferencesType SellerFavoriteItemPreferences, EndOfAuctionEmailPreferencesType EndOfAuctionEmailPreferences, bool EmailShipmentTrackingNumberPreference, bool RequiredShipPhoneNumberPreference, UnpaidItemAssistancePreferencesType UnpaidItemAssistancePreferences, PurchaseReminderEmailPreferencesType PurchaseReminderEmailPreferences, bool SellerThirdPartyCheckoutDisabled, DispatchCutoffTimePreferencesType DispatchCutoffTimePreference, bool GlobalShippingProgramListingPreference, bool OverrideGSPserviceWithIntlService)
		{
			this.BidderNoticePreferences = BidderNoticePreferences;
			this.CombinedPaymentPreferences = CombinedPaymentPreferences;
			this.CrossPromotionPreferences = CrossPromotionPreferences;
			this.SellerPaymentPreferences = SellerPaymentPreferences;
			this.SellerFavoriteItemPreferences = SellerFavoriteItemPreferences;
			this.EndOfAuctionEmailPreferences = EndOfAuctionEmailPreferences;
			this.EmailShipmentTrackingNumberPreference = EmailShipmentTrackingNumberPreference;
			this.RequiredShipPhoneNumberPreference = RequiredShipPhoneNumberPreference;
			this.UnpaidItemAssistancePreferences = UnpaidItemAssistancePreferences;
			this.PurchaseReminderEmailPreferences = PurchaseReminderEmailPreferences;
			this.SellerThirdPartyCheckoutDisabled = SellerThirdPartyCheckoutDisabled;
			this.DispatchCutoffTimePreference = DispatchCutoffTimePreference;
			this.GlobalShippingProgramListingPreference = GlobalShippingProgramListingPreference;
			this.OverrideGSPserviceWithIntlService = OverrideGSPserviceWithIntlService;

			Execute();
			
		}


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public void SetUserPreferences()
		{
			Execute();
		}
		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public void SetUserPreferences(BidderNoticePreferencesType BidderNoticePreferences, CombinedPaymentPreferencesType CombinedPaymentPreferences, CrossPromotionPreferencesType CrossPromotionPreferences, SellerPaymentPreferencesType SellerPaymentPreferences, SellerFavoriteItemPreferencesType SellerFavoriteItemPreferences, EndOfAuctionEmailPreferencesType EndOfAuctionEmailPreferences)
		{
			this.BidderNoticePreferences = BidderNoticePreferences;
			this.CombinedPaymentPreferences = CombinedPaymentPreferences;
			this.CrossPromotionPreferences = CrossPromotionPreferences;
			this.SellerPaymentPreferences = SellerPaymentPreferences;
			this.SellerFavoriteItemPreferences = SellerFavoriteItemPreferences;
			this.EndOfAuctionEmailPreferences = EndOfAuctionEmailPreferences;

			Execute();
			
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
		/// Gets or sets the <see cref="SetUserPreferencesRequestType"/> for this API call.
		/// </summary>
		public SetUserPreferencesRequestType ApiRequest
		{ 
			get { return (SetUserPreferencesRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="SetUserPreferencesResponseType"/> for this API call.
		/// </summary>
		public SetUserPreferencesResponseType ApiResponse
		{ 
			get { return (SetUserPreferencesResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="SetUserPreferencesRequestType.BidderNoticePreferences"/> of type <see cref="BidderNoticePreferencesType"/>.
		/// </summary>
		public BidderNoticePreferencesType BidderNoticePreferences
		{ 
			get { return ApiRequest.BidderNoticePreferences; }
			set { ApiRequest.BidderNoticePreferences = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetUserPreferencesRequestType.CombinedPaymentPreferences"/> of type <see cref="CombinedPaymentPreferencesType"/>.
		/// </summary>
		public CombinedPaymentPreferencesType CombinedPaymentPreferences
		{ 
			get { return ApiRequest.CombinedPaymentPreferences; }
			set { ApiRequest.CombinedPaymentPreferences = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetUserPreferencesRequestType.CrossPromotionPreferences"/> of type <see cref="CrossPromotionPreferencesType"/>.
		/// </summary>
		public CrossPromotionPreferencesType CrossPromotionPreferences
		{ 
			get { return ApiRequest.CrossPromotionPreferences; }
			set { ApiRequest.CrossPromotionPreferences = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetUserPreferencesRequestType.SellerPaymentPreferences"/> of type <see cref="SellerPaymentPreferencesType"/>.
		/// </summary>
		public SellerPaymentPreferencesType SellerPaymentPreferences
		{ 
			get { return ApiRequest.SellerPaymentPreferences; }
			set { ApiRequest.SellerPaymentPreferences = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetUserPreferencesRequestType.SellerFavoriteItemPreferences"/> of type <see cref="SellerFavoriteItemPreferencesType"/>.
		/// </summary>
		public SellerFavoriteItemPreferencesType SellerFavoriteItemPreferences
		{ 
			get { return ApiRequest.SellerFavoriteItemPreferences; }
			set { ApiRequest.SellerFavoriteItemPreferences = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetUserPreferencesRequestType.EndOfAuctionEmailPreferences"/> of type <see cref="EndOfAuctionEmailPreferencesType"/>.
		/// </summary>
		public EndOfAuctionEmailPreferencesType EndOfAuctionEmailPreferences
		{ 
			get { return ApiRequest.EndOfAuctionEmailPreferences; }
			set { ApiRequest.EndOfAuctionEmailPreferences = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetUserPreferencesRequestType.EmailShipmentTrackingNumberPreference"/> of type <see cref="bool"/>.
		/// </summary>
		public bool EmailShipmentTrackingNumberPreference
		{ 
			get { return ApiRequest.EmailShipmentTrackingNumberPreference; }
			set { ApiRequest.EmailShipmentTrackingNumberPreference = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetUserPreferencesRequestType.RequiredShipPhoneNumberPreference"/> of type <see cref="bool"/>.
		/// </summary>
		public bool RequiredShipPhoneNumberPreference
		{ 
			get { return ApiRequest.RequiredShipPhoneNumberPreference; }
			set { ApiRequest.RequiredShipPhoneNumberPreference = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetUserPreferencesRequestType.UnpaidItemAssistancePreferences"/> of type <see cref="UnpaidItemAssistancePreferencesType"/>.
		/// </summary>
		public UnpaidItemAssistancePreferencesType UnpaidItemAssistancePreferences
		{ 
			get { return ApiRequest.UnpaidItemAssistancePreferences; }
			set { ApiRequest.UnpaidItemAssistancePreferences = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetUserPreferencesRequestType.PurchaseReminderEmailPreferences"/> of type <see cref="PurchaseReminderEmailPreferencesType"/>.
		/// </summary>
		public PurchaseReminderEmailPreferencesType PurchaseReminderEmailPreferences
		{ 
			get { return ApiRequest.PurchaseReminderEmailPreferences; }
			set { ApiRequest.PurchaseReminderEmailPreferences = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetUserPreferencesRequestType.SellerThirdPartyCheckoutDisabled"/> of type <see cref="bool"/>.
		/// </summary>
		public bool SellerThirdPartyCheckoutDisabled
		{ 
			get { return ApiRequest.SellerThirdPartyCheckoutDisabled; }
			set { ApiRequest.SellerThirdPartyCheckoutDisabled = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetUserPreferencesRequestType.DispatchCutoffTimePreference"/> of type <see cref="DispatchCutoffTimePreferencesType"/>.
		/// </summary>
		public DispatchCutoffTimePreferencesType DispatchCutoffTimePreference
		{ 
			get { return ApiRequest.DispatchCutoffTimePreference; }
			set { ApiRequest.DispatchCutoffTimePreference = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetUserPreferencesRequestType.GlobalShippingProgramListingPreference"/> of type <see cref="bool"/>.
		/// </summary>
		public bool GlobalShippingProgramListingPreference
		{ 
			get { return ApiRequest.GlobalShippingProgramListingPreference; }
			set { ApiRequest.GlobalShippingProgramListingPreference = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetUserPreferencesRequestType.OverrideGSPserviceWithIntlService"/> of type <see cref="bool"/>.
		/// </summary>
		public bool OverrideGSPserviceWithIntlService
		{ 
			get { return ApiRequest.OverrideGSPserviceWithIntlService; }
			set { ApiRequest.OverrideGSPserviceWithIntlService = value; }
		}
		
		

		#endregion

		
	}
}
