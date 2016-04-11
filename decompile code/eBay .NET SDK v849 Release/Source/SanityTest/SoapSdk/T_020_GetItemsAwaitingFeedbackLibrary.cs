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

namespace AllTestsSuite.T_070_FeedbackTestsSuite
{
	[TestFixture]
	public class T_020_GetItemsAwaitingFeedbackLibrary : SOAPTestBase
	{
		[Test]
		public void GetItemsAwaitingFeedback()
		{
			GetItemsAwaitingFeedbackCall api = new GetItemsAwaitingFeedbackCall(this.apiContext);
			// Pagination
			PaginationType pt = new PaginationType();
			pt.EntriesPerPage = 100; pt.EntriesPerPageSpecified = true;
			pt.PageNumber = 1; pt.PageNumberSpecified = true;
			api.Pagination = pt;
			api.Sort = ItemSortTypeCodeType.EndTime;
			// Make API call.
			api.Execute();
//			TransactionTypeCollection trans = api.ApiResponse.ItemsAwaitingFeedback.TransactionArray;
//			PaginationResultType pr = api.ApiResponse.ItemsAwaitingFeedback.PaginationResult;
			
		}
	}
}