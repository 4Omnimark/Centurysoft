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
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using eBay.Service.EPS;
using eBay.Service.Util;

namespace eBay.Service.Util
{
	/// <summary>
	/// Helper class to work on motor attributes.
	/// </summary>
	public class MotorAttributeHelper
	{
		private AttributeSetType mSet;

		/// <summary>
		/// ID of Subtitle attribute.
		/// </summary>
		public const int ID_SUBTITLE = 10246;

		/// <summary>
		/// ID of DepositAmount attribute.
		/// </summary>
		public const int ID_DEPOSITAMOUNT = 10733;

		/// <summary>
		/// ID of DepositType attribute.
		/// </summary>
		public const int ID_DEPOSITTYPE = 10734;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="ast">The object that contains the motor attributes that you want to access.</param>
		public MotorAttributeHelper(AttributeSetType ast)
		{
			this.mSet = ast;
		}

		/// <summary>
		/// Get or set subtitle. Setting null string will remove Subtitle from the attribute list.
		/// </summary>
		public string Subtitle
		{
			get 
			{
				return AttributeHelper.GetValueLiteral(this.mSet, ID_SUBTITLE);
			}

			set 
			{
				if( value == null || value.Length == 0 )
					AttributeHelper.RemoveAttribute(this.mSet, ID_SUBTITLE);
				else
					AttributeHelper.InsertToAttributeSet(this.mSet, ID_SUBTITLE, 0, value);
			}
		}

		/// <summary>
		/// Get or set DepositAmount. Setting 0.0m will remove DepositType and DepositAmount
		/// from the attribute list.
		/// </summary>
		public decimal DepositAmount
		{
			get 
			{
				string t = AttributeHelper.GetValueLiteral(this.mSet, ID_DEPOSITAMOUNT);
				if( t != null && t.Length > 0 )
					return Decimal.Parse(t);
				else
					return 0.0m;
			}

			set 
			{
				if( value == 0.0m )
				{
					AttributeHelper.RemoveAttribute(this.mSet, ID_DEPOSITTYPE);
					AttributeHelper.RemoveAttribute(this.mSet, ID_DEPOSITAMOUNT);
				}
				else
				{
					AttributeHelper.InsertToAttributeSet(this.mSet, ID_DEPOSITAMOUNT, 0, value.ToString());
					AttributeHelper.InsertToAttributeSet(this.mSet, ID_DEPOSITTYPE, 0, "1");
				}
			}
		}
	}
}
