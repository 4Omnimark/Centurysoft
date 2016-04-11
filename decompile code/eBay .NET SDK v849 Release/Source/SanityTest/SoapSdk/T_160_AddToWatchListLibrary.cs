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
	public class T_160_AddToWatchListLibrary : SOAPTestBase
	{
		public static ItemType watchListItem = null;

		[Test]
		public void AddToWatchListFull()
		{
			watchListItem =	AddItem();
			Assert.IsNotNull(watchListItem, "Failed because failed to add item");
			Assert.AreNotEqual(watchListItem.ItemID,string.Empty);

			TestData.WatchedItem = watchListItem;
			AddToWatchListCall api = new AddToWatchListCall(this.apiContext);
			// Watch the first one.
			StringCollection ids = new StringCollection(); 
			ids.Add (watchListItem.ItemID);
			// Make API call.
			int num = api.AddToWatchList(ids, null);
			//check whether the call is success.
			Assert.IsTrue(api.ApiResponse.Ack==AckCodeType.Success || api.ApiResponse.Ack==AckCodeType.Warning,"do not success!");
			Assert.Greater(num,0);
			Assert.Greater(api.ApiResponse.WatchListCount,0);
			Assert.Greater(api.ApiResponse.WatchListMaximum,0);
		}

		private ItemType  AddItem()
		{
			ItemType item = ItemHelper.BuildItem();
			// Execute the API.
			FeeTypeCollection fees;
			// AddItem
			AddItemCall addItem = new AddItemCall(this.apiContext);
			fees = addItem.AddItem(item);
			Assert.IsNotNull(fees);
			// Save the result.
			return item;
		}

	}
}