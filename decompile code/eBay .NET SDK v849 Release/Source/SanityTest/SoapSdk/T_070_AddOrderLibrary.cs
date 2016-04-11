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
	public class T_070_AddOrderLibrary : SOAPTestBase
	{
		[Test]
		public void AddOrder()
		{
			Assert.IsNotNull(TestData.NewItem, "Failed because no item available -- requires successful AddItem test");
			// Make API call.
			ApiException gotException = null;
			try
			{
			AddOrderCall api = new AddOrderCall(this.apiContext);
			OrderType order = new OrderType();
			api.Order = order;
			TransactionType t1 = new TransactionType();
			t1.Item = TestData.NewItem;
			t1.TransactionID = "0";
			TransactionType t2 = new TransactionType();
			t2.Item = TestData.NewItem;
			t2.TransactionID = "0";
			TransactionTypeCollection tary = new TransactionTypeCollection();
				tary.Add(t1); tary.Add(t2);
			order.TransactionArray = tary;
			api.Order = order;
			// Make API call.
			/*AddOrderResponseType resp =*/ api.Execute();
			}
			catch(ApiException ex)
			{
				gotException = ex;
			}
			Assert.IsNotNull(gotException);
			
		}
	}
}