#region Copyright
//	Copyright (c) 2013 eBay, Inc.
//	
//	This program is licensed under the terms of the eBay Common Development and
//	Distribution License (CDDL) Version 1.0 (the "License") and any subsequent  
//	version thereof released by eBay.  The then-current version of the License can be 
//	found at http://www.opensource.org/licenses/cddl1.php and in the eBaySDKLicense 
//	file that is under the eBay SDK ../docs directory
#endregion

#region Namespaces
using System;
using NUnit.Framework;
using eBay.Service.Call;
using eBay.Service.Core.Soap;
using eBay.Service.Core.Sdk;
using eBay.Service.Util;
#endregion

namespace AllTestsSuite.T_020_OtherTestsSuite
{
	[TestFixture]
	public class T_080_GetUserPreferencesLibrary : SOAPTestBase
	{
		[Test]
		public void GetUserPreferences()
		{
			GetUserPreferencesCall api = new GetUserPreferencesCall(this.apiContext);
			DetailLevelCodeType[] detailLevels = new DetailLevelCodeType[] {
			DetailLevelCodeType.ReturnAll
			};
			api.DetailLevelList = new DetailLevelCodeTypeCollection(detailLevels);
			api.ShowBidderNoticePreferences = true;
			api.ShowCombinedPaymentPreferences = true;
			api.ShowCrossPromotionPreferences = true;
			api.ShowEndOfAuctionEmailPreferences = true;
			api.ShowSellerFavoriteItemPreferences = true;
			api.ShowSellerPaymentPreferences = true;
			// Make API call.
			api.Execute();
			Assert.IsNotNull(api.ApiResponse.BidderNoticePreferences);
			Assert.IsNotNull(api.ApiResponse.CombinedPaymentPreferences);
			Assert.IsNotNull(api.ApiResponse.CrossPromotionPreferences);
			Assert.IsNotNull(api.ApiResponse.EndOfAuctionEmailPreferences);
			Assert.IsNotNull(api.ApiResponse.SellerPaymentPreferences);
			Assert.IsNotNull(api.ApiResponse.SellerPaymentPreferences);
			TestData.UserPreferencesResponse = (GetUserPreferencesResponseType)api.ApiResponse;
			
		}
	}
}