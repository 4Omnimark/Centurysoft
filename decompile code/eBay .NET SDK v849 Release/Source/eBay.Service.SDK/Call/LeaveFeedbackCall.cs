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
	public class LeaveFeedbackCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public LeaveFeedbackCall()
		{
			ApiRequest = new LeaveFeedbackRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public LeaveFeedbackCall(ApiContext ApiContext)
		{
			ApiRequest = new LeaveFeedbackRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Enables a buyer and seller to leave feedback for their order partner at the
		/// conclusion of a successful order. Feedback is left at the order line item level,
		/// so multiple line item orders may have multiple Feedback entries.&nbsp;<b>
		/// Also for Half.com</b>.
		/// </summary>
		/// 
		/// <param name="ItemID">
		/// Unique identifier for an eBay item listing. A listing can have multiple
		/// order line items, but only one <b>ItemID</b>. Unless an
		/// <b>OrderLineItemID</b> is specified in the <b>LeaveFeedback</b> request, the <b>ItemID</b> is
		/// required along with the <b>TargetUser</b> to identify an order line item
		/// existing between the caller and the <b>TargetUser</b> that requires feedback. A
		/// Feedback entry will be posted for this order line item. If there are
		/// multiple order line items between the two order partners that still
		/// require feedback, the <b>TransactionID</b> will also be required to isolate the
		/// targeted order line item. Feedback cannot be left for order line items
		/// with creation dates more than 60 days in the past.
		/// </param>
		///
		/// <param name="CommentText">
		/// Textual comment that explains, clarifies, or justifies the Feedback rating
		/// specified in <b>CommentType</b>. 
		///  
		/// This comment will still be displayed even if submitted Feedback is withdrawn.
		/// </param>
		///
		/// <param name="CommentType">
		/// This value indicates the Feedback rating for the user specified in the 
		/// <b>TargetUser</b> field. This field is required in <b>LeaveFeedback</b>. 
		/// 
		/// A Positive rating increases the user's Feedback score, a Negative rating decreases the user's Feedback score, and a Neutral rating does not affect the user's Feedback score. 
		/// 
		/// Sellers cannot leave Neutral or Negative ratings for buyers. 
		/// </param>
		///
		/// <param name="TransactionID">
		/// Unique identifier for an eBay order line item (transaction). If there
		/// are multiple order line items between the two order partners that still
		/// require feedback, the <b>TransactionID</b> is required along with the
		/// corresponding <b>ItemID</b> and <b>TargetUser</b> to isolate the targeted order line
		/// item. If an <b>OrderLineItemID</b> is included in the response to identify a
		/// specific order line item, none of the preceding fields (<b>ItemID</b>,
		/// <b>TransactionID</b>, <b>TargetUser</b>) are needed. Feedback cannot be left for order
		/// line items with creation dates more than 60 days in the past.
		/// </param>
		///
		/// <param name="TargetUser">
		/// This eBay User ID identifies the recipient user for whom the feedback is being
		/// left.
		/// </param>
		///
		/// <param name="SellerItemRatingDetailArrayList">
		/// Container for detailed seller ratings (DSRs). If a buyer is providing DSRs,
		/// they are specified in this container. Sellers have access to the number of
		/// ratings they've received, as well as to the averages of the DSRs they've
		/// received in each DSR area (i.e., to the average of ratings in the
		/// item-description area, etc.).
		/// </param>
		///
		/// <param name="OrderLineItemID">
		/// <b>OrderLineItemID</b> is a unique identifier for an eBay order line item and
		/// is based upon the concatenation of <b>ItemID</b> and <b>TransactionID</b>, with a
		/// hyphen in between these two IDs. If an <b>OrderLineItemID</b> is included in
		/// the request, the <b>ItemID</b>, <b>TransactionID</b>, and <b>TargetUser</b> fields are not
		/// required. Feedback cannot be left for order line items with creation
		/// dates more than 60 days in the past. 
		/// </param>
		///
		public string LeaveFeedback(string ItemID, string CommentText, CommentTypeCodeType CommentType, string TransactionID, string TargetUser, ItemRatingDetailsTypeCollection SellerItemRatingDetailArrayList, string OrderLineItemID)
		{
			this.ItemID = ItemID;
			this.CommentText = CommentText;
			this.CommentType = CommentType;
			this.TransactionID = TransactionID;
			this.TargetUser = TargetUser;
			this.SellerItemRatingDetailArrayList = SellerItemRatingDetailArrayList;
			this.OrderLineItemID = OrderLineItemID;

			Execute();
			return ApiResponse.FeedbackID;
		}


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public string LeaveFeedback(string TargetUser, string ItemID, string TransactionID, CommentTypeCodeType CommentType, string CommentText)
		{
			this.TargetUser = TargetUser;
			this.ItemID = ItemID;
			this.TransactionID = TransactionID;
			this.CommentType = CommentType;
			this.CommentText = CommentText;
			Execute();
			return FeedbackID;
		}
		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public string LeaveFeedback(string TargetUser, string ItemID, CommentTypeCodeType CommentType, string CommentText)
		{
			this.TargetUser = TargetUser;
			this.ItemID = ItemID;
			this.CommentType = CommentType;
			this.CommentText = CommentText;
			Execute();
			return FeedbackID;
		}
		
				
		///
		/// For backward compatibility with old wrappers
		/// 
		///
		public string LeaveFeedback(string ItemID, string CommentText, CommentTypeCodeType CommentType, string TransactionID, string TargetUser)
		{
			this.ItemID = ItemID;
			this.CommentText = CommentText;
			this.CommentType = CommentType;
			this.TransactionID = TransactionID;
			this.TargetUser = TargetUser;

			Execute();
			return ApiResponse.FeedbackID;
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
		/// Gets or sets the <see cref="LeaveFeedbackRequestType"/> for this API call.
		/// </summary>
		public LeaveFeedbackRequestType ApiRequest
		{ 
			get { return (LeaveFeedbackRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="LeaveFeedbackResponseType"/> for this API call.
		/// </summary>
		public LeaveFeedbackResponseType ApiResponse
		{ 
			get { return (LeaveFeedbackResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="LeaveFeedbackRequestType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemID
		{ 
			get { return ApiRequest.ItemID; }
			set { ApiRequest.ItemID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="LeaveFeedbackRequestType.CommentText"/> of type <see cref="string"/>.
		/// </summary>
		public string CommentText
		{ 
			get { return ApiRequest.CommentText; }
			set { ApiRequest.CommentText = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="LeaveFeedbackRequestType.CommentType"/> of type <see cref="CommentTypeCodeType"/>.
		/// </summary>
		public CommentTypeCodeType CommentType
		{ 
			get { return ApiRequest.CommentType; }
			set { ApiRequest.CommentType = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="LeaveFeedbackRequestType.TransactionID"/> of type <see cref="string"/>.
		/// </summary>
		public string TransactionID
		{ 
			get { return ApiRequest.TransactionID; }
			set { ApiRequest.TransactionID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="LeaveFeedbackRequestType.TargetUser"/> of type <see cref="string"/>.
		/// </summary>
		public string TargetUser
		{ 
			get { return ApiRequest.TargetUser; }
			set { ApiRequest.TargetUser = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="LeaveFeedbackRequestType.SellerItemRatingDetailArray"/> of type <see cref="ItemRatingDetailsTypeCollection"/>.
		/// </summary>
		public ItemRatingDetailsTypeCollection SellerItemRatingDetailArrayList
		{ 
			get { return ApiRequest.SellerItemRatingDetailArray; }
			set { ApiRequest.SellerItemRatingDetailArray = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="LeaveFeedbackRequestType.OrderLineItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string OrderLineItemID
		{ 
			get { return ApiRequest.OrderLineItemID; }
			set { ApiRequest.OrderLineItemID = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="LeaveFeedbackResponseType.FeedbackID"/> of type <see cref="string"/>.
		/// </summary>
		public string FeedbackID
		{ 
			get { return ApiResponse.FeedbackID; }
		}
		

		#endregion

		
	}
}
