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
using System.Web;
using System.Collections;
using eBay.Service.Core.Sdk;
using System.Runtime.InteropServices;
#endregion

namespace eBay.Service.Util
{

	/// <summary>
	/// 
	/// </summary>
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public class SdkUtility
	{

		#region Static Methods
		/// <summary>
		/// Launches the sign in page for using the  Authentication &amp; Authorization feature.
		/// </summary>
		/// <param name="Context">The <see cref="ApiContext"/> which defines the SignInUrl and RuName.</param>
		/// <param name="SessionID">The SessionID which is used by <see cref="eBay.Service.Call.FetchTokenCall"/> to retrieve the token.</param>
		public static void LaunchSignInPage(ApiContext Context, string SessionID)
		{
			if(Context == null)
				throw new SdkException("Please specify the Context.", new System.ArgumentNullException());
			
			if(Context.SignInUrl == null || Context.SignInUrl.ToString().Length == 0)
				throw new SdkException("Please specify the SignInUrl in the Context object.", new System.ArgumentNullException());

			if (Context.RuName == null || Context.RuName.Length == 0)
				throw new SdkException("Please specify a RuName.", new System.ArgumentNullException());

			// Go to the page.
			string finalUrl = Context.SignInUrl + "&RuName=" + Context.RuName;
			if(SessionID != null && SessionID.Length > 0 )
				finalUrl += "&SessID=" + HttpUtility.UrlEncode(SessionID);

            System.Diagnostics.Process.Start(finalUrl);
		}
		#endregion
	}
}









