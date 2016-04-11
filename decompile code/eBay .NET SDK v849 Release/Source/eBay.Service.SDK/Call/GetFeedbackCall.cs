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
	public class GetFeedbackCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetFeedbackCall()
		{
			ApiRequest = new GetFeedbackRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetFeedbackCall(ApiContext ApiContext)
		{
			ApiRequest = new GetFeedbackRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Retrieves the accumulated feedback left for a specified user or the summary
		/// feedback data for a specific order line item or item listing.&nbsp;<b>Also for
		/// Half.com</b>.
		/// </summary>
		/// 
		/// <param name="UserID">
		/// Specifies the user whose feedback data is to be returned. If not
		/// specified, then the feedback returned is for the requesting user.
		/// </param>
		///
		/// <param name="FeedbackID">
		/// An ID that uniquely identifies a feedback record to be retrieved.
		/// Used only by the Feedback notification.
		/// </param>
		///
		/// <param name="ItemID">
		/// Unique identifier for an eBay item listing. A listing can have multiple
		/// order line items (transactions), but only one <b>ItemID</b>. If <b>ItemID</b> is
		/// specified in the <b>GetFeedback</b> request, the returned feedback records are
		/// restricted to the specified <b>ItemID</b>. The <b>FeedbackType</b> and <b>Pagination</b>
		/// fields (if included) are ignored. The maximum number of feedback records
		/// returned is 100.
		/// </param>
		///
		/// <param name="TransactionID">
		/// Unique identifier for an eBay order line item (transaction). A
		/// <b>TransactionID</b> can be paired up with its corresponding <b>ItemID</b> and used as
		/// an input filter in the <b>GetFeedback</b> request. If an <b>ItemID</b>/<b>TransactionID</b>
		/// pair or an <b>OrderLineItemID</b> value is used to retrieve a feedback record
		/// on a specific order line item, the <b>FeedbackType</b> and <b>Pagination</b>
		/// fields (if included) are ignored.
		/// </param>
		///
		/// <param name="CommentTypeList">
		/// Returns feedback of a specified type (positive, negative, or neutral) in 
		/// <b>FeedbackDetailArray</b>. You can include two comment types in your call if you want
		/// to exclude the third type from your results. If no <b>CommentType</b> is specified,
		/// all of the feedback types are returned.
		/// </param>
		///
		/// <param name="FeedbackType">
		/// Returns feedback that you received as a buyer or seller, or feedback you left
		/// for others (as either a buyer or a seller). You can include only one
		/// <b>FeedbackType</b> in your call. If no <b>FeedbackType</b> is specified, all of the
		/// available feedback is returned.
		/// </param>
		///
		/// <param name="Pagination">
		/// Controls the pagination of the result set. Child elements, <b>EntriesPerPage</b> and
		/// <b>PageNumber</b>, specify the maximum number of individual feedback records to return
		/// per call and which page of data to return. Only applicable if <b>DetailLevel</b> is
		/// set to <b>ReturnAll</b> and the call is returning feedback for a <b>UserID</b>. Feedback
		/// summary data is not paginated, but when pagination is used, it is returned
		/// after the last feedback detail entry.
		/// 
		/// Accepted values for <b>Pagination.EntriesPerPage</b> for GetFeedback is 25 (the
		/// default), 50, 100, and 200. If you specify a value of zero, or a value
		/// greater than 200, the call fails with an error. If you specify a value between
		/// one and twenty-four, the value is rounded up to 25. Values between 26 and 199
		/// that are not one of the accepted values are rounded down to the nearest
		/// accepted value.
		/// </param>
		///
		/// <param name="OrderLineItemID">
		/// <b>OrderLineItemID</b> is a unique identifier for an eBay order line item and
		/// is based upon the concatenation of <b>ItemID</b> and <b>TransactionID</b>, with a
		/// hyphen in between these two IDs. An <b>OrderLineItemID</b> can be used as an
		/// input filter in the <b>GetFeedback</b> request. If an <b>OrderLineItemID</b> value is
		/// used to retrieve a feedback record on a specific order line item, the
		/// <b>FeedbackType</b> and <b>Pagination</b> fields (if included) are ignored.
		/// </param>
		///
		public FeedbackDetailTypeCollection GetFeedback(string UserID, string FeedbackID, string ItemID, string TransactionID, CommentTypeCodeTypeCollection CommentTypeList, FeedbackTypeCodeType FeedbackType, PaginationType Pagination, string OrderLineItemID)
		{
			this.UserID = UserID;
			this.FeedbackID = FeedbackID;
			this.ItemID = ItemID;
			this.TransactionID = TransactionID;
			this.CommentTypeList = CommentTypeList;
			this.FeedbackType = FeedbackType;
			this.Pagination = Pagination;
			this.OrderLineItemID = OrderLineItemID;

			Execute();
			return ApiResponse.FeedbackDetailArray;
		}


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public FeedbackDetailTypeCollection GetFeedback()
		{
			Execute();
			return FeedbackList;
		}
		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public FeedbackDetailTypeCollection GetFeedback(string UserID)
		{
			this.UserID = UserID;
			Execute();
			return FeedbackList;
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
		/// Gets or sets the <see cref="GetFeedbackRequestType"/> for this API call.
		/// </summary>
		public GetFeedbackRequestType ApiRequest
		{ 
			get { return (GetFeedbackRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetFeedbackResponseType"/> for this API call.
		/// </summary>
		public GetFeedbackResponseType ApiResponse
		{ 
			get { return (GetFeedbackResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetFeedbackRequestType.UserID"/> of type <see cref="string"/>.
		/// </summary>
		public string UserID
		{ 
			get { return ApiRequest.UserID; }
			set { ApiRequest.UserID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetFeedbackRequestType.FeedbackID"/> of type <see cref="string"/>.
		/// </summary>
		public string FeedbackID
		{ 
			get { return ApiRequest.FeedbackID; }
			set { ApiRequest.FeedbackID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetFeedbackRequestType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemID
		{ 
			get { return ApiRequest.ItemID; }
			set { ApiRequest.ItemID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetFeedbackRequestType.TransactionID"/> of type <see cref="string"/>.
		/// </summary>
		public string TransactionID
		{ 
			get { return ApiRequest.TransactionID; }
			set { ApiRequest.TransactionID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetFeedbackRequestType.CommentType"/> of type <see cref="CommentTypeCodeTypeCollection"/>.
		/// </summary>
		public CommentTypeCodeTypeCollection CommentTypeList
		{ 
			get { return ApiRequest.CommentType; }
			set { ApiRequest.CommentType = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetFeedbackRequestType.FeedbackType"/> of type <see cref="FeedbackTypeCodeType"/>.
		/// </summary>
		public FeedbackTypeCodeType FeedbackType
		{ 
			get { return ApiRequest.FeedbackType; }
			set { ApiRequest.FeedbackType = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetFeedbackRequestType.Pagination"/> of type <see cref="PaginationType"/>.
		/// </summary>
		public PaginationType Pagination
		{ 
			get { return ApiRequest.Pagination; }
			set { ApiRequest.Pagination = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetFeedbackRequestType.OrderLineItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string OrderLineItemID
		{ 
			get { return ApiRequest.OrderLineItemID; }
			set { ApiRequest.OrderLineItemID = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetFeedbackResponseType.FeedbackDetailArray"/> of type <see cref="FeedbackDetailTypeCollection"/>.
		/// </summary>
		public FeedbackDetailTypeCollection FeedbackList
		{ 
			get { return ApiResponse.FeedbackDetailArray; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetFeedbackResponseType.FeedbackDetailItemTotal"/> of type <see cref="int"/>.
		/// </summary>
		public int FeedbackDetailItemTotal
		{ 
			get { return ApiResponse.FeedbackDetailItemTotal; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetFeedbackResponseType.FeedbackSummary"/> of type <see cref="FeedbackSummaryType"/>.
		/// </summary>
		public FeedbackSummaryType FeedbackSummary
		{ 
			get { return ApiResponse.FeedbackSummary; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetFeedbackResponseType.FeedbackScore"/> of type <see cref="int"/>.
		/// </summary>
		public int FeedbackScore
		{ 
			get { return ApiResponse.FeedbackScore; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetFeedbackResponseType.PaginationResult"/> of type <see cref="PaginationResultType"/>.
		/// </summary>
		public PaginationResultType PaginationResult
		{ 
			get { return ApiResponse.PaginationResult; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetFeedbackResponseType.EntriesPerPage"/> of type <see cref="int"/>.
		/// </summary>
		public int EntriesPerPage
		{ 
			get { return ApiResponse.EntriesPerPage; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetFeedbackResponseType.PageNumber"/> of type <see cref="int"/>.
		/// </summary>
		public int PageNumber
		{ 
			get { return ApiResponse.PageNumber; }
		}
		

		#endregion

		
	}
}
