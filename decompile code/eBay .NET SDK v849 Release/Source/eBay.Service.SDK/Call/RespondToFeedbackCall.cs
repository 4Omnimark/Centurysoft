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
	public class RespondToFeedbackCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public RespondToFeedbackCall()
		{
			ApiRequest = new RespondToFeedbackRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public RespondToFeedbackCall(ApiContext ApiContext)
		{
			ApiRequest = new RespondToFeedbackRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Used to reply to feedback that has been left for a user, or to post a
		/// follow-up comment to a feedback comment the user has left for someone else.
		/// </summary>
		/// 
		/// <param name="FeedbackID">
		/// A unique identifier for a Feedback record. Buying and selling partners
		/// leave feedback for one another after the completion of an order.
		/// Feedback is left at the order line item (transaction) level, so a
		/// Feedback comment for each line item in a Combined Payment order is
		/// expected from the buyer and seller. A unique <b>FeedbackID</b> is created
		/// whenever a buyer leaves feedback for a seller, and vice versa. A
		/// <b>FeedbackID</b> is created by eBay when feedback is left through the eBay
		/// site, or through the <b>LeaveFeedback</b> call. <b>FeedbackIDs</b> can be retrieved
		/// with the <b>GetFeedback</b> call. In the <b>RespondToFeedback</b> call, <b>FeedbackID</b> can
		/// be used as an input filter to respond to a specific Feedback comment.
		/// Since Feedback is always linked to a unique order line item, an
		/// <b>ItemID</b>/<b>TransactionID</b> pair or an <b>OrderLineItemID</b> can also be used to
		/// respond to a Feedback comment.
		/// </param>
		///
		/// <param name="ItemID">
		/// Unique identifier for an eBay item listing. A listing can have multiple
		/// order line items (transactions), but only one <b>ItemID</b>. An <b>ItemID</b> can be
		/// paired up with a corresponding <b>TransactionID</b> and used as an input filter
		/// to respond to a Feedback comment in the <b>RespondToFeedback</b> call. Unless
		/// the specific Feedback record is identified by a <b>FeedbackID</b> or an
		/// <b>OrderLineItemID</b> in the request, an <b>ItemID</b>/<b>TransactionID</b> pair is
		/// required.
		/// </param>
		///
		/// <param name="TransactionID">
		/// Unique identifier for an eBay order line item (transaction). A
		/// <b>TransactionID</b> can be paired up with its corresponding <b>ItemID</b> and used as
		/// an input filter to respond to a Feedback comment in the
		/// <b>RespondToFeedback</b> call. Unless the specific Feedback record is
		/// identified by a <b>FeedbackID</b> or an <b>OrderLineItemID</b> in the request, an
		/// <b>ItemID</b>/<b>TransactionID</b> pair is required.
		/// </param>
		///
		/// <param name="TargetUserID">
		/// The eBay user ID of the caller's order partner. The caller is either
		/// replyting to or following up on this user's Feedback comment.
		/// </param>
		///
		/// <param name="ResponseType">
		/// Specifies whether the response is a reply or a follow-up to a Feedback
		/// comment left by the user identified in the <b>TargetUserID</b> field.
		/// </param>
		///
		/// <param name="ResponseText">
		/// Textual comment that the user who is subject of feedback may leave in
		/// response or rebuttal to the Feedback comment. Alternatively, when the
		/// <b>ResponseType</b> is <b>FollowUp</b>, this value contains the text of the follow-up
		/// comment.
		/// </param>
		///
		/// <param name="OrderLineItemID">
		/// <b>OrderLineItemID</b> is a unique identifier for an eBay order line item and
		/// is based upon the concatenation of <b>ItemID</b> and <b>TransactionID</b>, with a
		/// hyphen in between these two IDs. Since Feedback is always linked to a
		/// unique order line item, an <b>OrderLineItemID</b> can be used to respond
		/// to a Feedback comment.
		/// 
		/// Unless an <b>ItemID</b>/<b>TransactionID</b> pair or a <b>FeedbackID</b> is used to identify
		/// a Feedback record, the <b>OrderLineItemID</b> must be specified.
		/// 
		/// </param>
		///
		public void RespondToFeedback(string FeedbackID, string ItemID, string TransactionID, string TargetUserID, FeedbackResponseCodeType ResponseType, string ResponseText, string OrderLineItemID)
		{
			this.FeedbackID = FeedbackID;
			this.ItemID = ItemID;
			this.TransactionID = TransactionID;
			this.TargetUserID = TargetUserID;
			this.ResponseType = ResponseType;
			this.ResponseText = ResponseText;
			this.OrderLineItemID = OrderLineItemID;

			Execute();
			
		}


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public void RespondToFeedback(string TargetUserID, string ItemID, string TransactionID, FeedbackResponseCodeType ResponseType, string ResponseText)
		{
			this.TargetUserID = TargetUserID;
			this.ItemID = ItemID;
			this.TransactionID = TransactionID;
			this.ResponseType = ResponseType;
			this.ResponseText = ResponseText;
			Execute();
		}
		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public void RespondToFeedback(string TargetUserID, string ItemID, FeedbackResponseCodeType ResponseType, string ResponseText)
		{
			this.TargetUserID = TargetUserID;
			this.ItemID = ItemID;
			this.ResponseType = ResponseType;
			this.ResponseText = ResponseText;
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
		/// Gets or sets the <see cref="RespondToFeedbackRequestType"/> for this API call.
		/// </summary>
		public RespondToFeedbackRequestType ApiRequest
		{ 
			get { return (RespondToFeedbackRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="RespondToFeedbackResponseType"/> for this API call.
		/// </summary>
		public RespondToFeedbackResponseType ApiResponse
		{ 
			get { return (RespondToFeedbackResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="RespondToFeedbackRequestType.FeedbackID"/> of type <see cref="string"/>.
		/// </summary>
		public string FeedbackID
		{ 
			get { return ApiRequest.FeedbackID; }
			set { ApiRequest.FeedbackID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="RespondToFeedbackRequestType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemID
		{ 
			get { return ApiRequest.ItemID; }
			set { ApiRequest.ItemID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="RespondToFeedbackRequestType.TransactionID"/> of type <see cref="string"/>.
		/// </summary>
		public string TransactionID
		{ 
			get { return ApiRequest.TransactionID; }
			set { ApiRequest.TransactionID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="RespondToFeedbackRequestType.TargetUserID"/> of type <see cref="string"/>.
		/// </summary>
		public string TargetUserID
		{ 
			get { return ApiRequest.TargetUserID; }
			set { ApiRequest.TargetUserID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="RespondToFeedbackRequestType.ResponseType"/> of type <see cref="FeedbackResponseCodeType"/>.
		/// </summary>
		public FeedbackResponseCodeType ResponseType
		{ 
			get { return ApiRequest.ResponseType; }
			set { ApiRequest.ResponseType = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="RespondToFeedbackRequestType.ResponseText"/> of type <see cref="string"/>.
		/// </summary>
		public string ResponseText
		{ 
			get { return ApiRequest.ResponseText; }
			set { ApiRequest.ResponseText = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="RespondToFeedbackRequestType.OrderLineItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string OrderLineItemID
		{ 
			get { return ApiRequest.OrderLineItemID; }
			set { ApiRequest.OrderLineItemID = value; }
		}
		
		

		#endregion

		
	}
}
