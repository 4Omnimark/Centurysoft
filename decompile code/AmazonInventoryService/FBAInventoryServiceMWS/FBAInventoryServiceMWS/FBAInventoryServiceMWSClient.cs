/******************************************************************************* 
 *  Copyright 2009 Amazon Services. All Rights Reserved.
 *  Licensed under the Apache License, Version 2.0 (the "License"); 
 *  
 *  You may not use this file except in compliance with the License. 
 *  You may obtain a copy of the License at: http://aws.amazon.com/apache2.0
 *  This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
 *  CONDITIONS OF ANY KIND, either express or implied. See the License for the 
 *  specific language governing permissions and limitations under the License.
 * ***************************************************************************** 
 * 
 *  FBA Inventory Service MWS CSharp Library
 *  API Version: 2010-10-01
 *  Generated: Fri Oct 22 09:53:30 UTC 2010 
 * 
 */

using System;
using System.Web;
using System.Net;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Globalization;
using System.Xml.Serialization;
using System.Collections.Generic;
using FBAInventoryServiceMWS.Model;
using FBAInventoryServiceMWS;
using System.Reflection;
using System.Collections.Specialized;


namespace FBAInventoryServiceMWS
{


   /**

    *
    * FBAInventoryServiceMWSClient is an implementation of FBAInventoryServiceMWS
    *
    */
    public class FBAInventoryServiceMWSClient : FBAInventoryServiceMWS
    {

        private String awsAccessKeyId = null;
        private String awsSecretAccessKey = null;
        private FBAInventoryServiceMWSConfig config = null;

        /// <summary>
        /// Constructs FBAInventoryServiceMWSClient with AWS Access Key ID and AWS Secret Key
        /// </summary>
        /// <param name="awsAccessKeyId">AWS Access Key ID</param>
        /// <param name="awsSecretAccessKey">AWS Secret Access Key</param>
        public FBAInventoryServiceMWSClient(String awsAccessKeyId, String awsSecretAccessKey)
            : this(awsAccessKeyId, awsSecretAccessKey, new FBAInventoryServiceMWSConfig())
        {
        }

        /// <summary>
        /// Constructs FBAInventoryServiceMWSClient with AWS Access Key ID and AWS Secret Key
        /// </summary>
        /// <param name="awsAccessKeyId">AWS Access Key ID</param>
        /// <param name="awsSecretAccessKey">AWS Secret Access Key</param>
        /// <param name="config">configuration</param>
        public FBAInventoryServiceMWSClient(String awsAccessKeyId, String awsSecretAccessKey, FBAInventoryServiceMWSConfig config)
        {
            this.awsAccessKeyId = awsAccessKeyId;
            this.awsSecretAccessKey = awsSecretAccessKey;
            if (!config.IsSetUserAgent())
            {
                throw new FBAInventoryServiceMWSException("Missing required value: User-Agent.");
            }
            this.config = config;
        }

        /// Constructs FBAInventoryServiceMWSClient with AWS Access Key ID and AWS Secret Key
        /// an application name, and application version.
        /// </summary>
        /// <param name="awsAccessKeyId">AWS Access Key ID</param>
        /// <param name="awsSecretAccessKey">AWS Secret Access Key</param>
        /// <param name="applicationName">Application Name</param>
        /// <param name="applicationVersion">Application Version</param>
        public FBAInventoryServiceMWSClient(
            String awsAccessKeyId, 
            String awsSecretAccessKey,
            String applicationName,
            String applicationVersion ) 
            : this(
                awsAccessKeyId, 
                awsSecretAccessKey, 
                applicationName,
                applicationVersion,
                new FBAInventoryServiceMWSConfig())
        {
        }


