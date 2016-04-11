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
	/// Helper class for attributes.
	/// </summary>
	public abstract class AttributeHelper
	{
		/// <summary>
		/// Find an attribute node.
		/// </summary>
		/// <param name="ast"></param>
		/// <param name="attributeID"></param>
		/// <returns></returns>
		public static AttributeType FindAttribute(AttributeSetType ast, int attributeID)
		{
			foreach(AttributeType attr in ast.Attribute)
			{
				if( attr.attributeID == attributeID )
					return attr;
			}
			return null;
		}

		/// <summary>
		/// Insert an attribute node to AttributeSetType or update the existing attribute node.
		/// </summary>
		/// <param name="ast"></param>
		/// <param name="attributeID"></param>
		/// <param name="valueID"></param>
		/// <param name="valStr"></param>
		public static void InsertToAttributeSet(AttributeSetType ast, int attributeID, int valueID, string valStr)
		{
			AttributeType attr = FindAttribute(ast, attributeID);
			if( attr == null )
			{
				attr = new AttributeType();
				attr.attributeID = attributeID;
				ast.Attribute.Add(attr);
			}

			ValType v = new ValType();
			v.ValueID = valueID;
			v.ValueLiteral = valStr;

			attr.Value = new ValTypeCollection();
			attr.Value.Add(v);
		}

		/// <summary>
		/// Get the first ValueLiteral of an attribute node.
		/// </summary>
		/// <param name="ast"></param>
		/// <param name="attributeID"></param>
		/// <returns></returns>
		public static string GetValueLiteral(AttributeSetType ast, int attributeID)
		{
			AttributeType attr = FindAttribute(ast, attributeID);
			if( attr != null && attr.Value != null && attr.Value.Count > 0 )
			{
				return attr.Value[0].ValueLiteral;
			}
			return null;
		}

		/// <summary>
		/// Get the first ValueID of an attribute node.
		/// </summary>
		/// <param name="ast"></param>
		/// <param name="attributeID"></param>
		/// <returns></returns>
		public static int GetValueID(AttributeSetType ast, int attributeID)
		{
			AttributeType attr = FindAttribute(ast, attributeID);
			if( attr != null && attr.Value != null && attr.Value.Count > 0 )
			{
				return attr.Value[0].ValueID;
			}
			return 0;
		}

		/// <summary>
		/// Remove an attribute from the AttributeSetType object.
		/// </summary>
		/// <param name="ast"></param>
		/// <param name="attributeID"></param>
		/// <returns>True means the attribute has been found and removed.</returns>
		public static bool RemoveAttribute(AttributeSetType ast, int attributeID)
		{
			AttributeType attr = FindAttribute(ast, attributeID);
			if( attr != null )
			{
				ast.Attribute.Remove(attr);
				return true;
			}
			return false;
		}
	}
}
