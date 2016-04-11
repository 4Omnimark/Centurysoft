using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarketplaceWebServiceOrders.Model;
using OmnimarkAmazon.Models;
using System.Data.Entity;
using Amazon.AWS;
using System.Configuration;
using System.IO;
using System.Data;
using eBay.Service.Call;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using eBay.Service.Util;
using OmnimarkAmazon;
using FBAInboundServiceMWS.Model;
using System.Globalization;

namespace OmnimarkAmazon
{
    class Program
    {
        bool LogWasLastNewLine = true;
        string DebugLogFile = "";

        static void Main(string[] args)
        {
            //try
            //{

            Entities db = new Entities();

            while (true)
            {
                Console.WriteLine(" 0) Quit");
                Console.WriteLine(" 1) GetOrders");
                Console.WriteLine(" 2) ItemSearch");
                Console.WriteLine(" 3) ListInventorySupply");
                Console.WriteLine(" 4) GetReport");
                Console.WriteLine(" 5) SalesReport");
                Console.WriteLine(" 6) SyncSKUs");
                Console.WriteLine(" 7) GetCompletedReports");
                Console.WriteLine(" 8) ProcessDownloadedReports");
                Console.WriteLine(" 9) ScrapeItemAttributes");
                Console.WriteLine("10) RequestReport");
                Console.WriteLine("11) UpdatePrice");
                Console.WriteLine("12) GetFeedSubmissionList");
                Console.WriteLine("13) GetFeedSubmissionResult");
                Console.WriteLine("14) DoProcessScheduledFeeds");
                Console.WriteLine("15) ScrapeOfferListings");
                Console.WriteLine("16) DownloadImage");
                Console.WriteLine("17) GetOrderHistory");
                Console.WriteLine("18) ProcessSpecificCompletedReport");
                Console.WriteLine("19) FixZeroPricedOrderLines");
                Console.WriteLine("20) FixMissingFulfillmentChannel");
                Console.WriteLine("21) GetOrderItems");
                Console.WriteLine("22) SynceBayItems");
                Console.WriteLine("23) SynceBayOrders");
                Console.WriteLine("24) ReconcileInventoryForAmazonOrdersShippedFromOrlando");
                Console.WriteLine("25) SyncInboundFBAShipments");
                Console.WriteLine("26) GetInboundShipmentItems");
                Console.WriteLine("27) GetInboundShipmentsSince20140310");
                Console.WriteLine("28) MoveInventoryFromCompletedeBayOrders");
                Console.WriteLine("29) DoAddProduct");
                Console.WriteLine("30) DoSyncInboundFBAShipment");
                Console.WriteLine("31) MoveInventoryFromInboundFBAShipment");
                Console.WriteLine("32) GetSettlementData");
                Console.WriteLine("33) DetermineForcedFBAShipments");
                Console.WriteLine("");
                Console.Write("Selection: ");

                string selection = Console.ReadLine();

                if (selection == "0")
                    break;

                if (selection == "1")
                    DoGetOrders(db);

                if (selection == "2")
                    DoItemSearch(db);

                if (selection == "3")
                    DoListInventorySupply(db);

                if (selection == "4")
                    DoGetReport(db);

                if (selection == "5")
                    DoSalesReport(db);

                if (selection == "6")
                    DoSyncSKUs(db);

                if (selection == "7")
                    DoGetCompletedReports(db);

                if (selection == "8")
                    DoProcessDownloadedReports(db);

                if (selection == "9")
                    DoScrapeItemAttributes(db);

                if (selection == "10")
                    DoRequestReport(db);

                if (selection == "11")
                    DoUpdatePrice(db);

                if (selection == "12")
                    DoGetFeedSubmissionList(db);

                if (selection == "13")
                    GetFeedSubmissionResult(db);

                if (selection == "14")
                    DoProcessScheduledFeeds(db);

                if (selection == "15")
                    ScrapeOfferListings(db);

                if (selection == "16")
                    DownloadImage(db);

                if (selection == "17")
                    GetOrderHistory(db);

                if (selection == "18")
                    ProcessSpecificCompletedReport(db);

                if (selection == "19")
                    FixZeroPricedOrderLines(db);

                if (selection == "20")
                    FixMissingFulfillmentChannel(db);

                if (selection == "21")
                    GetOrderItems(db);

                if (selection == "22")
                    SynceBayItems(db);

                if (selection == "23")
                    SynceBayOrders(db);

                if (selection == "24")
                    ReconcileInventoryForAmazonOrdersShippedFromOrlando(db);

                if (selection == "25")
                    SyncInboundFBAShipments(db);

                if (selection == "26")
                    GetInboundShipmentItems(db);

                if (selection == "27")
                    GetInboundShipmentsSince20140310(db);

                if (selection == "28")
                    MoveInventoryFromCompletedeBayOrders(db);

                if (selection == "29")
                    DoAddProduct(db);

                if (selection == "30")
                    DoSyncInboundFBAShipment(db);

                if (selection == "31")
                    MoveInventoryFromInboundFBAShipment(db);

                if (selection == "32")
                    GetSettlementData(db);

                if (selection == "33")
                    DetermineForcedFBAShipments(db);

            }
            //}
            //catch (Exception Ex)
            //{
            //    Console.Write(Ex.Message + "\n" + Ex.StackTrace);
            //    Console.ReadKey();
            //}

        }

        static void DoListInventorySupply(Entities db)
        {
            Program p = new Program();

            AmazonAccount Account = GetAccount(db);

            Console.Write("ASIN (optional):");
            string ASIN = Console.ReadLine();

            string SKU = "";

            if (ASIN == "")
            {
                Console.Write("SKU (optional): ");
                SKU = Console.ReadLine();
            }

            Console.Write("Debug File: ");
            p.DebugLogFile = Console.ReadLine();

            IEnumerable<InventorySupplySummary> inventory;

            List<string> SKUs = null;

            if (ASIN != "")
                SKUs = db.AmazonInventorySKUs.Where(ais => ais.ASIN == ASIN).Select(ais => ais.SKU).ToList();

            if (SKU != "")
                SKUs = new string[] { SKU }.ToList();

            if (p.DebugLogFile == "")
                inventory = Library.GetInventory(new List<Library.Throttler>() { new Library.Throttler(1000) }, Account, p.Log, SKUs);
            else
                inventory = Library.GetInventory(new List<Library.Throttler>() { new Library.Throttler(1000) }, Account, p.Log, p.DebugLog, SKUs);

            foreach (InventorySupplySummary i in inventory)
            {
                Console.WriteLine(i.ASIN + ": " + i.TotalSupplyQuantity);

                foreach (var sku in i.SKUs)
                    Console.WriteLine("\t" + sku.SellerSKU + ": InStock:" + sku.InStockSupplyQuantity + ", Total:" + sku.TotalSupplyQuantity);
            }

            Console.WriteLine("\nGot " + inventory.Count().ToString() + " results.\n");

            Console.WriteLine("");

            Console.Write("Update KnownASINs? ");
            string cki = Console.ReadLine();

            Console.WriteLine("");

            if (cki.ToUpper() == "Y")
            {
                int ChangeCount = Library.AddASINsToDBFromInventory(db, Account, inventory, p.Log);
                Console.WriteLine(ChangeCount.ToString() + " items updated/added.");
            }

            Console.WriteLine("");

            Console.Write("Update Inventory? ");
            cki = Console.ReadLine();

            Console.WriteLine("");

            if (cki.ToUpper() == "Y")
            {
                int ChangeCount = Library.UpdateInventory(db, Account, inventory, p.Log, false);
                Console.WriteLine(ChangeCount.ToString() + " items updated/added.");
                db.SaveChanges();
            }

            Console.WriteLine("");
        }

        static void DoGetReport(Entities db)
        {
            Program p = new Program();

            Console.WriteLine("");

            int AccountShortID = 0;

            while (db.AmazonAccounts.Count(aa => aa.ShortID == AccountShortID) == 0)
            {
                foreach (AmazonAccount aa in db.AmazonAccounts)
                    Console.WriteLine(aa.ShortID.ToString() + ") " + aa.Name);

                Console.Write("Account: ");
                string AccountShortIDString = Console.ReadLine();

                if (AccountShortIDString == "")
                    break;

                int.TryParse(AccountShortIDString, out AccountShortID);
            }

            AmazonAccount AmazonAccount = db.AmazonAccounts.Single(aa => aa.ShortID == AccountShortID);

            Library.ReportType[] ReportTypes = Startbutton.Library.GetEnumValues<Library.ReportType>().ToArray();

            int ReportTypeNumber = -1;

            while (ReportTypeNumber == -1)
            {

                Console.WriteLine("\n0) ANY");

                for (int x = 0; x < ReportTypes.Length; x++)
                    Console.WriteLine((x + 1).ToString() + ") " + ReportTypes[x].ToString());

                Console.Write("Report Type: ");
                string ReportTypeNumberString = Console.ReadLine();

                if (ReportTypeNumberString == "")
                    break;

                int.TryParse(ReportTypeNumberString, out ReportTypeNumber);
            }

            MarketplaceWebService.Model.ReportInfo[] Reports = null;

            if (ReportTypeNumber == 0)
                Reports = Library.GetReportList(new List<Library.Throttler>() { new Library.Throttler(2000) }, AmazonAccount, null, null, p.Log).ToArray();
            else
                Reports = Library.GetReportList(new List<Library.Throttler>() { new Library.Throttler(2000) }, AmazonAccount, ReportTypes[ReportTypeNumber - 1], null, p.Log).ToArray();

            if (Reports.Length > 0)
            {

                Console.Write("\n");

                int ReportNumber = 0;

                while (ReportNumber == 0)
                {

                    for (int x = 0; x < Reports.Length; x++)
                    {
                        MarketplaceWebService.Model.ReportInfo ri = Reports[x];
                        Console.WriteLine((x + 1).ToString() + ") " + ri.AvailableDate.ToString() + ": " + ri.ReportType + ": " + ri.ReportId);
                    }

                    Console.Write("Report: ");
                    string ReportNumberString = Console.ReadLine();

                    if (ReportNumberString == "")
                        break;

                    int.TryParse(ReportNumberString, out ReportNumber);
                }

                MemoryStream ReportStream = new MemoryStream();

                Library.GetReport(AmazonAccount, Reports[ReportNumber - 1].ReportId, ReportStream, p.Log);

                DataTable Report = Library.ReportStreamToDataTable(ReportStream);

                string[] Fields = new string[Report.Columns.Count];

                for (int x = 0; x < Report.Columns.Count; x++)
                    Fields[x] = Report.Columns[x].ColumnName;

                Console.Write("\n");

                int FieldNumber = -1;

                while (FieldNumber == -1)
                {
                    Console.WriteLine("0) Skip");

                    for (int x = 0; x < Fields.Length; x++)
                        Console.WriteLine((x + 1).ToString() + ") " + Fields[x]);

                    Console.Write("Field: ");
                    string FieldNumberString = Console.ReadLine();

                    if (FieldNumberString == "")
                        break;

                    int.TryParse(FieldNumberString, out FieldNumber);
                }

                if (FieldNumber != 0)
                    foreach (DataRow dr in Report.Rows)
                        Console.WriteLine(dr[FieldNumber - 1]);

                Console.Write("\nWrite To SQL Server? ");

                string answer = Console.ReadLine();

                if (answer.ToUpper() == "Y")
                    Library.WriteReportToTable(Reports[ReportNumber - 1].ReportId, Report, true, p.Log);
            }

            Console.WriteLine("");
        }

