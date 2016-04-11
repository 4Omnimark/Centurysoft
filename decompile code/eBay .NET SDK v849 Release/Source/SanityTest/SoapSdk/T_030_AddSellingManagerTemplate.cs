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
using UnitTests.Helper;
#endregion
namespace AllTestsSuite.T_120_SellingManagerTestsSuite
{
	/// <summary>
	/// Summary description for T_030_AddSellingManagerTemplate.
	/// </summary>
	[TestFixture]
	public class T_030_AddSellingManagerTemplate : SOAPTestBase
	{
		[Test]
		public void AddSellingManagerTemplate()
		{
			AddSellingManagerTemplateCall api = new AddSellingManagerTemplateCall(apiContext);
			String id_from_time = System.DateTime.Now.ToString("yyyyMMddhhmmss");
			api.SaleTemplateName = "sale template"+id_from_time;
			api.ProductID = TestData.ProductId;
			ItemType item = ItemHelper.BuildItem();
			api.Item=item;
			api.Execute();
			//check whether the call is success.
			Assert.IsTrue(api.ApiResponse.Ack==AckCodeType.Success || api.ApiResponse.Ack==AckCodeType.Warning,"do not success!");
			long templateId = api.SaleTemplateID;
			long groupId = api.SaleTemplateGroupID;
			long productId = api.SellingManagerProductDetails.ProductID;
			TestData.SaleTemplateId = templateId;
		}
	}
}
