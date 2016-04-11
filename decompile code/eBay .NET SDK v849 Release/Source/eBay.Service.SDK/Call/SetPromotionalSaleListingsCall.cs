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
	public class SetPromotionalSaleListingsCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public SetPromotionalSaleListingsCall()
		{
			ApiRequest = new SetPromotionalSaleListingsRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public SetPromotionalSaleListingsCall(ApiContext ApiContext)
		{
			ApiRequest = new SetPromotionalSaleListingsRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Enables the seller to change the item listings that are affected by a promotional sale.
		/// </summary>
		/// 
		/// <param name="PromotionalSaleID">
		/// The unique identifier of the seller's promotional sale. Based on the <b>Action</b> 
		/// value, listings will either be added to or removed from the promotional sale.
		/// </param>
		///
		/// <param name="Action">
		/// This required field determines whether you are adding (specify 'Add') or
		/// removing (specify 'Delete) one or more listings from the promotional sale
		/// identified by the <b>PromotionalSaleID</b> value in the request.
		/// 
		/// If you specify 'Delete', you must include one or more <b>ItemID</b>
		/// values under the <b>PromotionalSaleItemIDArray</b> container, and
		/// you cannot use the other filter options in the request. If you specify 'Add',
		/// you can add one or more listings using any of the filtering options in the
		/// request. Active auction listings that have one or more bids cannot be added to
		/// or removed from a promotional sale.
		/// </param>
		///
		/// <param name="PromotionalSaleItemIDArrayList">
		/// Container consisting of one or more <b>ItemID</b> values. Based on
		/// the <b>Action</b> value, the listings identified by these 
		/// <b>ItemID</b> values are either added to or removed from the 
		/// promotional sale.
		/// 
		/// This container is required if listings are being removed (<b>Action</b>='Delete') 
		/// from the promotional sale.
		/// </param>
		///
		/// <param name="StoreCategoryID">
		/// If a <b>StoreCategoryID</b> value is included in the call request,
		/// all active items in this store category are added to the promotional sale. This
		/// field cannot be used if the <b>Action</b> field is set to 'Delete'.
		/// </param>
		///
		/// <param name="CategoryID">
		/// If a <b>CategoryID</b> value is included in the call request,
		/// all active items in this eBay category are added to the promotional sale. This
		/// field cannot be used if the <b>Action</b> field is set to 'Delete'.
		/// </param>
		///
		/// <param name="AllFixedPriceItems">
		/// If this field is included and set to 'true' in the call request, all fixed-price
		/// listings are added to the promotional sale. This field cannot be used if the
		/// <b>Action</b> field is set to 'Delete'.
		/// </param>
		///
		/// <param name="AllStoreInventoryItems">
		/// If this field is included and set to 'true' in the call request, all store inventory
		/// items are added to the promotional sale. This field cannot be used if the
		/// <b>Action</b> field is set to 'Delete'.
		/// </param>
		///
		/// <param name="AllAuctionItems">
		/// If this field is included and set to 'true' in the call request, all auction
		/// listings are added to the promotional sale. This field cannot be used if the
		/// <b>Action</b> field is set to 'Delete'.
		/// </param>
		///
		public PromotionalSaleStatusCodeType SetPromotionalSaleListings(long PromotionalSaleID, ModifyActionCodeType Action, ItemIDArrayType PromotionalSaleItemIDArrayList, long StoreCategoryID, long CategoryID, bool AllFixedPriceItems, bool AllStoreInventoryItems, bool AllAuctionItems)
		{
			this.PromotionalSaleID = PromotionalSaleID;
			this.Action = Action;
			this.PromotionalSaleItemIDArrayList = PromotionalSaleItemIDArrayList;
			this.StoreCategoryID = StoreCategoryID;
			this.CategoryID = CategoryID;
			this.AllFixedPriceItems = AllFixedPriceItems;
			this.AllStoreInventoryItems = AllStoreInventoryItems;
			this.AllAuctionItems = AllAuctionItems;

			Execute();
			return ApiResponse.Status;
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
		/// Gets or sets the <see cref="SetPromotionalSaleListingsRequestType"/> for this API call.
		/// </summary>
		public SetPromotionalSaleListingsRequestType ApiRequest
		{ 
			get { return (SetPromotionalSaleListingsRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="SetPromotionalSaleListingsResponseType"/> for this API call.
		/// </summary>
		public SetPromotionalSaleListingsResponseType ApiResponse
		{ 
			get { return (SetPromotionalSaleListingsResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="SetPromotionalSaleListingsRequestType.PromotionalSaleID"/> of type <see cref="long"/>.
		/// </summary>
		public long PromotionalSaleID
		{ 
			get { return ApiRequest.PromotionalSaleID; }
			set { ApiRequest.PromotionalSaleID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetPromotionalSaleListingsRequestType.Action"/> of type <see cref="ModifyActionCodeType"/>.
		/// </summary>
		public ModifyActionCodeType Action
		{ 
			get { return ApiRequest.Action; }
			set { ApiRequest.Action = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetPromotionalSaleListingsRequestType.PromotionalSaleItemIDArray"/> of type <see cref="ItemIDArrayType"/>.
		/// </summary>
		public ItemIDArrayType PromotionalSaleItemIDArrayList
		{ 
			get { return ApiRequest.PromotionalSaleItemIDArray; }
			set { ApiRequest.PromotionalSaleItemIDArray = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetPromotionalSaleListingsRequestType.StoreCategoryID"/> of type <see cref="long"/>.
		/// </summary>
		public long StoreCategoryID
		{ 
			get { return ApiRequest.StoreCategoryID; }
			set { ApiRequest.StoreCategoryID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetPromotionalSaleListingsRequestType.CategoryID"/> of type <see cref="long"/>.
		/// </summary>
		public long CategoryID
		{ 
			get { return ApiRequest.CategoryID; }
			set { ApiRequest.CategoryID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetPromotionalSaleListingsRequestType.AllFixedPriceItems"/> of type <see cref="bool"/>.
		/// </summary>
		public bool AllFixedPriceItems
		{ 
			get { return ApiRequest.AllFixedPriceItems; }
			set { ApiRequest.AllFixedPriceItems = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetPromotionalSaleListingsRequestType.AllStoreInventoryItems"/> of type <see cref="bool"/>.
		/// </summary>
		public bool AllStoreInventoryItems
		{ 
			get { return ApiRequest.AllStoreInventoryItems; }
			set { ApiRequest.AllStoreInventoryItems = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetPromotionalSaleListingsRequestType.AllAuctionItems"/> of type <see cref="bool"/>.
		/// </summary>
		public bool AllAuctionItems
		{ 
			get { return ApiRequest.AllAuctionItems; }
			set { ApiRequest.AllAuctionItems = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="SetPromotionalSaleListingsResponseType.Status"/> of type <see cref="PromotionalSaleStatusCodeType"/>.
		/// </summary>
		public PromotionalSaleStatusCodeType Status
		{ 
			get { return ApiResponse.Status; }
		}
		

		#endregion

		
	}
}
