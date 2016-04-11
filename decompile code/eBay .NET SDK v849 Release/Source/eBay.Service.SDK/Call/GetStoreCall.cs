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
	public class GetStoreCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetStoreCall()
		{
			ApiRequest = new GetStoreRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetStoreCall(ApiContext ApiContext)
		{
			ApiRequest = new GetStoreRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Retrieves configuration information for the eBay store owned by the specified
		/// UserID, or by the caller.
		/// </summary>
		/// 
		/// <param name="CategoryStructureOnly">
		/// If this is set to True, only the category structure of the store is
		/// returned. If this is not specified or set to False, the complete store
		/// configuration is returned.
		/// </param>
		///
		/// <param name="RootCategoryID">
		/// Specifies the category ID for the topmost category to return (along with
		/// the subcategories under it, the value of the LevelLimit property
		/// determining how deep). This tag is optional. If RootCategoryID is not
		/// specified, then the category tree starting at that root Category is
		/// returned.
		/// </param>
		///
		/// <param name="LevelLimit">
		/// Specifies the limit for the number of levels of the category hierarchy
		/// to return, where the given root category is level 1 and its children are
		/// level 2. Only categories at or above the level specified are returned.
		/// This tag is optional. If LevelLimit is not set, the complete category
		/// hierarchy is returned. Stores support category hierarchies up to 3
		/// levels only.
		/// </param>
		///
		/// <param name="UserID">
		/// Specifies the user whose store data is to be returned. If not specified,
		/// then the store returned is that for the requesting user.
		/// </param>
		///
		public StoreType GetStore(bool CategoryStructureOnly, long RootCategoryID, int LevelLimit, string UserID)
		{
			this.CategoryStructureOnly = CategoryStructureOnly;
			this.RootCategoryID = RootCategoryID;
			this.LevelLimit = LevelLimit;
			this.UserID = UserID;

			Execute();
			return ApiResponse.Store;
		}


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public StoreType GetStore()
		{

			Execute();
			return ApiResponse.Store;
		}


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public StoreType GetStore(bool CategoryStructureOnly, long RootCategoryID, int LevelLimit)
		{
			this.CategoryStructureOnly = CategoryStructureOnly;
			this.RootCategoryID = RootCategoryID;
			this.LevelLimit = LevelLimit;

			Execute();
			return ApiResponse.Store;
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
		/// Gets or sets the <see cref="GetStoreRequestType"/> for this API call.
		/// </summary>
		public GetStoreRequestType ApiRequest
		{ 
			get { return (GetStoreRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetStoreResponseType"/> for this API call.
		/// </summary>
		public GetStoreResponseType ApiResponse
		{ 
			get { return (GetStoreResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetStoreRequestType.CategoryStructureOnly"/> of type <see cref="bool"/>.
		/// </summary>
		public bool CategoryStructureOnly
		{ 
			get { return ApiRequest.CategoryStructureOnly; }
			set { ApiRequest.CategoryStructureOnly = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetStoreRequestType.RootCategoryID"/> of type <see cref="long"/>.
		/// </summary>
		public long RootCategoryID
		{ 
			get { return ApiRequest.RootCategoryID; }
			set { ApiRequest.RootCategoryID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetStoreRequestType.LevelLimit"/> of type <see cref="int"/>.
		/// </summary>
		public int LevelLimit
		{ 
			get { return ApiRequest.LevelLimit; }
			set { ApiRequest.LevelLimit = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetStoreRequestType.UserID"/> of type <see cref="string"/>.
		/// </summary>
		public string UserID
		{ 
			get { return ApiRequest.UserID; }
			set { ApiRequest.UserID = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetStoreResponseType.Store"/> of type <see cref="StoreType"/>.
		/// </summary>
		public StoreType Store
		{ 
			get { return ApiResponse.Store; }
		}
		

		#endregion

		
	}
}
