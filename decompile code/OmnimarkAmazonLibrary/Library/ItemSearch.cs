using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarketplaceWebServiceOrders;
using MarketplaceWebServiceOrders.Model;
using OmnimarkAmazon.Models;
using Amazon.AWS;
using AmazonProductAdvtApi;
using System.IO;
using System.Xml;
using System.Net;
using System.Xml.Serialization;
using HtmlAgilityPack;
using System.Reflection;
using System.Threading;

namespace OmnimarkAmazon
{
    public static partial class Library
    {
        public class ScrapedText
        {
            public string HTML;
            public string Text;

            public ScrapedText(string HTML, string Text)
            {
                this.HTML = HTML;
                this.Text = Text;
            }
        }

        public class ScrapedItemAttributeInfo
        {
            public string ASIN;
            public DateTime TimeStamp;
            public Country Country;
        }

        public class ScrapedItemAttributes
        {
            public ScrapedItemAttributeInfo Info = new ScrapedItemAttributeInfo();
            public ScrapedText ItemDetails;
            public string ImageURL;
            public string Manufacturer;
            public Dictionary<string, string> ProductDetails = new Dictionary<string, string>();
            public ScrapedText ImportantInformation;
            public List<string> ProductFeatures = new List<string>();
            public string Title;
        }

        const string PDWTagToken = "<div class=\"productDescriptionWrapper\" >";

        static string[] AWSSearchIndicies = new string[] { "Baby", "Beauty", "HealthPersonalCare" };

        static MemoryStream GetAWSResponseStream(string RequestString, SignedRequestHelper helper)
        {
            string ResponseString = "";
            return GetAWSResponseStream(RequestString, helper, ref ResponseString);
        }

        static MemoryStream GetAWSResponseStream(string RequestString, SignedRequestHelper helper, ref string ResponseString)
        {
            string requestUrl = helper.Sign(RequestString);

            WebRequest Request = HttpWebRequest.Create(requestUrl);
            WebResponse Response = Request.GetResponse();
            Stream ResponseStream = Response.GetResponseStream();
            StreamReader sr = new StreamReader(ResponseStream);
            ResponseString = sr.ReadToEnd();
            return new MemoryStream(Startbutton.Library.StringToByteArray(ResponseString));
        }

        static T DeserialzeAWSResponse<T>(Stream AWSResponse)
        {
            XmlSerializer xmls = new XmlSerializer(typeof(T));

            try
            {
                return (T)xmls.Deserialize(AWSResponse);
            }
            catch (Exception Ex)
            {
                XmlDocument doc = new XmlDocument();
                AWSResponse.Seek(0, SeekOrigin.Begin);
                doc.Load(AWSResponse);

                String ErrorMessage = doc.GetElementsByTagName("Message", "")[0].InnerText;

                throw (new Exception(ErrorMessage, Ex));
            }

        }

        public static ItemLookupResponseItemsItem ItemLookup(int CountryCode, string ASIN, Action<bool, string> Log)
        {
            Entities db = new Entities();

            Country Country = db.Countries.Single(c => c.Code == CountryCode);

            SignedRequestHelper helper = new SignedRequestHelper(Country.AWSAccessKeyID, Country.AWSSecretAccessKey, Country.AWSServiceURL);

            String BaseRequestString = "Service=AWSECommerceService"
                + "&Version=2011-08-01"
                + "&Operation=ItemLookup"
                + "&AssociateTag=startbutton-20"
                + "&ItemId="
                + ASIN;

            String RequestString = BaseRequestString;
            DoLog(Log, false, "ItemLookup " + ASIN + ": ");
            string ResponseString = "";
            MemoryStream AWSResponse = null;

            while (ResponseString == "")
            {
                try
                {
                    AWSResponse = GetAWSResponseStream(RequestString, helper, ref ResponseString);
                }
                catch (Exception Ex)
                {
                    while (Ex.InnerException != null)
                        Ex = Ex.InnerException;
                    
                    DoLog(Log, true, "ERROR: " + Ex.Message); 
                }

            }

            ItemLookupResponse ilr = DeserialzeAWSResponse<ItemLookupResponse>(AWSResponse);

            if (ilr.Items.Item == null)
                Log(true, "NOT FOUND!");
            else
                Log(true, "Found.");
            
            return ilr.Items.Item;

        }

