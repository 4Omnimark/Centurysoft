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
	public class SetTaxTableCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public SetTaxTableCall()
		{
			ApiRequest = new SetTaxTableRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public SetTaxTableCall(ApiContext ApiContext)
		{
			ApiRequest = new SetTaxTableRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Sets the tax table for a seller on a given site.
		/// </summary>
		/// 
		/// <param name="TaxTableList">
		/// A container of tax jurisdiction information unique to a user/site combination.
		/// </param>
		///
		public void SetTaxTable(TaxJurisdictionTypeCollection TaxTableList)
		{
			this.TaxTableList = TaxTableList;

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
		/// Gets or sets the <see cref="SetTaxTableRequestType"/> for this API call.
		/// </summary>
		public SetTaxTableRequestType ApiRequest
		{ 
			get { return (SetTaxTableRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="SetTaxTableResponseType"/> for this API call.
		/// </summary>
		public SetTaxTableResponseType ApiResponse
		{ 
			get { return (SetTaxTableResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="SetTaxTableRequestType.TaxTable"/> of type <see cref="TaxJurisdictionTypeCollection"/>.
		/// </summary>
		public TaxJurisdictionTypeCollection TaxTableList
		{ 
			get { return ApiRequest.TaxTable; }
			set { ApiRequest.TaxTable = value; }
		}
		
		

		#endregion

		
	}
}
