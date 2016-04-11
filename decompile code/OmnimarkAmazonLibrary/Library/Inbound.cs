using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FBAInboundServiceMWS;
using FBAInboundServiceMWS.Model;
using OmnimarkAmazon.Models;
using System.Threading;

namespace OmnimarkAmazon
{
    public static partial class Library
    {

        public enum FBAInboundShipmentStatus
        {
            WORKING, SHIPPED, IN_TRANSIT, DELIVERED, CHECKED_IN, RECEIVING, CLOSED, CANCELLED, DELETED, ERROR
        }

        public static List<InboundShipmentInfo> GetInboundShipments(List<Throttler> Throttlers, AmazonAccount AmazonAccount, DateTime? LastUpdatedAfter, DateTime? LastUpdatedBefore, IEnumerable<string> ShipmentIDList, Action<bool, string> Log)
        {
            var service = GetAmazonService<FBAInboundServiceMWSClient>(AmazonAccount);

            ListInboundShipmentsRequest request = new ListInboundShipmentsRequest();

            if (LastUpdatedAfter != null)
            {
                request.LastUpdatedAfter = (DateTime)LastUpdatedAfter;
                request.LastUpdatedBefore = (DateTime)LastUpdatedBefore;
            }

            if (ShipmentIDList != null)
            {
                request.ShipmentIdList = new ShipmentIdList();
                foreach (string sid in ShipmentIDList)
                    request.ShipmentIdList.member.Add(sid);
            }

            request.SellerId = AmazonAccount.MerchantID;
            request.ShipmentStatusList = new ShipmentStatusList();

            foreach (var sts in Startbutton.Library.GetEnumValues<FBAInboundShipmentStatus>())
                request.ShipmentStatusList.member.Add(sts.ToString());

            ListInboundShipmentsResponse response;

            if (Log != null)
                Log(false, "Retreiving Inbound Shipments ");

            if (Throttlers != null)
                foreach (Throttler Throttler in Throttlers)
                    Throttler.DoWait(Log);

            response = service.ListInboundShipments(request);

            ListInboundShipmentsResult ListInboundShipmentsResult = response.ListInboundShipmentsResult;

            List<InboundShipmentInfo> rtn = new List<InboundShipmentInfo>();

            bool ResultIsShipmentData = ListInboundShipmentsResult.IsSetShipmentData();
            bool ResultIsSetNext = ListInboundShipmentsResult.IsSetNextToken();
            List<InboundShipmentInfo> ShipmentList = ListInboundShipmentsResult.ShipmentData.member;
            string NextToken = ListInboundShipmentsResult.NextToken;

            while (true)
            {
                if (ResultIsShipmentData)
                {
                    if (Log != null)
                        Log(true, "Got " + ShipmentList.Count.ToString());

                    foreach (InboundShipmentInfo o in ShipmentList)
                        rtn.Add(o);
                }
                else
                    if (Log != null)
                        Log(true, "Got none");

                if (!ResultIsSetNext)
                    break;

                ListInboundShipmentsByNextTokenRequest request2 = new ListInboundShipmentsByNextTokenRequest();

                request2.SellerId = AmazonAccount.MerchantID;
                request2.NextToken = NextToken;

                if (Log != null)
                    Log(false, "Retreiving More Inbound Shipments ");

                ListInboundShipmentsByNextTokenResponse response2 = null;

                foreach (Throttler Throttler in Throttlers)
                    if (Throttler != null)
                        Throttler.DoWait(Log);

                bool ThrottlerSlowed = false;

                while (response2 == null)
                {
                    try
                    {
                        response2 = service.ListInboundShipmentsByNextToken(request2);
                    }
                    catch (Exception Ex)
                    {
                        if (Ex.Message.StartsWith("Request is throttled"))
                        {
                            if (Log != null)
                                Log(false, "Throttled...");

                            foreach (Throttler Throttler in Throttlers)
                                if (Throttler != null)
                                {
                                    Throttler.LastRequest = DateTime.Now;

                                    if (!ThrottlerSlowed)
                                        Throttler.MillisecondsBetweenRequests = (int)((double)Throttler.MillisecondsBetweenRequests * 1.1);
                                }

                            ThrottlerSlowed = true;

                            Thread.Sleep(1000);
                        }
                    }
                }

                ListInboundShipmentsByNextTokenResult ListInboundShipmentsResult2 = response2.ListInboundShipmentsByNextTokenResult;

                ResultIsShipmentData = ListInboundShipmentsResult2.IsSetShipmentData();
                ResultIsSetNext = ListInboundShipmentsResult2.IsSetNextToken();
                ShipmentList = ListInboundShipmentsResult2.ShipmentData.member;
                NextToken = ListInboundShipmentsResult2.NextToken;

            }


            return rtn;

        }

