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

namespace AllTestsSuite.T_100_StoreTestsSuite
{
	[TestFixture]
	public class T_090_SetStoreCategoriesLibrary : SOAPTestBase
	{
		[Test]
		public void SetStoreCategories()
		{
			// Skip if the user is not store enabled.
			if (TestData.Store == null)
			return;
			SetStoreCategoriesCall api = new SetStoreCategoriesCall(this.apiContext);
			// Build the StoreType object.
			StoreType st = new StoreType();
			st.Description = TestData.Store.Description;
			st.Logo = TestData.Store.Logo;
			st.MerchDisplay = TestData.Store.MerchDisplay;
			st.Name = TestData.Store.Name;
			api.Action = StoreCategoryUpdateActionCodeType.Add;
			api.DestinationParentCategoryID = 1234;
			api.ItemDestinationCategoryID = 4321;
			StoreCustomCategoryTypeCollection catArray = new StoreCustomCategoryTypeCollection();
			api.StoreCategoryList = catArray;
			StoreCustomCategoryType cat = new StoreCustomCategoryType();
			catArray.Add(cat);
			cat.CategoryID = 102;
			cat.Name = "TestStoreCategory";
			// Make API call.
			ApiException gotException = null;
			// Negative test.
			try 
			{
				api.Execute();
			} 
			catch (ApiException ex) 
			{
				gotException = ex;
			}
			Assert.IsNotNull(gotException);		
		}
	}
}