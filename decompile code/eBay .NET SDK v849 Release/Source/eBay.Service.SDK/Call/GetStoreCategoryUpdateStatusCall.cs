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
	public class GetStoreCategoryUpdateStatusCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetStoreCategoryUpdateStatusCall()
		{
			ApiRequest = new GetStoreCategoryUpdateStatusRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetStoreCategoryUpdateStatusCall(ApiContext ApiContext)
		{
			ApiRequest = new GetStoreCategoryUpdateStatusRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Returns the status of the processing for category-structure changes specified
		/// with a call to SetStoreCategories.
		/// </summary>
		/// 
		/// <param name="TaskID">
		/// The task ID returned by the SetStoreCategories call. If the
		/// SetStoreCategories call was processed asynchronously, the TaskID will be
		/// a positive number. If SetStoreCategories returned a TaskID with a value of
		/// 0, the change was completed at the time the call was made (and there is
		/// no need to check status).
		/// </param>
		///
		public TaskStatusCodeType GetStoreCategoryUpdateStatus(long TaskID)
		{
			this.TaskID = TaskID;

			Execute();
			return ApiResponse.Status;
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
		/// Gets or sets the <see cref="GetStoreCategoryUpdateStatusRequestType"/> for this API call.
		/// </summary>
		public GetStoreCategoryUpdateStatusRequestType ApiRequest
		{ 
			get { return (GetStoreCategoryUpdateStatusRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetStoreCategoryUpdateStatusResponseType"/> for this API call.
		/// </summary>
		public GetStoreCategoryUpdateStatusResponseType ApiResponse
		{ 
			get { return (GetStoreCategoryUpdateStatusResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetStoreCategoryUpdateStatusRequestType.TaskID"/> of type <see cref="long"/>.
		/// </summary>
		public long TaskID
		{ 
			get { return ApiRequest.TaskID; }
			set { ApiRequest.TaskID = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetStoreCategoryUpdateStatusResponseType.Status"/> of type <see cref="TaskStatusCodeType"/>.
		/// </summary>
		public TaskStatusCodeType Status
		{ 
			get { return ApiResponse.Status; }
		}
		

		#endregion

		
	}
}
