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
	public class AddToWatchListCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public AddToWatchListCall()
		{
			ApiRequest = new AddToWatchListRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public AddToWatchListCall(ApiContext ApiContext)
		{
			ApiRequest = new AddToWatchListRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Adds one or more items to the user's My eBay watch list.
		/// </summary>
		/// 
		/// <param name="ItemIDList">
		/// The <b>ItemID</b> of the item that is to be added to the watch list.
		/// The item must be a currently active item, and the total number
		/// of items in the
		/// watch list (after the items in the request have been added) cannot exceed
		/// the maximum allowed number of watch list items. One or more <b>ItemID</b> fields can be specified. A separate error node will be
		/// returned for each item that fails.
		/// 
		/// Either <b>ItemID</b> or <b>VariationKey</b> is required
		/// (but do not pass in both).
		/// </param>
		///
		/// <param name="VariationKeyList">
		/// A variation (or set of variations) that you want to watch.
		/// Use this to watch a particular variation (not the entire item).
		/// Either the top-level <b>ItemID</b> or <b>VariationKey</b> is required
		/// (but do not pass in both).
		/// </param>
		///
		public int AddToWatchList(StringCollection ItemIDList, VariationKeyTypeCollection VariationKeyList)
		{
			this.ItemIDList = ItemIDList;
			this.VariationKeyList = VariationKeyList;

			Execute();
			return ApiResponse.WatchListCount;
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
		/// Gets or sets the <see cref="AddToWatchListRequestType"/> for this API call.
		/// </summary>
		public AddToWatchListRequestType ApiRequest
		{ 
			get { return (AddToWatchListRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="AddToWatchListResponseType"/> for this API call.
		/// </summary>
		public AddToWatchListResponseType ApiResponse
		{ 
			get { return (AddToWatchListResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="AddToWatchListRequestType.ItemID"/> of type <see cref="StringCollection"/>.
		/// </summary>
		public StringCollection ItemIDList
		{ 
			get { return ApiRequest.ItemID; }
			set { ApiRequest.ItemID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="AddToWatchListRequestType.VariationKey"/> of type <see cref="VariationKeyTypeCollection"/>.
		/// </summary>
		public VariationKeyTypeCollection VariationKeyList
		{ 
			get { return ApiRequest.VariationKey; }
			set { ApiRequest.VariationKey = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="AddToWatchListResponseType.WatchListCount"/> of type <see cref="int"/>.
		/// </summary>
		public int WatchListCount
		{ 
			get { return ApiResponse.WatchListCount; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="AddToWatchListResponseType.WatchListMaximum"/> of type <see cref="int"/>.
		/// </summary>
		public int WatchListMaximum
		{ 
			get { return ApiResponse.WatchListMaximum; }
		}
		

		#endregion

		
	}
}
