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
using UnitTests.Helper;
#endregion

namespace AllTestsSuite.T_050_ItemTestsSuite
{
	[TestFixture]
	public class T_040_SetUserNotesLibrary : SOAPTestBase
	{
		[Test]
		public void SetUserNotes()
		{
			Assert.IsNotNull(TestData.NewItem2, "Failed because no item available -- requires successful AddItem test");
			SetUserNotesCall api = new SetUserNotesCall(this.apiContext);
			api.Action = SetUserNotesActionCodeType.AddOrUpdate;
			api.ItemID = TestData.NewItem2.ItemID;
			api.NoteText = "Notes for eBay SDK sanity test.";

			DetailLevelCodeTypeCollection detailLevel=new DetailLevelCodeTypeCollection();
			DetailLevelCodeType type=DetailLevelCodeType.ReturnAll;
			detailLevel.Add(type);
			api.DetailLevelList=detailLevel;
			// Make API call.
			api.Execute();

			System.Threading.Thread.Sleep(3000);
			//check whether the call is success.
			//eBayNotes can not get by calling GetItemCall.
			Assert.IsTrue(api.ApiResponse.Ack==AckCodeType.Success || api.ApiResponse.Ack==AckCodeType.Warning,"do not success!");
		}
	}
}