        public static List<ItemSearchResponseItemsItem> ItemSearch(int CountryCode, string Search)
        {
            return ItemSearch(CountryCode, Search, null);
        }

        public static List<ItemSearchResponseItemsItem> ItemSearch(int CountryCode, string Search, Action<bool, string> Log)
        {

            Entities db = new Entities();

            Country Country = db.Countries.Single(c => c.Code == CountryCode);

            SignedRequestHelper helper = new SignedRequestHelper(Country.AWSAccessKeyID, Country.AWSSecretAccessKey, Country.AWSServiceURL);

            List<ItemSearchResponseItemsItem> rtn = new List<ItemSearchResponseItemsItem>();

            foreach (string SearchIndex in AWSSearchIndicies)
            {

                String BaseRequestString = "Service=AWSECommerceService"
                    + "&Version=2011-08-01"
                    + "&Operation=ItemSearch"
                    + "&SearchIndex=" + SearchIndex
                    + "&ResponseGroup=Small"
                    + "&AssociateTag=startbutton-20"
                    + "&Keywords="
                    + Search;

                String RequestString = BaseRequestString;
                int CurrentPage = 1;

                while (true)
                {
                    if (Log != null)
                        Log(true, "Index: " + SearchIndex + " - " + "Page: " + CurrentPage.ToString());

                    string ResponseString = "";
                    MemoryStream AWSResponse = null;

                    while (ResponseString == "")
                    {
                        try
                        {
                            AWSResponse = GetAWSResponseStream(RequestString, helper, ref ResponseString);
                        }
                        catch (Exception Ex)
                        {
                            while (Ex.InnerException != null)
                                Ex = Ex.InnerException;

                            if (Log != null)
                                Log(true, "ERROR: " + Ex.Message);
                        }
                    }

                    ItemSearchResponse isr = DeserialzeAWSResponse<ItemSearchResponse>(AWSResponse);

                    if (isr.Items.TotalResults == 0)
                        break;

                    foreach (ItemSearchResponseItemsItem i in isr.Items.Item)
                        rtn.Add(i);

                    if (isr.Items.TotalPages == CurrentPage || CurrentPage == 10)
                        break;

                    RequestString = BaseRequestString + "&ItemPage=" + (++CurrentPage).ToString();

                }

            }

            return rtn;

        }

        public static int AddASINsToDB(Country Country, List<ItemSearchResponseItemsItem> NewItems, string SearchTerm, Action<bool, string> Log)
        {
            Entities db = new Entities();

            int AddedCount = 0;

            foreach (ItemSearchResponseItemsItem i in NewItems)
            {

                KnownASIN Lookup;

                if ((Lookup = db.KnownASINs.Where(ka => ka.ASIN == i.ASIN && ka.MarketPlaceID == Country.AmazonMarketPlaceID).FirstOrDefault()) != null)
                {
                    Lookup.LastSeen = DateTime.Now;
                    db.SaveChanges();
                }
                else
                {
                    bool Filter = false;

                    if (i.ASIN != null && i.ItemAttributes != null && i.ItemAttributes.Title != null)
                    {
                        if (!i.ItemAttributes.Title.ToLower().Contains(SearchTerm.ToLower()))
                        {
                            ScrapedItemAttributes sia = ScrapeItemAttributes(null, null, Country, i.ASIN, i.DetailPageURL, false, Log);

                            if (sia == null)
                            {
                                Filter = true;

                                if (Log != null)
                                    Log(true, "Scrape failed.");
                            }
                            else
                            {
                                if (sia.ItemDetails == null)
                                {
                                    Log(true, "No productDescriptionWrapper!");
                                    Filter = true;
                                }
                                else
                                {
                                    if (!sia.ItemDetails.Text.ToLower().Contains(SearchTerm))
                                    {
                                        Filter = true;

                                        if (Log != null)
                                            Log(true, "Filtered.");
                                    }
                                    else
                                        if (Log != null)
                                            Log(true, "OK.");
                                }
                            }
                        }

                        KnownASIN NewRec = new KnownASIN();
                        NewRec.ASIN = i.ASIN;
                        NewRec.SearchTerm = SearchTerm;
                        NewRec.Title = i.ItemAttributes.Title.Length > 500 ? i.ItemAttributes.Title.Substring(0, 500) : i.ItemAttributes.Title;
                        NewRec.Filtered = Filter;
                        NewRec.TimeStamp = DateTime.Now;
                        NewRec.LastSeen = DateTime.Now;
                        NewRec.MarketPlaceID = Country.AmazonMarketPlaceID;

                        db.KnownASINs.Add(NewRec);
                        db.SaveChanges();

                        AddedCount++;
                    }

                }
            }

            return AddedCount;

        }

