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
using System.Xml;
using System.IO;
using NUnit.Framework;
using eBay.Service.Call;
using eBay.Service.Core.Soap;
using eBay.Service.Core.Sdk;
using eBay.Service.Util;
#endregion
namespace AllTestsSuite.T_120_SellingManagerTestsSuite
{
	/// <summary>
	/// Summary description for T_110_GetSellingManagerInventoryFolder.
	/// </summary>
	[TestFixture]
	public class T_110_GetSellingManagerInventoryFolder : SOAPTestBase
	{
		[Test]
		public void GetSellingManagerInventoryFolder()
		{
			Assert.IsTrue(TestData.Folder_id1!=long.MinValue);
			GetSellingManagerInventoryFolderCall api = new GetSellingManagerInventoryFolderCall(apiContext);
			api.Execute();
			//check whether the call is success.
			Assert.IsTrue(api.ApiResponse.Ack==AckCodeType.Success || api.ApiResponse.Ack==AckCodeType.Warning,"do not success!");
			Assert.IsTrue(contains(TestData.Folder_id1,api.Folder));
		}

		private bool contains(long folder_id1,SellingManagerFolderDetailsType returnedFolder) 
		{
			foreach(SellingManagerFolderDetailsType folder in returnedFolder.ChildFolder)
			{
				if(folder.FolderID.CompareTo(folder_id1) == 0)
				{
					return true;
				}

			}
			return false;
		}
	}
}
