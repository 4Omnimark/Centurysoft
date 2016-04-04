using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OmnimarkAmazon.Models;
using Startbutton.ExtensionMethods;
using com.ddmresources.EnumMetadata.Core;
using System.Web.Routing;

namespace OmnimarkAmazonWeb.Controllers
{
    public class AutomaticPriceChangeController : _BaseController
    {

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            RecTypeName = "Price Change";
        }

        void LoadSpanTypesToViewBag()
        {
            ViewBag.SpanTypes = new SelectList(Startbutton.Library.GetEnumValues<Startbutton.SpanType>().Select(i => new { ID = ((int)i).ToString(), Name = i.Info<Startbutton.SpanTypeInfoAttribute>().DisplayName }), "ID", "Name");
        }

        void LoadSelectListsToViewBag()
        {
            LoadSpanTypesToViewBag();
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Automatic Price Changes";
            var model = db.AutomaticPriceChangesView.OrderBy(apc => apc.Title);
            return View(model);
        }

        public ActionResult Create()
        {
            InitViewBag(ActionType.Create);
            LoadSelectListsToViewBag();

            return View("Edit", new AutomaticPriceChangeWeb());

        }

        TimeSpan ParseTime(string TimeString)
        {
            string[] a = TimeString.Split(' ');
            string[] b = a[0].Split(':');

            int hours = int.Parse(b[0]);
            int mins = int.Parse(b[1]);

            if (a[1] == "PM")
                hours += 12;

            return new TimeSpan(hours, mins, 0);
        }

        [HttpPost]
        public ActionResult Create(AutomaticPriceChangeWeb Form)
        {
            if (!ModelState.IsValid)
            {
                RecTypeName = "Price Change";
                InitViewBag(ActionType.Create);
                LoadSelectListsToViewBag();

                return View("Edit", Form);
            }
            else
            {
                AmazonInventorySKU ais = db.AmazonInventorySKUs.Where(aisx => aisx.SKU == Form.SKU).FirstOrDefault();

                if (ais == null)
                {
                    if ((Form.AmazonAccountID == null || Form.ASIN == null))
                    {
                        ModelState.AddModelError("", "SKU is unknown so you must select the Amazon Store and enter the ASIN");

                        RecTypeName = "Price Change";
                        InitViewBag(ActionType.Create);
                        LoadSelectListsToViewBag();
                        ViewBag.UnknownSKU = true;
                        ViewBag.AmazonAccounts = new SelectList(db.AmazonAccounts.ToList().Select(aa => new { ID = aa.ID.ToString(), Name = aa.Name }), "ID", "Name");

                        return View("Edit", Form);
                    }
                    else
                    {
                        if (db.KnownASINs.Count(ka => ka.ASIN == Form.ASIN) == 0)
                        {
                            ModelState.AddModelError("", "Unknown ASIN");

                            RecTypeName = "Price Change";
                            InitViewBag(ActionType.Create);
                            LoadSelectListsToViewBag();
                            ViewBag.UnknownSKU = true;
                            ViewBag.AmazonAccounts = new SelectList(db.AmazonAccounts.ToList().Select(aa => new { ID = aa.ID.ToString(), Name = aa.Name }), "ID", "Name");

                            return View("Edit", Form);
                        }
                    }
                }

                int ServerDateTimeOffset = db.GetServerDateTimeOffset();

                Startbutton.TimeSpan TheTimeSpan;
                Startbutton.SpanType SpanType = (Startbutton.SpanType)Form.TimeSpanSpanType;

                string Parm = null;

                if (SpanType == Startbutton.SpanType.DaysOfWeek)
                {
                    Parm = "";

                    for (int x = 0; x < 7; x++)
                        if (Request.Form["cb" + ((DayOfWeek)x).ToString()] != null)
                            Parm += ((Startbutton.DayOfWeekAbbr)x);
                }

                TheTimeSpan = new Startbutton.TimeSpan((int)Form.TimeSpanCount, SpanType, Parm);

                DateTime StartDateTime = ((DateTime)Form.TempPriceStartDateTime).Add(ParseTime(Form.TempPriceStartTime));
                DateTime EndDateTime = ((DateTime)Form.TempPriceEndDateTime).Add(ParseTime(Form.TempPriceEndTime));
                DateTime Now = db.GetServerNow(ServerDateTimeOffset);

                string Title;

                AutomaticPriceChange apc = new AutomaticPriceChange();
                apc.ID = Guid.NewGuid();
                apc.SKU = Form.SKU;
                apc.TempPrice = (decimal)Form.TempPrice;
                apc.RegularPrice = (decimal)Form.RegularPrice;
                apc.TempPriceStartDateTime = StartDateTime;
                apc.TempPriceEndDateTime = EndDateTime;
                apc.TimeSpan = TheTimeSpan.ToString();
                apc.TimeStamp = Now;

                if (ais == null)
                {
                    apc.AmazonAccountID = (Guid)Form.AmazonAccountID;
                    apc.ASIN = Form.ASIN;
                    Title = db.KnownASINs.Single(ka => ka.ASIN == Form.ASIN).Title;
                }
                else
                {
                    apc.AmazonAccountID = ais.AmazonAccountID;
                    apc.ASIN = ais.ASIN;
                    Title = ais.KnownASIN.Title;
                }

                string AccountName = db.AmazonAccounts.Single(aa => aa.ID == apc.AmazonAccountID).Name;

                FeedSubmissionScheduleRec fssr = new FeedSubmissionScheduleRec();
                fssr.ID = Guid.NewGuid();
                fssr.AmazonAccountID = apc.AmazonAccountID;
                fssr.FeedType = OmnimarkAmazon.Library.FeedType._POST_PRODUCT_PRICING_DATA_.ToString();
                fssr.MessageType = Amazon.XML.AmazonEnvelopeMessageType.Price.ToString();
                fssr.MessageXML = Startbutton.Library.XmlSerialize(new Amazon.XML.Price(Form.SKU, (decimal)Form.TempPrice));
                fssr.NextRun = StartDateTime;
                fssr.TimeSpan = TheTimeSpan.ToString();
                fssr.Name = "Start Price Change for " + Title + " on " + AccountName;
                fssr.TimeStamp = Now;
                fssr.Enabled = true;
                db.FeedSubmissionSchedule.Add(fssr);

                apc.StartFeedSubmissionScheduleID = fssr.ID;

                fssr = new FeedSubmissionScheduleRec();
                fssr.ID = Guid.NewGuid();
                fssr.AmazonAccountID = apc.AmazonAccountID;
                fssr.FeedType = OmnimarkAmazon.Library.FeedType._POST_PRODUCT_PRICING_DATA_.ToString();
                fssr.MessageType = Amazon.XML.AmazonEnvelopeMessageType.Price.ToString();
                fssr.MessageXML = Startbutton.Library.XmlSerialize(new Amazon.XML.Price(Form.SKU, (decimal)Form.RegularPrice));
                fssr.NextRun = EndDateTime;
                fssr.TimeSpan = TheTimeSpan.ToString();
                fssr.Name = "End Price Change for " + Title + " on " + AccountName;
                fssr.TimeStamp = Now;
                fssr.Enabled = true;
                db.FeedSubmissionSchedule.Add(fssr);

                apc.EndFeedSubmissionScheduleID = fssr.ID;

                db.SaveChanges();

                db.AutomaticPriceChanges.Add(apc);

                db.SaveChanges();

                return RedirectToAction("Index");

            }
        }