        /// <summary>
        /// Constructs FBAInventoryServiceMWSClient with AWS Access Key ID and AWS Secret Key
        /// an application name, and application version.
        /// </summary>
        /// <param name="awsAccessKeyId">AWS Access Key ID</param>
        /// <param name="awsSecretAccessKey">AWS Secret Access Key</param>
        /// <param name="applicationName">Application Name</param>
        /// <param name="applicationVersion">Application Version</param>
        /// <param name="applicationVersion">Custom Configuration (User-Agent not set)</param>
        public FBAInventoryServiceMWSClient(
            String applicationName,
            String applicationVersion,
            String awsAccessKeyId, 
            String awsSecretAccessKey,
            FBAInventoryServiceMWSConfig config)
        {
            this.awsAccessKeyId = awsAccessKeyId;
            this.awsSecretAccessKey = awsSecretAccessKey;
            this.config = config;

            buildUserAgentHeader(applicationName, applicationVersion, config);
        }

        private const string MWS_CLIENT_VERSION = "2010-10-01";

        private void buildUserAgentHeader(
            string applicationName,
            string applicationVersion,
            FBAInventoryServiceMWSConfig config)
        {
            config.SetUserAgentHeader(
                applicationName,
                applicationVersion,
                "C#",
                "CLI", Environment.Version.ToString(),
                "Platform", Environment.OSVersion.Platform + "/" + Environment.OSVersion.Version,
                "MWSClientVersion", MWS_CLIENT_VERSION );
        }
        
        // Public API ------------------------------------------------------------//

        
        /// <summary>
        /// List Inventory Supply By Next Token 
        /// </summary>
        /// <param name="request">List Inventory Supply By Next Token  request</param>
        /// <returns>List Inventory Supply By Next Token  Response from the service</returns>
        /// <remarks>
        /// Continues pagination over a resultset of inventory data for inventory
        /// items.
        /// 
        /// This operation is used in conjunction with ListUpdatedInventorySupply.
        /// Please refer to documentation for that operation for further details.
        /// 
        /// </remarks>
        public ListInventorySupplyByNextTokenResponse ListInventorySupplyByNextToken(ListInventorySupplyByNextTokenRequest request)
        {
            return Invoke<ListInventorySupplyByNextTokenResponse>(ConvertListInventorySupplyByNextToken(request));
        }

        
        /// <summary>
        /// List Inventory Supply 
        /// </summary>
        /// <param name="request">List Inventory Supply  request</param>
        /// <returns>List Inventory Supply  Response from the service</returns>
        /// <remarks>
        /// Get information about the supply of seller-owned inventory in
        /// Amazon's fulfillment network. "Supply" is inventory that is available
        /// for fulfilling (a.k.a. Multi-Channel Fulfillment) orders. In general
        /// this includes all sellable inventory that has been received by Amazon,
        /// that is not reserved for existing orders or for internal FC processes,
        /// and also inventory expected to be received from inbound shipments.
        /// This operation provides 2 typical usages by setting different
        /// ListInventorySupplyRequest value:
        /// 
        /// 1. Set value to SellerSkus and not set value to QueryStartDateTime,
        /// this operation will return all sellable inventory that has been received
        /// by Amazon's fulfillment network for these SellerSkus.
        /// 2. Not set value to SellerSkus and set value to QueryStartDateTime,
        /// This operation will return information about the supply of all seller-owned
        /// inventory in Amazon's fulfillment network, for inventory items that may have had
        /// recent changes in inventory levels. It provides the most efficient mechanism
        /// for clients to maintain local copies of inventory supply data.
        /// Only 1 of these 2 parameters (SellerSkus and QueryStartDateTime) can be set value for 1 request.
        /// If both with values or neither with values, an exception will be thrown.
        /// This operation is used with ListInventorySupplyByNextToken
        /// to paginate over the resultset. Begin pagination by invoking the
        /// ListInventorySupply operation, and retrieve the first set of
        /// results. If more results are available,continuing iteratively requesting further
        /// pages results by invoking the ListInventorySupplyByNextToken operation (each time
        /// passing in the NextToken value from the previous result), until the returned NextToken
        /// is null, indicating no further results are available.
        /// 
        /// </remarks>
        public ListInventorySupplyResponse ListInventorySupply(ListInventorySupplyRequest request)
        {
            return Invoke<ListInventorySupplyResponse>(ConvertListInventorySupply(request));
        }

        
        /// <summary>
        /// Get Service Status 
        /// </summary>
        /// <param name="request">Get Service Status  request</param>
        /// <returns>Get Service Status  Response from the service</returns>
        /// <remarks>
        /// Gets the status of the service.
        /// Status is one of GREEN, RED representing:
        /// GREEN: This API section of the service is operating normally.
        /// RED: The service is disrupted.
        /// 
        /// </remarks>
        public GetServiceStatusResponse GetServiceStatus(GetServiceStatusRequest request)
        {
            return Invoke<GetServiceStatusResponse>(ConvertGetServiceStatus(request));
        }

