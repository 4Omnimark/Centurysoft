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
	public class ReviseSellingManagerSaleRecordCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public ReviseSellingManagerSaleRecordCall()
		{
			ApiRequest = new ReviseSellingManagerSaleRecordRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public ReviseSellingManagerSaleRecordCall(ApiContext ApiContext)
		{
			ApiRequest = new ReviseSellingManagerSaleRecordRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Request type containing the input fields for the <b>ReviseSellingManagerSaleRecord</b>
		/// call. The standard Trading API deprecation process is not applicable to this
		/// call.
		/// </summary>
		/// 
		/// <param name="ItemID">
		/// Unique identifier for an eBay item listing. A listing can have multiple
		/// order line items (transactions), but only one <b>ItemID</b>. An <b>ItemID</b> can be
		/// paired up with a corresponding <b>TransactionID</b> and used as an input filter
		/// for <b>ReviseSellingManagerSaleRecord</b>. The <b>ItemID</b>/<b>TransactionID</b> pair
		/// corresponds to a Selling Manager <b>SaleRecordID</b>, which can be retrieved
		/// with the <b>GetSellingManagerSaleRecord</b> call. 
		/// 
		/// Unless an <b>OrderLineItemID</b> is used to identify a single line item order,
		/// or the <b>OrderID</b> is used to identify a single or multiple line item
		/// (Combined Payment) order, the <b>ItemID</b>/<b>TransactionID</b> pair must be
		/// specified. For a multiple line item (Combined Payment) order, <b>OrderID</b>
		/// should be used. If <b>OrderID</b> or <b>OrderLineItemID</b> are specified, the
		/// <b>ItemID</b>/<b>TransactionID</b> pair is ignored if present in the same request.
		/// </param>
		///
		/// <param name="TransactionID">
		/// Unique identifier for an eBay order line item (transaction). An order
		/// line item is created once there is a commitment from a buyer to purchase
		/// an item. Since an auction listing can only have one order line item
		/// during the duration of the listing, the <b>TransactionID</b> for
		/// auction listings is always 0. Along with its corresponding <b>ItemID</b>, a
		/// <b>TransactionID</b> is used and referenced during an order checkout flow and
		/// after checkout has been completed. The <b>ItemID</b>/<b>TransactionID</b> pair can be
		/// used as an input filter for <b>ReviseSellingManagerSaleRecord</b>. The
		/// <b>ItemID</b>/<b>TransactionID</b> pair corresponds to a Selling Manager <b>SaleRecordID</b>,
		/// which can be retrieved with the <b>GetSellingManagerSaleRecord</b> call.
		/// 
		/// Unless an <b>OrderLineItemID</b> is used to identify a single line item order,
		/// or the <b>OrderID</b> is used to identify a single or multiple line item
		/// (Combined Payment) order, the <b>ItemID</b>/<b>TransactionID</b> pair must be
		/// specified. For a multiple line item (Combined Payment) order, <b>OrderID</b>
		/// must be used. If <b>OrderID</b> or <b>OrderLineItemID</b> are specified, the
		/// <b>ItemID</b>/<b>TransactionID</b> pair is ignored if present in the same request.
		/// </param>
		///
		/// <param name="OrderID">
		/// A unique identifier that identifies a single line item or multiple line
		/// item (Combined Payment) order. 
		///  
		/// For a single line item order, the <b>OrderID</b> value is identical to the
		/// <b>OrderLineItemID</b> value that is generated upon creation of the order line
		/// item. For a Combined Payment order, the <b>OrderID</b> value is created by eBay
		/// when the buyer or seller (sharing multiple, common order line items)
		/// combines multiple order line items into a Combined Payment order through
		/// the eBay site. A Combined Payment order can also be created by the
		/// seller through the <b>AddOrder</b> call. The <b>OrderID</b> can be used as an input
		/// filter for <b>ReviseSellingManagerSaleRecord</b>. The <b>OrderID</b>
		/// is linked to a Selling Manager <b>SaleRecordID</b>, and can be retrieved
		/// with the <b>GetSellingManagerSaleRecord</b> call.
		/// 
		/// <b>OrderID</b> overrides an <b>OrderLineItemID</b> or <b>ItemID</b>/<b>TransactionID</b> pair if
		/// these fields are also specified in the same request.
		/// </param>
		///
		/// <param name="SellingManagerSoldOrder">
		/// Container consisting of order costs, shipping details, order status, and
		/// other information. The changes made under this container will update the
		/// order in Selling Manager.
		/// </param>
		///
		/// <param name="OrderLineItemID">
		/// <b>OrderLineItemID</b> is a unique identifier for an eBay order line item and
		/// is based upon the concatenation of <b>ItemID</b> and <b>TransactionID</b>, with a
		/// hyphen in between these two IDs. For a single line item order, the
		/// <b>OrderLineItemID</b> value can be passed into the <b>OrderID</b> field to revise the
		/// order in Selling Manager. 
		/// 
		/// Unless an <b>ItemID</b>/<b>TransactionID</b> pair is used to identify a single line
		/// item order, or the <b>OrderID</b> is used to identify a single or multiple line
		/// item (Combined Payment) order, the <b>OrderLineItemID</b> must be specified.
		/// For a multiple line item (Combined Payment) order, <b>OrderID</b> should be
		/// used. If <b>OrderLineItemID</b> is specified, the <b>ItemID</b>/<b>TransactionID</b> pair are
		/// ignored if present in the same request.
		/// </param>
		///
		public void ReviseSellingManagerSaleRecord(string ItemID, string TransactionID, string OrderID, SellingManagerSoldOrderType SellingManagerSoldOrder, string OrderLineItemID)
		{
			this.ItemID = ItemID;
			this.TransactionID = TransactionID;
			this.OrderID = OrderID;
			this.SellingManagerSoldOrder = SellingManagerSoldOrder;
			this.OrderLineItemID = OrderLineItemID;

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
		/// Gets or sets the <see cref="ReviseSellingManagerSaleRecordRequestType"/> for this API call.
		/// </summary>
		public ReviseSellingManagerSaleRecordRequestType ApiRequest
		{ 
			get { return (ReviseSellingManagerSaleRecordRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="ReviseSellingManagerSaleRecordResponseType"/> for this API call.
		/// </summary>
		public ReviseSellingManagerSaleRecordResponseType ApiResponse
		{ 
			get { return (ReviseSellingManagerSaleRecordResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseSellingManagerSaleRecordRequestType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemID
		{ 
			get { return ApiRequest.ItemID; }
			set { ApiRequest.ItemID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseSellingManagerSaleRecordRequestType.TransactionID"/> of type <see cref="string"/>.
		/// </summary>
		public string TransactionID
		{ 
			get { return ApiRequest.TransactionID; }
			set { ApiRequest.TransactionID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseSellingManagerSaleRecordRequestType.OrderID"/> of type <see cref="string"/>.
		/// </summary>
		public string OrderID
		{ 
			get { return ApiRequest.OrderID; }
			set { ApiRequest.OrderID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseSellingManagerSaleRecordRequestType.SellingManagerSoldOrder"/> of type <see cref="SellingManagerSoldOrderType"/>.
		/// </summary>
		public SellingManagerSoldOrderType SellingManagerSoldOrder
		{ 
			get { return ApiRequest.SellingManagerSoldOrder; }
			set { ApiRequest.SellingManagerSoldOrder = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ReviseSellingManagerSaleRecordRequestType.OrderLineItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string OrderLineItemID
		{ 
			get { return ApiRequest.OrderLineItemID; }
			set { ApiRequest.OrderLineItemID = value; }
		}
		
		

		#endregion

		
	}
}
