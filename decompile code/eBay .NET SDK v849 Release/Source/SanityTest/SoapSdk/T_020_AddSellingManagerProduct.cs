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
	/// Summary description for T_020_AddSellingManagerProduct.
	/// </summary>
	[TestFixture]
	public class T_020_AddSellingManagerProduct : SOAPTestBase
	{
		[Test]
		public void AddSellingManagerProduct()
		{
			Assert.IsTrue(TestData.Folder_id1!=long.MinValue);
			AddSellingManagerProductCall api = new AddSellingManagerProductCall(apiContext);
			SellingManagerProductDetailsType details = new SellingManagerProductDetailsType();
			details.ProductName="Product for test";
			details.QuantityAvailable=10;
			api.SellingManagerProductDetails=details;
			api.FolderID=TestData.Folder_id1;
			api.Execute();
			//check whether the call is success.
			Assert.IsTrue(api.ApiResponse.Ack==AckCodeType.Success || api.ApiResponse.Ack==AckCodeType.Warning,"fail!");
			Assert.IsTrue(10==api.SellingManagerProductDetails.QuantityAvailable);
			Assert.IsTrue("Product for test".Equals(api.SellingManagerProductDetails.ProductName));
			long productId = api.SellingManagerProductDetailsReturn.ProductID;
			Assert.IsTrue(productId != 0L);
			TestData.ProductId = productId;
		}
		
	}
}