        static DateTime GetDate()
        {
            DateTime rtn = DateTime.MinValue;

            while (rtn == DateTime.MinValue)
            {
                Console.Write("Start Date: ");

                string DateString = Console.ReadLine();

                if (DateString == "")
                    break;

                DateTime.TryParse(DateString, out rtn);
            }

            return rtn;
        }

        static void DoRequestReport(Entities db)
        {
            Program p = new Program();

            Console.WriteLine("");

            int AccountShortID = 0;

            while (db.AmazonAccounts.Count(aa => aa.ShortID == AccountShortID) == 0)
            {
                foreach (AmazonAccount aa in db.AmazonAccounts)
                    Console.WriteLine(aa.ShortID.ToString() + ") " + aa.Name);

                Console.Write("Account: ");
                string AccountShortIDString = Console.ReadLine();

                int.TryParse(AccountShortIDString, out AccountShortID);
            }

            AmazonAccount AmazonAccount = db.AmazonAccounts.Single(aa => aa.ShortID == AccountShortID);

            Library.ReportType[] ReportTypes = Startbutton.Library.GetEnumValues<Library.ReportType>().ToArray();

            int ReportTypeNumber = -1;

            while (ReportTypeNumber < 1 || ReportTypeNumber > ReportTypes.Length)
            {

                for (int x = 0; x < ReportTypes.Length; x++)
                    Console.WriteLine((x + 1).ToString() + ") " + ReportTypes[x].ToString());

                Console.Write("Report Type: ");
                string ReportTypeNumberString = Console.ReadLine();

                if (ReportTypeNumberString == "")
                    break;

                int.TryParse(ReportTypeNumberString, out ReportTypeNumber);
            }

            DateTime StartDate = GetDate();

            Dictionary<string, string> Parms = new Dictionary<string, string>();

            if (StartDate != DateTime.MinValue)
                Parms["StartDate"] = StartDate.ToString();

            string ReportRequestID = BLL.ReportActions.Start(db, AmazonAccount, ReportTypes[ReportTypeNumber - 1], "Nothing", Parms, p.Log);

            Console.WriteLine("ReportRequestID: " + ReportRequestID);

            Console.WriteLine("");
        }

        static void DoItemSearch(Entities db)
        {

            Country Country = GetCountry(db);

            Console.Write("Search: ");
            string search = Console.ReadLine();

            Program p = new Program();

            List<ItemSearchResponseItemsItem> results = Library.ItemSearch(Country.Code, search, p.Log);

            Console.WriteLine("\nGot " + results.Count.ToString() + " results.\n");

            foreach (ItemSearchResponseItemsItem i in results)
                if (i.ASIN != null && i.ItemAttributes.Title != null)
                    Console.WriteLine(i.ASIN + ": " + i.ItemAttributes.Title);

            Console.WriteLine("");

            Console.Write("Add ASINs to DB? ");
            ConsoleKeyInfo cki = Console.ReadKey();

            Console.WriteLine("");

            if (cki.KeyChar == 'Y' || cki.KeyChar == 'y')
            {
                int AddedCount = Library.AddASINsToDB(Country, results, search, p.Log);
                Console.WriteLine(AddedCount.ToString() + " items added.");
            }

            Console.WriteLine("");
        }

        static Country GetCountry(Entities db)
        {
            int CountryID = 0;

            List<Country> CountryList = db.Countries.Where(cx => cx.AmazonAccounts.Count > 0).ToList();

            while (CountryID < 1)
            {
                Console.WriteLine("");

                foreach (Country c in CountryList)
                    Console.WriteLine(c.Code.ToString() + ") " + c.CountryName);

                Console.Write("Country: ");
                int.TryParse(Console.ReadLine(), out CountryID);

                if (CountryList.Count(c => c.Code == CountryID) == 0)
                    CountryID = 0;
            }

            return db.Countries.Single(c => c.Code == CountryID);
        }

        static AmazonAccount GetAccount(Entities db)
        {
            int AccountShortID = 0;

            while (db.AmazonAccounts.Count(aa => aa.ShortID == AccountShortID) == 0)
            {
                foreach (AmazonAccount aa in db.AmazonAccounts)
                    Console.WriteLine(aa.ShortID.ToString() + ") " + aa.Name);

                Console.Write("Account: ");
                string AccountShortIDString = Console.ReadLine();

                if (AccountShortIDString == "")
                    break;

                int.TryParse(AccountShortIDString, out AccountShortID);
            }

            return db.AmazonAccounts.Single(aa => aa.ShortID == AccountShortID);
        }

        static void DoGetOrders(Entities db)
        {

            AmazonAccount Account = GetAccount(db);

            Console.Write("Orders Numbers: ");
            string OrderNumbers = Console.ReadLine();
            string date = "";

            if (OrderNumbers == "")
            {
                Console.Write("Orders From Date: ");
                date = Console.ReadLine();
            }

            Library.Throttler OrderThrottler = new Library.Throttler(8000);
            Library.Throttler OrderLineThrottler = new Library.Throttler(6000);

            Program p = new Program();
            List<Order> Orders = null;

            if (OrderNumbers != "")
                Orders = Library.GetOrders(new List<Library.Throttler>() { OrderThrottler }, Account, OrderNumbers.Split(',').ToList(), p.Log);

            if (date != "")
                Orders = Library.GetOrders(new List<Library.Throttler>() { OrderThrottler }, Account, DateTime.Parse(date), p.Log);

            Library.WriteOrdersToDatabase(new List<Library.Throttler>() { OrderLineThrottler }, db, Account, Orders, true, p.Log);

            Console.WriteLine("");
        }

        static void DoSalesReport(Entities db)
        {
            DateTime StartDate = DateTime.MinValue;

            while (StartDate == DateTime.MinValue)
            {
                Console.Write("Start Date: ");
                DateTime.TryParse(Console.ReadLine(), out StartDate);
            }

            int Days = 0;

            while (Days < 1)
            {
                Console.Write("Days: ");
                int.TryParse(Console.ReadLine(), out Days);
            }

            IQueryable<SalesReportRecord> recs = SQLFunctions.SalesReport(StartDate, Days);

            foreach (SalesReportRecord rec in recs)
                Console.WriteLine(rec.AmazonAccountID.ToString() + "\t" + rec.ASIN + "\t" + rec.QtySold.ToString());

            Console.WriteLine("");
        }

        static void DoSyncSKUs(Entities db)
        {
            Program p = new Program();

            int AccountShortID = 0;

            while (db.AmazonAccounts.Count(aa => aa.ShortID == AccountShortID) == 0)
            {
                foreach (AmazonAccount aa in db.AmazonAccounts)
                    Console.WriteLine(aa.ShortID.ToString() + ") " + aa.Name);

                Console.Write("Account: ");
                string AccountShortIDString = Console.ReadLine();

                if (AccountShortIDString == "")
                    break;

                int.TryParse(AccountShortIDString, out AccountShortID);
            }

            string ReportRequestID = BLL.ReportActions.StartSyncSKUs(db, db.AmazonAccounts.Single(aa => aa.ShortID == AccountShortID), p.Log);

            Console.WriteLine("ReportRequestID: " + ReportRequestID);

            Console.WriteLine("");

        }

        static void DoGetCompletedReports(Entities db)
        {
            Program p = new Program();

            List<OmnimarkAmazon.BLL.ReportActions.ReportError> ReportErrors = new List<BLL.ReportActions.ReportError>();

            BLL.ReportActions.GetCompletedReports(db, p.Log, ReportErrors);

            if (ReportErrors.Count > 0)
            {
                foreach (var re in ReportErrors)
                {
                    Console.WriteLine("ERROR! ReportActionID: " + re.ReportActionID.ToString() + " - ReportRequestID: " + re.ReportRequestID + "\n" +
                        re.Error.Message + "\n\n" +
                        re.Error.StackTrace + "\n\n" +
                        re.ReportText + "\n\n");
                }
            }

            Console.WriteLine("");
        }

        static void DoProcessDownloadedReports(Entities db)
        {
            Program p = new Program();

            BLL.ReportActions.ProcessDownloadedReports(db, p.Log);

            Console.WriteLine("");
        }

        static void DoScrapeItemAttributes(Entities db)
        {
            Program p = new Program();

            Country Country = GetCountry(db);

            Console.Write("ASIN (blank for multiple): ");

            string ASIN = Console.ReadLine();
            int RecordCount = 0;
            List<Library.ScrapedItemAttributes> sias = new List<Library.ScrapedItemAttributes>();

            if (ASIN == "")
            {
                Console.Write("Number of Records: ");
                while (RecordCount == 0)
                    int.TryParse(Console.ReadLine(), out RecordCount);

                foreach (KnownASIN ka in Library.GetASINsToScrape(db, RecordCount).ToList())
                {
                    Library.ScrapedItemAttributes sia = Library.ScrapeItemAttributes(db, ka, Country, ka.ASIN, null, true, p.Log, true);

                    if (sia != null)
                        sias.Add(sia);
                }
            }
            else
                sias.Add(Library.ScrapeItemAttributes(db, Country, ASIN, null, true, p.Log, true));

            Console.Write("Update DB? ");
            string Answer = Console.ReadLine();

            if (Answer.ToUpper() == "Y")
            {
                foreach (Library.ScrapedItemAttributes sia in sias)
                {

                    Console.Write("Saving " + sia.Info.ASIN + ": ");
                    int changes = Library.SaveScrapedAttributes(db, sia);

                    Console.WriteLine(changes.ToString() + " attributes added/changed.");

                    db.SaveChanges();
                }
            }

            Console.WriteLine("");

        }

        void Log(bool Linefeed, string line)
        {
            if (LogWasLastNewLine)
                Console.Write(DateTime.Now.ToString("yyyyMMdd HHmmss") + ": ");

            Console.Write(line);

            if (Linefeed)
            {
                Console.WriteLine("");
                LogWasLastNewLine = true;
            }
            else
                LogWasLastNewLine = false;

        }

        void DebugLog(bool Linefeed, string line)
        {

            StreamWriter sw = new StreamWriter(DebugLogFile, true);

            sw.Write(line);

            if (Linefeed)
                sw.WriteLine("");

            sw.Close();

        }

        static AmazonAccount GetAmazonAccount(Entities db)
        {
            int AccountShortID = 0;

            while (db.AmazonAccounts.Count(aa => aa.ShortID == AccountShortID) == 0)
            {
                foreach (AmazonAccount aa in db.AmazonAccounts)
                    Console.WriteLine(aa.ShortID.ToString() + ") " + aa.Name);

                Console.Write("Account: ");
                string AccountShortIDString = Console.ReadLine();

                int.TryParse(AccountShortIDString, out AccountShortID);
            }

            return db.AmazonAccounts.Single(aa => aa.ShortID == AccountShortID);
        }

