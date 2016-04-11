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
	public class AddTransactionConfirmationItemCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public AddTransactionConfirmationItemCall()
		{
			ApiRequest = new AddTransactionConfirmationItemRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public AddTransactionConfirmationItemCall(ApiContext ApiContext)
		{
			ApiRequest = new AddTransactionConfirmationItemRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Ends the eBay Motors listing specified by ItemID and creates a new Transaction
		/// Confirmation Request (TCR) for the item, thus enabling the TCR recipient to
		/// purchase the item. You can also use this call to see if a new TCR can be created
		/// for the specified item.
		/// </summary>
		/// 
		/// <param name="RecipientUserID">
		/// Specifies the user to whom the seller is offering the Transaction
		/// Confirmation Request.
		/// </param>
		///
		/// <param name="VerifyEligibilityOnly">
		/// If true, specifies that the seller is verifying whether a new Transaction
		/// Confirmation Request (TCR) can be created. Thus, if this value is passed
		/// as true, then no Transaction Confirmation Request is actually made. If
		/// VerifyEligibilityOnly is not passed, or is false, a Transaction
		/// Confirmation Request is actually made.
		/// </param>
		///
		/// <param name="RecipientPostalCode">
		/// Specifies the postal code of the user to whom the seller is offering the
		/// Transaction Confirmation Request. Required only if the user does not meet
		/// the other options listed in RecipientRelationCodeType. An error is
		/// returned if RecipientUserID and RecipientPostalCode do not match for more
		/// than 3 times for a seller per day.
		/// </param>
		///
		/// <param name="RecipientRelationType">
		/// Specifies the current relationship between the seller and the potential
		/// buyer. A seller can make a Transaction Confirmation Request (TCR) for an
		/// item to a potential buyer if the buyer meets one of several criteria. A
		/// TCR is sent by a seller to one of the following: a bidder, a best offer
		/// buyer, a member with an ASQ question, or any member with a postal code.
		/// See the values and annotations in RecipientRelationCodeType.
		/// </param>
		///
		/// <param name="NegotiatedPrice">
		/// The amount the offer recipient must pay to buy the item
		/// specified in the Transaction Confirmation Request (TCR).
		/// A negotiated amount between the buyer and the seller.
		/// </param>
		///
		/// <param name="ListingDuration">
		/// Specifies the length of time the item in the Transaction Confirmation
		/// Request (TCR) will be available for purchase.
		/// </param>
		///
		/// <param name="ItemID">
		/// The ItemID of the item that the seller wants to end in order to create a
		/// Transaction Confirmation Request (TCR).
		/// </param>
		///
		/// <param name="Comments">
		/// Comments the seller wants to send to the recipient (bidder, best offer
		/// buyer, member with an ASQ question, or member with a postal code).
		/// </param>
		///
		public string AddTransactionConfirmationItem(string RecipientUserID, string VerifyEligibilityOnly, string RecipientPostalCode, RecipientRelationCodeType RecipientRelationType, AmountType NegotiatedPrice, SecondChanceOfferDurationCodeType ListingDuration, string ItemID, string Comments)
		{
			this.RecipientUserID = RecipientUserID;
			this.VerifyEligibilityOnly = VerifyEligibilityOnly;
			this.RecipientPostalCode = RecipientPostalCode;
			this.RecipientRelationType = RecipientRelationType;
			this.NegotiatedPrice = NegotiatedPrice;
			this.ListingDuration = ListingDuration;
			this.ItemID = ItemID;
			this.Comments = Comments;

			Execute();
			return ApiResponse.ItemID;
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
		/// Gets or sets the <see cref="AddTransactionConfirmationItemRequestType"/> for this API call.
		/// </summary>
		public AddTransactionConfirmationItemRequestType ApiRequest
		{ 
			get { return (AddTransactionConfirmationItemRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="AddTransactionConfirmationItemResponseType"/> for this API call.
		/// </summary>
		public AddTransactionConfirmationItemResponseType ApiResponse
		{ 
			get { return (AddTransactionConfirmationItemResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="AddTransactionConfirmationItemRequestType.RecipientUserID"/> of type <see cref="string"/>.
		/// </summary>
		public string RecipientUserID
		{ 
			get { return ApiRequest.RecipientUserID; }
			set { ApiRequest.RecipientUserID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="AddTransactionConfirmationItemRequestType.VerifyEligibilityOnly"/> of type <see cref="string"/>.
		/// </summary>
		public string VerifyEligibilityOnly
		{ 
			get { return ApiRequest.VerifyEligibilityOnly; }
			set { ApiRequest.VerifyEligibilityOnly = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="AddTransactionConfirmationItemRequestType.RecipientPostalCode"/> of type <see cref="string"/>.
		/// </summary>
		public string RecipientPostalCode
		{ 
			get { return ApiRequest.RecipientPostalCode; }
			set { ApiRequest.RecipientPostalCode = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="AddTransactionConfirmationItemRequestType.RecipientRelationType"/> of type <see cref="RecipientRelationCodeType"/>.
		/// </summary>
		public RecipientRelationCodeType RecipientRelationType
		{ 
			get { return ApiRequest.RecipientRelationType; }
			set { ApiRequest.RecipientRelationType = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="AddTransactionConfirmationItemRequestType.NegotiatedPrice"/> of type <see cref="AmountType"/>.
		/// </summary>
		public AmountType NegotiatedPrice
		{ 
			get { return ApiRequest.NegotiatedPrice; }
			set { ApiRequest.NegotiatedPrice = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="AddTransactionConfirmationItemRequestType.ListingDuration"/> of type <see cref="SecondChanceOfferDurationCodeType"/>.
		/// </summary>
		public SecondChanceOfferDurationCodeType ListingDuration
		{ 
			get { return ApiRequest.ListingDuration; }
			set { ApiRequest.ListingDuration = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="AddTransactionConfirmationItemRequestType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemID
		{ 
			get { return ApiRequest.ItemID; }
			set { ApiRequest.ItemID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="AddTransactionConfirmationItemRequestType.Comments"/> of type <see cref="string"/>.
		/// </summary>
		public string Comments
		{ 
			get { return ApiRequest.Comments; }
			set { ApiRequest.Comments = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="AddTransactionConfirmationItemResponseType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemIDResult
		{ 
			get { return ApiResponse.ItemID; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="AddTransactionConfirmationItemResponseType.StartTime"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime StartTime
		{ 
			get { return ApiResponse.StartTime; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="AddTransactionConfirmationItemResponseType.EndTime"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime EndTime
		{ 
			get { return ApiResponse.EndTime; }
		}
		

		#endregion

		
	}
}
