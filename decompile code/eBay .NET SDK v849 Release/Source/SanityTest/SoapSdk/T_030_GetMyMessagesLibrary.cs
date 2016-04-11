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

namespace AllTestsSuite.T_010_MessageTestsSuite
{
	[TestFixture]
	public class T_030_GetMyMessagesLibrary : SOAPTestBase
	{
		[Test]
		public void GetMyMessages()
		{
			GetMyMessagesCall api = new GetMyMessagesCall(this.apiContext);
			api.DetailLevelList = new DetailLevelCodeTypeCollection();
			api.DetailLevelList.Add(DetailLevelCodeType.ReturnSummary);
			// Make API call.
			api.GetMyMessages();
			MyMessagesAlertTypeCollection alerts = api.ApiResponse.Alerts;
			Assert.IsNotNull(alerts);
			MyMessagesMessageTypeCollection msgs = api.ApiResponse.Messages;
			Assert.IsNotNull(msgs);
			MyMessagesSummaryType summary = api.ApiResponse.Summary;
			Assert.IsNotNull(summary);
			
		}
	}
}