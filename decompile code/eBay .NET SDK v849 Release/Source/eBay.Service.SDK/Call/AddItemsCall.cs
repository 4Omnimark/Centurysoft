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
	public class AddItemsCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public AddItemsCall()
		{
			ApiRequest = new AddItemsRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public AddItemsCall(ApiContext ApiContext)
		{
			ApiRequest = new AddItemsRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Defines from one to five items and lists them on a specified eBay site.
		/// </summary>
		/// 
		/// <param name="AddItemRequestContainerList">
		/// Defines a single item to be listed on eBay. This container is similar to an <b>AddItem</b> request. Up to five of these containers can be included in one <b>AddItems</b> request.
		/// </param>
		///
		public AddItemResponseContainerTypeCollection AddItems(AddItemRequestContainerTypeCollection AddItemRequestContainerList)
		{
			this.AddItemRequestContainerList = AddItemRequestContainerList;

			Execute();
			return ApiResponse.AddItemResponseContainer;
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
		/// Gets or sets the <see cref="AddItemsRequestType"/> for this API call.
		/// </summary>
		public AddItemsRequestType ApiRequest
		{ 
			get { return (AddItemsRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="AddItemsResponseType"/> for this API call.
		/// </summary>
		public AddItemsResponseType ApiResponse
		{ 
			get { return (AddItemsResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="AddItemsRequestType.AddItemRequestContainer"/> of type <see cref="AddItemRequestContainerTypeCollection"/>.
		/// </summary>
		public AddItemRequestContainerTypeCollection AddItemRequestContainerList
		{ 
			get { return ApiRequest.AddItemRequestContainer; }
			set { ApiRequest.AddItemRequestContainer = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="AddItemsResponseType.AddItemResponseContainer"/> of type <see cref="AddItemResponseContainerTypeCollection"/>.
		/// </summary>
		public AddItemResponseContainerTypeCollection AddItemResponseContainerList
		{ 
			get { return ApiResponse.AddItemResponseContainer; }
		}
		

		#endregion

		
	}
}