        public static ScrapedItemAttributes ScrapeItemAttributes(Entities db, Country Country, string ASIN, string URL, bool UpdateKnownASINs, Action<bool, string> Log, bool SaveChanges = false)
        {
            KnownASIN ka = null;

            if (UpdateKnownASINs)
                ka = db.KnownASINs.Single(kax => kax.ASIN == ASIN);

            return ScrapeItemAttributes(db, ka, Country, ASIN, URL, UpdateKnownASINs, Log, SaveChanges);
        }

        static HtmlDocument GetDocumentFromURL(string URL, Action<bool, string> Log)
        {
            Stream stream = null;
            int TryCount = 0;
            Exception Ex = null;
            HtmlDocument doc = new HtmlDocument();
            int SleepTime = 0;

            while (true)
            {
                Ex = null;

                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                    request.Method = "GET";
                    request.Timeout = 10000;
                    request.ReadWriteTimeout = 10000;
                    request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.168 Safari/535.19";

                    stream = Startbutton.Library.GetStreamFromHTTPAction(request, 10000);

                    try
                    {
                        doc.Load(stream);
                    }
                    catch (Exception Ex2)
                    {
                        Log(true, "Exception: " + Ex2.Message + " when reading stream!");
                    }

                    stream.Close();
                    stream.Dispose();

                }
                catch (Exception Exx)
                {
                    if (Log != null)
                        Log(false, "ERROR: " + Exx.Message + ".  ");

                    Thread.Sleep(SleepTime);
                    SleepTime += 500;

                    Ex = Exx;
                }

                if (Ex == null)
                    break;
                else if (++TryCount == 5)
                {
                    if (Log != null)
                        Log(true, "Giving up.");
                    break;
                }
            }

            return doc;
        }

        public static int ScrapeOfferListings(Entities db, ASINOfferScrape aos, string ASIN, Action<bool, string> Log, bool SaveChanges = false, bool LogDetails = false)
        {
            int rtn = 0;

            foreach (Country c in db.Countries.Where(cx => cx.DefaultURL != null).ToList())
                if (ScrapeOfferListings(db, aos, c, ASIN, Log, SaveChanges, LogDetails))
                    rtn++;

            return rtn;
        }

        static void FailScrape(ASINOfferScrape aos)
        {
            if (aos.FirstScrapeFail == null)
                aos.FirstScrapeFail = DateTime.Now;
            else
                aos.LastScrapeFail = DateTime.Now;

            if (aos.ScrapeFailCount == null)
                aos.ScrapeFailCount = 0;

            aos.ScrapeFailCount++;

        }

