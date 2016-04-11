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
	public class ValidateChallengeInputCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public ValidateChallengeInputCall()
		{
			ApiRequest = new ValidateChallengeInputRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public ValidateChallengeInputCall(ApiContext ApiContext)
		{
			ApiRequest = new ValidateChallengeInputRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Validates the user response to a <b class="con">GetChallengeToken</b>
		/// botblock challenge.
		/// </summary>
		/// 
		/// <param name="ChallengeToken">
		/// Botblock token that was returned by GetChallengeToken.
		/// </param>
		///
		/// <param name="UserInput">
		/// User response to a botblock challenge.
		/// </param>
		///
		/// <param name="KeepTokenValid">
		/// Whether the challenge token should remain valid for up to two minutes.
		/// </param>
		///
		public bool ValidateChallengeInput(string ChallengeToken, string UserInput, bool KeepTokenValid)
		{
			this.ChallengeToken = ChallengeToken;
			this.UserInput = UserInput;
			this.KeepTokenValid = KeepTokenValid;

			Execute();
			return ApiResponse.ValidToken;
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
		/// Gets or sets the <see cref="ValidateChallengeInputRequestType"/> for this API call.
		/// </summary>
		public ValidateChallengeInputRequestType ApiRequest
		{ 
			get { return (ValidateChallengeInputRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="ValidateChallengeInputResponseType"/> for this API call.
		/// </summary>
		public ValidateChallengeInputResponseType ApiResponse
		{ 
			get { return (ValidateChallengeInputResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="ValidateChallengeInputRequestType.ChallengeToken"/> of type <see cref="string"/>.
		/// </summary>
		public string ChallengeToken
		{ 
			get { return ApiRequest.ChallengeToken; }
			set { ApiRequest.ChallengeToken = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ValidateChallengeInputRequestType.UserInput"/> of type <see cref="string"/>.
		/// </summary>
		public string UserInput
		{ 
			get { return ApiRequest.UserInput; }
			set { ApiRequest.UserInput = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ValidateChallengeInputRequestType.KeepTokenValid"/> of type <see cref="bool"/>.
		/// </summary>
		public bool KeepTokenValid
		{ 
			get { return ApiRequest.KeepTokenValid; }
			set { ApiRequest.KeepTokenValid = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="ValidateChallengeInputResponseType.ValidToken"/> of type <see cref="bool"/>.
		/// </summary>
		public bool ValidToken
		{ 
			get { return ApiResponse.ValidToken; }
		}
		

		#endregion

		
	}
}
