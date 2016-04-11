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
using System.IO;
using System.Text;
using System.Xml;
#endregion

namespace eBay.Service.Util
{

	/// <summary>
	/// 
	/// </summary>
	public class ConsoleLogger : ApiLogger
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public ConsoleLogger()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="LogInformations"></param>
		/// <param name="LogApiMessages"></param>
		/// <param name="LogExceptions"></param>
		public ConsoleLogger(bool LogInformations, bool LogApiMessages, bool LogExceptions)
		{
			this.LogApiMessages = LogApiMessages;
			this.LogInformations = LogInformations;
			this.LogExceptions = LogExceptions;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="Message"></param>
		/// <param name="Severity"></param>
		public override void RecordMessage(string Message, MessageSeverity Severity)
		{
			StringBuilder message = new StringBuilder();

			message.Append("["+DateTime.Now.ToString(System.Globalization.CultureInfo.CurrentUICulture));
			message.Append(", " + Severity.ToString());
			message.Append("]\r\n" + Message + "\r\n");

			// Force the write to the underlying file
			Console.WriteLine(message);
		}
		#endregion

	}
}

