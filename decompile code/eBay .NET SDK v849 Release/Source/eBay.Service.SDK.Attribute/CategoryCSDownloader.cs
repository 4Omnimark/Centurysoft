using System;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Xml;
using System.Xml.Xsl;
using System.Resources;
using System.Runtime.InteropServices;
using MSXML2;
using System.Xml.Serialization;

using eBay.Service.Core.Soap;
using eBay.Service.Core.Sdk;
using eBay.Service.Call;
using eBay.Service.Util;
using eBay.Service.SDK.Util;

namespace eBay.Service.SDK.Attribute
{
	/// <summary>
	/// Implements <c>ICategoryCSProvider</c>. It gets CategoryCS data by calling eBay API.
	/// </summary>
	public class CategoryCSDownloader : ICategoryCSProvider
	{
		private CategoryTypeCollection mCats = null;
		private ApiContext apiContext;
		private string currentVersion;
		private const string CATCS_FILE_EXTENSION = "catcs";
		private const string SW_FILE_EXTENSION = "swcs";
		private string CATCS_FILE_NAME_PREFIX;
		private string CATCS_FILE_NAME;
		private string SW_FILE_NAME;
		private string ROOT_DIR; 
		private SiteWideCharacteristicsTypeCollection mSiteWideCharacteristicSets;

		/// <summary>
		/// Constructor.
		/// </summary>
		public CategoryCSDownloader()
		{
		}
		/// <summary>
		/// Constructor.
		/// </summary>
		public CategoryCSDownloader(ApiContext apiContext)
		{
			this.apiContext = apiContext;
		}

		private void init(string catId) 
		{
			if(currentVersion == null || currentVersion.Length == 0) 
			{
				currentVersion = getCurrentAttributeSystemVersion();
			}
			string site = apiContext.Site.ToString();
			CATCS_FILE_NAME_PREFIX = (catId == null)?"ALL":catId;
			ROOT_DIR = "C:\\TEMP";	//System.Environment.GetEnvironmentVariable(AttributesMaster.SDK_ENV_NAME);
			DirectoryInfo dirInfo = new DirectoryInfo(ROOT_DIR);
			if(!dirInfo.Exists) 
			{
				dirInfo.Create();
			}
			string fileNamePrefix = site + "." + CATCS_FILE_NAME_PREFIX + "." + currentVersion;
			string[] dataFiles = getDataFiles(dirInfo);
			bool foundVersion = false;
			bool foundAllVersion = false;
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
			CATCS_FILE_NAME = ROOT_DIR + "\\" + fileNamePrefix + "." + CATCS_FILE_EXTENSION;
			SW_FILE_NAME = ROOT_DIR + "\\" + fileNamePrefix + "." + SW_FILE_EXTENSION;
			if(foundVersion || foundAllVersion) 
			{
				mCats = readCategoryTypeObjectFromDisk();
				mSiteWideCharacteristicSets	= readSiteWideCharTypeObjectFromDisk();
			} 
			else 
			{
				mCats = DownloadCategoryCS(catId);
				writeCategoryTypeObjectToDisk(mCats);
				writeSiteWideCharTypeObjectToDisk(mSiteWideCharacteristicSets);
			}
		}

		private CategoryTypeCollection readCategoryTypeObjectFromDisk() 
		{
			XmlSerializer serializer = new XmlSerializer(typeof(CategoryTypeCollection));
			FileStream fs = new FileStream(CATCS_FILE_NAME, FileMode.Open);
			CategoryTypeCollection categoryTypes = (CategoryTypeCollection)serializer.Deserialize(fs);
			fs.Close();
			return categoryTypes;
		}

		private SiteWideCharacteristicsTypeCollection readSiteWideCharTypeObjectFromDisk() 
		{
			XmlSerializer serializer = new XmlSerializer(typeof(SiteWideCharacteristicsTypeCollection));
			FileStream fs = new FileStream(SW_FILE_NAME, FileMode.Open);
			SiteWideCharacteristicsTypeCollection siteWideCharacters = (SiteWideCharacteristicsTypeCollection)serializer.Deserialize(fs);
			fs.Close();

			return siteWideCharacters;
		}

		private void writeCategoryTypeObjectToDisk(CategoryTypeCollection obj) 
		{
			XmlSerializer serializer =  new XmlSerializer(typeof(CategoryTypeCollection));
			TextWriter writer = new StreamWriter(CATCS_FILE_NAME);
			serializer.Serialize(writer, obj);
			writer.Close();
		}

