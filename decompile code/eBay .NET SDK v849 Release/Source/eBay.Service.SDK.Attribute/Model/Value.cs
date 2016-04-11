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
	/// Predefined ValueIds.
	/// </summary>
	public enum ValueIds
	{
		/// <summary>
		/// Incomplete Single-Text-Date.
		/// </summary>
		PARTIAL_TEXT_DATE_ONE = -1,

		/// <summary>
		/// Complete Single-Text-Date.
		/// </summary>
		COMPLETE_TEXT_DATE_ONE = -2,

		/// <summary>
		/// Localized string.
		/// </summary>
		TEXT = -3,

		/// <summary>
		/// Incomplete Text-Date.
		/// </summary>
		PARTIAL_TEXT_DATE = -4,

		/// <summary>
		/// Complete Text-Date.
		/// </summary>
		COMPLETE_TEXT_DATE = -5,

		/// <summary>
		/// Other.
		/// </summary>
		OTHER = -6,

		/// <summary>
		/// None or initial value.
		/// </summary>
		NONE = -10,

		/// <summary>
		/// Not specified.
		/// </summary>
		NOT_SPECIFIED = -100,
	}

	//----------------------------------------------------------------

	/// <summary>
	/// Enumerates types of attribute value.
	/// </summary>
	public enum ValueTypes
	{
		/// <summary>
		/// Uses IValue.ValueId to get value data.
		/// </summary>
		ValueId,

		/// <summary>
		/// IValue.ValueLiteral is the value data.
		/// </summary>
		Text
	}

	//----------------------------------------------------------------

	/// <summary>
	/// Wrapper class for SOAP ValType.
	/// </summary>
	public class Value : ValType
	{
		private ValueTypes mType;

		/// <summary>
		/// 
		/// </summary>
		public Value()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		public Value(ValueTypes type)
		{
			this.mType = type;
		}

		/// <summary>
		/// Type of a attribute value.
		/// </summary>
		public ValueTypes Type
		{
			get
			{
				return this.mType;
			}
			set
			{
				this.mType = value;
			}
		}
	}
}
