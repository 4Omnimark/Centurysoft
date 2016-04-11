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
	public class GetItemsAwaitingFeedbackCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetItemsAwaitingFeedbackCall()
		{
			ApiRequest = new GetItemsAwaitingFeedbackRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetItemsAwaitingFeedbackCall(ApiContext ApiContext)
		{
			ApiRequest = new GetItemsAwaitingFeedbackRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Returns orders in which the user was involved and for which feedback
		/// is still needed from either the buyer or seller.
		/// </summary>
		/// 
		/// <param name="Sort">
		/// Specifies how the returned feedback items should be sorted.
		/// Valid values are Title, EndTime, QuestionCount, FeedbackLeft,
		/// FeedbackReceivedDescending, UserIDDescending, TitleDescending,
		/// and EndTimeDescending.
		/// </param>
		///
		/// <param name="Pagination">
		/// Specifies the number of entries per page and the page number to return
		/// in the result set.
		/// </param>
		///
		public PaginatedTransactionArrayType GetItemsAwaitingFeedback(ItemSortTypeCodeType Sort, PaginationType Pagination)
		{
			this.Sort = Sort;
			this.Pagination = Pagination;

			Execute();
			return ApiResponse.ItemsAwaitingFeedback;
		}


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public PaginatedTransactionArrayType GetItemsAwaitingFeedback(PaginationType Pagination)
		{
			this.Pagination = Pagination;
			Execute();
			return ItemsAwaitingFeedback;
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
		/// Gets or sets the <see cref="GetItemsAwaitingFeedbackRequestType"/> for this API call.
		/// </summary>
		public GetItemsAwaitingFeedbackRequestType ApiRequest
		{ 
			get { return (GetItemsAwaitingFeedbackRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetItemsAwaitingFeedbackResponseType"/> for this API call.
		/// </summary>
		public GetItemsAwaitingFeedbackResponseType ApiResponse
		{ 
			get { return (GetItemsAwaitingFeedbackResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetItemsAwaitingFeedbackRequestType.Sort"/> of type <see cref="ItemSortTypeCodeType"/>.
		/// </summary>
		public ItemSortTypeCodeType Sort
		{ 
			get { return ApiRequest.Sort; }
			set { ApiRequest.Sort = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetItemsAwaitingFeedbackRequestType.Pagination"/> of type <see cref="PaginationType"/>.
		/// </summary>
		public PaginationType Pagination
		{ 
			get { return ApiRequest.Pagination; }
			set { ApiRequest.Pagination = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetItemsAwaitingFeedbackResponseType.ItemsAwaitingFeedback"/> of type <see cref="PaginatedTransactionArrayType"/>.
		/// </summary>
		public PaginatedTransactionArrayType ItemsAwaitingFeedback
		{ 
			get { return ApiResponse.ItemsAwaitingFeedback; }
		}
		

		#endregion

		
	}
}
