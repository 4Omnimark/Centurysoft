using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OmnimarkAmazon.Models;
using System.IO;
using System.Data;
using System.Reflection;
using System.Globalization;
using System.Data.Entity.Infrastructure;

namespace OmnimarkAmazon.BLL
{

    public static class ReportActions
    {

        public static string Start(Entities db, AmazonAccount AmazonAccount, Library.ReportType ReportType, string Method, Dictionary<string, string> Parms, Action<bool, string> Log)
        {
            ReportAction ra = new ReportAction();
            ra.ID = Guid.NewGuid();
            ra.ReportType = ReportType.ToString();
            ra.ProcessingMethod = Method;
            ra.TimeStamp = DateTime.Now;
            ra.AmazonAccountID = AmazonAccount.ID;
            db.ReportActions.Add(ra);

            if (Parms == null)
                Parms = new Dictionary<string, string>();

            foreach (string k in Parms.Keys)
            {

                ReportActionParameter rap = new ReportActionParameter();
                rap.ReportActionID = ra.ID;
                rap.ParameterName = k;
                rap.ParameterValue = Parms[k];
                db.ReportActionParameters.Add(rap);
            }

            db.SaveChanges();

            string ReportRequestID = Library.RequestReport(AmazonAccount, ReportType, Parms.ContainsKey("StartDate") ? DateTime.Parse(Parms["StartDate"]) : (Nullable<DateTime>)null, Parms.ContainsKey("EndDate") ? DateTime.Parse(Parms["EndDate"]) : (Nullable<DateTime>)null, Parms.ContainsKey("Options") ? Parms["Options"] : null, Log);

            ra.ReportRequestID = ReportRequestID;
            db.SaveChanges();

            return ReportRequestID;

        }

        public class ReportError
        {
            public Guid ReportActionID;
            public string ReportRequestID;
            public Exception Error;
            public string ReportText;
        }

        public class CompletedReport
        {
            public string ReportRequestID;
            public Library.ReportType Type;

            public CompletedReport(string ReportRequestID, Library.ReportType Type)
            {
                this.ReportRequestID = ReportRequestID;
                this.Type = Type;
            }
        }

