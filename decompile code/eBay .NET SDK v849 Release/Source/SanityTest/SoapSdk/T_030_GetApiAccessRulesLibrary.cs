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
	public class T_030_GetApiAccessRulesLibrary : SOAPTestBase
	{
		[Test]
		public void GetApiAccessRules()
		{
			GetApiAccessRulesCall api = new GetApiAccessRulesCall(this.apiContext);
			// Make API call.
			api.Execute();
			ApiAccessRuleTypeCollection rules = api.ApiAccessRuleList;
			Assert.IsNotNull(rules);
			Assert.IsTrue(rules.Count > 0, "No rules found");
			
		}
	}
}