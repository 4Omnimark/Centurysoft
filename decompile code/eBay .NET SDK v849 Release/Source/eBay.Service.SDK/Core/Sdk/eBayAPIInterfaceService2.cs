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
using System.Net;
using eBay.Service.Core.Soap;

namespace eBay.Service.Core.Sdk
{
	/// <summary>
	/// Enhanced eBayAPIInterfaceService with GZIP compression support.
	/// </summary>
	internal class eBayAPIInterfaceService2 : eBayAPIInterfaceService
	{
		private bool mEnableComression = false;
		private ApiLogManager mLogger = null;
		private CallMetricsEntry mCallMetrics = null;
        private IWebProxy mWebProxy = null;

		public bool EnableComression
		{
			get { return this.mEnableComression; }
			set { this.mEnableComression = value; }
		}

		public ApiLogManager ApiLogManager
		{ 
			get { return mLogger; }
			set { mLogger = value; }
		}

		public CallMetricsEntry CallMetricsEntry
		{ 
			get { return mCallMetrics; }
			set { mCallMetrics = value; }
		}

        public IWebProxy WebProxy
        {
            get { return mWebProxy; }
            set { mWebProxy = value; }
        }

		protected override  WebRequest GetWebRequest(Uri uri)
		{
			if( this.mEnableComression )
			{
				HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(uri);
				request.ProtocolVersion = HttpVersion.Version10;

				request.Headers.Add("Accept-Encoding", "gzip, deflate");
				if (mCallMetrics != null)
				{
					mCallMetrics.NetworkSendStarted = DateTime.Now;
					request.Headers.Add("X-EBAY-API-METRICS", "true");
				}
                if (mWebProxy != null)
                {
                    request.Proxy = mWebProxy;
                }
				return request;
			}
			else
			{
				WebRequest request = base.GetWebRequest(uri);
				if (mCallMetrics != null)
				{
					mCallMetrics.NetworkSendStarted = DateTime.Now;
					request.Headers.Add("X-EBAY-API-METRICS", "true");
				}
                if (mWebProxy != null)
                {
                    request.Proxy = mWebProxy;
                }
				return request;
			}
		}

		protected override WebResponse GetWebResponse(WebRequest request)
		{

			if( this.mEnableComression )
			{
				HttpWebResponseDecompressed response = new HttpWebResponseDecompressed((HttpWebRequest)request);
				if (mCallMetrics != null) 
				{
					mCallMetrics.NetworkReceiveEnded = DateTime.Now;
					mCallMetrics.ServerProcessingTime = convertProcessingTime(response.Headers.Get("X-EBAY-API-PROCESS-TIME"));
				}
				response.ApiLogManager = this.mLogger;
				validate(response.CastToHttpWebResponse.StatusCode);
				return response;
			}
			else 
			{
				HttpWebResponse response = (HttpWebResponse) base.GetWebResponse(request);
				if (mCallMetrics != null)
				{
					mCallMetrics.NetworkReceiveEnded = DateTime.Now;
					mCallMetrics.ServerProcessingTime = convertProcessingTime(response.Headers.Get("X-EBAY-API-PROCESS-TIME"));
				}
				validate(response.StatusCode);
				return response;
			}
		}
		private void validate(HttpStatusCode code) 
		{
			int typecode = (int)code;
			if (typecode >= 300 && typecode <= 599 && typecode != 500)
				throw new HttpException(typecode);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		private long convertProcessingTime(String val) 
		{
			if (val == null)
				return 0;
			long retval = 0;
			try 
			{
				retval = (long) (float.Parse(val));
			} 
			catch (Exception) 
			{
			}
			return retval;
		}


	}
}
