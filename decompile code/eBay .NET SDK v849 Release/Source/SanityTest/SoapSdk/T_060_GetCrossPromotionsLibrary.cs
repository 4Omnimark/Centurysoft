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
using UnitTests.Helper;
#endregion

namespace AllTestsSuite.T_050_ItemTestsSuite
{
	[TestFixture]
	public class T_060_GetCrossPromotionsLibrary : SOAPTestBase
	{
		[Test]
		public void GetCrossPromotions()
		{
			Assert.IsNotNull(TestData.NewItem, "Failed because no item available -- requires successful AddItem test");
			GetCrossPromotionsCall api = new GetCrossPromotionsCall(this.apiContext);
			api.ItemID = TestData.NewItem.ItemID;
			api.PromotionMethod = PromotionMethodCodeType.CrossSell;
			api.PromotionViewMode = TradingRoleCodeType.Seller;
			// Make API call.
			api.Execute();
			CrossPromotionsType cr = api.ApiResponse.CrossPromotion;
			
		}

		[Test]
		public void GetCrossPromotionsFull()
		{
			Assert.IsNotNull(TestData.NewItem2, "Failed because no item available -- requires successful AddItem test");
			GetCrossPromotionsCall api = new GetCrossPromotionsCall(this.apiContext);
			api.ItemID = TestData.NewItem2.ItemID;
			api.PromotionMethod = PromotionMethodCodeType.CrossSell;
			api.PromotionViewMode = TradingRoleCodeType.Seller;
			// Make API call.
			api.Execute();
			//check whether the call is success.
			Assert.IsTrue(api.ApiResponse.Ack==AckCodeType.Success || api.ApiResponse.Ack==AckCodeType.Warning,"do not success!");
			CrossPromotionsType cr = api.ApiResponse.CrossPromotion;
			
			Assert.IsNotNull(cr);
			
			//the following item could not be null
			Assert.IsNotNull(cr.ItemID);
			if (cr.PromotedItem!=null&&cr.PromotedItem.Count>0)
			{
				Assert.IsNotNull(cr.PromotedItem[0].ListingType);
				Assert.IsNotNull(cr.PromotedItem[0].Position);
				Assert.IsNotNull(cr.PromotedItem[0].PromotionDetails);
				if (cr.PromotedItem[0].PromotionDetails.Count>0)
				{
					Assert.IsNotNull(cr.PromotedItem[0].PromotionDetails[0].PromotionPrice); 
					Assert.IsNotNull(cr.PromotedItem[0].PromotionDetails[0].PromotionPriceType); 
				}
				Assert.IsNotNull(cr.PromotedItem[0].SelectionType);
				Assert.IsNotNull(cr.PromotedItem[0].TimeLeft);
				Assert.IsNotNull(cr.PromotedItem[0].Title);
			}
			Assert.IsNotNull(cr.PromotionMethod);
			Assert.IsNotNull(cr.SellerID);
			Assert.IsNotNull(cr.ShippingDiscount);
		}

	}
}