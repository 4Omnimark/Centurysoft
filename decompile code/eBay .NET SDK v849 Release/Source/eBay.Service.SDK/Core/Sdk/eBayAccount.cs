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
using System.Runtime.InteropServices;
#endregion

namespace eBay.Service.Core.Sdk
{

	/// <summary>
	/// Used to store eBay user ID and password, normally as credentials needed for some 
    /// API calls. Normally, <see cref="ApiCredential.eBayToken"/> is used instead for
    /// API call access.
	/// </summary>
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public class eBayAccount 
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public eBayAccount()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="UserName">The eBay UserId to use.</param>
		/// <param name="Password">The user's password.</param>
		public eBayAccount(string UserName, string Password)
		{
			mUserName = UserName;
			mPassword = Password;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the user's password of type <see cref="string"/>.
		/// </summary>
		public string Password
		{ 
			get { return mPassword; }
			set { mPassword = value; }
		}

		/// <summary>
		/// Gets or sets the user's id of type <see cref="string"/>.
		/// </summary>
		public string UserName
		{ 
			get { return mUserName; }
			set { mUserName = value; }
		}
		#endregion

		/// <summary>
		/// this will return the UserName only
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return mUserName;
		}

		#region Private Fields
		private string mPassword = "";
		private string mUserName = "";
		#endregion

	}
}
