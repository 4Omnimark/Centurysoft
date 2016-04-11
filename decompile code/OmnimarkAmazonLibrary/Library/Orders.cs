using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarketplaceWebServiceOrders;
using MarketplaceWebServiceOrders.Model;
using OmnimarkAmazon.Models;
using System.Threading;
using System.Data.Entity.Infrastructure;

namespace OmnimarkAmazon
{
    public static partial class Library
    {
        public static List<Order> GetOrders(List<Throttler> Throttlers, int AmazonAccountShortID, DateTime CreatedAfter, Action<bool, string> Log)
        {
            Entities db = new Entities();
            return GetOrders(Throttlers, db.AmazonAccounts.Single(a => a.ShortID == AmazonAccountShortID), CreatedAfter, Log);
        }

        public static List<Order> GetOrders(List<Throttler> Throttlers, Guid AmazonAccountID, DateTime CreatedAfter, Action<bool, string> Log)
        {
            Entities db = new Entities();
            return GetOrders(Throttlers, db.AmazonAccounts.Single(a => a.ID == AmazonAccountID), CreatedAfter, Log);
        }

        public static List<Order> GetOrders(List<Throttler> Throttlers, AmazonAccount AmazonAccount, IEnumerable<string> OrderNumbers, Action<bool, string> Log)
        {
            MarketplaceWebServiceOrdersClient service = GetAmazonService<MarketplaceWebServiceOrdersClient>(AmazonAccount);
            GetOrderRequest request = new GetOrderRequest();

            OrderIdList ol = new OrderIdList();

            foreach (string OrderNumber in OrderNumbers)
                ol.Id.Add(OrderNumber);

            if (Log != null)
                Log(false, "Retreiving Orders ");

            foreach (Throttler Throttler in Throttlers)
                if (Throttler != null)
                    Throttler.DoWait(Log);

            request.SellerId = AmazonAccount.MerchantID;
            request.AmazonOrderId = ol;

            GetOrderResponse response = service.GetOrder(request);
            GetOrderResult result = response.GetOrderResult;

            List<Order> rtn = new List<Order>();

            if (result.IsSetOrders())
            {
                if (Log != null)
                    Log(true, "Got " + result.Orders.Order.Count.ToString());

                foreach (Order o in result.Orders.Order)
                    rtn.Add(o);
            }
            else
                if (Log != null)
                    Log(true, "Got none");


            return rtn;

        }
        public static List<Order> GetOrders(List<Throttler> Throttlers, AmazonAccount AmazonAccount, DateTime CreatedAfter, Action<bool, string> Log)
        {
            MarketplaceWebServiceOrdersConfig config = new MarketplaceWebServiceOrdersConfig();
            config.ServiceURL = AmazonAccount.Country.OrderServiceURL;

            MarketplaceWebServiceOrdersClient service = new MarketplaceWebServiceOrdersClient(applicationName, applicationVersion, AmazonAccount.AccessKeyID, AmazonAccount.SecretAccessKey, config);
            
            ListOrdersRequest request = new ListOrdersRequest();
            request.SellerId = AmazonAccount.MerchantID;
            request.CreatedAfter = CreatedAfter;
            
            MarketplaceIdList mpid = new MarketplaceIdList();
            mpid.Id = new List<string>() { AmazonAccount.Country.AmazonMarketPlaceID };

            request.MarketplaceId = mpid;
            ListOrdersResponse response;

            if (Log != null)
                Log(false, "Retreiving Orders ");

            if (Throttlers != null)
                foreach (Throttler Throttler in Throttlers)
                    Throttler.DoWait(Log);

            response = service.ListOrders(request);

            ListOrdersResult listOrdersResult = response.ListOrdersResult;

            List<Order> rtn = new List<Order>();

            bool ResultIsSetOrders = listOrdersResult.IsSetOrders();
            bool ResultIsSetNextOrder = listOrdersResult.IsSetNextToken();
            List<Order> OrderList = listOrdersResult.Orders.Order;
            string NextToken = listOrdersResult.NextToken;

            while (true)
            {
                if (ResultIsSetOrders)
                {
                    if (Log != null)
                        Log(true, "Got " + OrderList.Count.ToString());

                    foreach (Order o in OrderList)
                        rtn.Add(o);
                }
                else
                    if (Log != null)
                        Log(true, "Got none");

                if (!ResultIsSetNextOrder)
                    break;

                ListOrdersByNextTokenRequest request2 = new ListOrdersByNextTokenRequest();

                request2.SellerId = AmazonAccount.MerchantID;
                request2.NextToken = NextToken;

                if (Log != null)
                    Log(false, "Retreiving More Orders ");

                ListOrdersByNextTokenResponse response2 = null;

                foreach (Throttler Throttler in Throttlers)
                    if (Throttler != null)
                        Throttler.DoWait(Log);

                bool ThrottlerSlowed = false;

                while (response2 == null)
                {
                    try
                    {
                        response2 = service.ListOrdersByNextToken(request2);
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

                ListOrdersByNextTokenResult listOrdersResult2 = response2.ListOrdersByNextTokenResult;

                ResultIsSetOrders = listOrdersResult2.IsSetOrders();
                ResultIsSetNextOrder = listOrdersResult2.IsSetNextToken();
                OrderList = listOrdersResult2.Orders.Order;
                NextToken = listOrdersResult2.NextToken;

            }

            return rtn;
        }

        public static List<OrderItem> GetOrderItems(List<Throttler> Throttlers, AmazonAccount AmazonAccount, string AmazonOrderID, Action<bool, string> Log)
        {
            MarketplaceWebServiceOrdersConfig config = new MarketplaceWebServiceOrdersConfig();
            config.ServiceURL = AmazonAccount.Country.OrderServiceURL;

            MarketplaceWebServiceOrdersClient service = new MarketplaceWebServiceOrdersClient(applicationName, applicationVersion, AmazonAccount.AccessKeyID, AmazonAccount.SecretAccessKey, config);

            ListOrderItemsRequest request = new ListOrderItemsRequest();
            // @TODO: set request parameters here
            request.SellerId = AmazonAccount.MerchantID;
            request.AmazonOrderId = AmazonOrderID;

            ListOrderItemsResponse response;

            if (Log != null)
                Log(false, "Retreiving Order Items for " + AmazonOrderID + " ");

            foreach (Throttler Throttler in Throttlers)
                if (Throttler != null)
                    Throttler.DoWait(Log);

            response = service.ListOrderItems(request);

            ListOrderItemsResult listOrderItemsResult = response.ListOrderItemsResult;

            List<OrderItem> rtn = new List<OrderItem>();

            bool ResultIsSetOrders = listOrderItemsResult.IsSetOrderItems();
            bool ResultIsSetNextOrder = listOrderItemsResult.IsSetNextToken();
            List<OrderItem> OrderList = listOrderItemsResult.OrderItems.OrderItem;
            string NextToken = listOrderItemsResult.NextToken;

            while (true)
            {
                if (ResultIsSetOrders)
                {
                    if (Log != null)
                        Log(true, "Got " + OrderList.Count.ToString());

                    foreach (OrderItem oi in OrderList)
                        rtn.Add(oi);
                }
                else
                    if (Log != null)
                        Log(true, "Got none");

                if (!ResultIsSetNextOrder)
                    break;

                ListOrderItemsByNextTokenRequest request2 = new ListOrderItemsByNextTokenRequest();

                request2.SellerId = AmazonAccount.MerchantID;
                request2.NextToken = NextToken;

                if (Log != null)
                    Log(false, "Retreiving More Order Items for " + AmazonOrderID + " ");

                foreach (Throttler Throttler in Throttlers)
                    if (Throttler != null)
                        Throttler.DoWait(Log);

                ListOrderItemsByNextTokenResponse response2 = service.ListOrderItemsByNextToken(request2);
                ListOrderItemsByNextTokenResult listOrdersResult2 = response2.ListOrderItemsByNextTokenResult;

                ResultIsSetOrders = listOrdersResult2.IsSetOrderItems();
                ResultIsSetNextOrder = listOrdersResult2.IsSetNextToken();
                OrderList = listOrdersResult2.OrderItems.OrderItem;
                NextToken = listOrdersResult2.NextToken;

            }

            return rtn;
        }

        public static int WriteOrdersToDatabase(List<Throttler> Throttlers, Entities db, AmazonAccount Account, IEnumerable<Order> Orders, bool SaveRecords, Action<bool, string> Log)
        {
            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 1800;

            int cnt = 0;

            foreach (Order o in Orders)
            {
                Log(false, (cnt++).ToString() + ": " + o.OrderStatus + " - " + o.PurchaseDate.ToString("MM/dd/yyyy HH:mm:ss") + " " + o.AmazonOrderId + ": ");

                if (o.OrderStatus == OrderStatusEnum.Pending || o.OrderStatus == OrderStatusEnum.Canceled)
                    Log(false, "SKIPPED: " + o.OrderStatus.ToString());
                else
                {

                    AmazonOrder ao = db.AmazonOrders.Where(aox => aox.AmazonOrderID == o.AmazonOrderId).FirstOrDefault();

                    bool Process = false;

                    if (ao == null)
                        Process = true;
                    else if (ao.Status != (int)o.OrderStatus)
                    {
                        Log(false, "Updating order status... ");
                        ao.Status = (int)o.OrderStatus;
                        ao.LastStatusChangeNoticed = DateTime.Now;

                        if (SaveRecords)
                            db.SaveChanges();
                    }
                    else
                    {
                        Log(false, "Already in database! ");

                        if (ao.AmazonOrderLines.Count(aol => aol.Price == 0 && aol.Qty > 0) > 0)
                        {
                            Log(false, "Has OrderLine with ZERO PRICE.  Processing. ");
                            Process = true;

                            foreach (AmazonOrderLine aol in db.AmazonOrderLines.Where(aol => aol.AmazonOrderID == o.AmazonOrderId).ToList())
                                db.AmazonOrderLines.Remove(aol);
                        }
                    }

                    if (Process)
                    {
                        #region Create Order Record and Get Order Line Records
                        string[] names = (o.BuyerName == null ? " " : o.BuyerName).Split(' ');

                        if (ao == null)
                        {
                            ao = new AmazonOrder();
                            db.AmazonOrders.Add(ao);

                            ao.AmazonAccountID = Account.ID;
                            ao.AmazonOrderID = o.AmazonOrderId;
                            ao.PurchaseDate = o.PurchaseDate;
                            ao.NameFirst = names[0];
                            ao.NameLast = names[names.Length - 1];
                            ao.FulfillmentChannel = (int)o.FulfillmentChannel;
                            ao.TimeStamp = DateTime.Now;

                            if (o.ShippingAddress == null)
                                Log(false, "No Shipping Address");
                            else
                            {

                                Log(false, o.ShippingAddress.City + ", " + o.ShippingAddress.StateOrRegion);
                                Log(false, " " + o.ShippingAddress.Phone);

                                ao.ShipAddress1 = o.ShippingAddress.AddressLine1;
                                ao.ShipAddress2 = o.ShippingAddress.AddressLine2;
                                ao.ShipAddress3 = o.ShippingAddress.AddressLine3;
                                ao.ShipCity = o.ShippingAddress.City;
                                ao.ShipStateProvince = o.ShippingAddress.StateOrRegion;
                                ao.ShipPostalCode = o.ShippingAddress.PostalCode;
                                ao.ShipCountryCode = o.ShippingAddress.CountryCode;
                                ao.ShipPhone = o.ShippingAddress.Phone;
                                ao.ShipToName = o.ShippingAddress.Name;
                            }
                        }

                        List<OrderItem> Lines = Library.GetOrderItems(Throttlers, Account, o.AmazonOrderId, Log);

                        int line = 0;

                        foreach (OrderItem oi in Lines)
                        {
                            AmazonOrderLine aol = new AmazonOrderLine();
                            aol.AmazonAccountID = Account.ID;
                            aol.AmazonOrderID = o.AmazonOrderId;
                            aol.LineNumber = ++line;
                            aol.ASIN = oi.ASIN;

                            if (oi.ItemPrice == null)
                                aol.Price = 0;
                            else
                                aol.Price = decimal.Parse(oi.ItemPrice.Amount);

                            aol.Qty = oi.QuantityOrdered;
                            aol.Name = oi.Title;
                            aol.TimeStamp = DateTime.Now;

                            db.AmazonOrderLines.Add(aol);
                        }

                        Log(false, " - Got " + line.ToString() + " items.");

                        if (SaveRecords)
                        {
                            try
                            {
                                db.SaveChanges();
                            }
                            catch (Exception Ex)
                            {
                                string ErrMsg = "";

                                while (Ex != null)
                                {
                                    ErrMsg += Ex.Message + "\n\n" + Ex.StackTrace + "\n\n-----------------------------------------\n\n";
                                    Ex = Ex.InnerException;
                                }

                                Log(true, "ERROR SAVING! " + ErrMsg);
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        ao.ShipToName = o.ShippingAddress.Name;
                        db.SaveChanges();
                    }
                }

                Log(true, "");
            }

            return cnt;
        }

        static string GetASIN(List<Throttler> Throttlers, Entities db, AmazonAccount Account, string AmazonOrderId, string SKU, string OrderItemID, string Title, Action<bool, string> Log)
        {
            string ASIN = null;

            Log(false, "Getting ASIN for SKU: " + SKU + "...");

            AmazonInventorySKU ais = db.AmazonInventorySKUs.Where(aisx => aisx.SKU == SKU).FirstOrDefault();

            if (ais == null)
            {
                Log(false, "Adding SKU to database...");

                List<OrderItem> Lines = Library.GetOrderItems(Throttlers, Account, AmazonOrderId, Log);

                string OrderItemIDString = OrderItemID.ToString();

                OrderItem line = Lines.Single(l => l.OrderItemId == OrderItemIDString);

                ASIN = line.ASIN;

                AmazonInventorySKU nais = new AmazonInventorySKU();
                nais.AmazonAccountID = Account.ID;
                nais.ASIN = ASIN;
                nais.SKU = SKU;
                nais.TimeStamp = DateTime.Now;
                db.AmazonInventorySKUs.Add(nais);

                KnownASIN ka = db.KnownASINs.Where(kax => kax.ASIN == ASIN).FirstOrDefault();

                if (ka == null)
                {
                    Log(false, "Adding ASIN to database...");

                    ka = new KnownASIN();
                    ka.ASIN = ASIN;
                    ka.TimeStamp = DateTime.Now;
                    ka.OurProduct = true;
                    ka.MarketPlaceID = Account.Country.AmazonMarketPlaceID;
                    ka.Filtered = false;
                    ka.Reviewed = ka.TimeStamp;
                    ka.Title = Title;

                    db.KnownASINs.Add(ka);
                }
                else
                    ka.OurProduct = true;

                AmazonInventory ai = db.AmazonInventories.Where(aix => aix.AmazonAccountID == Account.ID && aix.ASIN == ASIN).FirstOrDefault();

                if (ai == null)
                {
                    Log(false, "Adding Inventory Record...");

                    ai = new AmazonInventory();
                    ai.AmazonAccountID = Account.ID;
                    ai.ASIN = ASIN;
                    ai.TimeStamp = DateTime.Now;

                    db.AmazonInventories.Add(ai);
                }
            }
            else
                ASIN = ais.ASIN;

            Log(false, "Got ASIN: " + ASIN + ".  ");

            return ASIN;
        }


    }

}
