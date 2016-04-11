using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.IO;
using OmnimarkAmazon.Models;

namespace OmnimarkAmazon.Service
{
    public partial class Service : ServiceBase
    {
        Timer OrderDownloaderTimer;
        Timer ScraperTimer;
        Timer ReportActionsTimer;
        Timer FeedSubmitterTimer;
        Timer ImageDownloaderTimer;
        Timer FixMissingReceivedProductInventoryAdjustmentsTimer;
        bool IsConsole;
        Dictionary<string, bool> LogWasLastNewLine = new Dictionary<string, bool>();
        bool ShutdownTriggered = false;
        string LogDir = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";

        public Service(bool IsConsole)
        {
            this.IsConsole = IsConsole;
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Startup();
        }

        public void Startup()
        {
            
            string LogFile = LogDir + "\\OmnimarkAmazonService_" + DateTime.Now.ToString("yyyyMMdd") + ".log";

            Log(LogFile, true, "");
            Log(LogFile, true, "Service Starting...");

            //Log(LogFile, true, "Spawning OrderDownloader Timer...");
            //OrderDownloaderTimer = new Timer(OrderDownloader, null, 0, 3600000);

            Log(LogFile, true, "Spawning ItemSearch thread...");
            ThreadPool.QueueUserWorkItem(new WaitCallback(ItemSearcher), null);

            //Log(LogFile, true, "Spawning Scraper thread...");
            //ScraperTimer = new Timer(Scraper, null, 0, 3600000);

            //Log(LogFile, true, "Spawning ReportActions thread...");
            //ReportActionsTimer = new Timer(ReportActionProcessor, null, 0, 600000);

            //Log(LogFile, true, "Spawning FeedSubmitter thread...");
            //FeedSubmitterTimer = new Timer(FeedSubmitter, null, 0, 120000);

            //Log(LogFile, true, "Spawning ImageDownloader thread...");
            //ImageDownloaderTimer = new Timer(ImageDownloader, null, 0, 3600000);

            //Log(LogFile, true, "Spawning Received Product Inventory Adjustments thread...");
            //FixMissingReceivedProductInventoryAdjustmentsTimer = new Timer(FixMissingReceivedProductInventoryAdjustments, null, 0, 86400000);

        }

        protected override void OnStop()
        {
            string LogFile = LogDir + "\\OmnimarkAmazonService_" + DateTime.Now.ToString("yyyyMMdd") + ".log";

            Shutdown();
            Log(LogFile, true, "");
            Log(LogFile, true, "Service Stopping...");

        }

        public void Shutdown()
        {
            OrderDownloaderTimer.Dispose();
            ScraperTimer.Dispose();
            ShutdownTriggered = true;
        }

        public void Scraper(Object state)
        {

            string LogFile = LogDir + "\\OmnimarkAmazonService_Scraper_" + DateTime.Now.ToString("yyyyMMdd") + ".log";

            ScraperTimer.Dispose();

            Entities db = new Entities();

            List<Country> Countries = db.Countries.ToList();

            try
            {

                foreach (OfferScrapeStatus oss in db.OfferScrapeStatuses.OrderBy(ossx => ossx.LastScrapeTry).ThenBy(ossx => ossx.ASIN).Take(1000).ToList())
                {
                    if (ShutdownTriggered)
                        break;

                    try
                    {
                        Library.ScrapeOfferListings(db, null, db.Countries.Single(c => c.Code == oss.CountryID), oss.ASIN, (lf, txt) => Log(LogFile, lf, txt), true);

                        if (oss.OurProduct == false)
                        {
                            if (db.OurListings.Count(ol => ol.ASIN == oss.ASIN) > 0)
                            {
                                db.KnownASINs.Single(kax => kax.ASIN == oss.ASIN).OurProduct = true;
                                db.SaveChanges();
                            }
                        }
                    }
                    catch(Exception Ex)
                    {
                        Log(LogFile, true, "\n\n\n**** OFFER SCRAPE EXCEPTION ****");

                        while (Ex != null)
                        {
                            Log(LogFile, true, "EXCEPTION: " + Ex.Message + "\n" + Ex.StackTrace);
                            Ex = Ex.InnerException;
                        }

                        Log(LogFile, true, "\n\n");

                    }
                }

                foreach (KnownASIN ka in Library.GetASINsToScrape(db, 1000).ToList())
                {

                    if (ShutdownTriggered)
                        break;

                    Log(LogFile, false, "Scraping Attributes for " + ka.ASIN + ": ");

                    Library.ScrapedItemAttributes sia = Library.ScrapeItemAttributes(db, ka, Countries.Single(c => c.AmazonMarketPlaceID == ka.MarketPlaceID), ka.ASIN, null, true, (lf, txt) => Log(LogFile, lf, txt));
                    db.SaveChanges();

                    if (sia != null)
                    {
                        Log(LogFile, false, "Saving... ");
                        int changes = Library.SaveScrapedAttributes(db, sia);

                        db.SaveChanges();

                        Log(LogFile, true, changes.ToString() + " attributes added/changed.");
                    }

                }

            }

            catch (Exception Ex)
            {
                while (Ex != null)
                {
                    Log(LogFile, true, "EXCEPTION: " + Ex.Message + "\n" + Ex.StackTrace);
                    Ex = Ex.InnerException;
                }
            }
    


    if (!ShutdownTriggered)
            {
                Log(LogFile, true, "Re-enabling Timer...");
                ScraperTimer = new Timer(Scraper, null, 3600000, 3600000);
            }

        }

