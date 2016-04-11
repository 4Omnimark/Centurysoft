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
	public class T_120_GetOrderTransactionsLibrary : SOAPTestBase
	{
		[Test]
		public void GetOrderTransactions()
		{
			//
			GetOrderTransactionsCall api = new GetOrderTransactionsCall(
			this.apiContext);
			StringCollection idArray = new StringCollection();
			api.OrderIDArrayList = idArray;
			idArray.Add("1111111111");
			//ItemTransactionIDTypeCollection tIdArray = new ItemTransactionIDTypeCollection();
			//api.ItemTransactionIDArrayList = tIdArray;
			//ItemTransactionIDType tId = new ItemTransactionIDType();
			//tIdArray.Add(tId);
			//String itemId = "2222222222";
			//tId.ItemID = itemId;
			//tId.TransactionID = "test transaction id";
			// Make API call.
			ApiException gotException = null;
			try {
			OrderTypeCollection orders = api.GetOrderTransactions(idArray);
			} catch (ApiException ex) {
				gotException = ex;
			}
			Assert.IsNull(gotException);
			
		}
	}
}