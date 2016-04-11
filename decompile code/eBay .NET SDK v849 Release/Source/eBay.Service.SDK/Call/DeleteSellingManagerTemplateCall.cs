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
	public class DeleteSellingManagerTemplateCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public DeleteSellingManagerTemplateCall()
		{
			ApiRequest = new DeleteSellingManagerTemplateRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public DeleteSellingManagerTemplateCall(ApiContext ApiContext)
		{
			ApiRequest = new DeleteSellingManagerTemplateRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Deletes a Selling Manager template.
		/// This call is subject to change without notice; the
		/// deprecation process is inapplicable to this call.
		/// </summary>
		/// 
		/// <param name="SaleTemplateID">
		/// ID of the template to be deleted.
		/// You can obtain a SaleTemplateID by calling GetSellingManagerInventory.
		/// </param>
		///
		public string DeleteSellingManagerTemplate(long SaleTemplateID)
		{
			this.SaleTemplateID = SaleTemplateID;

			Execute();
			return ApiResponse.DeletedSaleTemplateID;
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
		/// Gets or sets the <see cref="DeleteSellingManagerTemplateRequestType"/> for this API call.
		/// </summary>
		public DeleteSellingManagerTemplateRequestType ApiRequest
		{ 
			get { return (DeleteSellingManagerTemplateRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="DeleteSellingManagerTemplateResponseType"/> for this API call.
		/// </summary>
		public DeleteSellingManagerTemplateResponseType ApiResponse
		{ 
			get { return (DeleteSellingManagerTemplateResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="DeleteSellingManagerTemplateRequestType.SaleTemplateID"/> of type <see cref="long"/>.
		/// </summary>
		public long SaleTemplateID
		{ 
			get { return ApiRequest.SaleTemplateID; }
			set { ApiRequest.SaleTemplateID = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="DeleteSellingManagerTemplateResponseType.DeletedSaleTemplateID"/> of type <see cref="string"/>.
		/// </summary>
		public string DeletedSaleTemplateID
		{ 
			get { return ApiResponse.DeletedSaleTemplateID; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="DeleteSellingManagerTemplateResponseType.DeletedSaleTemplateName"/> of type <see cref="string"/>.
		/// </summary>
		public string DeletedSaleTemplateName
		{ 
			get { return ApiResponse.DeletedSaleTemplateName; }
		}
		

		#endregion

		
	}
}
