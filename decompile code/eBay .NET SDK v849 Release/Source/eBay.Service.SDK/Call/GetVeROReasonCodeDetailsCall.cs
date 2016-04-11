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
	public class GetVeROReasonCodeDetailsCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetVeROReasonCodeDetailsCall()
		{
			ApiRequest = new GetVeROReasonCodeDetailsRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetVeROReasonCodeDetailsCall(ApiContext ApiContext)
		{
			ApiRequest = new GetVeROReasonCodeDetailsRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Retrieves details for VeRO reason codes and their descriptions. You can specify a
		/// reason code ID to get details for a specific reason on the site specified in the
		/// request header. If ReasonCodeID is not passed in the request, all reason codes are
		/// returned. Set ReturnAllSites to true to retrieve reason codes for all sites.
		/// You must be a member of the Verified Rights Owner (VeRO) Program to use this call.
		/// </summary>
		/// 
		/// <param name="ReasonCodeID">
		/// Unique identifier for a reason code. If this ReasonCodeID is passed then 
		/// only details of this ReasonCodeID will be returned. If no reason code is 
		/// specified, all reason codes are returned.
		/// </param>
		///
		/// <param name="ReturnAllSites">
		/// Set to true to retrieve reason codes for all sites. If not specified, 
		/// reason codes are returned for the site specified in the request header 
		/// only. 
		/// If ReasonCodeID is specified, this parameter is ignored.
		/// </param>
		///
		public VeROSiteDetailTypeCollection GetVeROReasonCodeDetails(long ReasonCodeID, bool ReturnAllSites)
		{
			this.ReasonCodeID = ReasonCodeID;
			this.ReturnAllSites = ReturnAllSites;

			Execute();
			return ApiResponse.VeROReasonCodeDetails;
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
		/// Gets or sets the <see cref="GetVeROReasonCodeDetailsRequestType"/> for this API call.
		/// </summary>
		public GetVeROReasonCodeDetailsRequestType ApiRequest
		{ 
			get { return (GetVeROReasonCodeDetailsRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetVeROReasonCodeDetailsResponseType"/> for this API call.
		/// </summary>
		public GetVeROReasonCodeDetailsResponseType ApiResponse
		{ 
			get { return (GetVeROReasonCodeDetailsResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetVeROReasonCodeDetailsRequestType.ReasonCodeID"/> of type <see cref="long"/>.
		/// </summary>
		public long ReasonCodeID
		{ 
			get { return ApiRequest.ReasonCodeID; }
			set { ApiRequest.ReasonCodeID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetVeROReasonCodeDetailsRequestType.ReturnAllSites"/> of type <see cref="bool"/>.
		/// </summary>
		public bool ReturnAllSites
		{ 
			get { return ApiRequest.ReturnAllSites; }
			set { ApiRequest.ReturnAllSites = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetVeROReasonCodeDetailsResponseType.VeROReasonCodeDetails"/> of type <see cref="VeROSiteDetailTypeCollection"/>.
		/// </summary>
		public VeROSiteDetailTypeCollection VeROReasonCodeDetailList
		{ 
			get { return ApiResponse.VeROReasonCodeDetails; }
		}
		

		#endregion

		
	}
}
