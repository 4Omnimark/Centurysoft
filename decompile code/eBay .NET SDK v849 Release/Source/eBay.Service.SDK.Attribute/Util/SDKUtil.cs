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
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Globalization;

namespace eBay.Service.SDK.Util
{
	/// <summary>
	/// Contains utility functions for SDK.
	/// </summary>
	[ClassInterface(ClassInterfaceType.None)]
	public class SDKUtil
	{
		/// <summary>
		/// DateTime value that represents NULL.
		/// </summary>
		public static DateTime NULL_TIME = new DateTime(1900,1,1);

		internal static IFormatProvider DateFormatProvider = new System.Globalization.CultureInfo("en-US");
		internal const string EBAY_TIME_FORMAT = "yyyy'-'MM'-'dd HH':'mm':'ss";
		internal const string EBAY_DATE_FORMAT = "yyyy'-'MM'-'dd";

		/// <summary>
		/// Convert an eBay date time string to <d>DateTime</d> object.
		/// </summary>
		/// <param name="dt">The string to be converted.</param>
		/// <returns>The <c>DateTime</c> object</returns>
		public static DateTime ParseEBayTime(string dt)
		{
			return DateTime.ParseExact(dt, EBAY_TIME_FORMAT, DateFormatProvider);
		}

		/// <summary>
		/// Convert <c>DateTime</c> object to eBay time string.
		/// </summary>
		/// <param name="dt">The DateTime object.</param>
		/// <returns>The converted string.</returns>
		public static string ToEBayTime(DateTime dt)
		{
			return dt.ToString(EBAY_TIME_FORMAT, DateFormatProvider);
		}

		/// <summary>
		/// Convert an eBay date string to <d>DateTime</d> object.
		/// </summary>
		/// <param name="date">The date string to be converted.</param>
		/// <returns>The <c>DateTime</c> object</returns>
		public static DateTime ParseEBayDate(string date)
		{
			return DateTime.ParseExact(date, EBAY_DATE_FORMAT, DateFormatProvider);
		}

		/// <summary>
		/// Convert <c>DateTime</c> object to eBay date string (yyyy-mm-dd).
		/// </summary>
		/// <param name="date">The DateTime object.</param>
		/// <returns>The converted date string.</returns>
		public static string ToEBayDate(DateTime date)
		{
			return date.ToString(EBAY_DATE_FORMAT, DateFormatProvider);
		}
		
		/// <summary>
		/// Compute the MD5 digest of input string then encode the results.
		/// </summary>
		/// <param name="strData">The input string.</param>
		/// <returns>The result string.</returns>
		public static string GetStringDigest(string strData)
		{
			//
			MD5 md5Digest = new MD5CryptoServiceProvider();
			UnicodeEncoding UE = new UnicodeEncoding();

			byte[] data = UE.GetBytes(strData);
			byte[] digest = md5Digest.ComputeHash(data);

			return System.Convert.ToBase64String(digest, 0, digest.Length);
		}

		/// <summary>
		/// Determines if the length of a string is greater than 0.
		/// </summary>
		/// <param name="str">The string object.</param>
		/// <returns>true means the input string is not null and its size > 0.</returns>
		public static bool IsNonZeroString(string str)
		{
			return str != null && str.Length > 0;
		}
	}
}