        public static void GetCompletedReports(Entities db, Action<bool, string> Log, List<ReportError> ReportErrors = null)
        {
            Library.Throttler Throttler = new Library.Throttler(5000);

            foreach (AmazonAccount AmazonAccount in db.AmazonAccounts.Where(aa => aa.Enabled).ToList())
            {
                Log(false, "Checking for Completed Reports for " + AmazonAccount.Name + ": ");

                List<string> ReportRequestIDs = new List<string>();

                foreach (ReportAction ra in db.ReportActions.Where(rax => rax.AmazonAccountID == AmazonAccount.ID && rax.ReportRequestID != null && rax.ReportCompleted == null))
                    ReportRequestIDs.Add(ra.ReportRequestID);

                Log(true, "Got " + ReportRequestIDs.Count.ToString() + " active ReportActions.");

                if (ReportRequestIDs.Count > 0)
                {

                    List<MarketplaceWebService.Model.ReportRequestInfo> ReportRequests = Library.GetReportRequestList(new List<Library.Throttler>() { Throttler }, AmazonAccount, ReportRequestIDs, Log);

                    List<CompletedReport> ReportRequestIDsToDownload = new List<CompletedReport>();

                    foreach (MarketplaceWebService.Model.ReportRequestInfo rri in ReportRequests)
                    {
                        Log(false, "Checking Status of ReportRequestID: " + rri.ReportRequestId + ": " + rri.ReportProcessingStatus);

                        if (rri.ReportProcessingStatus == Library.ReportRequestStatus._DONE_.ToString() || rri.ReportProcessingStatus == Library.ReportRequestStatus._DONE_NO_DATA_.ToString())
                        {
                            #region Process Completed Report
                            ReportAction ra = db.ReportActions.Single(rax => rax.ReportRequestID == rri.ReportRequestId);
                            ra.ReportCompleted = rri.EndDate;

                            if (rri.ReportProcessingStatus == Library.ReportRequestStatus._DONE_.ToString())
                            {
                                ReportDownloadLog(ra, Log, true, " - Added to Download List");
                                ReportRequestIDsToDownload.Add(new CompletedReport(rri.ReportRequestId, Startbutton.Library.StringToEnum<OmnimarkAmazon.Library.ReportType>(rri.ReportType)));
                            }
                            else
                                ReportDownloadLog(ra, Log, true, " - Completed with No Data");

                            db.SaveChanges();
                            #endregion
                        }
                        else
                            Log(true, "");
                    }

                    if (ReportRequestIDsToDownload.Count > 0)
                    {

                        List<MarketplaceWebService.Model.ReportInfo> rl = Library.GetReportList(new List<Library.Throttler>() { Throttler }, AmazonAccount, null, ReportRequestIDsToDownload.Select(z => z.ReportRequestID).ToList(), Log);

                        foreach (MarketplaceWebService.Model.ReportInfo ri in rl)
                        {
                            #region Process Completed Report With Data
                            ReportAction ra = db.ReportActions.Single(rax => rax.ReportRequestID == ri.ReportRequestId);

                            ra.ReportID = ri.ReportId;
                            db.SaveChanges();

                            Log(true, "Downloading Completed Report! ReportRequestID: " + ri.ReportRequestId + " - ReportID: " + ri.ReportId);

                            MemoryStream ReportStream = new MemoryStream();

                            try
                            {
                                #region Download and Save Report Data

                                ReportDownloadLog(ra, Log, true, "Retreiving Report Data...");
                                Library.GetReport(AmazonAccount, ri.ReportId, ReportStream, (lbrk, logtxt) => ReportDownloadLog(ra, Log, lbrk, logtxt));

                                ReportDownloadLog(ra, Log, true, "Parsing Report Data...");

                                DataTable Report = null;

                                try
                                {
                                    Report = Library.ReportStreamToDataTable(ReportStream/*, ReportRequestIDsToDownload.Single(rrix => rrix.ReportRequestID == ri.ReportRequestId).Type*/);
                                }
                                catch (Exception Ex)
                                {
                                    ReportDownloadLog(ra, Log, true, "ERROR! " + Ex.Message);

                                    if (ReportErrors != null)
                                    {

                                        ReportStream.Close();
                                        ReportStream.Dispose();

                                        ReportStream = new MemoryStream();

                                        ReportDownloadLog(ra, Log, true, "Generating ReportError...");
                                        ReportDownloadLog(ra, Log, true, "Re-retreiving Report Data...");
                                        Library.GetReport(AmazonAccount, ri.ReportId, ReportStream, (lbrk, logtxt) => ReportDownloadLog(ra, Log, lbrk, logtxt));

                                        ReportError re = new ReportError();
                                        re.ReportActionID = ra.ID;
                                        re.ReportRequestID = ra.ReportRequestID;
                                        re.Error = Ex;

                                        StreamReader sr = new StreamReader(ReportStream);
                                        re.ReportText = sr.ReadToEnd();

                                        ReportErrors.Add(re);
                                    }
                                }

                                ReportStream.Close();
                                ReportStream.Dispose();

                                if (Report != null)
                                {

                                    ReportDownloadLog(ra, Log, true, "Saving Data...");
                                    Library.WriteReportToTable(ri.ReportId, Report, true, (lbrk, logtxt) => ReportDownloadLog(ra, Log, lbrk, logtxt));

                                    ra.ReportDataSaved = DateTime.Now;
                                    db.SaveChanges();
                                }
                                #endregion
                            }
                            catch (Exception Ex)
                            {
                                ReportDownloadLog(ra, Log, true, "\n\nERROR: " + Ex.Message + "\n\n" + Ex.StackTrace);
                                db.SaveChanges();
                            }
                            #endregion
                        }
                    }

                }

            }

        }

        public static void ProcessDownloadedReports(Entities db, Action<bool, string> Log)
        {
            foreach (AmazonAccount AmazonAccount in db.AmazonAccounts.Where(aa => aa.Enabled).ToList())
            {

                Log(true, "Checking for Unprocessed Downloaded Reports for " + AmazonAccount.Name);

                foreach (ReportAction ra in db.ReportActions.Where(rax => rax.AmazonAccountID == AmazonAccount.ID && rax.ReportDataSaved != null && rax.ProcessingStarted == null).ToList())
                {
                    Log(true, "Found ReportAction: " + ra.ID.ToString() + " with Method: " + ra.ProcessingMethod);

                    ra.ProcessingStarted = DateTime.Now;
                    db.SaveChanges();

                    MethodInfo mi = typeof(ReportActions).GetMethods().Where(m => m.Name == ra.ProcessingMethod).FirstOrDefault();

                    if (mi == null)
                        ProcessingResultLog(ra, Log, true, "Method " + ra.ProcessingMethod + " not found!");
                    else
                    {
                        try
                        {
                            mi.Invoke(null, new object[] { db, AmazonAccount, ra, Log });
                        }
                        catch (Exception Ex)
                        {
                            ProcessingResultLog(ra, Log, true, "\n\nERROR: " + Ex.InnerException.Message + "\n\n" + Ex.InnerException.StackTrace);
                        }
                    }

                    ra.ProcessingComplete = DateTime.Now;
                    db.SaveChanges();

                }

            }

        }

