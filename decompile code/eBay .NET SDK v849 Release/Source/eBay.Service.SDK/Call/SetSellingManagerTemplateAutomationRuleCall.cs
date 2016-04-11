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
	public class SetSellingManagerTemplateAutomationRuleCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public SetSellingManagerTemplateAutomationRuleCall()
		{
			ApiRequest = new SetSellingManagerTemplateAutomationRuleRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public SetSellingManagerTemplateAutomationRuleCall(ApiContext ApiContext)
		{
			ApiRequest = new SetSellingManagerTemplateAutomationRuleRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Revises, or adds to, the Selling Manager automation
		/// rules associated with a template.
		/// 
		/// 
		/// This call is subject to change without notice; the
		/// deprecation process is inapplicable to this call.
		/// 
		/// 
		/// Using this call, you can add either an autorelist rule or
		/// an autolist rule, but not both.
		/// You also can add a second chance
		/// offer rule (restricted to auction items and auction templates).
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
		/// (Instead, use DeleteSellingManagerTemplateAutomationRule.)
		/// </summary>
		/// 
		/// <param name="SaleTemplateID">
		/// The ID of the Selling Manager template whose automation rules
		/// you want to change. You can obtain a SaleTemplateID by calling
		/// GetSellingManagerInventory.
		/// </param>
		///
		/// <param name="AutomatedListingRule">
		/// The information for the automated listing rule to be associated with the template.
		/// </param>
		///
		/// <param name="AutomatedRelistingRule">
		/// The information for the automated relisting rule to be associated with the template.
		/// </param>
		///
		/// <param name="AutomatedSecondChanceOfferRule">
		/// The information for the automated second chance offer rule to be associated with the template.
		/// </param>
		///
		public SellingManagerAutoListType SetSellingManagerTemplateAutomationRule(long SaleTemplateID, SellingManagerAutoListType AutomatedListingRule, SellingManagerAutoRelistType AutomatedRelistingRule, SellingManagerAutoSecondChanceOfferType AutomatedSecondChanceOfferRule)
		{
			this.SaleTemplateID = SaleTemplateID;
			this.AutomatedListingRule = AutomatedListingRule;
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
		/// Gets or sets the <see cref="SetSellingManagerTemplateAutomationRuleRequestType"/> for this API call.
		/// </summary>
		public SetSellingManagerTemplateAutomationRuleRequestType ApiRequest
		{ 
			get { return (SetSellingManagerTemplateAutomationRuleRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="SetSellingManagerTemplateAutomationRuleResponseType"/> for this API call.
		/// </summary>
		public SetSellingManagerTemplateAutomationRuleResponseType ApiResponse
		{ 
			get { return (SetSellingManagerTemplateAutomationRuleResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="SetSellingManagerTemplateAutomationRuleRequestType.SaleTemplateID"/> of type <see cref="long"/>.
		/// </summary>
		public long SaleTemplateID
		{ 
			get { return ApiRequest.SaleTemplateID; }
			set { ApiRequest.SaleTemplateID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetSellingManagerTemplateAutomationRuleRequestType.AutomatedListingRule"/> of type <see cref="SellingManagerAutoListType"/>.
		/// </summary>
		public SellingManagerAutoListType AutomatedListingRule
		{ 
			get { return ApiRequest.AutomatedListingRule; }
			set { ApiRequest.AutomatedListingRule = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetSellingManagerTemplateAutomationRuleRequestType.AutomatedRelistingRule"/> of type <see cref="SellingManagerAutoRelistType"/>.
		/// </summary>
		public SellingManagerAutoRelistType AutomatedRelistingRule
		{ 
			get { return ApiRequest.AutomatedRelistingRule; }
			set { ApiRequest.AutomatedRelistingRule = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetSellingManagerTemplateAutomationRuleRequestType.AutomatedSecondChanceOfferRule"/> of type <see cref="SellingManagerAutoSecondChanceOfferType"/>.
		/// </summary>
		public SellingManagerAutoSecondChanceOfferType AutomatedSecondChanceOfferRule
		{ 
			get { return ApiRequest.AutomatedSecondChanceOfferRule; }
			set { ApiRequest.AutomatedSecondChanceOfferRule = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="SetSellingManagerTemplateAutomationRuleResponseType.AutomatedListingRule"/> of type <see cref="SellingManagerAutoListType"/>.
		/// </summary>
		public SellingManagerAutoListType AutomatedListingRuleReturn
		{ 
			get { return ApiResponse.AutomatedListingRule; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="SetSellingManagerTemplateAutomationRuleResponseType.AutomatedRelistingRule"/> of type <see cref="SellingManagerAutoRelistType"/>.
		/// </summary>
		public SellingManagerAutoRelistType AutomatedRelistingRuleReturn
		{ 
			get { return ApiResponse.AutomatedRelistingRule; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="SetSellingManagerTemplateAutomationRuleResponseType.AutomatedSecondChanceOfferRule"/> of type <see cref="SellingManagerAutoSecondChanceOfferType"/>.
		/// </summary>
		public SellingManagerAutoSecondChanceOfferType AutomatedSecondChanceOfferRuleReturn
		{ 
			get { return ApiResponse.AutomatedSecondChanceOfferRule; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="SetSellingManagerTemplateAutomationRuleResponseType.Fees"/> of type <see cref="FeeTypeCollection"/>.
		/// </summary>
		public FeeTypeCollection FeeList
		{ 
			get { return ApiResponse.Fees; }
		}
		

		#endregion

		
	}
}
