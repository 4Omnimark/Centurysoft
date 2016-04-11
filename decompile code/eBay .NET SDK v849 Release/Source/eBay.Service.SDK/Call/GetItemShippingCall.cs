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
	public class GetItemShippingCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetItemShippingCall()
		{
			ApiRequest = new GetItemShippingRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetItemShippingCall(ApiContext ApiContext)
		{
			ApiRequest = new GetItemShippingRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Returns shipping cost estimates for an item for every calculated shipping service
		/// that the seller has offered with the listing. This is analogous to the Shipping
		/// Calculator seen in both the buyer and seller web pages.
		/// </summary>
		/// 
		/// <param name="ItemID">
		/// The item ID that uniquely identifies the item listing for which
		/// to retrieve the data. Required input.
		/// </param>
		///
		/// <param name="QuantitySold">
		/// Number of items sold to a single buyer and to be shipped together.
		/// </param>
		///
		/// <param name="DestinationPostalCode">
		/// Destination country postal code (or zipcode, for US). Ignored if no
		/// country code is provided. Optional tag for some countries. More likely to
		/// be required for large countries.
		/// </param>
		///
		/// <param name="DestinationCountryCode">
		/// Destination country code. If DestinationCountryCode is US,
		/// a postal code is required and it represents the US zip code.
		/// </param>
		///
		public ShippingDetailsType GetItemShipping(string ItemID, int QuantitySold, string DestinationPostalCode, CountryCodeType DestinationCountryCode)
		{
			this.ItemID = ItemID;
			this.QuantitySold = QuantitySold;
			this.DestinationPostalCode = DestinationPostalCode;
			this.DestinationCountryCode = DestinationCountryCode;

			Execute();
			return ApiResponse.ShippingDetails;
		}


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public ShippingDetailsType GetItemShipping(string ItemID, string DestinationPostalCode)
		{
			this.ItemID = ItemID;
			this.DestinationPostalCode = DestinationPostalCode;
			Execute();
			return ShippingDetails;
		}
		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public ShippingDetailsType GetItemShipping(string ItemID, string DestinationPostalCode, CountryCodeType DestinationCountryCode)
		{
			this.DestinationCountryCode = DestinationCountryCode;
			this.ItemID = ItemID;
			this.DestinationPostalCode = DestinationPostalCode;
			Execute();
			return ShippingDetails;
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
		/// Gets or sets the <see cref="GetItemShippingRequestType"/> for this API call.
		/// </summary>
		public GetItemShippingRequestType ApiRequest
		{ 
			get { return (GetItemShippingRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetItemShippingResponseType"/> for this API call.
		/// </summary>
		public GetItemShippingResponseType ApiResponse
		{ 
			get { return (GetItemShippingResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetItemShippingRequestType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemID
		{ 
			get { return ApiRequest.ItemID; }
			set { ApiRequest.ItemID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetItemShippingRequestType.QuantitySold"/> of type <see cref="int"/>.
		/// </summary>
		public int QuantitySold
		{ 
			get { return ApiRequest.QuantitySold; }
			set { ApiRequest.QuantitySold = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetItemShippingRequestType.DestinationPostalCode"/> of type <see cref="string"/>.
		/// </summary>
		public string DestinationPostalCode
		{ 
			get { return ApiRequest.DestinationPostalCode; }
			set { ApiRequest.DestinationPostalCode = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetItemShippingRequestType.DestinationCountryCode"/> of type <see cref="CountryCodeType"/>.
		/// </summary>
		public CountryCodeType DestinationCountryCode
		{ 
			get { return ApiRequest.DestinationCountryCode; }
			set { ApiRequest.DestinationCountryCode = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetItemShippingResponseType.ShippingDetails"/> of type <see cref="ShippingDetailsType"/>.
		/// </summary>
		public ShippingDetailsType ShippingDetails
		{ 
			get { return ApiResponse.ShippingDetails; }
		}
		

		#endregion

		
	}
}
