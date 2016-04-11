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
	public class T_090_SetUserPreferencesLibrary : SOAPTestBase
	{
		[Test]
		public void SetUserPreferences()
		{
			if( TestData.UserPreferencesResponse == null )
			return;
			SetUserPreferencesCall api = new SetUserPreferencesCall(this.apiContext);
			api.BidderNoticePreferences = TestData.UserPreferencesResponse.BidderNoticePreferences;
	//		api.CombinedPaymentPreferences = TestData.UserPreferencesResponse.CombinedPaymentPreferences;
			api.CrossPromotionPreferences = TestData.UserPreferencesResponse.CrossPromotionPreferences;
			api.EndOfAuctionEmailPreferences = TestData.UserPreferencesResponse.EndOfAuctionEmailPreferences;
			api.SellerFavoriteItemPreferences = TestData.UserPreferencesResponse.SellerFavoriteItemPreferences;
			api.SellerPaymentPreferences = TestData.UserPreferencesResponse.SellerPaymentPreferences;
			// Make API call.
			ApiException gotException = null;
			try 
			{
				api.Execute();
			}
			catch (ApiException e)
			{
				gotException = e;
			}
			Assert.IsTrue(gotException == null || gotException.containsErrorCode("249"));
		}
	}
}