        public static bool ScrapeOfferListings(Entities db, ASINOfferScrape aos, Country Country, string ASIN, Action<bool, string> Log, bool SaveChanges = false, bool LogDetails = false)
        {

            int OfferCounter = 0;
            int PageCounter = 0;

            Log(true, "Scraping Offers for " + ASIN + " in " + Country.CountryName + ": ");

            if (aos == null)
                aos = db.ASINOfferScrapes.Where(aosx => aosx.ASIN == ASIN && aosx.CountryID == Country.Code).FirstOrDefault();

            if (aos == null)
            {
                aos = new ASINOfferScrape();
                aos.CountryID = Country.Code;
                aos.ASIN = ASIN;
                aos.TimeStamp = DateTime.Now;
                db.ASINOfferScrapes.Add(aos);
            }

            aos.LastScrapeTry = DateTime.Now;

            string URL = Country.DefaultURL + "/gp/offer-listing/" + ASIN + "?condition=new";

            HtmlDocument doc = GetDocumentFromURL(URL, Log);

            if (!doc.DocumentNode.HasChildNodes)
            {
                FailScrape(aos);

                if (SaveChanges)
                    db.SaveChanges();

                return false;
            }

            HtmlNodeCollection Nodes = doc.DocumentNode.SelectNodes("//input[@name='offeringID.1']");

            if (Nodes == null)
            {
                Log(true, "No offers.");
                FailScrape(aos);

                if (SaveChanges)
                    db.SaveChanges();

                return false;
            }
            else
            {


                foreach (var rec in db.ASINOfferListings.Where(aolx => aolx.ASIN == ASIN && aolx.CountryID == Country.Code).ToList())
                    db.ASINOfferListings.Remove(rec);

                aos.FirstScrapeFail = null;
                aos.LastScrapeFail = null;
                aos.ScrapeFailCount = null;

                PageCounter++;

                aos.ActualPages = null;
                aos.RecordsPerPage = null;
                aos.ActualOffers = null;

                string x = doc.ToString();


                OfferCounter += ProcessOfferPage(db, ASIN, Country, doc, Log, LogDetails);

                Nodes = doc.DocumentNode.SelectNodes("//li[@class='a-last']/a");

                while (Nodes != null)
                {

                    Log(true, "Scraping Offers for " + ASIN + " in " + Country.CountryName + " - Page " + (PageCounter + 1).ToString() + ": ");

                    URL = Country.DefaultURL + System.Net.WebUtility.HtmlDecode(Nodes[0].Attributes["href"].Value);

                    doc = GetDocumentFromURL(URL, Log);

                    if (doc.DocumentNode.HasChildNodes)
                    {
                        OfferCounter += ProcessOfferPage(db, ASIN, Country, doc, Log, LogDetails);
                        PageCounter++;
                        Nodes = doc.DocumentNode.SelectNodes("//li[@class='a-last']/a");
                    }
                    else
                        break;

                }

                aos.ActualOffers = OfferCounter;
                aos.ActualPages = PageCounter;
                aos.LastScrape = DateTime.Now;
                aos.PagesScraped = PageCounter;

                Log(true, "Got " + OfferCounter.ToString() + " offers.");

            }

            if (SaveChanges)
                db.SaveChanges();

            return true;

        }

        static int ProcessOfferPage(Entities db, string ASIN, Country Country, HtmlDocument doc, Action<bool, string> Log, bool LogDetails)
        {
            int OfferCounter = 0;

          
            HtmlNodeCollection Nodes = doc.DocumentNode.SelectNodes("//div[contains(@class,'olpOffer')]/div/p/a[starts-with(@href,'/gp/aag/main')]");

            if (Nodes != null)
            {

                foreach (var node in Nodes)
                {

                    string RowPath = node.ParentNode.ParentNode.ParentNode.XPath;
                    string href = node.Attributes["href"].Value;

                    Dictionary<string, string> vals = new Dictionary<string, string>();

                    string[] a = href.Split('?');

                    string[] b = a[1].Split(new string[] { "&amp;" }, StringSplitOptions.None);

                    for (int x = 0; x < b.Length; x++)
                    {
                        string[] c = b[x].Split('=');
                        vals.Add(c[0], c[1]);
                    }

                    vals["price"] = (Nodes = node.SelectNodes(RowPath + "//span[contains(@class,'olpOfferPrice')]")) == null ? null : Nodes.FirstOrDefault().InnerText.Trim();

                    if (Nodes == null)
                    {
                        Log(false, "No price for seller " + vals["seller"] + ".  ");
                        continue;
                    }

                    string PriceParentPath = Nodes[0].ParentNode.XPath;

                    vals["priceShipping"] = (Nodes = node.SelectNodes(PriceParentPath + "//span[@class='a-color-secondary']")) == null ? null : Nodes.FirstOrDefault().InnerText.Trim();
                    vals["pricePerUnit"] = (Nodes = node.SelectNodes(PriceParentPath + "//span[@class='pricePerUnit']")) == null ? null : Nodes.FirstOrDefault().InnerText;

                    if (LogDetails)
                    {
                        Log(false, "Got Offer: ");

                        bool first = true;

                        foreach (string k in vals.Keys)
                        {
                            if (first)
                                first = false;
                            else
                                Log(false, ",");

                            Log(false, k + "=" + vals[k]);
                        }

                        Log(true, "");
                    }

                    string MerchantID = vals["seller"];
                    ASINOfferListing aol = new ASINOfferListing();
                    aol.ID = Guid.NewGuid();
                    aol.ASIN = ASIN;
                    aol.MerchantID = MerchantID;
                    aol.TimeStamp = DateTime.Now;
                    aol.CountryID = Country.Code;
                    db.ASINOfferListings.Add(aol);

                    aol.AmazonFulfilled = vals["isAmazonFulfilled"] == "1";
                    aol.Price = decimal.Parse(vals["price"].Split(new string[] { Country.CurrencySymbolPrefix }, StringSplitOptions.None)[1]);
                    aol.OfferingIDDecoded = null;

                    if (vals.ContainsKey("priceShipping"))
                        if (vals["priceShipping"] != null)
                        {
                            string[] aa = vals["priceShipping"].Split(new string[] { Country.CurrencySymbolPrefix }, StringSplitOptions.None);

                            if (aa.Length > 1)
                            {
                                decimal sp = 0;
                                if (decimal.TryParse(aa[1].Split('\n')[0], out sp))
                                    aol.ShippingPrice = sp;
                            }
                        }

                    if (vals.ContainsKey("pricePerUnit"))
                        aol.PricePerUnit = vals["pricePerUnit"];

                    aol.UpdateTimeStamp = DateTime.Now;

                    OfferCounter++;

                }

            }

            return OfferCounter;
        }

