/*******************************************************************************
 * Copyright 2009-2013 Amazon Services. All Rights Reserved.
 * Licensed under the Apache License, Version 2.0 (the "License"); 
 *
 * You may not use this file except in compliance with the License. 
 * You may obtain a copy of the License at: http://aws.amazon.com/apache2.0
 * This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
 * CONDITIONS OF ANY KIND, either express or implied. See the License for the 
 * specific language governing permissions and limitations under the License.
 *******************************************************************************
 * FBA Inbound Service MWS
 * API Version: 2010-10-01
 * Library Version: 2013-11-01
 * Generated: Fri Nov 08 22:04:54 GMT 2013
 */

using FBAInboundServiceMWS.Model;
using System;
using System.Collections.Generic;

namespace FBAInboundServiceMWS {

    /// <summary>
    /// Runnable sample code to demonstrate usage of the C# client.
    ///
    /// To use, import the client source as a console application,
    /// and mark this class as the startup object. Then, replace
    /// parameters below with sensible values and run.
    /// </summary>
    public class FBAInboundServiceMWSSample {

        public static void Main(string[] args)
        {
            // TODO: Set the below configuration variables before attempting to run

            // Developer AWS access key
            string accessKey = "replaceWithAccessKey";

            // Developer AWS secret key
            string secretKey = "replaceWithSecretKey";

            // The client application name
            string appName = "CSharpSampleCode";

            // The client application version
            string appVersion = "1.0";

            // The endpoint for region service and version (see developer guide)
            // ex: https://mws.amazonservices.com
            string serviceURL = "replaceWithServiceURL";

            // Create a configuration object
            FBAInboundServiceMWSConfig config = new FBAInboundServiceMWSConfig();
            config.ServiceURL = serviceURL;
            // Set other client connection configurations here if needed
            // Create the client itself
            FBAInboundServiceMWS client = new FBAInboundServiceMWSClient(accessKey, secretKey, appName, appVersion, config);

            FBAInboundServiceMWSSample sample = new FBAInboundServiceMWSSample(client);

            // Uncomment the operation you'd like to test here
            // TODO: Modify the request created in the Invoke method to be valid

            try 
            {
                IMWSResponse response = null;
                // response = sample.InvokeConfirmTransportRequest();
                // response = sample.InvokeCreateInboundShipment();
                // response = sample.InvokeCreateInboundShipmentPlan();
                // response = sample.InvokeEstimateTransportRequest();
                // response = sample.InvokeGetBillOfLading();
                // response = sample.InvokeGetPackageLabels();
                // response = sample.InvokeGetServiceStatus();
                // response = sample.InvokeGetTransportContent();
                // response = sample.InvokeListInboundShipmentItems();
                // response = sample.InvokeListInboundShipmentItemsByNextToken();
                // response = sample.InvokeListInboundShipments();
                // response = sample.InvokeListInboundShipmentsByNextToken();
                // response = sample.InvokePutTransportContent();
                // response = sample.InvokeUpdateInboundShipment();
                // response = sample.InvokeVoidTransportRequest();
                Console.WriteLine("Response:");
                ResponseHeaderMetadata rhmd = response.ResponseHeaderMetadata;
                // We recommend logging the request id and timestamp of every call.
                Console.WriteLine("RequestId: " + rhmd.RequestId);
                Console.WriteLine("Timestamp: " + rhmd.Timestamp);
                string responseXml = response.ToXML();
                Console.WriteLine(responseXml);
            }
            catch (FBAInboundServiceMWSException ex)
            {
                // Exception properties are important for diagnostics.
                ResponseHeaderMetadata rhmd = ex.ResponseHeaderMetadata;
                Console.WriteLine("Service Exception:");
                if(rhmd != null)
                {
                    Console.WriteLine("RequestId: " + rhmd.RequestId);
                    Console.WriteLine("Timestamp: " + rhmd.Timestamp);
                }
                Console.WriteLine("Message: " + ex.Message);
                Console.WriteLine("StatusCode: " + ex.StatusCode);
                Console.WriteLine("ErrorCode: " + ex.ErrorCode);
                Console.WriteLine("ErrorType: " + ex.ErrorType);
                throw ex;
            }
        }

        private readonly FBAInboundServiceMWS client;

