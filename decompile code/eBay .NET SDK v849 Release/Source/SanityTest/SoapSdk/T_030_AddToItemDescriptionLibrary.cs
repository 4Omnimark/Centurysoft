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
	public class T_030_AddToItemDescriptionLibrary : SOAPTestBase
	{
		[Test]
		public void AddToItemDescription()
		{
			string message,description,itemID,originalDes;
			ItemType item;
			bool isSuccess;

			Assert.IsNotNull(TestData.NewItem2, "Failed because no item available -- requires successful AddItem test");
			AddToItemDescriptionCall api = new AddToItemDescriptionCall(this.apiContext);
			itemID = TestData.NewItem2.ItemID;
			originalDes = TestData.NewItem2.Description;
			description="SDK appended text.";
			
			DetailLevelCodeTypeCollection detailLevel=new DetailLevelCodeTypeCollection();
			DetailLevelCodeType type=DetailLevelCodeType.ReturnAll;
			detailLevel.Add(type);
			api.DetailLevelList=detailLevel;
			// Make API call.
			api.AddToItemDescription(itemID,description);

			System.Threading.Thread.Sleep(3000);
			//check whether the call is success.
			Assert.IsTrue(api.ApiResponse.Ack==AckCodeType.Success || api.ApiResponse.Ack==AckCodeType.Warning,"do not success!");
			isSuccess=ItemHelper.GetItem(TestData.NewItem2,this.apiContext,out message,out item);
			Assert.IsTrue(isSuccess,message);
			Assert.IsTrue(message==string.Empty,message);
			
			Assert.IsNotNull(item.Description,"the item description is null");
			Assert.Greater(item.Description.Length,originalDes.Length);
		}
	}
}