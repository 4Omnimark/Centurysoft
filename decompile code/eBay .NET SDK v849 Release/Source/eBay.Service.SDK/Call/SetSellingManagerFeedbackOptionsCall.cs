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
	public class SetSellingManagerFeedbackOptionsCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public SetSellingManagerFeedbackOptionsCall()
		{
			ApiRequest = new SetSellingManagerFeedbackOptionsRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public SetSellingManagerFeedbackOptionsCall(ApiContext ApiContext)
		{
			ApiRequest = new SetSellingManagerFeedbackOptionsRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Enables Selling Manager subscribers to store standard feedback comments that can
		/// be left for their buyers. Selling Manager Pro subscribers can also specify what
		/// events, if any, will trigger an automated feedback to buyers.
		/// </summary>
		/// 
		/// <param name="AutomatedLeaveFeedbackEvent">
		/// Specifies the event that will trigger automated feedback to the buyer.
		/// Applies to Selling Manager Pro subscribers only.
		/// </param>
		///
		/// <param name="StoredCommentList">
		/// Contains a set of comments from which one can be selected to leave
		/// feedback for a buyer. If automated feedback is used, a comment is
		/// selected from the set randomly. Automated feedback applies to Selling
		/// Manager Pro subscribers only. Stored comments cannot be replaced or
		/// edited individually. Submitting a stored comments array replaces all
		/// existing stored comments.
		/// 
		/// </param>
		///
		public void SetSellingManagerFeedbackOptions(AutomatedLeaveFeedbackEventCodeType AutomatedLeaveFeedbackEvent, StringCollection StoredCommentList)
		{
			this.AutomatedLeaveFeedbackEvent = AutomatedLeaveFeedbackEvent;
			this.StoredCommentList = StoredCommentList;

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
		/// Gets or sets the <see cref="SetSellingManagerFeedbackOptionsRequestType"/> for this API call.
		/// </summary>
		public SetSellingManagerFeedbackOptionsRequestType ApiRequest
		{ 
			get { return (SetSellingManagerFeedbackOptionsRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="SetSellingManagerFeedbackOptionsResponseType"/> for this API call.
		/// </summary>
		public SetSellingManagerFeedbackOptionsResponseType ApiResponse
		{ 
			get { return (SetSellingManagerFeedbackOptionsResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="SetSellingManagerFeedbackOptionsRequestType.AutomatedLeaveFeedbackEvent"/> of type <see cref="AutomatedLeaveFeedbackEventCodeType"/>.
		/// </summary>
		public AutomatedLeaveFeedbackEventCodeType AutomatedLeaveFeedbackEvent
		{ 
			get { return ApiRequest.AutomatedLeaveFeedbackEvent; }
			set { ApiRequest.AutomatedLeaveFeedbackEvent = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SetSellingManagerFeedbackOptionsRequestType.StoredComments"/> of type <see cref="StringCollection"/>.
		/// </summary>
		public StringCollection StoredCommentList
		{ 
			get { return ApiRequest.StoredComments; }
			set { ApiRequest.StoredComments = value; }
		}
		
		

		#endregion

		
	}
}
