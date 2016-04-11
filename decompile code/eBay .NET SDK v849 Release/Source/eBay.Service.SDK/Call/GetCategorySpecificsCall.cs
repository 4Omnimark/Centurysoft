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
	public class GetCategorySpecificsCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetCategorySpecificsCall()
		{
			ApiRequest = new GetCategorySpecificsRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetCategorySpecificsCall(ApiContext ApiContext)
		{
			ApiRequest = new GetCategorySpecificsRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Returns the most popular custom Item Specific names and values for each
		/// category you request.
		/// </summary>
		/// 
		/// <param name="CategoryIDList">
		/// An eBay category ID. This call retrieves recommended
		/// Item Specifics (if any) for each category you specify.
		/// To determine which categories support listing with custom
		/// Item Specifics, use GetCategoryFeatures.
		/// 
		/// <span class="tablenote"><b>Note:</b>
		/// This call may return recommendations for categories that don't
		/// support listing with custom Item Specifics. That is, the
		/// presence of recommendations for a category does not necessarily
		/// mean that AddItem supports custom Item Specifics for that
		/// category and site.
		/// </span>
		/// 
		/// The request requires either CategoryID, CategorySpecfics.CategoryID, or
		/// CategorySpecificsFileInfo (or the call returns an error). CategoryID and
		/// CategorySpecific.CategoryID can both be used in the same request.
		/// (CategorySpecific offers more options to control the response.)
		/// Some input fields, such as IncludeConfidence, only work when 
		/// CategoryID or CategorySpecfics.CategoryID is specified.
		/// 
		/// You can specify multiple categories, but more categories can result in
		/// longer response times. If your request times out, specify fewer IDs. If you
		/// specify the same ID twice, we use the first instance.
		/// </param>
		///
		/// <param name="LastUpdateTime">
		/// Causes the recommendation engine to check whether the list of
		/// popular Item Specifics for each specified category has changed
		/// since this date and time. If specified, this call returns no
		/// Item Specifics; it only returns whether the data has changed
		/// for any of the requested categories.
		/// 
		/// Typically, you pass in the Timestamp that was
		/// returned the last time you refreshed the list of names and values
		/// for the same categories. If the Updated flag returns true for any
		/// categories in the response, call GetCategorySpecifics again
		/// for those categories to get the latest names and values.
		/// (As downloading all the data may affect your application's
		/// performance, it may help to only download Item Specifics
		/// for a category if they have changed since you last checked.)
		/// </param>
		///
		/// <param name="MaxNames">
		/// Maximum number of Item Specifics to return
		/// per category (where each Item Specific is identified
		/// by a unique name within the category).
		/// Use this to retrieve fewer results per category.
		/// For example, if you only want up to 2 per category
		/// (the top 2 most popular names), specify 2.
		/// </param>
		///
		/// <param name="MaxValuesPerName">
		/// Maximum number of values to retrieve per item specific. 
		/// The best practice for using this field depends on your use case.
		/// For example, if you want all possible values (such as all brands 
		/// and sizes in a clothing category), then specify a very large 
		/// number. (This is recommended in most cases.) If you only want the most popular value (like the most popular color), then specify a small number.
		/// </param>
		///
		/// <param name="Name">
		/// The name of one Item Specific name to find values for.
		/// Use this if you want to find out whether a name
		/// that the seller provided has recommended values.
		/// If you specify multiple categories in the request,
		/// the recommendation engine returns all matching
		/// names and values it finds for each of those categories.
		/// At the time of this writing, this value is case-sensitive.
		/// (Wildcards are not supported.)
		/// 
		/// Name and CategorySpecific.ItemSpecific can be used in the
		/// request. (If you plan to only use one or the other in your application,
		/// you should use ItemSpecific, as it may offer more options in the future.)
		/// </param>
		///
		/// <param name="CategorySpecificList">
		/// Applicable with request version 609 and higher. (This
		/// does not retrieve data at all if your request version is lower
		/// than 609.)
		/// Contains a category for which you want recommended
		/// Item Specifics, and (optionally) names and values to help
		/// you refine the recommendations.
		/// You can specify multiple categories (but more categories
		/// can result in longer response times). If you specify the same
		/// category twice, we use the first instance.
		/// 
		/// Depending on how many recommendations are found, your request
		/// may time out if you specify too many categories.
		/// (Typically, you can download recommendations for about 275
		/// categories at a time.)
		/// 
		/// CategoryID and CategorySpecific.CategoryID can be used
		/// in the request. (If you plan to only use one or the other in
		/// your application, you should use CategorySpecific,
		/// as it may offer more options in the future.)
		/// </param>
		///
		/// <param name="ExcludeRelationships">
		/// If true, the Relationship node is not returned for any
		/// recommendations. Relationship recommendations tell you whether
		/// an Item Specific value has a logical dependency another
		/// Item Specific.
		/// 
		/// For example, in a clothing category, Size Type could be
		/// recommended as a parent of Size, because Size=Small would
		/// mean something different to buyers depending on whether
		/// Size Type=Petite or Size Type=Plus.
		/// 
		/// In general, it is a good idea to retrieve and use relationship
		/// recommendations, as this data can help buyers find the items
		/// they want more easily.
		/// </param>
		///
		/// <param name="IncludeConfidence">
		/// If true, returns eBay's level of confidence in the popularity of
		/// each name and value for the specified category. 
		/// Some sellers may find this useful when
		/// choosing whether to use eBay's recommendation or their own
		/// name or value.
		/// 
		/// Requires CategoryID to also be passed in.
		/// 
		/// If you try to use this with CategorySpecificsFileInfo 
		/// but without	CategoryID, the request fails and no 
		/// TaskReferenceID or FileReferenceID is returned.
		/// </param>
		///
		/// <param name="CategorySpecificsFileInfo">
		/// If true, the response includes FileReferenceID and
		/// TaskReferenceID. Use these IDs as inputs to the downloadFile
		/// call in the eBay File Transfer API. That API lets you retrieve
		/// a single (bulk) GetCategorySpecifics response with all the Item
		/// Specifics recommendations available for the requested site ID.
		/// (The downloadFile call downloads a .zip file as an
		/// attachment.)
		/// 
		/// Either CategorySpecificsFileInfo or a CategoryID is required
		/// (or you can specify both). 
		/// 
		/// <span class="tablenote"><b>Note:</b>
		/// You can use the File Transfer API without using or learning
		/// about the Bulk Data Exchange API or other
		/// Large Merchant Services APIs.
		/// </span>
		/// 
		/// </param>
		///
		public RecommendationsTypeCollection GetCategorySpecifics(StringCollection CategoryIDList, DateTime LastUpdateTime, int MaxNames, int MaxValuesPerName, string Name, CategoryItemSpecificsTypeCollection CategorySpecificList, bool ExcludeRelationships, bool IncludeConfidence, bool CategorySpecificsFileInfo)
		{
			this.CategoryIDList = CategoryIDList;
			this.LastUpdateTime = LastUpdateTime;
			this.MaxNames = MaxNames;
			this.MaxValuesPerName = MaxValuesPerName;
			this.Name = Name;
			this.CategorySpecificList = CategorySpecificList;
			this.ExcludeRelationships = ExcludeRelationships;
			this.IncludeConfidence = IncludeConfidence;
			this.CategorySpecificsFileInfo = CategorySpecificsFileInfo;

			Execute();
			return ApiResponse.Recommendations;
		}


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public RecommendationsTypeCollection GetCategorySpecifics(StringCollection CategoryIDList)
		{
			this.CategoryIDList = CategoryIDList;

			Execute();
			return ApiResponse.Recommendations;
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
		/// Gets or sets the <see cref="GetCategorySpecificsRequestType"/> for this API call.
		/// </summary>
		public GetCategorySpecificsRequestType ApiRequest
		{ 
			get { return (GetCategorySpecificsRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetCategorySpecificsResponseType"/> for this API call.
		/// </summary>
		public GetCategorySpecificsResponseType ApiResponse
		{ 
			get { return (GetCategorySpecificsResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetCategorySpecificsRequestType.CategoryID"/> of type <see cref="StringCollection"/>.
		/// </summary>
		public StringCollection CategoryIDList
		{ 
			get { return ApiRequest.CategoryID; }
			set { ApiRequest.CategoryID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetCategorySpecificsRequestType.LastUpdateTime"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime LastUpdateTime
		{ 
			get { return ApiRequest.LastUpdateTime; }
			set { ApiRequest.LastUpdateTime = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetCategorySpecificsRequestType.MaxNames"/> of type <see cref="int"/>.
		/// </summary>
		public int MaxNames
		{ 
			get { return ApiRequest.MaxNames; }
			set { ApiRequest.MaxNames = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetCategorySpecificsRequestType.MaxValuesPerName"/> of type <see cref="int"/>.
		/// </summary>
		public int MaxValuesPerName
		{ 
			get { return ApiRequest.MaxValuesPerName; }
			set { ApiRequest.MaxValuesPerName = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetCategorySpecificsRequestType.Name"/> of type <see cref="string"/>.
		/// </summary>
		public string Name
		{ 
			get { return ApiRequest.Name; }
			set { ApiRequest.Name = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetCategorySpecificsRequestType.CategorySpecific"/> of type <see cref="CategoryItemSpecificsTypeCollection"/>.
		/// </summary>
		public CategoryItemSpecificsTypeCollection CategorySpecificList
		{ 
			get { return ApiRequest.CategorySpecific; }
			set { ApiRequest.CategorySpecific = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetCategorySpecificsRequestType.ExcludeRelationships"/> of type <see cref="bool"/>.
		/// </summary>
		public bool ExcludeRelationships
		{ 
			get { return ApiRequest.ExcludeRelationships; }
			set { ApiRequest.ExcludeRelationships = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetCategorySpecificsRequestType.IncludeConfidence"/> of type <see cref="bool"/>.
		/// </summary>
		public bool IncludeConfidence
		{ 
			get { return ApiRequest.IncludeConfidence; }
			set { ApiRequest.IncludeConfidence = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetCategorySpecificsRequestType.CategorySpecificsFileInfo"/> of type <see cref="bool"/>.
		/// </summary>
		public bool CategorySpecificsFileInfo
		{ 
			get { return ApiRequest.CategorySpecificsFileInfo; }
			set { ApiRequest.CategorySpecificsFileInfo = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetCategorySpecificsResponseType.Recommendations"/> of type <see cref="RecommendationsTypeCollection"/>.
		/// </summary>
		public RecommendationsTypeCollection RecommendationList
		{ 
			get { return ApiResponse.Recommendations; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetCategorySpecificsResponseType.TaskReferenceID"/> of type <see cref="string"/>.
		/// </summary>
		public string TaskReferenceID
		{ 
			get { return ApiResponse.TaskReferenceID; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetCategorySpecificsResponseType.FileReferenceID"/> of type <see cref="string"/>.
		/// </summary>
		public string FileReferenceID
		{ 
			get { return ApiResponse.FileReferenceID; }
		}
		

		#endregion

		
	}
}
