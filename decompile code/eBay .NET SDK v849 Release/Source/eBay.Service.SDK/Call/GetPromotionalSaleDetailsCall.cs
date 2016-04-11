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
	public class GetPromotionalSaleDetailsCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetPromotionalSaleDetailsCall()
		{
			ApiRequest = new GetPromotionalSaleDetailsRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetPromotionalSaleDetailsCall(ApiContext ApiContext)
		{
			ApiRequest = new GetPromotionalSaleDetailsRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Retrieves information about promotional sales set up by an eBay store owner
		/// (the authenticated caller).
		/// </summary>
		/// 
		/// <param name="PromotionalSaleID">
		/// The ID of the promotional sale about which you want information. If you do
		/// not specify this field, then all promotional sales for the seller making
		/// the call are returned or only those promotional sales matching the
		/// specified promotional sale status filter, PromotionalSaleStatus.
		/// 
		/// If PromotionalSaleID and PromotionalSaleStatus are both specified, the
		/// single promotional sale specified by ID is returned only if its status
		/// matches the specified status filter.
		/// </param>
		///
		/// <param name="PromotionalSaleStatuList">
		/// Specifies the promotional sales to return, based upon their status. For
		/// example, specify "Scheduled" to retrieve only promotional sales with a
		/// Status of Scheduled. If you want to retrieve promotional sales for more
		/// than one status, you can repeat the field with an additional status value,
		/// such as Active.
		/// 
		/// If this field is used together with PromotionalSaleID, the single
		/// promotional sale specified by ID is returned only if its status
		/// matches the specified status filter.
		/// 
		/// If neither field is used, all of the seller's promotional sales are
		/// returned, regardless of status.
		/// </param>
		///
		public PromotionalSaleTypeCollection GetPromotionalSaleDetails(long PromotionalSaleID, PromotionalSaleStatusCodeTypeCollection PromotionalSaleStatuList)
		{
			this.PromotionalSaleID = PromotionalSaleID;
			this.PromotionalSaleStatuList = PromotionalSaleStatuList;

			Execute();
			return ApiResponse.PromotionalSaleDetails;
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
		/// Gets or sets the <see cref="GetPromotionalSaleDetailsRequestType"/> for this API call.
		/// </summary>
		public GetPromotionalSaleDetailsRequestType ApiRequest
		{ 
			get { return (GetPromotionalSaleDetailsRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetPromotionalSaleDetailsResponseType"/> for this API call.
		/// </summary>
		public GetPromotionalSaleDetailsResponseType ApiResponse
		{ 
			get { return (GetPromotionalSaleDetailsResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetPromotionalSaleDetailsRequestType.PromotionalSaleID"/> of type <see cref="long"/>.
		/// </summary>
		public long PromotionalSaleID
		{ 
			get { return ApiRequest.PromotionalSaleID; }
			set { ApiRequest.PromotionalSaleID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetPromotionalSaleDetailsRequestType.PromotionalSaleStatus"/> of type <see cref="PromotionalSaleStatusCodeTypeCollection"/>.
		/// </summary>
		public PromotionalSaleStatusCodeTypeCollection PromotionalSaleStatuList
		{ 
			get { return ApiRequest.PromotionalSaleStatus; }
			set { ApiRequest.PromotionalSaleStatus = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetPromotionalSaleDetailsResponseType.PromotionalSaleDetails"/> of type <see cref="PromotionalSaleTypeCollection"/>.
		/// </summary>
		public PromotionalSaleTypeCollection PromotionalSaleDetailList
		{ 
			get { return ApiResponse.PromotionalSaleDetails; }
		}
		

		#endregion

		
	}
}
