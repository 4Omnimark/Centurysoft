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
	public class T_370_SetPromotionalSaleListingsLibrary : SOAPTestBase
	{
		[Test]
		public void SetPromotionalSaleListings()
		{
			SetPromotionalSaleListingsCall api = new SetPromotionalSaleListingsCall(this.apiContext);
			ItemIDArrayType itemIDArrayType = new ItemIDArrayType();
			StringCollection strCollection = new StringCollection();
			strCollection.Add("445566778L");
			itemIDArrayType.ItemID = strCollection;
			try 
			{
				//add a param 'false' at the end by william, 3.13.2008
				PromotionalSaleStatusCodeType resp = 
					api.SetPromotionalSaleListings(16771004L, ModifyActionCodeType.Add,
												   itemIDArrayType, 234567890L, 12345L, true, false, false);
					
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