        static void DoUpdatePrice(Entities db)
        {
            int AccountShortID = 0;

            while (db.AmazonAccounts.Count(aa => aa.ShortID == AccountShortID) == 0)
            {
                foreach (AmazonAccount aa in db.AmazonAccounts)
                    Console.WriteLine(aa.ShortID.ToString() + ") " + aa.Name);

                Console.Write("Account: ");
                string AccountShortIDString = Console.ReadLine();

                int.TryParse(AccountShortIDString, out AccountShortID);
            }

            Console.Write("SKU: ");
            string SKU = Console.ReadLine();

            int Price = 0;

            while (Price == 0)
            {

                Console.Write("New Price: ");
                string PriceString = Console.ReadLine();

                int.TryParse(PriceString, out Price);
            }

            Amazon.XML.Price p = new Amazon.XML.Price(SKU, Price);

            Dictionary<int, object> msgs = new Dictionary<int, object>();
            msgs.Add(1, p);

            MarketplaceWebService.Model.SubmitFeedResult r = Library.SubmitPriceChangeFeed(db, AccountShortID, msgs);

            Console.Write("FeedSubmissionId: " + r.FeedSubmissionInfo.FeedSubmissionId);

            Console.WriteLine("");

        }

        static void DoGetFeedSubmissionList(Entities db)
        {
            Program p = new Program();

            AmazonAccount AmazonAccount = GetAmazonAccount(db);

            Console.Write("Submission ID: ");

            string FeedSubmissionID = Console.ReadLine();

            List<MarketplaceWebService.Model.FeedSubmissionInfo> sl;

            if (FeedSubmissionID == "")
                sl = Library.GetFeedSubmissionList(new List<Library.Throttler>(), AmazonAccount, null, p.Log);
            else
                sl = Library.GetFeedSubmissionList(new List<Library.Throttler>() { new Library.Throttler(3000) }, AmazonAccount, new string[] { FeedSubmissionID }, p.Log);

            foreach (MarketplaceWebService.Model.FeedSubmissionInfo fsi in sl)
                Console.WriteLine(fsi.FeedSubmissionId + ": " + fsi.FeedProcessingStatus);

        }

        static void GetFeedSubmissionResult(Entities db)
        {
            Program p = new Program();

            AmazonAccount AmazonAccount = GetAmazonAccount(db);

            Console.Write("Submission ID: ");

            string FeedSubmissionID = Console.ReadLine();

            string Result = Library.GetFeedSubmissionResult(new List<Library.Throttler>(), AmazonAccount, FeedSubmissionID, p.Log);

            Console.WriteLine(Result);
            Console.WriteLine();

        }

        static void DoProcessScheduledFeeds(Entities db)
        {
            Program p = new Program();

            OmnimarkAmazon.BLL.ScheduledFeeds.ProcessScheduledFeeds(db, p.Log);

            Console.WriteLine();
        }

        static void ScrapeOfferListings(Entities db)
        {
            Program p = new Program();

            Console.Write("ASIN: ");

            string ASIN = Console.ReadLine();

            Library.ScrapeOfferListings(db, null, ASIN, p.Log, true, true);

            Console.WriteLine();
        }

        static void DownloadImage(Entities db)
        {
            Program p = new Program();

            Console.Write("ASIN: ");

            string ASIN = Console.ReadLine();

            if (ASIN == "")
                ASIN = null;

            int Count = 0;

            int.TryParse(ASIN, out Count);

            if (Count != 0)
                Library.DownloadImage(db, AppDomain.CurrentDomain.BaseDirectory, p.Log, null, Count);
            else
                Library.DownloadImage(db, AppDomain.CurrentDomain.BaseDirectory, p.Log, ASIN);

            Console.WriteLine();
        }

        static void GetOrderHistory(Entities db)
        {
            Program p = new Program();

            AmazonAccount AmazonAccount = GetAmazonAccount(db);

            DateTime StartDate = GetDate();

            string ReportRequestID = BLL.ReportActions.StartGetOrderHistory(db, AmazonAccount, StartDate, p.Log);

            Console.WriteLine("ReportRequestID: " + ReportRequestID);

            Console.WriteLine("");

        }

        static void ProcessSpecificCompletedReport(Entities db)
        {
            Program p = new Program();

            Console.Write("ReportID: ");

            string ReportID = Console.ReadLine();

            BLL.ReportActions.ProcessSpecificCompletedReport(db, ReportID, p.Log);

            Console.WriteLine("");
        }

        static void FixZeroPricedOrderLines(Entities db)
        {
            Program p = new Program();

            DateTime Beginning = DateTime.Parse("7/30/2012");

            var ToProcess = db.AmazonOrderLines.Where(aol => aol.Price == 0 && aol.Qty > 0 && aol.TimeStamp > Beginning).Select(aol => new { aol.AmazonAccountID, aol.AmazonOrderID }).Distinct().ToList();

            p.Log(true, "Got " + ToProcess.Count.ToString() + " records.");

            Library.Throttler OrderThrottler = new Library.Throttler(8000);
            Library.Throttler OrderLineThrottler = new Library.Throttler(6000);

            List<Guid> Accounts = ToProcess.Select(tp => tp.AmazonAccountID).Distinct().ToList();

            foreach (var Account in Accounts)
            {
                AmazonAccount AmazonAccount = db.AmazonAccounts.Single(aa => aa.ID == Account);

                var OrderNumbers = ToProcess.Where(tp => tp.AmazonAccountID == Account).Select(tp => tp.AmazonOrderID).ToArray();

                List<string> Process = new List<string>();

                for (int x = 0; x < OrderNumbers.Length; x++)
                {
                    Process.Add(OrderNumbers[x]);

                    if (Process.Count() == 50)
                    {
                        FixZeroPricesOrderLinesForAccount(db, OrderThrottler, OrderLineThrottler, AmazonAccount, Process, p.Log);
                        Process = new List<string>();
                    }

                }

                if (Process.Count() > 0)
                    FixZeroPricesOrderLinesForAccount(db, OrderThrottler, OrderLineThrottler, AmazonAccount, Process, p.Log);

            }

            p.Log(true, "");

        }

        static void FixZeroPricesOrderLinesForAccount(Entities db, Library.Throttler OrderThrottler, Library.Throttler OrderLineThrottler, AmazonAccount AmazonAccount, List<string> OrderNumbers, Action<bool, string> Log)
        {
            Log(true, "Processing " + OrderNumbers.Count().ToString() + " records for " + AmazonAccount.Name);

            List<Order> Orders = Library.GetOrders(new List<Library.Throttler>() { OrderThrottler }, AmazonAccount, OrderNumbers, Log);
            Library.WriteOrdersToDatabase(new List<Library.Throttler>() { OrderLineThrottler }, db, AmazonAccount, Orders, true, Log);

        }

        static void FixMissingFulfillmentChannel(Entities db)
        {

            Library.Throttler OrderThrottler = new Library.Throttler(8000);

            Program p = new Program();

            p.Log(true, "Checking Orders for missing Fulfillment Channel...");

            foreach (AmazonAccount account in db.AmazonAccounts.ToList())
            {
                int LoopCount = 0;

                while (true)
                {

                    p.Log(true, "Processing Account: " + account.Name);

                    IEnumerable<AmazonOrder> orders = db.AmazonOrders.Where(ao => ao.AmazonAccountID == account.ID && ao.FulfillmentChannel == null).Take(50);

                    if (orders.Count() == 0)
                    {
                        p.Log(true, "No orders found.");
                        break;
                    }
                    else
                    {

                        List<MarketplaceWebServiceOrders.Model.Order> aorders = Library.GetOrders(new List<OmnimarkAmazon.Library.Throttler>() { OrderThrottler }, account, orders.Select(ao => ao.AmazonOrderID), p.Log);

                        foreach (var order in aorders)
                            orders.Single(ao => ao.AmazonOrderID == order.AmazonOrderId).FulfillmentChannel = (int)order.FulfillmentChannel;

                        db.SaveChanges();

                        if (++LoopCount == 4)
                            break;
                    }
                }

            }
        }

        static void SynceBayItems(Entities db)
        {
            Program p = new Program();
            Library.SynceBayItems(db, p.Log);
        }

        static void GetOrderItems(Entities db)
        {
            AmazonAccount Account = GetAccount(db);

            Console.Write("Orders Number: ");
            string OrderNumber = Console.ReadLine();
            Library.Throttler OrderLineThrottler = new Library.Throttler(6000);

            Program p = new Program();
            List<OrderItem> Lines = Library.GetOrderItems(new List<Library.Throttler> { OrderLineThrottler }, Account, OrderNumber, p.Log);

            foreach (var line in Lines)
                Console.WriteLine(line.ASIN + " - Qty: " + line.QuantityOrdered.ToString() + "OrderItemID: " + line.OrderItemId);
        }

        static void SynceBayOrders(Entities db)
        {

            Program p = new Program();

            Library.SynceBayOrders(db, p.Log);

        }

        static void ReconcileInventoryForAmazonOrdersShippedFromOrlando(Entities db)
        {
            Program p = new Program();
            Library.ReconcileInventoryForAmazonOrdersShippedFromOrlando(db, p.Log);
        }

        static void SyncInboundFBAShipments(Entities db)
        {
            Program p = new Program();

            Library.SyncInboundFBAShipments(db, p.Log);
        }

        static void GetInboundShipmentItems(Entities db)
        {
            Program p = new Program();

            AmazonAccount Account = GetAccount(db);

            Console.Write("Shipment ID: ");
            string ShipmentID = Console.ReadLine();

            Library.GetInboundShipmentItems(new List<Library.Throttler> { new Library.Throttler(2000) }, Account, ShipmentID, p.Log);
        }

        static void MoveInventoryFromCompletedeBayOrders(Entities db)
        {
            Program p = new Program();

            Library.MoveInventoryFromCompletedeBayOrders(db, p.Log);
        }

        class GunListing
        {
            public string Model;
            public string PDAModel;
            public int PDANumber;

            public GunListing(string Model, string PDAModel, int PDANumber)
            {
                this.Model = Model;
                this.PDAModel = PDAModel;
                this.PDANumber = PDANumber;
            }
        }

