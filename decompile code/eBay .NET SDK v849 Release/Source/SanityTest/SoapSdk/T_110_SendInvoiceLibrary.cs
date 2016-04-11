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

namespace AllTestsSuite.T_080_TransactionTestsSuite
{
	[TestFixture]
	public class T_110_SendInvoiceLibrary : SOAPTestBase
	{
		[Test]
		public void SendInvoice()
		{
			Assert.IsNotNull(TestData.NewItem, "Failed because no item available -- requires successful AddItem test");
			//
			SendInvoiceCall api = new SendInvoiceCall(this.apiContext);
			SendInvoiceRequestType req = new SendInvoiceRequestType();
			api.CheckoutInstructions = "SDK checkout instruction.";
			api.EmailCopyToSeller = true; req.EmailCopyToSellerSpecified = true;
			api.InsuranceFee = new AmountType(); api.InsuranceFee.Value = 2.0; api.InsuranceFee.currencyID = CurrencyCodeType.USD;
			api.InsuranceOption = InsuranceOptionCodeType.Required;
			api.ItemID = TestData.NewItem.ItemID;
			api.PayPalEmailAddress = "test@ebay.com";
			api.TransactionID = "0";

			// Make API call.
			ApiException gotException = null;
			try
			{
			api.Execute();
			}
			catch(ApiException ex)
			{
				gotException = ex;
			}
			Assert.IsNotNull(gotException);
			
		}
	}
}