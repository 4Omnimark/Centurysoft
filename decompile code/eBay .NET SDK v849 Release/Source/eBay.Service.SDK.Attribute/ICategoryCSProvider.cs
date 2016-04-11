using eBay.Service.Core.Soap;
using eBay.Service.Core.Sdk;
using System.Collections;



namespace eBay.Service.SDK.Attribute
{
	/// <summary>
	/// Supplies categoryId to CSId convertion service for <c>IAttributesMaster</c>.
	/// </summary>
	public interface ICategoryCSProvider
	{
		/// <summary>
		/// Get CSId by categoryId.
		/// </summary>
		/// <param name="categoryId">The categoryId for which you want to get CSId.</param>
		/// <returns>The CSId.</returns>
		/// 
		int GetVCSId(int categoryId);

		/// <summary>
		/// Get the categories data that it's using for conversion.
		/// </summary>
		/// <returns></returns>
		CategoryTypeCollection GetCategoriesCS();

		/// <summary>
		/// Get the categories data that it's using for conversion.
		/// </summary>
		/// <param name="categoryId">The categoryId for which you want to get CSId.</param>
		/// <returns></returns>
		CategoryTypeCollection GetCategoriesCS(string categoryId);

		/// <summary>
		/// Get CategoryCS data by calling eBay API.
		/// </summary>
		/// <param name="categoryId"></param>
		CategoryTypeCollection DownloadCategoryCS(string categoryId); 

		/// <summary>
		/// Get CategoryCS data by calling eBay API. Special version for fast example usage.
		/// </summary>
		/// <param name="asn">The <c>ApiContext</c> object to make API call.</param>
		/// <param name="categoryId">A specific category ID for which to download CategoryCS data.</param>
		CategoryTypeCollection DownloadCategoryCS(ApiContext asn, string categoryId);

		/// <summary>
		/// Get Site-Wide characteristic sets by category ID.
		/// </summary>
		/// <param name="categoryId">A specific category ID for which fetch Site-Wide CategoryCS data.</param>
		SiteWideCharacteristicsTypeCollection GetSiteWideCharacteristics(string categoryId);

		/// <summary>
		/// Get Site-Wide characteristic sets ids by category ID.
		/// </summary>
		/// <param name="categoryId">A specific category ID for which fetch Site-Wide CategoryCS data.</param>
		int[] GetSiteWideCharSetsAttrIds(string categoryId);

		/// <summary>
		/// Get CSIdArray by categoryId.
		/// </summary>
		/// <param name="categoryId">The categoryId for which you want to get CSId array</param>
		int[] GetVCSIdArray(int categoryId);
  
		/// <summary>
		/// Get getVCSIdMap by categoryId.
		/// </summary>
		/// <param name="categoryId">The categoryId for which you want to get CSId map.</param>
		Hashtable GetVCSIdMap(int categoryId);
	}

}