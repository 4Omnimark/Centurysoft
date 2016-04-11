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
	public class GetCharitiesCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetCharitiesCall()
		{
			ApiRequest = new GetCharitiesRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetCharitiesCall(ApiContext ApiContext)
		{
			ApiRequest = new GetCharitiesRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Searches for nonprofit charity organizations that meet the
		/// criteria specified in the request.
		/// It is recommended that you use at least one input filter, because this call
		/// no longer returns all charities when made without filters.
		/// </summary>
		/// 
		/// <param name="CharityID">
		/// A unique identification number assigned by eBay to registered nonprofit
		/// charity organizations.
		/// </param>
		///
		/// <param name="CharityName">
		/// A name assigned to a specified nonprofit
		/// organization. Accepts full charity nonprofit name
		/// or partial name as input. For example, enter a
		/// CharityName of "heart" (case-insensitive) to
		/// return all charity nonprofits that start with
		/// "heart." Use with a MatchType value of "Contains"
		/// to return all charity nonprofits that contain the
		/// string "heart."
		/// </param>
		///
		/// <param name="Query">
		/// Accepts a case-insensitive string used to
		/// find a nonprofit charity organization. Default
		/// behavior is to search the CharityName field. Use
		/// with an IncludeDescription value of true to
		/// include the Mission field in the search.
		/// </param>
		///
		/// <param name="CharityRegion">
		/// Region that the nonprofit charity organization is associated
		/// with. A specific nonprofit charity organization may be associated
		/// with only one region. Meaning of input values differs depending on
		/// the site. See GetCharities in the API Developer's Guide for the meaning
		/// of each input/output value. CharityRegion input value must be
		/// valid for that SiteID.
		/// </param>
		///
		/// <param name="CharityDomain">
		/// Domain (mission area) that a nonprofit charity organization
		/// belongs to. Nonprofit charity organizations may belong to multiple
		/// mission areas. Meaning of input values differs depending on the
		/// site. 
		/// </param>
		///
		/// <param name="IncludeDescription">
		/// Used with Query to search for charity nonprofit
		/// organizations. A value of true will search the Mission field as
		/// well as the CharityName field for a string specified in Query.
		/// </param>
		///
		/// <param name="MatchType">
		/// Indicates the type of string matching to use when a value is submitted in
		/// CharityName. If no value is specified, default behavior is "StartsWith."
		/// Does not apply to Query.
		/// </param>
		///
		/// <param name="Featured">
		/// Used to decide if the search is only for featured charities.
		/// A value of true will search for only featured charities.
		/// </param>
		///
		/// <param name="CampaignID">
		/// Reserved for future use.
		/// </param>
		///
		public CharityInfoTypeCollection GetCharities(string CharityID, string CharityName, string Query, int CharityRegion, int CharityDomain, bool IncludeDescription, StringMatchCodeType MatchType, bool Featured, long CampaignID)
		{
			this.CharityID = CharityID;
			this.CharityName = CharityName;
			this.Query = Query;
			this.CharityRegion = CharityRegion;
			this.CharityDomain = CharityDomain;
			this.IncludeDescription = IncludeDescription;
			this.MatchType = MatchType;
			this.Featured = Featured;
			this.CampaignID = CampaignID;

			Execute();
			return ApiResponse.Charity;
		}


		/// <summary>
		/// to support the call with out parameters.
		/// </summary>
		public CharityInfoTypeCollection GetCharities()
		{
			Execute();
			return ApiResponse.Charity;
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
		/// Gets or sets the <see cref="GetCharitiesRequestType"/> for this API call.
		/// </summary>
		public GetCharitiesRequestType ApiRequest
		{ 
			get { return (GetCharitiesRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetCharitiesResponseType"/> for this API call.
		/// </summary>
		public GetCharitiesResponseType ApiResponse
		{ 
			get { return (GetCharitiesResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetCharitiesRequestType.CharityID"/> of type <see cref="string"/>.
		/// </summary>
		public string CharityID
		{ 
			get { return ApiRequest.CharityID; }
			set { ApiRequest.CharityID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetCharitiesRequestType.CharityName"/> of type <see cref="string"/>.
		/// </summary>
		public string CharityName
		{ 
			get { return ApiRequest.CharityName; }
			set { ApiRequest.CharityName = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetCharitiesRequestType.Query"/> of type <see cref="string"/>.
		/// </summary>
		public string Query
		{ 
			get { return ApiRequest.Query; }
			set { ApiRequest.Query = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetCharitiesRequestType.CharityRegion"/> of type <see cref="int"/>.
		/// </summary>
		public int CharityRegion
		{ 
			get { return ApiRequest.CharityRegion; }
			set { ApiRequest.CharityRegion = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetCharitiesRequestType.CharityDomain"/> of type <see cref="int"/>.
		/// </summary>
		public int CharityDomain
		{ 
			get { return ApiRequest.CharityDomain; }
			set { ApiRequest.CharityDomain = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetCharitiesRequestType.IncludeDescription"/> of type <see cref="bool"/>.
		/// </summary>
		public bool IncludeDescription
		{ 
			get { return ApiRequest.IncludeDescription; }
			set { ApiRequest.IncludeDescription = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetCharitiesRequestType.MatchType"/> of type <see cref="StringMatchCodeType"/>.
		/// </summary>
		public StringMatchCodeType MatchType
		{ 
			get { return ApiRequest.MatchType; }
			set { ApiRequest.MatchType = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetCharitiesRequestType.Featured"/> of type <see cref="bool"/>.
		/// </summary>
		public bool Featured
		{ 
			get { return ApiRequest.Featured; }
			set { ApiRequest.Featured = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetCharitiesRequestType.CampaignID"/> of type <see cref="long"/>.
		/// </summary>
		public long CampaignID
		{ 
			get { return ApiRequest.CampaignID; }
			set { ApiRequest.CampaignID = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetCharitiesResponseType.Charity"/> of type <see cref="CharityInfoTypeCollection"/>.
		/// </summary>
		public CharityInfoTypeCollection CharityList
		{ 
			get { return ApiResponse.Charity; }
		}
		

		#endregion

		
	}
}