        public FBAInboundServiceMWSSample(FBAInboundServiceMWS client)
        {
            this.client = client;
        }

        public ConfirmTransportRequestResponse InvokeConfirmTransportRequest()
        {
            // Create a request.
            ConfirmTransportInputRequest request = new ConfirmTransportInputRequest();
            string sellerId = "example";
            request.SellerId = sellerId;
            string shipmentId = "example";
            request.ShipmentId = shipmentId;
            return this.client.ConfirmTransportRequest(request);
        }

        public CreateInboundShipmentResponse InvokeCreateInboundShipment()
        {
            // Create a request.
            CreateInboundShipmentRequest request = new CreateInboundShipmentRequest();
            string sellerId = "example";
            request.SellerId = sellerId;
            string marketplace = "example";
            request.Marketplace = marketplace;
            string shipmentId = "example";
            request.ShipmentId = shipmentId;
            InboundShipmentHeader inboundShipmentHeader = new InboundShipmentHeader();
            request.InboundShipmentHeader = inboundShipmentHeader;
            InboundShipmentItemList inboundShipmentItems = new InboundShipmentItemList();
            request.InboundShipmentItems = inboundShipmentItems;
            return this.client.CreateInboundShipment(request);
        }

        public CreateInboundShipmentPlanResponse InvokeCreateInboundShipmentPlan()
        {
            // Create a request.
            CreateInboundShipmentPlanRequest request = new CreateInboundShipmentPlanRequest();
            string sellerId = "example";
            request.SellerId = sellerId;
            string marketplace = "example";
            request.Marketplace = marketplace;
            Address shipFromAddress = new Address();
            request.ShipFromAddress = shipFromAddress;
            string labelPrepPreference = "example";
            request.LabelPrepPreference = labelPrepPreference;
            string shipToCountryCode = "example";
            request.ShipToCountryCode = shipToCountryCode;
            InboundShipmentPlanRequestItemList inboundShipmentPlanRequestItems = new InboundShipmentPlanRequestItemList();
            request.InboundShipmentPlanRequestItems = inboundShipmentPlanRequestItems;
            return this.client.CreateInboundShipmentPlan(request);
        }

        public EstimateTransportRequestResponse InvokeEstimateTransportRequest()
        {
            // Create a request.
            EstimateTransportInputRequest request = new EstimateTransportInputRequest();
            string sellerId = "example";
            request.SellerId = sellerId;
            string shipmentId = "example";
            request.ShipmentId = shipmentId;
            return this.client.EstimateTransportRequest(request);
        }

        public GetBillOfLadingResponse InvokeGetBillOfLading()
        {
            // Create a request.
            GetBillOfLadingRequest request = new GetBillOfLadingRequest();
            string sellerId = "example";
            request.SellerId = sellerId;
            string shipmentId = "example";
            request.ShipmentId = shipmentId;
            return this.client.GetBillOfLading(request);
        }

        public GetPackageLabelsResponse InvokeGetPackageLabels()
        {
            // Create a request.
            GetPackageLabelsRequest request = new GetPackageLabelsRequest();
            string sellerId = "example";
            request.SellerId = sellerId;
            string shipmentId = "example";
            request.ShipmentId = shipmentId;
            string pageType = "example";
            request.PageType = pageType;
            decimal numberOfPackages = 1;
            request.NumberOfPackages = numberOfPackages;
            return this.client.GetPackageLabels(request);
        }

        public GetServiceStatusResponse InvokeGetServiceStatus()
        {
            // Create a request.
            GetServiceStatusRequest request = new GetServiceStatusRequest();
            string sellerId = "example";
            request.SellerId = sellerId;
            string marketplace = "example";
            request.Marketplace = marketplace;
            return this.client.GetServiceStatus(request);
        }

        public GetTransportContentResponse InvokeGetTransportContent()
        {
            // Create a request.
            GetTransportContentRequest request = new GetTransportContentRequest();
            string sellerId = "example";
            request.SellerId = sellerId;
            string shipmentId = "example";
            request.ShipmentId = shipmentId;
            return this.client.GetTransportContent(request);
        }