        public void ReportActionProcessor(Object state)
        {

            string LogFile = LogDir + "\\OmnimarkAmazonService_ReportActions_" + DateTime.Now.ToString("yyyyMMdd") + ".log";

            ReportActionsTimer.Dispose();

            Entities db = new Entities();

            try
            {

                DateTime TwelveHoursAgo = DateTime.Now.AddHours(-12);
                DateTime Yesterday = DateTime.Today.AddDays(-1);

                foreach (AmazonAccount AmazonAccount in db.AmazonAccounts.Where(aa => aa.Enabled && (aa.LastSyncSKU == null || aa.LastSyncSKU < TwelveHoursAgo)).ToList())
                    BLL.ReportActions.StartSyncSKUs(db, AmazonAccount, (lb, txt) => Log(LogFile, lb, txt));

                foreach (AmazonAccount AmazonAccount in db.AmazonAccounts.Where(aa => aa.Enabled && (aa.LastOrderHistoryReportStartDate == null || aa.LastOrderHistoryReportStartDate < Yesterday)).ToList())
                {
                    DateTime StartDate;

                    if (AmazonAccount.LastOrderHistoryReportStartDate == null)
                        StartDate = DateTime.Parse("1/1/2013");
                    else
                        StartDate = ((DateTime)AmazonAccount.LastOrderHistoryReportStartDate).AddDays(1);
                    
                    BLL.ReportActions.StartGetOrderHistory(db, AmazonAccount, StartDate, (lb, txt) => Log(LogFile, lb, txt));
                    AmazonAccount.LastOrderHistoryReportStartDate = Yesterday;
                    db.SaveChanges();
                }

                BLL.ReportActions.GetCompletedReports(db, (lb, txt) => Log(LogFile, lb, txt));
                BLL.ReportActions.ProcessDownloadedReports(db, (lb, txt) => Log(LogFile, lb, txt));
            }
            catch (Exception Ex)
            {
                while (Ex != null)
                {
                    Log(LogFile, true, "EXCEPTION: " + Ex.Message + "\n" + Ex.StackTrace);
                    Ex = Ex.InnerException;
                }

            }

            if (!ShutdownTriggered)
            {
                Log(LogFile, true, "Re-enabling Timer...");
                ReportActionsTimer = new Timer(ReportActionProcessor, null, 600000, 600000);
            }

        }

        public void FeedSubmitter(Object state)
        {

            string LogFile = LogDir + "\\OmnimarkAmazonService_FeedSubmitter_" + DateTime.Now.ToString("yyyyMMdd") + ".log";

            FeedSubmitterTimer.Dispose();

            Entities db = new Entities();

            try
            {
                OmnimarkAmazon.BLL.ScheduledFeeds.ProcessScheduledFeeds(db, (lb, txt) => Log(LogFile, lb, txt));
            }
            catch (Exception Ex)
            {
                while (Ex != null)
                {
                    Log(LogFile, true, "EXCEPTION: " + Ex.Message + "\n" + Ex.StackTrace);
                    Ex = Ex.InnerException;
                }

            }

            if (!ShutdownTriggered)
            {
                Log(LogFile, true, "Re-enabling Timer...");
                FeedSubmitterTimer = new Timer(FeedSubmitter, null, 120000, 120000);
            }

        }

