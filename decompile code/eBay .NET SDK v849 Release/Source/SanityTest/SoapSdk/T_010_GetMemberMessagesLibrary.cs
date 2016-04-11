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
	public class T_010_GetMemberMessagesLibrary : SOAPTestBase
	{
		[Test]
		public void GetMemberMessages()
		{
			GetMemberMessagesCall api = new GetMemberMessagesCall(this.apiContext);
			api.DetailLevelList = new DetailLevelCodeTypeCollection();
			api.DetailLevelList.Add(DetailLevelCodeType.ReturnMessages);
			api.DisplayToPublic = false;
			api.MailMessageType = MessageTypeCodeType.AskSellerQuestion;
			api.MessageStatus = MessageStatusTypeCodeType.Unanswered;
			// Time filter
			System.DateTime calTo = System.DateTime.Now;
			System.DateTime calFrom = calTo.AddHours(-1);
			api.StartCreationTime = calFrom;
			api.EndCreationTime = calTo;
			// Make API call.
			api.Execute();
			TestData.MemberMessages = api.ApiResponse.MemberMessage;
			
		}
	}
}