        // Private API ------------------------------------------------------------//

        /**
         * Configure HttpClient with set of defaults as well as configuration
         * from FBAInventoryServiceMWSConfig instance
         */
        private HttpWebRequest ConfigureWebRequest(int contentLength)
        {
            HttpWebRequest request = WebRequest.Create(config.ServiceURL) as HttpWebRequest;

            if (config.IsSetProxyHost())
            {
                request.Proxy = new WebProxy(config.ProxyHost, config.ProxyPort);
            }
            //request.UserAgent = config.UserAgent;
            request.Headers.Add("X-Amazon-User-Agent", config.UserAgent);
            request.Method = "POST";
            request.Timeout = 100000;
            request.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
            request.ContentLength = contentLength;

            return request;
        }

        /**
         * Invoke request and return response
         */
        private T Invoke<T>(IDictionary<String, String> parameters)
        {
            String actionName = parameters["Action"];
            T response = default(T);
            String responseBody = null;
            HttpStatusCode statusCode = default(HttpStatusCode);

            // Verify service URL is set.
            if (String.IsNullOrEmpty(config.ServiceURL))
            {
                throw new FBAInventoryServiceMWSException(new ArgumentException(
                    "Missing serviceUrl configuration value. You may obtain a list of valid MWS URLs by consulting the MWS Developer's Guide, or reviewing the sample code published along side this library."));      
            }

            /* Add required request parameters */
            AddRequiredParameters(parameters);

            String queryString = GetParametersAsString(parameters);

            byte[] requestData = new UTF8Encoding().GetBytes(queryString);
            bool shouldRetry = true;
            int retries = 0;
            do
            {
                HttpWebRequest request = ConfigureWebRequest(requestData.Length);
                /* Submit the request and read response body */
                try
                {
                    using (Stream requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(requestData, 0, requestData.Length);
                    }
                    using (HttpWebResponse httpResponse = request.GetResponse() as HttpWebResponse)
                    {
                        statusCode = httpResponse.StatusCode;
                        StreamReader reader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8);
                        responseBody = reader.ReadToEnd();
                    }
                    /* Attempt to deserialize response into <Action> Response type */
                    XmlSerializer serlizer = new XmlSerializer(typeof(T));
                    response = (T)serlizer.Deserialize(new StringReader(responseBody));
                    shouldRetry = false;
                }
                /* Web exception is thrown on unsucessful responses */
                catch (WebException we)
                {
                    shouldRetry = false;
                    using (HttpWebResponse httpErrorResponse = (HttpWebResponse)we.Response as HttpWebResponse)
                    {
                        if (httpErrorResponse == null)
                        {
                            throw new FBAInventoryServiceMWSException(we);
                        }
                        statusCode = httpErrorResponse.StatusCode;
                        StreamReader reader = new StreamReader(httpErrorResponse.GetResponseStream(), Encoding.UTF8);
                        responseBody = reader.ReadToEnd();
                    }

                    /* Attempt to deserialize response into ErrorResponse type */
                    try
                    {
                        XmlSerializer serlizer = new XmlSerializer(typeof(ErrorResponse));
                        ErrorResponse errorResponse = (ErrorResponse)serlizer.Deserialize(new StringReader(responseBody));
                        Error error = errorResponse.Error[0];

                        bool retriableError = (statusCode == HttpStatusCode.InternalServerError || statusCode == HttpStatusCode.ServiceUnavailable);
                        retriableError = retriableError && error.Code != "RequestThrottled";
    
                        if (retriableError && retries < config.MaxErrorRetry)
                        {
                            PauseOnRetry(++retries);
                            shouldRetry = true;
                            continue;
                        }
                        else
                        {
                            shouldRetry = false;
                        }
                        /* Throw formatted exception with information available from the error response */
                        throw new FBAInventoryServiceMWSException(
                            error.Message,
                            statusCode,
                            error.Code,
                            error.Type,
                            errorResponse.RequestId,
                            errorResponse.ToXML());
                    }
                    /* Rethrow on deserializer error */
                    catch (Exception e)
                    {
                        if (e is FBAInventoryServiceMWSException)
                        {
                            throw e;
                        }
                        else
                        {
                            FBAInventoryServiceMWSException se = ReportAnyErrors(responseBody, statusCode, e);
                            throw se;
                        }
                    }
                }

                /* Catch other exceptions, attempt to convert to formatted exception,
                 * else rethrow wrapped exception */
                catch (Exception e)
                {
                    throw new FBAInventoryServiceMWSException(e);
                }
            } while (shouldRetry);

