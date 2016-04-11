using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarketplaceWebServiceOrders;
using MarketplaceWebServiceOrders.Model;
using OmnimarkAmazon.Models;
using System.Threading;
using AmazonProductAdvtApi;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Net;
using System.IO;
using Amazon.AWS;
using MarketplaceWebServiceProducts;
using MarketplaceWebServiceProducts.Model;
using MarketplaceWebService;
using MarketplaceWebService.Model;
using FBAInventoryServiceMWS;
using FBAInventoryServiceMWS.Model;
using FBAInboundServiceMWS;
using System.Reflection;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using eBay.Service.Call;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using eBay.Service.Util;
using System.Configuration;
using FBAInboundServiceMWS.Model;
using Startbutton.ExtensionMethods;
using System.Data.Linq;
using System.Globalization;
using System.Data.Entity.Infrastructure;

namespace OmnimarkAmazon
{

    public static partial class Library
    {
        public static readonly Guid AmazonMerchantOrderShippedFromWarehouseReasonID = Guid.Parse("271d7811-1734-4cb9-9ff7-058c7ea1a236");
        public static readonly Guid ItemReceivedReasonID = Guid.Parse("5d4f09b9-e63b-4c86-a028-c6a1aa3c4609");
        public static readonly Guid ItemReceiptDeletedReasonID = Guid.Parse("dd7a7db7-bc36-43e9-abf0-2e42baa4951c");
        public static readonly Guid OrlandoLocationID = Guid.Parse("1bb9a0ba-2159-422f-a7c2-ce45874845a2");
        public static readonly Guid OrlandoOutboundFBALocationID = Guid.Parse("3be3a3b7-f94d-4420-b72a-d5c210a8118f");
        public static readonly Guid AmazonFBAOutboundReasonID = Guid.Parse("98fdf146-b7b9-436f-a677-f4a2d2e93e25");
        public static readonly Guid eBayOrderShippedFromWarehouseReasonID = Guid.Parse("91109642-38ce-4379-98a3-63c46d7aef7f");

        public static class SalesVenueIDs
        {
            public static Guid Buy_Dot_Com = Guid.Parse("5c6099a5-bc96-4e7d-afa7-2b8297fd9db1");
        }

        public class Throttler
        {
            public int MillisecondsBetweenRequests = 2000;
            public DateTime LastRequest;

            public Throttler()
            {
            }

            public Throttler(int MillisecondsBetweenRequests)
            {
                this.MillisecondsBetweenRequests = MillisecondsBetweenRequests;
            }

            public void DoWait()
            {
                DoWait(null);
            }

            public void DoWait(Action<bool, string> Log)
            {
                if (LastRequest != null)
                {
                    while (LastRequest.AddMilliseconds(MillisecondsBetweenRequests) > DateTime.Now)
                    {
                        if (Log != null)
                            Log(false, ".");

                        Thread.Sleep(250);
                    }
                }

                LastRequest = DateTime.Now;
            }
        }

        //const String marketplaceId = "ATVPDKIKX0DER";

        // Service Intialization
        const string applicationName = "OmnimarkAmazonLibrary by Startbutton.com";
        const string applicationVersion = "0.1.0";
        //const string AWSAccessKeyID = "     ";
        //const string AWSSecretAccessKey = "QtM4nzrymd88SlzjxYk/2RCeb5Z039XAdoBysV+H";

        // Service URLs
        //const string ServiceURL = "https://mws.amazonservices.com";
        //const string OrderServiceURL = "https://mws.amazonservices.com/Orders/2011-01-01";
        //const string ProductsServiceURL = "https://mws.amazonservices.com/Products/2011-10-01";
        //const string InventoryServiceURL = "https://mws.amazonservices.com/FulfillmentInventory/2010-10-01/";
        //const string AWSServiceURL = "ecs.amazonaws.com";
        //const string AWSXMLNamespace = "http://webservices.amazon.com/AWSECommerceService/2011-08-01";

        // Returns an Amazon Service object for the ServiceType specified
        static ServiceType GetAmazonService<ServiceType>(AmazonAccount AmazonAccount)
        {
            string ThisServiceURL = null;
            Type ConfigType = null;

            if (typeof(ServiceType) == typeof(FBAInventoryServiceMWSClient))
            {
                ConfigType = typeof(FBAInventoryServiceMWSConfig);
                ThisServiceURL = AmazonAccount.Country.InventoryServiceURL;
            }

            if (typeof(ServiceType) == typeof(MarketplaceWebServiceClient))
            {
                ConfigType = typeof(MarketplaceWebServiceConfig);
                ThisServiceURL = AmazonAccount.Country.ServiceURL;
            }

            if (typeof(ServiceType) == typeof(MarketplaceWebServiceOrdersClient))
            {
                ConfigType = typeof(MarketplaceWebServiceOrdersConfig);
                ThisServiceURL = AmazonAccount.Country.OrderServiceURL;
            }

            if (typeof(ServiceType) == typeof(FBAInboundServiceMWSClient))
            {
                ConfigType = typeof(FBAInboundServiceMWSConfig);
                ThisServiceURL = AmazonAccount.Country.ServiceURL + "/FulfillmentInboundShipment/2010-10-01";
            }

            if (ConfigType == null)
                throw (new Exception("Unknown Service Type!"));

            object config = Activator.CreateInstance(ConfigType);

            PropertyInfo ServiceURLProperty = ConfigType.GetProperty("ServiceURL", BindingFlags.Public | BindingFlags.Instance);
            ServiceURLProperty.SetValue(config, ThisServiceURL, null);

            ServiceType service = (ServiceType)Activator.CreateInstance(typeof(ServiceType), new object[] { applicationName, applicationVersion, AmazonAccount.AccessKeyID, AmazonAccount.SecretAccessKey, config });

            return service;
        }

        // Handle logging- check for null Log
        static void DoLog(Action<bool, string> Log, bool Linebreak, string Message)
        {
            if (Log != null)
                Log(Linebreak, Message);
        }

        public static string GetGetASINsWithAttributesSQL(Entities db, IEnumerable<string> Fields, string SelectClauseExtension, string EndClauses, Nullable<int> NumberOfRecords)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append("select ");

            if (NumberOfRecords != null)
            {
                sql.Append("top ");
                sql.Append((int)NumberOfRecords);
                sql.Append(' ');
            }

            sql.Append('*');

            if (!string.IsNullOrEmpty(SelectClauseExtension))
            {
                sql.Append(", ");
                sql.Append(SelectClauseExtension);
            }

            sql.Append(" from (select ka.*");

            if (Fields == null)
                Fields = db.KnownASINAttributes.Select(kaa => kaa.Name).Distinct();

            foreach (string f in Fields)
            {
                sql.Append(", [");
                sql.Append(f);
                sql.Append("].Value as [");
                sql.Append(f);
                sql.Append("]\n");
            }

            sql.Append(" from KnownASINsForExport ka\n");

            foreach (string f in Fields)
            {
                sql.Append("left join KnownASINAttributes [");
                sql.Append(f);
                sql.Append("] on ka.ASIN = [");
                sql.Append(f);
                sql.Append("].ASIN and [");
                sql.Append(f);
                sql.Append("].Name = '");
                sql.Append(f);
                sql.Append("'\n");
            }

            sql.Append(") a\n");

