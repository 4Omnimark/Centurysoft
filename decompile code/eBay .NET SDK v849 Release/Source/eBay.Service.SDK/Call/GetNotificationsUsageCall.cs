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
	public class GetNotificationsUsageCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetNotificationsUsageCall()
		{
			ApiRequest = new GetNotificationsUsageRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetNotificationsUsageCall(ApiContext ApiContext)
		{
			ApiRequest = new GetNotificationsUsageRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Retrieves usage information about platform notifications for a given application.
		/// You can use this notification information to troubleshoot issues with platform
		/// notifications. You can call this up to 50 times per hour for a given application.
		/// </summary>
		/// 
		/// <param name="StartTime">
		/// Specifies the start date and time for which notification information
		/// will be retrieved. StartTime is optional. If no StartTime is specified,
		/// the default value of 24 hours prior to the call time is used. If no
		/// StartTime is specified or if an invalid StartTime is specified, date
		/// range errors are returned in the response. For a StartTime to be valid,
		/// it must be no more than 72 hours before the time of the call, it cannot
		/// be more recent than the EndTime, and it cannot be later than the time of
		/// the call. If an invalid StartTime is specified, the default value is
		/// used.
		/// </param>
		///
		/// <param name="EndTime">
		/// Specifies the end date and time for which notification information
		/// will be retrieved. EndTime is optional. If no EndTime is specified,
		/// the current time (the time the call is made) is used. If no EndTime
		/// is specified or if an invalid EndTime is specified, date range errors
		/// are returned in the response. For an EndTime to be valid, it must be no
		/// more than 72 hours before the time the of the call, it cannot be before
		/// the StartTime, and it cannot be later than the time of the call. If an
		/// invalid EndTime is specified, the current time is used.
		/// </param>
		///
		/// <param name="ItemID">
		/// Specifies an item ID for which detailed notification information will be
		/// retrieved. ItemID is optional. If no ItemID is specified, the response
		/// will not include any individual notification details.
		/// </param>
		///
		public void GetNotificationsUsage(DateTime StartTime, DateTime EndTime, string ItemID)
		{
			this.StartTime = StartTime;
			this.EndTime = EndTime;
			this.ItemID = ItemID;

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
		/// Gets or sets the <see cref="GetNotificationsUsageRequestType"/> for this API call.
		/// </summary>
		public GetNotificationsUsageRequestType ApiRequest
		{ 
			get { return (GetNotificationsUsageRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetNotificationsUsageResponseType"/> for this API call.
		/// </summary>
		public GetNotificationsUsageResponseType ApiResponse
		{ 
			get { return (GetNotificationsUsageResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetNotificationsUsageRequestType.StartTime"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime StartTime
		{ 
			get { return ApiRequest.StartTime; }
			set { ApiRequest.StartTime = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetNotificationsUsageRequestType.EndTime"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime EndTime
		{ 
			get { return ApiRequest.EndTime; }
			set { ApiRequest.EndTime = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetNotificationsUsageRequestType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemID
		{ 
			get { return ApiRequest.ItemID; }
			set { ApiRequest.ItemID = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetNotificationsUsageResponseType.StartTime"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime StartTimeReturn
		{ 
			get { return ApiResponse.StartTime; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetNotificationsUsageResponseType.EndTime"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime EndTimeReturn
		{ 
			get { return ApiResponse.EndTime; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetNotificationsUsageResponseType.NotificationDetailsArray"/> of type <see cref="NotificationDetailsTypeCollection"/>.
		/// </summary>
		public NotificationDetailsTypeCollection NotificationDetailsList
		{ 
			get { return ApiResponse.NotificationDetailsArray; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetNotificationsUsageResponseType.MarkUpMarkDownHistory"/> of type <see cref="MarkUpMarkDownEventTypeCollection"/>.
		/// </summary>
		public MarkUpMarkDownEventTypeCollection MarkUpMarkDownHistoryList
		{ 
			get { return ApiResponse.MarkUpMarkDownHistory; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetNotificationsUsageResponseType.NotificationStatistics"/> of type <see cref="NotificationStatisticsType"/>.
		/// </summary>
		public NotificationStatisticsType NotificationStatistics
		{ 
			get { return ApiResponse.NotificationStatistics; }
		}
		

		#endregion

		
	}
}
