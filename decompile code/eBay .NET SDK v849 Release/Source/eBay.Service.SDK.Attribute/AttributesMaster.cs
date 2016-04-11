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
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.Resources;
using System.Runtime.InteropServices;
using MSXML2;

using eBay.Service.Core.Soap;
using eBay.Service.Core.Sdk;
using eBay.Service.Call;
using eBay.Service.Util;
using eBay.Service.SDK.Util;

namespace eBay.Service.SDK.Attribute
{
	/// <summary>
	/// Implements interface <c>IAttributesMaster</c>.
	/// </summary>
	[ClassInterface(ClassInterfaceType.None)]
	public class AttributesMaster : IAttributesMaster
	{
		private const string CAT_CS_ID = "cat_cs_id";
		private const string ATTRIBUTE_SET = "AttributeSet";
		private const string ID = "id";
		private const string ATTRIBUTE = "Attribute";
		private const string VALUE = "Value";
		private const string NAME = "Name";
		private const string SELECTED_ATTRIBUTES = "SelectedAttributes";
		private const string RETURN_POLICY = "Return Policy";
		private const string RETURN_POLICY_PAGE_ID = "ReturnPolicy";
		private const string PAGE_ID = "pageId";

		private IAttributesXslProvider mXslProvider;
		private IAttributesXmlProvider mXmlProvider;
		private ICategoryCSProvider mCategoryCSProvider;

		private DOMDocument30 mXslDoc = null;
		private DOMDocument30 mXmlToRender = null;

		/// <summary>
		/// Constructor.
		/// </summary>
		public AttributesMaster()
		{
		}
		#region Public methods
		
		/// <summary>
		/// The Attributes XSL provider.
		/// </summary>
		public IAttributesXslProvider XslProvider
		{ 
			get { return mXslProvider; }
			set 
			{ 
				mXslProvider = value; 
				if( mXslProvider != null )
				{
					// Load XSL.
					DOMDocument30 doc30 = new DOMDocument30();
					doc30.async = false;
					doc30.loadXML(mXslProvider.GetXslText());
					mXslDoc = doc30;
				}
				else
					mXslDoc = null;
			}
		}

		
		/// <summary>
		/// The Attributes XML provider.
		/// </summary>
		public IAttributesXmlProvider XmlProvider
		{ 
			get { return mXmlProvider; }
			set { mXmlProvider = value; }
		}

		
		/// <summary>
		/// The Attributes CategoryCS provider. You only need to set this property if you
		/// are going to call <c>RenderHtmlForCategories</c> method.
		/// </summary>
		public ICategoryCSProvider CategoryCSProvider
		{ 
			get { return mCategoryCSProvider; }
			set { mCategoryCSProvider = value; }
		}


		/// <summary>
		/// Extract <c>IAttributeSetCollection</c> object from <c>IKeyValueCollection</c> object.
		/// </summary>
		/// <param name="nameValues">The <c>IKeyValueCollection</c> to be converted.</param>
		/// <returns>The extracted <c>IAttributeSetCollection</c> object.</returns>
		public IAttributeSetCollection NameValuesToAttributeSets(IKeyValueCollection nameValues)
		{
			IAttributeSetCollection attrSets = new AttributeSetCollection();			
			AttributeSet attrSet;
			NameValueCollection nvs = AttributesHelper.ConvertToNameValues(nameValues);
			for(int ordinal = 0; ; ordinal++ )
			{
				attrSet = ExtractOneCat(CAT_CS_ID + ordinal.ToString(), nvs);
				if( attrSet != null )
				{
					attrSet.CategoryOrdinal = attrSets.Count;
					attrSets.Add(attrSet);
				}
				else
					break;
			}

			return attrSets;
		}

