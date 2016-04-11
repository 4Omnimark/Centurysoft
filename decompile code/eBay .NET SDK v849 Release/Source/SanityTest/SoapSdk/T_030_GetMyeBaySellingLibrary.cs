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

namespace AllTestsSuite.T_060_MyeBayTestsSuite
{
	[TestFixture]
	public class T_030_GetMyeBaySellingLibrary : SOAPTestBase
	{
		[Test]
		public void GetMyeBaySelling()
		{
			GetMyeBaySellingCall api = new GetMyeBaySellingCall(this.apiContext);
			GetMyeBaySellingRequestType req = new GetMyeBaySellingRequestType();
			api.ApiRequest = req;
			ItemListCustomizationType lc = new ItemListCustomizationType();
			lc.Include = true; lc.IncludeSpecified = true;
			lc.IncludeNotes = true; lc.IncludeNotesSpecified = true;
			lc.Sort = ItemSortTypeCodeType.BidCount;
			req.ActiveList = lc;
			lc = new ItemListCustomizationType();
			lc.Include = true; lc.IncludeSpecified = true;
			lc.IncludeNotes = true; lc.IncludeNotesSpecified = true;
			lc.Sort = ItemSortTypeCodeType.Price;
			req.SoldList = lc;
			lc = new ItemListCustomizationType();
			lc.Include = true; lc.IncludeSpecified = true;
			lc.IncludeNotes = true; lc.IncludeNotesSpecified = true;
			lc.Sort = ItemSortTypeCodeType.EndTime;
			req.UnsoldList = lc;
			lc = new ItemListCustomizationType();
			lc.Include = true; lc.IncludeSpecified = true;
			lc.IncludeNotes = true; lc.IncludeNotesSpecified = true;
			lc.Sort = ItemSortTypeCodeType.StartTime;
			req.ScheduledList = lc;
			// Make API call.
			api.GetMyeBaySelling();
			GetMyeBaySellingResponseType resp = api.ApiResponse;
			
		}
	}
}