        static void DoAddProduct(Entities db)
        {
            Program program = new Program();

            AmazonAccount Account = GetAccount(db);

            GunListing[] gs = new GunListing[] {
                //new GunListing("Bersa Thunder - Extended Magazines", "PDA1", 1),
                //new GunListing("Beretta PX4 Storm Compact", "PDA1", 2),
                //new GunListing("Kimber Ultra Raptor II", "PDA1", 3),
                //new GunListing("KIMBER ULTRA II 45", "PDA1", 4),
                //new GunListing("Ruger SR9C-40C PINKY EXTENSION", "PDA1", 5),
                //new GunListing("Smith Sigma SD-VE 9mm+40", "PDA1", 6),
                //new GunListing("Springfield XDm 9mm & 40mm Compact w/3.8” - Extended magazine", "PDA1", 7),
                new GunListing("Taurus Judge Public Defender 2.5\" cylinder -with pistol canted.", "PDA1", 8)
                //new GunListing("Walther PK380 ALTERNATE", "PDA1", 9),
                //new GunListing("Charter Arms 44 Bulldog w/2.5", "PDA1", 10),
                //new GunListing("Glock G-19 & G23", "PDA1", 11),
                //new GunListing("Glock G-19 & G23", "PDA1", 12),
                //new GunListing("Kimber Ultra Carry II (mag cap 7)", "PDA1", 13),
                //new GunListing("Ruger SP101 w/2.25” barrel", "PDA1", 14),
                //new GunListing("Springfield XD(m) 40 Subcompact", "PDA1", 15),
                //new GunListing("SIG-SAUER P229", "PDA1", 16),
                //new GunListing("Sig P250 Compact", "PDA1", 17),
                //new GunListing("Super Carry Ultra+ Ultra+ CDP II", "PDA1", 18),
                //new GunListing("Compact CDP II Compact Stainless II Pro Covert II", "PDA1", 19),
                //new GunListing("Smith & Wesson Model 15-3, 2” revolver", "PDA1", 20),
                //new GunListing("Springfield XD40-4” barrel", "PDA1", 21),
                //new GunListing("Taurus Judge Public Defender 2.5” cylinder with rear cant", "PDA1", 22),
                //new GunListing("Para-Ordnance Wart Hog (flush magazine required) - Finger extension", "PDA1", 23),
                //new GunListing("Steyr C9 with flush magazine", "PDA1", 24),
                //new GunListing("Glock G-19 & G23 canted", "PDA1", 25),
                //new GunListing("S&W M&P 40 Compact", "PDA1", 26),
                //new GunListing("Springfield XD(m) 40 w/3.8” barrel", "PDA1", 27),
                //new GunListing("Walther PPS with Large magazine", "PDA2", 1),
                //new GunListing("Beretta Nano with +2 magazine extension", "PDA2", 2),
                //new GunListing("Beretta PX4 Storm Subcompact -finger extension needs minor breakin", "PDA2", 3),
                //new GunListing("Ruger LC9 w/extension", "PDA2", 4),
                //new GunListing("Taurus 709B Slim/9MM 7+1", "PDA2", 5),
                //new GunListing("Cobra FS380 (breakin required)", "PDA2", 6),
                //new GunListing("Springfield XD 9mm & 40SW 3” Sub-Compact - Extended magazine", "PDA2", 7),
                //new GunListing("Taurus PT111 or PT145", "PDA2", 8),
                //new GunListing("Walther PK380 (breakin required)", "PDA2", 9),
                //new GunListing("Kahr CW 45  (minor breakin required)", "PDA2", 10),
                //new GunListing("Taurus PT740B w/magazine extension", "PDA2", 11),
                //new GunListing("Glock G27 (.40) &G26 9MM 12 round magazines", "PDA2", 12),
                //new GunListing("Springfield XDS 45 without extension", "PDA3", 1),
                //new GunListing("Bersa Thunder", "PDA3", 2),
                //new GunListing("Glock G27 (.40) & G26 (9mm) with flush magazine", "PDA3", 3),
                //new GunListing("Glock 29 w/flush magazine", "PDA3", 4),
                //new GunListing("FEG PA63 (9X18)", "PDA3", 5),
                //new GunListing("Para-Ordnance Wart Hog", "PDA3", 6),
                //new GunListing("Sig-Saur P250 Subcompact", "PDA3", 7),
                //new GunListing("Glock S-Compacts", "PDA3", 8),
                //new GunListing("Ruger LCR-38", "PDA3", 9),
                //new GunListing("S&W 442 1.875”", "PDA3", 10),
                //new GunListing("S&W M&P 40 Compact - w/ flush magazine", "PDA3", 11),
                //new GunListing("Taurus Model 85-2” barrel", "PDA3", 12),
                //new GunListing("Colt Detective Special", "PDA3", 13),
                //new GunListing("Smith J-frame 38 shrouded", "PDA3", 14),
                //new GunListing("Smith & Wesson Bodyguard 38", "PDA3", 15),
                //new GunListing("Taurus 905-9mm revolver", "PDA3", 16),
                //new GunListing("Smith & Wesson Bodyguard 38 fits", "PDA3", 17),
                //new GunListing("S&W MODEL 640 CENTENNIAL", "PDA3", 18),
                //new GunListing("Taurus 709B Slim/9MM 7+1", "PDA3", 19),
                //new GunListing("Walther PPS with small and medium magazines", "PDA4", 1),
                //new GunListing("Beretta Nano w/+2 ext", "PDA4", 2),
                //new GunListing("Diamondback DB-9", "PDA4", 3),
                //new GunListing("Kahr CW40", "PDA4", 4),
                //new GunListing("Kahr CW9", "PDA4", 5),
                //new GunListing("Kel-Tec P11 w/Laser", "PDA4", 6),
                //new GunListing("Polish P64", "PDA4", 7),
                //new GunListing("Ruger LC9 w/out extension", "PDA4", 8),
                //new GunListing("TAURUS PT-25", "PDA4", 9),
                //new GunListing("Walther PPK 380/PPKS 9mm-40SW", "PDA4", 10),
                //new GunListing("SIG P238 w/7-Round extended magazine", "PDA4", 11),
                //new GunListing("Sig-Sauer P238 w/7-round magazine", "PDA4", 12),
                //new GunListing("Sig P938 w/extension", "PDA4", 13),
                //new GunListing("Springfield XDS 45 without extension", "PDA4", 14),
                //new GunListing("Cobra PAT380", "PDA4", 15),
                //new GunListing("Colt Gov Model IV 380", "PDA4", 16),
                //new GunListing("SCCY CPX2 with finger extension magazine", "PDA4", 17),
                //new GunListing("Springfield XD 9mm & 40SW 3” Sub-Compact", "PDA4", 18),
                //new GunListing("Walther PPK 380", "PDA4", 19),
                //new GunListing("Walther PPK/S 9MM", "PDA4", 20),
                //new GunListing("Walter PPK/S 380", "PDA4", 21),
                //new GunListing("Bersa Thunder 380 Concealed Carry", "PDA4", 22),
                //new GunListing("Colt Mustang II seven round", "PDA4", 23),
                //new GunListing("Smith & Wesson M&P Shield 9mm or .40 fit", "PDA4", 24),
                //new GunListing("Kahr CM40 & CM9", "PDA4", 25),
                //new GunListing("Taurus PT740B without extensions", "PDA4", 26),
                //new GunListing("Glock G30S fits", "PDA4", 27),
                //new GunListing("Sig-Saur P232", "PDA4", 28),
                //new GunListing("Taurus 709B Slim/9MM 7+1", "PDA4", 29),
                //new GunListing("Sig P938 w/o extension", "PDA5", 1),
                //new GunListing("Kahr P380 Finger extension", "PDA5", 2),
                //new GunListing("Taurus PT22 fits", "PDA5", 3),
                //new GunListing("Kimber Solo", "PDA5", 4),
                //new GunListing("Colt Mustang 380", "PDA5", 5),
                //new GunListing("NAA Guardian .32 & .380 with pinky extension", "PDA5", 6),
                //new GunListing("SCCY CPX2 with flush magazine", "PDA5", 7),
                //new GunListing("SIG P238 w/flush magazine", "PDA5", 8),
                //new GunListing("Sig-Sauer P238 w/Flush magazine", "PDA5", 9),
                //new GunListing("Beretta Bobcat 22/25 - Finger extension", "PDA5", 10),
                //new GunListing("Beretta Nano w/+2 ext w/ flush magazine", "PDA5", 11),
                //new GunListing("Beretta Nano with flush magazine", "PDA5", 12),
                //new GunListing("Kahr CM40 & CM9", "PDA5", 13),
                //new GunListing("Kel-Tec P-3AT+Ext", "PDA5", 14),
                //new GunListing("Sig P290", "PDA5", 15),
                //new GunListing("Smith & Wesson Bodyguard 380", "PDA5", 16),
                //new GunListing("Micro Carry .380ACP Solo Carry 9mm", "PDA5", 17),
                //new GunListing("Kahr PM9", "PDA5", 18),
                //new GunListing("Kel-Tec P-11", "PDA5", 19),
                //new GunListing("Kel-Tec PF-9", "PDA5", 20),
                //new GunListing("DIAMONDBACK DB380", "PDA6", 1),
                //new GunListing("Cobra CA380", "PDA6", 2),
                //new GunListing("Ruger LCP with flush magazine", "PDA6", 3),
                //new GunListing("Beretta Bobcat 22/25", "PDA6", 5),
                //new GunListing("Beretta Tomcat", "PDA6", 6),
                //new GunListing("Kahr P380", "PDA6", 7),
                //new GunListing("Kel-Tec P-3AT", "PDA6", 8),
                //new GunListing("KEL-TEC P-32", "PDA6", 9),
                //new GunListing("Ruger LCP w/extension", "PDA6", 10),
                //new GunListing("NAA Guardian .32 & .380", "PDA6", 11),
                //new GunListing("MR ME380", "PDA6", 12)
            };

            string Filename = "C:\\backup\\Workfiles\\SneakyPeteHolsterImport2.txt";

            Startbutton.Library.AppendToTextFile(Filename, "\"TemplateType=Sports\"	\"Version=2013.0903\"	\"The top 3 rows are for Amazon.com use only. Do not modify or delete the top 3 rows.\"									\"Offer - These attributes are required to make your item buyable for customers on the site\"																							\"Dimension - These attributes specify the size and weight of a product\"																							\"Discovery - These attributes have an effect on how customers can find your product on the site using browse or search\"																																	\"Image - These attributes provide links to images for a product\"										\"Fulfillment - Use these columns to provide fulfillment-related information for either Amazon-fulfilled (FBA) or seller-fulfilled orders.\"							\"Variation - Populate these attributes if your product is available in different variations (for example color or wattage)\"				\"Compliance - Attributes used to comply with consumer laws in the country or region where the item is sold\"								\"Ungrouped - These attributes create rich product listings for your buyers.\"																																																																																																																																																																																																																																																																																																																															\n");
            Startbutton.Library.AppendToTextFile(Filename, "\"SKU\"	\"Product ID\"	\"Product ID Type\"	\"Product Name\"	\"Product Description\"	\"Manufacturer\"	\"Manufacturer Part Number\"	\"Product Type\"	\"Brand Name\"	\"Item Type Keyword\"	\"Update Delete\"	\"Standard Price\"	\"Currency\"	\"Item Condition\"	\"Offer Condition Note\"	\"Quantity\"	\"Manufacturer's Suggested Retail Price\"	\"Minimum Advertised Price\"	\"Launch Date\"	\"Offering Release Date\"	\"Restock Date\"	\"Fulfillment Latency\"	\"Product Tax Code\"	\"Sale Price\"	\"Sale Start Date\"	\"Sale End Date\"	\"Max Aggregate Ship Quantity\"	\"Package Quantity\"	\"Offering Can Be Gift Messaged\"	\"Is Gift Wrap Available\"	\"Is Discontinued by Manufacturer\"	\"Product ID Override\"	\"Number of Items\"	\"Scheduled Delivery SKU List\"	\"Shipping Weight\"	\"Website Shipping Weight Unit Of Measure\"	\"Width\"	\"Item Display Width Unit Of Measure\"	\"Volume\"	\"Item Display Length\"	\"Item Display Length Unit Of Measure\"	\"Item Display Weight\"	\"Item Display Weight Unit Of Measure\"	\"Diameter\"	\"Item Display Diameter Unit Of Measure\"	\"Volume\"	\"Item Volume Unit Of Measure\"	\"Display Height\"	\"Item Display Height Unit Of Measure\"	\"Item Height\"	\"Item Height Unit Of Measure\"	\"Item Length\"	\"Item Length Unit Of Measure\"	\"Item Width\"	\"Item Width Unit Of Measure\"	\"Item Weight\"	\"Item Weight Unit Of Measure\"	\"Key Product Features1\"	\"Key Product Features2\"	\"Key Product Features3\"	\"Key Product Features4\"	\"Key Product Features5\"	\"Intended Use1\"	\"Intended Use2\"	\"Intended Use3\"	\"Intended Use4\"	\"Intended Use5\"	\"Target Audience1\"	\"Target Audience2\"	\"Target Audience3\"	\"Other Attributes1\"	\"Other Attributes2\"	\"Other Attributes3\"	\"Other Attributes4\"	\"Other Attributes5\"	\"Subject Matter1\"	\"Subject Matter2\"	\"Subject Matter3\"	\"Subject Matter4\"	\"Subject Matter5\"	\"Search Terms1\"	\"Search Terms2\"	\"Search Terms3\"	\"Search Terms4\"	\"Search Terms5\"	\"Platinum Keywords1\"	\"Platinum Keywords2\"	\"Platinum Keywords3\"	\"Platinum Keywords4\"	\"Platinum Keywords5\"	\"Main Image URL\"	\"Swatch Image URL\"	\"Other Image URL1\"	\"Other Image URL2\"	\"Other Image URL3\"	\"Other Image URL4\"	\"Other Image URL5\"	\"Other Image URL6\"	\"Other Image URL7\"	\"Other Image URL8\"	\"Fulfillment Center ID\"	\"Package Height\"	\"Package Width\"	\"Package Length\"	\"Package Length Unit Of Measure\"	\"Package Weight\"	\"Package Weight Unit Of Measure\"	\"Parentage\"	\"Parent SKU\"	\"Relationship Type\"	\"Variation Theme\"	\"Consumer Notice\"	\"CPSIA Cautionary Statement1\"	\"CPSIA Cautionary Statement2\"	\"CPSIA Cautionary Statement3\"	\"CPSIA Cautionary Statement4\"	\"CPSIA Cautionary Description\"	\"Country of Publication\"	\"Legal Disclaimer\"	\"Mfg Warranty Type (i.e. Parts, Labor)\"	\"Seller Warranty Description\"	\"Color\"	\"Color Map\"	\"Closure Type\"	\"Design\"	\"Event Name\"	\"Fabric Type1\"	\"Fabric Type2\"	\"Fabric Type3\"	\"Import Designation\"	\"Country as Labeled\"	\"Fur Description\"	\"Included Features\"	\"Material Type\"	\"Package Contents\"	\"Seasons\"	\"Size Map\"	\"Size\"	\"Skill Level\"	\"Sport Type\"	\"Floor Length\"	\"Floor Length Unit Of Measure\"	\"Floor Width\"	\"Floor Width Unit Of Measure\"	\"Minimum Tension Rating\"	\"Minimum Tension Rating Unit Of Measure\"	\"Maximum Stride Length\"	\"Maximum Stride Length Unit Of Measure\"	\"Watch Movement Type\"	\"Number Of Resistance Levels\"	\"Target Zones\"	\"Maximum Tension Rating\"	\"Maximum Tension Rating Unit Of Measure\"	\"Number Of Carriage Positions\"	\"Number Of Exercises\"	\"Number Of Foot Positions\"	\"Number Of Head Positions\"	\"Number Of Springs\"	\"Maximum Incline Percentage\"	\"Control Program Name\"	\"Resistance Mechanism\"	\"Scale\"	\"Speed Rating\"	\"Number of Programs\"	\"Construction Type\"	\"Alarm\"	\"Maximum Resistance\"	\"Maximum Resistance Unit Of Measure\"	\"Ingredients\"	\"Floor Area\"	\"Floor Area Unit Of Measure\"	\"Belt Style\"	\"Bottom Style\"	\"Collar Type\"	\"Cuff Type\"	\"Cup Size\"	\"Department1\"	\"Department2\"	\"Department3\"	\"Department4\"	\"Department5\"	\"Fabric Wash\"	\"Front Pleat Type\"	\"Glove Type\"	\"Leg Style\"	\"Neck Style\"	\"Pattern Style\"	\"Pocket Description\"	\"Shoe Width\"	\"Sleeve Type\"	\"Sleeve Length\"	\"Sleeve Length Unit Of Measure\"	\"Sock Height\"	\"Strap Type\"	\"Support Type\"	\"Theme\"	\"Top Style\"	\"Underwire Type\"	\"UV Protection\"	\"Waist Size\"	\"Waist Size Unit Of Measure\"	\"Wheel Type\"	\"Flex\"	\"Loft\"	\"Grip Size\"	\"Grip Type\"	\"Engine Displacement\"	\"Engine Displacement Unit Of Measure\"	\"Grip Material Type\"	\"Software Included\"	\"Thread Pitch\"	\"Rim Size\"	\"Rim Size Unit Of Measure\"	\"Crank Length\"	\"Crank Length Unit Of Measure\"	\"Frame Type\"	\"Top Tube Length\"	\"Top Tube Length Unit Of Measure\"	\"Wheel Size\"	\"Wheel Size Unit Of Measure\"	\"Brake Width\"	\"Brake Width Unit Of Measure\"	\"Seat Height\"	\"Seat Height Unit Of Measure\"	\"Bike Type\"	\"Inseam Length\"	\"Brake Style\"	\"Suspension Type\"	\"Frame Material Type\"	\"Assembly Instructions\"	\"Speed\"	\"Resistance\"	\"Frame Height\"	\"Frame Size Unit Of Measure\"	\"Lock Type\"	\"Amperage\"	\"Amperage Unit Of Measure\"	\"Diving Clothing Thickness\"	\"Line Weight\"	\"Tension Supported\"	\"Fishing Line Type\"	\"Lure Weight\"	\"Lure Weight Unit Of Measure\"	\"Backing Line Capacity\"	\"Bearing Material Type\"	\"Sonar Type\"	\"Beam Width\"	\"Beam Width Unit Of Measure\"	\"Centerline Length\"	\"Centreline Length Unit Of Measure\"	\"Hull Shape\"	\"Compatible Hose Diameter\"	\"Compatible Hose Diameter Unit Of Measure\"	\"Boom Length\"	\"Boom Length Unit Of Measure\"	\"Deck Length\"	\"Deck Width\"	\"Deck Dimensions Unit Of Measure\"	\"Number Of Speeds\"	\"Action\"	\"Number of Pieces\"	\"Handle Material\"	\"Breaking Point\"	\"Maximum Tension Load Unit Of Measure\"	\"Frequency Band\"	\"Display\"	\"Life Vest Type\"	\"HP\"	\"Cycles\"	\"Rotation\"	\"Handle Type\"	\"Subject\"	\"Manufacturer Series Number\"	\"Orientation\"	\"Water Resistance Depth\"	\"Water Resistance Depth Unit Of Measure\"	\"Hole Count\"	\"Model Name\"	\"Number Of Power Levels\"	\"Position Accuracy\"	\"Navigation Routes\"	\"Waypoints\"	\"Item Impact Force\"	\"Caliber\"	\"Rounds\"	\"Blade Shape\"	\"Boot Size\"	\"Calf Size\"	\"Capacity\"	\"Capacity Unit Of Measure\"	\"Flavor\"	\"Lens Color\"	\"Fitting Type\"	\"Number Of Pockets\"	\"Fuel Capacity\"	\"Fuel Capacity Unit Of Measure\"	\"Blade Type\"	\"Maximum Compatible Boot Size\"	\"Minimum Compatible Boot Size\"	\"Apparent Scale Size\"	\"Apparent Scale Size Unit Of Measure\"	\"Minimum Compatible Rope Diameter\"	\"Minimum Compatible Rope Diameter Unit Of Measure\"	\"CompatibleRopeDiameter\"	\"Maximum Compatible Rope Diameter Unit Of Measure\"	\"Turn Radius\"	\"Turn Radius Unit Of Measure\"	\"Number Of Doors\"	\"Static Elongation Percentage\"	\"UIAA Fall Rating\"	\"Number Of Gear Loops\"	\"Effective Edge Length\"	\"Effective Edge Length Unit Of Measure\"	\"Access Location\"	\"Boil Rate Description\"	\"Fill Material Type\"	\"Loudness\"	\"Water Bottle Cap Type\"	\"Mounting Type\"	\"State\"	\"Lamp\"	\"Number of Pages\"	\"GeographicCoverage\"	\"Static Load Capacity\"	\"Static Load Capacity Unit Of Measure\"	\"Heat Output\"	\"Heat Output Unit Of Measure\"	\"Number Of Blades\"	\"Temperature Rating\"	\"Temperature Rating Degrees Unit Of Measure\"	\"Blade Length\"	\"Blade Length Unit Of Measure\"	\"Lens Material\"	\"Lens Type\"	\"Maximum Height Recommendation\"	\"Maximum Height Recommendation Unit Of Measure\"	\"Fuel Type\"	\"BTUs\"	\"R-Value\"	\"Insulation Material Type\"	\"Pad Type\"	\"ShellMaterial\"	\"Lining Material\"	\"Breaking Strength\"	\"Breaking Strength Unit Of Measure\"	\"Minimum Torso Measurement\"	\"Tensile Strength Max\"	\"Tensile Strength Unit Of Measure\"	\"Occupancy\"	\"Number Of Poles\"	\"Folded Size\"	\"Light Source Type\"	\"Burn Time\"	\"Light Source Operating Life Unit Of Measure\"	\"Light Intensity\"	\"Cross Section Shape\"	\"Objective Lens Diameter\"	\"Objective Lens Diameter Unit Of Measure\"	\"Magnification Maximum\"	\"Magnification Minimum\"	\"Is Autographed\"	\"League Name\"	\"Curvature\"	\"Fencing Pommel Type\"	\"Hand Orientation\"	\"Head Size\"	\"Is Memorabilia\"	\"Shaft Length\"	\"Shaft Length Unit Of Measure\"	\"Shaft Material\"	\"Shaft Style Type\"	\"Shape\"	\"Wattage\"	\"Display Maximum Weight Recommendation\"	\"Display Maximum Weight Recommendation Unit Of Measure\"	\"Seating Capacity\"	\"Fit Type\"	\"Uniform Number\"	\"Lash Length\"	\"Lash Length Unit Of Measure\"	\"Maximum Pitch Speed\"	\"Maximum Pitch Speed Unit Of Measure\"	\"Guard Material Type\"	\"Athlete\"	\"Signed By\"	\"Style Name\"	\"Team Name\"	\"Operation Mode\"	\"Monitor Native Resolution\"	\"Power Source\"	\"Additional Features\"	\"Usage Capacity Unit Of Measure\"	\"Included Components\"	\"Display Color Support\"	\"Load Capacity\"	\"Load Capacity Unit Of Measure\"	\"Memory Storage Capacity\"	\"Memory Storage Capacity Unit Of Measure\"	\"Display Size\"	\"Display Size Unit Of Measure\"	\"Display Resolution Maximum\"	\"Number of Horses\"	\"Connector Type\"	\"Specific Uses For Product\"	\"Compatible Devices\"	\"Minimum Weight Recommendation\"	\"Minimum Weight Recommendation Unit Of Measure\"	\"Weight Supported\"	\"Maximum Weight Recommendation Unit Of Measure\"	\"Coverage Area\"	\"Item Area Unit Of Measure\"	\"Maximum Weight\"	\"Maximum Weight Capacity Unit Of Measure\"	\"Outside Diameter Derived\"	\"Item Diameter Unit Of Measure\"	\"Thickness Derived\"	\"Item Thickness Unit Of Measure\"	\"Unit Count\"	\"Unit Count Type\"	\"Batteries are Included\"	\"Are Batteries Required\"	\"Battery Type1\"	\"Battery Type2\"	\"Battery Type3\"	\"Number of Batteries1\"	\"Number of Batteries2\"	\"Number of Batteries3\"	\"Battery Average Life1\"	\"Battery Average Life2\"	\"Battery Average Life3\"	\"Battery Average Life Unit Of Measure1\"	\"Battery Average Life Unit Of Measure2\"	\"Battery Average Life Unit Of Measure3\"	\"Lithium Battery Energy Content\"	\"Lithium Battery Packaging\"	\"Lithium Battery Voltage\"	\"Lithium Battery Weight\"	\"Number of Lithium-ion Cells\"	\"Number of Lithium Metal Cells\"\n");
            Startbutton.Library.AppendToTextFile(Filename, "\"item_sku\"	\"external_product_id\"	\"external_product_id_type\"	\"item_name\"	\"product_description\"	\"manufacturer\"	\"part_number\"	\"feed_product_type\"	\"brand_name\"	\"item_type\"	\"update_delete\"	\"standard_price\"	\"currency\"	\"condition_type\"	\"condition_note\"	\"quantity\"	\"list_price\"	\"map_price\"	\"product_site_launch_date\"	\"merchant_release_date\"	\"restock_date\"	\"fulfillment_latency\"	\"product_tax_code\"	\"sale_price\"	\"sale_from_date\"	\"sale_end_date\"	\"max_aggregate_ship_quantity\"	\"item_package_quantity\"	\"offering_can_be_gift_messaged\"	\"offering_can_be_giftwrapped\"	\"is_discontinued_by_manufacturer\"	\"missing_keyset_reason\"	\"number_of_items\"	\"delivery_schedule_group_id\"	\"website_shipping_weight\"	\"website_shipping_weight_unit_of_measure\"	\"item_display_width\"	\"item_display_width_unit_of_measure\"	\"volume_capacity_name\"	\"item_display_length\"	\"item_display_length_unit_of_measure\"	\"item_display_weight\"	\"item_display_weight_unit_of_measure\"	\"item_display_diameter\"	\"item_display_diameter_unit_of_measure\"	\"item_volume\"	\"item_volume_unit_of_measure\"	\"item_display_height\"	\"item_display_height_unit_of_measure\"	\"item_height\"	\"item_height_unit_of_measure\"	\"item_length\"	\"item_length_unit_of_measure\"	\"item_width\"	\"item_width_unit_of_measure\"	\"item_weight\"	\"item_weight_unit_of_measure\"	\"bullet_point1\"	\"bullet_point2\"	\"bullet_point3\"	\"bullet_point4\"	\"bullet_point5\"	\"specific_uses_keywords1\"	\"specific_uses_keywords2\"	\"specific_uses_keywords3\"	\"specific_uses_keywords4\"	\"specific_uses_keywords5\"	\"target_audience_keywords1\"	\"target_audience_keywords2\"	\"target_audience_keywords3\"	\"thesaurus_attribute_keywords1\"	\"thesaurus_attribute_keywords2\"	\"thesaurus_attribute_keywords3\"	\"thesaurus_attribute_keywords4\"	\"thesaurus_attribute_keywords5\"	\"thesaurus_subject_keywords1\"	\"thesaurus_subject_keywords2\"	\"thesaurus_subject_keywords3\"	\"thesaurus_subject_keywords4\"	\"thesaurus_subject_keywords5\"	\"generic_keywords1\"	\"generic_keywords2\"	\"generic_keywords3\"	\"generic_keywords4\"	\"generic_keywords5\"	\"platinum_keywords1\"	\"platinum_keywords2\"	\"platinum_keywords3\"	\"platinum_keywords4\"	\"platinum_keywords5\"	\"main_image_url\"	\"swatch_image_url\"	\"other_image_url1\"	\"other_image_url2\"	\"other_image_url3\"	\"other_image_url4\"	\"other_image_url5\"	\"other_image_url6\"	\"other_image_url7\"	\"other_image_url8\"	\"fulfillment_center_id\"	\"package_height\"	\"package_width\"	\"package_length\"	\"package_length_unit_of_measure\"	\"package_weight\"	\"package_weight_unit_of_measure\"	\"parent_child\"	\"parent_sku\"	\"relationship_type\"	\"variation_theme\"	\"prop_65\"	\"cpsia_cautionary_statement1\"	\"cpsia_cautionary_statement2\"	\"cpsia_cautionary_statement3\"	\"cpsia_cautionary_statement4\"	\"cpsia_cautionary_description\"	\"country_of_origin\"	\"legal_disclaimer_description\"	\"mfg_warranty_description_type\"	\"seller_warranty_description\"	\"color_name\"	\"color_map\"	\"closure_type\"	\"pattern_name\"	\"event_name\"	\"fabric_type1\"	\"fabric_type2\"	\"fabric_type3\"	\"import_designation\"	\"country_as_labeled\"	\"fur_description\"	\"included_features\"	\"material_type\"	\"item_package_contents\"	\"seasons\"	\"size_map\"	\"size_name\"	\"skill_level\"	\"sport_type\"	\"floor_length\"	\"floor_length_unit_of_measure\"	\"floor_width\"	\"floor_width_unit_of_measure\"	\"minimum_tension_rating\"	\"minimum_tension_rating_unit_of_measure\"	\"maximum_stride_length\"	\"maximum_stride_length_unit_of_measure\"	\"watch_movement_type\"	\"number_of_resistance_levels\"	\"target_zone_calculation_type\"	\"maximum_tension_rating\"	\"maximum_tension_rating_unit_of_measure\"	\"number_of_carriage_positions\"	\"number_of_exercises\"	\"number_of_foot_positions\"	\"number_of_head_positions\"	\"number_of_springs\"	\"maximum_incline_percentage\"	\"control_program_name\"	\"resistance_mechanism\"	\"scale_name\"	\"speed_rating\"	\"number_of_programs\"	\"construction_type\"	\"alarm\"	\"maximum_resistance\"	\"maximum_resistance_unit_of_measure\"	\"ingredients\"	\"floor_area\"	\"floor_area_unit_of_measure\"	\"belt_style\"	\"bottom_style\"	\"collar_style\"	\"cuff_type\"	\"cup_size\"	\"department_name1\"	\"department_name2\"	\"department_name3\"	\"department_name4\"	\"department_name5\"	\"fabric_wash\"	\"front_style\"	\"glove_type\"	\"leg_style\"	\"neck_style\"	\"pattern_type\"	\"pocket_description\"	\"shoe_width\"	\"sleeve_type\"	\"sleeve_length\"	\"sleeve_length_unit_of_measure\"	\"rise_style\"	\"strap_type\"	\"support_type\"	\"theme\"	\"top_style\"	\"underwire_type\"	\"ultraviolet_light_protection\"	\"waist_size\"	\"waist_size_unit_of_measure\"	\"wheel_type\"	\"golf_club_flex\"	\"golf_club_loft\"	\"grip_size\"	\"grip_type\"	\"engine_displacement\"	\"engine_displacement_unit_of_measure\"	\"grip_material_type\"	\"software_included\"	\"thread_pitch_string\"	\"rim_size\"	\"rim_size_unit_of_measure\"	\"crank_length\"	\"crank_length_unit_of_measure\"	\"frame_type\"	\"top_tube_length\"	\"top_tube_length_unit_of_measure\"	\"wheel_size\"	\"wheel_size_unit_of_measure\"	\"brake_width\"	\"brake_width_unit_of_measure\"	\"seat_height\"	\"seat_height_unit_of_measure\"	\"bike_type\"	\"inseam_length\"	\"brake_style\"	\"suspension_type\"	\"frame_material_type\"	\"assembly_instructions\"	\"speed\"	\"resistance\"	\"frame_size\"	\"frame_size_unit_of_measure\"	\"lock_type\"	\"amperage\"	\"amperage_unit_of_measure\"	\"diving_clothing_thickness\"	\"line_weight\"	\"tension_level\"	\"fishing_line_type\"	\"lure_weight\"	\"lure_weight_unit_of_measure\"	\"backing_line_capacity\"	\"bearing_material_type\"	\"sonar_type\"	\"beam_width\"	\"beam_width_unit_of_measure\"	\"centerline_length\"	\"centerline_length_unit_of_measure\"	\"hull_shape\"	\"compatible_hose_diameter\"	\"compatible_hose_diameter_unit_of_measure\"	\"boom_length\"	\"boom_length_unit_of_measure\"	\"deck_length\"	\"deck_width\"	\"deck_dimensions_unit_of_measure\"	\"number_of_speeds\"	\"action\"	\"number_of_pieces\"	\"handle_material\"	\"maximum_tension_load\"	\"maximum_tension_load_unit_of_measure\"	\"frequency_bands_supported\"	\"display_type\"	\"life_vest_type\"	\"maximum_horsepower\"	\"motor_type\"	\"rotation_direction\"	\"handle_type\"	\"unknown_subject\"	\"mfg_series_number\"	\"orientation\"	\"water_resistance_depth\"	\"water_resistance_depth_unit_of_measure\"	\"hole_count\"	\"model_name\"	\"number_of_power_levels\"	\"location_accuracy\"	\"navigation_routes\"	\"waypoints\"	\"item_impact_force\"	\"caliber\"	\"number_of_rounds\"	\"blade_shape\"	\"boot_size\"	\"calf_size\"	\"capacity\"	\"capacity_unit_of_measure\"	\"flavor_name\"	\"lens_color\"	\"fitting_type\"	\"number_of_pockets\"	\"fuel_capacity\"	\"fuel_capacity_unit_of_measure\"	\"blade_edge_type\"	\"maximum_compatible_boot_size\"	\"minimum_compatible_boot_size\"	\"apparent_scale_size\"	\"apparent_scale_size_unit_of_measure\"	\"minimum_compatible_rope_diameter\"	\"minimum_compatible_rope_diameter_unit_of_measure\"	\"maximum_compatible_rope_diameter\"	\"maximum_compatible_rope_diameter_unit_of_measure\"	\"turn_radius\"	\"turn_radius_unit_of_measure\"	\"number_of_doors\"	\"static_elongation_percentage\"	\"uiaa_fall_rating\"	\"number_of_gear_loops\"	\"effective_edge_length\"	\"effective_edge_length_unit_of_measure\"	\"access_location\"	\"boil_rate_description\"	\"fill_material_type\"	\"sound_pressure\"	\"cap_type\"	\"mounting_type\"	\"state_string\"	\"lamp_type\"	\"pages\"	\"map_type\"	\"static_load_capacity\"	\"static_load_capacity_unit_of_measure\"	\"heat_output\"	\"heat_output_unit_of_measure\"	\"number_of_blades\"	\"temperature_rating_degrees\"	\"temperature_rating_degrees_unit_of_measure\"	\"blade_length\"	\"blade_length_unit_of_measure\"	\"lens_material_type\"	\"lens_type\"	\"maximum_height_recommendation\"	\"maximum_height_recommendation_unit_of_measure\"	\"fuel_type\"	\"maximum_energy_output\"	\"insulation_resistance\"	\"insulation_material_type\"	\"pad_type\"	\"outer_material_type\"	\"inner_material_type\"	\"breaking_strength\"	\"breaking_strength_unit_of_measure\"	\"minimum_torso_measurement\"	\"tensile_strength\"	\"tensile_strength_unit_of_measure\"	\"occupancy\"	\"number_of_poles\"	\"folded_size\"	\"light_source_type\"	\"light_source_operating_life\"	\"light_source_operating_life_unit_of_measure\"	\"luminous_intensity\"	\"cross_section_shape\"	\"objective_lens_diameter\"	\"objective_lens_diameter_unit_of_measure\"	\"magnification_maximum\"	\"magnification_minimum\"	\"is_autographed\"	\"league_name\"	\"curvature\"	\"fencing_pommel_type\"	\"hand_orientation\"	\"head_size\"	\"is_memorabilia\"	\"shaft_length\"	\"shaft_length_unit_of_measure\"	\"shaft_material\"	\"shaft_style_type\"	\"item_shape\"	\"wattage\"	\"display_maximum_weight_recommendation\"	\"display_maximum_weight_recommendation_unit_of_measure\"	\"seating_capacity\"	\"fit_type\"	\"uniform_number\"	\"lash_length\"	\"lash_length_unit_of_measure\"	\"maximum_pitch_speed\"	\"maximum_pitch_speed_unit_of_measure\"	\"guard_material_type\"	\"athlete\"	\"signed_by\"	\"style_name\"	\"team_name\"	\"operation_mode\"	\"native_resolution\"	\"power_source_type\"	\"special_features\"	\"capacity_name_unit_of_measure\"	\"included_components\"	\"display_color_support\"	\"load_capacity\"	\"load_capacity_unit_of_measure\"	\"memory_storage_capacity\"	\"memory_storage_capacity_unit_of_measure\"	\"display_size\"	\"display_size_unit_of_measure\"	\"display_resolution_maximum\"	\"capacity_name\"	\"connector_type\"	\"specific_uses_for_product\"	\"compatible_devices\"	\"minimum_weight_recommendation\"	\"minimum_weight_recommendation_unit_of_measure\"	\"maximum_weight_recommendation\"	\"maximum_weight_recommendation_unit_of_measure\"	\"item_area\"	\"item_area_unit_of_measure\"	\"maximum_weight_capacity\"	\"maximum_weight_capacity_unit_of_measure\"	\"item_diameter_derived\"	\"item_diameter_unit_of_measure\"	\"item_thickness_derived\"	\"item_thickness_unit_of_measure\"	\"unit_count\"	\"unit_count_type\"	\"are_batteries_included\"	\"batteries_required\"	\"battery_type1\"	\"battery_type2\"	\"battery_type3\"	\"number_of_batteries1\"	\"number_of_batteries2\"	\"number_of_batteries3\"	\"battery_average_life1\"	\"battery_average_life2\"	\"battery_average_life3\"	\"battery_average_life_unit_of_measure1\"	\"battery_average_life_unit_of_measure2\"	\"battery_average_life_unit_of_measure3\"	\"lithium_battery_energy_content\"	\"lithium_battery_packaging\"	\"lithium_battery_voltage\"	\"lithium_battery_weight\"	\"number_of_lithium_ion_cells\"	\"number_of_lithium_metal_cells\"\n");

            foreach (GunListing gl in gs)
                WriteGunListingToFlatFile(db, Filename, gl.Model, gl.PDAModel, gl.PDANumber);

            //int BatchSize = 3;
            //int MaxBatches = 10000;

            //for (int x = 0; x < gs.Length && x < MaxBatches; x += BatchSize)
            //{
            //    NewAmazonProduct.MessageBatches mb = new NewAmazonProduct.MessageBatches();

            //    foreach (GunListing gl in gs.ToList().Skip(x).Take(BatchSize))
            //        CreateGunListing(db, Account, gl.Model, gl.PDAModel, gl.PDANumber, mb);

            //    mb.Submit(program.Log);
            //}

        }