        public ListInboundShipmentItemsResponse InvokeListInboundShipmentItems()
        {
            // Create a request.
            ListInboundShipmentItemsRequest request = new ListInboundShipmentItemsRequest();
            string sellerId = "example";
            request.SellerId = sellerId;
            string marketplace = "example";
            request.Marketplace = marketplace;
            string shipmentId = "example";
            request.ShipmentId = shipmentId;
            DateTime lastUpdatedBefore = new DateTime();
            request.LastUpdatedBefore = lastUpdatedBefore;
            DateTime lastUpdatedAfter = new DateTime();
            request.LastUpdatedAfter = lastUpdatedAfter;
            return this.client.ListInboundShipmentItems(request);
        }

        public ListInboundShipmentItemsByNextTokenResponse InvokeListInboundShipmentItemsByNextToken()
        {
            // Create a request.
            ListInboundShipmentItemsByNextTokenRequest request = new ListInboundShipmentItemsByNextTokenRequest();
            string sellerId = "example";
            request.SellerId = sellerId;
            string marketplace = "example";
            request.Marketplace = marketplace;
            string nextToken = "example";
            request.NextToken = nextToken;
            return this.client.ListInboundShipmentItemsByNextToken(request);
        }

        public ListInboundShipmentsResponse InvokeListInboundShipments()
        {
            // Create a request.
            ListInboundShipmentsRequest request = new ListInboundShipmentsRequest();
            string sellerId = "example";
            request.SellerId = sellerId;
            string marketplace = "example";
            request.Marketplace = marketplace;
            ShipmentStatusList shipmentStatusList = new ShipmentStatusList();
            request.ShipmentStatusList = shipmentStatusList;
            ShipmentIdList shipmentIdList = new ShipmentIdList();
            request.ShipmentIdList = shipmentIdList;
            DateTime lastUpdatedBefore = new DateTime();
            request.LastUpdatedBefore = lastUpdatedBefore;
            DateTime lastUpdatedAfter = new DateTime();
            request.LastUpdatedAfter = lastUpdatedAfter;
            return this.client.ListInboundShipments(request);
        }

        public ListInboundShipmentsByNextTokenResponse InvokeListInboundShipmentsByNextToken()
        {
            // Create a request.
            ListInboundShipmentsByNextTokenRequest request = new ListInboundShipmentsByNextTokenRequest();
            string sellerId = "example";
            request.SellerId = sellerId;
            string marketplace = "example";
            request.Marketplace = marketplace;
            string nextToken = "example";
            request.NextToken = nextToken;
            return this.client.ListInboundShipmentsByNextToken(request);
        }

        public PutTransportContentResponse InvokePutTransportContent()
        {
            // Create a request.
            PutTransportContentRequest request = new PutTransportContentRequest();
            string sellerId = "example";
            request.SellerId = sellerId;
            string shipmentId = "example";
            request.ShipmentId = shipmentId;
            bool isPartnered = true;
            request.IsPartnered = isPartnered;
            string shipmentType = "example";
            request.ShipmentType = shipmentType;
            TransportDetailInput transportDetails = new TransportDetailInput();
            request.TransportDetails = transportDetails;
            return this.client.PutTransportContent(request);
        }

        public UpdateInboundShipmentResponse InvokeUpdateInboundShipment()
        {
            // Create a request.
            UpdateInboundShipmentRequest request = new UpdateInboundShipmentRequest();
            string sellerId = "example";
            request.SellerId = sellerId;
            string marketplace = "example";
            request.Marketplace = marketplace;
            string shipmentId = "example";
            request.ShipmentId = shipmentId;
            InboundShipmentHeader inboundShipmentHeader = new InboundShipmentHeader();
            request.InboundShipmentHeader = inboundShipmentHeader;
            InboundShipmentItemList inboundShipmentItems = new InboundShipmentItemList();
            request.InboundShipmentItems = inboundShipmentItems;
            return this.client.UpdateInboundShipment(request);
        }

        public VoidTransportRequestResponse InvokeVoidTransportRequest()
        {
            // Create a request.
            VoidTransportInputRequest request = new VoidTransportInputRequest();
            string sellerId = "example";
            request.SellerId = sellerId;
            string shipmentId = "example";
            request.ShipmentId = shipmentId;
            return this.client.VoidTransportRequest(request);
        }


    }
}
