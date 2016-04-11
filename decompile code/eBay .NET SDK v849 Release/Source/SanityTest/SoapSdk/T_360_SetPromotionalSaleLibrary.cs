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
	public class T_360_SetPromotionalSaleLibrary : SOAPTestBase
	{
		[Test]
		public void SetPromotionalSale()
		{
			SetPromotionalSaleCall api = new SetPromotionalSaleCall(this.apiContext);
			api.Site = SiteCodeType.US;
			PromotionalSaleType promoSaleType = new PromotionalSaleType();
			promoSaleType.DiscountType = DiscountCodeType.Price;
			promoSaleType.DiscountValue = 123.45;
			promoSaleType.PromotionalSaleStartTime = DateTime.Now.AddDays(3);
			promoSaleType.PromotionalSaleEndTime = DateTime.Now.AddDays(10);
			promoSaleType.PromotionalSaleID = 1234567890;
			promoSaleType.PromotionalSaleName = "Promo Sale";
			try 
			{
				PromotionalSaleStatusCodeType resp = api.SetPromotionalSale(ModifyActionCodeType.Add, promoSaleType);
				Assert.IsNotNull(resp);
				Console.WriteLine("T_360_SetPromotionalSaleLibrary: " + resp.ToString());
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