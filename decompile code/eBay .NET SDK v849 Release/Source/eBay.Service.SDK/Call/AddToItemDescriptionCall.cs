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
	public class AddToItemDescriptionCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public AddToItemDescriptionCall()
		{
			ApiRequest = new AddToItemDescriptionRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public AddToItemDescriptionCall(ApiContext ApiContext)
		{
			ApiRequest = new AddToItemDescriptionRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Appends a horizontal rule, then a message about what time the
		/// addition was made by the seller, and then the seller-specified text.
		/// </summary>
		/// 
		/// <param name="ItemID">
		/// Unique item ID that identifies the target item listing, the description
		/// of which is appended with the text specified in Description.
		/// </param>
		///
		/// <param name="DescriptionToAppend">
		/// Specifies the text to append to the end of the listing's description.
		/// Text appended to a listing's description must abide by the rules
		/// applicable to this data (such as no JavaScript) as is the case when
		/// first listing the item.
		/// </param>
		///
		public void AddToItemDescription(string ItemID, string DescriptionToAppend)
		{
			this.ItemID = ItemID;
			this.DescriptionToAppend = DescriptionToAppend;

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
		/// Gets or sets the <see cref="AddToItemDescriptionRequestType"/> for this API call.
		/// </summary>
		public AddToItemDescriptionRequestType ApiRequest
		{ 
			get { return (AddToItemDescriptionRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="AddToItemDescriptionResponseType"/> for this API call.
		/// </summary>
		public AddToItemDescriptionResponseType ApiResponse
		{ 
			get { return (AddToItemDescriptionResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="AddToItemDescriptionRequestType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemID
		{ 
			get { return ApiRequest.ItemID; }
			set { ApiRequest.ItemID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="AddToItemDescriptionRequestType.Description"/> of type <see cref="string"/>.
		/// </summary>
		public string DescriptionToAppend
		{ 
			get { return ApiRequest.Description; }
			set { ApiRequest.Description = value; }
		}
		
		

		#endregion

		
	}
}
