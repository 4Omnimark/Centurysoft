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
	public class VerifyRelistItemCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public VerifyRelistItemCall()
		{
			ApiRequest = new VerifyRelistItemRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public VerifyRelistItemCall(ApiContext ApiContext)
		{
			ApiRequest = new VerifyRelistItemRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Checks the arguments that you plan to use for a RelistItem call, looking for errors
		/// and listing fees, without actually relisting the item.
		/// </summary>
		/// 
		/// <param name="Item">
		/// Child elements hold the values for item properties that change for the
		/// item re-list. Item is a required input. At a minimum, the Item.ItemID
		/// property must be set to the ID of the listing being re-listed (a
		/// listing that ended in the past 90 days). By default, the new listing's
		/// Item object properties are the same as those of the original (ended)
		/// listing. By setting a new value in the Item object, the new listing
		/// uses the new value rather than the corresponding value from the old
		/// listing.
		/// </param>
		///
		/// <param name="DeletedFieldList">
		/// Specifies the name of the field to remove from a listing.
		/// See the Developer's Guide for rules on removing values when relisting
		/// items. DeletedField accepts the same values as
		/// DeletedField in RelistItem.
		/// DeletedField is case sensitive. The request can contain zero, one, or many
		/// instances of DeletedField.
		/// </param>
		///
		public string VerifyRelistItem(ItemType Item, StringCollection DeletedFieldList)
		{
			this.Item = Item;
			this.DeletedFieldList = DeletedFieldList;

			Execute();
			return ApiResponse.ItemID;
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
		/// Gets or sets the <see cref="VerifyRelistItemRequestType"/> for this API call.
		/// </summary>
		public VerifyRelistItemRequestType ApiRequest
		{ 
			get { return (VerifyRelistItemRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="VerifyRelistItemResponseType"/> for this API call.
		/// </summary>
		public VerifyRelistItemResponseType ApiResponse
		{ 
			get { return (VerifyRelistItemResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="VerifyRelistItemRequestType.Item"/> of type <see cref="ItemType"/>.
		/// </summary>
		public ItemType Item
		{ 
			get { return ApiRequest.Item; }
			set { ApiRequest.Item = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="VerifyRelistItemRequestType.DeletedField"/> of type <see cref="StringCollection"/>.
		/// </summary>
		public StringCollection DeletedFieldList
		{ 
			get { return ApiRequest.DeletedField; }
			set { ApiRequest.DeletedField = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="VerifyRelistItemResponseType.ItemID"/> of type <see cref="string"/>.
		/// </summary>
		public string ItemID
		{ 
			get { return ApiResponse.ItemID; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="VerifyRelistItemResponseType.Fees"/> of type <see cref="FeeTypeCollection"/>.
		/// </summary>
		public FeeTypeCollection FeeList
		{ 
			get { return ApiResponse.Fees; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="VerifyRelistItemResponseType.StartTime"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime StartTime
		{ 
			get { return ApiResponse.StartTime; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="VerifyRelistItemResponseType.EndTime"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime EndTime
		{ 
			get { return ApiResponse.EndTime; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="VerifyRelistItemResponseType.DiscountReason"/> of type <see cref="DiscountReasonCodeTypeCollection"/>.
		/// </summary>
		public DiscountReasonCodeTypeCollection DiscountReasonList
		{ 
			get { return ApiResponse.DiscountReason; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="VerifyRelistItemResponseType.ProductSuggestions"/> of type <see cref="ProductSuggestionsType"/>.
		/// </summary>
		public ProductSuggestionsType ProductSuggestions
		{ 
			get { return ApiResponse.ProductSuggestions; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="VerifyRelistItemResponseType.ListingRecommendations"/> of type <see cref="ListingRecommendationsType"/>.
		/// </summary>
		public ListingRecommendationsType ListingRecommendations
		{ 
			get { return ApiResponse.ListingRecommendations; }
		}
		

		#endregion

		
	}
}
