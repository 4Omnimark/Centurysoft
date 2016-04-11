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
	public class SetStoreCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public SetStoreCall()
		{
			ApiRequest = new SetStoreRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public SetStoreCall(ApiContext ApiContext)
		{
			ApiRequest = new SetStoreRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Sets the configuration of the eBay store owned by the caller.
		/// </summary>
		/// 
		/// <param name="Store">
		/// Specifies the Store configuration that is being set for the user.
		/// </param>
		///
		public void SetStore(StoreType Store)
		{
			this.Store = Store;

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
		/// Gets or sets the <see cref="SetStoreRequestType"/> for this API call.
		/// </summary>
		public SetStoreRequestType ApiRequest
		{ 
			get { return (SetStoreRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="SetStoreResponseType"/> for this API call.
		/// </summary>
		public SetStoreResponseType ApiResponse
		{ 
			get { return (SetStoreResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="SetStoreRequestType.Store"/> of type <see cref="StoreType"/>.
		/// </summary>
		public StoreType Store
		{ 
			get { return ApiRequest.Store; }
			set { ApiRequest.Store = value; }
		}
		
		

		#endregion

		
	}
}
