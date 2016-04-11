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
	public class VeROReportItemsCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public VeROReportItemsCall()
		{
			ApiRequest = new VeROReportItemsRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public VeROReportItemsCall(ApiContext ApiContext)
		{
			ApiRequest = new VeROReportItemsRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Reports items that allegedly infringe your copyright, trademark, or other
		/// intellectual property rights. You can report one or more items at a time with this
		/// call. You must be a member of the Verified Rights Owner (VeRO) Program to use this
		/// call.
		/// </summary>
		/// 
		/// <param name="RightsOwnerID">
		/// User ID of the VeRO member reporting the items.
		/// </param>
		///
		/// <param name="ReportItemList">
		/// Container (packet) for items being reported. You can report the same item 
		/// more than once in a packet if a different reason code is used each time.
		/// </param>
		///
		public long VeROReportItems(string RightsOwnerID, VeROReportItemTypeCollection ReportItemList)
		{
			this.RightsOwnerID = RightsOwnerID;
			this.ReportItemList = ReportItemList;

			Execute();
			return ApiResponse.VeROReportPacketID;
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
		/// Gets or sets the <see cref="VeROReportItemsRequestType"/> for this API call.
		/// </summary>
		public VeROReportItemsRequestType ApiRequest
		{ 
			get { return (VeROReportItemsRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="VeROReportItemsResponseType"/> for this API call.
		/// </summary>
		public VeROReportItemsResponseType ApiResponse
		{ 
			get { return (VeROReportItemsResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="VeROReportItemsRequestType.RightsOwnerID"/> of type <see cref="string"/>.
		/// </summary>
		public string RightsOwnerID
		{ 
			get { return ApiRequest.RightsOwnerID; }
			set { ApiRequest.RightsOwnerID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="VeROReportItemsRequestType.ReportItems"/> of type <see cref="VeROReportItemTypeCollection"/>.
		/// </summary>
		public VeROReportItemTypeCollection ReportItemList
		{ 
			get { return ApiRequest.ReportItems; }
			set { ApiRequest.ReportItems = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="VeROReportItemsResponseType.VeROReportPacketID"/> of type <see cref="long"/>.
		/// </summary>
		public long VeROReportPacketID
		{ 
			get { return ApiResponse.VeROReportPacketID; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="VeROReportItemsResponseType.VeROReportPacketStatus"/> of type <see cref="VeROReportPacketStatusCodeType"/>.
		/// </summary>
		public VeROReportPacketStatusCodeType VeROReportPacketStatus
		{ 
			get { return ApiResponse.VeROReportPacketStatus; }
		}
		

		#endregion

		
	}
}
