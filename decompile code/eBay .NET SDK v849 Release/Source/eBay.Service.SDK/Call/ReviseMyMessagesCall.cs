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
	public class ReviseMyMessagesCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public ReviseMyMessagesCall()
		{
			ApiRequest = new ReviseMyMessagesRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public ReviseMyMessagesCall(ApiContext ApiContext)
		{
			ApiRequest = new ReviseMyMessagesRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Sets the read state for messages, sets the flagged state of messages,
		/// and moves messages into and out of folders.
		/// </summary>
		/// 
		/// <param name="MessageIDList">
		/// Contains a list of up to 10 MessageID values.
		/// 
		/// MessageIDs must be included in
		/// the request. Messages in the Sent box cannot be moved,
		/// marked as Read, or Flagged.
		/// </param>
		///
		/// <param name="AlertIDList">
		/// This field will be deprecated in an upcoming release.
		/// This field formerly contained a list of up to 10 AlertID values.
		/// 
		/// Alerts cannot be flagged. Alerts cannot be
		/// moved into a new folder until they have been resolved.
		/// 
		/// Resolve alerts by marking Read (if no action is
		/// required), or by using ActionURL (if action is
		/// required).
		/// </param>
		///
		/// <param name="Read">
		/// Changes the read states of all
		/// messages specified in a request.
		/// At least one of the following
		/// must be specified in the
		/// request: Read, Flagged, or FolderID.
		/// Messages in the Sent box cannot be moved,
		/// marked as Read, or Flagged.
		/// 
		/// Note that messages retrieved
		/// with the API are not automatically marked Read,
		/// and must be marked Read with this call.
		/// </param>
		///
		/// <param name="Flagged">
		/// Changes the flagged states of all messages specified in
		/// a request by their MessageID values. At least one of
		/// Read, Flagged, or FolderID must be specified in the
		/// request. Messages in the Sent box cannot be moved,
		/// marked as Read, or Flagged.
		/// </param>
		///
		/// <param name="FolderID">
		/// An ID that uniquely identifies the My Messages folder to
		/// move messages into. At least one of Read,
		/// Flagged, or FolderID must be specified in the request.
		/// 
		/// Messages
		/// in the Sent box cannot be moved, marked as Read, or
		/// Flagged.
		/// </param>
		///
		public void ReviseMyMessages(StringCollection MessageIDList, StringCollection AlertIDList, bool Read, bool Flagged, long FolderID)
		{
			this.MessageIDList = MessageIDList;
			this.AlertIDList = AlertIDList;
			this.Read = Read;
			this.Flagged = Flagged;
			this.FolderID = FolderID;

			Execute();
			
		}


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public void ReviseMyMessages(bool Read, bool Flagged, StringCollection MessageIDList)
		{
			this.Read = Read;
			this.Flagged = Flagged;
			this.MessageIDList = MessageIDList;
			this.Execute();
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
		/// Gets or sets the <see cref="ReviseMyMessagesRequestType"/> for this API call.
		/// </summary>
		public ReviseMyMessagesRequestType ApiRequest
		{ 
			get { return (ReviseMyMessagesRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="ReviseMyMessagesResponseType"/> for this API call.
		/// </summary>
		public ReviseMyMessagesResponseType ApiResponse
		{ 
			get { return (ReviseMyMessagesResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseMyMessagesRequestType.MessageIDs"/> of type <see cref="StringCollection"/>.
		/// </summary>
		public StringCollection MessageIDList
		{ 
			get { return ApiRequest.MessageIDs; }
			set { ApiRequest.MessageIDs = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseMyMessagesRequestType.AlertIDs"/> of type <see cref="StringCollection"/>.
		/// </summary>
		public StringCollection AlertIDList
		{ 
			get { return ApiRequest.AlertIDs; }
			set { ApiRequest.AlertIDs = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseMyMessagesRequestType.Read"/> of type <see cref="bool"/>.
		/// </summary>
		public bool Read
		{ 
			get { return ApiRequest.Read; }
			set { ApiRequest.Read = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseMyMessagesRequestType.Flagged"/> of type <see cref="bool"/>.
		/// </summary>
		public bool Flagged
		{ 
			get { return ApiRequest.Flagged; }
			set { ApiRequest.Flagged = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseMyMessagesRequestType.FolderID"/> of type <see cref="long"/>.
		/// </summary>
		public long FolderID
		{ 
			get { return ApiRequest.FolderID; }
			set { ApiRequest.FolderID = value; }
		}
		
		

		#endregion

		
	}
}
