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
	public class SetPromotionalSaleCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public SetPromotionalSaleCall()
		{
			ApiRequest = new SetPromotionalSaleRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public SetPromotionalSaleCall(ApiContext ApiContext)
		{
			ApiRequest = new SetPromotionalSaleRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Creates or modifies a promotional sale. Promotional sales enable sellers
		/// to apply discounts and/or free shipping across many listings.
		/// </summary>
		/// 
		/// <param name="Action">
		/// The seller must include this field and set it to 'Add' to create a new 
		/// promotional sale, or set it to 'Update' to modify an existing promotional sale, 
		/// or set it to 'Delete' to delete a promotional sale.
		/// </param>
		///
		/// <param name="PromotionalSaleDetails">
		/// This container must be included in each <b>SetPromotionalSale</b> call. 
		/// The fields of this container that will be used will depend on whether the seller is 
		/// adding a new promotional sale, updating an existing promotional sale, or deleting an 
		/// existing promotional sale.
		/// </param>
		///
		public PromotionalSaleStatusCodeType SetPromotionalSale(ModifyActionCodeType Action, PromotionalSaleType PromotionalSaleDetails)
		{
			this.Action = Action;
			this.PromotionalSaleDetails = PromotionalSaleDetails;

			Execute();
			return ApiResponse.Status;
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
		/// Gets or sets the <see cref="SetPromotionalSaleRequestType"/> for this API call.
		/// </summary>
		public SetPromotionalSaleRequestType ApiRequest
		{ 
			get { return (SetPromotionalSaleRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="SetPromotionalSaleResponseType"/> for this API call.
		/// </summary>
		public SetPromotionalSaleResponseType ApiResponse
		{ 
			get { return (SetPromotionalSaleResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="SetPromotionalSaleRequestType.Action"/> of type <see cref="ModifyActionCodeType"/>.
		/// </summary>
		public ModifyActionCodeType Action
		{ 
			get { return ApiRequest.Action; }
			set { ApiRequest.Action = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetPromotionalSaleRequestType.PromotionalSaleDetails"/> of type <see cref="PromotionalSaleType"/>.
		/// </summary>
		public PromotionalSaleType PromotionalSaleDetails
		{ 
			get { return ApiRequest.PromotionalSaleDetails; }
			set { ApiRequest.PromotionalSaleDetails = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="SetPromotionalSaleResponseType.Status"/> of type <see cref="PromotionalSaleStatusCodeType"/>.
		/// </summary>
		public PromotionalSaleStatusCodeType Status
		{ 
			get { return ApiResponse.Status; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="SetPromotionalSaleResponseType.PromotionalSaleID"/> of type <see cref="long"/>.
		/// </summary>
		public long PromotionalSaleID
		{ 
			get { return ApiResponse.PromotionalSaleID; }
		}
		

		#endregion

		
	}
}