        public static ScrapedItemAttributes ScrapeItemAttributes(Entities db, KnownASIN ka, Country Country, string ASIN, string URL, bool UpdateKnownASINs, Action<bool, string> Log, bool SaveChanges = false)
        {
            if (UpdateKnownASINs)
                ka.LastScrapeTry = DateTime.Now;

            if (URL == null)
                URL = Country.DefaultItemDetailsURL + ASIN;

            if (Log != null)
                Log(false, "Scraping: " + ASIN + ": " + URL + ": ");

            HtmlDocument doc = GetDocumentFromURL(URL, Log);

            if (!doc.DocumentNode.HasChildNodes)
            {
                if (UpdateKnownASINs)
                {
                    if (ka.FirstScrapeFail == null)
                        ka.FirstScrapeFail = DateTime.Now;
                    else
                        ka.LastScrapeFail = DateTime.Now;

                    if (ka.ScrapeFailCount == null)
                        ka.ScrapeFailCount = 0;

                    ka.ScrapeFailCount++;

                    if (ka.ScrapeFailCount > 20)
                    {
                        if (((DateTime)ka.FirstScrapeFail).AddDays(14) < ((DateTime)ka.LastScrapeFail))
                            ka.GoneFromAmazon = true;
                    }
                }

                if (SaveChanges)
                    db.SaveChanges();

                return null;
            }

            if (SaveChanges)
                db.SaveChanges();

            ScrapedItemAttributes sia = new ScrapedItemAttributes();
            sia.Info.ASIN = ASIN;
            sia.Info.TimeStamp = DateTime.Now;
            sia.Info.Country = Country;

            HtmlNodeCollection Nodes;

            sia.Title = (Nodes = doc.DocumentNode.SelectNodes("//span[@id='btAsinTitle']")) == null ? null : System.Net.WebUtility.HtmlDecode(Nodes.FirstOrDefault().InnerHtml).Trim();
            sia.ItemDetails = (Nodes = doc.DocumentNode.SelectNodes("//div[@class='productDescriptionWrapper']")) == null ? null : new ScrapedText(System.Net.WebUtility.HtmlDecode(Nodes.FirstOrDefault().InnerHtml).Trim(), System.Net.WebUtility.HtmlDecode(Nodes.FirstOrDefault().InnerText).Trim());
            sia.Manufacturer = (Nodes = doc.DocumentNode.SelectNodes("//span[@class='bxgy-byline-text']")) == null ? null : Nodes.FirstOrDefault().InnerText.Replace("by ", "");

            sia.ImageURL = (Nodes = doc.DocumentNode.SelectNodes("//img[@id='prodImage']")) == null ? null : Nodes.FirstOrDefault().Attributes["src"].Value;

            if (sia.ImageURL == null)
            {

                string TryUrl = Country.DefaultURL + "/gp/product/images/" + ASIN + "/ref=dp_image_0?ie=UTF8&s=miscellaneous";

                Log(false, "No ImageURL.  Checking: " + TryUrl + ": ");

                HtmlDocument doc2 = GetDocumentFromURL(TryUrl, Log);

                if (doc2.DocumentNode.HasChildNodes)
                {
                    sia.ImageURL = (Nodes = doc2.DocumentNode.SelectNodes("//img[@id='prodImage']")) == null ? null : Nodes.FirstOrDefault().Attributes["src"].Value;

                    if (sia.ImageURL == null)
                        Log(true, "Still no image!");
                    else
                        Log(true, "GOOD!");
                }
            }

            #region ProductDetails
            HtmlNode ProductDetailsNode = doc.DocumentNode.SelectSingleNode("//h2[.='Product Details']");

            while (ProductDetailsNode != null)
            {
                if (ProductDetailsNode.Name == "div")
                    if (ProductDetailsNode.Attributes["class"] != null)
                        if (ProductDetailsNode.Attributes["class"].Value == "content")
                            break;

                ProductDetailsNode = ProductDetailsNode.NextSibling;
            }

            if (ProductDetailsNode != null)
                foreach (HtmlNode n in ProductDetailsNode.SelectNodes("ul/li"))
                {
                    if (n.ChildNodes.Count > 1)
                    {
                        HtmlNode NameNode = n.SelectNodes("b")[0];
                        string name = System.Net.WebUtility.HtmlDecode(NameNode.InnerText).Trim().Replace(" ", "").Replace(":", "");

                        string value = "";

                        HtmlNode ValueNode = NameNode;

                        while ((ValueNode = ValueNode.NextSibling) != null)
                            if (ValueNode.Name.ToLower() != "script")
                                value += System.Net.WebUtility.HtmlDecode(ValueNode.InnerText);

                        // string name = n.ChildNodes[0].InnerText.Trim().Replace(" ", "").Replace(":", "");
                        // string value = n.ChildNodes[1].InnerText.Trim();

                        //if (value.EndsWith(" ("))
                        //    value = value.Substring(0, value.Length - 2);

                        value = value.Trim();

                        if (value != "")
                            if (name != "ASIN" && name != "")
                                sia.ProductDetails.Add(name, value);
                    }
                }
            #endregion

            #region ImportantInformation
            HtmlNode ImportantInformationNode = doc.DocumentNode.SelectSingleNode("//h2[.='Important Information']");

            while (ImportantInformationNode != null)
            {
                if (ImportantInformationNode.Name == "div")
                    if (ImportantInformationNode.Attributes["class"] != null)
                        if (ImportantInformationNode.Attributes["class"].Value == "content")
                            break;

                ImportantInformationNode = ImportantInformationNode.NextSibling;
            }

            if (ImportantInformationNode != null)
                sia.ImportantInformation = new ScrapedText(ImportantInformationNode.InnerHtml.Trim(), ImportantInformationNode.InnerText.Trim());

            #endregion

            #region ProductFeatures
            HtmlNode ProductFeaturesNode = doc.DocumentNode.SelectSingleNode("//h2[.='Product Features']");

            while (ProductFeaturesNode != null)
            {
                if (ProductFeaturesNode.Name == "div")
                    if (ProductFeaturesNode.Attributes["class"] != null)
                        if (ProductFeaturesNode.Attributes["class"].Value == "content")
                            break;
                
                ProductFeaturesNode = ProductFeaturesNode.NextSibling;
            }

            if (ProductFeaturesNode != null)
                foreach (HtmlNode n in ProductFeaturesNode.SelectNodes("ul/li"))
                {
                    if (n.ChildNodes.Count > 0)
                    {
                        string name = n.ChildNodes[0].InnerText.Trim().Replace(" ", "").Replace(":", "");
                        string value = n.ChildNodes[0].InnerText.Trim();

                        if (value != "")
                            sia.ProductFeatures.Add(value);
                    }
                }
            #endregion

            if (Log != null)
                Log(true, "Success!");
            
            return sia;
        }

