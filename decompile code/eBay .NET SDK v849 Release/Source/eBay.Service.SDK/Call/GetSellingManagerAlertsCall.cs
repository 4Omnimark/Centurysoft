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
	public class GetSellingManagerAlertsCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetSellingManagerAlertsCall()
		{
			ApiRequest = new GetSellingManagerAlertsRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetSellingManagerAlertsCall(ApiContext ApiContext)
		{
			ApiRequest = new GetSellingManagerAlertsRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Retrieves Selling Manager alerts.
		/// This call is subject to change without notice; the deprecation process is
		/// inapplicable to this call.
		/// </summary>
		/// 
		public SellingManagerAlertTypeCollection GetSellingManagerAlerts()
		{

			Execute();
			return ApiResponse.Alert;
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
		/// Gets or sets the <see cref="GetSellingManagerAlertsRequestType"/> for this API call.
		/// </summary>
		public GetSellingManagerAlertsRequestType ApiRequest
		{ 
			get { return (GetSellingManagerAlertsRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetSellingManagerAlertsResponseType"/> for this API call.
		/// </summary>
		public GetSellingManagerAlertsResponseType ApiResponse
		{ 
			get { return (GetSellingManagerAlertsResponseType) AbstractResponse; }
		}

		
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellingManagerAlertsResponseType.Alert"/> of type <see cref="SellingManagerAlertTypeCollection"/>.
		/// </summary>
		public SellingManagerAlertTypeCollection AlertList
		{ 
			get { return ApiResponse.Alert; }
		}
		

		#endregion

		
	}
}