        static void WriteGunListingToFlatFile(Entities db, string Filename, string GunModel, string PDAModel, int PDANumber)
        {
            decimal MSRP = (decimal)49.99;
            decimal Price = (decimal)39.95;

            string[] pics = new string[] {
                "http://ecx.images-amazon.com/images/I/91v3ZQ18lVL._SL1500_.jpg",
                "http://ecx.images-amazon.com/images/I/91Gw2Mv-oqL._SL1500_.jpg",
                "http://ecx.images-amazon.com/images/I/91RqKDYTcnL._SL1500_.jpg",
                "http://ecx.images-amazon.com/images/I/91NpyAi7a7L._SL1500_.jpg",
                "http://ecx.images-amazon.com/images/I/717VJT2unWL._SL1500_.jpg"
            };

            WriteGunListingLine(db, Filename, GunModel, "Ballistic Nylon", MSRP, Price, PDAModel + "-BN-" + PDANumber.ToString(), null, null, pics);
            WriteGunListingLine(db, Filename, GunModel, "Ballistic Nylon", MSRP, Price, PDAModel + "-BN-R-" + PDANumber.ToString(), "Right", PDAModel + "-BN-" + PDANumber.ToString(), pics);
            WriteGunListingLine(db, Filename, GunModel, "Ballistic Nylon", MSRP, Price, PDAModel + "-BN-L-" + PDANumber.ToString(), "Left", PDAModel + "-BN-" + PDANumber.ToString(), pics);

            pics = new string[] {
                "http://www.revupcommerce.com/images/holsters/leather1.jpg",
                "http://www.revupcommerce.com/images/holsters/leather2.jpg",
                "http://www.revupcommerce.com/images/holsters/leather3.jpg"
            };

            WriteGunListingLine(db, Filename, GunModel, "Bonded Leather", MSRP + 10, Price + 10, PDAModel + "-BL-" + PDANumber.ToString(), null, null, pics);
            WriteGunListingLine(db, Filename, GunModel, "Bonded Leather", MSRP + 10, Price + 10, PDAModel + "-BL-R-" + PDANumber.ToString(), "Right", PDAModel + "-BL-" + PDANumber.ToString(), pics);
            WriteGunListingLine(db, Filename, GunModel, "Bonded Leather", MSRP + 10, Price + 10, PDAModel + "-BL-L-" + PDANumber.ToString(), "Left", PDAModel + "-BL-" + PDANumber.ToString(), pics);

        }

