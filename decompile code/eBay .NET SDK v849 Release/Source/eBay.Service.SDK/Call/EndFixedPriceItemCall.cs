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
	public class EndFixedPriceItemCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public EndFixedPriceItemCall()
		{
			ApiRequest = new EndFixedPriceItemRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public EndFixedPriceItemCall(ApiContext ApiContext)
		{
			ApiRequest = new EndFixedPriceItemRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Ends the specified fixed-price listing before the date and time at which
		/// it would normally end (per the listing duration).
		/// </summary>
		/// 
		/// <param name="ItemID">
		/// Unique item ID that identifies the item listing that you want to end.
		/// 
		/// In the EndFixedPriceItem request, either ItemID or SKU is required.
		/// If both are passed in and they don't refer to the same listing, eBay
		/// ignores SKU and considers only the ItemID.
		/// </param>
		///
		/// <param name="EndingReason">
		/// Indicates the seller's reason for ending the listing early. This field
		/// is required if the seller is ending the item early and the item did
		/// not successfully sell.
		/// </param>
		///
		/// <param name="SKU">
		/// The unique SKU of the item being ended. A SKU (stock keeping unit) is
		/// an identifier defined by a seller. SKU can only be used to end an
		/// item if you listed the item by using AddFixedPriceItem or
		/// RelistFixedPriceItem, and you set Item.InventoryTrackingMethod to SKU
		/// at the time the item was listed. (These criteria are necessary to
		/// uniquely identify the listing by a SKU.)
		/// 
		/// In the EndFixedPriceItem request, either ItemID or SKU is required. If
		/// both are passed in and they don't refer to the same listing, eBay
		/// ignores SKU and considers only the ItemID.
		/// 
		/// To remove a SKU when you revise or relist an item, use DeletedField in
		/// the revision or relist call.
		/// </param>
		///
		public DateTime EndFixedPriceItem(string ItemID, EndReasonCodeType EndingReason, string SKU)
		{
			this.ItemID = ItemID;
			this.EndingReason = EndingReason;
			this.SKU = SKU;

			Execute();
			return ApiResponse.EndTime;
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
		/// Gets or sets the <see cref="EndFixedPriceItemRequestType"/> for this API call.
		/// </summary>
		public EndFixedPriceItemRequestType ApiRequest
		{ 
			get { return (EndFixedPriceItemRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="EndFixedPriceItemResponseType"/> for this API call.
		/// </summary>
		public EndFixedPriceItemResponseType ApiResponse
		{ 
			get { return (EndFixedPriceItemResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="EndFixedPriceItemRequestType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemID
		{ 
			get { return ApiRequest.ItemID; }
			set { ApiRequest.ItemID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="EndFixedPriceItemRequestType.EndingReason"/> of type <see cref="EndReasonCodeType"/>.
		/// </summary>
		public EndReasonCodeType EndingReason
		{ 
			get { return ApiRequest.EndingReason; }
			set { ApiRequest.EndingReason = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="EndFixedPriceItemRequestType.SKU"/> of type <see cref="string"/>.
		/// </summary>
		public string SKU
		{ 
			get { return ApiRequest.SKU; }
			set { ApiRequest.SKU = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="EndFixedPriceItemResponseType.EndTime"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime EndTime
		{ 
			get { return ApiResponse.EndTime; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="EndFixedPriceItemResponseType.SKU"/> of type <see cref="string"/>.
		/// </summary>
		public string SKUReturn
		{ 
			get { return ApiResponse.SKU; }
		}
		

		#endregion

		
	}
}
