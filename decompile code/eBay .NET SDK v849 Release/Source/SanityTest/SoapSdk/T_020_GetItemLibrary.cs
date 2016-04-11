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
using AllTestsSuite.T_050_ItemTestsSuite;
#endregion


namespace AllTestsSuite.T_050_ItemTestsSuite
{

	[TestFixture]
	public class T_020_GetItemLibrary : SOAPTestBase
	{
		/// <summary>
		/// The input object "IncludeExpressRequirements" is no longer supported and will be ignored.
		/// </summary>
		[Test]
		public void GetItem()
		{
			bool includeCrossPromotion=true;
			bool includeItemSpecifics=true;
			bool includeTaxTable=false;
			bool includeWatchCount=true;
			ItemType item = null;

			Assert.IsNotNull(TestData.NewItem2, "Failed because no item available -- requires successful AddItem test");
			
			GetItemCall api=new GetItemCall(this.apiContext);
			
			api.IncludeCrossPromotion = includeCrossPromotion;
			api.IncludeItemSpecifics = includeItemSpecifics;
			api.IncludeTaxTable = includeTaxTable;
			api.IncludeWatchCount = includeWatchCount;
			api.ItemID = TestData.NewItem2.ItemID;
			api.Execute();

			//check whether the call is success.
			Assert.IsTrue(api.ApiResponse.Ack==AckCodeType.Success || api.ApiResponse.Ack==AckCodeType.Warning,"do not success!");
			Assert.IsNotNull(api.ApiResponse.Item);
			item = api.ApiResponse.Item;
			Assert.IsTrue(item.ItemSpecifics.Count>0,"this is no item spcifics");
			Assert.IsTrue(item.WatchCount>=0);
		}

	}
}
		