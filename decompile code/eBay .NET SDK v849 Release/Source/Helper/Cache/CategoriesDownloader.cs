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
using System.ComponentModel;
using System.Web;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlTypes;
using System.IO;
using System.Reflection;

using eBay.Service.Core.Soap;
using eBay.Service.Core.Sdk;
using eBay.Service.Util;
using eBay.Service.Call;
namespace Samples.Helper.Cache
{
	/// <summary>
	/// Helper class with cache function for GetCategories call
	/// </summary>
	public class CategoriesDownloader:BaseDownloader
	{
		#region constructor

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="context"></param>
		public CategoriesDownloader(ApiContext context)
		{
			this.context=context;
			//must initialize some super class fields
			this.filePrefix = "AllCategories";
			this.fileSuffix = "cats";
			this.objType = typeof(GetCategoriesResponseType);
		}

		#endregion

		#region public methods

		/// <summary>
		/// Get all categories for a given site
		/// </summary>
		public CategoryTypeCollection GetAllCategories()
		{
			object obj = getObject();
			GetCategoriesResponseType response = (GetCategoriesResponseType)obj;
			return response.CategoryArray;
		}

		#endregion
		
		#region protected methods

		
		/// <summary>
		/// get last update time from site
		/// </summary>
		/// <returns>string</returns>
		protected override string getLastUpdateTime()
		{
			GetCategoriesCall api = new GetCategoriesCall(context);			
			//set output selector
			api.ApiRequest.OutputSelector = new StringCollection(new String[]{"UpdateTime"});
			//execute call
			api.GetCategories();

			DateTime updateTime = api.ApiResponse.UpdateTime;

			return updateTime.ToString("yyyy-MM-dd-hh-mm-ss");
		}


		/// <summary>
		/// call GetCategories to get all categories for a given site
		/// </summary>
		/// <returns>generic object</returns>
		protected override object callApi()
		{
			GetCategoriesCall api = new GetCategoriesCall(context);
			//set detail level
			api.DetailLevelList.Add(DetailLevelCodeType.ReturnAll);	
			api.Timeout = 480000;
			api.ViewAllNodes =true;
			//execute call
			api.GetCategories();

			return api.ApiResponse;
		}

		#endregion

	}//close CategoriesDownloader class
}
