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
	public class GetCrossPromotionsCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetCrossPromotionsCall()
		{
			ApiRequest = new GetCrossPromotionsRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetCrossPromotionsCall(ApiContext ApiContext)
		{
			ApiRequest = new GetCrossPromotionsRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// <b>No longer recommended.</b> The eBay Store Cross Promotions are no longer
		/// supported in the Trading API. Retrieves a list of upsell or cross-sell items associated
		/// with the specified Item ID.
		/// </summary>
		/// 
		/// <param name="ItemID">
		/// The unique ID of the referring item. The cross-promoted
		/// items will supplement this item.
		/// </param>
		///
		/// <param name="PromotionMethod">
		/// The cross-promotion method you want to use for the
		/// returned list, either UpSell or CrossSell.
		/// </param>
		///
		/// <param name="PromotionViewMode">
		/// The role of the person requesting to view the cross-promoted
		/// items, either seller or buyer. Default is buyer.
		/// </param>
		///
		public CrossPromotionsType GetCrossPromotions(string ItemID, PromotionMethodCodeType PromotionMethod, TradingRoleCodeType PromotionViewMode)
		{
			this.ItemID = ItemID;
			this.PromotionMethod = PromotionMethod;
			this.PromotionViewMode = PromotionViewMode;

			Execute();
			return ApiResponse.CrossPromotion;
		}


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public CrossPromotionsType GetCrossPromotions(string ItemID, PromotionMethodCodeType PromotionMethod)
		{
			this.ItemID = ItemID;
			this.PromotionMethod = PromotionMethod;
			Execute();
			return CrossPromotion;
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
		/// Gets or sets the <see cref="GetCrossPromotionsRequestType"/> for this API call.
		/// </summary>
		public GetCrossPromotionsRequestType ApiRequest
		{ 
			get { return (GetCrossPromotionsRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetCrossPromotionsResponseType"/> for this API call.
		/// </summary>
		public GetCrossPromotionsResponseType ApiResponse
		{ 
			get { return (GetCrossPromotionsResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetCrossPromotionsRequestType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemID
		{ 
			get { return ApiRequest.ItemID; }
			set { ApiRequest.ItemID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetCrossPromotionsRequestType.PromotionMethod"/> of type <see cref="PromotionMethodCodeType"/>.
		/// </summary>
		public PromotionMethodCodeType PromotionMethod
		{ 
			get { return ApiRequest.PromotionMethod; }
			set { ApiRequest.PromotionMethod = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetCrossPromotionsRequestType.PromotionViewMode"/> of type <see cref="TradingRoleCodeType"/>.
		/// </summary>
		public TradingRoleCodeType PromotionViewMode
		{ 
			get { return ApiRequest.PromotionViewMode; }
			set { ApiRequest.PromotionViewMode = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetCrossPromotionsResponseType.CrossPromotion"/> of type <see cref="CrossPromotionsType"/>.
		/// </summary>
		public CrossPromotionsType CrossPromotion
		{ 
			get { return ApiResponse.CrossPromotion; }
		}
		

		#endregion

		
	}
}
