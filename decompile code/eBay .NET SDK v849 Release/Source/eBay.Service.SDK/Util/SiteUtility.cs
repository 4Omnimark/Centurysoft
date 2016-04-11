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
using System.Collections;
using eBay.Service.Core.Soap;
using System.Runtime.InteropServices;
#endregion

namespace eBay.Service.Util
{

	/// <summary>
	/// 
	/// </summary>
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public class SiteUtility
	{

		#region Static Methods
		/// <summary>
		/// Gets the Site associated with the numeric site id.
		/// </summary>
		/// <param name="SiteID">The numeric site id.</param>
		/// <returns>The <see cref="SiteCodeType"/>.</returns>
		public static SiteCodeType GetSiteCodeType(int SiteID)
		{
			if (!Enum.IsDefined(typeof(SiteValueEnum), SiteID))
				return SiteCodeType.CustomCode;
			else
				return((SiteCodeType) Enum.Parse(typeof(SiteCodeType), Enum.GetName(typeof(SiteValueEnum), SiteID)));
			
		}

		/// <summary>
		/// Gets the numeric id associated with a site.
		/// </summary>
		/// <param name="SiteCodeType">The <see cref="SiteCodeType"/>.</param>
		/// <returns>The numeric id associated with the <see cref="SiteCodeType"/>.</returns>
		public static int GetSiteID(SiteCodeType SiteCodeType)
		{
			if (!Enum.IsDefined(typeof(SiteValueEnum), SiteCodeType.ToString()))
				return 0;
			else
				return (int) Enum.Parse(typeof(SiteValueEnum), Enum.GetName(typeof(SiteCodeType), SiteCodeType));

		}

		/// <summary>
		/// Gets the CountryCodeType associated with a site.
		/// </summary>
		/// <param name="SiteCodeType">The <see cref="SiteCodeType"/>.</param>
		/// <returns>CountryCodeType associted with the <see cref="SiteCodeType"/>.</returns>
		/// 
		public static CountryCodeType GetCountryCodeType(SiteCodeType SiteCodeType)
		{
			CountryCodeType curr;
			switch( SiteCodeType )
			{
				case SiteCodeType.US:
				case SiteCodeType.eBayMotors:
					curr = CountryCodeType.US;
					break;
				case SiteCodeType.UK:
					curr = CountryCodeType.GB;
					break;
				case SiteCodeType.Canada:
					curr = CountryCodeType.CA;
					break;
				case SiteCodeType.Australia:
					curr = CountryCodeType.AU;
					break;
				case SiteCodeType.Taiwan:
					curr = CountryCodeType.TW;
					break;
				case SiteCodeType.Switzerland:
					curr = CountryCodeType.CH;
					break;
				case SiteCodeType.HongKong:
					curr = CountryCodeType.HK;
					break;
				case SiteCodeType.Singapore:
					curr = CountryCodeType.SG;
					break;
				case SiteCodeType.Malaysia:
					curr = CountryCodeType.MY;
					break;
				case SiteCodeType.Philippines:
					curr = CountryCodeType.PH;
					break;
				case SiteCodeType.China:
					curr = CountryCodeType.CN;
					break;
				case SiteCodeType.India:
					curr = CountryCodeType.IN;
					break;
				case SiteCodeType.France:
					curr = CountryCodeType.FR;
					break;
				case SiteCodeType.Germany:
					curr = CountryCodeType.DE;
					break;
				case SiteCodeType.Italy:
					curr = CountryCodeType.IT;
					break;
				case SiteCodeType.Netherlands:
					curr = CountryCodeType.NL;
					break;
				case SiteCodeType.Belgium_Dutch:
				case SiteCodeType.Belgium_French:
					curr = CountryCodeType.BE;
					break;
				case SiteCodeType.Austria:
					curr = CountryCodeType.AT;
					break;
				case SiteCodeType.Spain:
					curr = CountryCodeType.ES;
					break;
				default:
					curr = CountryCodeType.US;
					break;
			}
			return curr;
		
		}
		#endregion

		#region Private Enumerations
		/// <summary>
		/// 
		/// </summary>
		private enum SiteValueEnum
		{

			Australia = 15,
			Austria = 16,
			Belgium_Dutch = 123,
			Belgium_French = 23,
			Canada = 2,
			China = 223,
			eBayMotors = 100,
			France = 71,
			Germany = 77,
			HongKong = 201,
			India = 203,
			Italy = 101,
			Malaysia = 207,
			Netherlands = 146,
			Philippines = 211,
			Singapore = 216,
			Spain = 186,
			Switzerland = 193,
			Taiwan = 196,
			UK = 3,
			US = 0,		

			// Internal only.
			Ireland = 205,
			NewZealand = 208,
			Poland = 212,
			Sweden = 218,
			CanadaFrench = 210
		}
		#endregion

	}
}
