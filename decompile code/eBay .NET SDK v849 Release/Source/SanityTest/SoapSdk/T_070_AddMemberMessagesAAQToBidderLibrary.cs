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
	public class T_070_AddMemberMessagesAAQToBidderLibrary : SOAPTestBase
	{
		[Test]
		public void AddMemberMessagesAAQToBidder()
		{
			AddMemberMessagesAAQToBidderCall api = new AddMemberMessagesAAQToBidderCall(apiContext);
			AddMemberMessagesAAQToBidderRequestContainerTypeCollection msgs = new AddMemberMessagesAAQToBidderRequestContainerTypeCollection();
			api.AddMemberMessagesAAQToBidderRequestContainerList = msgs;
			AddMemberMessagesAAQToBidderRequestContainerType msg = new AddMemberMessagesAAQToBidderRequestContainerType();
			msgs.Add(msg);
			msg.CorrelationID = "TestCorrelationID";
			msg.ItemID = "1111111111";
			MemberMessageType memberMsg = new MemberMessageType();
			msg.MemberMessage = memberMsg;
			memberMsg.Subject = "SDK Sanity Test ASQ";
			memberMsg.Body = "SDK sanity test body";
			memberMsg.DisplayToPublic = false; memberMsg.DisplayToPublicSpecified = true;
			memberMsg.EmailCopyToSender = false; memberMsg.EmailCopyToSenderSpecified = true;
			memberMsg.HideSendersEmailAddress = true; memberMsg.HideSendersEmailAddressSpecified = true;
			memberMsg.MessageType = MessageTypeCodeType.ContactEbayMember;
			memberMsg.QuestionType = QuestionTypeCodeType.General;
			memberMsg.RecipientID = new StringCollection();
			memberMsg.RecipientID.Add(TestData.ApiUserID);
			// Make API call.
			ApiException gotException = null;
			// Negative test.
			try {
			api.Execute();
			} catch (ApiException ex) {
				gotException = ex;
			}
			Assert.IsNotNull(gotException);
			
		}
	}
}