        public ActionResult Delete(Guid id)
        {
            return View(db.AutomaticPriceChanges.Single(apc => apc.ID == id));
        }

        [HttpPost]
        public ActionResult Delete(Guid id, object x)
        {
            var rec = db.AutomaticPriceChanges.Single(apc => apc.ID == id);

            OmnimarkAmazon.BLL.FeedSubmissionSchedule.Delete(db, rec.StartFeedSubmissionScheduleID);
            OmnimarkAmazon.BLL.FeedSubmissionSchedule.Delete(db, rec.EndFeedSubmissionScheduleID);

            db.AutomaticPriceChanges.Remove(rec);

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(Guid id)
        {
            InitViewBag(ActionType.Edit);
            LoadSelectListsToViewBag();

            var rec = db.AutomaticPriceChangesView.Single(apc => apc.ID == id);

            AutomaticPriceChangeWeb model = new AutomaticPriceChangeWeb();

            Startbutton.Library.SetEntityMatchingFields(model, rec);
            model.TempPriceStartTime = rec.NextStartRun.ToString("h:mm tt");
            model.TempPriceEndTime = rec.NextEndRun.ToString("h:mm tt");
            model.TempPriceEndDateTime = rec.NextEndRun.Date;
            model.TempPriceStartDateTime = rec.NextStartRun.Date;

            Startbutton.TimeSpan ts = new Startbutton.TimeSpan(rec.TimeSpan);
            model.TimeSpanCount = ts.Count;
            model.TimeSpanSpanType = (int)ts.SpanType;

            if (ts.SpanType == Startbutton.SpanType.DaysOfWeek)
                ViewBag.DaysOfWeek = ts.DaysOfWeekArray;

            if (db.AmazonInventorySKUs.Count(ais => ais.SKU == rec.SKU) == 0)
            {
                ViewBag.UnknownSKU = true;
                ViewBag.AmazonAccounts = new SelectList(db.AmazonAccounts.ToList().Select(aa => new { ID = aa.ID.ToString(), Name = aa.Name }), "ID", "Name");
            }

            return View("Edit", model);

        }

        [HttpPost]
        public ActionResult Edit(Guid id, AutomaticPriceChangeWeb Form)
        {
            if (!ModelState.IsValid)
            {
                InitViewBag(ActionType.Edit);
                LoadSelectListsToViewBag();
                return View("Edit", Form);
            }
            else
            {
                AmazonInventorySKU ais = db.AmazonInventorySKUs.Where(aisx => aisx.SKU == Form.SKU).FirstOrDefault();

                if (ais == null)
                {
                    if ((Form.AmazonAccountID == null || Form.ASIN == null))
                    {
                        ModelState.AddModelError("", "SKU is unknown so you must select the Amazon Store and enter the ASIN");

                        RecTypeName = "Price Change";
                        InitViewBag(ActionType.Create);
                        LoadSelectListsToViewBag();
                        ViewBag.UnknownSKU = true;
                        ViewBag.AmazonAccounts = new SelectList(db.AmazonAccounts.ToList().Select(aa => new { ID = aa.ID.ToString(), Name = aa.Name }), "ID", "Name");

                        return View("Edit", Form);
                    }
                    else
                    {
                        if (db.KnownASINs.Count(ka => ka.ASIN == Form.ASIN) == 0)
                        {
                            ModelState.AddModelError("", "Unknown ASIN");

                            RecTypeName = "Price Change";
                            InitViewBag(ActionType.Create);
                            LoadSelectListsToViewBag();
                            ViewBag.UnknownSKU = true;
                            ViewBag.AmazonAccounts = new SelectList(db.AmazonAccounts.ToList().Select(aa => new { ID = aa.ID.ToString(), Name = aa.Name }), "ID", "Name");

                            return View("Edit", Form);
                        }
                    }
                }

                int ServerDateTimeOffset = db.GetServerDateTimeOffset();

                Startbutton.TimeSpan TheTimeSpan;
                Startbutton.SpanType SpanType = (Startbutton.SpanType)Form.TimeSpanSpanType;

                string Parm = null;

                if (SpanType == Startbutton.SpanType.DaysOfWeek)
                {
                    Parm = "";

                    for (int x = 0; x < 7; x++)
                        if (Request.Form["cb" + ((DayOfWeek)x).ToString()] != null)
                            Parm += ((Startbutton.DayOfWeekAbbr)x);
                }

                TheTimeSpan = new Startbutton.TimeSpan((int)Form.TimeSpanCount, SpanType, Parm);

                DateTime StartDateTime = ((DateTime)Form.TempPriceStartDateTime).Add(ParseTime(Form.TempPriceStartTime));
                DateTime EndDateTime = ((DateTime)Form.TempPriceEndDateTime).Add(ParseTime(Form.TempPriceEndTime));
                DateTime Now = db.GetServerNow(ServerDateTimeOffset);

                string Title;

                AutomaticPriceChange apc = db.AutomaticPriceChanges.Single(apcx => apcx.ID == id);
                apc.SKU = Form.SKU;
                apc.TempPrice = (decimal)Form.TempPrice;
                apc.RegularPrice = (decimal)Form.RegularPrice;
                apc.TempPriceStartDateTime = StartDateTime;
                apc.TempPriceEndDateTime = EndDateTime;
                apc.TimeSpan = TheTimeSpan.ToString();

                if (ais == null)
                {
                    apc.AmazonAccountID = (Guid)Form.AmazonAccountID;
                    apc.ASIN = Form.ASIN;
                    Title = db.KnownASINs.Single(ka => ka.ASIN == Form.ASIN).Title;
                }
                else
                {
                    apc.AmazonAccountID = ais.AmazonAccountID;
                    apc.ASIN = ais.ASIN;
                    Title = ais.KnownASIN.Title;
                }

                string AccountName = db.AmazonAccounts.Single(aa => aa.ID == apc.AmazonAccountID).Name;

                FeedSubmissionScheduleRec fssr = db.FeedSubmissionSchedule.Single(fss => fss.ID == apc.StartFeedSubmissionScheduleID);
                fssr.AmazonAccountID = apc.AmazonAccountID;
                fssr.FeedType = OmnimarkAmazon.Library.FeedType._POST_PRODUCT_PRICING_DATA_.ToString();
                fssr.MessageType = Amazon.XML.AmazonEnvelopeMessageType.Price.ToString();
                fssr.MessageXML = Startbutton.Library.XmlSerialize(new Amazon.XML.Price(Form.SKU, (decimal)Form.TempPrice));
                fssr.NextRun = StartDateTime;
                fssr.TimeSpan = TheTimeSpan.ToString();
                fssr.Name = "Start Price Change for " + Title + " on " + AccountName;
                fssr.Enabled = true;

                fssr = db.FeedSubmissionSchedule.Single(fss => fss.ID == apc.EndFeedSubmissionScheduleID);
                fssr.AmazonAccountID = apc.AmazonAccountID;
                fssr.FeedType = OmnimarkAmazon.Library.FeedType._POST_PRODUCT_PRICING_DATA_.ToString();
                fssr.MessageType = Amazon.XML.AmazonEnvelopeMessageType.Price.ToString();
                fssr.MessageXML = Startbutton.Library.XmlSerialize(new Amazon.XML.Price(Form.SKU, (decimal)Form.RegularPrice));
                fssr.NextRun = EndDateTime;
                fssr.TimeSpan = TheTimeSpan.ToString();
                fssr.Name = "End Price Change for " + Title + " on " + AccountName;
                fssr.Enabled = true;

                db.SaveChanges();

                return RedirectToAction("Index");

            }

        }

    }

}
