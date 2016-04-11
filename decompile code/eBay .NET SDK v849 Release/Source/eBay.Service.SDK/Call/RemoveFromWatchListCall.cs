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
	public class RemoveFromWatchListCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public RemoveFromWatchListCall()
		{
			ApiRequest = new RemoveFromWatchListRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public RemoveFromWatchListCall(ApiContext ApiContext)
		{
			ApiRequest = new RemoveFromWatchListRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Enables a user to remove one or more items from their My eBay watch list.
		/// </summary>
		/// 
		/// <param name="ItemIDList">
		/// The ID of the item to be removed from the
		/// watch list. Either ItemID, RemoveAllItems, or VariationKey must
		/// be specified, but NOT more than one of these.
		/// Multiple ItemID fields can be specified in the same request.
		/// </param>
		///
		/// <param name="RemoveAllItems">
		/// If true, then all the items in the user's
		/// watch list are removed. Either ItemID, RemoveAllItems, or VariationKey must be specified, but NOT more than one of these.
		/// </param>
		///
		/// <param name="VariationKeyList">
		/// A variation (or set of variations) that you want to remove
		/// from the watch list. Either ItemID, RemoveAllItems, or VariationKey must be specified, but NOT more than one of these.
		/// </param>
		///
		public int RemoveFromWatchList(StringCollection ItemIDList, bool RemoveAllItems, VariationKeyTypeCollection VariationKeyList)
		{
			this.ItemIDList = ItemIDList;
			this.RemoveAllItems = RemoveAllItems;
			this.VariationKeyList = VariationKeyList;

			Execute();
			return ApiResponse.WatchListCount;
		}


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public int RemoveFromWatchList(StringCollection ItemIDList)
		{
			this.ItemIDList = ItemIDList;
			Execute();
			return this.WatchListCount;
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
		/// Gets or sets the <see cref="RemoveFromWatchListRequestType"/> for this API call.
		/// </summary>
		public RemoveFromWatchListRequestType ApiRequest
		{ 
			get { return (RemoveFromWatchListRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="RemoveFromWatchListResponseType"/> for this API call.
		/// </summary>
		public RemoveFromWatchListResponseType ApiResponse
		{ 
			get { return (RemoveFromWatchListResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="RemoveFromWatchListRequestType.ItemID"/> of type <see cref="StringCollection"/>.
		/// </summary>
		public StringCollection ItemIDList
		{ 
			get { return ApiRequest.ItemID; }
			set { ApiRequest.ItemID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="RemoveFromWatchListRequestType.RemoveAllItems"/> of type <see cref="bool"/>.
		/// </summary>
		public bool RemoveAllItems
		{ 
			get { return ApiRequest.RemoveAllItems; }
			set { ApiRequest.RemoveAllItems = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="RemoveFromWatchListRequestType.VariationKey"/> of type <see cref="VariationKeyTypeCollection"/>.
		/// </summary>
		public VariationKeyTypeCollection VariationKeyList
		{ 
			get { return ApiRequest.VariationKey; }
			set { ApiRequest.VariationKey = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="RemoveFromWatchListResponseType.WatchListCount"/> of type <see cref="int"/>.
		/// </summary>
		public int WatchListCount
		{ 
			get { return ApiResponse.WatchListCount; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="RemoveFromWatchListResponseType.WatchListMaximum"/> of type <see cref="int"/>.
		/// </summary>
		public int WatchListMaximum
		{ 
			get { return ApiResponse.WatchListMaximum; }
		}
		

		#endregion

		
	}
}
