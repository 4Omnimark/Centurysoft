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
	public class GetApiAccessRulesCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetApiAccessRulesCall()
		{
			ApiRequest = new GetApiAccessRulesRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetApiAccessRulesCall(ApiContext ApiContext)
		{
			ApiRequest = new GetApiAccessRulesRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Reports how many calls your application has made and is allowed to make per hour or day.&nbsp;<b>Also for Half.com</b>.
		/// </summary>
		/// 
		public ApiAccessRuleTypeCollection GetApiAccessRules()
		{

			Execute();
			return ApiResponse.ApiAccessRule;
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
		/// Gets or sets the <see cref="GetApiAccessRulesRequestType"/> for this API call.
		/// </summary>
		public GetApiAccessRulesRequestType ApiRequest
		{ 
			get { return (GetApiAccessRulesRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetApiAccessRulesResponseType"/> for this API call.
		/// </summary>
		public GetApiAccessRulesResponseType ApiResponse
		{ 
			get { return (GetApiAccessRulesResponseType) AbstractResponse; }
		}

		
		
 		/// <summary>
		/// Gets the returned <see cref="GetApiAccessRulesResponseType.ApiAccessRule"/> of type <see cref="ApiAccessRuleTypeCollection"/>.
		/// </summary>
		public ApiAccessRuleTypeCollection ApiAccessRuleList
		{ 
			get { return ApiResponse.ApiAccessRule; }
		}
		

		#endregion

		
	}
}
