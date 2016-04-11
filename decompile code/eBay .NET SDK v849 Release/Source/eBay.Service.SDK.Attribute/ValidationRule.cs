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
using System.Xml;
using System.Xml.XPath;
using eBay.Service.Core.Soap;
using eBay.Service.Core.Sdk;
using eBay.Service.SDK.Util;
using eBay.Service.Util;

namespace eBay.Service.SDK.Attribute
{
	/// <summary>
	/// Summary description for ValidationRule.
	/// </summary>
	internal class ValidationRule
	{
		public int length;
		public string min;
		public string max;
		public string name = "";
		public int precision;
		public string required = "";
		public string regex = "";
		public string separator = "";
		public string mask = "";
		public int full;
		public string protocol = "";
		public string invalidhost = "";
		public string invalidhostoverride = "";

		public ValidationRule(XmlNode parent)
		{
			XPathNavigator nav = parent.CreateNavigator();

			this.name = XmlUtility.GetString(nav, NAME);

			this.length = XmlUtility.GetInteger(nav, LENGTH);
			string s = XmlUtility.GetString(nav, MIN);
			this.min = (s != null && s.Length > 0) ? s : "-1";
			s = XmlUtility.GetString(nav, MAX);
			this.max = (s != null && s.Length > 0) ? s : "-1";

			this.precision = XmlUtility.GetInteger(nav, PRECISION);

			this.required = XmlUtility.GetString(nav, REQUIRED);
			this.regex = XmlUtility.GetString(nav, REG_EXP);
			this.separator = XmlUtility.GetString(nav, SEPARATOR);
			this.mask = XmlUtility.GetString(nav, MASK);
			this.full = XmlUtility.GetInteger(nav, FULL);
			this.protocol = XmlUtility.GetString(nav, PROTOCOL);
			this.invalidhost = XmlUtility.GetString(nav, INVALIDHOST);
			this.invalidhostoverride = XmlUtility.GetString(nav, INVALIDHOSTOVERRIDE);
		}

		public const string NAME = "Name";
		public const string MIN = "Min";
		public const string MAX = "Max";
		public const string LENGTH = "Length";
		public const string PRECISION = "Precision";
		public const string REQUIRED = "Required";
		public const string REG_EXP = "RegularExpression";
		public const string SEPARATOR = "Separator";
		public const string MASK = "Mask";
		public const string FULL = "Full";
		public const string PROTOCOL = "Protocol";
		public const string INVALIDHOST = "InvalidHost";
		public const string INVALIDHOSTOVERRIDE = "InvalidHostOverride";
	}
}