		/// <summary>
		/// Render HTML text by specifying list of AttributeSet.
		/// </summary>
		/// <param name="attrSets">List of AttributeSet objects.</param>
		/// <param name="errorList">The <c>IErrorSetCollection</c> object returned by <c>Validate</c> method.
		/// Set null if you don't have one.</param>
		/// <returns>The generated HTML text that is encapsulated in HTML table element.</returns>
		public string RenderHtml(IAttributeSetCollection attrSets, IErrorSetCollection errorList)
		{
			StringBuilder sb = new StringBuilder();

			// Add identification information for parsing.
			int ordinal = 0;
			foreach(AttributeSet attrSet in attrSets)
			{
				string goodProdId = (attrSet.ProductID != null && attrSet.ProductID.Length > 0) ? attrSet.ProductID : "";
				AddHiddenInputTag(sb, CAT_CS_ID + ordinal.ToString(), 
					attrSet.CategoryID.ToString() + "_" + attrSet.attributeSetID.ToString() + "_" + goodProdId);
				ordinal ++;
			}

			XmlDocument xmlDoc = mXmlProvider.GetMultipleCSXml(attrSets);

			// Add SelectedAttributes node.
			XmlNode selectedAttributes = xmlDoc.CreateElement(SELECTED_ATTRIBUTES);
			XmlNode eBayNode = xmlDoc.SelectSingleNode("//eBay");
			eBayNode.AppendChild(selectedAttributes);
			foreach(AttributeSet attrSet in attrSets) 
			{
				string attrSetName = attrSet.Name;
				if(attrSetName != null && attrSetName.Equals(RETURN_POLICY)) 
				{
					XmlAttribute retPolicyAttr = xmlDoc.CreateAttribute(PAGE_ID);
					retPolicyAttr.Value = RETURN_POLICY_PAGE_ID;
					selectedAttributes.Attributes.Append(retPolicyAttr);
				} 
				selectedAttributes.AppendChild(GetSelectedAttributesXml(xmlDoc, attrSet));
			}

			// Add error node.
			XmlNode errNode = null;
			if( errorList != null && errorList.Count != 0 ) 
			{
				errNode = AddErrorElements(xmlDoc, eBayNode, errorList);
			}
			// Generate html text.
			DOMDocument30 doc = new DOMDocument30();
			doc.loadXML(xmlDoc.InnerXml);
			mXmlToRender = doc;
			string table = doc.transformNode(this.mXslDoc);
			sb.Append(table);

			// Cleanup.
			if( errNode != null ) 
			{
				eBayNode.RemoveChild(errNode);
			}
			eBayNode.RemoveChild(selectedAttributes);

			return sb.ToString();
		}

		/// <summary>
		/// get the Xml to render
		/// </summary>
		/// <returns>DOMDocument30</returns>
		public DOMDocument30 getXmlToRenderDoc() 
		{
			return mXmlToRender;
		}
		
		/// <summary>
		/// Render HTML text by specifying list of AttributeSet and xsl Document. 
		/// </summary>
		/// <param name="attrSets">List of AttributeSet objects.</param>
		/// <param name="xslDoc">Xsl Document</param>
		/// <param name="errorList">The <c>IErrorSetCollection</c> object returned by <c>Validate</c> method.
		/// Set null if you don't have one.</param>
		/// <returns>The generated HTML text that is encapsulated in HTML table element.</returns>
		public string RenderHtml(IAttributeSetCollection attrSets, DOMDocument30 xslDoc, IErrorSetCollection errorList) 
		{
			mXslDoc = xslDoc;
			return RenderHtml(attrSets, errorList);
		}

		/// <summary>
		/// Render HTML text by raw name-value pairs that you got during HTML submit. 
		/// </summary>
		/// <param name="nameValues">List of name-value pairs from submit of attributes HTML form
		/// generated by all these RenderHtml methods.</param>
		/// <param name="errorList">The <c>IErrorSetCollection</c> object returned by <c>Validate</c> method.
		/// Set null if you don't have one.</param>
		/// <returns>The generated HTML text that is encapsulated in HTML table element.</returns>
		public string RenderHtmlForPostback(IKeyValueCollection nameValues, IErrorSetCollection errorList)
		{
			IAttributeSetCollection attrSets = NameValuesToAttributeSets(nameValues);
			return RenderHtml(attrSets, errorList);
		}


