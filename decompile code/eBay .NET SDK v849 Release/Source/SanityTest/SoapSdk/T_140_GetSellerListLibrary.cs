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
	public class T_140_GetSellerListLibrary : SOAPTestBase
	{
		[Test]
		public void GetSellerList()
		{
			Assert.IsNotNull(TestData.NewItem, "Failed because no item available -- requires successful AddItem test");
			//
			GetSellerListCall gsl = new GetSellerListCall(this.apiContext);
			DetailLevelCodeType[] detailLevels = new DetailLevelCodeType[] {
																			   DetailLevelCodeType.ReturnAll
																		   };
			gsl.DetailLevelList = new DetailLevelCodeTypeCollection(detailLevels);
			// Time filter
			System.DateTime calTo = System.DateTime.Now.AddHours(10);
			System.DateTime calFrom = System.DateTime.Now.AddHours(-20);
			TimeFilter tf = new TimeFilter(calFrom, calTo);
			gsl.EndTimeFilter = tf;
			// Pagination
			PaginationType pt = new PaginationType();
			pt.EntriesPerPage = 100; pt.EntriesPerPageSpecified = true;
			pt.PageNumber = 1; pt.PageNumberSpecified = true;
			gsl.Pagination = pt;
			//
			gsl.Execute();
			ItemTypeCollection items = gsl.ApiResponse.ItemArray;
			Assert.IsNotNull(items);
			Assert.IsTrue(items.Count > 0);
			ItemType foundItem = findItem(items, TestData.NewItem.ItemID);
			Assert.IsNotNull(foundItem, "item not found");
		}


		[Test]
		public void GetSellerListFull()
		{
			Assert.IsNotNull(TestData.NewItem2, "Failed because no item available -- requires successful AddItem test");
			GetSellerListCall gsl = new GetSellerListCall(this.apiContext);
			DetailLevelCodeType[] detailLevels = new DetailLevelCodeType[] {
																			   DetailLevelCodeType.ReturnAll
																		   };

			//specify information
			gsl.AdminEndedItemsOnly=false;
			gsl.CategoryID=int.Parse(TestData.NewItem2.PrimaryCategory.CategoryID);
			gsl.StartTimeFrom=DateTime.Now.AddDays(-2);
			gsl.StartTimeTo=DateTime.Now.AddDays(1);
			//gsl.GranularityLevel=GranularityLevelCodeType.Fine;//if specify GranularityLevel, the DetailLevelList is ignored.
			gsl.IncludeWatchCount=true;
			gsl.DetailLevelList = new DetailLevelCodeTypeCollection(detailLevels);
			// Pagination
			PaginationType pt = new PaginationType();
			pt.EntriesPerPage = 100; 
			pt.EntriesPerPageSpecified = true;
			pt.PageNumber = 1; 
			pt.PageNumberSpecified = true;
			gsl.Pagination = pt;
			gsl.Sort=1;//descending sort
			//
			gsl.Execute();
				
			//check whether the call is success.
			Assert.IsTrue(gsl.AbstractResponse.Ack==AckCodeType.Success || gsl.AbstractResponse.Ack==AckCodeType.Warning,"do not success!");
			ItemTypeCollection items = gsl.ApiResponse.ItemArray;
			Assert.IsNotNull(items);
			Assert.IsTrue(items.Count > 0);

			ItemType foundItem = findItem(items, TestData.NewItem2.ItemID);
			Assert.IsNotNull(foundItem, "item not found");
				
			ItemType item=items[0];
			Assert.IsNotNull(item.HitCount);
			Assert.IsNotNull(gsl.ApiResponse.PaginationResult);
			Assert.IsNotNull(gsl.ApiResponse.Seller);
		}		


		static ItemType findItem(ItemTypeCollection items, String itemID)
		{
			if( items == null )
				return null;
			for(int i = 0; i < items.Count; i++ )
			{
				ItemType item = items[i];
				if( item != null && item.ItemID != null && item.ItemID.Equals(itemID) )
					return item;
			}
			return null;
			
		}

	}
}