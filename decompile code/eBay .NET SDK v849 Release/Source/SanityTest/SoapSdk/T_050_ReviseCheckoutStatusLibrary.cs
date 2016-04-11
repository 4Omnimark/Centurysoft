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
	public class T_050_ReviseCheckoutStatusLibrary : SOAPTestBase
	{
		[Test]
		public void ReviseCheckoutStatus()
		{
			ReviseCheckoutStatusCall api = new ReviseCheckoutStatusCall(this.apiContext);
			TransactionType tran = null;
			if( TestData.SellerTransactions != null && TestData.SellerTransactions.Count > 0 )
			tran = TestData.SellerTransactions[0];
			// Make API call.
			ApiException gotException = null;
			// Negative test.
			try
			{
				if( tran != null )
				{
					api.ItemID = tran.Item.ItemID;
					api.TransactionID = tran.TransactionID;
                    api.CheckoutStatus = CompleteStatusCodeType.Incomplete;
				}
				api.Execute();
			}
			catch(ApiException ex)
			{
				gotException = ex;
			}
			if( gotException != null )
			Assert.IsNull(tran);
			
		}
	}
}