		/// <summary>
		/// Convert attribute set array to the format of ItemType for AddItemCall.
		/// </summary>
		/// <param name="attrSets">The attribute set list generated by AttributeMaster.</param>
		/// <returns>The converted array that is compatible with ItemType in AddItemCall.</returns>
		public AttributeSetTypeCollection ConvertAttributeSetArray(IAttributeSetCollection attrSets)
		{
			AttributeSetTypeCollection toSets = new AttributeSetTypeCollection();

			foreach(AttributeSetType from in attrSets)
			{
				AttributeSetType toAst = new AttributeSetType();

				toAst.Any = from.Any;
				toAst.attributeSetID = from.attributeSetID;
				toAst.attributeSetIDSpecified = from.attributeSetIDSpecified;
				toAst.attributeSetVersion = from.attributeSetVersion;
				toAst.Attribute = ConvertAttributeArray(from.Attribute);

				toSets.Add(toAst);
			}

			return toSets;
		}

		
		/// <summary>
		/// Creates an array AttributeSet objects which contains item specific attribute sets and
		/// site wide attribute sets with the exception of the attribute set for return policy. 
		/// </summary>
		/// <param name="itemSpecAttrSets">IAttributeSetCollection</param>
		/// <param name="swAttrSets">IAttributeSetCollection</param>
		/// <returns>Joined collection of of item specific attributes sets and site wide attribute sets, except Return Policy</returns>
		public IAttributeSetCollection JoinItemSpecificAndSiteWideAttributeSets(IAttributeSetCollection itemSpecAttrSets, IAttributeSetCollection swAttrSets) 
		{
			if(swAttrSets == null || swAttrSets.Count == 0) 
			{
				if(itemSpecAttrSets == null || itemSpecAttrSets.Count == 0) 
				{
					return null;
				} 
				else 
				{
					return itemSpecAttrSets;
				}
			}
	  
			// Exclude Return Policy:
			IAttributeSetCollection swAttrNoRetPolicySets = ExcludeReturnPolicyFromSiteWideAttributes(swAttrSets);
			if(swAttrNoRetPolicySets == null || swAttrNoRetPolicySets.Count == 0) 
			{
				return itemSpecAttrSets;
			}

			// Append Site Wide attributes to the Site Specific attributes:
			IAttributeSetCollection joinedAttrSet = new AttributeSetCollection();
			if(itemSpecAttrSets!=null)
			{
				foreach(AttributeSet itemSpecAttrSet in itemSpecAttrSets) 
				{
					joinedAttrSet.Add(itemSpecAttrSet);
				}
			}
			foreach(AttributeSet swAttrNoRetPolicySet in swAttrNoRetPolicySets) 
			{
				joinedAttrSet.Add(swAttrNoRetPolicySet);
			}
	  
			return joinedAttrSet;
		}

		
		/// <summary>
		/// Returns collection of item specific AttributeSet objects for an array category Ids.
		/// Each element of the array contains a VCS Id, if it exists for a given category Id. 
		/// </summary>
		/// <param name="catIds">int[]</param>
		/// <returns>Collection of item specific AttributeSet objects</returns>
		public IAttributeSetCollection GetItemSpecificAttributeSetsForCategories(Int32Collection catIds) 
		{
			IAttributeSetCollection sets = new AttributeSetCollection();
			int i = 0;
			foreach(int catId in catIds) 
			{
				AttributeSet ast = new AttributeSet();
				int csId = mCategoryCSProvider.GetVCSId(catId);
				if (csId == 0) 
				{
					return null;
					//throw new SdkException("Unable to get CSID by category Id");
				}
				ast.attributeSetID = csId;
				ast.CategoryID = catId;
				ast.CategoryOrdinal = i++;

				sets.Add(ast);
			}

			return sets;
		}

		
		/// <summary>
		/// Extracts AttributeSet object for Return Policy from site wide attribute sets.
		/// </summary>
		/// <param name="siteWideAttrSets">IAttributeSetCollection</param>
		/// <returns>Return Policy AttributeSet object</returns>
		public AttributeSet GetReturnPolicyAttributeSet(IAttributeSetCollection siteWideAttrSets) 
		{
			AttributeSet retVal = null;
			foreach(AttributeSet attrSet in siteWideAttrSets)
			{
				if(attrSet.Name.Equals(RETURN_POLICY)) 
				{
					retVal = attrSet;
					break;
				}
			}	
	    
			return retVal;
		}

