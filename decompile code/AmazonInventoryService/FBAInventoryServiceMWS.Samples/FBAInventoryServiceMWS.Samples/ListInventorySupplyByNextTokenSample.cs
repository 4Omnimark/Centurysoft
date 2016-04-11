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
    /// List Inventory Supply By Next Token  Samples
    /// </summary>
    public class ListInventorySupplyByNextTokenSample
    {
    
                         
        /// <summary>
        /// Continues pagination over a resultset of inventory data for inventory
        /// items.
        /// 
        /// This operation is used in conjunction with ListUpdatedInventorySupply.
        /// Please refer to documentation for that operation for further details.
        /// 
        /// </summary>
        /// <param name="service">Instance of FBAInventoryServiceMWS service</param>
        /// <param name="request">ListInventorySupplyByNextTokenRequest request</param>
        public static void InvokeListInventorySupplyByNextToken(FBAInventoryServiceMWS service, ListInventorySupplyByNextTokenRequest request)
        {
            try 
            {
                ListInventorySupplyByNextTokenResponse response = service.ListInventorySupplyByNextToken(request);
                
                
                Console.WriteLine ("Service Response");
                Console.WriteLine ("=============================================================================");
                Console.WriteLine ();

                Console.WriteLine("        ListInventorySupplyByNextTokenResponse");
                if (response.IsSetListInventorySupplyByNextTokenResult())
                {
                    Console.WriteLine("            ListInventorySupplyByNextTokenResult");
                    ListInventorySupplyByNextTokenResult  listInventorySupplyByNextTokenResult = response.ListInventorySupplyByNextTokenResult;
                    if (listInventorySupplyByNextTokenResult.IsSetInventorySupplyList())
                    {
                        Console.WriteLine("                InventorySupplyList");
                        InventorySupplyList  inventorySupplyList = listInventorySupplyByNextTokenResult.InventorySupplyList;
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
                    if (listInventorySupplyByNextTokenResult.IsSetNextToken())
                    {
                        Console.WriteLine("                NextToken");
                        Console.WriteLine("                    {0}", listInventorySupplyByNextTokenResult.NextToken);
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
