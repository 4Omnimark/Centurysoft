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
using eBay.Service.Core.Sdk;

namespace eBay.Service.SDK.Attribute
{
	/// <summary>
	/// Summary description for ValidationParams.
	/// </summary>
	internal class ValidationParams
	{
		private ValidationParams() {}
		public static object[] getValidationParams(Attribute attr, ValidationRule rule)
		{
			string valStr = attr.Value[0].ValueLiteral;
			if (valStr == null) 
				valStr = "";

			// Decrement count of value list if value list contains -10.
			int passCount = attr.Value.Count;
			foreach(Value val in attr.Value )
			{
				if( val.ValueID == (int)ValueIds.NONE )
				{
					passCount --;
					break;
				}
			}

			return new object [] {attr.Type, passCount, valStr, rule};
			//return new object [] {attr.Type, attr.Value.Count + attr.SType, valStr, rule};
		}
	}
}
