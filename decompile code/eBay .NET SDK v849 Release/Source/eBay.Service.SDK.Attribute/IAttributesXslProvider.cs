using eBay.Service.Core.Sdk;

namespace eBay.Service.SDK.Attribute
{
	/// <summary>
	/// Supplies attributes XSL for <c>IAttributesMaster</c>.
	/// </summary>
	public interface IAttributesXslProvider
	{
		/// <summary>
		/// The XSL text.
		/// </summary>
		string GetXslText();

		/// <summary>
		/// The XSL file name.
		/// </summary>
		string GetXslFileName();

		/// <summary>
		/// Downloads XSL file.
		/// </summary>
		string DownloadXsl(ApiContext asn);

		/// <summary>
		/// Downloads XSL file.
		/// </summary>
		string DownloadXsl();

	}
}