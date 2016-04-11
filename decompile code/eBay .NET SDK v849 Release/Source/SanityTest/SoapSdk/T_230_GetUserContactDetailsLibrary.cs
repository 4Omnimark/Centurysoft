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
	public class T_230_GetUserContactDetailsLibrary : SOAPTestBase
	{
		[Test]
		public void GetUserContactDetails()
		{
			GetUserContactDetailsCall api = new GetUserContactDetailsCall(
			this.apiContext);
			// Make API call.
			ApiException gotException = null;
			// Negative test.
			try {
			String contact = api.GetUserContactDetails("test item id", "contact user id", "requester");
			} catch (ApiException ex) {
				gotException = ex;
			}
			Assert.IsNotNull(gotException);
			
		}
	}
}