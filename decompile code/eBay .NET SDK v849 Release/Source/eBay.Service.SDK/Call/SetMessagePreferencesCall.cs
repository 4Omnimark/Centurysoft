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
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using eBay.Service.EPS;
using eBay.Service.Util;

#endregion

namespace eBay.Service.Call
{

	/// <summary>
	/// 
	/// </summary>
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public class SetMessagePreferencesCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public SetMessagePreferencesCall()
		{
			ApiRequest = new SetMessagePreferencesRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public SetMessagePreferencesCall(ApiContext ApiContext)
		{
			ApiRequest = new SetMessagePreferencesRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Enables a seller to add custom Ask Seller a Question (ASQ) subjects to their
		/// Ask a Question page, or to reset any custom subjects to their default values.
		/// </summary>
		/// 
		/// <param name="ASQPreferences">
		/// Contains custom ASQ subjects or a flag to reset those
		/// subjects to their default values.
		/// </param>
		///
		public void SetMessagePreferences(ASQPreferencesType ASQPreferences)
		{
			this.ASQPreferences = ASQPreferences;

			Execute();
			
		}



		#endregion




		#region Properties
		/// <summary>
		/// The base interface object.
		/// </summary>
		/// <remarks>This property is reserved for users who have difficulty querying multiple interfaces.</remarks>
		public ApiCall ApiCallBase
		{
			get { return this; }
		}

		/// <summary>
		/// Gets or sets the <see cref="SetMessagePreferencesRequestType"/> for this API call.
		/// </summary>
		public SetMessagePreferencesRequestType ApiRequest
		{ 
			get { return (SetMessagePreferencesRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="SetMessagePreferencesResponseType"/> for this API call.
		/// </summary>
		public SetMessagePreferencesResponseType ApiResponse
		{ 
			get { return (SetMessagePreferencesResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="SetMessagePreferencesRequestType.ASQPreferences"/> of type <see cref="ASQPreferencesType"/>.
		/// </summary>
		public ASQPreferencesType ASQPreferences
		{ 
			get { return ApiRequest.ASQPreferences; }
			set { ApiRequest.ASQPreferences = value; }
		}
		
		

		#endregion

		
	}
}
