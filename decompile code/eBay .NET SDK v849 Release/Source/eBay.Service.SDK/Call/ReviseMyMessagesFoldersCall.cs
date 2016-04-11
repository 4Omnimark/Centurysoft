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
	public class ReviseMyMessagesFoldersCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public ReviseMyMessagesFoldersCall()
		{
			ApiRequest = new ReviseMyMessagesFoldersRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public ReviseMyMessagesFoldersCall(ApiContext ApiContext)
		{
			ApiRequest = new ReviseMyMessagesFoldersRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Renames, removes, or restores the specified My Messages folders for
		/// a given user.
		/// </summary>
		/// 
		/// <param name="Operation">
		/// Indicates the type of operation to perform on a
		/// specified My Messages folder. Operations include creating,
		/// renaming, removing, and restoring folders. Operations
		/// cannot be performed on the Inbox and Sent folders.
		/// </param>
		///
		/// <param name="FolderIDList">
		/// An ID that uniquely identifies the My Messages
		/// folder to perform the operation on. This value is set by
		/// eBay and cannot be changed. Retrieve FolderIDs
		/// by calling GetMyMessages with a DetailLevel of
		/// ReturnSummary. Inbox is FolderID = 0, and Sent is
		/// FolderID = 1.
		/// </param>
		///
		/// <param name="FolderNameList">
		/// The name of a specified My Messages folder. Depending
		/// on the specified Operation, the value is an existing
		/// folder name or a new folder name. Retrieve existing
		/// FolderNames by calling GetMyMessages with a DetailLevel
		/// of ReturnSummary. Inbox is FolderID = 0, and Sent is
		/// FolderID = 1.
		/// </param>
		///
		public void ReviseMyMessagesFolders(MyMessagesFolderOperationCodeType Operation, Int64Collection FolderIDList, StringCollection FolderNameList)
		{
			this.Operation = Operation;
			this.FolderIDList = FolderIDList;
			this.FolderNameList = FolderNameList;

			Execute();
			
		}


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public void ReviseMyMessagesFolders(MyMessagesFolderOperationCodeType Operation, StringCollection FolderNameList, Int64Collection FolderIDList)
		{
			this.Operation = Operation;
			this.FolderNameList = FolderNameList;
			this.FolderIDList = FolderIDList;
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
		/// Gets or sets the <see cref="ReviseMyMessagesFoldersRequestType"/> for this API call.
		/// </summary>
		public ReviseMyMessagesFoldersRequestType ApiRequest
		{ 
			get { return (ReviseMyMessagesFoldersRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="ReviseMyMessagesFoldersResponseType"/> for this API call.
		/// </summary>
		public ReviseMyMessagesFoldersResponseType ApiResponse
		{ 
			get { return (ReviseMyMessagesFoldersResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseMyMessagesFoldersRequestType.Operation"/> of type <see cref="MyMessagesFolderOperationCodeType"/>.
		/// </summary>
		public MyMessagesFolderOperationCodeType Operation
		{ 
			get { return ApiRequest.Operation; }
			set { ApiRequest.Operation = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseMyMessagesFoldersRequestType.FolderID"/> of type <see cref="Int64Collection"/>.
		/// </summary>
		public Int64Collection FolderIDList
		{ 
			get { return ApiRequest.FolderID; }
			set { ApiRequest.FolderID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseMyMessagesFoldersRequestType.FolderName"/> of type <see cref="StringCollection"/>.
		/// </summary>
		public StringCollection FolderNameList
		{ 
			get { return ApiRequest.FolderName; }
			set { ApiRequest.FolderName = value; }
		}
		
		

		#endregion

		
	}
}
