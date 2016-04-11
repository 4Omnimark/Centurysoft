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
	/// Summary description for T_010_AddSellingManagerInventoryFolderLibrary.
	/// </summary>
	[TestFixture]
	public class T_010_AddSellingManagerInventoryFolderLibrary : SOAPTestBase
	{
		[Test]
		public void AddSellingManagerInventoryFolder()
		{
			//create a parent folder
			AddSellingManagerInventoryFolderCall api = new AddSellingManagerInventoryFolderCall(apiContext);
			String id_from_time = System.DateTime.Now.ToString("yyyyMMddhhmmss");
			api.FolderName="Folder1"+id_from_time;
			api.Comment="Folder for api test.";
			api.Execute();
			//check whether the call is success.
			Assert.IsTrue(api.AbstractResponse.Ack==AckCodeType.Success || api.AbstractResponse.Ack==AckCodeType.Warning,"do not success!");
			long folderId = api.FolderID;
			Assert.IsNotNull(folderId);
			TestData.Folder_id1 = folderId;
			//create a sub folder			
			id_from_time = System.DateTime.Now.ToString("yyyyMMddhhmmss");
			api.FolderName = "Folder2"+id_from_time;
			api.Comment = "Folder for api test.";
			api.Execute();
			//check whether the call is success.
			Assert.IsTrue(api.ApiResponse.Ack==AckCodeType.Success || api.ApiResponse.Ack==AckCodeType.Warning,"do not success!");
			TestData.Folder_id2 = api.FolderID;
		}
	}
}
