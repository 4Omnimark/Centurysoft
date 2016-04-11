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
	public class GetUserDisputesCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetUserDisputesCall()
		{
			ApiRequest = new GetUserDisputesRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetUserDisputesCall(ApiContext ApiContext)
		{
			ApiRequest = new GetUserDisputesRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Requests a list of disputes the requester is involved in as buyer or seller.
		/// eBay Buyer Protection Item Not Received and Significantly Not As Described cases
		/// are not returned with this call. To retrieve eBay Buyer Protection cases, the
		/// getUserCases call of the Resolution Case Management API must be used instead.
		/// </summary>
		/// 
		/// <param name="DisputeFilterType">
		/// An inclusive filter that isolates the returned disputes to a certain
		/// type such as Item Not Received or Unpaid Item disputes. eBay Buyer
		/// Protection cases are not retrieved with this call, even if the
		/// ItemNotReceivedDisputes filter is included in the request.
		/// </param>
		///
		/// <param name="DisputeSortType">
		/// The value and sequence to use to sort the returned disputes.
		/// </param>
		///
		/// <param name="ModTimeFrom">
		/// A filter that retrieves disputes whose DisputeModifiedTime is later
		/// than or equal to this value. Specify the time value in GMT.
		/// See the eBay Web Services documentation for information about specifying time values.
		/// For more precise control of the date range filter, it is a good practice to also
		/// specify ModTimeTo. Otherwise, the end of the date range is the present time.
		/// Filtering by date range is optional. You can use date range filters in combination
		/// with other filters like DisputeFilterType to control the amount of data returned.
		/// </param>
		///
		/// <param name="ModTimeTo">
		/// A filter that retrieves disputes whose DisputeModifiedTime is earlier
		/// than or equal to this value. Specify the time value in GMT.
		/// See the eBay Features Guide for information about specifying time values.
		/// For more precise control of the date range filter, it is a good practice to also
		/// specify ModTimeFrom. Otherwise, all available disputes modified prior to the ModTimeTo value are returned.
		/// Filtering by date range is optional. You can use date range filters in combination
		/// with other filters like DisputeFilterType to control the amount of data returned.
		/// </param>
		///
		/// <param name="Pagination">
		/// The virtual page number of the result set to display. A result set has a number of disputes
		/// divided into virtual pages, with 200 disputes per page. The response can only display one page.
		/// The first page in the result set is number 1. Required. If not specified, a warning is returned
		/// and Pagination.PageNumber is set to 1 by default.
		/// </param>
		///
		public DisputeTypeCollection GetUserDisputes(DisputeFilterTypeCodeType DisputeFilterType, DisputeSortTypeCodeType DisputeSortType, DateTime ModTimeFrom, DateTime ModTimeTo, PaginationType Pagination)
		{
			this.DisputeFilterType = DisputeFilterType;
			this.DisputeSortType = DisputeSortType;
			this.ModTimeFrom = ModTimeFrom;
			this.ModTimeTo = ModTimeTo;
			this.Pagination = Pagination;

			Execute();
			return ApiResponse.DisputeArray;
		}


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public DisputeTypeCollection GetUserDisputes(PaginationType Pagination)
		{
			this.Pagination = Pagination;
			Execute();
			return DisputeList;
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
		/// Gets or sets the <see cref="GetUserDisputesRequestType"/> for this API call.
		/// </summary>
		public GetUserDisputesRequestType ApiRequest
		{ 
			get { return (GetUserDisputesRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetUserDisputesResponseType"/> for this API call.
		/// </summary>
		public GetUserDisputesResponseType ApiResponse
		{ 
			get { return (GetUserDisputesResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetUserDisputesRequestType.DisputeFilterType"/> of type <see cref="DisputeFilterTypeCodeType"/>.
		/// </summary>
		public DisputeFilterTypeCodeType DisputeFilterType
		{ 
			get { return ApiRequest.DisputeFilterType; }
			set { ApiRequest.DisputeFilterType = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetUserDisputesRequestType.DisputeSortType"/> of type <see cref="DisputeSortTypeCodeType"/>.
		/// </summary>
		public DisputeSortTypeCodeType DisputeSortType
		{ 
			get { return ApiRequest.DisputeSortType; }
			set { ApiRequest.DisputeSortType = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetUserDisputesRequestType.ModTimeFrom"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime ModTimeFrom
		{ 
			get { return ApiRequest.ModTimeFrom; }
			set { ApiRequest.ModTimeFrom = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetUserDisputesRequestType.ModTimeTo"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime ModTimeTo
		{ 
			get { return ApiRequest.ModTimeTo; }
			set { ApiRequest.ModTimeTo = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetUserDisputesRequestType.Pagination"/> of type <see cref="PaginationType"/>.
		/// </summary>
		public PaginationType Pagination
		{ 
			get { return ApiRequest.Pagination; }
			set { ApiRequest.Pagination = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetUserDisputesResponseType.StartingDisputeID"/> of type <see cref="string"/>.
		/// </summary>
		public string StartingDisputeID
		{ 
			get { return ApiResponse.StartingDisputeID; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetUserDisputesResponseType.EndingDisputeID"/> of type <see cref="string"/>.
		/// </summary>
		public string EndingDisputeID
		{ 
			get { return ApiResponse.EndingDisputeID; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetUserDisputesResponseType.DisputeArray"/> of type <see cref="DisputeTypeCollection"/>.
		/// </summary>
		public DisputeTypeCollection DisputeList
		{ 
			get { return ApiResponse.DisputeArray; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetUserDisputesResponseType.ItemsPerPage"/> of type <see cref="int"/>.
		/// </summary>
		public int ItemsPerPage
		{ 
			get { return ApiResponse.ItemsPerPage; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetUserDisputesResponseType.PageNumber"/> of type <see cref="int"/>.
		/// </summary>
		public int PageNumber
		{ 
			get { return ApiResponse.PageNumber; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetUserDisputesResponseType.DisputeFilterCount"/> of type <see cref="DisputeFilterCountTypeCollection"/>.
		/// </summary>
		public DisputeFilterCountTypeCollection DisputeFilterList
		{ 
			get { return ApiResponse.DisputeFilterCount; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetUserDisputesResponseType.PaginationResult"/> of type <see cref="PaginationResultType"/>.
		/// </summary>
		public PaginationResultType PaginationResult
		{ 
			get { return ApiResponse.PaginationResult; }
		}
		

		#endregion

		
	}
}
