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
	public class AddMemberMessageAAQToPartnerCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public AddMemberMessageAAQToPartnerCall()
		{
			ApiRequest = new AddMemberMessageAAQToPartnerRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public AddMemberMessageAAQToPartnerCall(ApiContext ApiContext)
		{
			ApiRequest = new AddMemberMessageAAQToPartnerRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Enables a buyer and seller in an order relationship to
		/// send messages to each other's My Messages Inboxes.
		/// </summary>
		/// 
		/// <param name="ItemID">
		/// The unique ID of the item about which the question was asked.
		/// </param>
		///
		/// <param name="MemberMessage">
		/// Container for the message. Includes the message body, plus other values
		/// related to the message.
		/// </param>
		///
		public void AddMemberMessageAAQToPartner(string ItemID, MemberMessageType MemberMessage)
		{
			this.ItemID = ItemID;
			this.MemberMessage = MemberMessage;

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
		/// Gets or sets the <see cref="AddMemberMessageAAQToPartnerRequestType"/> for this API call.
		/// </summary>
		public AddMemberMessageAAQToPartnerRequestType ApiRequest
		{ 
			get { return (AddMemberMessageAAQToPartnerRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="AddMemberMessageAAQToPartnerResponseType"/> for this API call.
		/// </summary>
		public AddMemberMessageAAQToPartnerResponseType ApiResponse
		{ 
			get { return (AddMemberMessageAAQToPartnerResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="AddMemberMessageAAQToPartnerRequestType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemID
		{ 
			get { return ApiRequest.ItemID; }
			set { ApiRequest.ItemID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="AddMemberMessageAAQToPartnerRequestType.MemberMessage"/> of type <see cref="MemberMessageType"/>.
		/// </summary>
		public MemberMessageType MemberMessage
		{ 
			get { return ApiRequest.MemberMessage; }
			set { ApiRequest.MemberMessage = value; }
		}
		
		

		#endregion

		
	}
}
