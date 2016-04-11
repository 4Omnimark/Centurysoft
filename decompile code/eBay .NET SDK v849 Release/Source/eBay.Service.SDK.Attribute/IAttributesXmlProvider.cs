using eBay.Service.Core.Sdk;
using System.Xml;

namespace eBay.Service.SDK.Attribute
{
	
	/// <summary>
	/// Supplies attributes XML for <c>IAttributesMaster</c>. 
	/// The implementation of IAttributeXmlProvider should handle AttributeSet 
	/// objects that has valid AttributeSet.ProductId nd return attributes with pre-filled 
	/// product information for them. 
	/// </summary>
	public interface IAttributesXmlProvider
	{
		/// <summary>
		/// Get CharacteristicSet(CS) xml. Set AttributeSet.ProductId to retrieve CSXml with
		/// pre-filled product information.
		/// </summary>
		/// <param name="csInfo">Identification information about the CS for which 
		/// you want to get CSXml. You only need to set CS.CSId and, optionally, you can
		/// set CS.ProductId if you want to get the CSXml that is associated with
		/// specific product information.
		/// Set CS.CSId to 0 to get entire CharacteristicSet xml for all CSs.</param>
		/// <returns>The xml text of the CS.</returns>
		string GetCSXmlText(AttributeSet csInfo);

		/// <summary>
		/// Get xml that contains multiple CSs. Only set AttributeSet.CSId
		/// and AttributeSet.ProductId (optional). Set AttributeSet.ProductId to retrieve CSXml with
		/// pre-filled product information.
		/// </summary>
		/// <param name="asList">List of <c>AttributeSet</c> objects for which
		/// you want to get CSXml.</param>
		/// <returns>The CS xml for specified CSs.</returns>
		/// 
		XmlDocument GetMultipleCSXml(IAttributeSetCollection asList);

		/// <summary>
		/// Get xml that contains multiple CSs. Only set AttributeSet.CSId
		/// and AttributeSet.ProductId (optional). Set AttributeSet.ProductId to retrieve CSXml with
		/// pre-filled product information.
		/// </summary>
		/// <param name="asList">List of <c>AttributeSet</c> objects for which
		/// you want to get CSXml.</param>
		/// <returns>The CS xml text for specified CSs.</returns>
		string GetMultipleCSXmlText(IAttributeSetCollection asList);

		/// <summary>
		/// Get CS xml data by calling eBay API.
		/// </summary>
		/// 
		XmlDocument DownloadXml();

		/// <summary>
		/// Get CS xml data by calling eBay API.
		/// </summary>
		/// <param name="asn">The <c>ApiContext</c> object to make API call.</param>
		XmlDocument DownloadXml(ApiContext asn);

		/// <summary>
		/// Get all the CS IDs. 
		/// </summary>
		/// <returns>Array of CSIDs.</returns>
		int[] GetCSIDs();
	}

}