        public void ImageDownloader(Object state)
        {

            string LogFile = LogDir + "\\OmnimarkAmazonService_ImageDownloader_" + DateTime.Now.ToString("yyyyMMdd") + ".log";

            ImageDownloaderTimer.Dispose();

            Entities db = new Entities();

            try
            {
                Library.DownloadImage(db, "C:\\Websites\\images.enutramart.com", (lb, txt) => Log(LogFile, lb, txt), null, 100000);
            }
            catch (Exception Ex)
            {
                while (Ex != null)
                {
                    Log(LogFile, true, "EXCEPTION: " + Ex.Message + "\n" + Ex.StackTrace);
                    Ex = Ex.InnerException;
                }

            }

            if (!ShutdownTriggered)
            {
                Log(LogFile, true, "Re-enabling Timer...");
                ImageDownloaderTimer = new Timer(ImageDownloader, null, 3600000, 3600000);
            }

        }

        public void FixMissingReceivedProductInventoryAdjustments(Object state)
        {

            string LogFile = LogDir + "\\OmnimarkAmazonService_FixMissingReceivedProductInventoryAdjustments_" + DateTime.Now.ToString("yyyyMMdd") + ".log";

            FixMissingReceivedProductInventoryAdjustmentsTimer.Dispose();

            Entities db = new Entities();

            try
            {
                db.Database.ExecuteSqlCommand("FixMissingReceivedProductInventoryAdjustments");
            }
            catch (Exception Ex)
            {
                while (Ex != null)
                {
                    Log(LogFile, true, "EXCEPTION: " + Ex.Message + "\n" + Ex.StackTrace);
                    Ex = Ex.InnerException;
                }

            }

            if (!ShutdownTriggered)
            {
                Log(LogFile, true, "Re-enabling Timer...");
                FixMissingReceivedProductInventoryAdjustmentsTimer = new Timer(FixMissingReceivedProductInventoryAdjustments, null, 86400000, 86400000);
            }
        }

        public void OrderDownloader(Object state)
        {

            OrderDownloaderTimer.Dispose();

            string LogFile = LogDir + "\\OmnimarkAmazonService_OrderDownloader_" + DateTime.Now.ToString("yyyyMMdd") + ".log";

            try
            {

                OmnimarkAmazon.Library.Throttler Throttler = new OmnimarkAmazon.Library.Throttler();

                Entities db = new Entities();

                OmnimarkAmazon.Library.Throttler OrderThrottler = new OmnimarkAmazon.Library.Throttler(10000);

                Log(LogFile, true, "Downloading Orders...");

                foreach (AmazonAccount account in db.AmazonAccounts.Where(aa => aa.Enabled).ToList())
                {
                    Log(LogFile, true, "Processing Account: " + account.Name);

                    OmnimarkAmazon.Library.Throttler OrderLineThrottler = new OmnimarkAmazon.Library.Throttler(6000);

                    OmnimarkAmazon.Library.WriteOrdersToDatabase(new List<OmnimarkAmazon.Library.Throttler>() { OrderLineThrottler }, db, account, OmnimarkAmazon.Library.GetOrders(new List<OmnimarkAmazon.Library.Throttler>() { OrderThrottler }, account, DateTime.Now.AddDays(-7), (lf, txt) => Log(LogFile, lf, txt)), true, (lf, txt) => Log(LogFile, lf, txt));

                    if (ShutdownTriggered)
                        break;
                }

                Log(LogFile, true, "Checking Orders for missing Fulfillment Channel or Status or Email...");

                foreach (AmazonAccount account in db.AmazonAccounts.Where(aa => aa.Enabled).ToList())
                {
                    int LoopCount = 0;
                    
                    while (true)
                    {

                        Log(LogFile, true, "Processing Account: " + account.Name);

                        IEnumerable<AmazonOrder> orders = db.AmazonOrders.Where(ao => ao.AmazonAccountID == account.ID && (ao.FulfillmentChannel == null || ao.Status == null || ao.Email == null)).OrderByDescending(o => o.PurchaseDate).Take(50);

                        if (orders.Count() == 0)
                        {
                            Log(LogFile, true, "No orders found.");
                            break;
                        }
                        else
                        {

                            List<MarketplaceWebServiceOrders.Model.Order> aorders = Library.GetOrders(new List<OmnimarkAmazon.Library.Throttler>() { OrderThrottler }, account, orders.Select(ao => ao.AmazonOrderID), (lf, txt) => Log(LogFile, lf, txt));

                            foreach (var order in aorders)
                            {
                                var o = orders.Single(ao => ao.AmazonOrderID == order.AmazonOrderId);
                                o.FulfillmentChannel = (int)order.FulfillmentChannel;

                                if (o.Email == null)
                                {
                                    if (order.BuyerEmail == null)
                                        o.Email = "null";
                                    else
                                        o.Email = order.BuyerEmail;
                                }

                                if (o.Status != (int)order.OrderStatus)
                                {
                                    o.Status = (int)order.OrderStatus;
                                    o.LastStatusChangeNoticed = o.TimeStamp;
                                }

                            }

                            db.SaveChanges();

                            if (++LoopCount == 4)
                                break;
                        }
                    }

                }
                if (!ShutdownTriggered)
                {
                    Log(LogFile, true, "Enabling OrderDownloaderTimer...");
                    OrderDownloaderTimer = new Timer(OrderDownloader, null, 3600000, 3600000);
                }
            }
            catch (Exception Ex)
            {
                while (Ex != null)
                {
                    Log(LogFile, true, "EXCEPTION: " + Ex.Message + "\n" + Ex.StackTrace);
                    Ex = Ex.InnerException;
                }

                if (!ShutdownTriggered)
                {
                    Log(LogFile, true, "Re-enabling Timer...");
                    OrderDownloaderTimer = new Timer(OrderDownloader, null, 3600000, 3600000);
                }

            }
        }

