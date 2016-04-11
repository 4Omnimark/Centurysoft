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

namespace AllTestsSuite.T_060_MyeBayTestsSuite
{
	[TestFixture]
	public class T_020_GetMyeBayBuyingLibrary : SOAPTestBase
	{
		[Test]
		public void GetMyeBayBuying()
		{
			GetMyeBayBuyingCall api = new GetMyeBayBuyingCall(this.apiContext);
			if (this.apiContext.SoapApiServerUrl.IndexOf("sandbox") != -1)
				return;
			GetMyeBayBuyingRequestType req = new GetMyeBayBuyingRequestType();
			api.ApiRequest = req;
			ItemListCustomizationType lc = new ItemListCustomizationType();
			lc.Include = true; lc.IncludeSpecified = true;
			lc.IncludeNotes = true; lc.IncludeNotesSpecified = true;
			lc.Sort = ItemSortTypeCodeType.EndTime;
			req.BestOfferList = lc;
			lc = new ItemListCustomizationType();
			lc.Include = true; lc.IncludeSpecified = true;
			lc.IncludeNotes = true; lc.IncludeNotesSpecified = true;
			lc.Sort = ItemSortTypeCodeType.BidCount;
			req.BidList = lc;
			lc = new ItemListCustomizationType();
			lc.Include = true; lc.IncludeSpecified = true;
			lc.IncludeNotes = true; lc.IncludeNotesSpecified = true;
			lc.Sort = ItemSortTypeCodeType.EndTime;
			req.LostList = lc;
			lc = new ItemListCustomizationType();
			lc.Include = true; lc.IncludeSpecified = true;
			lc.IncludeNotes = true; lc.IncludeNotesSpecified = true;
			lc.Sort = ItemSortTypeCodeType.Price;
			req.WonList = lc;
			MyeBaySelectionType selection = new MyeBaySelectionType();
			selection.Include = true; selection.IncludeSpecified = true;
			selection.Sort = SortOrderCodeType.Ascending;
			req.FavoriteSearches = selection;
			MyeBaySelectionType sellerSel = new MyeBaySelectionType();
			sellerSel.Include = true; sellerSel.IncludeSpecified = true;
			sellerSel.Sort = SortOrderCodeType.Ascending;
			req.FavoriteSellers = sellerSel;
			// Make API call.
			api.GetMyeBayBuying();
			GetMyeBayBuyingResponseType resp = api.ApiResponse;
			
		}
	}
}