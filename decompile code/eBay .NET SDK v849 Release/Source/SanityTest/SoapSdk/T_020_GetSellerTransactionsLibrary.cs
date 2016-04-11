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
	public class T_020_GetSellerTransactionsLibrary : SOAPTestBase
	{
		[Test]
		public void GetSellerTransactions()
		{
			GetSellerTransactionsCall gst = new GetSellerTransactionsCall(this.apiContext);
			// Time filter
			System.DateTime calTo = System.DateTime.Now;
			System.DateTime calFrom = calTo.AddHours(-1);
			TimeFilter tf = new TimeFilter(calFrom, calTo);
			gst.ModTimeFilter = tf;
			// Pagination
			PaginationType pt = new PaginationType();
			pt.EntriesPerPage = 100; pt.EntriesPerPageSpecified = true;
			pt.PageNumber = 1; pt.PageNumberSpecified = true;
			gst.Pagination = pt;
			gst.Execute();
			TestData.SellerTransactions = gst.ApiResponse.TransactionArray;
			
		}
	}
}