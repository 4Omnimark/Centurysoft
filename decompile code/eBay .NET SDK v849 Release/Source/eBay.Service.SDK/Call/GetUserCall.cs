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
	public class GetUserCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetUserCall()
		{
			ApiRequest = new GetUserRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetUserCall(ApiContext ApiContext)
		{
			ApiRequest = new GetUserRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Retrieves data pertaining to a single eBay user. Callers can use this call to
		/// return their own user data or the data of another eBay user. Unless the caller
		/// passes in an ItemID that identifies a current or past common order, not all
		/// data (like email addresses) will be returned in the User object.
		/// </summary>
		/// 
		/// <param name="ItemID">
		/// Specify the item ID for a successfully concluded listing in which the
		/// requestor and target user were participants (one as seller and the other
		/// as buyer). Necessary to return certain data (like an email address). Not
		/// necessary if the requestor is retrieving their own data. ItemID is an
		/// optional input.
		/// </param>
		///
		/// <param name="UserID">
		/// Specify the user whose data you want returned by the call. UserID is
		/// optional. If not specified, eBay returns data pertaining to the
		/// requesting user (as specified with the eBayAuthToken).
		/// </param>
		///
		/// <param name="IncludeExpressRequirements">
		/// This field is deprecated.
		/// </param>
		///
		/// <param name="IncludeFeatureEligibility">
		/// If IncludeFeatureEligibility is true, the response includes a
		/// FeatureEligibility node. (The FeatureEligibility node indicates whether
		/// the seller can list with certain features.)
		/// </param>
		///
		public UserType GetUser(string ItemID, string UserID, bool IncludeExpressRequirements, bool IncludeFeatureEligibility)
		{
			this.ItemID = ItemID;
			this.UserID = UserID;
			this.IncludeExpressRequirements = IncludeExpressRequirements;
			this.IncludeFeatureEligibility = IncludeFeatureEligibility;

			Execute();
			return ApiResponse.User;
		}


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public UserType GetUser(string UserID)
		{
			this.UserID = UserID;
			Execute();
			return User;
		}
		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public UserType GetUser()
		{
			Execute();
			return User;
		}
		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public UserType GetUser(string UserID, string ItemID)
		{
			this.ItemID = ItemID;
			this.UserID = UserID;

			Execute();
			return ApiResponse.User;
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
		/// Gets or sets the <see cref="GetUserRequestType"/> for this API call.
		/// </summary>
		public GetUserRequestType ApiRequest
		{ 
			get { return (GetUserRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetUserResponseType"/> for this API call.
		/// </summary>
		public GetUserResponseType ApiResponse
		{ 
			get { return (GetUserResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetUserRequestType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemID
		{ 
			get { return ApiRequest.ItemID; }
			set { ApiRequest.ItemID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetUserRequestType.UserID"/> of type <see cref="string"/>.
		/// </summary>
		public string UserID
		{ 
			get { return ApiRequest.UserID; }
			set { ApiRequest.UserID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetUserRequestType.IncludeExpressRequirements"/> of type <see cref="bool"/>.
		/// </summary>
		public bool IncludeExpressRequirements
		{ 
			get { return ApiRequest.IncludeExpressRequirements; }
			set { ApiRequest.IncludeExpressRequirements = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetUserRequestType.IncludeFeatureEligibility"/> of type <see cref="bool"/>.
		/// </summary>
		public bool IncludeFeatureEligibility
		{ 
			get { return ApiRequest.IncludeFeatureEligibility; }
			set { ApiRequest.IncludeFeatureEligibility = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetUserResponseType.User"/> of type <see cref="UserType"/>.
		/// </summary>
		public UserType User
		{ 
			get { return ApiResponse.User; }
		}
		

		#endregion

		
	}
}
