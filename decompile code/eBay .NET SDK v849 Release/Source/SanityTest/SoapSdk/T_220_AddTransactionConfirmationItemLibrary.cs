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

namespace AllTestsSuite.T_050_ItemTestsSuite
{
	[TestFixture]
	public class T_220_AddTransactionConfirmationItemLibrary : SOAPTestBase
	{
		/// <summary>
		/// Ends the listing specified by ItemID (if listed for at least 24 hours).
		/// 
		/// </summary>
		[Test]
		public void AddTransactionConfirmationItem()
		{
			AddTransactionConfirmationItemCall api = new AddTransactionConfirmationItemCall(this.apiContext);
			ApiException gotException = null;
			try 
			{
				AmountType a = new AmountType();
				a.Value = 2.0;
				a.currencyID = CurrencyCodeType.USD;
				api.AddTransactionConfirmationItem("apitest20", "true", "94566", RecipientRelationCodeType.Item1, a, SecondChanceOfferDurationCodeType.Days_7, "1111111111", "Comment");
			} 
			catch (ApiException ex) 
			{
				gotException = ex;
			}
			Assert.IsNotNull(gotException);

		}
	}
}