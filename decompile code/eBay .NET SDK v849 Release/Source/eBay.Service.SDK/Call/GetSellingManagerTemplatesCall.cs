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
	public class GetSellingManagerTemplatesCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetSellingManagerTemplatesCall()
		{
			ApiRequest = new GetSellingManagerTemplatesRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetSellingManagerTemplatesCall(ApiContext ApiContext)
		{
			ApiRequest = new GetSellingManagerTemplatesRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Retrieves Selling Manager templates.
		/// This call is subject to change without notice; the
		/// deprecation process is inapplicable to this call.
		/// </summary>
		/// 
		/// <param name="SaleTemplateIDList">
		/// The ID of the template whose data will be returned.
		/// A SaleTemplateID is the ID of a Selling Manager template.
		/// A Selling Manager template contains the data needed to list an item.
		/// One or more template IDs can be specified, each in
		/// its own container.
		/// You can obtain a SaleTemplateID by calling GetSellingManagerInventory.
		/// </param>
		///
		public SellingManagerTemplateDetailsTypeCollection GetSellingManagerTemplates(Int64Collection SaleTemplateIDList)
		{
			this.SaleTemplateIDList = SaleTemplateIDList;

			Execute();
			return ApiResponse.SellingManagerTemplateDetailsArray;
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
		/// Gets or sets the <see cref="GetSellingManagerTemplatesRequestType"/> for this API call.
		/// </summary>
		public GetSellingManagerTemplatesRequestType ApiRequest
		{ 
			get { return (GetSellingManagerTemplatesRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetSellingManagerTemplatesResponseType"/> for this API call.
		/// </summary>
		public GetSellingManagerTemplatesResponseType ApiResponse
		{ 
			get { return (GetSellingManagerTemplatesResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetSellingManagerTemplatesRequestType.SaleTemplateID"/> of type <see cref="Int64Collection"/>.
		/// </summary>
		public Int64Collection SaleTemplateIDList
		{ 
			get { return ApiRequest.SaleTemplateID; }
			set { ApiRequest.SaleTemplateID = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetSellingManagerTemplatesResponseType.SellingManagerTemplateDetailsArray"/> of type <see cref="SellingManagerTemplateDetailsTypeCollection"/>.
		/// </summary>
		public SellingManagerTemplateDetailsTypeCollection SellingManagerTemplateDetailsList
		{ 
			get { return ApiResponse.SellingManagerTemplateDetailsArray; }
		}
		

		#endregion

		
	}
}
