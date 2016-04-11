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
	public class GetUserContactDetailsCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetUserContactDetailsCall()
		{
			ApiRequest = new GetUserContactDetailsRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetUserContactDetailsCall(ApiContext ApiContext)
		{
			ApiRequest = new GetUserContactDetailsRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Returns contact information for a specified user, given that a bidding relationship
		/// (as either a buyer or seller) exists between the caller and the user.
		/// </summary>
		/// 
		/// <param name="ItemID">
		/// An eBay item ID that uniquely identifies a currently active item.
		/// </param>
		///
		/// <param name="ContactID">
		/// An eBay ID that uniquely identifies a given user for whom the caller is seeking
		/// information. This is the user's eBay username. Either a seller's or bidder's
		/// username can be specified here, as long as a bidding relationship exists between
		/// the requester and the user specified by this field. That is, a bidder must be
		/// bidding on the seller's active item, or have made an offer on the item via Best
		/// Offer.
		/// </param>
		///
		/// <param name="RequesterID">
		/// An eBay ID that uniquely identifies the person who is making the call. This is the
		/// requester's eBay username. Either a seller's or bidder's username can be specified
		/// here, as long as a bidding relationship exists between the requester and the
		/// user for whom information is being requested.
		/// </param>
		///
		public string GetUserContactDetails(string ItemID, string ContactID, string RequesterID)
		{
			this.ItemID = ItemID;
			this.ContactID = ContactID;
			this.RequesterID = RequesterID;

			Execute();
			return ApiResponse.UserID;
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
		/// Gets or sets the <see cref="GetUserContactDetailsRequestType"/> for this API call.
		/// </summary>
		public GetUserContactDetailsRequestType ApiRequest
		{ 
			get { return (GetUserContactDetailsRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetUserContactDetailsResponseType"/> for this API call.
		/// </summary>
		public GetUserContactDetailsResponseType ApiResponse
		{ 
			get { return (GetUserContactDetailsResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetUserContactDetailsRequestType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemID
		{ 
			get { return ApiRequest.ItemID; }
			set { ApiRequest.ItemID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetUserContactDetailsRequestType.ContactID"/> of type <see cref="string"/>.
		/// </summary>
		public string ContactID
		{ 
			get { return ApiRequest.ContactID; }
			set { ApiRequest.ContactID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetUserContactDetailsRequestType.RequesterID"/> of type <see cref="string"/>.
		/// </summary>
		public string RequesterID
		{ 
			get { return ApiRequest.RequesterID; }
			set { ApiRequest.RequesterID = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetUserContactDetailsResponseType.UserID"/> of type <see cref="string"/>.
		/// </summary>
		public string UserID
		{ 
			get { return ApiResponse.UserID; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetUserContactDetailsResponseType.ContactAddress"/> of type <see cref="AddressType"/>.
		/// </summary>
		public AddressType ContactAddress
		{ 
			get { return ApiResponse.ContactAddress; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetUserContactDetailsResponseType.RegistrationDate"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime RegistrationDate
		{ 
			get { return ApiResponse.RegistrationDate; }
		}
		

		#endregion

		
	}
}
