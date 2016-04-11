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
	public class T_150_GetAdFormatLeadsLibrary : SOAPTestBase
	{
		[Test]
		public void GetAdFormatLeads()
		{
			ItemType itemTest = TestData.NewItem;
			Assert.IsNotNull(itemTest);
			//
			GetAdFormatLeadsCall api = new GetAdFormatLeadsCall(this.apiContext);
			DetailLevelCodeType[] detailLevels = new DetailLevelCodeType[] {
																			   DetailLevelCodeType.ReturnAll
																		   };
			api.DetailLevelList = new DetailLevelCodeTypeCollection(detailLevels);
			api.ItemID = itemTest.ItemID;
			// Negative test.
			ApiException gotException = null;
			try
			{
				api.GetAdFormatLeads(api.ItemID);
			}
			catch(ApiException ex)
			{
				gotException = ex;
			}
			Assert.IsNotNull(gotException);
			Assert.AreEqual(gotException.Errors[0].ErrorCode, "580");
			
		}

		/// <summary>
		/// can not get a category accordling to my function.
		///otherwise ,there is an effeciency problem.
		///so, Comment it.
		/// </summary>
		//[Test]
		public void GetAdFormatLeadsFull()
		{
			TestData.AdFormatItem = addAdFormatItem();
			ItemType item = TestData.AdFormatItem;
			Assert.IsNotNull(item);
			//
			GetAdFormatLeadsCall api = new GetAdFormatLeadsCall(this.apiContext);
			DetailLevelCodeType[] detailLevels = new DetailLevelCodeType[] {
			DetailLevelCodeType.ReturnAll};
			api.DetailLevelList = new DetailLevelCodeTypeCollection(detailLevels);
			string itemID = item.ItemID;
			bool ncludeMemberMessages=true;
			MessageStatusTypeCodeType status= MessageStatusTypeCodeType.Unanswered;
			DateTime startCreationTime = DateTime.Now.AddDays(-1);
			DateTime endCreationTime  = DateTime.Now;
			AdFormatLeadTypeCollection adFormat=  api.GetAdFormatLeads(itemID,status,
				ncludeMemberMessages,startCreationTime,endCreationTime);

			//check whether the call is success.
			Assert.IsTrue(api.ApiResponse.Ack==AckCodeType.Success || api.ApiResponse.Ack==AckCodeType.Warning,"do not success!");
			Assert.IsNotNull(adFormat);
		}

		
		private ItemType  addAdFormatItem()
		{
			ItemType item = ItemHelper.BuildItem();
			
			GetCategoryFeaturesCall api = new GetCategoryFeaturesCall(this.apiContext);
			DetailLevelCodeType[] detailLevels = new DetailLevelCodeType[] {
			DetailLevelCodeType.ReturnAll
			};
			api.DetailLevelList = new DetailLevelCodeTypeCollection(detailLevels);
			api.LevelLimit = 10;
			api.ViewAllNodes = true;
			bool viewAllNodes=true;
			bool isSuccess;
			CategoryTypeCollection categories;
			string message;

			//get an item which supports the Ad-format
			isSuccess = CategoryHelper.GetAdFormatCategory(this.apiContext,1,out categories,out message);
			Assert.IsTrue(isSuccess,message);
			Assert.IsNotNull(categories);
			Assert.Greater(categories.Count,0);
			Assert.IsTrue(categories[0].CategoryID!=string.Empty);
			item.PrimaryCategory.CategoryID=categories[0].CategoryID;

			// get the list duration value according to the category
			CategoryFeatureTypeCollection features = api.GetCategoryFeatures(item.PrimaryCategory.CategoryID,api.LevelLimit,viewAllNodes,null, true);
		    Assert.IsNotNull(features);
			Assert.Greater(features.Count,0);
			Assert.IsNotNull(features[0].ListingDuration);
			Assert.Greater(features[0].ListingDuration.Count,0);

			//modify item property to adapt the AdFormatItem
			item.ListingType = ListingTypeCodeType.AdType;
			item.ListingDuration = features[0].ListingDuration[0].Value.ToString();

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