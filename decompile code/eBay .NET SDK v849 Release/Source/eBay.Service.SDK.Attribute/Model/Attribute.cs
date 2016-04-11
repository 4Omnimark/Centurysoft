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
using eBay.Service.Core.Soap;

namespace eBay.Service.SDK.Attribute
{
	/// <summary>
	/// Enumerates attribute type.
	/// </summary>
	public enum AttributeTypes
	{
		/// <summary>
		/// 
		/// </summary>
		ATTR_ID = 0,

		/// <summary>
		/// 
		/// </summary>
		ATTR_IDS = 1,

		/// <summary>
		/// 
		/// </summary>
		ATTR_TEXT = 11,

		/// <summary>
		/// 
		/// </summary>
		ATTR_TEXT_DATE = 12,

		/// <summary>
		/// 
		/// </summary>
		ATTR_TEXT_DATE_ONE = 13,

		/// <summary>
		/// 
		/// </summary>
		ATTR_DATE = 100,

		/// <summary>
		/// 
		/// </summary>
		ATTR_DATE_D = 101,

		/// <summary>
		/// 
		/// </summary>
		ATTR_DATE_M = 102,

		/// <summary>
		/// 
		/// </summary>
		ATTR_DATE_Y = 103,
	}

	/// <summary>
	/// Wraper class for SOAP AttributeType.
	/// </summary>
	public class Attribute : AttributeType
	{
		private AttributeTypes mType;

		/// <summary>
		/// 
		/// </summary>
		public Attribute()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		public Attribute(AttributeTypes type)
		{
			this.mType = type;
		}

		/// <summary>
		/// Type of the attribute.
		/// </summary>
		public AttributeTypes Type
		{
			get
			{
				return mType;
			}
			set
			{
				this.mType = value;
			}
		}
	}
}
