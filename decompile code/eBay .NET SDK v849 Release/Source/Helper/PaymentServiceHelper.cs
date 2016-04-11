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
using eBay.Service.Core.Soap;
using Samples.Helper.UI;

namespace Samples.Helper
{
	/// <summary>
	/// Summary description for PaymentServiceHelper.
	/// </summary>
	public class PaymentServiceHelper
	{
		private static PaymentServiceHelper _helper = new PaymentServiceHelper();

		Hashtable htPaymentMethods = new Hashtable();

		private PaymentServiceHelper()
		{
			Init();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static PaymentServiceHelper GetInstance()
		{
			return _helper;
		}

		private void Init()
		{
			InitPaymentMethods();
		}

		private void InitPaymentMethods()
		{
			ControlTagItem[] items = new ControlTagItem[] {
																  new ControlTagItem("Money Order or Cashier's Check", BuyerPaymentMethodCodeType.MOCC),
																  new ControlTagItem("Personal Check", BuyerPaymentMethodCodeType.PersonalCheck),
																  new ControlTagItem("Visa or Master Card", BuyerPaymentMethodCodeType.VisaMC),
																  new ControlTagItem("American Express", BuyerPaymentMethodCodeType.AmEx),
																  new ControlTagItem("Discover Card", BuyerPaymentMethodCodeType.Discover),
																  new ControlTagItem("Payment Option See Description", BuyerPaymentMethodCodeType.PaymentSeeDescription),
																  new ControlTagItem("PayPal", BuyerPaymentMethodCodeType.PayPal)};	
			this.htPaymentMethods.Add(SiteCodeType.US, items);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="site"></param>
		/// <returns></returns>
		public ControlTagItem[] GetPaymentMethods(SiteCodeType site)
		{
			return (ControlTagItem[])this.htPaymentMethods[site];
		}
	}
}
