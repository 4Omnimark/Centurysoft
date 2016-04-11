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
    /// List Inventory Supply  Samples
    /// </summary>
    public class ListInventorySupplySample
    {
    
                             
        /// <summary>
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
        /// </summary>
        /// <param name="service">Instance of FBAInventoryServiceMWS service</param>
        /// <param name="request">ListInventorySupplyRequest request</param>
        public static void InvokeListInventorySupply(FBAInventoryServiceMWS service, ListInventorySupplyRequest request)
        {
            try 
            {
                ListInventorySupplyResponse response = service.ListInventorySupply(request);
                
                
                Console.WriteLine ("Service Response");
                Console.WriteLine ("=============================================================================");
                Console.WriteLine ();

                Console.WriteLine("        ListInventorySupplyResponse");
                if (response.IsSetListInventorySupplyResult())
                {
                    Console.WriteLine("            ListInventorySupplyResult");
                    ListInventorySupplyResult  listInventorySupplyResult = response.ListInventorySupplyResult;
                    if (listInventorySupplyResult.IsSetInventorySupplyList())
                    {
                        Console.WriteLine("                InventorySupplyList");
                        InventorySupplyList  inventorySupplyList = listInventorySupplyResult.InventorySupplyList;
                        List<InventorySupply> memberList = inventorySupplyList.member;
                        foreach (InventorySupply member in memberList)
                        {
                            Console.WriteLine("                    member");
                            if (member.IsSetSellerSKU())
                            {
                                Console.WriteLine("                        SellerSKU");
                                Console.WriteLine("                            {0}", member.SellerSKU);
                            }
                            if (member.IsSetFNSKU())
                            {
                                Console.WriteLine("                        FNSKU");
                                Console.WriteLine("                            {0}", member.FNSKU);
                            }
                            if (member.IsSetASIN())
                            {
                                Console.WriteLine("                        ASIN");
                                Console.WriteLine("                            {0}", member.ASIN);
                            }
                            if (member.IsSetCondition())
                            {
                                Console.WriteLine("                        Condition");
                                Console.WriteLine("                            {0}", member.Condition);
                            }
                            if (member.IsSetTotalSupplyQuantity())
                            {
                                Console.WriteLine("                        TotalSupplyQuantity");
                                Console.WriteLine("                            {0}", member.TotalSupplyQuantity);
                            }
                            if (member.IsSetInStockSupplyQuantity())
                            {
                                Console.WriteLine("                        InStockSupplyQuantity");
                                Console.WriteLine("                            {0}", member.InStockSupplyQuantity);
                            }
                            if (member.IsSetEarliestAvailability())
                            {
                                Console.WriteLine("                        EarliestAvailability");
                                Timepoint  earliestAvailability = member.EarliestAvailability;
                                if (earliestAvailability.IsSetTimepointType())
                                {
                                    Console.WriteLine("                            TimepointType");
                                    Console.WriteLine("                                {0}", earliestAvailability.TimepointType);
                                }
                                if (earliestAvailability.IsSetDateTime())
                                {
                                    Console.WriteLine("                            DateTime");
                                    Console.WriteLine("                                {0}", earliestAvailability.DateTime);
                                }
                            }
                            if (member.IsSetSupplyDetail())
                            {
                                Console.WriteLine("                        SupplyDetail");
                                InventorySupplyDetailList  supplyDetail = member.SupplyDetail;
                                List<InventorySupplyDetail> member1List = supplyDetail.member;
                                foreach (InventorySupplyDetail member1 in member1List)
                                {
                                    Console.WriteLine("                            member");
                                    if (member1.IsSetQuantity())
                                    {
                                        Console.WriteLine("                                Quantity");
                                        Console.WriteLine("                                    {0}", member1.Quantity);
                                    }
                                    if (member1.IsSetSupplyType())
                                    {
                                        Console.WriteLine("                                SupplyType");
                                        Console.WriteLine("                                    {0}", member1.SupplyType);
                                    }
                                    if (member1.IsSetEarliestAvailableToPick())
                                    {
                                        Console.WriteLine("                                EarliestAvailableToPick");
                                        Timepoint  earliestAvailableToPick = member1.EarliestAvailableToPick;
                                        if (earliestAvailableToPick.IsSetTimepointType())
                                        {
                                            Console.WriteLine("                                    TimepointType");
                                            Console.WriteLine("                                        {0}", earliestAvailableToPick.TimepointType);
                                        }
                                        if (earliestAvailableToPick.IsSetDateTime())
                                        {
                                            Console.WriteLine("                                    DateTime");
                                            Console.WriteLine("                                        {0}", earliestAvailableToPick.DateTime);
                                        }
                                    }
                                    if (member1.IsSetLatestAvailableToPick())
                                    {
                                        Console.WriteLine("                                LatestAvailableToPick");
                                        Timepoint  latestAvailableToPick = member1.LatestAvailableToPick;
                                        if (latestAvailableToPick.IsSetTimepointType())
                                        {
                                            Console.WriteLine("                                    TimepointType");
                                            Console.WriteLine("                                        {0}", latestAvailableToPick.TimepointType);
                                        }
                                        if (latestAvailableToPick.IsSetDateTime())
                                        {
                                            Console.WriteLine("                                    DateTime");
                                            Console.WriteLine("                                        {0}", latestAvailableToPick.DateTime);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (listInventorySupplyResult.IsSetNextToken())
                    {
                        Console.WriteLine("                NextToken");
                        Console.WriteLine("                    {0}", listInventorySupplyResult.NextToken);
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
