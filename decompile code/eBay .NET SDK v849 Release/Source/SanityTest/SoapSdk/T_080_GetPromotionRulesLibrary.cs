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
	public class T_080_GetPromotionRulesLibrary : SOAPTestBase
	{
		[Test]
		public void GetPromotionRules()
		{
			GetPromotionRulesCall api = new GetPromotionRulesCall(this.apiContext);
			api.PromotionMethod = PromotionMethodCodeType.CrossSell;
			api.StoreCategoryID = 1;
			// Make API call.
			PromotionRuleTypeCollection rules = api.GetPromotionRules(api.ItemID, api.PromotionMethod);
			// Verify the result.
			Assert.IsNotNull(rules);
		}

		
		[Test]
		public void GetPromotionRulesFull()
		{
			bool isTherePropertyNull;
			int nullPropertyNums;
			string nullPropertyNames;

			Assert.IsNotNull(TestData.NewItem2, "Failed because no item available -- requires successful AddItem test");
			GetPromotionRulesCall api = new GetPromotionRulesCall(this.apiContext);
			
			string itemID = TestData.NewItem2.ItemID;
			PromotionMethodCodeType promotionType= PromotionMethodCodeType.UpSell;
			PromotionRuleTypeCollection rules = api.GetPromotionRules(itemID,promotionType);
			//check whether the call is success.
			Assert.IsTrue(api.ApiResponse.Ack==AckCodeType.Success || api.ApiResponse.Ack==AckCodeType.Warning,"do not success!");

			Assert.IsNotNull(rules);
			if(rules!=null && rules.Count>0)
			{
				isTherePropertyNull=ReflectHelper.IsProperteValueNotNull(rules[0],out nullPropertyNums,out nullPropertyNames);
				Assert.IsTrue(isTherePropertyNull,"there are" +nullPropertyNums.ToString()+ " properties(" +nullPropertyNames+ ")value is null");
			}
			
		}
	}
}