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
	public class GetMyeBayBuyingCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetMyeBayBuyingCall()
		{
			ApiRequest = new GetMyeBayBuyingRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetMyeBayBuyingCall(ApiContext ApiContext)
		{
			ApiRequest = new GetMyeBayBuyingRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Returns items from the Buying section of the user's My eBay
		/// account, including items that the user is watching, bidding on, has
		/// won, has not won, and has made Best Offers on.
		/// </summary>
		/// 
		/// <param name="WatchList">
		/// Returns the list of items being watched by the user.
		/// 
		/// Set Include to true to return the default response set.
		/// </param>
		///
		/// <param name="BidList">
		/// Returns the list of items on which the user has bid.
		/// 
		/// Set Include to true to return the default response set.
		/// </param>
		///
		/// <param name="BestOfferList">
		/// Returns the list of items on which the user has placed Best Offers.
		/// 
		/// Set Include to true to return the default response set.
		/// </param>
		///
		/// <param name="WonList">
		/// Returns the list of items on which the use has bid.
		/// 
		/// Set Include to true to return the default response set.
		/// </param>
		///
		/// <param name="LostList">
		/// Returns the list of items on which the user has bid on and lost.
		/// 
		/// Set Include to true to return the default response set.
		/// </param>
		///
		/// <param name="FavoriteSearches">
		/// Returns the list of searches that the user has saved in My eBay. Returned
		/// only if the user has saved searches.
		/// 
		/// Set Include to true to return the default response set.
		/// </param>
		///
		/// <param name="FavoriteSellers">
		/// Returns the list of favorite sellers that the user has saved in My eBay.
		/// Returned only if the user has saved a list of favorite sellers.
		/// 
		/// Set Include to true to return the default response set.
		/// </param>
		///
		/// <param name="SecondChanceOffer">
		/// Returns the list of second chance offers made by the user. Returned only
		/// if the user has made second chance offers.
		/// 
		/// Set Include to true to return the default response set.
		/// </param>
		///
		/// <param name="BidAssistantList">
		/// Indicates whether or not the Bid Assistant feature is being used. 
		/// </param>
		///
		/// <param name="DeletedFromWonList">
		/// Returns the list of items the user has won, and subsequently deleted from
		/// their My eBay page.
		/// 
		/// Set Include to true to return the default response set.
		/// </param>
		///
		/// <param name="DeletedFromLostList">
		/// Returns the list of items (auctions) the user did not win and were
		/// subsequently deleted from their My eBay page.
		/// 
		/// Set Include to true to return the default response set.
		/// </param>
		///
		/// <param name="BuyingSummary">
		/// Returns a summary of the user's buying activity.
		/// 
		/// Add a BuyingSummary element in
		/// the request with an Include field set to true to receive a BuyingSummary
		/// container in your response. Add a BuyingSummary element with an Include field
		/// set to false to exclude the BuyingSummary from your response.
		/// </param>
		///
		/// <param name="UserDefinedLists">
		/// Returns the user defined lists, which are lists created by the user in the eBay
		/// UI and filled with items, or sellers, or searches using the 
		/// "Add to List" feature.
		/// 
		/// Set Include to true to return the default response set.
		/// </param>
		///
		/// <param name="HideVariations">
		/// If true, the Variations node is omitted for all multi-variation
		/// listings in the response.
		/// If false, the Variations node is returned for all
		/// multi-variation listings in the response. 
		/// 
		/// Please note that if the seller includes a large number of
		/// variations in many listings, retrieving variations (setting this
		/// flag to false) may degrade the call's performance. Therefore,
		/// when this is false, you may need to reduce the total
		/// number of items you're requesting at once (by using other input
		/// fields, such as Pagination).
		/// </param>
		///
		public BuyingSummaryType GetMyeBayBuying(ItemListCustomizationType WatchList, ItemListCustomizationType BidList, ItemListCustomizationType BestOfferList, ItemListCustomizationType WonList, ItemListCustomizationType LostList, MyeBaySelectionType FavoriteSearches, MyeBaySelectionType FavoriteSellers, MyeBaySelectionType SecondChanceOffer, BidAssistantListType BidAssistantList, ItemListCustomizationType DeletedFromWonList, ItemListCustomizationType DeletedFromLostList, ItemListCustomizationType BuyingSummary, MyeBaySelectionType UserDefinedLists, bool HideVariations)
		{
			this.WatchList = WatchList;
			this.BidList = BidList;
			this.BestOfferList = BestOfferList;
			this.WonList = WonList;
			this.LostList = LostList;
			this.FavoriteSearches = FavoriteSearches;
			this.FavoriteSellers = FavoriteSellers;
			this.SecondChanceOffer = SecondChanceOffer;
			this.BidAssistantList = BidAssistantList;
			this.DeletedFromWonList = DeletedFromWonList;
			this.DeletedFromLostList = DeletedFromLostList;
			this.BuyingSummary = BuyingSummary;
			this.UserDefinedLists = UserDefinedLists;
			this.HideVariations = HideVariations;

			Execute();
			return ApiResponse.BuyingSummary;
		}


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public BuyingSummaryType GetMyeBayBuying(ItemListCustomizationType WatchList, ItemListCustomizationType BidList, ItemListCustomizationType BestOfferList, ItemListCustomizationType WonList, ItemListCustomizationType LostList, MyeBaySelectionType FavoriteSearches, MyeBaySelectionType FavoriteSellers, MyeBaySelectionType SecondChanceOffer)
		{
			this.WatchList = WatchList;
			this.BidList = BidList;
			this.BestOfferList = BestOfferList;
			this.WonList = WonList;
			this.LostList = LostList;
			this.FavoriteSearches = FavoriteSearches;
			this.FavoriteSellers = FavoriteSellers;
			this.SecondChanceOffer = SecondChanceOffer;

			Execute();
			return ApiResponse.BuyingSummary;
		}
		
		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public void GetMyeBayBuying()
		{
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
		/// Gets or sets the <see cref="GetMyeBayBuyingRequestType"/> for this API call.
		/// </summary>
		public GetMyeBayBuyingRequestType ApiRequest
		{ 
			get { return (GetMyeBayBuyingRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetMyeBayBuyingResponseType"/> for this API call.
		/// </summary>
		public GetMyeBayBuyingResponseType ApiResponse
		{ 
			get { return (GetMyeBayBuyingResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetMyeBayBuyingRequestType.WatchList"/> of type <see cref="ItemListCustomizationType"/>.
		/// </summary>
		public ItemListCustomizationType WatchList
		{ 
			get { return ApiRequest.WatchList; }
			set { ApiRequest.WatchList = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetMyeBayBuyingRequestType.BidList"/> of type <see cref="ItemListCustomizationType"/>.
		/// </summary>
		public ItemListCustomizationType BidList
		{ 
			get { return ApiRequest.BidList; }
			set { ApiRequest.BidList = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetMyeBayBuyingRequestType.BestOfferList"/> of type <see cref="ItemListCustomizationType"/>.
		/// </summary>
		public ItemListCustomizationType BestOfferList
		{ 
			get { return ApiRequest.BestOfferList; }
			set { ApiRequest.BestOfferList = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetMyeBayBuyingRequestType.WonList"/> of type <see cref="ItemListCustomizationType"/>.
		/// </summary>
		public ItemListCustomizationType WonList
		{ 
			get { return ApiRequest.WonList; }
			set { ApiRequest.WonList = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetMyeBayBuyingRequestType.LostList"/> of type <see cref="ItemListCustomizationType"/>.
		/// </summary>
		public ItemListCustomizationType LostList
		{ 
			get { return ApiRequest.LostList; }
			set { ApiRequest.LostList = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetMyeBayBuyingRequestType.FavoriteSearches"/> of type <see cref="MyeBaySelectionType"/>.
		/// </summary>
		public MyeBaySelectionType FavoriteSearches
		{ 
			get { return ApiRequest.FavoriteSearches; }
			set { ApiRequest.FavoriteSearches = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetMyeBayBuyingRequestType.FavoriteSellers"/> of type <see cref="MyeBaySelectionType"/>.
		/// </summary>
		public MyeBaySelectionType FavoriteSellers
		{ 
			get { return ApiRequest.FavoriteSellers; }
			set { ApiRequest.FavoriteSellers = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetMyeBayBuyingRequestType.SecondChanceOffer"/> of type <see cref="MyeBaySelectionType"/>.
		/// </summary>
		public MyeBaySelectionType SecondChanceOffer
		{ 
			get { return ApiRequest.SecondChanceOffer; }
			set { ApiRequest.SecondChanceOffer = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetMyeBayBuyingRequestType.BidAssistantList"/> of type <see cref="BidAssistantListType"/>.
		/// </summary>
		public BidAssistantListType BidAssistantList
		{ 
			get { return ApiRequest.BidAssistantList; }
			set { ApiRequest.BidAssistantList = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetMyeBayBuyingRequestType.DeletedFromWonList"/> of type <see cref="ItemListCustomizationType"/>.
		/// </summary>
		public ItemListCustomizationType DeletedFromWonList
		{ 
			get { return ApiRequest.DeletedFromWonList; }
			set { ApiRequest.DeletedFromWonList = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetMyeBayBuyingRequestType.DeletedFromLostList"/> of type <see cref="ItemListCustomizationType"/>.
		/// </summary>
		public ItemListCustomizationType DeletedFromLostList
		{ 
			get { return ApiRequest.DeletedFromLostList; }
			set { ApiRequest.DeletedFromLostList = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetMyeBayBuyingRequestType.BuyingSummary"/> of type <see cref="ItemListCustomizationType"/>.
		/// </summary>
		public ItemListCustomizationType BuyingSummary
		{ 
			get { return ApiRequest.BuyingSummary; }
			set { ApiRequest.BuyingSummary = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetMyeBayBuyingRequestType.UserDefinedLists"/> of type <see cref="MyeBaySelectionType"/>.
		/// </summary>
		public MyeBaySelectionType UserDefinedLists
		{ 
			get { return ApiRequest.UserDefinedLists; }
			set { ApiRequest.UserDefinedLists = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetMyeBayBuyingRequestType.HideVariations"/> of type <see cref="bool"/>.
		/// </summary>
		public bool HideVariations
		{ 
			get { return ApiRequest.HideVariations; }
			set { ApiRequest.HideVariations = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetMyeBayBuyingResponseType.BuyingSummary"/> of type <see cref="BuyingSummaryType"/>.
		/// </summary>
		public BuyingSummaryType BuyingSummaryReturn
		{ 
			get { return ApiResponse.BuyingSummary; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetMyeBayBuyingResponseType.WatchList"/> of type <see cref="PaginatedItemArrayType"/>.
		/// </summary>
		public PaginatedItemArrayType WatchListReturn
		{ 
			get { return ApiResponse.WatchList; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetMyeBayBuyingResponseType.BidList"/> of type <see cref="PaginatedItemArrayType"/>.
		/// </summary>
		public PaginatedItemArrayType BidListReturn
		{ 
			get { return ApiResponse.BidList; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetMyeBayBuyingResponseType.BestOfferList"/> of type <see cref="PaginatedItemArrayType"/>.
		/// </summary>
		public PaginatedItemArrayType BestOfferListReturn
		{ 
			get { return ApiResponse.BestOfferList; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetMyeBayBuyingResponseType.WonList"/> of type <see cref="PaginatedOrderTransactionArrayType"/>.
		/// </summary>
		public PaginatedOrderTransactionArrayType WonListReturn
		{ 
			get { return ApiResponse.WonList; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetMyeBayBuyingResponseType.LostList"/> of type <see cref="PaginatedItemArrayType"/>.
		/// </summary>
		public PaginatedItemArrayType LostListReturn
		{ 
			get { return ApiResponse.LostList; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetMyeBayBuyingResponseType.FavoriteSearches"/> of type <see cref="MyeBayFavoriteSearchListType"/>.
		/// </summary>
		public MyeBayFavoriteSearchListType FavoriteSearchesReturn
		{ 
			get { return ApiResponse.FavoriteSearches; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetMyeBayBuyingResponseType.FavoriteSellers"/> of type <see cref="MyeBayFavoriteSellerListType"/>.
		/// </summary>
		public MyeBayFavoriteSellerListType FavoriteSellersReturn
		{ 
			get { return ApiResponse.FavoriteSellers; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetMyeBayBuyingResponseType.SecondChanceOffer"/> of type <see cref="ItemTypeCollection"/>.
		/// </summary>
		public ItemTypeCollection SecondChanceOfferReturn
		{ 
			get { return ApiResponse.SecondChanceOffer; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetMyeBayBuyingResponseType.BidAssistantList"/> of type <see cref="BidGroupTypeCollection"/>.
		/// </summary>
		public BidGroupTypeCollection BidAssistantListList
		{ 
			get { return ApiResponse.BidAssistantList; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetMyeBayBuyingResponseType.DeletedFromWonList"/> of type <see cref="PaginatedOrderTransactionArrayType"/>.
		/// </summary>
		public PaginatedOrderTransactionArrayType DeletedFromWonListReturn
		{ 
			get { return ApiResponse.DeletedFromWonList; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetMyeBayBuyingResponseType.DeletedFromLostList"/> of type <see cref="PaginatedItemArrayType"/>.
		/// </summary>
		public PaginatedItemArrayType DeletedFromLostListReturn
		{ 
			get { return ApiResponse.DeletedFromLostList; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetMyeBayBuyingResponseType.UserDefinedList"/> of type <see cref="UserDefinedListTypeCollection"/>.
		/// </summary>
		public UserDefinedListTypeCollection UserDefinedListList
		{ 
			get { return ApiResponse.UserDefinedList; }
		}
		

		#endregion

		
	}
}
