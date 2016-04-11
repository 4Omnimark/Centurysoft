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

namespace AllTestsSuite.T_020_OtherTestsSuite
{
	[TestFixture]
	public class T_130_GetAccountLibrary : SOAPTestBase
	{
		[Test]
		public void GetAccount()
		{
			GetAccountCall api = new GetAccountCall(this.apiContext);
			api.AccountHistorySelection = AccountHistorySelectionCodeType.LastInvoice;
			/*
			System.DateTime calTo = System.DateTime.Instance;
			System.DateTime calFrom = (System.DateTime)calTo.clone();
			calFrom.add(System.DateTime.DATE, -1);
			TimeFilter tf = new TimeFilter(calFrom, calTo);
			api.ViewPeriod = tf;
			*/
			// Pagination
			PaginationType pt = new PaginationType();
			pt.EntriesPerPage = 0; // No details will be returned.
			pt.EntriesPerPageSpecified = true;
			pt.PageNumber = 1;
			pt.PageNumberSpecified = true;
			api.Pagination = pt;
			ApiException gotException = null;
			try 
			{
				api.GetAccount(AccountHistorySelectionCodeType.LastInvoice);
			}
			catch (ApiException e)
			{
				gotException = e;
			}
			Assert.IsTrue(gotException == null || gotException.containsErrorCode("20154"));

		}
	}
}