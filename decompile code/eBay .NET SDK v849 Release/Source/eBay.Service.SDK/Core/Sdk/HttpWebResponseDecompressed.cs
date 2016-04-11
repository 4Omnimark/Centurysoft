#region Copyright
//	Copyright (c) 2013 eBay, Inc.
//	
//	This program is licensed under the terms of the eBay Common Development and
//	Distribution License (CDDL) Version 1.0 (the "License") and any subsequent  
//	version thereof released by eBay.  The then-current version of the License can be 
//	found at http://www.opensource.org/licenses/cddl1.php and in the eBaySDKLicense 
//	file that is under the eBay SDK ../docs directory
#endregion

using System;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace eBay.Service.Core.Sdk
{
	/// <summary>
	/// Summary description for HttpWebResponseDecompressed.
	/// </summary>
	public class HttpWebResponseDecompressed : System.Net.WebResponse
	{
		private HttpWebResponse response;
		private ApiLogManager mLogger = null;

		/// <summary>
		/// 
		/// </summary>
		public ApiLogManager ApiLogManager
		{ 
			get { return mLogger; }
			set { mLogger = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		public HttpWebResponseDecompressed(WebRequest request)
		{
			WebResponse webResponse = null;
			try
			{
				//request.Timeout = timeout;
				webResponse = request.GetResponse();
			}
			catch (WebException e)
			{
				if (e.Response == null)
				{
					throw e;
				}
				webResponse = e.Response;
			}			
			
			//response = (HttpWebResponse)request.GetResponse();
			this.response = (HttpWebResponse)webResponse;
		}

		/// <summary>
		/// 
		/// </summary>
		public HttpWebResponse CastToHttpWebResponse
		{
			get { return response; }
		}

		/// <summary>
		/// 
		/// </summary>
		public override void Close()
		{
			response.Close();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override Stream GetResponseStream()
		{
            Stream responseStream = response.GetResponseStream();

            Stream decompressedStream = null;
            if (response.ContentEncoding.ToLower().Contains("gzip")) {
                decompressedStream = new GZipStream(responseStream, CompressionMode.Decompress);
            }
            //else if (response.ContentEncoding.ToLower().Contains("deflate")) {
            //    decompressedStream = new DeflateStream(responseStream, CompressionMode.Decompress);
            //}

            if (decompressedStream != null)
            {
                // copy to memory stream
                MemoryStream memoryStream = new MemoryStream();
                int size = 2048;
                byte[] buf = new byte[2048];
                while (true)
                {
                    size = decompressedStream.Read(buf, 0, buf.Length);
                    if (size > 0)
                    {
                        memoryStream.Write(buf, 0, size);
                    }
                    else
                    {
                        break;
                    }
                }
                long originalSize = memoryStream.Position;
                memoryStream.Seek(0, SeekOrigin.Begin);

                if (this.mLogger != null)
                {
                    string msg = String.Format("Http Compression - decompressed {0}-encoded response data: CompressedSize={1} OriginalSize={2}",
                        response.ContentEncoding, this.ContentLength, originalSize);
                    this.mLogger.RecordMessage(msg);
                }

                return memoryStream;
            }

            return responseStream;
		}

		/// <summary>
		/// 
		/// </summary>
		public override long ContentLength
		{
			get { return response.ContentLength; }
		}

		/// <summary>
		/// 
		/// </summary>
		public override string ContentType
		{
			get { return response.ContentType; }
		}

		/// <summary>
		/// 
		/// </summary>
		public override System.Net.WebHeaderCollection Headers
		{
			get { return response.Headers; }
		}

		/// <summary>
		/// 
		/// </summary>
		public override System.Uri ResponseUri
		{
			get { return response.ResponseUri; }
		} 	
	}
}