            return response;
        }


        /**
         * Look for additional error strings in the response and return formatted exception
         */
        private FBAInventoryServiceMWSException ReportAnyErrors(String responseBody, HttpStatusCode status, Exception e)
        {

            FBAInventoryServiceMWSException ex = null;

            if (responseBody != null && responseBody.StartsWith("<"))
            {
                Match errorMatcherOne = Regex.Match(responseBody, "<RequestId>(.*)</RequestId>.*<Error>" +
                        "<Code>(.*)</Code><Message>(.*)</Message></Error>.*(<Error>)?", RegexOptions.Multiline);
                Match errorMatcherTwo = Regex.Match(responseBody, "<Error><Code>(.*)</Code><Message>(.*)" +
                        "</Message></Error>.*(<Error>)?.*<RequestID>(.*)</RequestID>", RegexOptions.Multiline);

                if (errorMatcherOne.Success)
                {
                    String requestId = errorMatcherOne.Groups[1].Value;
                    String code = errorMatcherOne.Groups[2].Value;
                    String message = errorMatcherOne.Groups[3].Value;

                    ex = new FBAInventoryServiceMWSException(message, status, code, "Unknown", requestId, responseBody);

                }
                else if (errorMatcherTwo.Success)
                {
                    String code = errorMatcherTwo.Groups[1].Value;
                    String message = errorMatcherTwo.Groups[2].Value;
                    String requestId = errorMatcherTwo.Groups[4].Value;

                    ex = new FBAInventoryServiceMWSException(message, status, code, "Unknown", requestId, responseBody);
                }
                else
                {
                    ex = new FBAInventoryServiceMWSException("Internal Error", status);
                }
            }
            else
            {
                ex = new FBAInventoryServiceMWSException("Internal Error", status);
            }
            return ex;
        }

        /**
         * Exponential sleep on failed request
         */
        private void PauseOnRetry(int retries)
        {
            int delay = (int)Math.Pow(4, retries) * 100;
            System.Threading.Thread.Sleep(delay);
        }

        /**
         * Add authentication related and version parameters
         */
        private void AddRequiredParameters(IDictionary<String, String> parameters)
        {
            parameters.Add("AWSAccessKeyId", this.awsAccessKeyId);
            parameters.Add("Timestamp", GetFormattedTimestamp(DateTime.Now));
            parameters.Add("Version", config.ServiceVersion);
            parameters.Add("SignatureVersion", config.SignatureVersion);
            parameters.Add("Signature", SignParameters(parameters, this.awsSecretAccessKey));
        }

