using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using OmnimarkAmazon.Models;
using Startbutton.ExtensionMethods;

namespace OmnimarkAmazonWeb.Controllers
{
    public class FeedSubmissionScheduleController : _BaseController
    {
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            RecTypeName = "Scheduled Feed Submission";
        }
        

        public ActionResult Index()
        {
            return View();
        }

        void LoadAmazonAccountsToViewBag()
        {
            ViewBag.AmazonAccounts = new SelectList(db.AmazonAccounts.OrderBy(aa => aa.Name), "ID", "Name");
        }

        void LoadFeedTypesToViewBag()
        {
            ViewBag.FeedTypes = new SelectList(Startbutton.Library.GetEnumValues<OmnimarkAmazon.Library.FeedType>().Select(i => new { ID = i.ToString(), Name = i.ToString() }), "ID", "Name");
        }

        void LoadMessageTypesToViewBag()
        {
            ViewBag.MessageTypes = new SelectList(Startbutton.Library.GetEnumValues<Amazon.XML.AmazonEnvelopeMessageType>().Select(i => new { ID = i.ToString(), Name = i.ToString() }), "ID", "Name");
        }

        void LoadSpanTypesToViewBag()
        {
            ViewBag.SpanTypes = new SelectList(Startbutton.Library.GetEnumValues<Startbutton.SpanType>().Select(i => new { ID = ((int)i).ToString(), Name = i.ToString() + "(s)" }), "ID", "Name");
        }

        void LoadSelectListsToViewBag()
        {
            LoadAmazonAccountsToViewBag();
            LoadFeedTypesToViewBag();
            LoadMessageTypesToViewBag();
            LoadSpanTypesToViewBag();
        }

        public ActionResult Create()
        {
            InitViewBag(ActionType.Create);
            LoadSelectListsToViewBag();
            return View("Edit", new FeedSubmissionScheduleRecWeb());
        }

        public ActionResult Edit(Guid id)
        {
            FeedSubmissionScheduleRecWeb rec = Startbutton.Library.GetEntityFromObject<FeedSubmissionScheduleRecWeb>(db.FeedSubmissionSchedule.Single(fsr => fsr.ID == id));

            Startbutton.TimeSpan ts = new Startbutton.TimeSpan(rec.TimeSpan);
            rec.TimeSpanCount = ts.Count;
            rec.TimeSpanSpanType = (int)ts.SpanType;

            InitViewBag(ActionType.Edit);
            LoadSelectListsToViewBag();
            return View("Edit", rec);
        }

        [HttpPost]
        public ActionResult Edit(Guid id, FeedSubmissionScheduleRecWeb Form)
        {
            if (!ModelState.IsValid)
            {
                InitViewBag(ActionType.Create);
                LoadSelectListsToViewBag();
                return View("Edit", Form);
            }
            else
            {
                SetMessageXML(Form);

                var rec = db.FeedSubmissionSchedule.Single(fsr => fsr.ID == id);
                Form.TimeSpan = Startbutton.TimeSpan.ToString((int)Form.TimeSpanCount, (Startbutton.SpanType)Form.TimeSpanSpanType);
                Form.TimeStamp = rec.TimeStamp;
                Startbutton.Library.SetEntityMatchingFields(rec, Form);
                db.SaveChanges();

                return RedirectToAction("Index");

            }
        }

        [HttpPost]
        public ActionResult Create(FeedSubmissionScheduleRecWeb Form)
        {
            if (!ModelState.IsValid)
            {
                InitViewBag(ActionType.Create);
                LoadSelectListsToViewBag();
                return View("Edit", Form);
            }
            else
            {

                Form.ID = Guid.NewGuid();
                Form.TimeStamp = DateTime.Now;
                Form.TimeSpan = Startbutton.TimeSpan.ToString((int)Form.TimeSpanCount, (Startbutton.SpanType)Form.TimeSpanSpanType);

                SetMessageXML(Form);

                db.FeedSubmissionSchedule.Add(Startbutton.Library.GetEntityFromObject<FeedSubmissionScheduleRec>(Form));

                db.SaveChanges();

                return RedirectToAction("Index");

            }
        }

        void SetMessageXML(FeedSubmissionScheduleRecWeb Rec)
        {
            if (Rec.MessageType == "Price")
            {
                Amazon.XML.Price p = new Amazon.XML.Price(Request.Form["SKU"], decimal.Parse(Request.Form["Price"]));
                Rec.MessageXML = Startbutton.Library.XmlSerialize<Amazon.XML.Price>(p);
            }
        }

        public ActionResult Delete(Guid id)
        {
            return View(db.FeedSubmissionSchedule.Single(fss => fss.ID == id));
        }

        [HttpPost]
        public ActionResult Delete(Guid id, object it)
        {
            OmnimarkAmazon.BLL.FeedSubmissionSchedule.Delete(db, id);

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult GetMessagePartial(Guid id, string MessageType)
        {
            FeedSubmissionScheduleRec fssr = null;
            
            if (id != Guid.Empty)
                fssr = db.FeedSubmissionSchedule.Single(fss => fss.ID == id);

            if (MessageType == "Price")
            {
                Amazon.XML.PriceWeb pw;

                if (id == Guid.Empty)
                    pw = new Amazon.XML.PriceWeb();
                else
                {
                    Amazon.XML.Price p = Startbutton.Library.XmlDeserialize<Amazon.XML.Price>(fssr.MessageXML);
                    pw = new Amazon.XML.PriceWeb();
                    pw.Price = p.StandardPrice.Value;
                    pw.SKU = p.SKU;
                }

                return PartialView("MessageFields/Price", pw);
            }
            else
            {
                if (ViewExists("MessageFields/" + MessageType))
                    return View("MessageFields/" + MessageType);
                else
                    return Content("");
            }
        }

        public ActionResult GetSystemStatus()
        {
            ViewBag.ActiveFeedSubmissionCount = db.FeedSubmissionQueue.Count(OmnimarkAmazon.BLL.ScheduledFeeds.IsActiveListing);

            return View("SystemStatus", "BlankLayout");
        }

        public ActionResult GetScheduleListing()
        {
            var model = db.FeedSubmissionScheduleStatuses.Where(fss => fss.Deleted == null).OrderBy(fss => fss.Name);

            return View("ScheduleListing", "BlankLayout", model);
        }

        public ActionResult GetActiveSubmissions()
        {
            var model = db.FeedSubmissionQueue.Where(OmnimarkAmazon.BLL.ScheduledFeeds.IsActiveListing);

            return View("QueueList", "BlankLayout", model);
        }

        public ActionResult GetLog()
        {
            var model = db.FeedSubmissionQueue.OrderByDescending(fsq => fsq.TimeStamp);

            return View("QueueList", "BlankLayout", model);
        }

    }
}