        public static int SaveScrapedAttributes(Entities db, ScrapedItemAttributes sia)
        {
            int rtn = 0;
            KnownASIN ka = db.KnownASINs.Where(kax => kax.ASIN == sia.Info.ASIN).FirstOrDefault();

            if (ka == null)
            {
                ka = new KnownASIN();
                ka.ASIN = sia.Info.ASIN;
                ka.Filtered = false;
                ka.TimeStamp = DateTime.Now;
                ka.Reviewed = ka.TimeStamp;
                ka.OurProduct = true;
                ka.MarketPlaceID = sia.Info.Country.AmazonMarketPlaceID;

                db.KnownASINs.Add(ka);
            }

            ka.LastScrape = sia.Info.TimeStamp;
            ka.FirstScrapeFail = null;
            ka.LastScrapeFail = null;
            ka.ScrapeFailCount = null;

            if (sia.Title != null)
                ka.Title = sia.Title;

            foreach (PropertyInfo mi in typeof(ScrapedItemAttributes).GetProperties())
                ProcessScrapedItemAttributeMember(db, sia, mi.PropertyType, (MemberInfo)mi, (siax => mi.GetValue(siax, null)), ref rtn);

            foreach (FieldInfo mi in typeof(ScrapedItemAttributes).GetFields())
                ProcessScrapedItemAttributeMember(db, sia, mi.FieldType, (MemberInfo)mi, (siax => mi.GetValue(siax)), ref rtn);

            return rtn;
        }

