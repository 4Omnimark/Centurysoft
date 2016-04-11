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
using FBAInventoryServiceMWS.Mock;
using FBAInventoryServiceMWS.Model;

namespace FBAInventoryServiceMWS.Samples
{

    /// <summary>
    /// FBA Inventory Service MWS  Samples
    /// </summary>
    public class FBAInventoryServiceMWSSamples 
    {
    
       /**
        * Samples for FBA Inventory Service MWS functionality
        */
        public static void Main(string [] args) 
        {
            Console.WriteLine("===========================================");
            Console.WriteLine("Welcome to FBA Inventory Service MWS Samples!");
            Console.WriteLine("===========================================");

            Console.WriteLine("To get started:");
            Console.WriteLine("===========================================");
            Console.WriteLine("  - Fill in your MWS credentials");
            Console.WriteLine("  - Uncomment sample you're interested in trying");
            Console.WriteLine("  - Set request with desired parameters");
            Console.WriteLine("  - Hit F5 to run!");
            Console.WriteLine();

            Console.WriteLine("===========================================");
            Console.WriteLine("Samples Output");
            Console.WriteLine("===========================================");
            Console.WriteLine();

           /************************************************************************
            * Access Key ID and Secret Acess Key ID, obtained from:
            * http://mws.amazon.com
            ***********************************************************************/
            String accessKeyId = "<Your Access Key ID>";
            String secretAccessKey = "<Your Secret Access Key>";

            /************************************************************************
             * Marketplace and Seller IDs are required parameters for all 
             * MWS calls.
             ***********************************************************************/
            const string marketplaceId = "<Your Marketplace ID>";
            const string sellerId = "<Your Seller ID>";

            /************************************************************************
             * The application name and version are included in each MWS call's
             * HTTP User-Agent field. These are required fields.
             ***********************************************************************/
            const string applicationName = "<Your Application Name>";
            const string applicationVersion = "<Your Application Version or Build Number or Release Date>";

            /************************************************************************
            * Uncomment to try advanced configuration options. Available options are:
            *
            *  - Proxy Host and Proxy Port
            *  - MWS Service endpoint URL
            *  - User Agent String to be sent to FBA Inventory Service MWS  service
            *
            ***********************************************************************/
            FBAInventoryServiceMWSConfig config = new FBAInventoryServiceMWSConfig();
            //config.ProxyHost = "https://PROXY_URL";
            //config.ProxyPort = 9090;
            //
            // IMPORTANT: Uncomment out the appropiate line for the country you wish 
            // to sell in:
            // 
            // US
            // config.ServiceURL = "https://mws.amazonservices.com/FulfillmentInventory/2010-10-01/";
            // UK
            // config.ServiceURL = "https://mws.amazonservices.co.uk/FulfillmentInventory/2010-10-01/";
            // Germany
            // config.ServiceURL = "https://mws.amazonservices.de/FulfillmentInventory/2010-10-01/";
            // France
            // config.ServiceURL = "https://mws.amazonservices.fr/FulfillmentInventory/2010-10-01/";
            // Japan
            // config.ServiceURL = "https://mws.amazonservices.jp/FulfillmentInventory/2010-10-01/";
            // China
            // config.ServiceURL = "https://mws.amazonservices.com.cn/FulfillmentInventory/2010-10-01/";

            // ProxyPort=-1 ; MaxErrorRetry=3
            config.SetUserAgentHeader(
                applicationName,
                applicationVersion,
                "C#",
                "-1", "3");

            /************************************************************************
            * Instantiate Implementation of FBA Inventory Service MWS 
            ***********************************************************************/
            FBAInventoryServiceMWS service =
                new FBAInventoryServiceMWSClient(
                    accessKeyId,
                    secretAccessKey,
                    applicationName,
                    applicationVersion,
                    config);

            // FBAInventoryServiceMWS service = new FBAInventoryServiceMWSClient(accessKeyId, secretAccessKey, config);

            /************************************************************************
            * Uncomment to try out Mock Service that simulates FBA Inventory Service MWS 
            * responses without calling FBA Inventory Service MWS  service.
            *
            * Responses are loaded from local XML files. You can tweak XML files to
            * experiment with various outputs during development
            *
            * XML files available under FBAInventoryServiceMWS\Mock tree
            *
            ***********************************************************************/
            // FBAInventoryServiceMWS service = new FBAInventoryServiceMWSMock();

            /************************************************************************
            * Uncomment to invoke List Inventory Supply By Next Token Action
            ***********************************************************************/
            // ListInventorySupplyByNextTokenRequest request = new ListInventorySupplyByNextTokenRequest();
            // @TODO: set request parameters here
            // request.SellerId = sellerId;
            // request.Marketplace = marketplaceId;

            // ListInventorySupplyByNextTokenSample.InvokeListInventorySupplyByNextToken(service, request);
            /************************************************************************
            * Uncomment to invoke List Inventory Supply Action
            ***********************************************************************/
            // ListInventorySupplyRequest request = new ListInventorySupplyRequest();
            // @TODO: set request parameters here
            // request.SellerId = sellerId;
            // request.Marketplace = marketplaceId;

            // ListInventorySupplySample.InvokeListInventorySupply(service, request);
            /************************************************************************
            * Uncomment to invoke Get Service Status Action
            ***********************************************************************/
            // GetServiceStatusRequest request = new GetServiceStatusRequest();
            // @TODO: set request parameters here
            // request.SellerId = sellerId;
            // request.Marketplace = marketplaceId;

            // GetServiceStatusSample.InvokeGetServiceStatus(service, request);
            Console.WriteLine();
            Console.WriteLine("===========================================");
            Console.WriteLine("End of output. You can close this window");
            Console.WriteLine("===========================================");

            System.Threading.Thread.Sleep(50000);
        }

    }
}
