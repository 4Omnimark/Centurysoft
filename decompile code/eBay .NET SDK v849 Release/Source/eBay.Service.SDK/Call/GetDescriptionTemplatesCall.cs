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
	public class GetDescriptionTemplatesCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetDescriptionTemplatesCall()
		{
			ApiRequest = new GetDescriptionTemplatesRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetDescriptionTemplatesCall(ApiContext ApiContext)
		{
			ApiRequest = new GetDescriptionTemplatesRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Retrieves Theme and Layout specifications for the display of an item's description.
		/// </summary>
		/// 
		/// <param name="CategoryID">
		/// The category for which to retrieve templates. Enter any
		/// category ID, including Motors vehicle categories. This
		/// is ignored if you also send MotorVehicles.
		/// </param>
		///
		/// <param name="LastModifiedTime">
		/// If specified, only those templates modified on or after the
		/// specified date are returned. If not specified, all templates are returned.
		/// </param>
		///
		/// <param name="MotorVehicles">
		/// Indicates whether to retrieve templates for motor vehicle
		/// categories for eBay Motors (site 100). If true, templates
		/// are returned for motor vehicle categories. If false,
		/// templates are returned for non-motor vehicle categories
		/// such as Parts and Accessories. If included as an input field (whether true or false), this overrides any value provided for CategoryID.
		/// </param>
		///
		public DescriptionTemplateTypeCollection GetDescriptionTemplates(string CategoryID, DateTime LastModifiedTime, bool MotorVehicles)
		{
			this.CategoryID = CategoryID;
			this.LastModifiedTime = LastModifiedTime;
			this.MotorVehicles = MotorVehicles;

			Execute();
			return ApiResponse.DescriptionTemplate;
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
		/// Gets or sets the <see cref="GetDescriptionTemplatesRequestType"/> for this API call.
		/// </summary>
		public GetDescriptionTemplatesRequestType ApiRequest
		{ 
			get { return (GetDescriptionTemplatesRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetDescriptionTemplatesResponseType"/> for this API call.
		/// </summary>
		public GetDescriptionTemplatesResponseType ApiResponse
		{ 
			get { return (GetDescriptionTemplatesResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetDescriptionTemplatesRequestType.CategoryID"/> of type <see cref="string"/>.
		/// </summary>
		public string CategoryID
		{ 
			get { return ApiRequest.CategoryID; }
			set { ApiRequest.CategoryID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetDescriptionTemplatesRequestType.LastModifiedTime"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime LastModifiedTime
		{ 
			get { return ApiRequest.LastModifiedTime; }
			set { ApiRequest.LastModifiedTime = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetDescriptionTemplatesRequestType.MotorVehicles"/> of type <see cref="bool"/>.
		/// </summary>
		public bool MotorVehicles
		{ 
			get { return ApiRequest.MotorVehicles; }
			set { ApiRequest.MotorVehicles = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetDescriptionTemplatesResponseType.DescriptionTemplate"/> of type <see cref="DescriptionTemplateTypeCollection"/>.
		/// </summary>
		public DescriptionTemplateTypeCollection DescriptionTemplateList
		{ 
			get { return ApiResponse.DescriptionTemplate; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetDescriptionTemplatesResponseType.LayoutTotal"/> of type <see cref="int"/>.
		/// </summary>
		public int LayoutTotal
		{ 
			get { return ApiResponse.LayoutTotal; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetDescriptionTemplatesResponseType.ObsoleteLayoutID"/> of type <see cref="Int32Collection"/>.
		/// </summary>
		public Int32Collection ObsoleteLayoutIDList
		{ 
			get { return ApiResponse.ObsoleteLayoutID; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetDescriptionTemplatesResponseType.ObsoleteThemeID"/> of type <see cref="Int32Collection"/>.
		/// </summary>
		public Int32Collection ObsoleteThemeIDList
		{ 
			get { return ApiResponse.ObsoleteThemeID; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetDescriptionTemplatesResponseType.ThemeGroup"/> of type <see cref="ThemeGroupTypeCollection"/>.
		/// </summary>
		public ThemeGroupTypeCollection ThemeGroupList
		{ 
			get { return ApiResponse.ThemeGroup; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetDescriptionTemplatesResponseType.ThemeTotal"/> of type <see cref="int"/>.
		/// </summary>
		public int ThemeTotal
		{ 
			get { return ApiResponse.ThemeTotal; }
		}
		

		#endregion

		
	}
}