		/// <summary>
		/// Returns collection of Site Wide AttributeSet objects for a given array category Ids.
		/// Each element of the array contains a VCS Id, if it exists for a given category Id. 
		/// </summary>
		/// <param name="catIds">int[]</param>
		/// <returns>Collection of site wide AttributeSet objects</returns>
		public IAttributeSetCollection GetSiteWideAttributeSetsForCategories(Int32Collection catIds) 
		{
			IAttributeSetCollection attrSetsList = new AttributeSetCollection();
			int i = 0;
			foreach(int catId in catIds) 
			{
				SiteWideCharacteristicsTypeCollection swAttrs = mCategoryCSProvider.GetSiteWideCharacteristics(catId.ToString());
				foreach(SiteWideCharacteristicsType swChar in swAttrs) 
				{
					AttributeSet swAst = new AttributeSet();
					swAst.attributeSetID = swChar.CharacteristicsSet.AttributeSetID;
					swAst.CategoryID = catId;
					swAst.CategoryOrdinal = i++;
					swAst.Name = swChar.CharacteristicsSet.Name;
					attrSetsList.Add(swAst);
				}
			}

			return attrSetsList;
		}
		#endregion

		#region Private Methods
		private AttributeSet ExtractOneCat(string catCsName, NameValueCollection request)
		{
			AttributeSet attrSet = null;
			string cat_cs = request[catCsName];
			if (cat_cs != null)
			{
				string[] delim = cat_cs.Split(new char[] {'_'});
				int catId = Int32.Parse(delim[0]);
				int csId = Int32.Parse(delim[1]);
				//int csId = this.mCategoryCSProvider.GetVCSId(catId);
				string prodId= delim[2];
				if (csId != 0) 
				{
					attrSet = AttrParamParser.Parse(csId.ToString(), request);
					attrSet.CategoryID = catId;
					attrSet.attributeSetID = csId;

					if( prodId.Length > 1 )
					{
						attrSet.ProductID = prodId;
					}
				}
			}

			return attrSet;
		}

		private static XmlNode AddErrorElements(XmlDocument doc, XmlNode eBayNode, IErrorSetCollection errList)
		{
			XmlNode errors = doc.CreateElement("Errors");
			eBayNode.AppendChild(errors);
			foreach (ErrorSet errSet in errList) 
			{
				XmlNode err = errSet.toXml(doc);
				errors.AppendChild(err);
			}

			return errors;
		}

		private static void AddDateToValueNode(XmlNode valNode, string date)
		{
			string year = "", month = "", day = "";

			try
			{
				year = date.Substring(0, 4);
				month = date.Substring(4, 2);
				day = date.Substring(6, 2);
			}
			catch(Exception)
			{
				//string t = ex.Message;
			}

			XmlUtility.AddChild(valNode.OwnerDocument, valNode, "Year", year);
			XmlUtility.AddChild(valNode.OwnerDocument, valNode, "Month", month);
			XmlUtility.AddChild(valNode.OwnerDocument, valNode, "Day", day);
		}

		private void CheckSingleValueListItem(AttributeType attr)
		{
			if( attr.Value.Count != 1 )
				throw new SdkException("Invalid AttributeType object. One one value in ValueList is expected.");
		}

