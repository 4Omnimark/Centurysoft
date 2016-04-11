#region Copyright
//	Copyright (c) 2013 eBay, Inc.
//	
//	This program is licensed under the terms of the eBay Common Development and
//	Distribution License (CDDL) Version 1.0 (the "License") and any subsequent  
//	version thereof released by eBay.  The then-current version of the License can be 
//	found at http://www.opensource.org/licenses/cddl1.php and in the eBaySDKLicense 
//	file that is under the eBay SDK ../docs directory
#endregion

using System;
using System.Reflection;
using System.Configuration;
using NUnit.Framework;
using eBay.Service.Call;
using eBay.Service.Core.Soap;
using eBay.Service.Core.Sdk;
using eBay.Service.Util;

namespace AllTestsSuite
{
	public class SOAPTestBase : UnitTestBase
	{
		public SOAPTestBase()
		{
		}
		
		public ApiContext apiContext;
		public ApiLogManager Logger;
		public bool teststarted = false;

		[TestFixtureSetUp]
		public override void TestFixtureSetup()
		{
			base.TearDown();
			apiContext = ApiContextLoader.LoadApiContext("");
			Logger = apiContext.ApiLogManager;
		}

		[TestFixtureTearDown]
		public override void TestFixtureTearDown()
		{
		}

		[SetUp]
		public override void Setup()
		{
			string formatmsg = "******************** [Begin Test] ********************";
			Logger.RecordMessage(formatmsg);
		}

		[TearDown]
		public override void TearDown()
		{
			string formatmsg = "********************* [End Test] *********************";
			Logger.RecordMessage(formatmsg);
		}

		public string LoadAppConfig(string key)
		{
			return System.Configuration.ConfigurationManager.AppSettings.Get(key);
		}

		public void LogTestCaseStart(string name)
		{
			string formatmsg = String.Format("*******************************************\r\n[{0}]\r\n", name);
			Logger.RecordMessage(formatmsg, MessageType.Information, MessageSeverity.Informational);

		}
		public void LogTestCasePassed(string name)
		{
			string formatmsg = String.Format("[{0}: Passed]\r\n*******************************************\r\n", name);
			Logger.RecordMessage(formatmsg, MessageType.Information, MessageSeverity.Informational);

		}

		public void LogTestCaseFailed(string name)
		{
			string formatmsg = String.Format("[{0}: Failed]\r\n*******************************************\r\n", name);
			Logger.RecordMessage(formatmsg, MessageType.Exception, MessageSeverity.Error);

		}

		public void RunTest(MethodBase method)
		{
			try
			{
				LogTestCaseStart(method.Name);
				teststarted = true;
				method.Invoke(this, null);
				LogTestCasePassed(method.Name);
	
			}

			catch (TargetInvocationException aex)
			{
				Logger.RecordMessage(aex.InnerException.Message, MessageType.Exception, MessageSeverity.Error);
				LogTestCaseFailed(method.Name);

				throw aex.InnerException;
			}
			finally
			{
				teststarted = false;
			}
		}

	
	}
}
