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
using eBay.Service.Util;
using eBay.Service.Call;


namespace Samples.Helper.Cache
{
	/// <summary>
	/// Helper class with cache function for GetCategoryFeatures call
	/// </summary>
	public class FeaturesDownloader:BaseDownloader
	{
		#region constructor

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="context"></param>
		public FeaturesDownloader(ApiContext context)
		{
			this.context=context;
			//must initialize some super class fields
			this.filePrefix = "AllCatFeatures";
			this.fileSuffix = "cfs";
			this.objType = typeof(GetCategoryFeaturesResponseType);
		}

		#endregion

		#region public methods

		/// <summary>
		/// Get all features for a given category
		/// </summary>
		public GetCategoryFeaturesResponseType GetCategoryFeatures()
		{
			object obj = getObject();
			return (GetCategoryFeaturesResponseType)obj;
		}

		#endregion


		#region private methods

		/// <summary>
		/// get last update time from site
		/// </summary>
		/// <returns>string</returns>
		protected override string getLastUpdateTime()
		{
			GetCategoryFeaturesCall api = new GetCategoryFeaturesCall(context);		
			//set output selector
			api.ApiRequest.OutputSelector = new StringCollection(new String[]{"CategoryVersion"});
			//api.ApiRequest.OutputSelector = new StringCollection(new String[]{"UpdateTime"});
			//execute call
			api.GetCategoryFeatures();

			//workaround, for GetCategoryFeaturesCall, we just use CategoryVersion as last update time
			return api.ApiResponse.CategoryVersion;
			//DateTime updateTime = api.ApiResponse.UpdateTime;

			//return updateTime.ToString("yyyy-MM-dd-hh-mm-ss");
		}


		/// <summary>
		/// call GetCategories to get all categories for a given site
		/// </summary>
		/// <returns>generic object</returns>
		protected override object callApi()
		{
			GetCategoryFeaturesCall api = new GetCategoryFeaturesCall(context);	
			//set detail level
			api.DetailLevelList.Add(DetailLevelCodeType.ReturnAll);
            api.ViewAllNodes = true;
			FeatureIDCodeTypeCollection featureCol = new FeatureIDCodeTypeCollection();
			featureCol.Add(FeatureIDCodeType.ListingDurations);
			featureCol.Add(FeatureIDCodeType.ItemSpecificsEnabled);
			featureCol.Add(FeatureIDCodeType.ReturnPolicyEnabled);
			featureCol.Add(FeatureIDCodeType.PaymentMethods);
            featureCol.Add(FeatureIDCodeType.ConditionEnabled);
            featureCol.Add(FeatureIDCodeType.ConditionValues);
			//execute call
			api.FeatureIDList = featureCol;
			api.GetCategoryFeatures();

			return api.ApiResponse;
		}


		#endregion


	}
}