        static void ProcessScrapedItemAttributeMember(Entities db, ScrapedItemAttributes sia, Type Type, MemberInfo mi, Func<ScrapedItemAttributes, object> GetValue, ref int Count)
        {

            if (Type == typeof(string))
            {
                if (AddOrUpdateKnownASINAttribute(db, sia.Info.ASIN, mi.Name, (string)GetValue(sia)))
                    Count++;
            }
            else if (Type == typeof(Dictionary<string, string>))
            {
                Dictionary<string, string> values = (Dictionary<string, string>)GetValue(sia);

                foreach (string name in values.Keys)
                    if (AddOrUpdateKnownASINAttribute(db, sia.Info.ASIN, mi.Name + "_" + name, values[name]))
                        Count++;
            }
            else if (Type == typeof(ScrapedText))
            {

                ScrapedText st = (ScrapedText)GetValue(sia);

                if (st != null)
                {
                    if (AddOrUpdateKnownASINAttribute(db, sia.Info.ASIN, mi.Name + "_HTML", st.HTML))
                        Count++;

                    if (AddOrUpdateKnownASINAttribute(db, sia.Info.ASIN, mi.Name + "_Text", st.Text))
                        Count++;
                }
            }
            else if (Type == typeof(List<string>))
            {
                List<string> values = (List<string>)GetValue(sia);

                int x = 0;

                foreach (string value in values)
                    if (AddOrUpdateKnownASINAttribute(db, sia.Info.ASIN, mi.Name + "_" + (++x).ToString(), value))
                        Count++;
            }
        }

        static bool AddOrUpdateKnownASINAttribute(Entities db, string ASIN, string Name, string Value)
        {
            if (Value == null)
                return false;

            KnownASINAttribute kaa = db.KnownASINAttributes.Where(kaax => kaax.ASIN == ASIN && kaax.Name == Name).FirstOrDefault();

            if (kaa == null)
            {
                kaa = new KnownASINAttribute();
                kaa.ASIN = ASIN;
                kaa.Name = Name;
                kaa.Value = Value;
                kaa.TimeStamp = DateTime.Now;
                db.KnownASINAttributes.Add(kaa);

                return true;
            }
            else if (kaa.Value != Value)
            {
                kaa.Value = Value;
                kaa.UpdateTimeStamp = DateTime.Now;

                return true;
            }

            return false;

        }

        public static IEnumerable<KnownASIN> GetASINsToScrape(Entities db, int Count, bool MustBeOurProduct = true)
        {
            if (MustBeOurProduct)
                return db.KnownASINs.Where(kax => kax.OurProduct == true && kax.GoneFromAmazon == false).OrderBy(kax => kax.LastScrapeTry).ThenBy(kax => kax.ASIN).Take(Count);
            else
                return db.KnownASINs.Where(kax => kax.GoneFromAmazon == false).OrderBy(kax => kax.LastScrapeTry).ThenBy(kax => kax.ASIN).Take(Count);
        }
    }

}
