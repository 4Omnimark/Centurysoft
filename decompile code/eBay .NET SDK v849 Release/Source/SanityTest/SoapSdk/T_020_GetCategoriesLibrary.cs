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

namespace AllTestsSuite.T_030_CategoryTestsSuite
{
	[TestFixture]
	public class T_020_GetCategoriesLibrary : SOAPTestBase
	{
		[Test]
		public void GetCategories()
		{
			bool isValid=true;
			GetCategoriesCall api = new GetCategoriesCall(this.apiContext);
			DetailLevelCodeType[] detailLevels = new DetailLevelCodeType[] {
			DetailLevelCodeType.ReturnAll
			};
			api.DetailLevelList = new DetailLevelCodeTypeCollection(detailLevels);
			api.LevelLimit = 1;
			api.ViewAllNodes = true;
			api.Timeout = 300000;
			//
			CategoryTypeCollection cats = api.GetCategories();
			//check whether the call is success.
			Assert.IsTrue(api.ApiResponse.Ack==AckCodeType.Success || api.ApiResponse.Ack==AckCodeType.Warning,"do not success!");
			Assert.IsNotNull(cats);
			Assert.IsTrue(cats.Count > 0);

			//the return category's level must be 1.
			foreach(CategoryType category in cats)
			{
				if(category.CategoryLevel!=1)
				{
					isValid=false;
					break;
				}
			}

			Assert.IsTrue(isValid,"the return value is not valid");
			// Save the result.
			TestData.Categories = cats;
			
		}

	}
}