            if (!string.IsNullOrEmpty(EndClauses))
                sql.Append(EndClauses);

            return sql.ToString();
        }

        public static string GetGetASINsWithAttributesSQLFromExportSpec(Entities db, Guid id, ref ExportSpec es, ref Dictionary<string, string> ColumnNames)
        {
            string ExtraSQL = null;

            List<string> s = new List<string>();

            PrepareGetASINsWithAttributes(db, id, ref es, ref s, ref ColumnNames, ref ExtraSQL);

            return GetGetASINsWithAttributesSQL(db, null, es.SelectClauseExtension, ExtraSQL, es.RecordCount);
        }

        public static List<Dictionary<string, object>> GetASINsWithAttributesFromExportSpec(Entities db, Guid id, ref ExportSpec es, ref List<string> FieldList, ref Dictionary<string, string> ColumnNames)
        {
            string SQL = null;
            return GetASINsWithAttributesFromExportSpec(db, id, ref es, ref FieldList, ref ColumnNames, ref SQL);
        }

        public static List<Dictionary<string, object>> GetASINsWithAttributesFromExportSpec(Entities db, Guid id, ref ExportSpec es, ref List<string> FieldList, ref Dictionary<string, string> ColumnNames, ref string SQL)
        {
            string ExtraSQL = null;
            PrepareGetASINsWithAttributes(db, id, ref es, ref FieldList, ref ColumnNames, ref ExtraSQL);
            return OmnimarkAmazon.Library.GetASINsWithAttributes(db, null, es.SelectClauseExtension, ExtraSQL, es.RecordCount, ref SQL);
        }

        public static void PrepareGetASINsWithAttributes(Entities db, Guid id, ref ExportSpec es, ref List<string> FieldList, ref Dictionary<string, string> ColumnNames, ref string ExtraSQL)
        {
            es = db.ExportSpecs.Single(e => e.ID == id);

            Dictionary<string, string> FieldTranslations = new Dictionary<string, string>();

            FieldList = es.FieldList.Split(',').ToList();

            if (es.FieldNameTranslations != null)
            {
                string[] a = es.FieldNameTranslations.Split(',');

                for (int x = 0; x < a.Length; x++)
                {
                    string[] b = a[x].Split('=');
                    FieldTranslations.Add(b[0], b[1]);
                }
            }

            foreach (string f in FieldList)
                if (FieldTranslations.Keys.Contains(f))
                    ColumnNames.Add(f, FieldTranslations[f]);
                else
                    ColumnNames.Add(f, f);

            ExtraSQL = "";

            if (!string.IsNullOrEmpty(es.WhereClause) || es.ExcludeExportsToSalesVenueID != null)
                ExtraSQL = "where ";

            if (es.ExcludeExportsToSalesVenueID != null)
                ExtraSQL += "ASIN not in (select ASIN from ASINsSuccessfullyExported where SalesVenueID='" + es.ExcludeExportsToSalesVenueID.ToString() + "')";

            if (!string.IsNullOrEmpty(es.WhereClause))
                ExtraSQL += (es.ExcludeExportsToSalesVenueID != null ? " and " : "") + es.WhereClause;

            if (!string.IsNullOrEmpty(es.OrderByClause))
                ExtraSQL += " order by " + es.OrderByClause;

        }

        public static List<Dictionary<string, object>> GetASINsWithAttributes(Entities db, IEnumerable<string> Fields, string SelectClauseExtension, string EndClauses, Nullable<int> NumberOfRecords)
        {
            string SQL = null;
            return GetASINsWithAttributes(db, Fields, SelectClauseExtension, EndClauses, NumberOfRecords, ref SQL);
        }

        public static List<Dictionary<string, object>> GetASINsWithAttributes(Entities db, IEnumerable<string> Fields, string SelectClauseExtension, string EndClauses, Nullable<int> NumberOfRecords, ref string SQL)
        {

            SQL = OmnimarkAmazon.Library.GetGetASINsWithAttributesSQL(db, Fields, SelectClauseExtension, EndClauses, NumberOfRecords);

            List<Dictionary<string, object>> rtn = new List<Dictionary<string, object>>();

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = Startbutton.Library.GetConnectionString("Main");
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = SQL;
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {

                        while (rdr.Read())
                        {
                            Dictionary<string, object> rtnrec = new Dictionary<string, object>();

                            for (int x = 0; x < rdr.FieldCount; x++)
                                rtnrec.Add(rdr.GetName(x), rdr[x]);

                            rtn.Add(rtnrec);
                        }
                    }
                }

