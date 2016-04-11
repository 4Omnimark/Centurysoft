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
	public class GetPromotionRulesCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetPromotionRulesCall()
		{
			ApiRequest = new GetPromotionRulesRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetPromotionRulesCall(ApiContext ApiContext)
		{
			ApiRequest = new GetPromotionRulesRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// <b>No longer recommended.</b> eBay Store Cross Promotions are no
		/// longer supported in the Trading API. Retrieves all promotion rules associated with the specified item or store category.
		/// </summary>
		/// 
		/// <param name="ItemID">
		/// The unique ID of the item for which to retrieve promotion rules.
		/// Mutually exclusive with StoreCategoryID.
		/// </param>
		///
		/// <param name="StoreCategoryID">
		/// The unique ID of the store category for which to retrieve promotion rules.
		/// Mutually exclusive with ItemID.
		/// </param>
		///
		/// <param name="PromotionMethod">
		/// The type of promotion. (CrossSell: items that are related to or
		/// useful in combination with this item. UpSell: items that are more
		/// expensive than or of higher quality than this item.)
		/// </param>
		///
		public PromotionRuleTypeCollection GetPromotionRules(string ItemID, long StoreCategoryID, PromotionMethodCodeType PromotionMethod)
		{
			this.ItemID = ItemID;
			this.StoreCategoryID = StoreCategoryID;
			this.PromotionMethod = PromotionMethod;

			Execute();
			return ApiResponse.PromotionRuleArray;
		}


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public PromotionRuleTypeCollection GetPromotionRules(string ItemID, PromotionMethodCodeType PromotionMethod)
		{
			this.ItemID = ItemID;
			this.PromotionMethod = PromotionMethod;
			Execute();
			return PromotionRuleList;
		}
		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public PromotionRuleTypeCollection GetPromotionRules(int StoreCategoryID, PromotionMethodCodeType PromotionMethod)
		{
			this.StoreCategoryID = StoreCategoryID;
			this.PromotionMethod = PromotionMethod;
			Execute();
			return PromotionRuleList;
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
		/// Gets or sets the <see cref="GetPromotionRulesRequestType"/> for this API call.
		/// </summary>
		public GetPromotionRulesRequestType ApiRequest
		{ 
			get { return (GetPromotionRulesRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetPromotionRulesResponseType"/> for this API call.
		/// </summary>
		public GetPromotionRulesResponseType ApiResponse
		{ 
			get { return (GetPromotionRulesResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetPromotionRulesRequestType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemID
		{ 
			get { return ApiRequest.ItemID; }
			set { ApiRequest.ItemID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetPromotionRulesRequestType.StoreCategoryID"/> of type <see cref="long"/>.
		/// </summary>
		public long StoreCategoryID
		{ 
			get { return ApiRequest.StoreCategoryID; }
			set { ApiRequest.StoreCategoryID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetPromotionRulesRequestType.PromotionMethod"/> of type <see cref="PromotionMethodCodeType"/>.
		/// </summary>
		public PromotionMethodCodeType PromotionMethod
		{ 
			get { return ApiRequest.PromotionMethod; }
			set { ApiRequest.PromotionMethod = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetPromotionRulesResponseType.PromotionRuleArray"/> of type <see cref="PromotionRuleTypeCollection"/>.
		/// </summary>
		public PromotionRuleTypeCollection PromotionRuleList
		{ 
			get { return ApiResponse.PromotionRuleArray; }
		}
		

		#endregion

		
	}
}
