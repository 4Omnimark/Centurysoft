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
	public class GeteBayOfficialTimeCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GeteBayOfficialTimeCall()
		{
			ApiRequest = new GeteBayOfficialTimeRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GeteBayOfficialTimeCall(ApiContext ApiContext)
		{
			ApiRequest = new GeteBayOfficialTimeRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Gets the official eBay system time in GMT.
		/// </summary>
		/// 
		public DateTime GeteBayOfficialTime()
		{

			Execute();
			return ApiResponse.Timestamp;
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
		/// Gets or sets the <see cref="GeteBayOfficialTimeRequestType"/> for this API call.
		/// </summary>
		public GeteBayOfficialTimeRequestType ApiRequest
		{ 
			get { return (GeteBayOfficialTimeRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GeteBayOfficialTimeResponseType"/> for this API call.
		/// </summary>
		public GeteBayOfficialTimeResponseType ApiResponse
		{ 
			get { return (GeteBayOfficialTimeResponseType) AbstractResponse; }
		}

		/// <summary>
		/// Gets the <see cref="AbstractResponseType.Timestamp"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime EBayTime
		{ 
		   	get { return ApiResponse.Timestamp; }
		}
		

		#endregion

		
	}
}
