using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using OmnimarkAmazon.Models;
using System.Web.Script.Serialization;
using Startbutton.ExtensionMethods;
using OmnimarkAmazon;

namespace OmnimarkAmazonWeb.Controllers
{
    public class ProductInventoryController : _BaseController
    {

        public ActionResult Index()
        {
            DataTable dt = Startbutton.Library.ExecuteSQLToDataTable("Main", "ProductsWithInventory null");

            var Locations = db.ProductInventoryLocations.OrderBy(il => il.Seq).ToList();
            var Stores = db.AmazonAccounts.Where(s=>s.Enabled==true).OrderBy(s => s.DisplaySeq).ToList();

            var r = db.ProductInventoryAdjustmentReasons.ToList();
            
            ViewBag.Locations = Locations;
            ViewBag.Stores = Stores;
            ViewBag.Reasons = new SelectList(r.Where(iar => iar.Types.Contains("M") && !iar.Types.ContainsAny("1|2|3|4|5|6|7|8|9", '|')).OrderBy(s => s.Seq), "ID", "text");
            ViewBag.ReasonsTable = (new JavaScriptSerializer()).Serialize(r.Where(iar => iar.Types.Contains("M") && !iar.Types.ContainsAny("1|2|3|4|5|6|7|8|9", '|')).Select(iar => new { iar.ID, iar.ShortCode, iar.Seq, iar.ReasonRef1Required, iar.ReasonRef1Text, iar.ReasonRef2Required, iar.ReasonRef2Text, iar.ReasonRef3Required, iar.ReasonRef3Text, iar.Text }));

            List<InventoryReportRecord> model = new List<InventoryReportRecord>();

            foreach (DataRow row in dt.Rows)
            {
                InventoryReportRecord ir = new InventoryReportRecord();
                ir.ProductID = (Guid)row["ID"];
                ir.Name = (string)row["Name"];

                decimal Total = 0;

                foreach (var loc in Locations)
                    if (row["Qty" + loc.ShortCode] == DBNull.Value)
                        ir.Qtys.Add(loc.ShortCode, null);
                    else
                    {
                        decimal Qty = (decimal)row["Qty" + loc.ShortCode];
                        ir.Qtys.Add(loc.ShortCode, Qty);
                        Total += Qty;
                    }

                foreach (var s in Stores)
                    if (row["AmazonQty" + s.CharID] == DBNull.Value)
                        ir.Qtys.Add("Amz" + s.CharID, null);
                    else
                    {
                        decimal Qty = (decimal)row["AmazonQty" + s.CharID];
                        ir.Qtys.Add("Amz" + s.CharID, Qty);
                        Total += Qty;
                    }

                ir.Qtys.Add("Total", Total);

                model.Add(ir);

            }

            dt.Dispose();
            dt = null;

            return View(model.OrderBy(m => m.Name));
        }