                conn.Close();
            }

            return rtn;
        }


        public static void RecreateKnownASINsForExportView(Entities db)
        {
            db.Database.ExecuteSqlCommand(GetCreateASINProductTagsViewSQL(db));
            db.RecreateView("KnownUniqueProductComboASINs");
            db.RecreateView("KnownASINsForExport");
        }

        public static string GetCreateASINProductTagsViewSQL(Entities db)
        {

            ProductTag[] tags = db.ProductTags.ToArray();

            string sql = @"
                        alter view ASINProductTags as
                        select ka.ASIN as _ignore_ASIN /* use the ignore prefix do field doesn't appear in the exportable fields list */
                    ";

            for (int x = 0; x < tags.Length; x++)
                sql += ",MAX(case when ppt" + x.ToString() + ".ProductID is null then 0 else 1 end) as [ProductTag_" + tags[x].Name + "]\n";

            sql += @"
                        from KnownASINs ka
                        left join AmazonInventoryProducts aip on ka.ASIN = aip.ASIN
                        left join Products p on aip.ProductID = p.ID
                    ";

            for (int x = 0; x < tags.Length; x++)
                sql += "left join ProductProductTags ppt" + x.ToString() + " on p.ID = ppt" + x.ToString() + ".ProductID and ppt" + x.ToString() + ".ProductTagID='" + tags[x].ID.ToString() + "'\n";

            sql += @"
                        group by ka.ASIN";

            return sql;
        }

        class ImageToDownload
        {
            public string ASIN { get; set; }
            public string ImageURL { get; set; }
        }

        public static void DownloadImage(Entities db, string Path, Action<bool, string> Log, string ASIN = null, Nullable<int> Count = null)
        {
            if (!Path.EndsWith("\\"))
                Path += "\\";

            string sql;

            if (ASIN != null)
                sql = @"
                    select kaa.ASIN, Value as ImageURL from KnownASINAttributes kaa 
                    join KnownASINs ka on kaa.ASIN = ka.ASIN
                    where kaa.Name='ImageURL'
                    and ka.GoneFromAmazon=0
                    and ka.ASIN='" + ASIN.Replace("'", "''") + "'";
            else if (Count == null)
                sql = @"
                    select kaa.ASIN, Value as ImageURL from KnownASINAttributes kaa 
                    join KnownASINs ka on kaa.ASIN = ka.ASIN
                    where kaa.Name='ImageURL'
                    and ka.GoneFromAmazon=0
                    and ka.ImageName is null";
            else
                sql = @"
                    select top " + ((int)Count).ToString() + @" kaa.ASIN, Value as ImageURL from KnownASINAttributes kaa 
                    join KnownASINs ka on kaa.ASIN = ka.ASIN
                    where kaa.Name='ImageURL'
                    and ka.GoneFromAmazon=0
                    and ka.ImageName is null";

            var qry = db.Database.SqlQuery<ImageToDownload>(sql).ToList();

            foreach (var row in qry)
            {
                Log(false, "Retreiving " + row.ImageURL + " for ASIN: " + row.ASIN + "... ");

                try
                {
                    Stream img = Startbutton.Library.HTTPGetStream(row.ImageURL, 30000);

                    string[] a = row.ImageURL.Split('.');
                    string Filename = row.ASIN + "." + a[a.Length - 1];
                    string Filepath = Path + Filename;

                    KnownASIN ka = db.KnownASINs.Single(kax => kax.ASIN == row.ASIN);

                    if (ka.ImageName != null && ka.ImageName != Filename && File.Exists(Path + ka.ImageName))
                        File.Delete(Path + ka.ImageName);

                    ka.ImageName = Filename;

                    Log(true, "Saving " + Filepath);

                    if (File.Exists(Filepath))
                        File.Delete(Filepath);

                    FileStream fs = new FileStream(Filepath, FileMode.CreateNew);
                    img.CopyTo(fs);

                    db.SaveChanges();

                    fs.Dispose();
                    img.Dispose();
                }
                catch (Exception Ex)
                {
                    Log(true, "ERROR: " + Ex.Message + "\n\n" + Ex.StackTrace);
                }
            }

        }

        public static ApiContext eBayApiContext
        {
            get
            {

                ApiCredential apiCredential = new ApiCredential();
                apiCredential.eBayToken = ConfigurationManager.AppSettings["UserAccount.ApiToken"];

                ApiContext apiContext = new ApiContext();
                apiContext.SoapApiServerUrl = ConfigurationManager.AppSettings["Environment.ApiServerUrl"];
                apiContext.ApiCredential = apiCredential;
                apiContext.Site = SiteCodeType.US;

                return apiContext;
            }
        }

        public static void SynceBayOrders(Entities db, Action<bool, string> Log)
        {

            SystemStatu ss = db.SystemStatus.First();

            DateTime ModTimeFrom = ss.LasteBayOrderDownloadQueryDate == null ? DateTime.Today.AddDays(-10) : (DateTime)ss.LasteBayOrderDownloadQueryDate;
            ss.LasteBayOrderDownloadQueryDate = DateTime.Now;

            for (DateTime d = ModTimeFrom; d < ss.LasteBayOrderDownloadQueryDate; d = d.AddDays(30))
                DoSynceBayOrders(db, d, null, Log);

            db.SaveChanges();

        }

        static void DoSynceBayOrders(Entities db, Nullable<DateTime> CreateStartDate, Nullable<DateTime> ModifyStartDate, Action<bool, string> Log)
        {
            GetOrdersCall call = null;

            if (CreateStartDate != null)
            {
                Log(true, "Syncing orders CREATED from " + ((DateTime)CreateStartDate).ToShortDateString() + " to " + ((DateTime)CreateStartDate).AddDays(30).ToShortDateString() + "...");

                call = new GetOrdersCall(Library.eBayApiContext)
                {
                    CreateTimeFrom = (DateTime)CreateStartDate,
                    CreateTimeTo = ((DateTime)CreateStartDate).AddDays(30),
                    DetailLevelList = new DetailLevelCodeTypeCollection(new DetailLevelCodeType[] { DetailLevelCodeType.ReturnAll })
                };
            }

            if (ModifyStartDate != null)
            {
                Log(true, "Syncing orders MODIFIED from " + ((DateTime)ModifyStartDate).ToShortDateString() + " to " + ((DateTime)ModifyStartDate).AddDays(30).ToShortDateString() + "...");

                call = new GetOrdersCall(Library.eBayApiContext)
                {
                    ModTimeFrom = (DateTime)ModifyStartDate,
                    ModTimeTo = ((DateTime)ModifyStartDate).AddDays(30),
                    DetailLevelList = new DetailLevelCodeTypeCollection(new DetailLevelCodeType[] { DetailLevelCodeType.ReturnAll })
                };
            }

            call.Pagination = new PaginationType()
            {
                EntriesPerPage = 25
            };

            Log(false, "Getting Orders (p1): ");

            call.Execute();

            Log(true, "Got " + call.ApiResponse.OrderArray.Count.ToString());

            OrderTypeCollection Orders = new OrderTypeCollection();

            Orders.AddRange(call.ApiResponse.OrderArray);

            int page = 1;

            while (call.ApiResponse.HasMoreOrders)
            {
                call.Pagination = new PaginationType()
                {
                    EntriesPerPage = 25,
                    PageNumber = ++page
                };

                Log(false, "Getting More Orders (p " + page.ToString() + "): ");

                call.Execute();

                Orders.AddRange(call.ApiResponse.OrderArray);

                Log(true, "Got " + call.ApiResponse.OrderArray.Count.ToString() + ".  Total: " + Orders.Count.ToString());

            }

            Log(true, call.ApiResponse.Ack + ".  Got " + Orders.Count.ToString() + " records.");

            foreach (OrderType o in Orders)
            {
                #region Handle Order

                eBayOrder eo = db.eBayOrders.Where(eox => eox.ID == o.OrderID).FirstOrDefault();

                Log(false, "Order " + o.OrderID + ": ");

                if (eo == null)
                {
                    eo = new eBayOrder();
                    eo.ID = o.OrderID;
                    eo.TimeStamp = DateTime.Now;
                    eo.Created = o.CreatedTime;

                    db.eBayOrders.Add(eo);

                    Log(false, "Added. ");
                }
                else
                    eo.UpdateTimeStamp = DateTime.Now;

                if (o.OrderStatus == OrderStatusCodeType.Completed && eo.Status != (int)OrderStatusCodeType.Completed)
                {
                    eo.ShippedNoticedTimeStamp = DateTime.Now;
                    Log(false, "Marked Shipped. ");
                }

                if (eo.Status != (int)o.OrderStatus)
                {
                    eo.Status = (int)o.OrderStatus;
                    Log(false, "Updated Status. ");
                    eo.UpdateTimeStamp = DateTime.Now;
                }

                string RawData = Startbutton.Library.XmlSerialize<OrderType>(o);

                if (eo.RawData != RawData)
                {
                    eo.RawData = RawData;
                    Log(false, "Updated RawData. ");
                    eo.UpdateTimeStamp = DateTime.Now;
                }

                if (eo.ShippedTime != o.ShippedTime && o.ShippedTime != DateTime.MinValue)
                {
                    eo.ShippedTime = o.ShippedTime;
                    Log(false, "Updated ShippedTime to " + o.ShippedTime.ToString() + ". ");
                    eo.UpdateTimeStamp = DateTime.Now;
                }

                if (o.TransactionArray[0] != null)
                    if (o.TransactionArray[0].Item != null)
                        if (eo.Site != o.TransactionArray[0].Item.Site.ToString())
                        {
                            eo.Site = o.TransactionArray[0].Item.Site.ToString();
                            Log(false, "Updated Site. ");
                            eo.UpdateTimeStamp = DateTime.Now;
                        }

                for (int x = 0; x < o.TransactionArray.Count; x++)
                {
                    var t = o.TransactionArray[x];

                    eBayOrderLine eol = db.eBayOrderLines.Where(eolx => eolx.ID == t.OrderLineItemID).FirstOrDefault();

                    if (eol == null)
                    {
                        eol = new eBayOrderLine();
                        eol.OrderID = o.OrderID;
                        eol.ID = t.OrderLineItemID;
                        eol.TimeStamp = DateTime.Now;
                        eol.Qty = o.TransactionArray[x].QuantityPurchased;
                        eol.ItemID = t.Item.ItemID;
                        eol.SellingManagerSalesRecordNumber = t.ShippingDetails.SellingManagerSalesRecordNumber;
                        eol.Price = (decimal)t.TransactionPrice.Value;

                        db.eBayOrderLines.Add(eol);

                        Log(false, "Line " + (x + 1).ToString() + " Added. ");

                    }

                    if (eol.Qty != o.TransactionArray[x].QuantityPurchased)
                    {
                        eol.Qty = o.TransactionArray[x].QuantityPurchased;
                        eol.UpdateTimeStamp = DateTime.Now;
                        Log(false, "Line " + (x + 1).ToString() + " Updated Qty. ");

                    }

                    if (eol.ItemID != t.Item.ItemID)
                    {
                        eol.ItemID = t.Item.ItemID;
                        eol.UpdateTimeStamp = DateTime.Now;
                        Log(false, "Line " + (x + 1).ToString() + " Updated ItemID. ");
                    }

                    if (eol.SellingManagerSalesRecordNumber != t.ShippingDetails.SellingManagerSalesRecordNumber)
                    {
                        eol.SellingManagerSalesRecordNumber = t.ShippingDetails.SellingManagerSalesRecordNumber;
                        eol.UpdateTimeStamp = DateTime.Now;
                        Log(false, "Line " + (x + 1).ToString() + " Updated SellingManagerSalesRecordNumber. ");
                    }

                    if (eol.Price != (decimal)t.TransactionPrice.Value)
                    {
                        eol.Price = (decimal)t.TransactionPrice.Value;
                        eol.UpdateTimeStamp = DateTime.Now;
                        Log(false, "Line " + (x + 1).ToString() + " Updated Price. ");
                    }

                }

                #endregion

                Log(true, "");
            }


        }

        public static void SynceBayItems(Entities db, Action<bool, string> Log)
        {
            //eBayGetSellerList(db, false, Log);
            eBayGetSellerList(db, true, Log);
        }

        static void eBayGetSellerList(Entities db, bool Forward, Action<bool, string> Log)
        {
            int EmptyCount = 0;

            GetSellerListCall call = new GetSellerListCall(Library.eBayApiContext);

            SystemStatu ss = db.SystemStatus.First();

            DateTime StartDateTime;

            if (Forward)
                StartDateTime = ss.LatesteBayRawItemQueryDate == null ? DateTime.Today.AddMonths(-1) : (DateTime)ss.LatesteBayRawItemQueryDate;
            else
                StartDateTime = ss.OldesteBayRawItemQueryDate == null ? DateTime.Today : (DateTime)ss.OldesteBayRawItemQueryDate;

            int DayChunkSize = 120;

            while (true)
            {
                int page = 0;

                while (true)
                {
                    page++;

                    if (Forward)
                    {

                        call.StartTimeTo = StartDateTime.AddDays(DayChunkSize);
                        call.StartTimeFrom = StartDateTime;
                    }
                    else
                    {
                        call.StartTimeTo = StartDateTime;
                        call.StartTimeFrom = StartDateTime.AddDays(-DayChunkSize);
                    }

                    call.DetailLevelList = new DetailLevelCodeTypeCollection(new DetailLevelCodeType[] { DetailLevelCodeType.ReturnAll });
                    call.Pagination = new PaginationType()
                    {
                        EntriesPerPage = 200,
                        PageNumber = page
                    };

                    Log(false, call.StartTimeFrom.ToShortDateString() + " through " + call.StartTimeTo.ToShortDateString() + ", Page " + page.ToString() + ": ");

                    call.Execute();

                    Log(true, call.ApiResponse.Ack.ToString() + ". Got " + call.ApiResponse.ItemArray.Count.ToString() + " records.");

                    var ItemsGot = call.ApiResponse.ItemArray.ToArray().Select(ig => ig.ItemID).ToList();

                    var ItemsToModify = db.eBayRawItemDatas.Where(erid => ItemsGot.Contains(erid.ItemID));

                    foreach (ItemType item in call.ApiResponse.ItemArray)
                    {
                        eBayRawItemData rid;

                        rid = ItemsToModify.Where(ridx => ridx.ItemID == item.ItemID).FirstOrDefault();

                        if (rid == null)
                        {
                            rid = new Models.eBayRawItemData();
                            rid.TimeStamp = DateTime.Now;
                            db.eBayRawItemDatas.Add(rid);
                        }

                        string RawData = Startbutton.Library.XmlSerialize<ItemType>(item);

                        if (rid.ItemID != item.ItemID)
                        {
                            rid.ItemID = item.ItemID;
                            rid.UpdateTimeStamp = DateTime.Now;
                        }

                        if (rid.RawData != RawData)
                        {
                            rid.RawData = RawData;
                            rid.UpdateTimeStamp = DateTime.Now;
                        }

                    }

                    Console.WriteLine("");

                    if (!call.ApiResponse.HasMoreItems)
                        break;
                }

                if (call.ApiResponse.ItemArray.Count == 0)
                    EmptyCount++;

                if (EmptyCount == 3)
                    break;

                if (Forward)
                {
                    ss.LatesteBayRawItemQueryDate = DateTime.Today;
                    StartDateTime = StartDateTime.AddDays(DayChunkSize);
                    break;
                }
                else
                {
                    StartDateTime = StartDateTime.AddDays(-DayChunkSize);
                    ss.OldesteBayRawItemQueryDate = StartDateTime;
                }

            }

            db.SaveChanges();

        }

        public static int MoveInventoryFromCompletedeBayOrders(Entities db, Action<bool, string> Log)
        {

            Log(true, "Reducing inventory for completed eBay orders.");

            int cnt = 0;

            var recs = db.Database.SqlQuery<eBayCompleteOrderLines>(@"
                select eo.ID as OrderID, eol.ItemID, ProductID, eol.Qty * eip.Qty as Qty, SellingManagerSalesRecordNumber
                from eBayOrders eo
                join eBayOrderLines eol on eo.ID = eol.OrderID
                left join eBayItemsProducts eip on eol.ItemID = eip.ItemID
                where ShippedTime > '3/10/2014' and ProductInventoryMoveID is null and Site='US'
            ").ToList();

            var oidlist = recs.Select(r => r.OrderID).ToList();

            var orders = db.eBayOrders.Where(eo => oidlist.Contains(eo.ID)).ToList();

            foreach (string oid in recs.Select(r => r.OrderID).Distinct())
            {

                DateTime Now = DateTime.Now;

                Log(false, "Order: " + oid + ": ");

                var Products = recs.Where(r => r.OrderID == oid);
                var order = orders.Single(o => o.ID == oid);

                if (Products.Count(p => p.ProductID == null) > 0)
                    Log(true, "Missing product associations for: " + string.Join<string>(",", Products.Where(p => p.ProductID == null).Select(p => p.ItemID)));
                else
                {

                    Log(true, "Got " + Products.Count().ToString() + " unique products with total quantity of " + Products.Sum(p => p.Qty).ToString());

                    Guid aid = Guid.Empty;

                    order.ProductInventoryMoveID = CreateProductInventoryAdjustmentBatch(db, "REDUCE-EBAY").ID;

                    foreach (var p in Products)
                        aid = AdjustInventory(db, (Guid)p.ProductID, OrlandoLocationID, -(decimal)p.Qty, eBayOrderShippedFromWarehouseReasonID, order.ID, p.SellingManagerSalesRecordNumber.ToString(), null, order.ProductInventoryMoveID);

                    db.SaveChanges();

                    cnt++;
                }

            }

            return cnt;
        }

        public static ProductInventoryAdjustmentBatch CreateProductInventoryAdjustmentBatch(Entities db, string Type)
        {
            ProductInventoryAdjustmentBatch piab = new ProductInventoryAdjustmentBatch();
            piab.ID = Guid.NewGuid();
            piab.Type = Type;
            piab.TimeStamp = DateTime.Now;

            db.ProductInventoryAdjustmentBatches.Add(piab);

            return piab;
        }

        public static int ReconcileInventoryForAmazonOrdersShippedFromOrlando(Entities db, Action<bool, string> Log)
        {

            Log(true, "Reconciling Inventory for Amazon Orders Shipped From Orlando.");
            try
            {
                int cnt = 0;
                ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 1800;
                var recs = db.Database.SqlQuery<UnreconciledAmazonOrdersShippedFromOrlando>("GetUnreconciledAmazonOrdersShippedFromOrlando").ToList();
                var oidlist = recs.Select(r => r.AmazonOrderID).ToList();

                var orders = db.AmazonOrders.Where(ao => oidlist.Contains(ao.AmazonOrderID)).ToList();

                foreach (string oid in recs.Select(r => r.AmazonOrderID).Distinct())
                {

                    DateTime Now = DateTime.Now;

                    Log(false, "Order: " + oid + ": ");

                    var Products = recs.Where(r => r.AmazonOrderID == oid);
                    var order = orders.Single(o => o.AmazonOrderID == oid);

                    Log(true, "Got " + Products.Count().ToString() + " unique products with total quantity of " + Products.Sum(p => p.AdjustmentQty).ToString());

                    Guid aid = Guid.Empty;

                    order.ProductInventoryAdjustmentBatchID = CreateProductInventoryAdjustmentBatch(db, "REDUCE-AMZ-MRCH").ID;

                    foreach (var p in Products)
                        aid = AdjustInventory(db, p.ProductID, OrlandoLocationID, -p.AdjustmentQty, AmazonMerchantOrderShippedFromWarehouseReasonID, order.AmazonOrderID, null, null, order.ProductInventoryAdjustmentBatchID);

                    db.SaveChanges();

                    cnt++;

                }

                return cnt;
            }
            catch (Exception e)
            {
                Log(true, "Exceptions!" + e);
                throw;
            }
        }

        /// <summary>
        /// Get all FBA Shipments which Amazon claims have been updated between 1/1/2014 and Now
        /// Create record in local table but do not get line items or reduce inventory
        /// set ScheduledForProcessing automatically based on CreatedOn date
        /// Check and record status changes
        /// Parse CreatedOn from "ShipmentName" since Amazon does not explicitly provide a created on date
        /// </summary>
        /// <param name="db"></param>
        /// <param name="Log"></param>
        public static void GetAllFBAShipments(Entities db, Action<bool, string> Log)
        {
            Library.Throttler GetInboundShipmentThrottler = new Library.Throttler(2000);
            bool isError = false;
            foreach (var account in db.AmazonAccounts.Where(aa => aa.Enabled || aa.AccessKeyID == "AKIAJJE7HWBEDEJ7FXLQ").OrderBy(aa => aa.DisplaySeq).ToList())
            {
                try
                {


                    Log(true, "Getting all Inbound FBA Shipments scheduled for processing from " + account.Name);
                    var Shipments = GetInboundShipments(new List<Library.Throttler> { GetInboundShipmentThrottler }, account, DateTime.Now.AddMonths(-6), DateTime.Now, null, Log);

                    DoSyncInboundFBAShipments(db, account, GetInboundShipmentThrottler, Shipments, false, Log);

                    Log(true, "DONE!");

                    account.LastFBAInboundShipmentSync = DateTime.Now;

                    db.SaveChanges();

                    Log(true, "");
                }
                catch
                {


                    isError = true;
                }
                if (isError) continue;
            }

        }

        /// <summary>
        /// Get all existing (in local table) FBA Shipments from Amazon for all stores, which are scheduled for processing and not removed from processing
        /// Get all line items, and adjust inventory accordingly
        /// set ScheduledForProcessing automatically based on CreatedOn date
        /// Check and record status changes
        /// Parse CreatedOn from "ShipmentName" since Amazon does not explicitly provide a created on date
        /// </summary>
        /// <param name="db"></param>
        /// <param name="Log"></param>
        public static void SyncInboundFBAShipments(Entities db, Action<bool, string> Log)
        {
            Library.Throttler GetInboundShipmentThrottler = new Library.Throttler(2000);
            bool isError = false;
            foreach (var account in db.AmazonAccounts.Where(aa => aa.Enabled || aa.AccessKeyID == "AKIAJJE7HWBEDEJ7FXLQ").OrderBy(aa => aa.DisplaySeq).ToList())
            {
                try
                {


                    List<string> valid_shipments = db.InboundFBAShipments.Where(ifs => ifs.AmazonAccountID == account.ID && ifs.ScheduledForInventoryAdjustmentProcessing != null && ifs.RemovedFromInventoryAdjustmentProcessing == null).Select(ifs => ifs.ID).ToList();

                    Log(true, "Getting open Inbound FBA Shipments scheduled for processing from " + account.Name);
                    var Shipments = GetInboundShipments(new List<Library.Throttler> { GetInboundShipmentThrottler }, account, DateTime.Now.AddMonths(-6), DateTime.Now, valid_shipments, Log);

                    DoSyncInboundFBAShipments(db, account, GetInboundShipmentThrottler, Shipments, true, Log);

                    Log(true, "DONE!");

                    account.LastFBAInboundShipmentSync = DateTime.Now;

                    db.SaveChanges();

                    Log(true, "");
                }
                catch
                {


                    isError = true;
                }
                if (isError) continue;
            }


        }

        /// <summary>
        /// Handle FBA Shipments for the list provided, optionally get line items and reduce inventory
        /// set ScheduledForProcessing automatically based on CreatedOn date
        /// Check and record status changes
        /// Parse CreatedOn from "ShipmentName" since Amazon does not explicitly provide a created on date
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="account"></param>
        /// <param name="GetInboundShipmentThrottler"></param>
        /// <param name="Shipments"></param>
        /// <param name="GetLineItems"></param>
        /// <param name="Log"></param>
        public static void DoSyncInboundFBAShipments(Entities db, AmazonAccount account, Library.Throttler GetInboundShipmentThrottler, List<InboundShipmentInfo> Shipments, bool GetLineItems, Action<bool, string> Log)
        {
            Log(true, "Processing...");
            bool isError = false;
            foreach (InboundShipmentInfo shp in Shipments)
            {
                try
                {


                    Log(false, shp.ShipmentId + ": ");

                    DateTime CreatedOn = DateTime.MinValue;
                    bool GotCreatedOn = false;

                    string[] a = shp.ShipmentName.Split('(');

                    if (a.Length > 1)
                    {
                        string[] b = a[1].Split(')');

                        if (b.Length > 1)
                        {
                            if (account.CountryID == 840)
                                GotCreatedOn = DateTime.TryParse(b[0], out CreatedOn);
                            else
                            {
                                var formats = new[] { "d/M/yyyy HH:mm" };
                                GotCreatedOn = DateTime.TryParseExact(b[0], formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out CreatedOn);
                            }
                        }
                    }


                    var ifs = db.InboundFBAShipments.Where(ifsx => ifsx.ID == shp.ShipmentId).FirstOrDefault();

                    if (ifs == null)
                    {
                        ifs = new InboundFBAShipment();
                        ifs.ID = shp.ShipmentId;
                        ifs.TimeStamp = DateTime.Now;
                        ifs.AmazonAccountID = account.ID;

                        if (GotCreatedOn && CreatedOn < DateTime.Parse("10/1/2014"))
                            ifs.RemovedFromInventoryAdjustmentProcessing = DateTime.Now;

                        db.InboundFBAShipments.Add(ifs);
                    }
                    else if (GotCreatedOn)
                    {
                        if (ifs.RemovedFromInventoryAdjustmentProcessing == null)
                            if (CreatedOn < DateTime.Parse("10/1/2014"))
                                ifs.RemovedFromInventoryAdjustmentProcessing = DateTime.Now;
                    }

                    if (ifs.CreatedOn == null && GotCreatedOn)
                        ifs.CreatedOn = CreatedOn;

                    if (ifs.Name != shp.ShipmentName)
                    {
                        ifs.Name = shp.ShipmentName;
                        ifs.UpdateTimeStamp = DateTime.Now;
                    }

                    FBAInboundShipmentStatus sts = Startbutton.Library.StringToEnum<FBAInboundShipmentStatus>(shp.ShipmentStatus);

                    if (ifs.Status != (int)sts)
                    {

                        Log(true, "Status updated from " + (ifs.Status == null ? "NULL" : ((FBAInboundShipmentStatus)ifs.Status).ToString()) + " to " + shp.ShipmentStatus);

                        InboundFBAShipmentStatusChange ifssc = new InboundFBAShipmentStatusChange();
                        ifssc.ShipmentID = ifs.ID;
                        ifssc.ID = Guid.NewGuid();
                        ifssc.TimeStamp = DateTime.Now;
                        ifssc.OldStatus = ifs.Status;
                        ifssc.NewStatus = (int)sts;

                        db.InboundFBAShipmentStatusChanges.Add(ifssc);

                        ifs.Status = (int)sts;
                        ifs.UpdateTimeStamp = DateTime.Now;
                    }

                    Log(true, "ScheduledForInventoryAdjustmentProcessing: " + ifs.ScheduledForInventoryAdjustmentProcessing.ToString() + " - RemovedFromInventoryAdjustmentProcessing: " + ifs.RemovedFromInventoryAdjustmentProcessing.ToString());

                    if (GetLineItems && ifs.ScheduledForInventoryAdjustmentProcessing != null && ifs.RemovedFromInventoryAdjustmentProcessing == null)
                        GetInboundFBAShipmentItems(db, account, ifs, shp, new List<Library.Throttler> { GetInboundShipmentThrottler }, Log);

                    Log(false, "Saving changes... ");
                    db.SaveChanges();
                    Log(true, "Saved.");
                }
                catch
                {


                    isError = true;
                }
                if (isError) continue;

            }

            if (GetLineItems)
                if (MoveInventoryFromInboundFBAShipment(db, account, Shipments.Select(s => s.ShipmentId), Log).Count > 0)
                {
                    Log(false, "Saving changes... ");
                    db.SaveChanges();
                    Log(true, "Saved.");
                }

        }

        public static Dictionary<string, Guid> MoveInventoryFromInboundFBAShipment(Entities db, AmazonAccount Account, IEnumerable<string> ShipmentIDs, Action<bool, string> Log)
        {
            string ShipmentList = ShipmentIDs == null ? "" : string.Join(",", ShipmentIDs);

            Log(true, "Reducing inventory for " + (ShipmentIDs == null ? "all shipments." : ShipmentList));

            var itms = db.Database.SqlQuery<MoveInventoryFromInboundFBAShipmentRec>(@"
                select AmazonAccountName, ShipmentID, ProductID, ProductName, ShouldBe-ActuallyIs as AdjustmentAmount from InboundFBAProductInventoryAdjustmentAudit
                where " +
                    (Account == null ? "" : "AmazonAccountID='" + Account.ID.ToString() + "' and ") + @"
                    (MissingProductAssociations=0 or MissingProductAssociations is null) and 
                    ShouldBe != ActuallyIs " +
                    (ShipmentIDs == null ? "" : "and ShipmentID in ('" + string.Join("','", ShipmentIDs) + "')") + @"
                order by ShipmentID
            ").ToList();

            return DoMoveInventoryFromInboundFBAShipment(db, itms, OrlandoLocationID, null, Log);

        }

        static Dictionary<string, Guid> DoMoveInventoryFromInboundFBAShipment(Entities db, IEnumerable<MoveInventoryFromInboundFBAShipmentRec> itms, Guid From, Guid? To, Action<bool, string> Log)
        {
            Dictionary<string, Guid> rtn = new Dictionary<string, Guid>();

            string FromName = db.ProductInventoryLocations.Single(pil => pil.ID == From).Name;
            string ToName = To == null ? null : db.ProductInventoryLocations.Single(pil => pil.ID == To).Name;

            foreach (string shpid in itms.Select(i => i.ShipmentID).Distinct())
            {

                ProductInventoryAdjustmentBatch piab = CreateProductInventoryAdjustmentBatch(db, "FBA");

                //foreach (var i in itms.Where(ix => ix.ShipmentID == shpid))
                //{
                //    Log(true, "For " + i.ShipmentID + ": Moving " + i.AdjustmentAmount.ToString("###,###,##0") + " " + i.ProductName + " From " + FromName + (To == null ? "" : " To " + ToName));

                //    AdjustInventory(db, i.ProductID, From, -i.AdjustmentAmount, AmazonFBAOutboundReasonID, i.ShipmentID, i.AmazonAccountName, null, piab.ID);

                //    if (To != null)
                //        AdjustInventory(db, i.ProductID, (Guid)To, i.AdjustmentAmount, AmazonFBAOutboundReasonID, i.ShipmentID, i.AmazonAccountName, null, piab.ID);
                //}

                InboundFBAShipmentProductInventoryAdjustment ifpia = new InboundFBAShipmentProductInventoryAdjustment();
                ifpia.ID = Guid.NewGuid();
                ifpia.InboundFBAShipmentID = shpid;
                ifpia.ProductInventoryAdjustmentBatchID = piab.ID;
                ifpia.TimeStamp = DateTime.Now;

                db.InboundFBAShipmentProductInventoryAdjustments.Add(ifpia);

                rtn.Add(shpid, piab.ID);
            }

            return rtn;
        }

        public static void UndoMoveInventoryFromInboundFBAShipment(Entities db, string ShipmentID, Action<bool, string> Log)
        {

        }

        //        static Nullable<Guid> DoMoveInventoryFromInboundFBAShipment(Entities db, string Type, Guid From, Nullable<Guid> To, string ShipmentID, Dictionary<string, decimal> Changes, Action<bool, string> Log)
        //        {
        //            string SKUs = string.Join(",", Changes.Select(x => "'" + x.Key + "'").ToArray());

        //            var MissingASINs = db.ExecuteStoreQuery<string>(@"
        //                select distinct ais.ASIN 
        //                from AmazonInventorySKUs ais 
        //                left join AmazonInventoryProducts aip on ais.ASIN=aip.ASIN
        //                where aip.ASIN is null and ais.SKU in (" + SKUs + @")
        //            ").ToList();

        //            if (MissingASINs.Count() > 0)
        //            {
        //                Log(true, "MISSING PRODUCT ASSOCIATIONS FOR: " + string.Join<string>(",", MissingASINs));
        //                return null;
        //            }

        //            var itms = db.ExecuteStoreQuery<GetProductInventoryMoveListFromInboundFBAShipmentChanges>(@"
        //                select ProductID, ais.SKU, sum(aip.Qty) as Qty 
        //                from AmazonInventorySKUs ais 
        //                join AmazonInventoryProducts aip on ais.ASIN=aip.ASIN
        //                where ais.SKU in (" + SKUs + @")
        //                group by ProductID, ais.SKU
        //            ").ToList();

        //            Log(true, "Got " + itms.Count().ToString() + " unique products.");

        //            ProductInventoryAdjustmentBatch piab = CreateProductInventoryAdjustmentBatch(db, Type);

        //            foreach (var i in itms)
        //            {
        //                AdjustInventory(db, i.ProductID, From, -i.Qty * Changes[i.SKU], AmazonFBAOutboundReasonID, ShipmentID, null, null, piab.ID);

        //                if (To != null)
        //                    AdjustInventory(db, i.ProductID, (Guid)To, i.Qty * Changes[i.SKU], AmazonFBAOutboundReasonID, ShipmentID, null, null, piab.ID);
        //            }

        //            return piab.ID;

        //        }

        static Dictionary<string, decimal> GetInboundFBAShipmentItems(Entities db, AmazonAccount Account, InboundFBAShipment ifs, InboundShipmentInfo shp, List<Library.Throttler> Throttlers, Action<bool, string> Log)
        {
            Dictionary<string, decimal> Changes = new Dictionary<string, decimal>();

            int ChangeCount = 0;

            List<InboundShipmentItem> items = Library.GetInboundShipmentItems(Throttlers, ifs.AmazonAccount, shp.ShipmentId, Log);

            ifs.ItemCount = items.Count;

            List<string> SellerSKUs = items.Select(i => i.SellerSKU).ToList();

            var skus = db.AmazonInventorySKUs.Where(ais => SellerSKUs.Contains(ais.SKU)).ToList();
            var InboundFBAShipmentItems = db.InboundFBAShipmentItems.Where(ifsix => ifsix.ShipmentID == ifs.ID).ToList();

            foreach (InboundShipmentItem item in items)
            {
                bool ChangeCounted = false;

                AmazonInventorySKU sku = skus.Where(ais => ais.SKU == item.SellerSKU).FirstOrDefault();

                if (sku == null)
                {
                    Log(false, "Got new SKU " + item.SellerSKU + ". ");
                    IEnumerable<InventorySupplySummary> inventory = Library.GetInventory(Throttlers, Account, Log, new string[] { item.SellerSKU }.ToList());
                    Library.AddASINsToDBFromInventory(db, Account, inventory, Log);
                    Library.UpdateInventory(db, Account, inventory, Log, false, true);
                    Log(true, "");
                }

                InboundFBAShipmentItem ifsi = InboundFBAShipmentItems.Where(ifsix => ifsix.SKU == item.SellerSKU).FirstOrDefault();

                if (ifsi == null)
                {
                    ifsi = new InboundFBAShipmentItem();
                    ifsi.ShipmentID = shp.ShipmentId;
                    ifsi.SKU = item.SellerSKU;
                    ifsi.TimeStamp = DateTime.Now;

                    db.InboundFBAShipmentItems.Add(ifsi);

                    if (!ChangeCounted)
                    {
                        ChangeCount++;
                        ChangeCounted = true;
                    }
                }

                if (ifsi.QuantityShipped != item.QuantityShipped)
                {
                    Changes[item.SellerSKU] = item.QuantityShipped - (ifsi.QuantityShipped == null ? (decimal)0 : (decimal)ifsi.QuantityShipped);

                    ifsi.QuantityShipped = item.QuantityShipped;
                    ifsi.UpdateTimeStamp = DateTime.Now;
                    ifs.UpdateTimeStamp = DateTime.Now;

                    if (!ChangeCounted)
                    {
                        ChangeCount++;
                        ChangeCounted = true;
                    }
                }

                if (ifsi.QuantityReceived != item.QuantityReceived)
                {
                    ifsi.QuantityReceived = item.QuantityReceived;
                    ifsi.UpdateTimeStamp = DateTime.Now;
                    ifs.UpdateTimeStamp = DateTime.Now;

                    if (!ChangeCounted)
                    {
                        ChangeCount++;
                        ChangeCounted = true;
                    }
                }

                if (ifsi.QuantityInCase != item.QuantityInCase)
                {
                    ifsi.QuantityInCase = item.QuantityInCase;
                    ifsi.UpdateTimeStamp = DateTime.Now;
                    ifs.UpdateTimeStamp = DateTime.Now;

                    if (!ChangeCounted)
                    {
                        ChangeCount++;
                        ChangeCounted = true;
                    }
                }

            }

            Log(true, ChangeCount.ToString() + " Inbound Shipment Items added/updated.");

            var deleted_items = InboundFBAShipmentItems.Where(p => !items.Any(p2 => p2.SellerSKU == p.SKU));

            if (deleted_items.Count() > 0)
            {
                foreach (var di in deleted_items)
                    db.InboundFBAShipmentItems.Remove(di);

                Log(true, deleted_items.Count().ToString() + " Inbound Shipment Items deleted.");
            }


            return Changes;
        }

        //public static void HandleInboundFBAShipmentStatusChanges(Entities db, Action<bool, string> Log)
        //{

        //    DateTime START_DATE = DateTime.Parse("3/10/2014");

        //    foreach (var j in
        //        db.InboundFBAShipmentStatusChanges
        //            .Join(db.InboundFBAShipments, ibssc => ibssc.ShipmentID, ifs => ifs.ID, (ifssc, ifs) => new { ifssc, ifs })
        //            .Where(j => j.ifssc.ProductInventoryMoveID == null && j.ifssc.TimeStamp > START_DATE)
        //            .OrderBy(j => j.ifssc.TimeStamp)
        //            .ToList())
        //    {
        //        string LogMsg = null;
        //        Guid From = Guid.Empty;
        //        Nullable<Guid> To = null;
        //        string MoveType = null;

        //        // got new shipment in working status
        //        if (j.ifssc.OldStatus == null)
        //        {
        //            LogMsg = "Allocating inventory for";
        //            MoveType = "ALLOCATE-FBA";
        //            From = OrlandoLocationID;
        //        }

        //        if (LogMsg == null)
        //            Log(true, "Nothing to do with Shipment " + j.ifs.ID + "! OldStatus: " + (j.ifssc.OldStatus == null ? "null" : ((FBAInboundShipmentStatus)j.ifssc.OldStatus).ToString()) + " - NewStatus: " + ((FBAInboundShipmentStatus)j.ifssc.NewStatus).ToString());
        //        else
        //        {
        //            Log(false, LogMsg + " shipment " + j.ifs.ID + ": ");
        //            j.ifssc.ProductInventoryMoveID = MoveInventoryFromInboundFBAShipment(db, MoveType, From, To, j.ifs.ID, Log);
        //            db.SaveChanges();
        //        }

        //    }

        //}

        public static Guid AdjustInventory(Entities db, Guid ProductID, Guid LocationID, decimal Qty, Guid ReasonID, string ReasonRef1, string ReasonRef2, string ReasonRef3, Nullable<Guid> ProductInventoryAdjustmentBatchID = null)
        {
            DateTime Now = DateTime.Now;

            ProductInventory pi = db.ProductInventories.Where(pix => pix.ProductID == ProductID && pix.LocationID == LocationID).FirstOrDefault();

            if (pi == null)
            {
                pi = db.ProductInventories.Local.Where(pix => pix.LocationID == LocationID && pix.ProductID == ProductID).FirstOrDefault();

                if (pi == null)
                {
                    pi = new ProductInventory();
                    pi.LocationID = LocationID;
                    pi.ProductID = ProductID;
                    pi.TimeStamp = DateTime.Now;
                    pi.Qty = 0;
                    db.ProductInventories.Add(pi);
                }
            }

            return AdjustInventory(db, pi, LocationID, Now, Qty, ReasonID, ReasonRef1, ReasonRef2, ReasonRef3, ProductInventoryAdjustmentBatchID);
        }

        static Guid AdjustInventory(Entities db, ProductInventory pi, Guid LocationID, decimal Qty, Guid ReasonID, string ReasonRef1, string ReasonRef2, string ReasonRef3, Nullable<Guid> ProductInventoryAdjustmentBatchID = null)
        {
            return AdjustInventory(db, pi, LocationID, DateTime.Now, Qty, ReasonID, ReasonRef1, ReasonRef2, ReasonRef3, ProductInventoryAdjustmentBatchID);
        }

        static Guid AdjustInventory(Entities db, ProductInventory pi, Guid LocationID, DateTime Now, decimal Qty, Guid ReasonID, string ReasonRef1, string ReasonRef2, string ReasonRef3, Nullable<Guid> ProductInventoryAdjustmentBatchID = null)
        {

            ProductInventoryAdjustment pia = new ProductInventoryAdjustment();
            pia.ID = Guid.NewGuid();
            pia.TimeStamp = Now;
            pia.ProductID = pi.ProductID;
            pia.LocationID = LocationID;
            pia.ReasonID = ReasonID;
            pia.ReasonRef1 = ReasonRef1;
            pia.ReasonRef2 = ReasonRef2;
            pia.ReasonRef3 = ReasonRef3;
            pia.OldQty = pi.Qty;
            pia.NewQty = pi.Qty + Qty;
            pia.AdjustmentAmount = Qty;
            pia.ProductInventoryAdjustmentBatchID = ProductInventoryAdjustmentBatchID;

            pi.Qty = pi.Qty + Qty;
            pi.UpdateTimeStamp = Now;

            db.ProductInventoryAdjustments.Add(pia);


            return pia.ID;

        }

        public static IEnumerable<string> GetSettlementData(Entities db, Action<bool, string> Log, bool SaveChanges = true)
        {
            List<string> rtn = new List<string>();

            var Accounts = db.AmazonAccounts.ToList();

            foreach (var Account in Accounts.Where(a => a.Enabled || a.AccessKeyID == "AKIAJJE7HWBEDEJ7FXLQ"))
            {
                bool iserror = false;
                Log(true, "Getting Settlement Data for " + Account.Name);
                try
                {


                    ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 1800;
                    var Reports = Library.GetReportList(new List<Library.Throttler>() { new Library.Throttler(2000) }, Account, ReportType._GET_V2_SETTLEMENT_REPORT_DATA_FLAT_FILE_V2_, null, Log);

                    DateTime NewLastGetSettlementDate = DateTime.Now;

                    var OldestBeforeLast = Reports.Where(r => r.AvailableDate < Account.LastGetSettlementData).OrderByDescending(r => r.AvailableDate).Take(1).FirstOrDefault();

                    DateTime From = OldestBeforeLast == null ? Account.LastGetSettlementData : OldestBeforeLast.AvailableDate;

                    foreach (string ReportID in Reports.Where(r => r.AvailableDate >= From).Select(r => r.ReportId))
                    {

                        using (SqlConnection conn = new SqlConnection())
                        {
                            conn.ConnectionString = Startbutton.Library.GetConnectionString("Main");
                            conn.Open();

                            using (MemoryStream ReportStream = new MemoryStream())
                            {

                                Log(false, "Getting Report " + ReportID + "...");

                                GetReport(Account, ReportID, ReportStream, Log);

                                string x = Startbutton.Library.StreamToString(ReportStream, 0);

                                Log(false, " Formatting...");
                                DataTable Report = ReportStreamToDataTable(ReportStream);
                                Log(true, " Done!");

                                foreach (DataColumn col in Report.Columns)
                                    col.ColumnName = col.ColumnName.Replace("-", "_");

                                using (SqlTransaction tran = conn.BeginTransaction())
                                {

                                    using (SqlCommand cmd = conn.CreateCommand())
                                    {
                                        Log(true, "Deleting Settlements: " + string.Join(",", Report.AsEnumerable().Select(dr => dr.Field<string>(0)).Distinct().ToArray()));

                                        string SettlementIDs = "'" + string.Join("','", Report.AsEnumerable().Select(dr => dr.Field<string>(0)).Distinct().ToArray()) + "'";

                                        cmd.CommandText = "delete from AmazonSettlementTransactions where settlement_id in (" + SettlementIDs + ")";
                                        cmd.CommandType = CommandType.Text;
                                        cmd.Transaction = tran;
                                        cmd.CommandTimeout = 1800;
                                        cmd.ExecuteNonQuery();
                                    }

                                    Log(true, "Inserting Records... ");
                                    using (Startbutton.SqlTableCreator stc = new Startbutton.SqlTableCreator(conn, tran))
                                    {

                                        stc.DestinationTableName = "AmazonSettlementTransactions";
                                        stc.InsertIntoTable(Report, Log, true, true);
                                        //cmd.CommandType = CommandType.Text;
                                        //cmd.Transaction = tran;
                                        //cmd.CommandTimeout = 0;
                                        //cmd.ExecuteNonQuery();
                                    }

                                    Log(false, "Saving Changes... ");
                                    tran.Commit();
                                    Log(true, "Done!");
                                }

                            }

                            rtn.Add(ReportID);
                        }

                    }

                    Account.LastGetSettlementData = NewLastGetSettlementDate;

                    if (SaveChanges)
                    {
                        Log(true, "Saving NewLastGetSettlementDate.");
                        db.SaveChanges();
                    }
                }
                catch 
                {

                    iserror = true;
                }
                if (iserror) continue;

            }

            return rtn;

        }
    }
}
