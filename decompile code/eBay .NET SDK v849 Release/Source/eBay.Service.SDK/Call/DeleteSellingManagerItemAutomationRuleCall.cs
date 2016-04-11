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
	public class DeleteSellingManagerItemAutomationRuleCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public DeleteSellingManagerItemAutomationRuleCall()
		{
			ApiRequest = new DeleteSellingManagerItemAutomationRuleRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public DeleteSellingManagerItemAutomationRuleCall(ApiContext ApiContext)
		{
			ApiRequest = new DeleteSellingManagerItemAutomationRuleRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Removes the association of Selling Manager automation rules
		/// to an item. Returns the remaining rules in the response.
		/// 
		/// This call is subject to change without notice; the
		/// deprecation process is inapplicable to this call.
		/// </summary>
		/// 
		/// <param name="ItemID">
		/// The ID of the item from which to delete automation rules.
		/// </param>
		///
		/// <param name="DeleteAutomatedRelistingRule">
		/// If true, the automated relisting rules are removed from the item.
		/// </param>
		///
		/// <param name="DeleteAutomatedSecondChanceOfferRule">
		/// If true, the automated second chance offer rule is removed from the item.
		/// </param>
		///
		public SellingManagerAutoListType DeleteSellingManagerItemAutomationRule(string ItemID, bool DeleteAutomatedRelistingRule, bool DeleteAutomatedSecondChanceOfferRule)
		{
			this.ItemID = ItemID;
			this.DeleteAutomatedRelistingRule = DeleteAutomatedRelistingRule;
			this.DeleteAutomatedSecondChanceOfferRule = DeleteAutomatedSecondChanceOfferRule;

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
		/// Gets or sets the <see cref="DeleteSellingManagerItemAutomationRuleRequestType"/> for this API call.
		/// </summary>
		public DeleteSellingManagerItemAutomationRuleRequestType ApiRequest
		{ 
			get { return (DeleteSellingManagerItemAutomationRuleRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="DeleteSellingManagerItemAutomationRuleResponseType"/> for this API call.
		/// </summary>
		public DeleteSellingManagerItemAutomationRuleResponseType ApiResponse
		{ 
			get { return (DeleteSellingManagerItemAutomationRuleResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="DeleteSellingManagerItemAutomationRuleRequestType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemID
		{ 
			get { return ApiRequest.ItemID; }
			set { ApiRequest.ItemID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="DeleteSellingManagerItemAutomationRuleRequestType.DeleteAutomatedRelistingRule"/> of type <see cref="bool"/>.
		/// </summary>
		public bool DeleteAutomatedRelistingRule
		{ 
			get { return ApiRequest.DeleteAutomatedRelistingRule; }
			set { ApiRequest.DeleteAutomatedRelistingRule = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="DeleteSellingManagerItemAutomationRuleRequestType.DeleteAutomatedSecondChanceOfferRule"/> of type <see cref="bool"/>.
		/// </summary>
		public bool DeleteAutomatedSecondChanceOfferRule
		{ 
			get { return ApiRequest.DeleteAutomatedSecondChanceOfferRule; }
			set { ApiRequest.DeleteAutomatedSecondChanceOfferRule = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="DeleteSellingManagerItemAutomationRuleResponseType.AutomatedListingRule"/> of type <see cref="SellingManagerAutoListType"/>.
		/// </summary>
		public SellingManagerAutoListType AutomatedListingRule
		{ 
			get { return ApiResponse.AutomatedListingRule; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="DeleteSellingManagerItemAutomationRuleResponseType.AutomatedRelistingRule"/> of type <see cref="SellingManagerAutoRelistType"/>.
		/// </summary>
		public SellingManagerAutoRelistType AutomatedRelistingRule
		{ 
			get { return ApiResponse.AutomatedRelistingRule; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="DeleteSellingManagerItemAutomationRuleResponseType.AutomatedSecondChanceOfferRule"/> of type <see cref="SellingManagerAutoSecondChanceOfferType"/>.
		/// </summary>
		public SellingManagerAutoSecondChanceOfferType AutomatedSecondChanceOfferRule
		{ 
			get { return ApiResponse.AutomatedSecondChanceOfferRule; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="DeleteSellingManagerItemAutomationRuleResponseType.Fees"/> of type <see cref="FeeTypeCollection"/>.
		/// </summary>
		public FeeTypeCollection FeeList
		{ 
			get { return ApiResponse.Fees; }
		}
		

		#endregion

		
	}
}