        public static void ProcessSpecificCompletedReport(Entities db, string ReportID, Action<bool, string> Log)
        {
            ReportAction ra = db.ReportActions.Single(rax => rax.ReportID == ReportID);

            AmazonAccount AmazonAccount = db.AmazonAccounts.Single(aa => aa.ID == ra.AmazonAccountID);

            MethodInfo mi = typeof(ReportActions).GetMethods().Where(m => m.Name == ra.ProcessingMethod).FirstOrDefault();

            if (mi == null)
                ProcessingResultLog(ra, Log, true, "Method " + ra.ProcessingMethod + " not found!");
            else
            {
                try
                {
                    mi.Invoke(null, new object[] { db, AmazonAccount, ra, Log });
                }
                catch (Exception Ex)
                {
                    ProcessingResultLog(ra, Log, true, "\n\nERROR: " + Ex.InnerException.Message + "\n\n" + Ex.InnerException.StackTrace);
                }

                ra.ProcessingComplete = DateTime.Now;
                db.SaveChanges();

            }
        }

        static void ReportDownloadLog(ReportAction ra, Action<bool, string> Log, bool lbrk, string logtxt)
        {
            ra.ReportDataDownloadLog += logtxt;

            if (lbrk)
                ra.ReportDataDownloadLog += "\n";

            Log(lbrk, logtxt);
        }

        static void ProcessingResultLog(ReportAction ra, Action<bool, string> Log, bool lbrk, string logtxt)
        {
            ra.ProcessingResult += logtxt;

            if (lbrk)
                ra.ProcessingResult += "\n";

            Log(lbrk, logtxt);
        }

        public static string StartSyncSKUs(Entities db, AmazonAccount AmazonAccount, Action<bool, string> Log)
        {
            string ReportRequestID = Start(db, AmazonAccount, Library.ReportType._GET_MERCHANT_LISTINGS_DATA_, "SyncSKUs", null, Log);
            AmazonAccount.LastSyncSKU = DateTime.Now;
            db.SaveChanges();

            return ReportRequestID;
        }

        public static string StartGetOrderHistory(Entities db, AmazonAccount AmazonAccount, DateTime StartDate, Action<bool, string> Log)
        {
            Dictionary<string, string> Parms = new Dictionary<string, string>();
            Parms["StartDate"] = StartDate.ToString();

            string ReportRequestID = Start(db, AmazonAccount, Library.ReportType._GET_FLAT_FILE_ORDERS_DATA_, "GetOrderHistory", Parms, Log);

            return ReportRequestID;
        }

        static DateTime ConvertDateString(List<OmnimarkAmazon.Models.TimeZone> TimeZones, string DateTimeFormat, string DateString)
        {
            string[] a = DateString.Split(' ');

            string WithOffset = a[0] + " " + a[1] + " " + TimeZones.Single(tz => tz.Abbreviation == a[2]).OffsetText;

            return DateTime.ParseExact(WithOffset, DateTimeFormat, CultureInfo.InvariantCulture);
        }

        static void DeleteReportTable(Entities db, ReportAction ra, Action<bool, string> Log)
        {
            db.Database.ExecuteSqlCommand("drop table " + OmnimarkAmazon.Library.ReportTablePrefix + ra.ReportID);
            db.SaveChanges();

            Log(true, "Dropped table " + OmnimarkAmazon.Library.ReportTablePrefix + ra.ReportID + ".");
        }

