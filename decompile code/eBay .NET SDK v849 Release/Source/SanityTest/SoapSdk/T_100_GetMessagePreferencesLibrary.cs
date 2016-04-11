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

namespace AllTestsSuite.T_010_MessageTestsSuite
{
	[TestFixture]
	public class T_100_GetMessagesPreferencesLibrary : SOAPTestBase
	{
		[Test]
		public void GetMessagePreferences()
		{
			GetUserCall userApi = new GetUserCall(this.apiContext);
			DetailLevelCodeType[] detailLevels = new DetailLevelCodeType[] {DetailLevelCodeType.ReturnAll};
			userApi.DetailLevelList = new DetailLevelCodeTypeCollection(detailLevels);
			UserType user = userApi.GetUser();
			Assert.IsNotNull(user.Email);
			Assert.IsNotNull(user.EIASToken);
			GetMessagePreferencesCall api = new GetMessagePreferencesCall(this.apiContext);
			api.Site = SiteCodeType.US;
			//
			ASQPreferencesType resp = api.GetMessagePreferences(user.UserID, true);
			Assert.IsNotNull(resp);
			Console.WriteLine("T_100_GetMessagesPreferencesLibrary: " + resp.ToString());
		}
	}
}