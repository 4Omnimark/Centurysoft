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
	public class GetDisputeCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetDisputeCall()
		{
			ApiRequest = new GetDisputeRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetDisputeCall(ApiContext ApiContext)
		{
			ApiRequest = new GetDisputeRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Retrieves the details of a specific eBay dispute corresponding to the supplied dispute ID.
		/// </summary>
		/// 
		/// <param name="DisputeID">
		/// The unique identifier of an eBay dispute. The caller passes in this value to
		/// retrieve detailed information on a specific dispute.
		/// </param>
		///
		public DisputeType GetDispute(string DisputeID)
		{
			this.DisputeID = DisputeID;

			Execute();
			return ApiResponse.Dispute;
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
		/// Gets or sets the <see cref="GetDisputeRequestType"/> for this API call.
		/// </summary>
		public GetDisputeRequestType ApiRequest
		{ 
			get { return (GetDisputeRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetDisputeResponseType"/> for this API call.
		/// </summary>
		public GetDisputeResponseType ApiResponse
		{ 
			get { return (GetDisputeResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetDisputeRequestType.DisputeID"/> of type <see cref="string"/>.
		/// </summary>
		public string DisputeID
		{ 
			get { return ApiRequest.DisputeID; }
			set { ApiRequest.DisputeID = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetDisputeResponseType.Dispute"/> of type <see cref="DisputeType"/>.
		/// </summary>
		public DisputeType Dispute
		{ 
			get { return ApiResponse.Dispute; }
		}
		

		#endregion

		
	}
}