		private XmlNode GetSelectedAttributesXml(XmlDocument doc, AttributeSet attrSet)
		{
			XmlNode attrSetNode = doc.CreateElement(ATTRIBUTE_SET);
			XmlUtility.AddAttributeNode(doc, attrSetNode, ID, attrSet.attributeSetID.ToString());

			if( attrSet.Attribute != null )
			{
				foreach(Attribute attr in attrSet.Attribute)
				{
					XmlNode attrNode = XmlUtility.AddChild(doc, attrSetNode, ATTRIBUTE);
					XmlUtility.AddAttributeNode(doc, attrNode, ID, attr.attributeID.ToString());

					XmlNode valueElement;
					Value val;
					if( attr.Type == AttributeTypes.ATTR_ID )
					{
						CheckSingleValueListItem(attr);
						val = (Value)attr.Value[0];

						valueElement = XmlUtility.AddChild(doc, attrNode, VALUE);
						XmlUtility.AddAttributeNode(doc, valueElement, ID, val.ValueID);

						if( val.ValueID == (int)ValueIds.OTHER && val.ValueLiteral != null )
						{
							// Add one more Attribute node with the same id.
							XmlNode attrOther = XmlUtility.AddChild(doc, attrSetNode, ATTRIBUTE);
							XmlUtility.AddAttributeNode(doc, attrOther, ID, attr.attributeID.ToString());

							XmlNode v_e = XmlUtility.AddChild(doc, attrOther, VALUE);
							XmlUtility.AddChild(doc, v_e, NAME, val.ValueLiteral);
						}
					}
					else if( attr.Type == AttributeTypes.ATTR_IDS )
					{
						foreach(Value v_s in attr.Value )
						{
							valueElement = XmlUtility.AddChild(doc, attrNode, VALUE);
							XmlUtility.AddAttributeNode(doc, valueElement, ID, v_s.ValueID);
						}
					}
					else if( attr.Type == AttributeTypes.ATTR_TEXT )
					{
						CheckSingleValueListItem(attr);
						val = (Value)attr.Value[0];

						valueElement = XmlUtility.AddChild(doc, attrNode, VALUE);
						XmlUtility.AddChild(doc, valueElement, NAME, val.ValueLiteral);

						/*
						if( val.ValueId == (int)ValueIds.OTHER )
						{
							// Add another attribute node with the same attribute Id
							XmlNode attrOther = XmlUtility.AddChild(doc, attrSetNode, ATTRIBUTE);
							XmlUtility.AddAttributeNode(doc, attrOther, ID, attr.AttributeId.ToString());

							valueElement = XmlUtility.AddChild(doc, attrOther, VALUE);
							XmlUtility.AddAttributeNode(doc, valueElement, ID, val.ValueId.ToString());
						}
						*/
					}
					else if( attr.Type == AttributeTypes.ATTR_TEXT_DATE )
					{
						CheckSingleValueListItem(attr);
						val = (Value)attr.Value[0];
					
						valueElement = XmlUtility.AddChild(doc, attrNode, VALUE);
						XmlUtility.AddAttributeNode(doc, valueElement, ID, val.ValueID);

						AddDateToValueNode(valueElement, val.ValueLiteral);
					}
					else
					{
						// Unsupported attribute type.
						foreach(Value v_o in attr.Value )
						{
							valueElement = XmlUtility.AddChild(doc, attrNode, VALUE);
							XmlUtility.AddChild(doc, valueElement, NAME, v_o.ValueLiteral);

							if( v_o.ValueID != 0 )
								XmlUtility.AddAttributeNode(doc, valueElement, ID, v_o.ValueID.ToString());
						}
					}
				}
			}

			return attrSetNode;
		}

		private static string getRuleSelectString(int csId, int aId)
		{
			const string s = "//eBay/Characteristics/CharacteristicsSet";
			string select = s + "[@id='" + csId + "']";
			select += "/CharacteristicsList/Initial/Attribute";
			select += "[@id='" + aId + "']";
			select += "/ValidationRules/Rule";
			return select;
		}

		private static void AddHiddenInputTag(StringBuilder sb, string name, string val)
		{
			string str = "<input type=\"hidden\" name=\"" + name + "\" value=\"" + val + "\"/>";
			sb.Append(str);
		}

		/// <summary>
		/// Validate <c>IAttributeSetCollection</c> object against eBay Attributes rules.
		/// IErrorSetCollection.Count == 0 means validation succeeded. Otherwise means failure and you
		/// have to call the above RenderHtml... methods and pass in the <c>IErrorSetCollection</c> object
		/// to re-generate Attributes HTML text that contains all the error messages.
		/// </summary>
		/// <param name="attrSets">The <c>IAttributeSetCollection</c> object which you want to validate.</param>
		/// <returns>The returned <c>IAttributeSetCollection</c> object. IAttributeSetCollection == 0 means 
		/// validation succeeded.</returns>
		public IErrorSetCollection Validate(IAttributeSetCollection attrSets)
		{
			IErrorSetCollection errList = new ErrorSetCollection();

			foreach(AttributeSet attrSet in attrSets)
			{
				IErrorSet errSet = ValidateOneSet(attrSet);
				if (errSet != null) 
					errList.Add(errSet);
			}

			return errList;
		}