        public static List<InboundShipmentItem> GetInboundShipmentItems(List<Throttler> Throttlers, AmazonAccount AmazonAccount, string ShipmentID, Action<bool, string> Log)
        {
            var service = GetAmazonService<FBAInboundServiceMWSClient>(AmazonAccount);
            DateTime LastUpdatedAfter = DateTime.Now.AddMonths(-6);
            DateTime LastUpdatedBefore = DateTime.Now;

            ListInboundShipmentItemsRequest request = new ListInboundShipmentItemsRequest();
            request.SellerId = AmazonAccount.MerchantID;
            request.ShipmentId = ShipmentID;

            ListInboundShipmentItemsResponse response;

            if (Log != null)
                Log(false, "Retreiving Inbound Shipment Items for " + ShipmentID + " ");

            if (Throttlers != null)
                foreach (Throttler Throttler in Throttlers)
                    Throttler.DoWait(Log);

            response = service.ListInboundShipmentItems(request);

            ListInboundShipmentItemsResult ListInboundShipmentItemsResult = response.ListInboundShipmentItemsResult;

            List<InboundShipmentItem> rtn = new List<InboundShipmentItem>();

            bool ResultIsShipmentData = ListInboundShipmentItemsResult.IsSetItemData();
            bool ResultIsSetNext = ListInboundShipmentItemsResult.IsSetNextToken();
            List<InboundShipmentItem> ShipmentList = ListInboundShipmentItemsResult.ItemData.member;
            string NextToken = ListInboundShipmentItemsResult.NextToken;

            while (true)
            {
                if (ResultIsShipmentData)
                {
                    if (Log != null)
                        Log(true, "Got " + ShipmentList.Count.ToString());

                    foreach (InboundShipmentItem o in ShipmentList)
                        rtn.Add(o);
                }
                else
                    if (Log != null)
                        Log(true, "Got none");

                if (!ResultIsSetNext)
                    break;

                ListInboundShipmentItemsByNextTokenRequest request2 = new ListInboundShipmentItemsByNextTokenRequest();

                request2.SellerId = AmazonAccount.MerchantID;
                request2.NextToken = NextToken;

                if (Log != null)
                    Log(false, "Retreiving More Inbound Shipment Items ");

                ListInboundShipmentItemsByNextTokenResponse response2 = null;

                foreach (Throttler Throttler in Throttlers)
                    if (Throttler != null)
                        Throttler.DoWait(Log);

                bool ThrottlerSlowed = false;

                while (response2 == null)
                {
                    try
                    {
                        response2 = service.ListInboundShipmentItemsByNextToken(request2);
                    }
                    catch (Exception Ex)
                    {
                        if (Ex.Message.StartsWith("Request is throttled"))
                        {
                            if (Log != null)
                                Log(false, "Throttled...");

                            foreach (Throttler Throttler in Throttlers)
                                if (Throttler != null)
                                {
                                    Throttler.LastRequest = DateTime.Now;

                                    if (!ThrottlerSlowed)
                                        Throttler.MillisecondsBetweenRequests = (int)((double)Throttler.MillisecondsBetweenRequests * 1.1);
                                }

                            ThrottlerSlowed = true;

                            Thread.Sleep(1000);
                        }
                    }
                }

                ListInboundShipmentItemsByNextTokenResult ListInboundShipmentItemsResult2 = response2.ListInboundShipmentItemsByNextTokenResult;

                ResultIsShipmentData = ListInboundShipmentItemsResult2.IsSetItemData();
                ResultIsSetNext = ListInboundShipmentItemsResult2.IsSetNextToken();
                ShipmentList = ListInboundShipmentItemsResult2.ItemData.member;
                NextToken = ListInboundShipmentItemsResult2.NextToken;

            }

            return rtn;

        }


    }
}
