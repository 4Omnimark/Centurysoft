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
    /// Helper class with cache function for GeteBayDetails call
    /// </summary>
    public class DetailsDownloader : BaseDownloader
    {
        #region constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public DetailsDownloader(ApiContext context)
        {
            this.context = context;
            //must initialize some super class fields
            this.filePrefix = "EBayDetails";
            this.fileSuffix = "eds";
            this.objType = typeof(GeteBayDetailsResponseType);
        }

        #endregion

        #region public methods

        /// <summary>
        /// Get eBay details
        /// </summary>
        public GeteBayDetailsResponseType GeteBayDetails()
        {
            object obj = getObject();
            return (GeteBayDetailsResponseType)obj;
        }

        #endregion


        #region private methods

        /// <summary>
        /// get last update time from site
        /// </summary>
        /// <returns>string</returns>
        protected override string getLastUpdateTime()
        {
            GeteBayDetailsCall api = new GeteBayDetailsCall(context);
            //set output selector
            api.ApiRequest.OutputSelector = new StringCollection(new String[] { "UpdateTime" });

            //execute call
            DetailNameCodeTypeCollection dns = this.getDetailsNames();
            api.GeteBayDetails(dns);

            DateTime updateTime = api.ApiResponse.UpdateTime;

            return updateTime.ToString("yyyy-MM-dd-hh-mm-ss");
        }

        private DetailNameCodeTypeCollection getDetailsNames()
        {
        	DetailNameCodeTypeCollection names=new DetailNameCodeTypeCollection();
			names.Add(DetailNameCodeType.ShippingLocationDetails);
			names.Add(DetailNameCodeType.ShippingServiceDetails);
			names.Add(DetailNameCodeType.ReturnPolicyDetails);
            return names;
        }


        /// <summary>
        /// call GeteBayDetails to get eBay details for a given site
        /// </summary>
        /// <returns>generic object</returns>
        protected override object callApi()
        {
            GeteBayDetailsCall api = new GeteBayDetailsCall(context);
            DetailNameCodeTypeCollection dns = this.getDetailsNames();
            api.GeteBayDetails(dns);

            return api.ApiResponse;
        }


        #endregion


    }
}
