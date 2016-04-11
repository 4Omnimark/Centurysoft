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
	public class T_040_RespondToFeedbackLibrary : SOAPTestBase
	{
		[Test]
		public void RespondToFeedback()
		{
			Assert.IsNotNull(TestData.NewItem, "Failed because no item available -- requires successful AddItem test");
			//
			RespondToFeedbackCall api = new RespondToFeedbackCall(this.apiContext);
			api.FeedbackID = "0";
			api.ItemID = TestData.NewItem.ItemID;
			api.TransactionID = "0";
			api.ResponseText = "Test feedback response";
			api.ResponseType = FeedbackResponseCodeType.Reply;
			api.TargetUserID = "UID";
			// Make API call.
			ApiException gotException = null;
			try {
			api.Execute();
			}
			catch (ApiException ex) {
				gotException = ex;
			}
			Assert.IsNotNull(gotException);
			
		}
	}
}