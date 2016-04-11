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

using UnitTests.Helper;
#endregion

namespace AllTestsSuite.T_050_ItemTestsSuite
{
	/// <summary>
	/// Summary description for T_185_PlaceOfferLibrary.
	/// </summary>
	[TestFixture]
	public class T_185_PlaceOfferLibrary: SOAPTestBase
	{
		/// <summary>
		/// make a bid
		/// </summary>
		[Test]
		public void PlaceOfferFull()
		{
			string originalToken = this.apiContext.ApiCredential.eBayToken;

			try
			{

				Assert.IsNotNull(TestData.ChineseAuctionItem,"ChineseAuctionItem is null");
				ItemType item=TestData.ChineseAuctionItem;
				//change user
				this.apiContext.ApiCredential.eBayToken=TestData.BuyerToken;

				PlaceOfferCall api=new PlaceOfferCall(this.apiContext);
				api.ItemID=TestData.ChineseAuctionItem.ItemID;

				OfferType offer = new OfferType();
				offer.Action = BidActionCodeType.Bid;
				offer.Quantity = 1;
				AmountType at = new AmountType();
				at.Value = 1;
				offer.ConvertedPrice = at;
				at.Value=2;
				offer.MaxBid=at;
				api.Offer=offer;
                api.AbstractRequest.EndUserIP = "10.249.72.135";
			
				api.Execute();

				//check whether the call is success.
				Assert.IsTrue(api.ApiResponse.Ack==AckCodeType.Success || api.ApiResponse.Ack==AckCodeType.Warning,"do not success!");
			}
			finally
			{
				//reset token
				this.apiContext.ApiCredential.eBayToken=originalToken;
			}
			
		}
	}
}