        static void WriteGunListingLine(Entities db, string Filename, string GunModel, string FabricType, decimal MSRP, decimal Price, string SKU, string VariationText, string ParentSKU, string[] pics)
        {
            Dictionary<string, object> cells = new Dictionary<string, object>();

            cells["A"] = SKU;
            cells["B"] = Library.GetUPCFromSKU(db, SKU);
            cells["C"] = "UPC";
            cells["D"] = "Be a Sneaky Pete with PDA HOLSTER for your " + GunModel + " (" + FabricType + ")";

            if (VariationText != null)
                cells["D"] += " (" + VariationText + " Handed)";
            
            cells["E"] = "<b>Be a Sneaky Pete with your PDA Holster and SAFELY conceal your " + GunModel + @"!</b><br /><br />
                PDA Holsters has answered the question of how do you comfortably and safely carry your firearm while also being in compliance with your state or local
                concealed carry laws.  THE 2 IN 1 design (WITH OPTIONAL PADDLE INSERT ADD ON) enables you to carry your firearm concealed with and without a belt, on the
                outside of your pants so that you have easy access if the need ever arises.<br />
                <br />
                Our unique design has 3 amazing features that no other holster has:<br />
                <ol>
                <li>Our ""Quick Draw"" cutout that enables you to firmly get your pointer and
                middle finger under the grip to be able to quickly draw your weapon.  No other holster of this kind enables that and when you need it most, ease of access is
                more important than almost everything else.</li>
                <li>Our ""Trigger Guard"" design also makes it the safest holster by shielding the trigger as it comes out
                of the holster so as to prevent accidental fire during the unholstering process.  This makes the PDA holster the safest of its kind.</li>
                <li>Being discreet is the name of the game and our pen pocket further disguises the fact that this is not your cell phone or a pda but rather a concealed
                firearm at the ready.  Even after staring at the PDA holster, you will not be able to tell that this is a firearm carry case.
                Made out of " + FabricType + @", the PDA Holster ensures that this will stand up to the rigors of life and will be the last holster you ever need to buy.</li>
                </ol>
            ";
            cells["F"] = "PDAHOLSTERS.COM";
            cells["I"] = "PDA Holster";
            cells["J"] = "general-sporting-equipment";
            cells["L"] = Price;
            cells["M"] = "USD";
            cells["N"] = "NEW";
            cells["P"] = 9999;
            cells["Q"] = MSRP;
            cells["BF"] = "Fits: " + GunModel;
            cells["BG"] = "Right hand and left hand orientation available";
            cells["BH"] = "Worn with belt and optional paddle accessory enables wearing without a belt";
            cells["BI"] = "Safest openly concealed carry holster on the market due to our \"trigger guarded design\"!";
            cells["BJ"] = "The only quick draw holster that enables you to openly conceal your firearm on the market today!";
            cells["CC"] = "Sneaky Pete Holster";
            cells["CD"] = "Sneaky Pete Holsters";
            cells["CE"] = GunModel;
            cells["CM"] = pics[0];

            if (pics.Length > 1)
                cells["CO"] = pics[1];

            if (pics.Length > 2)
                cells["CP"] = pics[2];

            if (pics.Length > 3)
                cells["CQ"] = pics[3];

            if (pics.Length > 4)
                cells["CR"] = pics[4];

            if (pics.Length > 5)
                cells["CS"] = pics[5];

            if (pics.Length > 6)
                cells["CT"] = pics[6];

            if (pics.Length > 7)
                cells["CU"] = pics[7];

            if (pics.Length > 8)
                cells["CV"] = pics[8];

            if (ParentSKU == null)
                cells["DD"] = "parent";
            else
            {
                cells["DD"] = "child";
                cells["DE"] = ParentSKU;
                cells["DF"] = "VARIATION";
            }

            cells["DG"] = "HAND";
            cells["DW"] = FabricType;
            cells["NA"] = VariationText;

            WriteLineFromCells(Filename, cells);

        }

