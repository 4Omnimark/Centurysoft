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
using System.Configuration;
using NUnit.Framework;
using eBay.Service.Call;
using eBay.Service.Core.Soap;
using eBay.Service.Core.Sdk;
using eBay.Service.Util;

namespace AllTestsSuite
{
	/// <summary>
	/// Summary description for ApiContextLoader.
	/// </summary>
	public class ApiContextLoader
	{
		public ApiContextLoader()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static ApiContext LoadApiContext(string name)
		{
			ApiContext context = new ApiContext();
			
			context.ApiCredential.ApiAccount.Application = LoadAppConfig(name+"appid");
			context.ApiCredential.ApiAccount.Developer = LoadAppConfig(name+"devid");
			context.ApiCredential.ApiAccount.Certificate = LoadAppConfig(name+"cert");
			context.ApiCredential.eBayToken = LoadAppConfig(name+"token");
			context.SoapApiServerUrl = LoadAppConfig("soapurl");
			context.XmlApiServerUrl = LoadAppConfig("sdkurl");
			context.EPSServerUrl = LoadAppConfig("epsurl");
            string timeout = LoadAppConfig("timeout");
            if (timeout != null && string.Empty != timeout)
            {
                context.Timeout = int.Parse(timeout);
            }

			ApiLogManager Logger = new ApiLogManager();
			Logger.EnableLogging = true;

			string logfile = LoadAppConfig("logfile");
			if (logfile != "" && logfile != null)
				Logger.ApiLoggerList.Add(new FileLogger(logfile));
			else
				Logger.ApiLoggerList.Add(new ConsoleLogger());

			if (LoadAppConfig("logexception").ToUpper() == "TRUE")
				Logger.ApiLoggerList[0].LogExceptions = true;
			
			if (LoadAppConfig("logmessages").ToUpper() == "TRUE")
				Logger.ApiLoggerList[0].LogApiMessages = true;

			Logger.ApiLoggerList[0].LogInformations = true;
			context.ApiLogManager = Logger;

			return context;
		}

		public static string LoadAppConfig(string key)
		{
			return System.Configuration.ConfigurationManager.AppSettings.Get(key);
		}

	}
}
