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
	public class GetMessagePreferencesCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetMessagePreferencesCall()
		{
			ApiRequest = new GetMessagePreferencesRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetMessagePreferencesCall(ApiContext ApiContext)
		{
			ApiRequest = new GetMessagePreferencesRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Returns a seller's Ask Seller a Question (ASQ) subjects, each in
		/// its own Subject node.
		/// </summary>
		/// 
		/// <param name="SellerID">
		/// The ID of the user to retrieve ASQ subjects for. This
		/// value must be specified in the request, but does not
		/// need to be the same user as the user making the
		/// request.
		/// </param>
		///
		/// <param name="IncludeASQPreferences">
		/// If true, indicates that the ASQ subjects for the
		/// specified user should be returned.
		/// </param>
		///
		public ASQPreferencesType GetMessagePreferences(string SellerID, bool IncludeASQPreferences)
		{
			this.SellerID = SellerID;
			this.IncludeASQPreferences = IncludeASQPreferences;

			Execute();
			return ApiResponse.ASQPreferences;
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
		/// Gets or sets the <see cref="GetMessagePreferencesRequestType"/> for this API call.
		/// </summary>
		public GetMessagePreferencesRequestType ApiRequest
		{ 
			get { return (GetMessagePreferencesRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetMessagePreferencesResponseType"/> for this API call.
		/// </summary>
		public GetMessagePreferencesResponseType ApiResponse
		{ 
			get { return (GetMessagePreferencesResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetMessagePreferencesRequestType.SellerID"/> of type <see cref="string"/>.
		/// </summary>
		public string SellerID
		{ 
			get { return ApiRequest.SellerID; }
			set { ApiRequest.SellerID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetMessagePreferencesRequestType.IncludeASQPreferences"/> of type <see cref="bool"/>.
		/// </summary>
		public bool IncludeASQPreferences
		{ 
			get { return ApiRequest.IncludeASQPreferences; }
			set { ApiRequest.IncludeASQPreferences = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetMessagePreferencesResponseType.ASQPreferences"/> of type <see cref="ASQPreferencesType"/>.
		/// </summary>
		public ASQPreferencesType ASQPreferences
		{ 
			get { return ApiResponse.ASQPreferences; }
		}
		

		#endregion

		
	}
}
