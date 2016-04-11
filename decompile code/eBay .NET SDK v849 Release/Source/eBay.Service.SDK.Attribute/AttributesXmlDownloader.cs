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
	/// Implements <c>IAttributesXmlProvider</c>. It gets CS xml data by calling eBay API.
	/// This implementation does not support catalog.
	/// </summary>
	public class AttributesXmlDownloader : IAttributesXmlProvider
	{
		private ApiContext apiContext = null;
		private XmlDocument mXml = null;
		private string currentVersion;
		private const string ATTR_XML_FILE_EXTENSION = "attrcs";
		private string ATTR_XML_FILE_NAME_PREFIX;
		private string ATTR_XML_FILE_NAME;
		private string ROOT_DIR; 
		/// <summary>
		/// Name of the XSL Overrides node.
		/// </summary>
		internal const string OverridesName = "API.XSL.Overrides";

		/// <summary>
		/// Name of the XSL GlobalSettings node.
		/// </summary>
		internal const string GlobalSettingsName = "GlobalSettings";

		private const string CS_ID = "CSId";

		/// <summary>
		/// XPath of AttributeSet node.
		/// </summary>
		public const string SELECT_AS = "/eBay//Attributes/AttributeSet[@id=\"{0}\"]";

		/// <summary>
		/// XPath of CharacteristicsSet node.
		/// </summary>
		public const string SELECT_CS = "/eBay//Characteristics/CharacteristicsSet[@id=\"{0}\"]";

		/// <summary>
		/// XPath of all CharacteristicsSet nodes.
		/// </summary>
		public const string SELECT_ALL_CS = "/eBay//Characteristics/CharacteristicsSet";

		/// <summary>
		/// XPath of overrides node.
		/// </summary>
		public const string SELECT_OVERRIDES = "/eBay//" + OverridesName;

		/// <summary>
		/// XPath of version node.
		/// </summary>
		public const string SELECT_VERSION = "/eBay//Version";

		/// <summary>
		/// XPath of GlobalSettings node.
		/// </summary>
		public const string SELECT_GLOBALSETTINGS = "/eBay//" + GlobalSettingsName;


		private static readonly string XmlDeclaration = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n";

		/// <summary>
		/// Constructor.
		/// </summary>
		public AttributesXmlDownloader()
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public AttributesXmlDownloader(ApiContext context)
		{
			apiContext = context;
		}

		private void init() 
		{
			if(currentVersion == null || currentVersion.Length == 0) 
			{
				currentVersion = getCurrentAttributeSystemVersion();
			}
			string site = apiContext.Site.ToString();
			ROOT_DIR = "C:\\TEMP";	//System.Environment.GetEnvironmentVariable(AttributesMaster.SDK_ENV_NAME);
			DirectoryInfo dirInfo = new DirectoryInfo(ROOT_DIR);
			if(!dirInfo.Exists) 
			{
				dirInfo.Create();
			}
			string[] dataFiles = getDataFiles(dirInfo);
			bool foundVersion = false;
			bool foundAllVersion = false;
			string fileNamePrefix = site + "." + ATTR_XML_FILE_NAME_PREFIX + "." + currentVersion;
			if(dataFiles != null && dataFiles.Length != 0) 
			{
				foundVersion = FoundVersion(dataFiles, fileNamePrefix);
				if(!foundVersion) 
				{
					foundAllVersion = FoundVersion(dataFiles, site + "." + "ALL" + "." + currentVersion);
				} 
				if(foundAllVersion) 
				{
					fileNamePrefix = site + "." + "ALL" + "." + currentVersion;
				}
			}
			ATTR_XML_FILE_NAME = ROOT_DIR + "\\" + fileNamePrefix + "." + ATTR_XML_FILE_EXTENSION;
			if(foundVersion || foundAllVersion) 
			{
				mXml = loadXmlFile(ATTR_XML_FILE_NAME);
			} 
			else 
			{
				DownloadXml();
				saveXmlFile(mXml, ATTR_XML_FILE_NAME);
			}
		}

		private XmlDocument loadXmlFile(string fileName) 
		{
			XmlDocument doc = new XmlDocument();
			XmlTextReader reader = new XmlTextReader(fileName);
			//reader.Read();
			doc.Load(reader);

			return doc;
		} 

		//This method writes a XML document to a file
		private void saveXmlFile(XmlDocument doc, string filename) 
		{
			XmlTextWriter writer = new XmlTextWriter(filename, null);
			writer.Formatting = Formatting.Indented;
			doc.Save(writer);
			writer.Close();
		}

		private bool FoundVersion(string[] dataFiles, string fileNamePrefix) 
		{
			for(int i = 0; i < dataFiles.Length; i++) 
			{
				if(dataFiles[i].StartsWith(fileNamePrefix)) 
				{
					return true;
				} 
			}
			return false;
		}

		private string createXmlFilePrefix(Int32[] csIdsArray) 
		{
			if(csIdsArray == null || csIdsArray.Length == 0) 
			{
				return "ALL";
			}
			int attrIdSum = 0;
			for(int i = 0; i < csIdsArray.Length; i++) 
			{
				int attrId = csIdsArray[i];
				attrIdSum += attrId;
			}
			return attrIdSum.ToString();

		}

		private string[] getDataFiles(DirectoryInfo dir) 
		{
			FileInfo[] attrXmlFiles = dir.GetFiles("*." + ATTR_XML_FILE_EXTENSION);
			string site = apiContext.Site.ToString();
			ArrayList fileNameList = new ArrayList();
			for(int i = 0; i < attrXmlFiles.Length; i++) 
			{
				string fname = attrXmlFiles[i].Name;
				bool acceptIt = (fname.StartsWith(site + "." + ATTR_XML_FILE_NAME_PREFIX) || fname.StartsWith(site + "." + "ALL"));
				if(acceptIt) 
				{
					fileNameList.Add(fname);
				}
			}
			string[] fnameArray = new string[fileNameList.Count];
			int j = 0;
			foreach(Object obj in fileNameList) 
			{
				fnameArray[j] = (string)obj;
				j++;
			}
			return fnameArray;
		}

		private bool validateCurrentVersion() {
			string currentSystemVersion = getCurrentAttributeSystemVersion();
			if(currentVersion == null) 
			{
				currentVersion = currentSystemVersion;
				return false;
			}
			return currentVersion.Equals(currentSystemVersion);
		}

		private string getCurrentAttributeSystemVersion() {
			GetAttributesCSCall api = new GetAttributesCSCall(apiContext);
			api.DetailLevelList.Add(DetailLevelCodeType.ReturnSummary);
		    
			return api.GetAttributesCSVersion();  
		}

		/// <summary>
		/// Get CS xml data by calling eBay API.
		/// </summary>
		public XmlDocument DownloadXml() 
		{
			return DownloadXml(apiContext);
		}
	   

		/// <summary>
		/// Get CS xml data by calling eBay API.
		/// </summary>
		/// <param name="asn">The <c>ApiContext</c> object to make API call.</param>
		public XmlDocument DownloadXml(ApiContext asn)
		{
			this.apiContext = asn;

			//
			GetAttributesCSCall api = new GetAttributesCSCall(asn);
			//api.ErrorLevel = ErrorLevelEnum.BothShortAndLongErrorStrings;
			api.DetailLevelList.Add(DetailLevelCodeType.ReturnAll);
			api.Timeout = 480000;
			
			string csXml = XmlDeclaration + "\n" + api.GetAttributesCS();

			// Apply style XSL.
			string xslText = GetDefaultStyleXsl();
			string fixedXml = FixAttributesXml(csXml, xslText);

			XmlDocument xml = new XmlDocument();
			xml.LoadXml(fixedXml);
			
			mXml = xml;

			return mXml;
		}


		/// <summary>
		/// Get CharacteristicSet(CS) xml. This implementation does not support
		/// catalog, i.e., csInfo.ProductId will be ignored.
		/// </summary>
		/// <param name="csInfo">Identification information about the CS for which 
		/// you want to get CSXml. You only need to set CS.CSId and, optionally, you can
		/// set CS.ProductId if you want to get the CSXml that is associated with
		/// specific product information.
		/// Set CS.CSId to 0 to get entire CharacteristicSet xml for all CSs.</param>
		/// <returns>The xml text of the CS.</returns>
		public string GetCSXmlText(AttributeSet csInfo)
		{
			if(csInfo == null) 
			{
				return null;
			}
			Int32Collection csIds = new Int32Collection();
			csIds.Add(csInfo.attributeSetID);

			ATTR_XML_FILE_NAME_PREFIX = createXmlFilePrefix(csIds.ToArray());

			if(!validateCurrentVersion()) 
			{
				init();
			}

			XmlDocument xml = ExtractCSFromXml(this.mXml, csInfo.attributeSetID, true);
			return xml.InnerXml;
		}


		/// <summary>
		/// Get xml that contains multiple CSs. Only set AttributeSet.CSId
		/// and AttributeSet.ProductId (optional).
		/// </summary>
		/// <param name="asList">List of <c>AttributeSet</c> objects for which
		/// you want to get CSXml.</param>
		/// <returns>The CS xml text for specified CSs.</returns>
		public string GetMultipleCSXmlText(IAttributeSetCollection asList)
		{
			XmlDocument xml = GetMultipleCSXml(asList);
			return xml.InnerXml;

		}

		/// <summary>
		/// Get xml that contains multiple CSs. Only set AttributeSet.CSId
		/// and AttributeSet.ProductId (optional).
		/// </summary>
		/// <param name="asList">List of <c>AttributeSet</c> objects for which
		/// you want to get CSXml.</param>
		/// <returns>The CS xml for specified CSs.</returns>
		public XmlDocument GetMultipleCSXml(IAttributeSetCollection asList)
		{
			bool validVersion = validateCurrentVersion();
			Int32Collection csIds = new Int32Collection();
			if(asList != null) 
			{
				foreach(AttributeSet ast in asList) 
				{
					if(ast == null) 
					{
						continue;
					}
					csIds.Add(ast.attributeSetID);
				}
			}
			ATTR_XML_FILE_NAME_PREFIX = createXmlFilePrefix(csIds.ToArray());
			DownloadXml();
			if(!validateCurrentVersion()) 
			{
				init();
			}

			XmlDocument xml = ExtractMultiCSFromXml(mXml, csIds, true);
			
			return xml;
		}

		/// <summary>
		/// Extract CS xml by CSId from combined CS Xml document.
		/// </summary>
		/// <param name="allCSXml">The CS Xml document that contains multiple CS Xml.</param>
		/// <param name="csId">Id of the CS that you want to get Xml for. Set to null to return
		/// xml for all CS.</param>
		/// <param name="copyOverrides">Copy Overrides node or not..</param>
		/// <returns>The new generated Xml document that contains the specified CS Xml.</returns>
		public static XmlDocument ExtractCSFromXml(XmlDocument allCSXml, int csId, bool copyOverrides)
		{
			Int32Collection csIds = null;
			if( csId != 0 )
			{
				csIds = new Int32Collection();
				csIds.Add(csId);
			}
			return ExtractMultiCSFromXml(allCSXml, csIds, copyOverrides);
		}

		/// <summary>
		/// Extract CS xml by list of CSId from combined CS Xml document.
		/// </summary>
		/// <param name="allCSXml">The CS Xml document that contains multiple CS Xml.</param>
		/// <param name="csIds">List of CSIDs that you want to get Xml for. Set to null to return
		/// xml for all CS.</param>
		/// <param name="copyOverrides">Copy Overrides node or not..</param>
		/// <returns>The new generated Xml document that contains the specified CS Xml.</returns>
		public static XmlDocument ExtractMultiCSFromXml(XmlDocument allCSXml, Int32Collection csIds, bool copyOverrides)
		{
			XmlDocument retXml = null;
			if( csIds == null || csIds.Count == 0 )
				retXml = allCSXml;
			else
			{
				XmlNode verNode = allCSXml.SelectSingleNode(SELECT_VERSION);

				//
				retXml = new XmlDocument();
				XmlNode node = retXml.CreateXmlDeclaration("1.0", System.Text.Encoding.Unicode.BodyName, null);
				retXml.AppendChild(node);

				XmlNode eBayNode = XmlUtility.AddChild(retXml, retXml, "eBay");
				retXml.AppendChild(eBayNode);

				XmlNode asNode = XmlUtility.AddChild(retXml, eBayNode, "Attributes");
				XmlNode csNode = XmlUtility.AddChild(retXml, eBayNode, "Characteristics");

				if( verNode != null )
					eBayNode.AppendChild(retXml.ImportNode(verNode, true));

				XmlNode fromNode;

				foreach(int csId in csIds)
				{
					// Copy Attributes/AttributeSet
					fromNode = allCSXml.SelectSingleNode(String.Format(SELECT_AS, csId));
					if( fromNode == null ) 
                    {
                        string expMsg = "ExtractMultiCSFromXml: Unable to find source Attributes node the given CSId.\r\n";
                        expMsg += "There is error in the attributes meta-data, please check if the category is \r\n";
                        expMsg += "item specifics enabled(using GetCategoryFeatures) and use eBay item specifics related API instead.\r\n";
                        expMsg += "You may also contact eBay and report the error.";
						throw new SdkException(expMsg);
                    }
					asNode.AppendChild(retXml.ImportNode(fromNode, true));

					// Copy Characteristics/CharacteristicsSet
					fromNode = allCSXml.SelectSingleNode(String.Format(SELECT_CS, csId));
					if( fromNode == null )
						throw new SdkException("ExtractMultiCSFromXml: Unable to find source Characteristics node the given CSId.");
					csNode.AppendChild(retXml.ImportNode(fromNode, true));
				}

				// Copy eBay//API.XSL.Overrides
				if( copyOverrides )
				{
					fromNode = allCSXml.SelectSingleNode(SELECT_OVERRIDES);
					if( fromNode != null )
						eBayNode.AppendChild(retXml.ImportNode(fromNode, true));

					// Copy GlobalSettings
					fromNode = allCSXml.SelectSingleNode(SELECT_GLOBALSETTINGS);
					if( fromNode != null )
						eBayNode.AppendChild(retXml.ImportNode(fromNode, true));
				}
			}

			return retXml;
		}

		/// <summary>
		/// Get the default Style XSL.
		/// </summary>
		/// <returns>The default Style Xsl text.</returns>
		public static string GetDefaultStyleXsl()
		{
			Stream xslStrm = typeof(AttributesXmlDownloader).Assembly.GetManifestResourceStream(
				"eBay.Service.SDK.Attribute.Resources.Attributes_Style.xsl");

			StreamReader sr = new StreamReader(xslStrm);
			return sr.ReadToEnd();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="attrXmlDoc"></param>
		/// <param name="styleXsl"></param>
		/// <returns></returns>
		internal static string FixAttributesXml(string attrXmlDoc, string styleXsl)
		{
			//Disable the generation of HTML elements from the stylesheet

			//
            System.Xml.Xsl.XslCompiledTransform xsl = new XslCompiledTransform();

			StringReader sr = new StringReader(styleXsl);
			XmlTextReader xtr = new XmlTextReader(sr);
			xsl.Load(xtr);

            XmlReader reader = XmlReader.Create(new StringReader(attrXmlDoc));
            //String to store the resulting transformed XML
            StringBuilder resultString = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(resultString);

			xsl.Transform(reader, writer);
			return resultString.ToString();
		}
		/// <summary>
		/// Get all the CS IDs. 
		/// </summary>
		/// <returns>Array of CSIDs.</returns>
		public int[] GetCSIDs()
		{
			if(mXml == null) 
			{
				DownloadXml();
			}
			XmlDocument allCSXML    = mXml;
			XmlNodeList listCSNodes = allCSXML.SelectNodes( SELECT_ALL_CS );
            
			int[]       aRet = new int[listCSNodes.Count];
			int         i    = 0;
             
			foreach ( XmlNode nd in listCSNodes )
			{
				XmlElement e = (XmlElement)nd;
				string strAttrId = e.GetAttribute( "id" );
				aRet[i] = int.Parse( strAttrId );
				i++;
			}
    
			return aRet;
		}
	}
}