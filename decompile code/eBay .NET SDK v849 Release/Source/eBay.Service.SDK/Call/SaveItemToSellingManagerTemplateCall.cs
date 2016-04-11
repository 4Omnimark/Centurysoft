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
	public class SaveItemToSellingManagerTemplateCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public SaveItemToSellingManagerTemplateCall()
		{
			ApiRequest = new SaveItemToSellingManagerTemplateRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public SaveItemToSellingManagerTemplateCall(ApiContext ApiContext)
		{
			ApiRequest = new SaveItemToSellingManagerTemplateRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Creates a Selling Manager listing template that is similar to an item.
		/// This call is subject to change without notice; the
		/// deprecation process is inapplicable to this call.
		/// </summary>
		/// 
		/// <param name="ItemID">
		/// ItemID which should be created as a template and saved to inventory.
		/// </param>
		///
		/// <param name="ProductID">
		/// Associates the new template with a product.
		/// </param>
		///
		/// <param name="TemplateName">
		/// Name associated with the template. If no name is submitted, the template will be named automatically.
		/// </param>
		///
		public long SaveItemToSellingManagerTemplate(string ItemID, long ProductID, string TemplateName)
		{
			this.ItemID = ItemID;
			this.ProductID = ProductID;
			this.TemplateName = TemplateName;

			Execute();
			return ApiResponse.TemplateID;
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
		/// Gets or sets the <see cref="SaveItemToSellingManagerTemplateRequestType"/> for this API call.
		/// </summary>
		public SaveItemToSellingManagerTemplateRequestType ApiRequest
		{ 
			get { return (SaveItemToSellingManagerTemplateRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="SaveItemToSellingManagerTemplateResponseType"/> for this API call.
		/// </summary>
		public SaveItemToSellingManagerTemplateResponseType ApiResponse
		{ 
			get { return (SaveItemToSellingManagerTemplateResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="SaveItemToSellingManagerTemplateRequestType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemID
		{ 
			get { return ApiRequest.ItemID; }
			set { ApiRequest.ItemID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SaveItemToSellingManagerTemplateRequestType.ProductID"/> of type <see cref="long"/>.
		/// </summary>
		public long ProductID
		{ 
			get { return ApiRequest.ProductID; }
			set { ApiRequest.ProductID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="SaveItemToSellingManagerTemplateRequestType.TemplateName"/> of type <see cref="string"/>.
		/// </summary>
		public string TemplateName
		{ 
			get { return ApiRequest.TemplateName; }
			set { ApiRequest.TemplateName = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="SaveItemToSellingManagerTemplateResponseType.TemplateID"/> of type <see cref="long"/>.
		/// </summary>
		public long TemplateID
		{ 
			get { return ApiResponse.TemplateID; }
		}
		

		#endregion

		
	}
}
