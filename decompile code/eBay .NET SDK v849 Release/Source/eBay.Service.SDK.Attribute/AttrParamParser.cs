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
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Xml;

using eBay.Service.Core.Soap;
using eBay.Service.Core.Sdk;

namespace eBay.Service.SDK.Attribute
{
	internal struct AttrInfo 
	{
		public AttributeTypes typeId;
	    public string csid;
		public string attrId;
		public string val;
	}

	/// <summary>
	/// Summary description for AttrParamParser.
	/// </summary>
	internal class AttrParamParser
	{
		private AttrParamParser()
		{
		}

		public static AttributeSet Parse(string vcsid, NameValueCollection request)
		{
			int index;
			Hashtable htAttr = new Hashtable();

			foreach (string key in request.AllKeys) 
			{
				if( !(key.StartsWith(ATTR + vcsid) ||
					key.StartsWith(ATTR_D + vcsid) ||
					key.StartsWith(ATTR_T + vcsid))
					)
					continue;

				AttributeTypes type = AttributeTypes.ATTR_ID;

				string val = request[key];
				string skey = key.Substring(ATTR.Length);
				if (skey.StartsWith(USD)) 
				{
					index = skey.LastIndexOf(US);
					string ekey = skey.Substring(index);
					skey = skey.Substring(0, index);
					skey = skey.Substring(USD.Length);
					if (USC.Equals(ekey)) 
					{
						type = AttributeTypes.ATTR_TEXT;
					}
					else if (USD.Equals(ekey)) 
					{
						type = AttributeTypes.ATTR_DATE_D;
					}
					else if (USM.Equals(ekey)) 
					{
						type = AttributeTypes.ATTR_DATE_M;
					}
					else if (USY.Equals(ekey)) 
					{
						type = AttributeTypes.ATTR_DATE_Y;
					}
				}
				else if (skey.StartsWith(UST))
				{
					skey = skey.Substring(UST.Length);
					type = AttributeTypes.ATTR_TEXT;
				}
				else 
				{
					if (val.IndexOf(SEP) > -1) 
					{
						type = AttributeTypes.ATTR_IDS;
					}
					else 
					{
						type = AttributeTypes.ATTR_ID;
					}
				}

				AttrInfo info = new AttrInfo();
				info.typeId = type;

				// Peel of the vcsid
				index = skey.IndexOf(US);
				info.attrId = skey.Substring(index + 1);
				info.csid = skey.Substring(0, index);
				info.val = 	request[key];
				skey = info.attrId;

				object obj = htAttr[info.attrId];
				if (obj != null)
				{
					if (obj is ArrayList) 
					{
						((ArrayList)obj).Add(info);
					}
					else 
					{
						ArrayList al = new ArrayList();
						al.Add(obj);
						al.Add(info);
						htAttr.Remove(info.attrId);
						htAttr.Add(skey, al);
					}
				}
				else 
				{									
					htAttr.Add(info.attrId, info);
				}
			}

			return Compile(htAttr, vcsid);
		}

		private static void PadString(ref string str, int desiredLen)
		{
			if( str == null )
				str = new string('0', desiredLen);
			else
			{
				int len = str.Length;
				if( len > desiredLen )
					str = str.Substring(0, desiredLen);
				else if( len < desiredLen )
					str = new string('0', desiredLen - len) + str;
			}
		}

		private static void FixYMD(ref string year, ref string month, ref string day)
		{
			PadString(ref year, 4);
			PadString(ref month, 2);
			PadString(ref day, 2);
		}

