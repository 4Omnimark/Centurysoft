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
	public class SetSellingManagerItemAutomationRuleCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public SetSellingManagerItemAutomationRuleCall()
		{
			ApiRequest = new SetSellingManagerItemAutomationRuleRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public SetSellingManagerItemAutomationRuleCall(ApiContext ApiContext)
		{
			ApiRequest = new SetSellingManagerItemAutomationRuleRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Revises, or adds to, the set of Selling Manager automation
		/// rules associated with an item.
		/// 
		/// 
		/// This call is subject to change without notice; the
		/// deprecation process is inapplicable to this call.
		/// 
		/// 
		/// Using this call, you can add an autolist rule.
		/// You also can add a second chance offer
		/// rule (restricted to auction items and auction templates).
		/// Note that autorelist rules can only be set on templates.
		/// An autorelist rule for an item is inherited from a template.
		/// 
		/// 
		/// This call also enables you to specify particular information about automation
		/// rules.
		/// 
		/// 
		/// If a node is not passed in the call, the setting for the corresponding
		/// automation rule remains unchanged.
		/// 
		/// 
		/// Although this call can revise (overwrite) an existing rule,
		/// this call cannot delete an automation rule.
		/// (Instead, use DeleteSellingManagerItemAutomationRule.)
		/// </summary>
		/// 
		/// <param name="ItemID">
		/// The item ID whose automation rules you want to change.
		/// </param>
		///
		/// <param name="AutomatedRelistingRule">
		/// The information for the automated relisting rule to be associated with the item.
		/// </param>
		///
		/// <param name="AutomatedSecondChanceOfferRule">
		/// The information for the automated second chance offer rule to be associated with the item.
		/// </param>
		///
		public SellingManagerAutoListType SetSellingManagerItemAutomationRule(string ItemID, SellingManagerAutoRelistType AutomatedRelistingRule, SellingManagerAutoSecondChanceOfferType AutomatedSecondChanceOfferRule)
		{
			this.ItemID = ItemID;
			this.AutomatedRelistingRule = AutomatedRelistingRule;
			this.AutomatedSecondChanceOfferRule = AutomatedSecondChanceOfferRule;

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
		/// Gets or sets the <see cref="SetSellingManagerItemAutomationRuleRequestType"/> for this API call.
		/// </summary>
		public SetSellingManagerItemAutomationRuleRequestType ApiRequest
		{ 
			get { return (SetSellingManagerItemAutomationRuleRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="SetSellingManagerItemAutomationRuleResponseType"/> for this API call.
		/// </summary>
		public SetSellingManagerItemAutomationRuleResponseType ApiResponse
		{ 
			get { return (SetSellingManagerItemAutomationRuleResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="SetSellingManagerItemAutomationRuleRequestType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemID
		{ 
			get { return ApiRequest.ItemID; }
			set { ApiRequest.ItemID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetSellingManagerItemAutomationRuleRequestType.AutomatedRelistingRule"/> of type <see cref="SellingManagerAutoRelistType"/>.
		/// </summary>
		public SellingManagerAutoRelistType AutomatedRelistingRule
		{ 
			get { return ApiRequest.AutomatedRelistingRule; }
			set { ApiRequest.AutomatedRelistingRule = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetSellingManagerItemAutomationRuleRequestType.AutomatedSecondChanceOfferRule"/> of type <see cref="SellingManagerAutoSecondChanceOfferType"/>.
		/// </summary>
		public SellingManagerAutoSecondChanceOfferType AutomatedSecondChanceOfferRule
		{ 
			get { return ApiRequest.AutomatedSecondChanceOfferRule; }
			set { ApiRequest.AutomatedSecondChanceOfferRule = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="SetSellingManagerItemAutomationRuleResponseType.AutomatedListingRule"/> of type <see cref="SellingManagerAutoListType"/>.
		/// </summary>
		public SellingManagerAutoListType AutomatedListingRule
		{ 
			get { return ApiResponse.AutomatedListingRule; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="SetSellingManagerItemAutomationRuleResponseType.AutomatedRelistingRule"/> of type <see cref="SellingManagerAutoRelistType"/>.
		/// </summary>
		public SellingManagerAutoRelistType AutomatedRelistingRuleReturn
		{ 
			get { return ApiResponse.AutomatedRelistingRule; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="SetSellingManagerItemAutomationRuleResponseType.AutomatedSecondChanceOfferRule"/> of type <see cref="SellingManagerAutoSecondChanceOfferType"/>.
		/// </summary>
		public SellingManagerAutoSecondChanceOfferType AutomatedSecondChanceOfferRuleReturn
		{ 
			get { return ApiResponse.AutomatedSecondChanceOfferRule; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="SetSellingManagerItemAutomationRuleResponseType.Fees"/> of type <see cref="FeeTypeCollection"/>.
		/// </summary>
		public FeeTypeCollection FeeList
		{ 
			get { return ApiResponse.Fees; }
		}
		

		#endregion

		
	}
}
