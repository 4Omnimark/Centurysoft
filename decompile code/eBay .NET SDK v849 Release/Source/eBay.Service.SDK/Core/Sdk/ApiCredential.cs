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
	/// Used to store credentials needed to make API calls to the eBay API server. Normally,
    /// <see cref="eBayToken"/> (which stores the authorization token) is used rather than the older
    /// <see cref="ApiAccount"/>. 
	/// </summary>
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public class ApiCredential
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public ApiCredential()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="eBayToken">The token to use when making API calls.</param>
		public ApiCredential(string eBayToken)
		{
			meBayToken = eBayToken;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Authorization (user) tokens are valid for about 18 months, after which time they
        /// expire. This property can be read to determine when the token expires.
		/// </summary>
		/// <param name="expirationDate"></param>
		public void TokenHardExpirationWarning(DateTime expirationDate)
		{
			if( this.OnTokenHardExpirationWarning != null )
				this.OnTokenHardExpirationWarning(this,  new TokenHardExpirationEventArgs(expirationDate));

		}
		#endregion

		#region Properties
		/// <summary>
        /// Gets or sets the ApiAccount object used to store program credentials (Application, Developer, 
        /// and Certificate IDs) if ApiAccount is used to supply these. This property is ignored if 
        /// eBayToken is set. (Normally, eBayToken should be 
        /// used instead of ApiAccount.) Type <see cref="ApiAccount"/>.
		/// </summary>
		public ApiAccount ApiAccount
		{ 
			get { return mApiAccount; }
			set { mApiAccount = value; }
		}

		/// <summary>
		/// Gets or sets the eBayAccount object used to store user credentials (eBay user id, 
        /// password). This property is ignored if eBayToken is set. (Normally, eBayToken should be 
        /// used instead.) Type <see cref="eBayAccount"/>.
		/// </summary>
		public eBayAccount eBayAccount
		{ 
			get { return meBayAccount; }
			set { meBayAccount = value; }
		}

		/// <summary>
        /// Gets or sets the eBayToken object used to store the authorization (user) token. Type <see cref="string"/>.
        /// This token is required in order to make API calls, as it represents the user's authorization of your 
        /// application to access eBay on behalf of that user. For more information on eBay authorization tokens,
        /// see <see href="http://developer.ebay.com/DevZone/XML/docs/WebHelp/GettingTokens-.html">Getting Tokens.</see>
		/// </summary>
		public string eBayToken
		{ 
			get { return meBayToken; }
			set { meBayToken = value; }
		}
		#endregion

		#region Events
		/// <summary>
		/// 
		/// </summary>
		public event TokenHardExpirationWarningEventHandler OnTokenHardExpirationWarning;
		#endregion

		#region Private Fields
		private ApiAccount mApiAccount =  new ApiAccount();
		private eBayAccount meBayAccount =  new eBayAccount();
		private string meBayToken = "";
		#endregion

	}

	#region TokenHardExpirationEventArgs
	/// <summary>
	/// 
	/// </summary>
	[ComVisible(false)]
	public class TokenHardExpirationEventArgs : EventArgs 
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ExpirationDate">The time the token expires of type <see cref="DateTime"/>.</param>
		public TokenHardExpirationEventArgs(DateTime ExpirationDate)
		{
			mExpirationDate = ExpirationDate;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the time the token will expire.
		/// </summary>
		public DateTime ExpirationDate
		{
			get { return mExpirationDate; }
		}
		#endregion

		#region Private Fields
		private DateTime mExpirationDate;
		#endregion
	}
	#endregion

	#region Delegates
	/// <summary>
	/// 
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">An <see cref="TokenHardExpirationEventArgs"/> containing the event data.</param>
	[ComVisible(false)]
	public delegate void TokenHardExpirationWarningEventHandler(object sender, TokenHardExpirationEventArgs e);
	#endregion

}
