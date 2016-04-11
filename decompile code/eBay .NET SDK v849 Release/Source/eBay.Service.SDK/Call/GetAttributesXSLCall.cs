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
	public class GetAttributesXSLCall : ApiCall
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public GetAttributesXSLCall()
		{
			ApiRequest = new GetAttributesXSLRequestType();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ApiContext">The <see cref="ApiCall.ApiContext"/> for this API Call of type <see cref="ApiContext"/>.</param>
		public GetAttributesXSLCall(ApiContext ApiContext)
		{
			ApiRequest = new GetAttributesXSLRequestType();
			this.ApiContext = ApiContext;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// This type is deprecated as the call is no longer available.
		/// </summary>
		/// 
		/// <param name="XSLFileName">
		/// The name of the XSL file to retrieve. If not specified, the call
		/// returns the latest versions of all available XSL files.
		/// FileName is an optional input. Valid values
		/// 
		/// syi_attributes.xsl
		/// </param>
		///
		/// <param name="XSLFileVersion">
		/// The desired version of the XSL file. Required if FileName is specified.
		/// If not specified, the call returns the latest version of the file.
		/// (This is not a filter for retrieving changes to the XSL file.)
		/// </param>
		///
		public XSLFileTypeCollection GetAttributesXSL(string XSLFileName, string XSLFileVersion)
		{
			this.XSLFileName = XSLFileName;
			this.XSLFileVersion = XSLFileVersion;

			Execute();
			return ApiResponse.XSLFile;
		}

 		/// <summary>
 		/// Call GetAttributesXSLVersions to retrieve the Item Specifics XSL stylesheet(s) versions for a site.
 		/// </summary>
 		/// <returns>The <see cref="XSLFileList"/>.</returns>
 		public XSLFileTypeCollection GetAttributesXSLVersions()
 		{
 			this.DetailLevelOverride = true;
 			Execute();
 			this.DetailLevelOverride = false;
 			return XSLFileList;
 		}


		/// <summary>
		/// For backward compatibility with old wrappers.
		/// </summary>
		public XSLFileTypeCollection GetAttributesXSL()
		{
			Execute();
			return XSLFileList;
		}

		#endregion



		#region Static Methods
		/// <summary>
		/// Decodes and replaces the Base64 Encoded <see cref="XSLFileType.FileContent"/>.
		/// </summary>
		/// <param name="XSLFile">The XSL File to Decode.</param>
		public static void DecodeFileContent(XSLFileType XSLFile)
		{
			try
			{
				byte[] xslPBytes = System.Convert.FromBase64String(XSLFile.FileContent);
				XSLFile.FileContent = System.Text.Encoding.ASCII.GetString(xslPBytes, 0, xslPBytes.Length);
			} 
			catch (Exception ex)
			{
				throw new ApiException(ex.Message, ex);
			}
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
		/// Gets or sets the <see cref="GetAttributesXSLRequestType"/> for this API call.
		/// </summary>
		public GetAttributesXSLRequestType ApiRequest
		{ 
			get { return (GetAttributesXSLRequestType) AbstractRequest; }
			set { AbstractRequest = value; }
		}

		/// <summary>
		/// Gets the <see cref="GetAttributesXSLResponseType"/> for this API call.
		/// </summary>
		public GetAttributesXSLResponseType ApiResponse
		{ 
			get { return (GetAttributesXSLResponseType) AbstractResponse; }
		}

		
 		/// <summary>
		/// Gets or sets the <see cref="GetAttributesXSLRequestType.FileName"/> of type <see cref="string"/>.
		/// </summary>
		public string XSLFileName
		{ 
			get { return ApiRequest.FileName; }
			set { ApiRequest.FileName = value; }
		}
		
 		/// <summary>
		/// Gets or sets the <see cref="GetAttributesXSLRequestType.FileVersion"/> of type <see cref="string"/>.
		/// </summary>
		public string XSLFileVersion
		{ 
			get { return ApiRequest.FileVersion; }
			set { ApiRequest.FileVersion = value; }
		}
		
		
 		/// <summary>
		/// Gets the returned <see cref="GetAttributesXSLResponseType.XSLFile"/> of type <see cref="XSLFileTypeCollection"/>.
		/// </summary>
		public XSLFileTypeCollection XSLFileList
		{ 
			get { return ApiResponse.XSLFile; }
		}
		

		#endregion

		
	}
}
