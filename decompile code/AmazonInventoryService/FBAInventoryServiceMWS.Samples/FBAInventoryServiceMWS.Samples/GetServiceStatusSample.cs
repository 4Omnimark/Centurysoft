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
using System.Xml.Serialization;
using System.Collections.Generic;
using FBAInventoryServiceMWS;
using FBAInventoryServiceMWS.Model;



namespace FBAInventoryServiceMWS.Samples
{

    /// <summary>
    /// Get Service Status  Samples
    /// </summary>
    public class GetServiceStatusSample
    {
    
                                 
        /// <summary>
        /// Gets the status of the service.
        /// Status is one of GREEN, RED representing:
        /// GREEN: This API section of the service is operating normally.
        /// RED: The service is disrupted.
        /// 
        /// </summary>
        /// <param name="service">Instance of FBAInventoryServiceMWS service</param>
        /// <param name="request">GetServiceStatusRequest request</param>
        public static void InvokeGetServiceStatus(FBAInventoryServiceMWS service, GetServiceStatusRequest request)
        {
            try 
            {
                GetServiceStatusResponse response = service.GetServiceStatus(request);
                
                
                Console.WriteLine ("Service Response");
                Console.WriteLine ("=============================================================================");
                Console.WriteLine ();

                Console.WriteLine("        GetServiceStatusResponse");
                if (response.IsSetGetServiceStatusResult())
                {
                    Console.WriteLine("            GetServiceStatusResult");
                    GetServiceStatusResult  getServiceStatusResult = response.GetServiceStatusResult;
                    if (getServiceStatusResult.IsSetStatus())
                    {
                        Console.WriteLine("                Status");
                        Console.WriteLine("                    {0}", getServiceStatusResult.Status);
                    }
                    if (getServiceStatusResult.IsSetTimestamp())
                    {
                        Console.WriteLine("                Timestamp");
                        Console.WriteLine("                    {0}", getServiceStatusResult.Timestamp);
                    }
                }
                if (response.IsSetResponseMetadata())
                {
                    Console.WriteLine("            ResponseMetadata");
                    ResponseMetadata  responseMetadata = response.ResponseMetadata;
                    if (responseMetadata.IsSetRequestId())
                    {
                        Console.WriteLine("                RequestId");
                        Console.WriteLine("                    {0}", responseMetadata.RequestId);
                    }
                }

            } 
            catch (FBAInventoryServiceMWSException ex) 
            {
                Console.WriteLine("Caught Exception: " + ex.Message);
                Console.WriteLine("Response Status Code: " + ex.StatusCode);
                Console.WriteLine("Error Code: " + ex.ErrorCode);
                Console.WriteLine("Error Type: " + ex.ErrorType);
                Console.WriteLine("Request ID: " + ex.RequestId);
                Console.WriteLine("XML: " + ex.XML);
            }
        }
        }
}
