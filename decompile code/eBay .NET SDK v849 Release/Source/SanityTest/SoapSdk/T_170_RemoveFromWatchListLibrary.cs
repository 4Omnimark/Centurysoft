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

namespace AllTestsSuite.T_050_ItemTestsSuite
{
	[TestFixture]
	public class T_170_RemoveFromWatchListLibrary : SOAPTestBase
	{
		[Test]
		public void RemoveFromWatchList()
		{
			ItemType watchListItem = TestData.WatchedItem;
			Assert.IsNotNull(watchListItem, "Failed because no category listings available -- requires successful GetCategoryListings test");
			RemoveFromWatchListCall api = new RemoveFromWatchListCall(this.apiContext);
			// Remove first one.
//			api.RemoveAllItems = true;
			api.ItemIDList = new StringCollection();
			api.ItemIDList.Add(watchListItem.ItemID);
			// Make API call,the api call will return the rest watchList count.
			int result = api.RemoveFromWatchList(api.ItemIDList);
			//check whether the call is success.
			Assert.IsTrue(api.ApiResponse.Ack==AckCodeType.Success || api.ApiResponse.Ack==AckCodeType.Warning,"do not success!");
			
		}
	}
}