		private IErrorSet ValidateOneSet(AttributeSet attrSet)
		{
			IErrorSet errSet = null;

			XmlDocument xml = new XmlDocument();
			xml.LoadXml(this.XmlProvider.GetCSXmlText(attrSet));

			foreach(Attribute attr in attrSet.Attribute )
			{
				string select = getRuleSelectString(attrSet.attributeSetID, attr.attributeID);
				XmlNodeList rules = xml.SelectNodes(select);
				
				int rCnt = rules.Count;
				for (int r = 0; r < rCnt; r++) 
				{
					IError err = ValidateAttr(attr, rules.Item(r));
					if (err != null) 
					{
						if (errSet == null) 
							errSet = new ErrorSet(attrSet.attributeSetID);

						errSet.Add(err);
					}
				}
			}

			return errSet;
		}

		private IError ValidateAttr(Attribute attr, XmlNode rule)
		{
			ValidationRule v = null;
			ValidationResult result = null;

			try 
			{
				v = new ValidationRule(rule);
				object [] vParams = ValidationParams.getValidationParams(attr, v);
				result = (ValidationResult)SimpleValidator.Validate(v.name, vParams);
			}
			catch(Exception) 
			{
			}

			if (!result.Success) 
			{
				IError err = new Error();
				err.AttrId = attr.attributeID;
				err.RuleName = v.name;
				err.Message = result.ErrorMessage; //SimpleValidator.getErrorMessage(v.name);
				return err;
			}

			return null;
		}

		private static ValTypeCollection ConvertValTypeCollection(ValTypeCollection fromVals)
		{
			ValTypeCollection toVals = new ValTypeCollection();

			foreach(ValType fromVal in fromVals)
			{
				ValType newVal = new ValType();

				newVal.Any = fromVal.Any;
				newVal.SuggestedValueLiteral = fromVal.SuggestedValueLiteral;
				newVal.ValueID = fromVal.ValueID;
				newVal.ValueIDSpecified = fromVal.ValueIDSpecified;
				newVal.ValueLiteral = fromVal.ValueLiteral;

				toVals.Add(newVal);
			}

			return toVals;
		}

		private static AttributeTypeCollection ConvertAttributeArray(AttributeTypeCollection from)
		{
			AttributeTypeCollection toAttrs = new AttributeTypeCollection();

			foreach(AttributeType fromA in from)
			{
				AttributeType toA = new AttributeType();

				toA.Any = fromA.Any;
				toA.attributeID = fromA.attributeID;
				toA.attributeIDSpecified = fromA.attributeIDSpecified;
				toA.Value = ConvertValTypeCollection(fromA.Value);

				toAttrs.Add(toA);
			}

			return toAttrs;
		}

		private IAttributeSetCollection ExcludeReturnPolicyFromSiteWideAttributes(IAttributeSetCollection siteWideAttributeSet) 
		{
			if(siteWideAttributeSet == null) 
			{
				return null;
			}
			AttributeSet retPolicyAttrSet = GetReturnPolicyAttributeSet(siteWideAttributeSet);
			if(retPolicyAttrSet == null) 
			{
				return siteWideAttributeSet;
			}
			int retPolicyAttrId = retPolicyAttrSet.attributeSetID;
			IAttributeSetCollection attrList = new AttributeSetCollection();
			foreach(AttributeSet swAttrSet in siteWideAttributeSet) 
			{
				if(swAttrSet.attributeSetID == retPolicyAttrId) 
				{
					continue;
				} 
				else 
				{
					attrList.Add(swAttrSet);
				}
			}
			return attrList;
		}

		#endregion
	}
}
