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
	public class GetCategoryFeaturesCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetCategoryFeaturesCall()
		{
			ApiRequest = new GetCategoryFeaturesRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetCategoryFeaturesCall(ApiContext ApiContext)
		{
			ApiRequest = new GetCategoryFeaturesRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Returns information about the features that are applicable to different categories,
		/// such as listing durations, shipping term requirements, and Best Offer support.
		/// </summary>
		/// 
		/// <param name="CategoryID">
		/// Specifies the category for which you want to retrieve the feature settings.
		/// 
		/// Specify a CategoryID, set DetailLevel to ReturnAll, and set
		/// ViewAllNodes to true to return the default site settings, the
		/// overrides for the specified category, plus all the child
		/// categories that have overrides on the features they inherit.
		/// 
		/// If you also set AllFeaturesForCategory to true, eBay returns the site
		/// defaults, plus all the settings for the specified category. Child
		/// category information is not returned in this case.
		/// 
		/// If CategoryID is not specified, eBay returns the feature settings for
		/// the site. To return details on all categories that have overrides on
		/// the properties they inherit, set DetailLevel to ReturnAll, and set
		/// ViewAllNodes to true. If you also set AllFeaturesForCategory to true,
		/// eBay returns only the site defaults with no child category information.
		/// </param>
		///
		/// <param name="LevelLimit">
		/// A level of depth in the category hierarchy. Retrieves all category
		/// nodes with a CategoryLevel less than or equal to the LevelLimit
		/// value.
		/// </param>
		///
		/// <param name="ViewAllNodes">
		/// You must set DetailLevel to ReturnAll in order to correctly populate the
		/// response when you set ViewAllNodes to true. In this case, eBay returns the
		/// site defaults along with all the categories that override the feature
		/// settings they inherit. Here, each Category container shows only the
		/// features that it has overridden from its parent node.
		/// 
		/// If you also specify a CategoryID, eBay returns the details for that category,
		/// along with containers for each of its child categories that have feature
		/// overrides.
		/// 
		/// Note that if ViewAllNodes is set to false (the default) and DetailLevel is
		/// set to ReturnAll, eBay returns only the leaf categories that have features
		/// that override the settings they inherit. In this case, the call will not
		/// return leaf categories that do not have overrides.
		/// </param>
		///
		/// <param name="FeatureIDList">
		/// Use this field if you want to know if specific features are enabled at the site
		/// or root category level. Multiple <b>FeatureID</b> elements can be
		/// used in the request. If no <b>FeatureID</b> elements are used, the
		/// call retrieves data for all features, as applicable to the other request 
		/// parameters.
		/// </param>
		///
		/// <param name="AllFeaturesForCategory">
		/// Use this switch to view all of the feature settings for a specific category.
		/// All feature settings are returned, regardless of the site default settings.
		/// This element works in conjunction with CategoryID--refer to the notes for
		/// that element for more details.
		/// 
		/// If you also set FeatureID, eBay returns the status of the specified
		/// features only, for the specified category.
		/// </param>
		///
		public CategoryFeatureTypeCollection GetCategoryFeatures(string CategoryID, int LevelLimit, bool ViewAllNodes, FeatureIDCodeTypeCollection FeatureIDList, bool AllFeaturesForCategory)
		{
			this.CategoryID = CategoryID;
			this.LevelLimit = LevelLimit;
			this.ViewAllNodes = ViewAllNodes;
			this.FeatureIDList = FeatureIDList;
			this.AllFeaturesForCategory = AllFeaturesForCategory;

			Execute();
			return ApiResponse.Category;
		}


 		/// <summary>
 		/// Call GetCategoryFeaturesVersion to retrieve the Category version for a site.
 		/// </summary>
 		/// <returns>The <see cref="GetCategoryFeaturesResponseType.CategoryVersion"/>.</returns>
 		public string GetCategoryFeaturesVersion()
 		{
 			this.DetailLevelOverride = true;
 			Execute();
 			this.DetailLevelOverride = false;
 			return ApiResponse.CategoryVersion;
 		}	

		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public CategoryFeatureTypeCollection GetCategoryFeatures(/*FeatureIDCodeTypeCollection FeatureIDList*/)
		{
			Execute();
			return CategoryList;
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
		/// Gets or sets the <see cref="GetCategoryFeaturesRequestType"/> for this API call.
		/// </summary>
		public GetCategoryFeaturesRequestType ApiRequest
		{ 
			get { return (GetCategoryFeaturesRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetCategoryFeaturesResponseType"/> for this API call.
		/// </summary>
		public GetCategoryFeaturesResponseType ApiResponse
		{ 
			get { return (GetCategoryFeaturesResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetCategoryFeaturesRequestType.CategoryID"/> of type <see cref="string"/>.
		/// </summary>
		public string CategoryID
		{ 
			get { return ApiRequest.CategoryID; }
			set { ApiRequest.CategoryID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetCategoryFeaturesRequestType.LevelLimit"/> of type <see cref="int"/>.
		/// </summary>
		public int LevelLimit
		{ 
			get { return ApiRequest.LevelLimit; }
			set { ApiRequest.LevelLimit = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetCategoryFeaturesRequestType.ViewAllNodes"/> of type <see cref="bool"/>.
		/// </summary>
		public bool ViewAllNodes
		{ 
			get { return ApiRequest.ViewAllNodes; }
			set { ApiRequest.ViewAllNodes = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetCategoryFeaturesRequestType.FeatureID"/> of type <see cref="FeatureIDCodeTypeCollection"/>.
		/// </summary>
		public FeatureIDCodeTypeCollection FeatureIDList
		{ 
			get { return ApiRequest.FeatureID; }
			set { ApiRequest.FeatureID = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetCategoryFeaturesRequestType.AllFeaturesForCategory"/> of type <see cref="bool"/>.
		/// </summary>
		public bool AllFeaturesForCategory
		{ 
			get { return ApiRequest.AllFeaturesForCategory; }
			set { ApiRequest.AllFeaturesForCategory = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetCategoryFeaturesResponseType.CategoryVersion"/> of type <see cref="string"/>.
		/// </summary>
		public string CategoryVersion
		{ 
			get { return ApiResponse.CategoryVersion; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetCategoryFeaturesResponseType.UpdateTime"/> of type <see cref="DateTime"/>.
		/// </summary>
		public DateTime UpdateTime
		{ 
			get { return ApiResponse.UpdateTime; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetCategoryFeaturesResponseType.Category"/> of type <see cref="CategoryFeatureTypeCollection"/>.
		/// </summary>
		public CategoryFeatureTypeCollection CategoryList
		{ 
			get { return ApiResponse.Category; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetCategoryFeaturesResponseType.SiteDefaults"/> of type <see cref="SiteDefaultsType"/>.
		/// </summary>
		public SiteDefaultsType SiteDefaults
		{ 
			get { return ApiResponse.SiteDefaults; }
		}
		
 		/// <summary>
		/// Gets the returned <see cref="GetCategoryFeaturesResponseType.FeatureDefinitions"/> of type <see cref="FeatureDefinitionsType"/>.
		/// </summary>
		public FeatureDefinitionsType FeatureDefinitions
		{ 
			get { return ApiResponse.FeatureDefinitions; }
		}
		

		#endregion

		
	}
}
