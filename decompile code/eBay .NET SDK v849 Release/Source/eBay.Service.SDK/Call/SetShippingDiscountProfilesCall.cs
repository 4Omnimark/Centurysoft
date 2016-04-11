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
	public class SetShippingDiscountProfilesCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public SetShippingDiscountProfilesCall()
		{
			ApiRequest = new SetShippingDiscountProfilesRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public SetShippingDiscountProfilesCall(ApiContext ApiContext)
		{
			ApiRequest = new SetShippingDiscountProfilesRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Enables a seller to define shipping cost discount profiles for things such as combined
		/// payments for shipping and handling costs.
		/// </summary>
		/// 
		/// <param name="CurrencyID">
		/// The three-digit code of the currency to be used for shipping cost discounts and
		/// insurance for combined payment orders. A discount profile can only be associated
		/// with a listing if the <b>CurrencyID</b> value of the profile matches the 
		/// <b>Item.Currency</b> value specified in a listing.
		/// 
		/// Required if the user creates flat rate shipping discount profiles, a promotional
		/// discount, a packaging/handling cost profile based on a variable
		/// discount rule, or if the user defines shipping insurance range/fee pairs.
		/// 
		/// Note: There is a currencyID attribute on many elements of SetShippingDiscountProfiles.
		/// To avoid an error, be sure to use the same <b>CurrencyID</b> value 
		/// in each occurrence within the same request.
		/// </param>
		///
		/// <param name="CombinedDuration">
		/// This field is used to specify the number of days after the sale of an
		/// item in which the buyer or seller can combine multiple and mutual order
		/// line items into one Combined Payment order. In a Combined Payment order,
		/// the buyer makes one payment for all order line items, hence only unpaid
		/// order line items can be combined into a Combined Payment order.
		/// </param>
		///
		/// <param name="ModifyActionCode">
		/// Indicates what action to take on the specified flat shipping discount,
		/// calculated shipping discount or promotional discount.
		/// If the action is Delete and if a flat rate or calculated shipping discount
		/// profile is specified, the discount profile identified by
		/// DiscountProfile.DiscountProfileID is deleted
		/// (see DiscountProfile.MappedDiscountProfileID for related details).
		/// </param>
		///
		/// <param name="FlatShippingDiscount">
		/// Details of a shipping cost discount profile for flat rate shipping.
		/// If this is provided, CalculatedShippingDiscount and PromotionalShippingDiscountDetails
		/// should be omitted.
		/// </param>
		///
		/// <param name="CalculatedShippingDiscount">
		/// Details of a shipping cost discount profile for calculated shipping.
		/// If this is provided, FlatShippingDiscount and PromotionalShippingDiscountDetails
		/// should be omitted.
		/// </param>
		///
		/// <param name="CalculatedHandlingDiscount">
		/// This container is used by the seller to specify/modify packaging and handling discounts that are applied 
		/// for combined payment orders.
		/// </param>
		///
		/// <param name="PromotionalShippingDiscountDetails">
		/// The data for the promotional shipping discount.
		/// If this is provided, FlatShippingDiscount and CalculatedShippingDiscount
		/// should be omitted.
		/// </param>
		///
		/// <param name="ShippingInsurance">
		/// Information establishing what fee to apply for domestic shipping insurance
		/// for combined payment depending on which range the order item-total price
		/// falls into.
		/// </param>
		///
		/// <param name="InternationalShippingInsurance">
		/// Information establishing what fee to apply for international shipping
		/// insurance for combined payment depending on which range the order item-total
		/// price falls into.
		/// </param>
		///
		public void SetShippingDiscountProfiles(CurrencyCodeType CurrencyID, CombinedPaymentPeriodCodeType CombinedDuration, ModifyActionCodeType ModifyActionCode, FlatShippingDiscountType FlatShippingDiscount, CalculatedShippingDiscountType CalculatedShippingDiscount, CalculatedHandlingDiscountType CalculatedHandlingDiscount, PromotionalShippingDiscountDetailsType PromotionalShippingDiscountDetails, ShippingInsuranceType ShippingInsurance, ShippingInsuranceType InternationalShippingInsurance)
		{
			this.CurrencyID = CurrencyID;
			this.CombinedDuration = CombinedDuration;
			this.ModifyActionCode = ModifyActionCode;
			this.FlatShippingDiscount = FlatShippingDiscount;
			this.CalculatedShippingDiscount = CalculatedShippingDiscount;
			this.CalculatedHandlingDiscount = CalculatedHandlingDiscount;
			this.PromotionalShippingDiscountDetails = PromotionalShippingDiscountDetails;
			this.ShippingInsurance = ShippingInsurance;
			this.InternationalShippingInsurance = InternationalShippingInsurance;

			Execute();
			
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
		/// Gets or sets the <see cref="SetShippingDiscountProfilesRequestType"/> for this API call.
		/// </summary>
		public SetShippingDiscountProfilesRequestType ApiRequest
		{ 
			get { return (SetShippingDiscountProfilesRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="SetShippingDiscountProfilesResponseType"/> for this API call.
		/// </summary>
		public SetShippingDiscountProfilesResponseType ApiResponse
		{ 
			get { return (SetShippingDiscountProfilesResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="SetShippingDiscountProfilesRequestType.CurrencyID"/> of type <see cref="CurrencyCodeType"/>.
		/// </summary>
		public CurrencyCodeType CurrencyID
		{ 
			get { return ApiRequest.CurrencyID; }
			set { ApiRequest.CurrencyID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetShippingDiscountProfilesRequestType.CombinedDuration"/> of type <see cref="CombinedPaymentPeriodCodeType"/>.
		/// </summary>
		public CombinedPaymentPeriodCodeType CombinedDuration
		{ 
			get { return ApiRequest.CombinedDuration; }
			set { ApiRequest.CombinedDuration = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetShippingDiscountProfilesRequestType.ModifyActionCode"/> of type <see cref="ModifyActionCodeType"/>.
		/// </summary>
		public ModifyActionCodeType ModifyActionCode
		{ 
			get { return ApiRequest.ModifyActionCode; }
			set { ApiRequest.ModifyActionCode = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetShippingDiscountProfilesRequestType.FlatShippingDiscount"/> of type <see cref="FlatShippingDiscountType"/>.
		/// </summary>
		public FlatShippingDiscountType FlatShippingDiscount
		{ 
			get { return ApiRequest.FlatShippingDiscount; }
			set { ApiRequest.FlatShippingDiscount = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetShippingDiscountProfilesRequestType.CalculatedShippingDiscount"/> of type <see cref="CalculatedShippingDiscountType"/>.
		/// </summary>
		public CalculatedShippingDiscountType CalculatedShippingDiscount
		{ 
			get { return ApiRequest.CalculatedShippingDiscount; }
			set { ApiRequest.CalculatedShippingDiscount = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetShippingDiscountProfilesRequestType.CalculatedHandlingDiscount"/> of type <see cref="CalculatedHandlingDiscountType"/>.
		/// </summary>
		public CalculatedHandlingDiscountType CalculatedHandlingDiscount
		{ 
			get { return ApiRequest.CalculatedHandlingDiscount; }
			set { ApiRequest.CalculatedHandlingDiscount = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetShippingDiscountProfilesRequestType.PromotionalShippingDiscountDetails"/> of type <see cref="PromotionalShippingDiscountDetailsType"/>.
		/// </summary>
		public PromotionalShippingDiscountDetailsType PromotionalShippingDiscountDetails
		{ 
			get { return ApiRequest.PromotionalShippingDiscountDetails; }
			set { ApiRequest.PromotionalShippingDiscountDetails = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetShippingDiscountProfilesRequestType.ShippingInsurance"/> of type <see cref="ShippingInsuranceType"/>.
		/// </summary>
		public ShippingInsuranceType ShippingInsurance
		{ 
			get { return ApiRequest.ShippingInsurance; }
			set { ApiRequest.ShippingInsurance = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetShippingDiscountProfilesRequestType.InternationalShippingInsurance"/> of type <see cref="ShippingInsuranceType"/>.
		/// </summary>
		public ShippingInsuranceType InternationalShippingInsurance
		{ 
			get { return ApiRequest.InternationalShippingInsurance; }
			set { ApiRequest.InternationalShippingInsurance = value; }
		}
		
		

		#endregion

		
	}
}
