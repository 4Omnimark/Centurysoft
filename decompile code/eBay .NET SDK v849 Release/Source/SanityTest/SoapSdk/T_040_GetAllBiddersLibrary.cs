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

namespace AllTestsSuite.T_080_TransactionTestsSuite
{
	[TestFixture]
	public class T_040_GetAllBiddersLibrary : SOAPTestBase
	{
		[Test]
		public void GetAllBidders()
		{
			Assert.IsNotNull(TestData.NewItem, "Failed because no item available -- requires successful AddItem test");
			GetAllBiddersCall api = new GetAllBiddersCall(this.apiContext);
			api.ItemID = TestData.NewItem.ItemID;
			api.CallMode = GetAllBiddersModeCodeType.ViewAll;
			// Make API call.
			OfferTypeCollection bidders = api.GetAllBidders(api.ItemID, api.CallMode);
			// No bidders for the new item.
			Assert.IsTrue(bidders == null || bidders.Count == 0);
			
		}
	}
}