        public static void SyncSKUs(Entities db, AmazonAccount AmazonAccount, ReportAction ra, Action<bool, string> Log)
        {

            List<OmnimarkAmazon.Models.TimeZone> TimeZones = db.TimeZones.ToList();

            (db as IObjectContextAdapter).ObjectContext.CommandTimeout = 180;

            IEnumerable<_GET_MERCHANT_LISTINGS_DATA_BACK_COMPAT_> qry = db.Database.SqlQuery<_GET_MERCHANT_LISTINGS_DATA_BACK_COMPAT_>("select * from " + Library.ReportTablePrefix + ra.ReportID).ToList();

            foreach (_GET_MERCHANT_LISTINGS_DATA_BACK_COMPAT_ row in qry)
            {
                Log(false, ".");

                if (row.product_id_type != "1")
                {
                    ProcessingResultLog(ra, Log, true, "\nUnknown product_id_type for listing_id: " + row.listing_id + " - seller_sku: " + row.seller_sku);
                }
                else
                {

                    DateTime OpenDate = ConvertDateString(TimeZones, AmazonAccount.Country.DateTimeFormat, row.open_date);

                    KnownASIN ka = db.KnownASINs.Where(kax => kax.ASIN == row.product_id).FirstOrDefault();

                    if (ka == null)
                    {

                        ka = new KnownASIN();
                        ka.ASIN = row.product_id;
                        ka.Filtered = false;
                        ka.TimeStamp = DateTime.Now;
                        ka.Reviewed = ka.TimeStamp;
                        ka.OurProduct = true;
                        ka.MarketPlaceID = AmazonAccount.Country.AmazonMarketPlaceID;

                        if (AmazonAccount.CountryID == 840)
                        {
                            ka.OurMostRecentUSListingAccountID = AmazonAccount.ID;
                            ka.OurMostRecentUSListingOpenDate = OpenDate;
                            ka.OurMostRecentUSListingPrice = decimal.Parse(row.price);
                        }

                        if (row.item_name != null)
                            ka.Title = row.item_name;
                        else
                        {
                            Amazon.AWS.ItemLookupResponseItemsItem ilr = Library.ItemLookup(AmazonAccount.Country.Code, row.product_id, (lbrk, logtxt) => ProcessingResultLog(ra, Log, lbrk, logtxt));

                            if (ilr != null && ilr.ItemAttributes.Title != null)
                                ka.Title = ilr.ItemAttributes.Title;
                        }

                        db.KnownASINs.Add(ka);

                        ProcessingResultLog(ra, Log, true, "Added ASIN: " + row.product_id);

                        db.SaveChanges();
                    }
                    else
                    {
                        ka.OurProduct = true;

                        if (AmazonAccount.CountryID == 840)
                        {
                            if (ka.OurMostRecentUSListingOpenDate == null || ka.OurMostRecentUSListingOpenDate < OpenDate)
                            {
                                ka.OurMostRecentUSListingAccountID = AmazonAccount.ID;
                                ka.OurMostRecentUSListingOpenDate = OpenDate;
                                ka.OurMostRecentUSListingPrice = decimal.Parse(row.price);

                                ProcessingResultLog(ra, Log, true, "Updated OurMostRecentUSListing Info for ASIN: " + row.product_id);

                                db.SaveChanges();

                            }
                        }
                    }


                    AmazonInventorySKU ais = db.AmazonInventorySKUs.Where(aisx => aisx.AmazonAccountID == AmazonAccount.ID && aisx.ASIN == row.product_id && aisx.SKU == row.seller_sku).FirstOrDefault();

                    AmazonInventory ai = db.AmazonInventories.Where(aix => aix.AmazonAccountID == AmazonAccount.ID && aix.ASIN == row.product_id).FirstOrDefault();

                    Nullable<int> Qty = null;

                    if (row.quantity != null)
                        Qty = int.Parse(row.quantity);

                    if (ais == null)
                    {

                        if (ai == null)
                        {
                            ai = new AmazonInventory();
                            ai.AmazonAccountID = AmazonAccount.ID;
                            ai.ASIN = row.product_id;
                            ai.TimeStamp = DateTime.Now;

                            db.AmazonInventories.Add(ai);

                            ProcessingResultLog(ra, Log, true, "Added Amazon Inventory for ASIN: " + row.product_id + " - SKU: " + row.seller_sku);

                            db.SaveChanges();

                        }

                        ais = new AmazonInventorySKU();
                        ais.AmazonAccountID = AmazonAccount.ID;
                        ais.ASIN = row.product_id;
                        ais.SKU = row.seller_sku;
                        ais.TimeStamp = DateTime.Now;

                        ais.ListingID = row.listing_id;
                        ais.Price = decimal.Parse(row.price);
                        ais.Quantity = Qty;
                        ais.OpenDate = OpenDate;
                        ais.ItemCondition = int.Parse(row.item_condition);

                        db.AmazonInventorySKUs.Add(ais);

                        ProcessingResultLog(ra, Log, true, "Added SKU: " + row.seller_sku + " for ASIN: " + row.product_id);

                        db.SaveChanges();

                    }
                    else
                    {

                        if (ais.ListingID != row.listing_id ||
                            ais.Price != decimal.Parse(row.price) ||
                            ais.Quantity != Qty ||
                            ais.ItemCondition != int.Parse(row.item_condition) || 
                            ais.OpenDate != OpenDate)
                        {

                            ais.UpdateTimeStamp = DateTime.Now;

                            ais.ListingID = row.listing_id;
                            ais.Price = decimal.Parse(row.price);
                            ais.Quantity = Qty;
                            ais.OpenDate = OpenDate;
                            ais.ItemCondition = int.Parse(row.item_condition);
                            
                            ProcessingResultLog(ra, Log, true, "Updated SKU: " + row.seller_sku);

                            db.SaveChanges();

                        }
                    }

                    if (ai.LastKnownListingOpenDate == null || ai.LastKnownListingOpenDate < OpenDate)
                    {
                        ai.LastKnownListingOpenDate = OpenDate;
                        ai.LastKnownListingPrice = decimal.Parse(row.price);

                        ProcessingResultLog(ra, Log, true, "Updated LastKnownPrice");

                        if (ka.OurLowestPrice == null || ai.LastKnownListingPrice < ka.OurLowestPrice)
                        {
                            ka.OurLowestPrice = ai.LastKnownListingPrice;
                            ProcessingResultLog(ra, Log, true, "Updated OurLowestPrice");

                        }

                        db.SaveChanges();
                    }


                }
            }

            db.SaveChanges();

            DeleteReportTable(db, ra, Log);
        }

