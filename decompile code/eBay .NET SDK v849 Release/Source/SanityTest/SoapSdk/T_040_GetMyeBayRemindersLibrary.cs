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
	public class T_040_GetMyeBayRemindersLibrary : SOAPTestBase
	{
		[Test]
		public void GetMyeBayReminders()
		{
			GetMyeBayRemindersCall api = new GetMyeBayRemindersCall(this.apiContext);
			ReminderCustomizationType rc = new ReminderCustomizationType();
			rc.Include = true; rc.IncludeSpecified = true;
			rc.DurationInDays = 7; rc.DurationInDaysSpecified = true;
			api.BuyingReminders = rc;
			rc = new ReminderCustomizationType();
			rc.Include = true; rc.IncludeSpecified = true;
			rc.DurationInDays = 7; rc.DurationInDaysSpecified = true;
			api.SellingReminders = rc;
			// Make API call.
			api.GetMyeBayReminders();
			RemindersType buying = api.ApiResponse.BuyingReminders;
			RemindersType selling = api.ApiResponse.SellingReminders;
			
		}
	}
}