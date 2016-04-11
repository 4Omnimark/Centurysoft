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

namespace AllTestsSuite.T_090_DisputeTestsSuite
{
	[TestFixture]
	public class T_010_AddDisputeLibrary : SOAPTestBase
	{
		[Test]
		public void AddDispute()
		{
			ItemType item = this.AddItem();
			Assert.IsNotNull(item, "Failed because no item available -- requires successful AddItem test");
			//
			AddDisputeCall api = new AddDisputeCall(this.apiContext);
			//api.DisputeExplanation = DisputeExplanationCodeType.BuyerHasNotResponded;
			api.DisputeReason = DisputeReasonCodeType.BuyerHasNotPaid;
			api.ItemID = item.ItemID;
			api.TransactionID = "0";
			// Make API call.
			ApiException gotException = null;
			try
			{
			/* String disputeId = */ api.Execute();
			}
			catch(ApiException ex)
			{
				gotException = ex;
			}
			Assert.IsNotNull(gotException);
			
		}

		public ItemType  AddItem()
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