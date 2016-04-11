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
	public class MoveSellingManagerInventoryFolderCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public MoveSellingManagerInventoryFolderCall()
		{
			ApiRequest = new MoveSellingManagerInventoryFolderRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public MoveSellingManagerInventoryFolderCall(ApiContext ApiContext)
		{
			ApiRequest = new MoveSellingManagerInventoryFolderRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Moves a Selling Manager inventory folder.
		/// 
		/// This call is subject to change without notice; the deprecation process is
		/// inapplicable to this call.
		/// </summary>
		/// 
		/// <param name="FolderID">
		/// Unique ID of the folder that will be moved. User can retrieve the FolderId
		/// using GetSellingManagerInventoryFolder.
		/// </param>
		///
		/// <param name="NewParentFolderID">
		/// Unique folder ID for the new parent folder. If no NewParentFolderID is
		/// submitted, the folder is moved to the root level.
		/// </param>
		///
		public void MoveSellingManagerInventoryFolder(long FolderID, long NewParentFolderID)
		{
			this.FolderID = FolderID;
			this.NewParentFolderID = NewParentFolderID;

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
		/// Gets or sets the <see cref="MoveSellingManagerInventoryFolderRequestType"/> for this API call.
		/// </summary>
		public MoveSellingManagerInventoryFolderRequestType ApiRequest
		{ 
			get { return (MoveSellingManagerInventoryFolderRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="MoveSellingManagerInventoryFolderResponseType"/> for this API call.
		/// </summary>
		public MoveSellingManagerInventoryFolderResponseType ApiResponse
		{ 
			get { return (MoveSellingManagerInventoryFolderResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="MoveSellingManagerInventoryFolderRequestType.FolderID"/> of type <see cref="long"/>.
		/// </summary>
		public long FolderID
		{ 
			get { return ApiRequest.FolderID; }
			set { ApiRequest.FolderID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="MoveSellingManagerInventoryFolderRequestType.NewParentFolderID"/> of type <see cref="long"/>.
		/// </summary>
		public long NewParentFolderID
		{ 
			get { return ApiRequest.NewParentFolderID; }
			set { ApiRequest.NewParentFolderID = value; }
		}
		
		

		#endregion

		
	}
}
