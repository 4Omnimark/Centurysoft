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
	public class GetItemCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetItemCall()
		{
			ApiRequest = new GetItemRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetItemCall(ApiContext ApiContext)
		{
			ApiRequest = new GetItemRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Returns item data such as title, description, price information, seller information, and so on, for the specified <b>ItemID</b>. &nbsp;<b>Also for Half.com</b>.
		/// </summary>
		/// 
		/// <param name="ItemID">
		/// Specifies the <b>ItemID</b> that uniquely identifies the item listing for which
		/// to retrieve the data.
		/// 
		/// <b>ItemID</b> is a required input in most cases. <b>SKU</b> can be used instead in certain
		/// cases (see the description of SKU). If both <b>ItemID</b> and <b>SKU</b> are specified for
		/// items where the inventory tracking method is <b>ItemID</b>, <b>ItemID</b> takes precedence.
		/// </param>
		///
		/// <param name="IncludeWatchCount">
		/// Indicates if the caller wants to include watch count for that item in the
		/// response. You must be the seller of the item to retrieve the watch count.
		/// </param>
		///
		/// <param name="IncludeCrossPromotion">
		/// This flag should no longer be used as eBay Store Cross Promotions are no
		/// longer supported by the Trading API.
		/// 
		/// Specifies whether or not to include cross-promotion information for
		/// the item in the call response.
		/// </param>
		///
		/// <param name="IncludeItemSpecifics">
		/// If <code>true</code>, the response returns the <b>ItemSpecifics</b> node
		/// (if the listing has custom Item Specifics).
		/// 
		/// Item Specifics are well-known aspects of items in a given
		/// category. For example, items in a washer and dryer category
		/// might have an aspect like Type=Top-Loading; whereas
		/// items in a jewelry category might have an aspect like
		/// Gemstone=Amber.
		/// 
		/// Including this field set to <code>true</code> also returns the <strong>UnitInfo</strong> node, which enables European Union sellers to provide the required price-per-unit information so buyers can accurately compare prices for certain types of products.
		/// <br/><br/>
		/// (This does not cause the response to include ID-based
		/// attributes. To also retrieve ID-based attributes,
		/// pass <b>DetailLevel</b> in the request with the value
		/// <b>ItemReturnAttributes</b> or <b>ReturnAll</b>.)
		/// </param>
		///
		/// <param name="IncludeTaxTable">
		/// If true, an associated tax table is returned in the response.
		/// If no tax table is associated with the item, then no
		/// tax table is returned, even if <b>IncludeTaxTable</b> is set to true.
		/// </param>
		///
		/// <param name="SKU">
		/// Retrieves an item that was listed by the user identified
		/// in AuthToken and that is being tracked by this SKU.
		/// 
		/// A SKU (stock keeping unit) is an identifier defined by a seller.
		/// Some sellers use SKUs to track complex flows of products
		/// and information on the client side.
		/// eBay preserves the SKU on the item, enabling you
		/// to obtain it before and after an order line item is created.
		/// (SKU is recommended as an alternative to
		/// ApplicationData.)
		/// 
		/// In <b>GetItem</b>, <b>SKU</b> can only be used to retrieve one of your
		/// own items, where you listed the item by using <b>AddFixedPriceItem</b>
		/// or <b>RelistFixedPriceItem</b>,
		/// and you set <b>Item.InventoryTrackingMethod</b> to <b>SKU</b> at
		/// the time the item was listed. (These criteria are necessary to
		/// uniquely identify the listing by a SKU.)
		/// 
		/// Either <b>ItemID</b> or <b>SKU</b> is required in the request.
		/// If both are passed, they must refer to the same item,
		/// and that item must have <b>InventoryTrackingMethod</b> set to <b>SKU</b>.
		/// </param>
		///
		/// <param name="VariationSKU">
		/// Variation-level SKU that uniquely identifes a Variation within
		/// the listing identified by <b>ItemID</b>. Only applicable when the
		/// seller listed the item with Variation-level SKU (<b>Variation.SKU</b>)
		/// values. Retrieves all the usual <b>Item</b> fields, but limits the
		/// <b>Variations</b> content to the specified Variation.
		/// If not specified, the response includes all Variations.
		/// </param>
		///
		/// <param name="VariationSpecificList">
		/// Name-value pairs that identify one or more Variations within the
		/// listing identified by <b>ItemID</b>. Only applicable when the seller
		/// listed the item with Variations. Retrieves all the usual <b>Item</b>
		/// fields, but limits the Variations content to the specified
		/// Variation(s). If the specified pairs do not match any Variation,
		/// eBay returns all Variations.
		/// 
		/// To retrieve only one variation, specify the full set of
		/// name/value pairs that match all the name-value pairs of one
		/// Variation. 
		/// 
		/// To retrieve multiple variations (using a wildcard),
		/// specify one or more name/value pairs that partially match the
		/// desired variations. For example, if the listing contains
		/// Variations for shirts in different colors and sizes, specify
		/// Color as Red (and no other name/value pairs) to retrieve
		/// all the red shirts in all sizes (but no other colors).
		/// </param>
		///
		/// <param name="TransactionID">
		/// A unique identifier for an order line item (transaction). An order line item is created
		/// when a buyer commits to purchasing an item.
		/// 
		/// Since you can change active multiple-quantity fixed-price listings even
		/// after one of the items has been purchased, the <b>TransactionID</b> is
		/// associated with a snapshot of the item data at the time of the purchase.
		/// 
		/// After one item in a multi-quantity listing has been sold, sellers can not
		/// change the values in the Title, Primary Category, Secondary Category,
		/// Listing Duration, and Listing Type fields. However, all other fields are
		/// editable.
		/// 
		/// Specifying a <b>TransactionID</b> in the <b>GetItem</b> request allows you to retrieve
		/// a snapshot of the listing as it was when the order line item was created.
		/// </param>
		///
		/// <param name="IncludeItemCompatibilityList">
		/// This field is used to specify whether to retrieve Parts Compatiblity information. If <code>true</code>, any compatible applications associated with the item will be returned in the response (<b class="con"> Item.ItemCompatibilityList</b>). If no compatible applications have been specified for the item, no item compatibilities will be returned.
		/// 
		/// If <code>false</code> or not specified, the response will return a compatibility count (<b class="con">ItemCompatibilityCoun</b>t) when parts compatibilities have been specified for the item.
		/// 
		/// Parts Compatibility is supported in limited Parts & Accessories categories, for the eBay US Motors (100), UK (3), AU (15) and DE (77) sites only.
		/// </param>
		///
		public ItemType GetItem(string ItemID, bool IncludeWatchCount, bool IncludeCrossPromotion, bool IncludeItemSpecifics, bool IncludeTaxTable, string SKU, string VariationSKU, NameValueListTypeCollection VariationSpecificList, string TransactionID, bool IncludeItemCompatibilityList)
		{
			this.ItemID = ItemID;
			this.IncludeWatchCount = IncludeWatchCount;
			this.IncludeCrossPromotion = IncludeCrossPromotion;
			this.IncludeItemSpecifics = IncludeItemSpecifics;
			this.IncludeTaxTable = IncludeTaxTable;
			this.SKU = SKU;
			this.VariationSKU = VariationSKU;
			this.VariationSpecificList = VariationSpecificList;
			this.TransactionID = TransactionID;
			this.IncludeItemCompatibilityList = IncludeItemCompatibilityList;

			Execute();
			return ApiResponse.Item;
		}


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public ItemType GetItem(string ItemID)
		{
			this.ItemID = ItemID;
			Execute();
			return Item;
		}
		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public ItemType GetItem(string ItemID, bool IncludeWatchCount)
		{
			this.ItemID = ItemID;
			this.IncludeWatchCount = IncludeWatchCount;

			Execute();
			return ApiResponse.Item;
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
		/// Gets or sets the <see cref="GetItemRequestType"/> for this API call.
		/// </summary>
		public GetItemRequestType ApiRequest
		{ 
			get { return (GetItemRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetItemResponseType"/> for this API call.
		/// </summary>
		public GetItemResponseType ApiResponse
		{ 
			get { return (GetItemResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetItemRequestType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemID
		{ 
			get { return ApiRequest.ItemID; }
			set { ApiRequest.ItemID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetItemRequestType.IncludeWatchCount"/> of type <see cref="bool"/>.
		/// </summary>
		public bool IncludeWatchCount
		{ 
			get { return ApiRequest.IncludeWatchCount; }
			set { ApiRequest.IncludeWatchCount = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetItemRequestType.IncludeCrossPromotion"/> of type <see cref="bool"/>.
		/// </summary>
		public bool IncludeCrossPromotion
		{ 
			get { return ApiRequest.IncludeCrossPromotion; }
			set { ApiRequest.IncludeCrossPromotion = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetItemRequestType.IncludeItemSpecifics"/> of type <see cref="bool"/>.
		/// </summary>
		public bool IncludeItemSpecifics
		{ 
			get { return ApiRequest.IncludeItemSpecifics; }
			set { ApiRequest.IncludeItemSpecifics = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetItemRequestType.IncludeTaxTable"/> of type <see cref="bool"/>.
		/// </summary>
		public bool IncludeTaxTable
		{ 
			get { return ApiRequest.IncludeTaxTable; }
			set { ApiRequest.IncludeTaxTable = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetItemRequestType.SKU"/> of type <see cref="string"/>.
		/// </summary>
		public string SKU
		{ 
			get { return ApiRequest.SKU; }
			set { ApiRequest.SKU = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetItemRequestType.VariationSKU"/> of type <see cref="string"/>.
		/// </summary>
		public string VariationSKU
		{ 
			get { return ApiRequest.VariationSKU; }
			set { ApiRequest.VariationSKU = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetItemRequestType.VariationSpecifics"/> of type <see cref="NameValueListTypeCollection"/>.
		/// </summary>
		public NameValueListTypeCollection VariationSpecificList
		{ 
			get { return ApiRequest.VariationSpecifics; }
			set { ApiRequest.VariationSpecifics = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetItemRequestType.TransactionID"/> of type <see cref="string"/>.
		/// </summary>
		public string TransactionID
		{ 
			get { return ApiRequest.TransactionID; }
			set { ApiRequest.TransactionID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetItemRequestType.IncludeItemCompatibilityList"/> of type <see cref="bool"/>.
		/// </summary>
		public bool IncludeItemCompatibilityList
		{ 
			get { return ApiRequest.IncludeItemCompatibilityList; }
			set { ApiRequest.IncludeItemCompatibilityList = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetItemResponseType.Item"/> of type <see cref="ItemType"/>.
		/// </summary>
		public ItemType Item
		{ 
			get { return ApiResponse.Item; }
		}
		

		#endregion

		
	}
}