        public void ItemSearcher(object o)
        {

            string LogFile = LogDir + "\\OmnimarkAmazonService_ItemSearch_" + DateTime.Now.ToString("yyyyMMdd") + ".log";

            while (true)
            {
                try
                {
                    Entities db = new Entities();

                    DateTime SearchStartTime = DateTime.Now;

                    Log(LogFile, true, "Beginning ItemSearches...");

                    var Terms = db.SearchTerms.OrderBy(t => t.LastSearch).ToList();

                    foreach (var Term in Terms)
                    {
                        foreach (Country Country in db.Countries.Where(c => c.AmazonMarketPlaceID != null).ToList())
                        {
                            Log(LogFile, true, "Searching: " + Term.Term + " - Country: " + Country.CountryName);

                            OmnimarkAmazon.Library.AddASINsToDB(Country, OmnimarkAmazon.Library.ItemSearch(Country.Code, Term.Term, (lf, txt) => Log(LogFile, lf, txt)), Term.Term, (lf, txt) => Log(LogFile, lf, txt));

                            Term.LastSearch = DateTime.Now;
                            db.SaveChanges();

                            if (ShutdownTriggered)
                                break;
                        }

                        if (ShutdownTriggered)
                            break;

                        Thread.Sleep(60000);
                    }

                    db.SystemStatus.First().LastSearch = SearchStartTime;
                    db.SaveChanges();
                }
                catch (Exception Ex)
                {
                    while (Ex != null)
                    {
                        Log(LogFile, true, "EXCEPTION: " + Ex.Message + "\n" + Ex.StackTrace);
                        Ex = Ex.InnerException;
                    }

                }

                if (ShutdownTriggered)
                    break;
            }
        }

        void Log(string LogFile, bool Linefeed, string line)
        {
            if (!LogWasLastNewLine.ContainsKey(LogFile))
                LogWasLastNewLine.Add(LogFile, true);

            if (IsConsole)
            {
                if (LogWasLastNewLine[LogFile])
                    Console.Write(DateTime.Now.ToString("yyyyMMdd HHmmss") + ": ");

                Console.Write(line);

                if (Linefeed)
                    Console.WriteLine("");

            }

            StreamWriter sw = new StreamWriter(LogFile, true);

            if (LogWasLastNewLine[LogFile])
                sw.Write(DateTime.Now.ToString("HHmmss") + ": ");

            sw.Write(line);

            if (Linefeed)
            {
                sw.WriteLine("");
                LogWasLastNewLine[LogFile] = true;
            }
            else
                LogWasLastNewLine[LogFile] = false;

            sw.Close();

        }

    }

}