        public static void GetOrderHistory(Entities db, AmazonAccount AmazonAccount, ReportAction ra, Action<bool, string> Log)
        {

            Library.Throttler OrderLineThrottler = new Library.Throttler(6000);

            (db as IObjectContextAdapter).ObjectContext.CommandTimeout = 180;

            IEnumerable<_GET_FLAT_FILE_ORDERS_DATA_> qry = db.Database.SqlQuery<_GET_FLAT_FILE_ORDERS_DATA_>("select * from " + Library.ReportTablePrefix + ra.ReportID + " where purchase_date is not null").ToList();

            List<string> AllOrders = db.AmazonOrders.Select(ao => ao.AmazonOrderID).ToList();

            List<string> MissingOrders = qry.Where(q => !AllOrders.Contains(q.order_id)).Select(q => q.order_id).ToList();

            for(int x = 0; x < MissingOrders.Count; x += 20)
            {
                var OrdersChunk = Library.GetOrders(new List<OmnimarkAmazon.Library.Throttler>() { OrderLineThrottler }, AmazonAccount, MissingOrders.Skip(x).Take(20), Log);
                OmnimarkAmazon.Library.WriteOrdersToDatabase(new List<OmnimarkAmazon.Library.Throttler>() { OrderLineThrottler }, db, AmazonAccount, OrdersChunk, true, Log);
            }

            //foreach (_GET_FLAT_FILE_ORDERS_DATA_ row in qry)
            //{
            //    DateTime PurchaseDate = DateTime.ParseExact(row.purchase_date, "yyyy'-'MM'-'dd'T'HH':'mm':'sszzzz", DateTimeFormatInfo.InvariantInfo);
            //    Library.WriteOrderToDatabase(new List<Library.Throttler>() { OrderLineThrottler }, db, AmazonAccount, PurchaseDate, row.order_id, row.buyer_name, row.ship_address_1, row.ship_address_2, row.ship_address_3, row.ship_city, row.ship_state, row.ship_postal_code, row.ship_country, row.ship_phone_number, null, row.sku, decimal.Parse(row.item_price), decimal.Parse(row.quantity_purchased), row.product_name, row.order_item_id, true, Log);
            //}

            DeleteReportTable(db, ra, Log);
        }
    }

}

    