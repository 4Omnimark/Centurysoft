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
	public class GetMyeBayRemindersCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetMyeBayRemindersCall()
		{
			ApiRequest = new GetMyeBayRemindersRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetMyeBayRemindersCall(ApiContext ApiContext)
		{
			ApiRequest = new GetMyeBayRemindersRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Requests totals for the Buying and Selling reminders from the user's
		/// My eBay account.
		/// </summary>
		/// 
		/// <param name="BuyingReminders">
		/// Specifies the type of buying reminders for which you want information.
		/// </param>
		///
		/// <param name="SellingReminders">
		/// Specifies the type of selling reminders for which you want information.
		/// </param>
		///
		public RemindersType GetMyeBayReminders(ReminderCustomizationType BuyingReminders, ReminderCustomizationType SellingReminders)
		{
			this.BuyingReminders = BuyingReminders;
			this.SellingReminders = SellingReminders;

			Execute();
			return ApiResponse.BuyingReminders;
		}


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public void GetMyeBayReminders()
		{
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
		/// Gets or sets the <see cref="GetMyeBayRemindersRequestType"/> for this API call.
		/// </summary>
		public GetMyeBayRemindersRequestType ApiRequest
		{ 
			get { return (GetMyeBayRemindersRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetMyeBayRemindersResponseType"/> for this API call.
		/// </summary>
		public GetMyeBayRemindersResponseType ApiResponse
		{ 
			get { return (GetMyeBayRemindersResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetMyeBayRemindersRequestType.BuyingReminders"/> of type <see cref="ReminderCustomizationType"/>.
		/// </summary>
		public ReminderCustomizationType BuyingReminders
		{ 
			get { return ApiRequest.BuyingReminders; }
			set { ApiRequest.BuyingReminders = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetMyeBayRemindersRequestType.SellingReminders"/> of type <see cref="ReminderCustomizationType"/>.
		/// </summary>
		public ReminderCustomizationType SellingReminders
		{ 
			get { return ApiRequest.SellingReminders; }
			set { ApiRequest.SellingReminders = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetMyeBayRemindersResponseType.BuyingReminders"/> of type <see cref="RemindersType"/>.
		/// </summary>
		public RemindersType BuyingRemindersReturn
		{ 
			get { return ApiResponse.BuyingReminders; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetMyeBayRemindersResponseType.SellingReminders"/> of type <see cref="RemindersType"/>.
		/// </summary>
		public RemindersType SellingRemindersReturn
		{ 
			get { return ApiResponse.SellingReminders; }
		}
		

		#endregion

		
	}
}
