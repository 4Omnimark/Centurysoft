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
	public class T_010_GetBidderListLibrary : SOAPTestBase
	{
		[Test]
		public void GetBidderList()
		{
			GetBidderListCall api = new GetBidderListCall(this.apiContext);
			// Set the item to be ended.
			api.ActiveItemsOnly = false;
			// Time filter
			System.DateTime calTo = DateTime.Now;
			System.DateTime calFrom = calTo;
			calFrom.AddHours(-1);
			api.EndTimeFrom = calFrom;
			api.EndTimeTo = calTo;
			// Make API call.
			ItemTypeCollection items = api.GetBidderList(api.ActiveItemsOnly, calFrom, calTo, null, GranularityLevelCodeType.Coarse);
			UserType bidder = api.ApiResponse.Bidder;
			
		}
	}
}