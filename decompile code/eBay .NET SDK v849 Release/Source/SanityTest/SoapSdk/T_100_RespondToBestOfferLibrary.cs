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
	public class T_100_RespondToBestOfferLibrary : SOAPTestBase
	{
		[Test]
		public void RespondToBestOffer()
		{
			Assert.IsNotNull(TestData.NewItem, "Failed because no item available -- requires successful AddItem test");
			//
			RespondToBestOfferCall api = new RespondToBestOfferCall(this.apiContext);
			// Make API call.
			ApiException gotException = null;
			try
			{
			api.Action = BestOfferActionCodeType.Accept;
			api.BestOfferIDList = new StringCollection();
			api.BestOfferIDList.Add("0");
			api.ItemID = TestData.NewItem.ItemID;
			api.SellerResponse = "Hello SDK user!";
			api.Execute();
			}
			catch(ApiException ex)
			{
				gotException = ex;
			}
			Assert.IsNotNull(gotException);
			
		}
	}
}