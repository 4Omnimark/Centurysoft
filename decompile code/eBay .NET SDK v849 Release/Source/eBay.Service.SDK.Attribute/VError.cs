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
using System.Runtime.InteropServices;

namespace eBay.Service.SDK.Attribute
{
	/// <summary>
	/// Defines attribute error object.
	/// </summary>
	public interface IError
	{
		/// <summary>
		/// Attribute Id.
		/// </summary>
		int AttrId
		{ get; set; }

		/// <summary>
		/// Rule name.
		/// </summary>
		string RuleName
		{ get; set; }

		/// <summary>
		/// Error message.
		/// </summary>
		string Message
		{ get; set; }
	}

	/// <summary>
	/// Defines attribute error object.
	/// </summary>
	[ClassInterface(ClassInterfaceType.None)]
	internal class Error : IError
	{
		private int mAttrId;
		private string mRuleName;
		private string mMessage;

		public Error()
		{
		}

		/// <summary>
		/// Attribute Id.
		/// </summary>
		public int AttrId
		{ 
			get { return mAttrId; }
			set { mAttrId = value; }
		}

		/// <summary>
		/// Rule name.
		/// </summary>
		public string RuleName
		{ 
			get { return mRuleName; }
			set { mRuleName = value; }
		}

		/// <summary>
		/// Error message.
		/// </summary>
		public string Message
		{ 
			get { return mMessage; }
			set { mMessage = value; }
		}

		internal XmlNode toXml()
		{
			return toXml(new XmlDocument());
		}

		internal XmlNode toXml(XmlDocument doc)
		{
			XmlNode node = doc.CreateElement(ERROR);
			XmlAttribute attr = doc.CreateAttribute("", ERROR_CODE, "");
			attr.Value = this.mRuleName;
			node.Attributes.Append(attr);
			attr = doc.CreateAttribute("", "id", "");
			attr.Value = this.mAttrId.ToString();
			node.Attributes.Append(attr);
			XmlNode text = doc.CreateCDataSection(this.mMessage);
			node.AppendChild(text);
			return node;
		}

		private const string ERROR = "Error";
		private const string ERROR_CODE = "errorcode";
	}
}