		private void writeSiteWideCharTypeObjectToDisk(SiteWideCharacteristicsTypeCollection obj) 
		{
			XmlSerializer serializer =  new XmlSerializer(typeof(SiteWideCharacteristicsTypeCollection));
			TextWriter writer = new StreamWriter(SW_FILE_NAME);
			serializer.Serialize(writer, obj);
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

		private string[] getDataFiles(DirectoryInfo dir) 
		{
			FileInfo[] attrXmlFiles = dir.GetFiles("*." + CATCS_FILE_EXTENSION);
			string site = apiContext.Site.ToString();
			ArrayList fileNameList = new ArrayList();
			for(int i = 0; i < attrXmlFiles.Length; i++) 
			{
				string fname = attrXmlFiles[i].Name;
				bool acceptIt = (fname.StartsWith(site + "." + CATCS_FILE_NAME_PREFIX) || fname.StartsWith(site + "." + "ALL"));
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

		private bool validate() 
		{
			string currentSystemVersion = getCurrentAttributeSystemVersion();
			if(currentVersion == null) 
			{
				currentVersion = currentSystemVersion;
				return false;
			}
			return currentVersion.Equals(currentSystemVersion);
		}

		private string getCurrentAttributeSystemVersion() 
		{
			GetCategory2CSCall api = new GetCategory2CSCall(apiContext);
			api.DetailLevelList.Add(DetailLevelCodeType.ReturnSummary);
		    
			return api.GetCategory2CSVersion();  
		}

		/// <summary>
		/// Get the categories data that it's using for conversion.
		/// </summary>
		public CategoryTypeCollection GetCategoriesCS(string catId)
		{
			if(!validate()) 
			{
				init(catId);
			}
			return mCats;
		}

		/// <summary>
		/// Get the categories data that it's using for conversion.
		/// </summary>
		public CategoryTypeCollection GetCategoriesCS() 
		{
			return GetCategoriesCS(null);
		}

		/// <summary>
		/// Get CategoryCS data by calling eBay API.
		/// </summary>
		/// <param name="asn"></param>
		public CategoryTypeCollection DownloadCategoryCS(ApiContext asn)
		{
			return DownloadCategoryCS(asn, null);
		}

		/// <summary>
		/// Get CategoryCS data by calling eBay API.
		/// </summary>
		public CategoryTypeCollection DownloadCategoryCS() 
		{
			return DownloadCategoryCS(apiContext, null);
		}

		/// <summary>
		/// Get CategoryCS data by calling eBay API.
		/// </summary>
		/// <param name="catId"></param>
		public CategoryTypeCollection DownloadCategoryCS(string catId) 
		{
			return DownloadCategoryCS(apiContext, catId);
		}

		/// <summary>
		/// Get CategoryCS data by calling eBay API. Special version for fast example usage.
		/// </summary>
		/// <param name="asn">The <c>ApiContext</c> object to make API call.</param>
		/// <param name="categoryId">A specific category ID for which to download CategoryCS data.</param>
		public CategoryTypeCollection DownloadCategoryCS(ApiContext asn, string categoryId)
		{
			GetCategory2CSCall	api = new GetCategory2CSCall(asn);
			//api.ErrorLevel = ErrorLevelEnum.BothShortAndLongErrorStrings;
			if (categoryId != null) 
			{
				api.CategoryID = categoryId;
			}
			api.DetailLevelList.Add(DetailLevelCodeType.ReturnAll);	//.DetailLevel = 1;
			api.Timeout = 480000;
			mCats = api.GetCategory2CS();
			mSiteWideCharacteristicSets = api.SiteWideCharacteristicList;

			return mCats;
		}


		/// <summary>
		/// Get CSId by categoryId.
		/// </summary>
		/// <param name="categoryId">The categoryId for which you want to get CSId.</param>
		/// <returns>The CSId.</returns>
		public int GetVCSId(int categoryId)
		{
			int[] vcsIds =  GetVCSIdArray(categoryId);
			return vcsIds[0];
		}

		/// <summary>
		/// Get response from GetCategory2CSCall
		/// </summary>
		/// <returns>CategoryTypeCollection</returns>
		public CategoryTypeCollection GetResponse() 
		{
			return GetResponse(null);
		}

		/// <summary>
		/// Get response from GetCategory2CSCall
		/// </summary>
		/// <param name="categoryId">The categoryId for which you want to get the response.</param>
		/// <returns>CategoryTypeCollection</returns>		
		public CategoryTypeCollection GetResponse(String categoryId)
		{
			if(!validate()) {
				init(categoryId);
			}
			return mCats;
		}		

		private bool isExcludedCategoryID(string catId, SiteWideCharacteristicsType swCharSet) 
		{
			bool isExcluded = false;
			eBay.Service.Core.Soap.StringCollection excluded = swCharSet.ExcludeCategoryID;
			IEnumerator myEnumerator = excluded.GetEnumerator();
			while(myEnumerator.MoveNext()) 
			{
				string current = (string)myEnumerator.Current;
				if(current.Equals(catId)) 
				{
					isExcluded = true;
					break;
				}
			}
			return isExcluded;
		}

		/// <summary>
		/// Get CSIdArray by categoryId.
		/// </summary>
		/// <param name="categoryId">The categoryId for which you want to get CSId array</param>
		public int[] GetVCSIdArray(int categoryId) 
		{
			Hashtable vcsIdMap =  GetVCSIdMap(categoryId);
			CharacteristicsSetType[] sets = (CharacteristicsSetType[])vcsIdMap[categoryId.ToString()];
			int setsLength = (sets == null)?1:sets.Length;
			int[] vcsIds = new int[setsLength];
			if(sets == null) 
			{
				vcsIds[0] = 0;
			} 
			else 
			{
				for(int i = 0; i < setsLength; i++) 
				{
					vcsIds[i] = sets[i].AttributeSetID;
				}
			}
			return vcsIds;
		}

		/// <summary>
		/// Get getVCSIdMap by categoryId.
		/// </summary>
		/// <param name="categoryId">The categoryId for which you want to get CSId map.</param>
		public Hashtable GetVCSIdMap(int categoryId) 
		{
			Hashtable vcsIdMap = new Hashtable(1);
			if(!validate()) 
			{
				init(categoryId.ToString());
			}
			if(mCats == null || mCats.Count == 0) 
			{
				vcsIdMap.Add("-1", "-1"); // dummy value
				return vcsIdMap;
			}
			for(int i = 0; i < mCats.Count; i++) 
			{
				CategoryType cat = mCats[i];
				CharacteristicsSetType[] sets = cat.CharacteristicsSets.ToArray();
				if(sets != null && sets.Length > 0)
					vcsIdMap.Add(cat.CategoryID.ToString(), sets);
			}
			return vcsIdMap;
		}

		/// <summary>
		/// Get Site-Wide characteristics sets by category ID.
		/// </summary>
		/// <param name="catId">A specific category ID for which fetch Site-Wide CategoryCS data.</param>
		/// <returns>SiteWideCharacteristicsTypeCollection</returns>
		public SiteWideCharacteristicsTypeCollection GetSiteWideCharacteristics(string catId) 
		{
			if(!validate()) 
			{
				init(catId);
			}
			SiteWideCharacteristicsTypeCollection swAttrs = new SiteWideCharacteristicsTypeCollection();
			for(int i = 0; i < mSiteWideCharacteristicSets.Count; i++) 
			{
				SiteWideCharacteristicsType swCharSet = mSiteWideCharacteristicSets.ToArray()[i];
				if(isExcludedCategoryID(catId, swCharSet)) 
				{
					continue;
				}
				swAttrs.Add(swCharSet);
			}
			return swAttrs;
		}

		/// <summary>
		/// Get Site-Wide characteristics sets by category ID.
		/// </summary>
		/// <param name="catId">A specific category ID for which fetch Site-Wide CategoryCS data.</param>
		public int[] GetSiteWideCharSetsAttrIds(string catId) 
		{
			SiteWideCharacteristicsTypeCollection swAttrs = GetSiteWideCharacteristics(catId);
			Int32Collection attrSetIds = new Int32Collection();
			for(int i = 0; i < swAttrs.Count; i++) {
				SiteWideCharacteristicsType swCharSet = swAttrs.ToArray()[i];
				int attrSetId = swCharSet.CharacteristicsSet.AttributeSetID;
				attrSetIds.Add(attrSetId);
			}

			return attrSetIds.ToArray();
		}
		


	}
}