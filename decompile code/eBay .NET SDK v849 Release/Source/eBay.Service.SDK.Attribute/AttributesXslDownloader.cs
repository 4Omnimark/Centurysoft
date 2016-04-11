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
	/// Implements <c>IAttributesXslProvider</c>. It gets XSL data by calling eBay API.
	/// </summary>
	public class AttributesXslDownloader : IAttributesXslProvider
	{
		private ApiContext apiContext;
		private string currentVersion;
		private string ROOT_DIR; 
		private string XSL_FILE_NAME;
		private const string ATTR_XSL_FILE_EXTENSION = "attrxsl";
		private string mXslText;
		private string mXslFileName;

		/// <summary>
		/// Constructor.
		/// </summary>
		public AttributesXslDownloader()
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public AttributesXslDownloader(ApiContext apiContext)
		{
			this.apiContext = apiContext;
		}

		private void init() 
		{
			string site = apiContext.Site.ToString();
			ROOT_DIR = "C:\\TEMP";	//System.Environment.GetEnvironmentVariable(AttributesMaster.SDK_ENV_NAME);
			DirectoryInfo dirInfo = new DirectoryInfo(ROOT_DIR);
			if(!dirInfo.Exists) 
			{
				dirInfo.Create();
			}
			FileInfo[] dataFiles = getDataFiles(dirInfo);
			XSL_FILE_NAME = ROOT_DIR + "\\" + site + "." + currentVersion + "." + ATTR_XSL_FILE_EXTENSION;
			bool foundVersion = false;
			if(dataFiles.Length != 0)  
			{
				if(dataFiles != null) 
				{
					for(int i = 0; i < dataFiles.Length; i++) 
					{
						if(dataFiles[i].Name.StartsWith(site + "." + currentVersion)) 
						{
							foundVersion = true;
							break;
						}
					}
				}
			}
			if(foundVersion) 
			{
				try 
				{
					mXslText = loadXslFile();
				} 
				catch(IOException ioe) 
				{
					throw new SdkException("Error processing file: " + XSL_FILE_NAME + " : " + ioe.Message);
				}
			} 
			else 
			{
				mXslText = DownloadXsl(apiContext);
				saveXslTextInFile(mXslText);
			}
		}

		private void saveXslTextInFile(string xslText) {
			FileStream file = new FileStream(XSL_FILE_NAME, FileMode.CreateNew, FileAccess.Write);
			StreamWriter sw = new StreamWriter(file);
			sw.Write(xslText);
			sw.Close();	  
		}

		/// <summary>
		/// Download XSL text from eBay by calling eBay API.
		/// </summary>
		public string DownloadXsl()
		{
			return DownloadXsl(apiContext);
		}

		/// <summary>
		/// Download XSL text from eBay by calling eBay API.
		/// </summary>
		/// <param name="asn">The <c>ApiContext</c> object to make API call.</param>
		public string DownloadXsl(ApiContext asn)
		{
			GetAttributesXSLCall api = new GetAttributesXSLCall(asn);
			api.DetailLevelList.Add(DetailLevelCodeType.ReturnAll);
			
			//api.ErrorLevel = ErrorLevelEnum.BothShortAndLongErrorStrings;
			//api.DetailLevel = 1;

			XSLFileTypeCollection xslFiles = api.GetAttributesXSL();
			
			if( xslFiles.Count > 0 )
			{
				GetAttributesXSLCall.DecodeFileContent(xslFiles[0]);
				this.mXslText = xslFiles[0].FileContent;
				this.mXslFileName = xslFiles[0].FileName;
			}
			else 
			{
				this.mXslText = null;
			}
			return mXslText;

		}

		/// <summary>
		/// The XSL file name.
		/// </summary>
		public string GetXslFileName()
		{ 
			return mXslFileName;
		}

		/// <summary>
		/// The XSL text.
		/// </summary>
		public string GetXslText()
		{ 
			if(!validateCurrentVersion()) 
			{
				init();
			}
			return mXslText;
		}

		private string loadXslFile() {
			FileStream file = new FileStream(XSL_FILE_NAME, FileMode.OpenOrCreate, FileAccess.Read);
			StreamReader sr = new StreamReader(file);
			string s = sr.ReadToEnd();
			sr.Close();

			file.Close();
	   
			return s;
		}

		private bool validateCurrentVersion() {
			string currentFileVersion = getCurrentFileVersion();
			if(currentVersion == null) 
			{
				currentVersion = currentFileVersion;
				return false;
			}
			return this.currentVersion.Equals(currentFileVersion);
		}

		private FileInfo[] getDataFiles(DirectoryInfo dir) 
		{
			FileInfo[] attrXslFiles = dir.GetFiles("*." + ATTR_XSL_FILE_EXTENSION);
		    return attrXslFiles;
	    }

		private string getCurrentFileVersion() {
			GetAttributesXSLCall api = new GetAttributesXSLCall(apiContext);
			api.DetailLevelList.Add(DetailLevelCodeType.ReturnSummary);
			XSLFileTypeCollection xslFiles = api.GetAttributesXSL();
			string fileVersion = xslFiles[0].FileVersion;

			return fileVersion; 
		}
	}
}

