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
	public class AddSellingManagerInventoryFolderCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public AddSellingManagerInventoryFolderCall()
		{
			ApiRequest = new AddSellingManagerInventoryFolderRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public AddSellingManagerInventoryFolderCall(ApiContext ApiContext)
		{
			ApiRequest = new AddSellingManagerInventoryFolderRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Adds a new product folder to a user's Selling Manager account.
		/// </summary>
		/// 
		/// <param name="FolderName">
		/// Name of the new Selling Manager inventory folder.
		/// </param>
		///
		/// <param name="ParentFolderID">
		/// Unique Folder ID of parent folder. If no <b>ParentFolderID</b> is submitted, Folder
		/// is added to root level.
		/// </param>
		///
		/// <param name="Comment">
		/// Contains comments that will be associated with this folder.
		/// </param>
		///
		public long AddSellingManagerInventoryFolder(string FolderName, long ParentFolderID, string Comment)
		{
			this.FolderName = FolderName;
			this.ParentFolderID = ParentFolderID;
			this.Comment = Comment;

			Execute();
			return ApiResponse.FolderID;
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
		/// Gets or sets the <see cref="AddSellingManagerInventoryFolderRequestType"/> for this API call.
		/// </summary>
		public AddSellingManagerInventoryFolderRequestType ApiRequest
		{ 
			get { return (AddSellingManagerInventoryFolderRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="AddSellingManagerInventoryFolderResponseType"/> for this API call.
		/// </summary>
		public AddSellingManagerInventoryFolderResponseType ApiResponse
		{ 
			get { return (AddSellingManagerInventoryFolderResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="AddSellingManagerInventoryFolderRequestType.FolderName"/> of type <see cref="string"/>.
		/// </summary>
		public string FolderName
		{ 
			get { return ApiRequest.FolderName; }
			set { ApiRequest.FolderName = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="AddSellingManagerInventoryFolderRequestType.ParentFolderID"/> of type <see cref="long"/>.
		/// </summary>
		public long ParentFolderID
		{ 
			get { return ApiRequest.ParentFolderID; }
			set { ApiRequest.ParentFolderID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="AddSellingManagerInventoryFolderRequestType.Comment"/> of type <see cref="string"/>.
		/// </summary>
		public string Comment
		{ 
			get { return ApiRequest.Comment; }
			set { ApiRequest.Comment = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="AddSellingManagerInventoryFolderResponseType.FolderID"/> of type <see cref="long"/>.
		/// </summary>
		public long FolderID
		{ 
			get { return ApiResponse.FolderID; }
		}
		

		#endregion

		
	}
}
