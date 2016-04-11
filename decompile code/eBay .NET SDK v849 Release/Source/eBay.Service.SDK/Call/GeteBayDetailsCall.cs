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
	public class GeteBayDetailsCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GeteBayDetailsCall()
		{
			ApiRequest = new GeteBayDetailsRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GeteBayDetailsCall(ApiContext ApiContext)
		{
			ApiRequest = new GeteBayDetailsRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Retrieves eBay IDs and codes (e.g., site IDs and shipping service
		/// codes), enumerated data (e.g., payment methods), and other common eBay
		/// meta-data. This call enables you to keep certain data up to date in your
		/// applications without referring to the schema, the documentation, or the
		/// eBay online help. Other data is returned for your reference, but you may
		/// need to refer to the schema or the documentation for information about
		/// valid values and usage.
		/// 
		/// In some cases, the data returned in the response will vary according to
		/// the site that you use for the request.
		/// 
		/// If you use GeteBayDetails in preparation for listing in the US Motors Parts
		/// and Accessories categories, use site ID 0 (which is the site ID of the US
		/// site) when you call GeteBayDetails.
		/// 
		/// Sellers who engage in cross-border trade on sites that require a recoupment agreement, must agree to the
		/// recoupment terms before adding items to the site. This agreement allows eBay to reimburse
		/// a buyer during a dispute and then recoup the cost from the seller. Information about whether a site
		/// is a recoupment site is returned in the GeteBayDetailsResponse.RecoupmentPolicyDetails container.
		/// </summary>
		/// 
		/// <param name="DetailNameList">
		/// A designation of what kind of information you wish returned for the
		/// specified eBay site. If omitted, all details are returned. The
		/// possible values for input (the enumeration values of
		/// DetailNameCodeType) are the same name as fields returned by the
		/// response. See the documentation for the GeteBayDetails response to
		/// better understand the DetailName options.
		/// </param>
		///
		public void GeteBayDetails(DetailNameCodeTypeCollection DetailNameList)
		{
			this.DetailNameList = DetailNameList;

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
		/// Gets or sets the <see cref="GeteBayDetailsRequestType"/> for this API call.
		/// </summary>
		public GeteBayDetailsRequestType ApiRequest
		{ 
			get { return (GeteBayDetailsRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GeteBayDetailsResponseType"/> for this API call.
		/// </summary>
		public GeteBayDetailsResponseType ApiResponse
		{ 
			get { return (GeteBayDetailsResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GeteBayDetailsRequestType.DetailName"/> of type <see cref="DetailNameCodeTypeCollection"/>.
		/// </summary>
		public DetailNameCodeTypeCollection DetailNameList
		{ 
			get { return ApiRequest.DetailName; }
			set { ApiRequest.DetailName = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GeteBayDetailsResponseType.CountryDetails"/> of type <see cref="CountryDetailsTypeCollection"/>.
		/// </summary>
		public CountryDetailsTypeCollection CountryDetailList
		{ 
			get { return ApiResponse.CountryDetails; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GeteBayDetailsResponseType.CurrencyDetails"/> of type <see cref="CurrencyDetailsTypeCollection"/>.
		/// </summary>
		public CurrencyDetailsTypeCollection CurrencyDetailList
		{ 
			get { return ApiResponse.CurrencyDetails; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GeteBayDetailsResponseType.DispatchTimeMaxDetails"/> of type <see cref="DispatchTimeMaxDetailsTypeCollection"/>.
		/// </summary>
		public DispatchTimeMaxDetailsTypeCollection DispatchTimeMaxDetailList
		{ 
			get { return ApiResponse.DispatchTimeMaxDetails; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GeteBayDetailsResponseType.PaymentOptionDetails"/> of type <see cref="PaymentOptionDetailsTypeCollection"/>.
		/// </summary>
		public PaymentOptionDetailsTypeCollection PaymentOptionDetailList
		{ 
			get { return ApiResponse.PaymentOptionDetails; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GeteBayDetailsResponseType.RegionDetails"/> of type <see cref="RegionDetailsTypeCollection"/>.
		/// </summary>
		public RegionDetailsTypeCollection RegionDetailList
		{ 
			get { return ApiResponse.RegionDetails; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GeteBayDetailsResponseType.ShippingLocationDetails"/> of type <see cref="ShippingLocationDetailsTypeCollection"/>.
		/// </summary>
		public ShippingLocationDetailsTypeCollection ShippingLocationDetailList
		{ 
			get { return ApiResponse.ShippingLocationDetails; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GeteBayDetailsResponseType.ShippingServiceDetails"/> of type <see cref="ShippingServiceDetailsTypeCollection"/>.
		/// </summary>
		public ShippingServiceDetailsTypeCollection ShippingServiceDetailList
		{ 
			get { return ApiResponse.ShippingServiceDetails; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GeteBayDetailsResponseType.SiteDetails"/> of type <see cref="SiteDetailsTypeCollection"/>.
		/// </summary>
		public SiteDetailsTypeCollection SiteDetailList
		{ 
			get { return ApiResponse.SiteDetails; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GeteBayDetailsResponseType.TaxJurisdiction"/> of type <see cref="TaxJurisdictionTypeCollection"/>.
		/// </summary>
		public TaxJurisdictionTypeCollection TaxJurisdictionList
		{ 
			get { return ApiResponse.TaxJurisdiction; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GeteBayDetailsResponseType.URLDetails"/> of type <see cref="URLDetailsTypeCollection"/>.
		/// </summary>
		public URLDetailsTypeCollection URLDetailList
		{ 
			get { return ApiResponse.URLDetails; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GeteBayDetailsResponseType.TimeZoneDetails"/> of type <see cref="TimeZoneDetailsTypeCollection"/>.
		/// </summary>
		public TimeZoneDetailsTypeCollection TimeZoneDetailList
		{ 
			get { return ApiResponse.TimeZoneDetails; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GeteBayDetailsResponseType.ItemSpecificDetails"/> of type <see cref="ItemSpecificDetailsTypeCollection"/>.
		/// </summary>
		public ItemSpecificDetailsTypeCollection ItemSpecificDetailList
		{ 
			get { return ApiResponse.ItemSpecificDetails; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GeteBayDetailsResponseType.UnitOfMeasurementDetails"/> of type <see cref="UnitOfMeasurementDetailsTypeCollection"/>.
		/// </summary>
		public UnitOfMeasurementDetailsTypeCollection UnitOfMeasurementDetailList
		{ 
			get { return ApiResponse.UnitOfMeasurementDetails; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GeteBayDetailsResponseType.RegionOfOriginDetails"/> of type <see cref="RegionOfOriginDetailsTypeCollection"/>.
		/// </summary>
		public RegionOfOriginDetailsTypeCollection RegionOfOriginDetailList
		{ 
			get { return ApiResponse.RegionOfOriginDetails; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GeteBayDetailsResponseType.ShippingPackageDetails"/> of type <see cref="ShippingPackageDetailsTypeCollection"/>.
		/// </summary>
		public ShippingPackageDetailsTypeCollection ShippingPackageDetailList
		{ 
			get { return ApiResponse.ShippingPackageDetails; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GeteBayDetailsResponseType.ShippingCarrierDetails"/> of type <see cref="ShippingCarrierDetailsTypeCollection"/>.
		/// </summary>
		public ShippingCarrierDetailsTypeCollection ShippingCarrierDetailList
		{ 
			get { return ApiResponse.ShippingCarrierDetails; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GeteBayDetailsResponseType.ReturnPolicyDetails"/> of type <see cref="ReturnPolicyDetailsType"/>.
		/// </summary>
		public ReturnPolicyDetailsType ReturnPolicyDetails
		{ 
			get { return ApiResponse.ReturnPolicyDetails; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GeteBayDetailsResponseType.ListingStartPriceDetails"/> of type <see cref="ListingStartPriceDetailsTypeCollection"/>.
		/// </summary>
		public ListingStartPriceDetailsTypeCollection ListingStartPriceDetailList
		{ 
			get { return ApiResponse.ListingStartPriceDetails; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GeteBayDetailsResponseType.BuyerRequirementDetails"/> of type <see cref="SiteBuyerRequirementDetailsTypeCollection"/>.
		/// </summary>
		public SiteBuyerRequirementDetailsTypeCollection BuyerRequirementDetailList
		{ 
			get { return ApiResponse.BuyerRequirementDetails; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GeteBayDetailsResponseType.ListingFeatureDetails"/> of type <see cref="ListingFeatureDetailsTypeCollection"/>.
		/// </summary>
		public ListingFeatureDetailsTypeCollection ListingFeatureDetailList
		{ 
			get { return ApiResponse.ListingFeatureDetails; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GeteBayDetailsResponseType.VariationDetails"/> of type <see cref="VariationDetailsType"/>.
		/// </summary>
		public VariationDetailsType VariationDetails
		{ 
			get { return ApiResponse.VariationDetails; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GeteBayDetailsResponseType.ExcludeShippingLocationDetails"/> of type <see cref="ExcludeShippingLocationDetailsTypeCollection"/>.
		/// </summary>
		public ExcludeShippingLocationDetailsTypeCollection ExcludeShippingLocationDetailList
		{ 
			get { return ApiResponse.ExcludeShippingLocationDetails; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GeteBayDetailsResponseType.UpdateTime"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime UpdateTime
		{ 
			get { return ApiResponse.UpdateTime; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GeteBayDetailsResponseType.RecoupmentPolicyDetails"/> of type <see cref="RecoupmentPolicyDetailsTypeCollection"/>.
		/// </summary>
		public RecoupmentPolicyDetailsTypeCollection RecoupmentPolicyDetailList
		{ 
			get { return ApiResponse.RecoupmentPolicyDetails; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GeteBayDetailsResponseType.ShippingCategoryDetails"/> of type <see cref="ShippingCategoryDetailsTypeCollection"/>.
		/// </summary>
		public ShippingCategoryDetailsTypeCollection ShippingCategoryDetailList
		{ 
			get { return ApiResponse.ShippingCategoryDetails; }
		}
		

		#endregion

		
	}
}
