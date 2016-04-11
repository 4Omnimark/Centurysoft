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
	public class ReviseInventoryStatusCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public ReviseInventoryStatusCall()
		{
			ApiRequest = new ReviseInventoryStatusRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public ReviseInventoryStatusCall(ApiContext ApiContext)
		{
			ApiRequest = new ReviseInventoryStatusRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Enables a seller to change the price and quantity of a currently-
		/// active, fixed-price listing. Using ReviseInventoryStatus to modify
		/// data qualifies as revising the listing.
		/// 
		/// 
		/// Inputs are the item IDs or SKUs of the listings being revised,
		/// and the price and quantity that are
		/// being changed for each revision. Only applicable to fixed-price listings.
		/// 
		/// Changing the price or quantity of a currently-
		/// active, fixed-price listing does not reset the Best Match performance score.
		/// For Best Match information related to multi-variation listings, see the Best
		/// Match information at the following topic:
		/// <a href="http://pages.ebay.com/sell/variation/">Multi-quantity Fixed Price
		/// listings with variations</a>.
		/// 
		/// As with all listing calls, the site you specify in the request URL
		/// (for SOAP) or the X-EBAY-API-SITEID HTTP header (for XML)
		/// should match the original listing's <b>Item.Site</b> value.
		/// In particular, this is a best practice when you submit new and
		/// revised listings.
		/// 
		/// <b>For Large Merchant Services users:</b> When you use ReviseInventoryStatus within a Merchant Data file,
		/// it must be enclosed within two BulkDataExchangeRequest tags.
		/// After release 637, a namespace is returned in the BulkDataExchangeResponseType
		/// (top level) of the response. The BulkDataExchange tags are not shown in the call input/output descriptions.
		/// </summary>
		/// 
		/// <param name="InventoryStatuList">
		/// Contains the updated quantity and/or price of a listing
		/// being revised. You should not modify the same listing twice
		/// (by using a duplicate ItemID or SKU) in the same request;
		/// otherwise, you may get an error or unexpected results.
		/// (For example, you should not use one InventoryStatus node to
		/// update the quantity and another InventoryStatus node to update
		/// the price of the same item.) You can pass up to 4 InventoryStatus nodes in a single request.
		/// </param>
		///
		public InventoryStatusTypeCollection ReviseInventoryStatus(InventoryStatusTypeCollection InventoryStatuList)
		{
			this.InventoryStatuList = InventoryStatuList;

			Execute();
			return ApiResponse.InventoryStatus;
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
		/// Gets or sets the <see cref="ReviseInventoryStatusRequestType"/> for this API call.
		/// </summary>
		public ReviseInventoryStatusRequestType ApiRequest
		{ 
			get { return (ReviseInventoryStatusRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="ReviseInventoryStatusResponseType"/> for this API call.
		/// </summary>
		public ReviseInventoryStatusResponseType ApiResponse
		{ 
			get { return (ReviseInventoryStatusResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseInventoryStatusRequestType.InventoryStatus"/> of type <see cref="InventoryStatusTypeCollection"/>.
		/// </summary>
		public InventoryStatusTypeCollection InventoryStatuList
		{ 
			get { return ApiRequest.InventoryStatus; }
			set { ApiRequest.InventoryStatus = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="ReviseInventoryStatusResponseType.InventoryStatus"/> of type <see cref="InventoryStatusTypeCollection"/>.
		/// </summary>
		public InventoryStatusTypeCollection InventoryStatuListReturn
		{ 
			get { return ApiResponse.InventoryStatus; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="ReviseInventoryStatusResponseType.Fees"/> of type <see cref="InventoryFeesTypeCollection"/>.
		/// </summary>
		public InventoryFeesTypeCollection FeeList
		{ 
			get { return ApiResponse.Fees; }
		}
		

		#endregion

		
	}
}
