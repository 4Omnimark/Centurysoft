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
using NUnit.Framework;
using eBay.Service.Call;
using eBay.Service.Core.Soap;
using eBay.Service.Core.Sdk;
using eBay.Service.Util;
#endregion

namespace AllTestsSuite.T_020_OtherTestsSuite
{
	[TestFixture]
	public class T_380_SetShippingDiscountProfilesLibrary : SOAPTestBase
	{
		[Test]
		public void SetShippingDiscountProfilesLibrary() {
			SetShippingDiscountProfilesCall api = new SetShippingDiscountProfilesCall(apiContext);
			CalculatedHandlingDiscountType calcHandlingDiscount = new CalculatedHandlingDiscountType();
			calcHandlingDiscount.DiscountName = HandlingNameCodeType.IndividualHandlingFee;
			AmountType amount = new AmountType();
			amount.Value = 123.45;
			calcHandlingDiscount.OrderHandlingAmount = amount;
			FlatShippingDiscountType flatShippingDiscount = new FlatShippingDiscountType();
			flatShippingDiscount.DiscountName = DiscountNameCodeType.IndividualItemWeight;
			//
			CalculatedShippingDiscountType calcShippingDiscount = new CalculatedShippingDiscountType();
			calcShippingDiscount.DiscountName = DiscountNameCodeType.EachAdditionalAmount;
			ShippingInsuranceType shipInsurType = new ShippingInsuranceType();
			shipInsurType.InsuranceOption = InsuranceOptionCodeType.Optional;
			ShippingInsuranceType internInsurType = shipInsurType;
			PromotionalShippingDiscountDetailsType promoShippingDiscountDetails = new PromotionalShippingDiscountDetailsType();
			promoShippingDiscountDetails.DiscountName = DiscountNameCodeType.MaximumShippingCostPerOrder;

			try 
			{
				api.SetShippingDiscountProfiles(CurrencyCodeType.USD, CombinedPaymentPeriodCodeType.Days_14,
												ModifyActionCodeType.Add, flatShippingDiscount, calcShippingDiscount,
												calcHandlingDiscount, promoShippingDiscountDetails, 
												shipInsurType, internInsurType);
			} 
			catch(ApiException apie) 
			{
				Console.WriteLine("ApiException: " + apie.Message); 
			} 
			catch(SdkException sdke) 
			{
				Assert.Fail("SdkException: " + sdke.Message);
			}
			
		}
	}
}