        static void WriteLineFromCells(string Filename, Dictionary<string, object> Cells)
        {
            string Line = "";

            for (int x = 0; x < 439; x++)
            {
                string cell = "";

                if (x > 25)
                    cell += Convert.ToChar((x / 26) + 64);

                cell += Convert.ToChar((x % 26) + 65);

                if (!Cells.ContainsKey(cell) || Cells[cell] == null)
                    Line += (char)9;
                else
                {
                    Line += "\"" + Cells[cell].ToString().Replace("\"", "\"\"").Replace("\r\n", "") + "\"" + (char)9;
                }

            }


            Startbutton.Library.AppendToTextFile(Filename, Line + "\r\n");
            Console.Write(Filename, Line + "\r\n");
        }

        static void CreateGunListing(Entities db, AmazonAccount Account, string GunModel, string PDAModel, int PDANumber, NewAmazonProduct.MessageBatches mb)
        {
            decimal MSRP = (decimal)49.99;
            decimal Price = (decimal)39.95;

            string[] pics = new string[] {
                "http://ecx.images-amazon.com/images/I/91v3ZQ18lVL._SL1500_.jpg",
                "http://ecx.images-amazon.com/images/I/91Gw2Mv-oqL._SL1500_.jpg",
                "http://ecx.images-amazon.com/images/I/91RqKDYTcnL._SL1500_.jpg",
                "http://ecx.images-amazon.com/images/I/91NpyAi7a7L._SL1500_.jpg",
                "http://ecx.images-amazon.com/images/I/717VJT2unWL._SL1500_.jpg"
            };

            mb.Add(Account, CreateHolsterListing(db, Account, GunModel, "Ballistic Nylon", MSRP, Price, PDAModel + "-BN-" + PDANumber.ToString(), null, null, pics));
            mb.Add(Account, CreateHolsterListing(db, Account, GunModel, "Ballistic Nylon", MSRP, Price, PDAModel + "-BN-R-" + PDANumber.ToString(), "Right", PDAModel + "-BN-" + PDANumber.ToString(), pics));
            mb.Add(Account, CreateHolsterListing(db, Account, GunModel, "Ballistic Nylon", MSRP, Price, PDAModel + "-BN-L-" + PDANumber.ToString(), "Left", PDAModel + "-BN-" + PDANumber.ToString(), pics));

            pics = new string[] {
                "http://www.revupcommerce.com/images/holsters/leather1.jpg",
                "http://www.revupcommerce.com/images/holsters/leather2.jpg",
                "http://www.revupcommerce.com/images/holsters/leather3.jpg"
            };

            mb.Add(Account, CreateHolsterListing(db, Account, GunModel, "Bonded Leather", MSRP + 10, Price + 10, PDAModel + "-BL-" + PDANumber.ToString(), null, null, pics));
            mb.Add(Account, CreateHolsterListing(db, Account, GunModel, "Bonded Leather", MSRP + 10, Price + 10, PDAModel + "-BL-R-" + PDANumber.ToString(), "Right", PDAModel + "-BL-" + PDANumber.ToString(), pics));
            mb.Add(Account, CreateHolsterListing(db, Account, GunModel, "Bonded Leather", MSRP + 10, Price + 10, PDAModel + "-BL-L-" + PDANumber.ToString(), "Left", PDAModel + "-BL-" + PDANumber.ToString(), pics));

        }

