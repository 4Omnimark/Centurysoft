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
	public class ReviseSellingManagerInventoryFolderCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public ReviseSellingManagerInventoryFolderCall()
		{
			ApiRequest = new ReviseSellingManagerInventoryFolderRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public ReviseSellingManagerInventoryFolderCall(ApiContext ApiContext)
		{
			ApiRequest = new ReviseSellingManagerInventoryFolderRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Renames a Selling Manager inventory folder.
		/// This call is subject to change without notice; the deprecation process is
		/// inapplicable to this call.
		/// </summary>
		/// 
		/// <param name="Folder">
		/// Identifier of the folder to be modified. Valid inputs are folder ID,
		/// folder name, and comments.
		/// </param>
		///
		public SellingManagerFolderDetailsType ReviseSellingManagerInventoryFolder(SellingManagerFolderDetailsType Folder)
		{
			this.Folder = Folder;

			Execute();
			return ApiResponse.Folder;
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
		/// Gets or sets the <see cref="ReviseSellingManagerInventoryFolderRequestType"/> for this API call.
		/// </summary>
		public ReviseSellingManagerInventoryFolderRequestType ApiRequest
		{ 
			get { return (ReviseSellingManagerInventoryFolderRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="ReviseSellingManagerInventoryFolderResponseType"/> for this API call.
		/// </summary>
		public ReviseSellingManagerInventoryFolderResponseType ApiResponse
		{ 
			get { return (ReviseSellingManagerInventoryFolderResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseSellingManagerInventoryFolderRequestType.Folder"/> of type <see cref="SellingManagerFolderDetailsType"/>.
		/// </summary>
		public SellingManagerFolderDetailsType Folder
		{ 
			get { return ApiRequest.Folder; }
			set { ApiRequest.Folder = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="ReviseSellingManagerInventoryFolderResponseType.Folder"/> of type <see cref="SellingManagerFolderDetailsType"/>.
		/// </summary>
		public SellingManagerFolderDetailsType FolderReturn
		{ 
			get { return ApiResponse.Folder; }
		}
		

		#endregion

		
	}
}
