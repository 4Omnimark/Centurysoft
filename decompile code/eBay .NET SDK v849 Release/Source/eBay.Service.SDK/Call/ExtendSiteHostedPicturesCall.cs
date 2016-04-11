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
	public class ExtendSiteHostedPicturesCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public ExtendSiteHostedPicturesCall()
		{
			ApiRequest = new ExtendSiteHostedPicturesRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public ExtendSiteHostedPicturesCall(ApiContext ApiContext)
		{
			ApiRequest = new ExtendSiteHostedPicturesRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Gives approved sellers the ability to extend the default and
		/// ongoing lifetime of pictures uploaded to eBay.
		/// </summary>
		/// 
		/// <param name="PictureURLList">
		/// The URL of the image hosted by eBay Picture Services.
		/// </param>
		///
		/// <param name="ExtensionInDays">
		/// The number of days by which to extend the expiration date for the
		/// specified image.
		/// </param>
		///
		public StringCollection ExtendSiteHostedPictures(StringCollection PictureURLList, int ExtensionInDays)
		{
			this.PictureURLList = PictureURLList;
			this.ExtensionInDays = ExtensionInDays;

			Execute();
			return ApiResponse.PictureURL;
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
		/// Gets or sets the <see cref="ExtendSiteHostedPicturesRequestType"/> for this API call.
		/// </summary>
		public ExtendSiteHostedPicturesRequestType ApiRequest
		{ 
			get { return (ExtendSiteHostedPicturesRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="ExtendSiteHostedPicturesResponseType"/> for this API call.
		/// </summary>
		public ExtendSiteHostedPicturesResponseType ApiResponse
		{ 
			get { return (ExtendSiteHostedPicturesResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="ExtendSiteHostedPicturesRequestType.PictureURL"/> of type <see cref="StringCollection"/>.
		/// </summary>
		public StringCollection PictureURLList
		{ 
			get { return ApiRequest.PictureURL; }
			set { ApiRequest.PictureURL = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="ExtendSiteHostedPicturesRequestType.ExtensionInDays"/> of type <see cref="int"/>.
		/// </summary>
		public int ExtensionInDays
		{ 
			get { return ApiRequest.ExtensionInDays; }
			set { ApiRequest.ExtensionInDays = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="ExtendSiteHostedPicturesResponseType.PictureURL"/> of type <see cref="StringCollection"/>.
		/// </summary>
		public StringCollection PictureURLListReturn
		{ 
			get { return ApiResponse.PictureURL; }
		}
		

		#endregion

		
	}
}