        public ActionResult ChangeInventory(Guid id, string loccode, Nullable<decimal> oldqty, Nullable<decimal> newqty, Guid ReasonID, string ReasonRef1, string ReasonRef2, string ReasonRef3)
        {
            ProductInventory pi = db.ProductInventories.Where(pix => pix.ProductID == id && pix.ProductInventoryLocation.ShortCode == loccode).FirstOrDefault();

            if (pi == null)
            {
                pi = new ProductInventory();
                pi.ProductID = id;
                pi.LocationID = db.ProductInventoryLocations.Single(il => il.ShortCode == loccode).ID;
                pi.TimeStamp = DateTime.Now;

                db.ProductInventories.Add(pi);
            }
            else
                pi.UpdateTimeStamp = DateTime.Now;

            pi.Qty = newqty;

            ProductInventoryAdjustment pia = new ProductInventoryAdjustment();
            pia.ID = Guid.NewGuid();
            pia.TimeStamp = DateTime.Now;
            pia.ProductID = id;
            pia.LocationID = db.ProductInventoryLocations.Single(il => il.ShortCode == loccode).ID;
            pia.NewQty = newqty;
            pia.OldQty = oldqty;
            pia.AdjustmentAmount = (newqty == null ? 0 : (decimal)newqty) - (oldqty == null ? 0 : (decimal)oldqty);
            pia.ReasonID = ReasonID;
            pia.ReasonRef1 = ReasonRef1;
            pia.ReasonRef2 = ReasonRef2;
            pia.ReasonRef3 = ReasonRef3;

            db.ProductInventoryAdjustments.Add(pia);

            db.SaveChanges();

            return Json("ok", JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetLog(Guid id, string loccode)
        {
            return Json(
                db.ProductInventoryAdjustments
                    .Where(pia => pia.ProductID == id && pia.ProductInventoryLocation.ShortCode == loccode)
                    .OrderByDescending(pia => pia.TimeStamp)
                    .ToList()
                    .Select(pia => new { 
                        Date = pia.TimeStamp.ToString("MM/dd/yyyy hh:mm:ss tt"), 
                        Reason = pia.ProductInventoryAdjustmentReason.Text + 
                            (
                                pia.ProductInventoryAdjustmentReason.ReasonRef1Text != null ||
                                pia.ProductInventoryAdjustmentReason.ReasonRef2Text != null ||
                                pia.ProductInventoryAdjustmentReason.ReasonRef3Text != null ? " (" : ""
                            ) +
                            (pia.ProductInventoryAdjustmentReason.ReasonRef1Text != null ? pia.ProductInventoryAdjustmentReason.ReasonRef1Text + ": " + (pia.ReasonRef1 == null ? "" : pia.ReasonRef1) : "") +
                            (pia.ProductInventoryAdjustmentReason.ReasonRef2Text != null ? " - " + pia.ProductInventoryAdjustmentReason.ReasonRef2Text + ": " + (pia.ReasonRef2 == null ? "" : pia.ReasonRef2) : "") +
                            (pia.ProductInventoryAdjustmentReason.ReasonRef3Text != null ? " - " + pia.ProductInventoryAdjustmentReason.ReasonRef3Text + ": " + (pia.ReasonRef3 == null ? "" : pia.ReasonRef3) : "") +
                            (
                                pia.ProductInventoryAdjustmentReason.ReasonRef1Text != null ||
                                pia.ProductInventoryAdjustmentReason.ReasonRef2Text != null ||
                                pia.ProductInventoryAdjustmentReason.ReasonRef3Text != null ? ")" : ""
                            ),
                        Amount = pia.AdjustmentAmount,
                        Qty = pia.NewQty
                    })
                , JsonRequestBehavior.AllowGet);
        }

        public ActionResult ManageFBAShipments(string update)
        {
            if (update != null)
            {
                string Log = "";

                Library.GetAllFBAShipments(db, (br, txt) => Log += txt + (br ? "<br/>" : ""));

                ViewBag.UpdateLog = Log;
            }

            return View(db.InboundFBAShipmentManagementViews.OrderByDescending(ifs => ifs.OrderBy).ThenByDescending(ifs => ifs.TimeStamp));
        }
        public ActionResult ProcessFBAShipmentNow(string id)
        {

            string log = "";

            Action<bool, string> Log = (b, s) =>
            {
                log += s;
                if (b) log += "\n";
            };

            var ifs = db.InboundFBAShipments.Single(ifsx => ifsx.ID == id);
            var shp = Library.GetInboundShipments(new Library.Throttler[] { new Library.Throttler(2000) }.ToList(), ifs.AmazonAccount, null, null, new string[] { id }.ToList(), Log);
            Library.DoSyncInboundFBAShipments(db, ifs.AmazonAccount, new Library.Throttler(2000), shp, true, Log);

            ifs.ScheduledForInventoryAdjustmentProcessing = null;
            db.SaveChanges();

            return Json(new
            {
                Success = true,
                LogText = log,
                Html = Startbutton.Web.Library.RenderPartialViewToString(ControllerContext, ViewData, TempData, "InboundFBAShipmentManagementViewRow", db.InboundFBAShipmentManagementViews.Single(x => x.ID == id))
            }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ScheduleFBAShipment(string id)
        {

            var ifs = db.InboundFBAShipments.Single(ifsx => ifsx.ID == id);
            ifs.ScheduledForInventoryAdjustmentProcessing = DateTime.Now;
            db.SaveChanges();

            return Json(new
            {
                Success = true,
                Html = Startbutton.Web.Library.RenderPartialViewToString(ControllerContext, ViewData, TempData, "InboundFBAShipmentManagementViewRow", db.InboundFBAShipmentManagementViews.Single(x => x.ID == id))
            }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult RemoveFBAShipment(string id)
        {

            db.Database.ExecuteSqlCommand("UndoFBAShipmentInventoryAdjustments " + id);

            var ifs = db.InboundFBAShipments.Single(ifsx => ifsx.ID == id);
            ifs.RemovedFromInventoryAdjustmentProcessing = DateTime.Now;
            db.SaveChanges();

            return Json(new
            {
                Success = true,
                Html = Startbutton.Web.Library.RenderPartialViewToString(ControllerContext, ViewData, TempData, "InboundFBAShipmentManagementViewRow", db.InboundFBAShipmentManagementViews.Single(x => x.ID == id))
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
