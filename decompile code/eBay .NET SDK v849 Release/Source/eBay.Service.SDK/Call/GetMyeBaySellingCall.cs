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
	public class GetMyeBaySellingCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetMyeBaySellingCall()
		{
			ApiRequest = new GetMyeBaySellingRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetMyeBaySellingCall(ApiContext ApiContext)
		{
			ApiRequest = new GetMyeBaySellingRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Returns items from the Selling section of the user's My eBay account,
		/// including items that the user is currently selling (the Active list),
		/// items that have bids, sold items, and unsold items.
		/// </summary>
		/// 
		/// <param name="ScheduledList">
		/// Returns the list of items the user has scheduled to sell but whose
		/// listings have not yet opened.
		/// 
		/// Include and set the <b>ScheduledList.Include</b> flag to 'true' to
		/// retrieve the default response set of <b>ScheduledList</b> in the response.
		/// </param>
		///
		/// <param name="ActiveList">
		/// Returns the list of items the user is actively selling (the currently
		/// active listings).
		/// 
		/// Include and set the <b>ActiveList.Include</b> flag to 'true' to
		/// retrieve the default response set of <b>ActiveList</b> in the response.
		/// </param>
		///
		/// <param name="SoldList">
		/// Returns the list of items the user has sold.
		/// 
		/// Include and set the <b>SoldList.Include</b> flag to 'true' to
		/// retrieve the default response set of <b>SoldList</b> in the response.
		/// </param>
		///
		/// <param name="UnsoldList">
		/// Returns the list of items the user has listed, but whose listings
		/// have ended without being sold.
		/// 
		/// Include and set the <b>UnsoldList.Include</b> flag to 'true' to
		/// retrieve the default response set of <b>UnsoldList</b> in the response.
		/// </param>
		///
		/// <param name="BidList">
		/// This container is no longer applicable to <b>GetMyeBaySelling</b>.
		/// </param>
		///
		/// <param name="DeletedFromSoldList">
		/// Returns the list of items the user sold, and then deleted from
		/// their My eBay page. Allowed values for DurationInDays are 0-60.
		/// 
		/// Include and set the <b>DeletedFromSoldList.Include</b> flag to 'true' to
		/// retrieve the default response set of <b>DeletedFromSoldList</b> in the response.
		/// </param>
		///
		/// <param name="DeletedFromUnsoldList">
		/// Returns the list of items the user either ended or did not sell, and
		/// subsequently were deleted them from their My eBay page. Allowed
		/// values for DurationInDays are 0-60.
		/// 
		/// Include and set the <b>DeletedFromUnsoldList.Include</b> flag to 'true' to
		/// retrieve the default response set of <b>DeletedFromUnsoldList</b> in the response.
		/// </param>
		///
		/// <param name="SellingSummary">
		/// Returns a summary of the user's buying activity.
		/// 
		/// Include and set the <b>SellingSummary.Include</b> flag to 'true' to
		/// retrieve the default response set of <b>SellingSummary</b> in the response.
		/// </param>
		///
		/// <param name="HideVariations">
		/// If 'true', the Variations node is omitted for all multi-variation
		/// listings in the response.
		/// If 'false', the Variations node is returned for all
		/// multi-variation listings in the response. 
		/// 
		/// Please note that if the seller includes a large number of
		/// variations in many listings, retrieving variations (setting this
		/// flag to 'false') may degrade the call's performance. Therefore,
		/// when this is 'false', you may need to reduce the total
		/// number of items you're requesting at once (by using other input
		/// fields, such as Pagination).
		/// </param>
		///
		public SellingSummaryType GetMyeBaySelling(ItemListCustomizationType ScheduledList, ItemListCustomizationType ActiveList, ItemListCustomizationType SoldList, ItemListCustomizationType UnsoldList, ItemListCustomizationType BidList, ItemListCustomizationType DeletedFromSoldList, ItemListCustomizationType DeletedFromUnsoldList, ItemListCustomizationType SellingSummary, bool HideVariations)
		{
			this.ScheduledList = ScheduledList;
			this.ActiveList = ActiveList;
			this.SoldList = SoldList;
			this.UnsoldList = UnsoldList;
			this.BidList = BidList;
			this.DeletedFromSoldList = DeletedFromSoldList;
			this.DeletedFromUnsoldList = DeletedFromUnsoldList;
			this.SellingSummary = SellingSummary;
			this.HideVariations = HideVariations;

			Execute();
			return ApiResponse.SellingSummary;
		}


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public void GetMyeBaySelling()
		{
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
		/// Gets or sets the <see cref="GetMyeBaySellingRequestType"/> for this API call.
		/// </summary>
		public GetMyeBaySellingRequestType ApiRequest
		{ 
			get { return (GetMyeBaySellingRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetMyeBaySellingResponseType"/> for this API call.
		/// </summary>
		public GetMyeBaySellingResponseType ApiResponse
		{ 
			get { return (GetMyeBaySellingResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetMyeBaySellingRequestType.ScheduledList"/> of type <see cref="ItemListCustomizationType"/>.
		/// </summary>
		public ItemListCustomizationType ScheduledList
		{ 
			get { return ApiRequest.ScheduledList; }
			set { ApiRequest.ScheduledList = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetMyeBaySellingRequestType.ActiveList"/> of type <see cref="ItemListCustomizationType"/>.
		/// </summary>
		public ItemListCustomizationType ActiveList
		{ 
			get { return ApiRequest.ActiveList; }
			set { ApiRequest.ActiveList = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetMyeBaySellingRequestType.SoldList"/> of type <see cref="ItemListCustomizationType"/>.
		/// </summary>
		public ItemListCustomizationType SoldList
		{ 
			get { return ApiRequest.SoldList; }
			set { ApiRequest.SoldList = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetMyeBaySellingRequestType.UnsoldList"/> of type <see cref="ItemListCustomizationType"/>.
		/// </summary>
		public ItemListCustomizationType UnsoldList
		{ 
			get { return ApiRequest.UnsoldList; }
			set { ApiRequest.UnsoldList = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetMyeBaySellingRequestType.BidList"/> of type <see cref="ItemListCustomizationType"/>.
		/// </summary>
		public ItemListCustomizationType BidList
		{ 
			get { return ApiRequest.BidList; }
			set { ApiRequest.BidList = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetMyeBaySellingRequestType.DeletedFromSoldList"/> of type <see cref="ItemListCustomizationType"/>.
		/// </summary>
		public ItemListCustomizationType DeletedFromSoldList
		{ 
			get { return ApiRequest.DeletedFromSoldList; }
			set { ApiRequest.DeletedFromSoldList = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetMyeBaySellingRequestType.DeletedFromUnsoldList"/> of type <see cref="ItemListCustomizationType"/>.
		/// </summary>
		public ItemListCustomizationType DeletedFromUnsoldList
		{ 
			get { return ApiRequest.DeletedFromUnsoldList; }
			set { ApiRequest.DeletedFromUnsoldList = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetMyeBaySellingRequestType.SellingSummary"/> of type <see cref="ItemListCustomizationType"/>.
		/// </summary>
		public ItemListCustomizationType SellingSummary
		{ 
			get { return ApiRequest.SellingSummary; }
			set { ApiRequest.SellingSummary = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetMyeBaySellingRequestType.HideVariations"/> of type <see cref="bool"/>.
		/// </summary>
		public bool HideVariations
		{ 
			get { return ApiRequest.HideVariations; }
			set { ApiRequest.HideVariations = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetMyeBaySellingResponseType.SellingSummary"/> of type <see cref="SellingSummaryType"/>.
		/// </summary>
		public SellingSummaryType SellingSummaryReturn
		{ 
			get { return ApiResponse.SellingSummary; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetMyeBaySellingResponseType.ScheduledList"/> of type <see cref="PaginatedItemArrayType"/>.
		/// </summary>
		public PaginatedItemArrayType ScheduledListReturn
		{ 
			get { return ApiResponse.ScheduledList; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetMyeBaySellingResponseType.ActiveList"/> of type <see cref="PaginatedItemArrayType"/>.
		/// </summary>
		public PaginatedItemArrayType ActiveListReturn
		{ 
			get { return ApiResponse.ActiveList; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetMyeBaySellingResponseType.SoldList"/> of type <see cref="PaginatedOrderTransactionArrayType"/>.
		/// </summary>
		public PaginatedOrderTransactionArrayType SoldListReturn
		{ 
			get { return ApiResponse.SoldList; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetMyeBaySellingResponseType.UnsoldList"/> of type <see cref="PaginatedItemArrayType"/>.
		/// </summary>
		public PaginatedItemArrayType UnsoldListReturn
		{ 
			get { return ApiResponse.UnsoldList; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetMyeBaySellingResponseType.Summary"/> of type <see cref="MyeBaySellingSummaryType"/>.
		/// </summary>
		public MyeBaySellingSummaryType Summary
		{ 
			get { return ApiResponse.Summary; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetMyeBaySellingResponseType.BidList"/> of type <see cref="PaginatedItemArrayType"/>.
		/// </summary>
		public PaginatedItemArrayType BidListReturn
		{ 
			get { return ApiResponse.BidList; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetMyeBaySellingResponseType.DeletedFromSoldList"/> of type <see cref="PaginatedOrderTransactionArrayType"/>.
		/// </summary>
		public PaginatedOrderTransactionArrayType DeletedFromSoldListReturn
		{ 
			get { return ApiResponse.DeletedFromSoldList; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetMyeBaySellingResponseType.DeletedFromUnsoldList"/> of type <see cref="PaginatedItemArrayType"/>.
		/// </summary>
		public PaginatedItemArrayType DeletedFromUnsoldListReturn
		{ 
			get { return ApiResponse.DeletedFromUnsoldList; }
		}
		

		#endregion

		
	}
}