        /**
         * Convert Dictionary of paremeters to Url encoded query string
         */
        private string GetParametersAsString(IDictionary<String, String> parameters)
        {
            StringBuilder data = new StringBuilder();
            foreach (String key in (IEnumerable<String>)parameters.Keys)
            {
                String value = parameters[key];
                if (value != null && value != "")
                {
                    data.Append(key);
                    data.Append('=');
                    data.Append(UrlEncode(value, false));
                    data.Append('&');
                }
            }
            String result = data.ToString();
            return result.Remove(result.Length - 1);
        }

        /**
         * Computes RFC 2104-compliant HMAC signature for request parameters
         * Implements AWS Signature, as per following spec:
         *
         * Sorts all  parameters (including SignatureVersion and excluding Signature,
         * the value of which is being created), ignoring case.
         *
         * Iterate over the sorted list and append the parameter name (in original case)
         * and then its value. It will not URL-encode the parameter values before
         * constructing this string. There are no separators.
         *
         * Signature Version 0: This is not supported in the MWS.
         *
         * Signature Version 1: This is not supported in the MWS.
         *
         * Signature Version is 2, string to sign is based on following:
         *
         *    1. The HTTP Request Method followed by an ASCII newline (%0A)
         *    2. The HTTP Host header in the form of lowercase host, followed by an ASCII newline.
         *    3. The URL encoded HTTP absolute path component of the URI
         *       (up to but not including the query string parameters);
         *       if this is empty use a forward '/'. This parameter is followed by an ASCII newline.
         *    4. The concatenation of all query string components (names and values)
         *       as UTF-8 characters which are URL encoded as per RFC 3986
         *       (hex characters MUST be uppercase), sorted using lexicographic byte ordering.
         *       Parameter names are separated from their values by the '=' character
         *       (ASCII character 61), even if the value is empty.
         *       Pairs of parameter and values are separated by the '&' character (ASCII code 38).
         *
         */
        private String SignParameters(IDictionary<String, String> parameters, String key)
        {
            String signatureVersion = parameters["SignatureVersion"];

            KeyedHashAlgorithm algorithm = new HMACSHA1();

            String stringToSign = null;
            if ("2".Equals(signatureVersion))
            {
                String signatureMethod = config.SignatureMethod;
                algorithm = KeyedHashAlgorithm.Create(signatureMethod.ToUpper());
                parameters.Add("SignatureMethod", signatureMethod);
                stringToSign = CalculateStringToSignV2(parameters);
            }
            else
            {
                throw new Exception("Invalid Signature Version specified");
            }

            return Sign(stringToSign, key, algorithm);
        }

        private String CalculateStringToSignV2(IDictionary<String, String> parameters)
        {
            StringBuilder data = new StringBuilder();
            IDictionary<String, String> sorted =
                  new SortedDictionary<String, String>(parameters, StringComparer.Ordinal);
            data.Append("POST");
            data.Append("\n");
            Uri endpoint = new Uri(config.ServiceURL);

            data.Append(endpoint.Host);
            if (endpoint.Port > 0) 
            {
                // data.Append(":" + endpoint.Port);
            }
            data.Append("\n");
            String uri = endpoint.AbsolutePath;
            if (uri == null || uri.Length == 0)
            {
                uri = "/";
            }
            data.Append(UrlEncode(uri, true));
            data.Append("\n");
            foreach (KeyValuePair<String, String> pair in sorted)
            {
                if (pair.Value != null && pair.Value != "")
                {
                    data.Append(UrlEncode(pair.Key, false));
                    data.Append("=");
                    data.Append(UrlEncode(pair.Value, false));
                    data.Append("&");
                }

            }

            String result = data.ToString();
            return result.Remove(result.Length - 1);
        }

