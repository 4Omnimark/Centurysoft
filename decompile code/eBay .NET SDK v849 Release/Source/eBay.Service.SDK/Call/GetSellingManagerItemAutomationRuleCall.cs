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
	public class GetSellingManagerItemAutomationRuleCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetSellingManagerItemAutomationRuleCall()
		{
			ApiRequest = new GetSellingManagerItemAutomationRuleRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetSellingManagerItemAutomationRuleCall(ApiContext ApiContext)
		{
			ApiRequest = new GetSellingManagerItemAutomationRuleRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Retrieves the set of Selling Manager automation rules
		/// associated with an item.
		/// This call is subject to change without notice; the
		/// deprecation process is inapplicable to this call.
		/// </summary>
		/// 
		/// <param name="ItemID">
		/// The ID of the item whose Selling Manager automation rules
		/// you want to retrieve.
		/// </param>
		///
		public SellingManagerAutoListType GetSellingManagerItemAutomationRule(string ItemID)
		{
			this.ItemID = ItemID;

			Execute();
			return ApiResponse.AutomatedListingRule;
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
		/// Gets or sets the <see cref="GetSellingManagerItemAutomationRuleRequestType"/> for this API call.
		/// </summary>
		public GetSellingManagerItemAutomationRuleRequestType ApiRequest
		{ 
			get { return (GetSellingManagerItemAutomationRuleRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetSellingManagerItemAutomationRuleResponseType"/> for this API call.
		/// </summary>
		public GetSellingManagerItemAutomationRuleResponseType ApiResponse
		{ 
			get { return (GetSellingManagerItemAutomationRuleResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellingManagerItemAutomationRuleRequestType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemID
		{ 
			get { return ApiRequest.ItemID; }
			set { ApiRequest.ItemID = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellingManagerItemAutomationRuleResponseType.AutomatedListingRule"/> of type <see cref="SellingManagerAutoListType"/>.
		/// </summary>
		public SellingManagerAutoListType AutomatedListingRule
		{ 
			get { return ApiResponse.AutomatedListingRule; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellingManagerItemAutomationRuleResponseType.AutomatedRelistingRule"/> of type <see cref="SellingManagerAutoRelistType"/>.
		/// </summary>
		public SellingManagerAutoRelistType AutomatedRelistingRule
		{ 
			get { return ApiResponse.AutomatedRelistingRule; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellingManagerItemAutomationRuleResponseType.AutomatedSecondChanceOfferRule"/> of type <see cref="SellingManagerAutoSecondChanceOfferType"/>.
		/// </summary>
		public SellingManagerAutoSecondChanceOfferType AutomatedSecondChanceOfferRule
		{ 
			get { return ApiResponse.AutomatedSecondChanceOfferRule; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellingManagerItemAutomationRuleResponseType.Fees"/> of type <see cref="FeeTypeCollection"/>.
		/// </summary>
		public FeeTypeCollection FeeList
		{ 
			get { return ApiResponse.Fees; }
		}
		

		#endregion

		
	}
}
