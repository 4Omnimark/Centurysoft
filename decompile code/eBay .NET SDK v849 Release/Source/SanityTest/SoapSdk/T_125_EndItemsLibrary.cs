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
using System.Xml;
using System.IO;
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
	public class T_125_EndItemsLibrary: SOAPTestBase
	{
		[Test]
		public void EndItems()
		{
			Assert.IsNotNull(TestData.itemIds);
			Assert.IsTrue(TestData.itemIds.Count>0);

			EndItemsCall api = new EndItemsCall(this.apiContext);
			EndItemRequestContainerTypeCollection endItemsContainers=new EndItemRequestContainerTypeCollection();
			EndItemRequestContainerType container;
			foreach(String itemID in TestData.itemIds)
			{
				container=new EndItemRequestContainerType();
				container.ItemID=itemID;
				container.EndingReason=EndReasonCodeType.LostOrBroken;
				container.MessageID=Convert.ToString(endItemsContainers.Count+1);
				endItemsContainers.Add(container);
			}
			api.EndItemRequestContainerList=endItemsContainers;
			api.Execute();
			
			//check whether the call is success.
			Assert.IsTrue(api.ApiResponse.Ack==AckCodeType.Success || api.ApiResponse.Ack==AckCodeType.Warning,"do not success!");
			Assert.IsNotNull(api.EndItemResponseContainerList);
			Assert.IsTrue(api.EndItemResponseContainerList.Count==TestData.itemIds.Count);
			TestData.itemIds=null;
		}
	}
}