		private static AttributeSet Compile(Hashtable ht, string vcsid)
		{
			AttributeSet attrSet = new AttributeSet();
			attrSet.attributeSetID = int.Parse(vcsid);
			attrSet.Attribute = new AttributeTypeCollection();

			int cnt = ht.Count;
			ICollection coll = ht.Keys;
			IEnumerator iter = coll.GetEnumerator();
		
			string key;
			object val;
			AttrInfo info;
			ArrayList al;
			Attribute attr = null;

			while(iter.MoveNext()) 
			{
				key = iter.Current.ToString();                
				val = ht[key];
				if (val is ArrayList) 
				{
					al = (ArrayList)val;
					cnt = al.Count;
					// Option Other
					if (cnt == 2) 
					{
						AttrInfo info0 = (AttrInfo)al[0];
						AttrInfo info1 = (AttrInfo)al[1];
						if (info0.typeId == AttributeTypes.ATTR_ID) 
						{
							attr = ExtractAttr2(info0, info1);
							attrSet.Attribute.Add(attr);
						}
						else if (info1.typeId == AttributeTypes.ATTR_ID)
						{
							attr = ExtractAttr2(info1, info0);
							attrSet.Attribute.Add(attr);
						}
						else 
						{
							//Unknow type.
						}
					}
					// DateTime
					else if (cnt == 3) 
					{
						string day = null, month = null, year = null;
						for (int i = 0; i < cnt; i ++) 
						{
							info = (AttrInfo)al[i];
							switch(info.typeId) 
							{
								case AttributeTypes.ATTR_DATE_D:
									day = info.val;
									break;
								case AttributeTypes.ATTR_DATE_M:
									month = info.val;
									break;
								case AttributeTypes.ATTR_DATE_Y:
									year = info.val;
									break;
							}
						}

						FixYMD(ref year, ref month, ref day);

						if (day != null && month!= null && year != null) 
						{
							info = (AttrInfo)al[0];
							al.RemoveRange(0, cnt);
						
							info.typeId = AttributeTypes.ATTR_TEXT_DATE;
							info.val = year + month + day;
							al.Add(info);

							attr = ExtractAttr(info);
							attr.Value[0].ValueID = (int)ValueIds.COMPLETE_TEXT_DATE;

							attrSet.Attribute.Add(attr);
						}
					}
				}
				else 
				{
					info = (AttrInfo) val;

					// Only year field.
					if( info.typeId == AttributeTypes.ATTR_DATE_Y )
					{
						string year = info.val, month = "0", day = "0";
						FixYMD(ref year, ref month, ref day);

						info.typeId = AttributeTypes.ATTR_TEXT_DATE;
						info.val = year + month + day;
						
						attr = ExtractAttr(info);
						attr.Value[0].ValueID = (int)ValueIds.COMPLETE_TEXT_DATE;

						attrSet.Attribute.Add(attr);
					}
					else
					{
						attr = ExtractAttr(info);
						if( attr.Type == AttributeTypes.ATTR_TEXT )
							attr.Value[0].ValueID = (int)ValueIds.TEXT;

						attrSet.Attribute.Add(attr);
					}
				}
			}

			return attrSet;
		}

		private static ValueTypes AttrToValType(AttributeTypes attrType)
		{
			if( attrType == AttributeTypes.ATTR_ID || attrType == AttributeTypes.ATTR_IDS )
				return ValueTypes.ValueId;
			else
				return ValueTypes.Text;
		}

		private static Attribute ExtractAttr(AttrInfo info)
		{
			Value val = null;
			Attribute attr = new Attribute();
			attr.attributeID = int.Parse(info.attrId);
			attr.Type = info.typeId;
			attr.Value = new ValTypeCollection();

			ValueTypes valType = AttrToValType(info.typeId);

			if (info.typeId == AttributeTypes.ATTR_ID) 
			{
				val = new Value(valType);
				val.ValueID = int.Parse(info.val);
				attr.Value.Add(val);

				/*
				if (info.val.Equals("-10")) 
				{
					attr.SType = -1;
				}
				*/
			}
			else if (info.typeId == AttributeTypes.ATTR_IDS)
			{
				string [] ss = info.val.Split(SEPS);
				for (int i = 0; i < ss.Length; i++) 
				{
					val = new Value(valType);
					val.ValueID = int.Parse(ss[i]);
					attr.Value.Add(val);
				}
				/*
				if (info.val.IndexOf("-10") > -1) 
				{
					attr.SType = -1;
				}
				*/
			}
			else 
			{
				val = new Value(valType);
				val.ValueLiteral = info.val;
				attr.Value.Add(val);
			}

			return attr;
		}

		private static Attribute ExtractAttr2(AttrInfo info1, AttrInfo info2)
		{
			Attribute attr = new Attribute();
			attr.Value = new ValTypeCollection();
			attr.attributeID = int.Parse(info1.attrId);
			attr.Type = info1.typeId;

			Value val = new Value();
			val.ValueID = int.Parse(info1.val);
			val.ValueLiteral = info2.val;
			attr.Value.Add(val);

			return attr;
		}

		private static Attribute ExtractAttr(AttrInfo [] infos)
		{
			Value val = null;
			Attribute attr = new Attribute();
			attr.attributeID = int.Parse(infos[0].attrId);
			attr.Value = new ValTypeCollection();

			AttrInfo info;
			int cnt = infos.Length;
			for (int i = 0; i < cnt; i ++) 
			{
				info = infos[i];
				if (info.typeId == AttributeTypes.ATTR_ID) 
				{
					val = new Value();
					val.ValueID = int.Parse(info.val);
					attr.Value.Add(val);
				}
				else if (info.typeId == AttributeTypes.ATTR_IDS)
				{
					string [] ss = info.val.Split(SEPS);
					for (int j = 0; j < ss.Length; j++) 
					{
						val = new Value();
						val.ValueID = int.Parse(ss[j]);
						attr.Value.Add(val);
					}
				}
				else 
				{
					val = new Value();
					val.ValueLiteral = info.val;
					attr.Value.Add(val);
				}
			}

			return attr;
		}

		public const string ATTR = "attr";
		public const string ATTR_D = "attr_d";
		public const string ATTR_T = "attr_t";

		public const string US = "_";
		public const string USC = "_c";
		public const string USD = "_d";
		public const string USM = "_m";
		public const string UST = "_t";
		public const string USY = "_y";

		public const string SEP = ",";
		public static char [] SEPS = {','};

		private int ATTR_OFFSET = ATTR.Length + 1;
	}
}