        static NewAmazonProduct.ProductInfo CreateHolsterListing(Entities db, AmazonAccount Account, string GunModel, string FabricType, decimal MSRP, decimal Price, string SKU, string VariationText, string ParentSKU, string[] pics)
        {

            string UPC = Library.GetUPCFromSKU(db, SKU);

            NewAmazonProduct.ProductInfo pi = new NewAmazonProduct.ProductInfo();

            Amazon.XML.Product p = new Amazon.XML.Product();

            p.SKU = SKU;
            
            p.ProductData = new Amazon.XML.ProductProductData();

            Amazon.XML.Sports item = new Amazon.XML.Sports();
            item.ProductType = Amazon.XML.SportsProductType.SportingGoods;
            item.FabricType = FabricType;

            p.StandardProductID = new Amazon.XML.StandardProductID();
            p.StandardProductID.Type = Amazon.XML.StandardProductIDType.UPC;
            p.StandardProductID.Value = UPC;

            p.DescriptionData = new Amazon.XML.ProductDescriptionData();
            p.DescriptionData.Manufacturer = "PDA Holster";
            p.DescriptionData.ItemType = "general-sporting-equipment";
            p.DescriptionData.Title = "Be a Sneaky Pete with PDA HOLSTER for your " + GunModel + " (" + FabricType + ")";
            
            if (VariationText != null)
                p.DescriptionData.Title += " (" + VariationText + " Handed)";

            item.VariationData = new Amazon.XML.SportsVariationData();
            item.VariationData.ParentageSpecified = true;
            item.VariationData.VariationThemeSpecified = true;

            if (ParentSKU == null)
            {
                item.VariationData.VariationTheme = Amazon.XML.SportsVariationDataVariationTheme.Hand;
                item.VariationData.Parentage = Amazon.XML.SportsVariationDataParentage.parent;
            }
            else
            {
                item.VariationData.VariationTheme = Amazon.XML.SportsVariationDataVariationTheme.Hand;
                item.VariationData.Parentage = Amazon.XML.SportsVariationDataParentage.child;
                item.VariationData.Hand = VariationText;

                p.DescriptionData.SearchTerms = new string[] {
                    "Sneaky Pete Holster",
                    "Sneaky Pete Holsters",
                    GunModel
                };


            }

            p.ProductData.Item = item;
            
            p.DescriptionData.Brand = "PDAHOLSTERS.COM";
            p.DescriptionData.Description = "<b>Be a Sneaky Pete with your PDA Holster and SAFELY conceal your " + GunModel + @"!</b><br /><br />
                PDA Holsters has answered the question of how do you comfortably and safely carry your firearm while also being in compliance with your state or local
                concealed carry laws.  THE 2 IN 1 design (WITH OPTIONAL PADDLE INSERT ADD ON) enables you to carry your firearm concealed with and without a belt, on the
                outside of your pants so that you have easy access if the need ever arises.<br />
                <br />
                Our unique design has 3 amazing features that no other holster has:<br />
                <ol>
                <li>Our ""Quick Draw"" cutout that enables you to firmly get your pointer and
                middle finger under the grip to be able to quickly draw your weapon.  No other holster of this kind enables that and when you need it most, ease of access is
                more important than almost everything else.</li>
                <li>Our ""Trigger Guard"" design also makes it the safest holster by shielding the trigger as it comes out
                of the holster so as to prevent accidental fire during the unholstering process.  This makes the PDA holster the safest of its kind.</li>
                <li>Being discreet is the name of the game and our pen pocket further disguises the fact that this is not your cell phone or a pda but rather a concealed
                firearm at the ready.  Even after staring at the PDA holster, you will not be able to tell that this is a firearm carry case.
                Made out of " + FabricType + @", the PDA Holster ensures that this will stand up to the rigors of life and will be the last holster you ever need to buy.</li>
                </ol>
            ";
            p.DescriptionData.BulletPoint = new string[] { 
                "Fits: " + GunModel, 
                "Right hand and left hand orientation available",
                "Worn with belt and optional paddle accessory enables wearing without a belt",
                "Safest openly concealed carry holster on the market due to our \"trigger guarded design\"!",
                "The only quick draw holster that enables you to openly conceal your firearm on the market today!"
            };

            p.DescriptionData.MSRP = new Amazon.XML.CurrencyAmount();
            p.DescriptionData.MSRP.currency = Amazon.XML.BaseCurrencyCode.USD;
            p.DescriptionData.MSRP.Value = MSRP;

            p.Condition = new Amazon.XML.ConditionInfo();
            p.Condition.ConditionType = Amazon.XML.ConditionType.New;

            pi.Product = p;

            // --------------------------------------------------------------------------------

            for(int x = 0; x< pics.Length; x++)
            {
                Amazon.XML.ProductImage pic = new Amazon.XML.ProductImage();
                pic.SKU = SKU;

                if (x == 0)
                    pic.ImageType = Amazon.XML.ProductImageImageType.Main;
                else
                    pic.ImageType = Startbutton.Library.StringToEnum<Amazon.XML.ProductImageImageType>("PT" + x.ToString());

                pic.ImageLocation = pics[x];
                pi.Images.Add(pic);
            }

            // --------------------------------------------------------------------------------

            if (ParentSKU != null)
            {            
                Amazon.XML.Inventory inv = new Amazon.XML.Inventory();
                inv.SKU = SKU;
                inv.Item = "9999";

                pi.Inventory = inv;

                // --------------------------------------------------------------------------------

                pi.Price = new Amazon.XML.Price(SKU, Price);


            }

            pi.ParentSKU = ParentSKU;

            return pi;
            
        }

        static void GetInboundShipmentsSince20140310(Entities db)
        {
            Program p = new Program();

            Library.Throttler GetInboundShipmentThrottler = new Library.Throttler(2000);

            foreach (var account in db.AmazonAccounts.OrderBy(aa => aa.DisplaySeq).ToList())
            {

                DateTime LastFBAInboundShipmentSync = DateTime.Parse("3/10/2014");

                p.Log(true, "Getting Inbound FBA Shipments changed since " + LastFBAInboundShipmentSync.ToString("M/d/yy hh:mm:ss tt") + " for " + account.Name);

                var Shipments = Library.GetInboundShipments(new List<Library.Throttler> { GetInboundShipmentThrottler }, account, LastFBAInboundShipmentSync, DateTime.Now, null, p.Log);

                foreach (InboundShipmentInfo shp in Shipments)
                {
                    string txt = "\"" + account.Name + "\",\"" + shp.ShipmentId + "\"\n";
                    
                    p.Log(true, txt);
                    
                    Startbutton.Library.AppendToTextFile("c:\\backup\\Workfiles\\FBAShipments.csv", txt);
                }
            }
        }

        static void DoSyncInboundFBAShipment(Entities db)
        {
            Program p = new Program();

            AmazonAccount Account = GetAccount(db);

            Library.Throttler GetInboundShipmentThrottler = new Library.Throttler(2000);

            Console.Write("ShipmentID: ");

            string sid = Console.ReadLine();

            List<string> shplst;

            if (sid == "")
            {
                shplst = db.Database.SqlQuery<string>("select distinct ShipmentID from InboundFBAProductInventoryAdjustmentAudit where AmazonAccountID='" + Account.ID.ToString() + "' and MissingProductAssociations=0 and ShouldBe != ActuallyIs").ToList();
            }
            else
                shplst = new string[] { sid }.ToList();

            if (shplst.Count > 0)
            {

                var Shipments = Library.GetInboundShipments(new List<Library.Throttler> { GetInboundShipmentThrottler }, Account, null, null, shplst, p.Log);

                Library.DoSyncInboundFBAShipments(db, Account, GetInboundShipmentThrottler, Shipments, true,p.Log);
            }

            Console.Write("\n");
        }

        static void MoveInventoryFromInboundFBAShipment(Entities db)
        {
            Program p = new Program();

            Library.MoveInventoryFromInboundFBAShipment(db, null, null, p.Log);

            p.Log(false, "Saving changes... ");
            db.SaveChanges();
            p.Log(true, "Saved!");

            Console.Write("\n");
        }

        static void GetSettlementData(Entities db)
        {
            Program p = new Program();

            Library.GetSettlementData(db, p.Log);

            Console.Write("\n");
        }

        static void DetermineForcedFBAShipments(Entities db)
        {
            Program p = new Program();

            Library.Throttler GetInboundShipmentThrottler = new Library.Throttler(2000);

            StringBuilder sql = new StringBuilder();

            foreach (var Account in db.AmazonAccounts.ToList())
            {
                var Shipments = Library.GetInboundShipments(new List<Library.Throttler> { GetInboundShipmentThrottler }, Account, DateTime.Parse("3/10/2014"), DateTime.Now, null, p.Log);

                foreach (var s in Shipments)
                {

                    DateTime CreatedOn = DateTime.MinValue;
                    bool GotCreatedOn = false;

                    string[] a = s.ShipmentName.Split('(');

                    if (a.Length > 1)
                    {
                        string[] b = a[1].Split(')');

                        if (b.Length > 1)
                        {
                            if (Account.CountryID == 840)
                                GotCreatedOn = DateTime.TryParse(b[0], out CreatedOn);
                            else
                            {
                                var formats = new[] { "d/M/yyyy HH:mm" };
                                GotCreatedOn = DateTime.TryParseExact(b[0], formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out CreatedOn);
                            }
                        }
                    }

                    p.Log(false, Account.Name + " - " + s.ShipmentName + ": ");

                    if (GotCreatedOn)
                    {
                        if (CreatedOn > DateTime.Parse("3/10/2014"))
                        {
                            sql.Append(@"insert into InboundFBAShipmentsCreatedAfter20140310
                                            select '" + Account.Name + "', '" + s.ShipmentId + "', getdate() where '" + s.ShipmentId + "' not in (select ShipmentID from InboundFBAShipmentsCreatedAfter20140310)\n");
                            p.Log(true, "Trying to created record.");
                        }
                        else
                            p.Log(true, "Nope!");
                    }
                    else
                        p.Log(true, "ERROR parsing!");
                }
            }

            db.Database.ExecuteSqlCommand(sql.ToString());

            db.SaveChanges();

        }
    }
}