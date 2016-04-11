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
	public class DeleteSellingManagerInventoryFolderCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public DeleteSellingManagerInventoryFolderCall()
		{
			ApiRequest = new DeleteSellingManagerInventoryFolderRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public DeleteSellingManagerInventoryFolderCall(ApiContext ApiContext)
		{
			ApiRequest = new DeleteSellingManagerInventoryFolderRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Removes an inventory folder when a user deletes it in My eBay.
		/// This call is subject to change without notice; the deprecation process is inapplicable
		/// to this call.
		/// </summary>
		/// 
		/// <param name="FolderID">
		/// Unique ID of the folder to be deleted.
		/// </param>
		///
		public void DeleteSellingManagerInventoryFolder(long FolderID)
		{
			this.FolderID = FolderID;

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
		/// Gets or sets the <see cref="DeleteSellingManagerInventoryFolderRequestType"/> for this API call.
		/// </summary>
		public DeleteSellingManagerInventoryFolderRequestType ApiRequest
		{ 
			get { return (DeleteSellingManagerInventoryFolderRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="DeleteSellingManagerInventoryFolderResponseType"/> for this API call.
		/// </summary>
		public DeleteSellingManagerInventoryFolderResponseType ApiResponse
		{ 
			get { return (DeleteSellingManagerInventoryFolderResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="DeleteSellingManagerInventoryFolderRequestType.FolderID"/> of type <see cref="long"/>.
		/// </summary>
		public long FolderID
		{ 
			get { return ApiRequest.FolderID; }
			set { ApiRequest.FolderID = value; }
		}
		
		

		#endregion

		
	}
}
