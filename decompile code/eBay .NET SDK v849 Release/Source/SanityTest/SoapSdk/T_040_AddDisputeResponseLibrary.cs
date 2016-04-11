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

namespace AllTestsSuite.T_090_DisputeTestsSuite
{
	[TestFixture]
	public class T_040_AddDisputeResponseLibrary : SOAPTestBase
	{
		[Test]
		public void AddDisputeResponse()
		{
			AddDisputeResponseCall api = new AddDisputeResponseCall(this.apiContext);
			api.DisputeActivity = DisputeActivityCodeType.SellerShippedItem;
			api.ShipmentTrackNumber = "0000-1111";
			api.ShippingCarrierUsed = "UPS";
			System.DateTime calTo = DateTime.Now;
			api.ShippingTime = calTo;
			api.DisputeID = "Test";
			api.MessageText = "SDK test dispute response";
			// Make API call.
			ApiException gotException = null;
			try
			{
			api.Execute();
			}
			catch(ApiException ex)
			{
				gotException = ex;
			}
			Assert.IsNotNull(gotException);
			
		}
	}
}