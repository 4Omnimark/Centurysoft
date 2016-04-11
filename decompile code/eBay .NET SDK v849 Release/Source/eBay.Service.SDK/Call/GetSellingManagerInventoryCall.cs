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
	public class GetSellingManagerInventoryCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetSellingManagerInventoryCall()
		{
			ApiRequest = new GetSellingManagerInventoryRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetSellingManagerInventoryCall(ApiContext ApiContext)
		{
			ApiRequest = new GetSellingManagerInventoryRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Retrieves a paginated list containing details of a user's Selling Manager inventory.
		/// This call is subject to change without notice; the deprecation process is
		/// inapplicable to this call.
		/// </summary>
		/// 
		/// <param name="Sort">
		/// Sets the sorting method for the results.
		/// </param>
		///
		/// <param name="FolderID">
		/// Specifies the inventory folder containing the requested inventory information.
		/// </param>
		///
		/// <param name="Pagination">
		/// Details about how many Products to return per page and which page to view.
		/// </param>
		///
		/// <param name="SortOrder">
		/// Order to be used for sorting retrieved product lists.
		/// </param>
		///
		/// <param name="Search">
		/// Specifies types and values to search for in the seller's listings.
		/// </param>
		///
		/// <param name="StoreCategoryID">
		/// Specifies a store category whose products will be returned.
		/// </param>
		///
		/// <param name="FilterList">
		/// Container for the list of filters that can be applied to the inventory information requested.
		/// </param>
		///
		public DateTime GetSellingManagerInventory(SellingManagerProductSortCodeType Sort, long FolderID, PaginationType Pagination, SortOrderCodeType SortOrder, SellingManagerSearchType Search, long StoreCategoryID, SellingManagerInventoryPropertyTypeCodeTypeCollection FilterList)
		{
			this.Sort = Sort;
			this.FolderID = FolderID;
			this.Pagination = Pagination;
			this.SortOrder = SortOrder;
			this.Search = Search;
			this.StoreCategoryID = StoreCategoryID;
			this.FilterList = FilterList;

			Execute();
			return ApiResponse.InventoryCountLastCalculatedDate;
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
		/// Gets or sets the <see cref="GetSellingManagerInventoryRequestType"/> for this API call.
		/// </summary>
		public GetSellingManagerInventoryRequestType ApiRequest
		{ 
			get { return (GetSellingManagerInventoryRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetSellingManagerInventoryResponseType"/> for this API call.
		/// </summary>
		public GetSellingManagerInventoryResponseType ApiResponse
		{ 
			get { return (GetSellingManagerInventoryResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellingManagerInventoryRequestType.Sort"/> of type <see cref="SellingManagerProductSortCodeType"/>.
		/// </summary>
		public SellingManagerProductSortCodeType Sort
		{ 
			get { return ApiRequest.Sort; }
			set { ApiRequest.Sort = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellingManagerInventoryRequestType.FolderID"/> of type <see cref="long"/>.
		/// </summary>
		public long FolderID
		{ 
			get { return ApiRequest.FolderID; }
			set { ApiRequest.FolderID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellingManagerInventoryRequestType.Pagination"/> of type <see cref="PaginationType"/>.
		/// </summary>
		public PaginationType Pagination
		{ 
			get { return ApiRequest.Pagination; }
			set { ApiRequest.Pagination = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellingManagerInventoryRequestType.SortOrder"/> of type <see cref="SortOrderCodeType"/>.
		/// </summary>
		public SortOrderCodeType SortOrder
		{ 
			get { return ApiRequest.SortOrder; }
			set { ApiRequest.SortOrder = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellingManagerInventoryRequestType.Search"/> of type <see cref="SellingManagerSearchType"/>.
		/// </summary>
		public SellingManagerSearchType Search
		{ 
			get { return ApiRequest.Search; }
			set { ApiRequest.Search = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellingManagerInventoryRequestType.StoreCategoryID"/> of type <see cref="long"/>.
		/// </summary>
		public long StoreCategoryID
		{ 
			get { return ApiRequest.StoreCategoryID; }
			set { ApiRequest.StoreCategoryID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellingManagerInventoryRequestType.Filter"/> of type <see cref="SellingManagerInventoryPropertyTypeCodeTypeCollection"/>.
		/// </summary>
		public SellingManagerInventoryPropertyTypeCodeTypeCollection FilterList
		{ 
			get { return ApiRequest.Filter; }
			set { ApiRequest.Filter = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellingManagerInventoryResponseType.InventoryCountLastCalculatedDate"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime InventoryCountLastCalculatedDate
		{ 
			get { return ApiResponse.InventoryCountLastCalculatedDate; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellingManagerInventoryResponseType.SellingManagerProduct"/> of type <see cref="SellingManagerProductTypeCollection"/>.
		/// </summary>
		public SellingManagerProductTypeCollection SellingManagerProductList
		{ 
			get { return ApiResponse.SellingManagerProduct; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellingManagerInventoryResponseType.PaginationResult"/> of type <see cref="PaginationResultType"/>.
		/// </summary>
		public PaginationResultType PaginationResult
		{ 
			get { return ApiResponse.PaginationResult; }
		}
		

		#endregion

		
	}
}
