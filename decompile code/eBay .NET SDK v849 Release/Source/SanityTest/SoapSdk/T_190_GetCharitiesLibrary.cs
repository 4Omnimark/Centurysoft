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

namespace AllTestsSuite.T_020_OtherTestsSuite
{
	[TestFixture]
	public class T_190_GetCharitiesLibrary : SOAPTestBase
	{
		[Test]
		public void GetCharities()
		{
			GetCharitiesCall api = new GetCharitiesCall(this.apiContext);
			GetCharitiesRequestType req = new GetCharitiesRequestType();
			req.IncludeDescription = false; req.IncludeDescriptionSpecified = true;
			req.CharityRegion = 7; req.CharityRegionSpecified = true;
			req.Query = "eBay";
			// Make API call.
			api.Execute();
			CharityInfoTypeCollection charities = api.ApiResponse.Charity;
			Assert.IsTrue(charities.Count > 0, "No charities fould");
			
		}
	}
}