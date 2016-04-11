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
	public class SellerReverseDisputeCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public SellerReverseDisputeCall()
		{
			ApiRequest = new SellerReverseDisputeRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public SellerReverseDisputeCall(ApiContext ApiContext)
		{
			ApiRequest = new SellerReverseDisputeRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Enables a seller to "reverse" an Unpaid Item dispute that has been closed, for
		/// example, if buyer and seller reach an agreement. The seller's Final Value Fee
		/// credit and the buyer's strike are both reversed, if applicable.
		/// The dispute might have resulted
		/// in a strike to the buyer and a Final Value Fee credit to the seller. A buyer and
		/// seller sometimes come to agreement after a dispute has been closed. In particular,
		/// the seller might discover that the buyer actually paid, or the buyer might agree
		/// to pay the seller's fees in exchange for having the strike removed.
		/// 
		/// A dispute can only be reversed if it was closed with DisputeActivity set to
		/// SellerEndCommunication, CameToAgreementNeedFVFCredit, or
		/// MutualAgreementOrNoBuyerResponse.
		/// </summary>
		/// 
		/// <param name="DisputeID">
		/// The unique identifier of the dispute that was returned when the dispute was created.
		/// The dispute must be an Unpaid Item dispute that the seller opened.
		/// </param>
		///
		/// <param name="DisputeResolutionReason">
		/// The reason the dispute is being reversed.
		/// </param>
		///
		public void SellerReverseDispute(string DisputeID, DisputeResolutionReasonCodeType DisputeResolutionReason)
		{
			this.DisputeID = DisputeID;
			this.DisputeResolutionReason = DisputeResolutionReason;

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
		/// Gets or sets the <see cref="SellerReverseDisputeRequestType"/> for this API call.
		/// </summary>
		public SellerReverseDisputeRequestType ApiRequest
		{ 
			get { return (SellerReverseDisputeRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="SellerReverseDisputeResponseType"/> for this API call.
		/// </summary>
		public SellerReverseDisputeResponseType ApiResponse
		{ 
			get { return (SellerReverseDisputeResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="SellerReverseDisputeRequestType.DisputeID"/> of type <see cref="string"/>.
		/// </summary>
		public string DisputeID
		{ 
			get { return ApiRequest.DisputeID; }
			set { ApiRequest.DisputeID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SellerReverseDisputeRequestType.DisputeResolutionReason"/> of type <see cref="DisputeResolutionReasonCodeType"/>.
		/// </summary>
		public DisputeResolutionReasonCodeType DisputeResolutionReason
		{ 
			get { return ApiRequest.DisputeResolutionReason; }
			set { ApiRequest.DisputeResolutionReason = value; }
		}
		
		

		#endregion

		
	}
}
