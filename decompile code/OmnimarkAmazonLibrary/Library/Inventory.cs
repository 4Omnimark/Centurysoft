using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarketplaceWebServiceOrders;
using MarketplaceWebServiceOrders.Model;
using OmnimarkAmazon.Models;
using FBAInventoryServiceMWS;
using FBAInventoryServiceMWS.Model;
using Startbutton.ExtensionMethods;

namespace OmnimarkAmazon
{
    public static partial class Library
    {
        public static IEnumerable<InventorySupplySummary> GetInventory(List<Throttler> Throttlers, AmazonAccount AmazonAccount, Action<bool, string> Log, List<string> SKUs = null)
        {
            return GetInventory(Throttlers, AmazonAccount, Log, null, SKUs);
        }

        public static IEnumerable<InventorySupplySummary> GetInventory(List<Throttler> Throttlers, AmazonAccount AmazonAccount, Action<bool, string> Log, Action<bool, string> DebugLog, List<string> SKUs = null)
        {
            if (Throttlers == null)
                Throttlers = new List<Library.Throttler>() { new Library.Throttler(1000) };

            var service = GetAmazonService<FBAInventoryServiceMWSClient>(AmazonAccount);

            DoLog(Log, false, "Getting Inventory Supply for " + AmazonAccount.Name + ": ");

            if (Throttlers != null)
                foreach (Throttler Throttler in Throttlers)
                    Throttler.DoWait(Log);

            List<InventorySupply> rtn = new List<InventorySupply>();
            ListInventorySupplyResponse response;

            SellerSkuList ssl = new SellerSkuList();

            if (SKUs != null)
            {
                foreach (string SKU in SKUs)
                    ssl.member.Add(SKU);

                response = service.ListInventorySupply(new ListInventorySupplyRequest().WithSellerId(AmazonAccount.MerchantID).WithSellerSkus(ssl));
            }
            else
                response = service.ListInventorySupply(new ListInventorySupplyRequest().WithSellerId(AmazonAccount.MerchantID).WithQueryStartDateTime(DateTime.Parse("1/1/2000")));

            if (DebugLog != null)
                DebugLog(false, Startbutton.Library.JsonSerialize<ListInventorySupplyResponse>(response));

            ListInventorySupplyResult result = response.ListInventorySupplyResult;

            bool ResultIsSet = result.IsSetInventorySupplyList();
            bool ResultIsSetNextOrder = result.IsSetNextToken();
            string NextToken = result.NextToken;
            List<InventorySupply> List = result.InventorySupplyList.member;

            while (true)
            {
                if (ResultIsSet)
                {
                    if (Log != null)
                        Log(true, "Got " + List.Count.ToString());

                    foreach (InventorySupply o in List)
                        rtn.Add(o);
                }
                else
                    if (Log != null)
                        Log(true, "Got none");

                if (!ResultIsSetNextOrder)
                    break;

                ListInventorySupplyByNextTokenRequest request2 = new ListInventorySupplyByNextTokenRequest();

                request2.SellerId = AmazonAccount.MerchantID;
                request2.NextToken = NextToken;

                if (Log != null)
                    Log(false, "Retreiving More Inventory Supply: ");

                foreach (Throttler Throttler in Throttlers)
                    if (Throttler != null)
                        Throttler.DoWait(Log);

                ListInventorySupplyByNextTokenResponse response2 = service.ListInventorySupplyByNextToken(request2);
                
                if (DebugLog != null)
                    DebugLog(false, Startbutton.Library.JsonSerialize<ListInventorySupplyByNextTokenResponse>(response2));

                ListInventorySupplyByNextTokenResult result2 = response2.ListInventorySupplyByNextTokenResult;

                ResultIsSet = result2.IsSetInventorySupplyList();
                ResultIsSetNextOrder = result2.IsSetNextToken();
                NextToken = result2.NextToken;
                List = result2.InventorySupplyList.member;

            }

            IEnumerable<InventorySupplySummary> summary = rtn.GroupBy(r => r.ASIN).Select(g => new InventorySupplySummary { ASIN = g.Key, TotalSupplyQuantity = g.Sum(r => r.TotalSupplyQuantity), InStockSupplyQuantity = g.Sum(r => r.InStockSupplyQuantity), SKUs = rtn.Where(r => r.ASIN == g.Key) });

            return summary;

        }

