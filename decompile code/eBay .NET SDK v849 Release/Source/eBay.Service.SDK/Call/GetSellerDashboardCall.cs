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
	public class GetSellerDashboardCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetSellerDashboardCall()
		{
			ApiRequest = new GetSellerDashboardRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetSellerDashboardCall(ApiContext ApiContext)
		{
			ApiRequest = new GetSellerDashboardRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Retrieves a brief summary of the requester's status as an eBay seller.
		/// </summary>
		/// 
		public SearchStandingDashboardType GetSellerDashboard()
		{

			Execute();
			return ApiResponse.SearchStanding;
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
		/// Gets or sets the <see cref="GetSellerDashboardRequestType"/> for this API call.
		/// </summary>
		public GetSellerDashboardRequestType ApiRequest
		{ 
			get { return (GetSellerDashboardRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetSellerDashboardResponseType"/> for this API call.
		/// </summary>
		public GetSellerDashboardResponseType ApiResponse
		{ 
			get { return (GetSellerDashboardResponseType) AbstractResponse; }
		}

		
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellerDashboardResponseType.SearchStanding"/> of type <see cref="SearchStandingDashboardType"/>.
		/// </summary>
		public SearchStandingDashboardType SearchStanding
		{ 
			get { return ApiResponse.SearchStanding; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellerDashboardResponseType.SellerFeeDiscount"/> of type <see cref="SellerFeeDiscountDashboardType"/>.
		/// </summary>
		public SellerFeeDiscountDashboardType SellerFeeDiscount
		{ 
			get { return ApiResponse.SellerFeeDiscount; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellerDashboardResponseType.PowerSellerStatus"/> of type <see cref="PowerSellerDashboardType"/>.
		/// </summary>
		public PowerSellerDashboardType PowerSellerStatus
		{ 
			get { return ApiResponse.PowerSellerStatus; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellerDashboardResponseType.PolicyCompliance"/> of type <see cref="PolicyComplianceDashboardType"/>.
		/// </summary>
		public PolicyComplianceDashboardType PolicyCompliance
		{ 
			get { return ApiResponse.PolicyCompliance; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellerDashboardResponseType.BuyerSatisfaction"/> of type <see cref="BuyerSatisfactionDashboardType"/>.
		/// </summary>
		public BuyerSatisfactionDashboardType BuyerSatisfaction
		{ 
			get { return ApiResponse.BuyerSatisfaction; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellerDashboardResponseType.SellerAccount"/> of type <see cref="SellerAccountDashboardType"/>.
		/// </summary>
		public SellerAccountDashboardType SellerAccount
		{ 
			get { return ApiResponse.SellerAccount; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellerDashboardResponseType.Performance"/> of type <see cref="PerformanceDashboardTypeCollection"/>.
		/// </summary>
		public PerformanceDashboardTypeCollection PerformanceList
		{ 
			get { return ApiResponse.Performance; }
		}
		

		#endregion

		
	}
}
