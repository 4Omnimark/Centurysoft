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
	public class T_090_GetItemShippingLibrary : SOAPTestBase
	{
		[Test]
		public void GetItemShipping()
		{
			Assert.IsNotNull(TestData.NewItem, "Failed because no item available -- requires successful AddItem test");
			//
			GetItemShippingCall api = new GetItemShippingCall(this.apiContext);

			string itemID = TestData.NewItem.ItemID;
			string postalCode= "95125";
			int quantitySold=1;
			CountryCodeType countryCode=CountryCodeType.US;
			// Make API call.

			ShippingDetailsType shippingDetails = api.GetItemShipping(itemID,quantitySold,postalCode,countryCode);
			
			//check whether the call is success.
			Assert.IsTrue(api.ApiResponse.Ack==AckCodeType.Success || api.ApiResponse.Ack==AckCodeType.Warning,"do not success!");
			Assert.IsNotNull(shippingDetails);
		}

	}
}