        public static int UpdateInventory(Entities db, AmazonAccount Account, IEnumerable<InventorySupplySummary> InventoryList, Action<bool, string> Log, bool UpdateInventoryUpdateTimeStamp = true, bool UpdateOnlyNoDelete = false)
        {
            int rtn = 0;

            if (UpdateInventoryUpdateTimeStamp)
            {
                db.AmazonAccounts.Single(ao => ao.ID == Account.ID).LastInventoryUpdateStart = DateTime.Now;
                db.SaveChanges();
            }

            foreach (InventorySupplySummary i in InventoryList.Where(il => il.ASIN != null))
            {
                Log(false, i.ASIN + " | ");

                AmazonInventory ai = db.AmazonInventories.Where(aix => aix.ASIN == i.ASIN && aix.AmazonAccountID == Account.ID).FirstOrDefault();

                if (ai == null)
                    ai = db.AmazonInventories.Local.Where(aix => aix.ASIN == i.ASIN && aix.AmazonAccountID == Account.ID).FirstOrDefault();

                if (ai == null)
                {
                    Log(false, "Adding new inventory record. ");

                    ai = new AmazonInventory();
                    ai.ASIN = i.ASIN;
                    ai.AmazonAccountID = Account.ID;
                    ai.AmazonStockQty = i.TotalSupplyQuantity;
                    ai.AmazonStockTimeStamp = DateTime.Now;
                    ai.TimeStamp = (DateTime)ai.AmazonStockTimeStamp;

                    db.AmazonInventories.Add(ai);

                    rtn++;

                }
                else
                {
                    ai.AmazonStockTimeStamp = DateTime.Now;

                    if (ai.AmazonStockQty != i.TotalSupplyQuantity || ai.AmazonInStockQty != i.InStockSupplyQuantity)
                    {
                        ai.AmazonStockQty = i.TotalSupplyQuantity;
                        ai.AmazonInStockQty = i.InStockSupplyQuantity;
                        rtn++;
                    }

                }

                if (UpdateOnlyNoDelete)
                {
                    #region only update
                    foreach (InventorySupply isup in i.SKUs)
                    {
                        AmazonInventorySKU ais = db.AmazonInventorySKUs.Where(aisx => aisx.AmazonAccountID == Account.ID && aisx.ASIN == i.ASIN && aisx.SKU == isup.SellerSKU).FirstOrDefault();

                        if (ais == null)
                            ais = db.AmazonInventorySKUs.Local.Where(aisx => aisx.AmazonAccountID == Account.ID && aisx.ASIN == i.ASIN && aisx.SKU == isup.SellerSKU).FirstOrDefault();

                        if (ais == null)
                        {
                            ais = new AmazonInventorySKU();
                            ais.AmazonAccountID = Account.ID;
                            ais.ASIN = i.ASIN;
                            ais.SKU = isup.SellerSKU;
                            ais.TimeStamp = DateTime.Now;

                            db.AmazonInventorySKUs.Add(ais);
                        }

                        ais.InStockQty = isup.InStockSupplyQuantity;
                        ais.TotalQty = isup.TotalSupplyQuantity;
                        ais.UpdateTimeStamp = DateTime.Now;

                    }
                    #endregion
                }
                else
                {
                    #region delete first, then re-insert

                    var ToDelete = db.AmazonInventorySKUs.Where(ais => ais.ASIN == i.ASIN && ais.AmazonAccountID == Account.ID).ToList();

                    foreach (AmazonInventorySKU ais in ToDelete)
                        db.AmazonInventorySKUs.Remove(ais);

                    foreach (InventorySupply isup in i.SKUs)
                    {
                        AmazonInventorySKU ais = new AmazonInventorySKU();
                        ais.AmazonAccountID = Account.ID;
                        ais.ASIN = i.ASIN;
                        ais.SKU = isup.SellerSKU;
                        ais.InStockQty = isup.InStockSupplyQuantity;
                        ais.TotalQty = isup.TotalSupplyQuantity;
                        ais.TimeStamp = DateTime.Now;

                        db.AmazonInventorySKUs.Add(ais);

                    }

                    #endregion
                }
            
            }

            if (UpdateInventoryUpdateTimeStamp)
                db.AmazonAccounts.Single(ao => ao.ID == Account.ID).LastInventoryUpdate = DateTime.Now;

            return rtn;
        }

        public static int AddASINsToDBFromInventory(Entities db, AmazonAccount AmazonAccount, IEnumerable<InventorySupplySummary> InventoryList, Action<bool, string> Log)
        {
            int rtn = 0;

            foreach (InventorySupplySummary i in InventoryList)
            {
                if (i.ASIN != null)
                {
                    KnownASIN ka = db.KnownASINs.Where(kax => kax.ASIN == i.ASIN).FirstOrDefault();

                    if (ka == null)
                    {
                        ka = db.KnownASINs.Local.Where(kax => kax.ASIN == i.ASIN).FirstOrDefault();

                        if (ka == null)
                        {

                            Log(false, "Got new ASIN " + i.ASIN + ". ");

                            Amazon.AWS.ItemLookupResponseItemsItem item = ItemLookup(AmazonAccount.CountryID, i.ASIN, Log);

                            ka = new KnownASIN();
                            ka.ASIN = i.ASIN;
                            ka.Filtered = false;
                            ka.TimeStamp = DateTime.Now;
                            ka.Reviewed = ka.TimeStamp;
                            ka.OurProduct = true;
                            ka.MarketPlaceID = AmazonAccount.Country.AmazonMarketPlaceID;

                            if (item != null && item.ItemAttributes.Title != null)
                                ka.Title = item.ItemAttributes.Title;

                            db.KnownASINs.Add(ka);

                            rtn++;
                        }
                    }
                    else
                    {
                        if (ka.OurProduct == false || ka.Reviewed == null)
                        {
                            ka.OurProduct = true;

                            if (ka.Reviewed == null)
                                ka.Reviewed = DateTime.Now;

                            rtn++;
                        }

                    }

                }

            }

            return rtn;
        }

    }

}