        private String UrlEncode(String data, bool path)
        {
            StringBuilder encoded = new StringBuilder();
            String unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~" + (path ? "/" : "");

            foreach (char symbol in System.Text.Encoding.UTF8.GetBytes(data))
            {
                if (unreservedChars.IndexOf(symbol) != -1)
                {
                    encoded.Append(symbol);
            }
            else
            {
                    encoded.Append("%" + String.Format("{0:X2}", (int)symbol));
            }
            }

            return encoded.ToString();

        }

        /**
         * Computes RFC 2104-compliant HMAC signature.
         */
        private String Sign(String data, String key, KeyedHashAlgorithm algorithm)
        {
            Encoding encoding = new UTF8Encoding();
            algorithm.Key = encoding.GetBytes(key);
            return Convert.ToBase64String(algorithm.ComputeHash(
                encoding.GetBytes(data.ToCharArray())));
        }


        /**
         * Formats date as ISO 8601 timestamp
         */
        private String GetFormattedTimestamp(DateTime dt)
        {
            DateTime utcTime;
            if (dt.Kind == DateTimeKind.Local)
            {
                utcTime = new DateTime(
                    dt.Year,
                    dt.Month,
                    dt.Day,
                    dt.Hour,
                    dt.Minute,
                    dt.Second,
                    dt.Millisecond,
                    DateTimeKind.Local).ToUniversalTime();
            }
            else
            {
                // If DateTimeKind.Unspecified, assume UTC.
                utcTime = dt;

            }
            
            return utcTime.ToString("yyyy-MM-dd\\THH:mm:ss.fff\\Z", CultureInfo.InvariantCulture);
        }

                                                
        /**
         * Convert ListInventorySupplyByNextTokenRequest to name value pairs
         */
        private IDictionary<String, String> ConvertListInventorySupplyByNextToken(ListInventorySupplyByNextTokenRequest request)
        {
            
            IDictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("Action", "ListInventorySupplyByNextToken");
            if (request.IsSetSellerId())
            {
                parameters.Add("SellerId", request.SellerId);
            }
            if (request.IsSetMarketplace())
            {
                parameters.Add("Marketplace", request.Marketplace);
            }
            if (request.IsSetNextToken())
            {
                parameters.Add("NextToken", request.NextToken);
            }

            return parameters;
        }
        
                                                
        /**
         * Convert ListInventorySupplyRequest to name value pairs
         */
        private IDictionary<String, String> ConvertListInventorySupply(ListInventorySupplyRequest request)
        {
            
            IDictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("Action", "ListInventorySupply");
            if (request.IsSetSellerId())
            {
                parameters.Add("SellerId", request.SellerId);
            }
            if (request.IsSetMarketplace())
            {
                parameters.Add("Marketplace", request.Marketplace);
            }
            if (request.IsSetSellerSkus())
            {
                SellerSkuList  listInventorySupplyRequestSellerSkus = request.SellerSkus;
                List<String> sellerSkusmemberList  =  listInventorySupplyRequestSellerSkus.member;
                int sellerSkusmemberListIndex = 1;
                foreach  (String sellerSkusmember in sellerSkusmemberList)
                {
                    parameters.Add("SellerSkus" + "." + "member" + "."  + sellerSkusmemberListIndex, sellerSkusmember);
                    sellerSkusmemberListIndex++;
                }
            }
            if (request.IsSetQueryStartDateTime())
            {
                parameters.Add("QueryStartDateTime", GetFormattedTimestamp(request.QueryStartDateTime) + "");
            }
            if (request.IsSetResponseGroup())
            {
                parameters.Add("ResponseGroup", request.ResponseGroup);
            }

            return parameters;
        }
        
                                                
        /**
         * Convert GetServiceStatusRequest to name value pairs
         */
        private IDictionary<String, String> ConvertGetServiceStatus(GetServiceStatusRequest request)
        {
            
            IDictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("Action", "GetServiceStatus");
            if (request.IsSetSellerId())
            {
                parameters.Add("SellerId", request.SellerId);
            }
            if (request.IsSetMarketplace())
            {
                parameters.Add("Marketplace", request.Marketplace);
            }

            return parameters;
        }
        
                                                                        

    }
}