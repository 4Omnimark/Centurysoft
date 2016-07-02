﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using OmnimarkAmazon.Models;
using Startbutton.ConfigSections;
using System.Configuration;
using System.Data;
using OmnimarkAmazonWeb.Models;
using Startbutton.ExtensionMethods;
using UkListing;
using System.Data.OleDb;
using System.Runtime.Remoting.Contexts;
using System.Text.RegularExpressions;
using System.Data.Objects;
using System.Data.Entity.Infrastructure;
using System.ServiceProcess;
using System.Management;
using System.Threading.Tasks;
using System.Transactions;
using ReviewProduct;




namespace OmnimarkAmazonWeb.Controllers
{

    public class ItemManagementController : _BaseController
    {
        UKOmnimarkEntities ukdb = new UKOmnimarkEntities();

        string Quantity = ConfigurationManager.AppSettings["ProductQty"];
        string FulfillmentLatency = ConfigurationManager.AppSettings["FulfillmentLatency"];
        public static int fcountSport = 1, fcountToys = 1, fcountBeauty = 1, fcountAllExport = 1, fcountBaby = 1, fcountWatches = 1, fcountJewelry = 1, canadaSport = 1, canadatoys = 1, canadabeauty = 1, fcountHomeandKitchen = 1;
        public ActionResult Index()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Main"].ToString());
            con.Open();

            if (Request.HttpMethod == "POST")
            {
                string str = Request.Form["txt"].ToString();
                SqlCommand cmd = new SqlCommand("Update CSVComment set Description='" + str.Trim() + "' where ID=1", con);
                cmd.ExecuteNonQuery();
                return Redirect("/ItemManagement/index");
            }
            else
            {

                SqlCommand cmd = new SqlCommand("Select Description from CSVComment where ID=1", con);
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    ViewBag.comment = sdr["Description"].ToString();
                }

                sdr.Close();
                return View();
            }
        }
        public ActionResult AddShipment()
        {
            ViewBag.AmazonAccounts = db.AmazonAccounts.OrderBy(ao => ao.Name);
            return View();
        }
        [HttpPost]
        public ActionResult AddShipment(ShippedtoAmazon SA, AmazonAccount aa)
        {
            string AccountID = Request.Form["ShipmentAccount"].ToString();
            Guid temp = new Guid(AccountID);
            ShippedtoAmazon STA = new ShippedtoAmazon();
            STA.ID = Guid.NewGuid();
            STA.ShipmentID = SA.ShipmentID.Trim();
            STA.AccountID = AccountID;
            STA.Status = "true";
            STA.AccountName = db.AmazonAccounts.Single(s => s.ID == temp).Name;

            STA.TimeStamp = SA.TimeStamp;
            db.ShippedtoAmazons.Add(STA);
            db.SaveChanges();

            TempData["Success"] = "ShipmentID Added Successfully";

            return RedirectToAction("index", "home");
        }
        public ActionResult DeductInward(Nullable<Guid> ProductID)
        {
            if (ProductID != null)
            {
                ViewBag.ProductID = ProductID;
                ViewBag.ProductName = db.Products.Single(p => p.ID == (Guid)ProductID).Name;
            }
            ViewBag.Reasons = new SelectList(db.ProductInventoryAdjustmentReasons.Where(iar => iar.Types.Contains("D")).OrderBy(s => s.Seq), "ID", "Text");
            return View();
        }
        [HttpPost]
        public ActionResult DeductInward(DeductInward di, Guid ProductID)
        {

            if (ModelState.IsValid)
            {
                Guid id = OmnimarkAmazon.Library.AdjustInventory(db, ProductID, OmnimarkAmazon.Library.OrlandoLocationID, -di.Qty, di.ReasonID, di.OrderID, di.CustomerName, di.SalesVenue, null);
                db.SaveChanges();

                if (di.TimeStamp != DateTime.Now.Date)
                {
                    ProductInventoryAdjustment pia = db.ProductInventoryAdjustments.Where(x => x.ID == id).FirstOrDefault();

                    pia.TimeStamp = di.TimeStamp;

                    db.SaveChanges();
                }
                return RedirectToAction("index", "home");
            }
            else
            {
                ViewBag.Reasons = new SelectList(db.ProductInventoryAdjustmentReasons.Where(iar => iar.Types.Contains("D")).OrderBy(s => s.Seq), "ID", "Text");
                return View(di);
            }
        }
        public ActionResult DeductFBAShipment()
        {
            ViewBag.Shipments = db.ShippedtoAmazons.Where(x => x.Status == "true");
            return View();
        }
        [HttpGet]
        public ActionResult Result(string shipid)
        {
            ViewBag.ShipmentID = shipid;
            List<MoveInventoryFromInboundFBAShipmentRec> item = db.Database.SqlQuery<MoveInventoryFromInboundFBAShipmentRec>(@"
                select AmazonAccountName,UnitQty,QuantityShipped, ShipmentID,SKU,ASIN, ProductID, ProductName, ShouldBe as AdjustmentAmount  from InboundFBAProductInventoryAdjustmentAudit
                where ShipmentID='" + shipid + "'order by SKU asc").ToList();

            //            ViewBag.newtemp = db.Database.SqlQuery<MoveInventoryFromInboundFBAShipmentRec>(@"
            //                select count(distinct SKU) as SKUQty ,SUM(QuantityShipped) as TotalShip from InboundFBAProductInventoryAdjustmentAudit
            //                where ShipmentID='" + shipid + "'");
            ViewBag.newtemp = db.Database.SqlQuery<MoveInventoryFromInboundFBAShipmentRec>(@"
                   select SUM(QuantityShipped) as TotalShip,count(SKU) as SKUQty from  (select SKU, QuantityShipped from InboundFBAProductInventoryAdjustmentAudit  where ShipmentID='" + shipid + "' group by SKU,QuantityShipped) as tb");

            ViewBag.AccountName = db.Database.SqlQuery<MoveInventoryFromInboundFBAShipmentRec>(@"
                select AmazonAccountName from InboundFBAProductInventoryAdjustmentAudit
                where ShipmentID='" + shipid + "'");

            ViewBag.ManageShip = db.InboundFBAShipments.Where(s => s.ID == shipid);

            return View(item);


        }
        [HttpPost]
        public ActionResult Result(IEnumerable<MoveInventoryFromInboundFBAShipmentRec> rec)
        {
            ProductInventoryAdjustmentBatch piab = OmnimarkAmazon.Library.CreateProductInventoryAdjustmentBatch(db, "FBA");
            foreach (var itm in rec)
            {
                Guid id = OmnimarkAmazon.Library.AdjustInventory(db, itm.ProductID, OmnimarkAmazon.Library.OrlandoLocationID, -itm.AdjustmentAmount, OmnimarkAmazon.Library.AmazonFBAOutboundReasonID, itm.ShipmentID, itm.AmazonAccountName, null, piab.ID);
                ShippedtoAmazon sa = db.ShippedtoAmazons.Single(x => x.ShipmentID == itm.ShipmentID);
                sa.Status = "false";
                db.SaveChanges();
                TempData["Success"] = "Inventory has been deducted successfully";

            }
            return RedirectToAction("DeductFBAShipment", "ItemManagement");

        }
        public ActionResult Delete(string id, string sku)
        {
            if (Request.HttpMethod == "POST")
            {
                db.InboundFBAShipmentItems.Remove(db.InboundFBAShipmentItems.Single(p => p.SKU == sku && p.ShipmentID == id));
                db.SaveChanges();

                return Redirect("/ItemManagement/Result?shipid=" + id);
            }
            else
            {
                MoveInventoryFromInboundFBAShipmentRec item = db.Database.SqlQuery<MoveInventoryFromInboundFBAShipmentRec>(@"
                select AmazonAccountName,UnitQty,QuantityShipped, ShipmentID,SKU,ASIN, ProductID, ProductName, ShouldBe as AdjustmentAmount  from InboundFBAProductInventoryAdjustmentAudit
                where SKU='" + sku + "' and ShipmentID='" + id + "'").Single();
                return View(item);
            }
        }
        public ActionResult DisplayItem()
        {
            return View();
        }
        public ActionResult AddMissingShipment(string shipid)
        {
            ViewBag.shipid = shipid;

            return View();
        }
        [HttpPost]
        public ActionResult AddMissingShipment(InboundFBAShipmentItem ifsi, string shipid)
        {

            ifsi.SKU = Request.Form["SKU"].ToString();
            ifsi.TimeStamp = DateTime.Now;
            ifsi.ShipmentID = Convert.ToString(shipid);
            db.InboundFBAShipmentItems.Add(ifsi);
            db.SaveChanges();
            TempData["msg"] = "SKU Saved Successfully";
            return Redirect("/ItemManagement/Result?shipid=" + shipid);
        }
        public ActionResult FBAShipment()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FBAShipment(InboundFBAShipment ifs)
        {
            InboundFBAShipmentItem ifsi = new InboundFBAShipmentItem();

            var item = db.InboundFBAShipmentItems.Where(x => x.ShipmentID == ifs.ID);
            return PartialView("InboundShipmentItem", item);
        }
        public ActionResult ExportByAccount()
        {
            ViewBag.SellingVenue = ukdb.SellingPlaces.OrderBy(x => x.SellingPlace1);

            return View();
        }
        [HttpPost]
        public ActionResult ExportByAccount(ExportByAccount ea)
        {
            ((IObjectContextAdapter)ukdb).ObjectContext.CommandTimeout = 1800;
            if (ModelState.IsValid)
            {

                if (ea.marketplace == ConstantData.ED)
                {
                    if (ea.Cat == ConstantData.Toys)
                    {
                        if (ea.chkresult == true)
                        {
                            UkProhibitionTbl("tbl_Toys");
                        }
                        DateTime dt = DateTime.Now.AddHours(-24.00);
                        IEnumerable<tbl_Toys> data = ukdb.tbl_Toys.Where(x => x.Account1_AmazonActive == 0 && x.Account2_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.TimeStamp < dt && x.Account1_Status != 1 && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                        ExportToys(ea.PriceValue, ea.marketplace, data);
                    }
                    else
                    {
                        if (ea.Cat == ConstantData.Beauty)
                        {
                            if (ea.chkresult == true)
                            {
                                UkProhibitionTbl("tbl_Beauty");
                            }
                            DateTime dt = DateTime.Now.AddHours(-24.00);
                            IEnumerable<tbl_Beauty> data = ukdb.tbl_Beauty.Where(x => x.Account1_AmazonActive == 0 && x.Account2_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.TimeStamp < dt && x.Account1_Status != 1 && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                            ExportBeauty(ea.PriceValue, ea.marketplace, data);
                        }
                        else
                            if (ea.Cat == ConstantData.SportingGoods)
                            {
                                if (ea.chkresult == true)
                                {
                                    UkProhibitionTbl("tbl_Sports");
                                }
                                DateTime dt = DateTime.Now.AddHours(-24.00);
                                IEnumerable<tbl_Sports> data = ukdb.tbl_Sports.Where(x => x.Account1_AmazonActive == 0 && x.Account2_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.TimeStamp < dt && x.Account1_Status != 1 && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                                ExportForUK(ea.PriceValue, ea.marketplace, data);
                            }
                            else
                                if (ea.Cat == ConstantData.Baby)
                                {
                                    if (ea.chkresult == true)
                                    {
                                        UkProhibitionTbl("tbl_Baby");
                                    }
                                    DateTime dt = DateTime.Now.AddHours(-24.00);
                                    IEnumerable<tbl_Baby> data = ukdb.tbl_Baby.Where(x => x.Account1_AmazonActive == 0 && x.Account2_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.TimeStamp < dt && x.Account1_Status != 1 && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                                    ExportBaby(ea.PriceValue, ea.marketplace, data);
                                }
                                else
                                    if (ea.Cat == ConstantData.Watches)
                                    {
                                        if (ea.chkresult == true)
                                        {
                                            UkProhibitionTbl("tbl_Watches");
                                        }
                                        DateTime dt = DateTime.Now.AddHours(-24.00);
                                        IEnumerable<tbl_Watches> data = ukdb.tbl_Watches.Where(x => x.Account1_AmazonActive == 0 && x.Account2_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.TimeStamp < dt && x.Account1_Status != 1 && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                                        ExportWatches(ea.PriceValue, ea.marketplace, data);
                                    }
                                    else
                                        if (ea.Cat == ConstantData.Jewelry)
                                        {
                                            if (ea.chkresult == true)
                                            {
                                                UkProhibitionTbl("tbl_Jewelry");
                                            }
                                            DateTime dt = DateTime.Now.AddHours(-24.00);
                                            IEnumerable<tbl_Jewellery> data = ukdb.tbl_Jewellery.Where(x => x.Account1_AmazonActive == 0 && x.Account2_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.TimeStamp < dt && x.Account1_Status != 1 && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                                            ExportJewelry(ea.PriceValue, ea.marketplace, data);
                                        }
                                        else if (ea.Cat == ConstantData.HomeandKitchen)
                                        {
                                            if (ea.chkresult == true)
                                            {
                                                UkProhibitionTbl("tbl_HomeandKitchen");
                                            }
                                            DateTime dt = DateTime.Now.AddHours(-24.00);
                                            IEnumerable<tbl_HomeandKitchen> data = ukdb.tbl_HomeandKitchen.Where(x => x.Account1_AmazonActive == 0 && x.Account2_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.TimeStamp < dt && x.Account1_Status != 1 && x.Prime == 1 && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                                            ExportHomeAndKitchen(ea.PriceValue, ea.marketplace, data);
                                        }
                    }

                }
                else
                {
                    if (ea.marketplace == ConstantData.EM)
                    {
                        if (ea.Cat == ConstantData.SportingGoods)
                        {
                            if (ea.chkresult == true)
                            {
                                UkProhibitionTbl("tbl_Sports");
                            }
                            DateTime dt = DateTime.Now.AddHours(-24.00);
                            IEnumerable<tbl_Sports> data = ukdb.tbl_Sports.Where(x => x.Account1_AmazonActive == 0 && x.Account2_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.Account2_Status != 1 && x.TimeStamp < dt && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                            ExportForUK(ea.PriceValue, ea.marketplace, data);
                        }
                        else
                        {
                            if (ea.Cat == ConstantData.Beauty)
                            {
                                if (ea.chkresult == true)
                                {
                                    UkProhibitionTbl("tbl_Beauty");
                                }
                                DateTime dt = DateTime.Now.AddHours(-24.00);
                                IEnumerable<tbl_Beauty> data = ukdb.tbl_Beauty.Where(x => x.Account1_AmazonActive == 0 && x.Account2_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.Account2_Status != 1 && x.TimeStamp < dt && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                                ExportBeauty(ea.PriceValue, ea.marketplace, data);
                            }
                            else
                                if (ea.Cat == ConstantData.Toys)
                                {
                                    if (ea.chkresult == true)
                                    {
                                        UkProhibitionTbl("tbl_Toys");
                                    }
                                    DateTime dt = DateTime.Now.AddHours(-24.00);
                                    IEnumerable<tbl_Toys> data = ukdb.tbl_Toys.Where(x => x.Account1_AmazonActive == 0 && x.Account2_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.Account2_Status != 1 && x.TimeStamp < dt && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                                    ExportToys(ea.PriceValue, ea.marketplace, data);
                                }
                                else
                                    if (ea.Cat == ConstantData.Baby)
                                    {
                                        if (ea.chkresult == true)
                                        {
                                            UkProhibitionTbl("tbl_Baby");
                                        }
                                        DateTime dt = DateTime.Now.AddHours(-24.00);
                                        IEnumerable<tbl_Baby> data = ukdb.tbl_Baby.Where(x => x.Account1_AmazonActive == 0 && x.Account2_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.Account2_Status != 1 && x.TimeStamp < dt && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                                        ExportBaby(ea.PriceValue, ea.marketplace, data);
                                    }
                                    else
                                        if (ea.Cat == ConstantData.Watches)
                                        {
                                            if (ea.chkresult == true)
                                            {
                                                UkProhibitionTbl("tbl_Watches");
                                            }
                                            DateTime dt = DateTime.Now.AddHours(-24.00);
                                            IEnumerable<tbl_Watches> data = ukdb.tbl_Watches.Where(x => x.Account1_AmazonActive == 0 && x.Account2_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.Account2_Status != 1 && x.TimeStamp < dt && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                                            ExportWatches(ea.PriceValue, ea.marketplace, data);
                                        }
                                        else
                                            if (ea.Cat == ConstantData.Jewelry)
                                            {
                                                if (ea.chkresult == true)
                                                {
                                                    UkProhibitionTbl("tbl_Jewelry");
                                                }
                                                DateTime dt = DateTime.Now.AddHours(-24.00);
                                                IEnumerable<tbl_Jewellery> data = ukdb.tbl_Jewellery.Where(x => x.Account1_AmazonActive == 0 && x.Account2_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.Account2_Status != 1 && x.TimeStamp < dt && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                                                ExportJewelry(ea.PriceValue, ea.marketplace, data);
                                            }
                                            else if (ea.Cat == ConstantData.HomeandKitchen)
                                            {
                                                if (ea.chkresult == true)
                                                {
                                                    UkProhibitionTbl("tbl_HomeandKitchen");
                                                }
                                                DateTime dt = DateTime.Now.AddHours(-24.00);
                                                IEnumerable<tbl_HomeandKitchen> data = ukdb.tbl_HomeandKitchen.Where(x => x.Account1_AmazonActive == 0 && x.Account2_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.TimeStamp < dt && x.Account2_Status != 1 && x.Prime == 1 && x.Reviewed == 1 && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                                                ExportHomeAndKitchen(ea.PriceValue, ea.marketplace, data);
                                            }
                        }

                    }
                    else
                        if (ea.marketplace == ConstantData.DC)
                        {
                            if (ea.Cat == ConstantData.SportingGoods)
                            {
                                if (ea.canadachk == true)
                                {
                                    CanadaProhibitionTbl("tbl_Sports");
                                }
                                DateTime dt = DateTime.Now.AddHours(-24.00);
                                IEnumerable<tbl_Sports> data = ukdb.tbl_Sports.Where(x => x.Reviewed == 1 && x.TimeStamp < dt && x.Account3_Status != 1 && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.CanadaProhibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                                ExportSportsCanada(ea.PriceValue, ea.marketplace, data);
                            }
                            else
                            {
                                if (ea.Cat == ConstantData.Beauty)
                                {
                                    if (ea.canadachk == true)
                                    {
                                        CanadaProhibitionTbl("tbl_Beauty");
                                    }
                                    DateTime dt = DateTime.Now.AddHours(-24.00);
                                    IEnumerable<tbl_Beauty> data = ukdb.tbl_Beauty.Where(x => x.Reviewed == 1 && x.TimeStamp < dt && x.Account3_Status != 1 && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.CanadaProhibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                                    ExportBeautyCanada(ea.PriceValue, ea.marketplace, data);
                                }
                                else
                                    if (ea.Cat == ConstantData.Toys)
                                    {
                                        if (ea.canadachk == true)
                                        {
                                            CanadaProhibitionTbl("tbl_Toys");
                                        }
                                        DateTime dt = DateTime.Now.AddHours(-24.00);
                                        IEnumerable<tbl_Toys> data = ukdb.tbl_Toys.Where(x => x.Reviewed == 1 && x.TimeStamp < dt && x.Account3_Status != 1 && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.CanadaProhibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                                        ExportToysCanada(ea.PriceValue, ea.marketplace, data);
                                    }
                                    else
                                        if (ea.Cat == ConstantData.Baby)
                                        {
                                            if (ea.canadachk == true)
                                            {
                                                CanadaProhibitionTbl("tbl_Baby");
                                            }
                                            DateTime dt = DateTime.Now.AddHours(-24.00);
                                            IEnumerable<tbl_Baby> data = ukdb.tbl_Baby.Where(x => x.Reviewed == 1 && x.TimeStamp < dt && x.Account3_Status != 1 && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.CanadaProhibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                                            ExportBabyCanada(ea.PriceValue, ea.marketplace, data);
                                        }
                                        else
                                            if (ea.Cat == ConstantData.Watches)
                                            {
                                                if (ea.canadachk == true)
                                                {
                                                    CanadaProhibitionTbl("tbl_Watches");
                                                }
                                                DateTime dt = DateTime.Now.AddHours(-24.00);
                                                IEnumerable<tbl_Watches> data = ukdb.tbl_Watches.Where(x => x.Reviewed == 1 && x.TimeStamp < dt && x.Account3_Status != 1 && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.CanadaProhibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                                                ExportWatchesCanada(ea.PriceValue, ea.marketplace, data);
                                            }
                                            else
                                                if (ea.Cat == ConstantData.Jewelry)
                                                {
                                                    if (ea.canadachk == true)
                                                    {
                                                        CanadaProhibitionTbl("tbl_Jewelry");
                                                    }
                                                    DateTime dt = DateTime.Now.AddHours(-24.00);
                                                    IEnumerable<tbl_Jewellery> data = ukdb.tbl_Jewellery.Where(x => x.Reviewed == 1 && x.TimeStamp < dt && x.Account3_Status != 1 && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.CanadaProhibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                                                    ExportJewelryCanada(ea.PriceValue, ea.marketplace, data);
                                                }
                                                else if (ea.Cat == ConstantData.HomeandKitchen)
                                                {
                                                    if (ea.chkresult == true)
                                                    {
                                                        CanadaProhibitionTbl("tbl_HomeandKitchen");
                                                    }
                                                    DateTime dt = DateTime.Now.AddHours(-24.00);
                                                    IEnumerable<tbl_HomeandKitchen> data = ukdb.tbl_HomeandKitchen.Where(x => x.TimeStamp < dt && x.Account3_Status != 1 && x.Prime == 1 && x.WeightUnits < 501 && x.CanadaProhibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                                                    ExportHomeAndKitchenCanada(ea.PriceValue, ea.marketplace, data);
                                                }
                            }

                        }
                        else
                            if (ea.marketplace == ConstantData.DI)
                            {
                                if (ea.Cat == ConstantData.SportingGoods)
                                {
                                    if (ea.chkresult == true)
                                    {
                                        UkProhibitionTbl("tbl_Sports");
                                    }
                                    DateTime dt = DateTime.Now.AddHours(-24.00);
                                    IEnumerable<tbl_Sports> data = ukdb.tbl_Sports.Where(x => x.Account1_AmazonActive == 0 && x.Account2_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.Account4_Status != 1 && x.TimeStamp < dt && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();

                                    ExportForUK(ea.PriceValue, ea.marketplace, data);
                                }
                                else
                                {
                                    if (ea.Cat == ConstantData.Beauty)
                                    {
                                        if (ea.chkresult == true)
                                        {
                                            UkProhibitionTbl("tbl_Beauty");
                                        }
                                        DateTime dt = DateTime.Now.AddHours(-24.00);
                                        IEnumerable<tbl_Beauty> data = ukdb.tbl_Beauty.Where(x => x.Account1_AmazonActive == 0 && x.Account2_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.Account4_Status != 1 && x.TimeStamp < dt && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                                        ExportBeauty(ea.PriceValue, ea.marketplace, data);
                                    }
                                    else
                                        if (ea.Cat == ConstantData.Toys)
                                        {
                                            if (ea.chkresult == true)
                                            {
                                                UkProhibitionTbl("tbl_Toys");
                                            }
                                            DateTime dt = DateTime.Now.AddHours(-24.00);
                                            IEnumerable<tbl_Toys> data = ukdb.tbl_Toys.Where(x => x.Account1_AmazonActive == 0 && x.Account2_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.Account4_Status != 1 && x.TimeStamp < dt && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                                            ExportToys(ea.PriceValue, ea.marketplace, data);
                                        }
                                        else
                                            if (ea.Cat == ConstantData.Baby)
                                            {
                                                if (ea.chkresult == true)
                                                {
                                                    UkProhibitionTbl("tbl_Baby");
                                                }
                                                DateTime dt = DateTime.Now.AddHours(-24.00);
                                                IEnumerable<tbl_Baby> data = ukdb.tbl_Baby.Where(x => x.Account1_AmazonActive == 0 && x.Account2_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.Account4_Status != 1 && x.TimeStamp < dt && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                                                ExportBaby(ea.PriceValue, ea.marketplace, data);
                                            }
                                            else
                                                if (ea.Cat == ConstantData.Watches)
                                                {
                                                    if (ea.chkresult == true)
                                                    {
                                                        UkProhibitionTbl("tbl_Watches");
                                                    }
                                                    DateTime dt = DateTime.Now.AddHours(-24.00);
                                                    IEnumerable<tbl_Watches> data = ukdb.tbl_Watches.Where(x => x.Account1_AmazonActive == 0 && x.Account2_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.Account4_Status != 1 && x.TimeStamp < dt && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                                                    ExportWatches(ea.PriceValue, ea.marketplace, data);
                                                }
                                                else
                                                    if (ea.Cat == ConstantData.Jewelry)
                                                    {
                                                        if (ea.chkresult == true)
                                                        {
                                                            UkProhibitionTbl("tbl_Jewelry");
                                                        }
                                                        DateTime dt = DateTime.Now.AddHours(-24.00);
                                                        IEnumerable<tbl_Jewellery> data = ukdb.tbl_Jewellery.Where(x => x.Account1_AmazonActive == 0 && x.Account2_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.Account4_Status != 1 && x.TimeStamp < dt && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                                                        ExportJewelry(ea.PriceValue, ea.marketplace, data);
                                                    }
                                                    else if (ea.Cat == ConstantData.HomeandKitchen)
                                                    {
                                                        if (ea.chkresult == true)
                                                        {
                                                            UkProhibitionTbl("tbl_HomeandKitchen");
                                                        }
                                                        DateTime dt = DateTime.Now.AddHours(-24.00);
                                                        IEnumerable<tbl_HomeandKitchen> data = ukdb.tbl_HomeandKitchen.Where(x => x.Account1_AmazonActive == 0 && x.Account2_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.TimeStamp < dt && x.Account4_Status != 1 && x.Prime == 1 && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                                                        ExportHomeAndKitchen(ea.PriceValue, ea.marketplace, data);
                                                    }
                                }

                            }


                }
            }

            ViewBag.SellingVenue = ukdb.SellingPlaces.OrderBy(x => x.SellingPlace1);
            ModelState.Clear();
            return View();

        }


        public JsonResult Account(string acc)
        {

            var acountname = ukdb.tbl_Account.Where(x => x.SellingVenue == acc).ToList();
            return Json(acountname, JsonRequestBehavior.AllowGet);

        }
        public JsonResult Category(string sc)
        {
            var data = ukdb.tbl_Category.Where(x => x.Account_Name == sc && x.Enabled == 1).Select(x => x.CategoryName);
            return Json(data.ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult UKExport()
        {
            ViewBag.uklist = new SelectList(ukdb.tbl_Category.Select(x => x.CategoryName));

            return View();
        }
        [HttpPost]

        public ActionResult ExportStatus()
        {
            ViewBag.category = new SelectList(ukdb.tbl_Category.Select(x => x.CategoryName));
            return View();
        }
        [HttpPost]
        public ActionResult ExportStatus(ExportStatusModel objesm)
        {
            if (objesm.Category == "SportingGoods")
            {
                try
                {
                    var fname = ukdb.tbl_Sports.Where(x => x.FileName == objesm.FileName).ToList();
                    foreach (var nm in fname)
                    {
                        nm.Status = 0;
                        ukdb.SaveChanges();
                        TempData["message"] = "The Status updated Successfully.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["message"] = "File Name is Incorrect or Not present in System.";
                }
            }
            else
                if (objesm.Category == "Toys")
                {
                    try
                    {

                        var fname = ukdb.tbl_Toys.Where(x => x.FileName == objesm.FileName).ToList();
                        foreach (var nm in fname)
                        {
                            nm.Status = 0;
                            ukdb.SaveChanges();
                            TempData["message"] = "The Status updated Successfully.";
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["message"] = "File Name is Incorrect or Not present in System.";
                    }

                }
                else
                {
                    TempData["message"] = "File Name Incorrect or Not present in System.";
                }
            ViewBag.category = new SelectList(ukdb.tbl_Category.Select(x => x.CategoryName));
            return View();
        }
        public ActionResult AddProhibitedKey()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddProhibitedKey(UkProhibitedKeywords up, string submit)
        {

            if (ModelState.IsValid)
            {
                UKOmnimarkEntities dbcontext = new UKOmnimarkEntities();
                if (submit == "ADD To UK")
                {
                    tbl_Prohibited_Keywords tpk = new tbl_Prohibited_Keywords();
                    tpk.ProhibitedKeys = up.keyname;
                    tpk.TimeStamp = DateTime.Now;
                    dbcontext.tbl_Prohibited_Keywords.Add(tpk);
                    dbcontext.SaveChanges();
                }
                else
                    if (submit == "ADD To Canada")
                    {
                        Canada_Prohibited_Keywords cpk = new Canada_Prohibited_Keywords();
                        cpk.Keywords = up.keyname;
                        cpk.TimeStamp = DateTime.Now;
                        dbcontext.Canada_Prohibited_Keywords.Add(cpk);
                        dbcontext.SaveChanges();
                    }

                TempData["msg"] = "Data Saved Successfully.";

            }
            return View();
        }

        //Not Prime Export

        public ActionResult ExportByAccountNotPrime()
        {
            ViewBag.SellingVenue = ukdb.SellingPlaces.OrderBy(x => x.SellingPlace1);

            return View();
        }

        [HttpPost]
        public ActionResult ExportByAccountNotPrime(ExportByAccount ea)
        {
            ((IObjectContextAdapter)ukdb).ObjectContext.CommandTimeout = 1800;
            if (ModelState.IsValid)
            {

                if (ea.marketplace == ConstantData.ED)
                {
                    if (ea.Cat == ConstantData.Toys)
                    {
                        if (ea.chkresult == true)
                        {
                            UkProhibitionTbl("tbl_ToysNotPrime");
                        }
                        DateTime dt = DateTime.Now.AddHours(-24.00);
                        IEnumerable<tbl_ToysNotPrime> data = ukdb.tbl_ToysNotPrime.Where(x => x.Account2_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.Account1_Status != 1 && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                        ExportToysNotPrime(ea.PriceValue, ea.marketplace, data);
                    }
                    else if (ea.Cat == ConstantData.Beauty)
                    {
                        if (ea.chkresult == true)
                        {
                            UkProhibitionTbl("tbl_BeautyNotPrime");
                        }
                        DateTime dt = DateTime.Now.AddHours(-24.00);
                        IEnumerable<tbl_BeautyNotPrime> data = ukdb.tbl_BeautyNotPrime.Where(x => x.Account2_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.Account1_Status != 1 && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                        ExportBeautyNotPrime(ea.PriceValue, ea.marketplace, data);
                    }
                    else if (ea.Cat == ConstantData.SportingGoods)
                    {
                        if (ea.chkresult == true)
                        {
                            UkProhibitionTbl("tbl_SportsNotPrime");
                        }
                        DateTime dt = DateTime.Now.AddHours(-24.00);
                        IEnumerable<tbl_SportsNotPrime> data = ukdb.tbl_SportsNotPrime.Where(x => x.Account2_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.Account1_Status != 1 && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                        ExportForUKNotPrime(ea.PriceValue, ea.marketplace, data);
                    }
                    else if (ea.Cat == ConstantData.Baby)
                    {
                        if (ea.chkresult == true)
                        {
                            UkProhibitionTbl("tbl_BabyNotPrime");
                        }
                        DateTime dt = DateTime.Now.AddHours(-24.00);
                        IEnumerable<tbl_BabyNotPrime> data = ukdb.tbl_BabyNotPrime.Where(x => x.Account2_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.Account1_Status != 1 && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                        ExportBabyNotPrime(ea.PriceValue, ea.marketplace, data);
                    }
                    else if (ea.Cat == ConstantData.Watches)
                    {
                        if (ea.chkresult == true)
                        {
                            UkProhibitionTbl("tbl_WatchesNotPrime");
                        }
                        DateTime dt = DateTime.Now.AddHours(-24.00);
                        IEnumerable<tbl_WatchesNotPrime> data = ukdb.tbl_WatchesNotPrime.Where(x => x.Account2_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.Account1_Status != 1 && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                        ExportWatchesNotPrime(ea.PriceValue, ea.marketplace, data);
                    }
                    else if (ea.Cat == ConstantData.Jewelry)
                    {
                        if (ea.chkresult == true)
                        {
                            UkProhibitionTbl("tbl_JewelleryNotPrime");
                        }
                        DateTime dt = DateTime.Now.AddHours(-24.00);
                        IEnumerable<tbl_JewelleryNotPrime> data = ukdb.tbl_JewelleryNotPrime.Where(x => x.Account2_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.Account1_Status != 1 && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                        ExportJewelryNotPrime(ea.PriceValue, ea.marketplace, data);
                    }
                    else if (ea.Cat == ConstantData.HomeandKitchen)
                    {
                        if (ea.chkresult == true)
                        {
                            UkProhibitionTbl("tbl_HomeAndKitchenNotPrime");
                        }
                        DateTime dt = DateTime.Now.AddHours(-24.00);
                        IEnumerable<tbl_HomeAndKitchenNotPrime> data = ukdb.tbl_HomeAndKitchenNotPrime.Where(x => x.Account2_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.Account1_Status != 1 && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                        ExportHomeAndKitchenNotPrime(ea.PriceValue, ea.marketplace, data);
                    }


                }
                else
                {
                    if (ea.marketplace == ConstantData.EM)
                    {
                        if (ea.Cat == ConstantData.SportingGoods)
                        {
                            if (ea.chkresult == true)
                            {
                                UkProhibitionTbl("tbl_SportsNotPrime");
                            }
                            DateTime dt = DateTime.Now.AddHours(-24.00);
                            IEnumerable<tbl_SportsNotPrime> data = ukdb.tbl_SportsNotPrime.Where(x => x.Account1_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.Account2_Status != 1 && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                            ExportForUKNotPrime(ea.PriceValue, ea.marketplace, data);
                        }
                        else if (ea.Cat == ConstantData.Beauty)
                        {
                            if (ea.chkresult == true)
                            {
                                UkProhibitionTbl("tbl_BeautyNotPrime");
                            }
                            DateTime dt = DateTime.Now.AddHours(-24.00);
                            IEnumerable<tbl_BeautyNotPrime> data = ukdb.tbl_BeautyNotPrime.Where(x => x.Account1_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.Account2_Status != 1 && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                            ExportBeautyNotPrime(ea.PriceValue, ea.marketplace, data);
                        }
                        else if (ea.Cat == ConstantData.Toys)
                        {
                            if (ea.chkresult == true)
                            {
                                UkProhibitionTbl("tbl_ToysNotPrime");
                            }
                            DateTime dt = DateTime.Now.AddHours(-24.00);
                            IEnumerable<tbl_ToysNotPrime> data = ukdb.tbl_ToysNotPrime.Where(x => x.Account1_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.Account2_Status != 1 && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                            ExportToysNotPrime(ea.PriceValue, ea.marketplace, data);
                        }
                        else if (ea.Cat == ConstantData.Baby)
                        {
                            if (ea.chkresult == true)
                            {
                                UkProhibitionTbl("tbl_BabyNotPrime");
                            }
                            DateTime dt = DateTime.Now.AddHours(-24.00);
                            IEnumerable<tbl_BabyNotPrime> data = ukdb.tbl_BabyNotPrime.Where(x => x.Account1_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.Account2_Status != 1 && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                            ExportBabyNotPrime(ea.PriceValue, ea.marketplace, data);
                        }
                        else if (ea.Cat == ConstantData.Watches)
                        {
                            if (ea.chkresult == true)
                            {
                                UkProhibitionTbl("tbl_WatchesNotPrime");
                            }
                            DateTime dt = DateTime.Now.AddHours(-24.00);
                            IEnumerable<tbl_WatchesNotPrime> data = ukdb.tbl_WatchesNotPrime.Where(x => x.Account1_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.Account2_Status != 1 && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                            ExportWatchesNotPrime(ea.PriceValue, ea.marketplace, data);
                        }
                        else if (ea.Cat == ConstantData.Jewelry)
                        {
                            if (ea.chkresult == true)
                            {
                                UkProhibitionTbl("tbl_JewelleryNotPrime");
                            }
                            DateTime dt = DateTime.Now.AddHours(-24.00);
                            IEnumerable<tbl_JewelleryNotPrime> data = ukdb.tbl_JewelleryNotPrime.Where(x => x.Account1_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.Account2_Status != 1 && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                            ExportJewelryNotPrime(ea.PriceValue, ea.marketplace, data);
                        }
                        else if (ea.Cat == ConstantData.HomeandKitchen)
                        {
                            if (ea.chkresult == true)
                            {
                                UkProhibitionTbl("tbl_HomeAndKitchenNotPrime");
                            }
                            DateTime dt = DateTime.Now.AddHours(-24.00);
                            IEnumerable<tbl_HomeAndKitchenNotPrime> data = ukdb.tbl_HomeAndKitchenNotPrime.Where(x => x.Account1_AmazonActive == 0 && x.Account4_AmazonActive == 0 && x.Reviewed == 1 && x.Account2_Status != 1 && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                            ExportHomeAndKitchenNotPrime(ea.PriceValue, ea.marketplace, data);
                        }


                    }
                    else if (ea.marketplace == ConstantData.DC)
                    {

                        if (ea.Cat == ConstantData.SportingGoods)
                        {
                            if (ea.canadachk == true)
                            {
                                CanadaProhibitionTbl("tbl_SportsNotPrime");
                            }
                            DateTime dt = DateTime.Now.AddHours(-24.00);
                            IEnumerable<tbl_SportsNotPrime> data = ukdb.tbl_SportsNotPrime.Where(x => x.Reviewed == 1 && x.Account3_Status != 1 && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                            ExportSportsCanadaNotPrime(ea.marketplace, ea.PriceValue, data);
                        }
                        else if (ea.Cat == ConstantData.Beauty)
                        {
                            if (ea.canadachk == true)
                            {
                                CanadaProhibitionTbl("tbl_BeautyNotPrime");
                            }
                            DateTime dt = DateTime.Now.AddHours(-24.00);
                            IEnumerable<tbl_BeautyNotPrime> data = ukdb.tbl_BeautyNotPrime.Where(x => x.Reviewed == 1 && x.Account3_Status != 1 && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                            ExportBeautyCanadaNotPrime(ea.PriceValue, ea.marketplace, data);
                        }
                        else if (ea.Cat == ConstantData.Toys)
                        {
                            if (ea.canadachk == true)
                            {
                                CanadaProhibitionTbl("tbl_ToysNotPrime");
                            }
                            DateTime dt = DateTime.Now.AddHours(-24.00);
                            IEnumerable<tbl_ToysNotPrime> data = ukdb.tbl_ToysNotPrime.Where(x => x.Reviewed == 1 && x.Account3_Status != 1 && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                            ExportToysCanadaNotPrime(ea.PriceValue, ea.marketplace, data);
                        }
                        else if (ea.Cat == ConstantData.Baby)
                        {
                            if (ea.canadachk == true)
                            {
                                CanadaProhibitionTbl("tbl_BabyNotPrime");
                            }
                            DateTime dt = DateTime.Now.AddHours(-24.00);
                            IEnumerable<tbl_BabyNotPrime> data = ukdb.tbl_BabyNotPrime.Where(x => x.Reviewed == 1 && x.Account3_Status != 1 && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                            ExportBabyCanadaNotPrime(ea.PriceValue, ea.marketplace, data);
                        }
                        else if (ea.Cat == ConstantData.Watches)
                        {
                            if (ea.canadachk == true)
                            {
                                CanadaProhibitionTbl("tbl_WatchesNotPrime");
                            }
                            DateTime dt = DateTime.Now.AddHours(-24.00);
                            IEnumerable<tbl_WatchesNotPrime> data = ukdb.tbl_WatchesNotPrime.Where(x => x.Reviewed == 1 && x.Account3_Status != 1 && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                            ExportWatchesCanadaNotPrime(ea.PriceValue, ea.marketplace, data);
                        }
                        else if (ea.Cat == ConstantData.Jewelry)
                        {
                            if (ea.canadachk == true)
                            {
                                CanadaProhibitionTbl("tbl_JewelleryNotPrime");
                            }
                            DateTime dt = DateTime.Now.AddHours(-24.00);
                            IEnumerable<tbl_JewelleryNotPrime> data = ukdb.tbl_JewelleryNotPrime.Where(x => x.Reviewed == 1 && x.Account3_Status != 1 && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                            ExportJewelryCanadaNotPrime(ea.PriceValue, ea.marketplace, data);

                        }
                        else if (ea.Cat == ConstantData.HomeandKitchen)
                        {
                            if (ea.canadachk == true)
                            {
                                CanadaProhibitionTbl("tbl_HomeAndKitchenNotPrime");
                            }
                            DateTime dt = DateTime.Now.AddHours(-24.00);
                            IEnumerable<tbl_HomeAndKitchenNotPrime> data = ukdb.tbl_HomeAndKitchenNotPrime.Where(x => x.Reviewed == 1 && x.Account3_Status != 1 && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                            ExportHomeAndKitchenCanadaNotPrime(ea.PriceValue, ea.marketplace, data);

                        }


                    }
                    else if (ea.marketplace == ConstantData.DI)
                    {
                        if (ea.Cat == ConstantData.SportingGoods)
                        {
                            if (ea.chkresult == true)
                            {
                                UkProhibitionTbl("tbl_SportsNotPrime");
                            }
                            DateTime dt = DateTime.Now.AddHours(-24.00);
                            IEnumerable<tbl_SportsNotPrime> data = ukdb.tbl_SportsNotPrime.Where(x => x.Account1_AmazonActive == 0 && x.Account2_AmazonActive == 0 && x.Reviewed == 1 && x.Account4_Status != 1 && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();

                            ExportForUKNotPrime(ea.PriceValue, ea.marketplace, data);
                        }
                        else if (ea.Cat == ConstantData.Beauty)
                        {
                            if (ea.chkresult == true)
                            {
                                UkProhibitionTbl("tbl_BeautyNotPrime");
                            }
                            DateTime dt = DateTime.Now.AddHours(-24.00);
                            IEnumerable<tbl_BeautyNotPrime> data = ukdb.tbl_BeautyNotPrime.Where(x => x.Account1_AmazonActive == 0 && x.Account2_AmazonActive == 0 && x.Reviewed == 1 && x.Account4_Status != 1 && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                            ExportBeautyNotPrime(ea.PriceValue, ea.marketplace, data);
                        }
                        else if (ea.Cat == ConstantData.Toys)
                        {
                            if (ea.chkresult == true)
                            {
                                UkProhibitionTbl("tbl_ToysNotPrime");
                            }
                            DateTime dt = DateTime.Now.AddHours(-24.00);
                            IEnumerable<tbl_ToysNotPrime> data = ukdb.tbl_ToysNotPrime.Where(x => x.Account1_AmazonActive == 0 && x.Account2_AmazonActive == 0 && x.Reviewed == 1 && x.Account4_Status != 1 && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                            //IEnumerable<tbl_Toys> data = ukdb.tbl_Toys.Where(x => x.UPC != "null" && (x.Account2_Status == 1 || x.Account2_Status == null) && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                            ExportToysNotPrime(ea.PriceValue, ea.marketplace, data);
                        }
                        else if (ea.Cat == ConstantData.Baby)
                        {
                            if (ea.chkresult == true)
                            {
                                UkProhibitionTbl("tbl_BabyNotPrime");
                            }
                            DateTime dt = DateTime.Now.AddHours(-24.00);
                            IEnumerable<tbl_BabyNotPrime> data = ukdb.tbl_BabyNotPrime.Where(x => x.Account1_AmazonActive == 0 && x.Account2_AmazonActive == 0 && x.Reviewed == 1 && x.Account4_Status != 1 && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                            ExportBabyNotPrime(ea.PriceValue, ea.marketplace, data);
                        }
                        else if (ea.Cat == ConstantData.Watches)
                        {
                            if (ea.chkresult == true)
                            {
                                UkProhibitionTbl("tbl_WatchesNotPrime");
                            }
                            DateTime dt = DateTime.Now.AddHours(-24.00);
                            IEnumerable<tbl_WatchesNotPrime> data = ukdb.tbl_WatchesNotPrime.Where(x => x.Account1_AmazonActive == 0 && x.Account2_AmazonActive == 0 && x.Reviewed == 1 && x.Account4_Status != 1 && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                            ExportWatchesNotPrime(ea.PriceValue, ea.marketplace, data);
                        }
                        else if (ea.Cat == ConstantData.Jewelry)
                        {
                            if (ea.chkresult == true)
                            {
                                UkProhibitionTbl("tbl_JewelleryNotPrime");
                            }
                            DateTime dt = DateTime.Now.AddHours(-24.00);
                            IEnumerable<tbl_JewelleryNotPrime> data = ukdb.tbl_JewelleryNotPrime.Where(x => x.Account1_AmazonActive == 0 && x.Account2_AmazonActive == 0 && x.Reviewed == 1 && x.Account4_Status != 1 && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                            ExportJewelryNotPrime(ea.PriceValue, ea.marketplace, data);
                        }
                        else if (ea.Cat == ConstantData.HomeandKitchen)
                        {
                            if (ea.chkresult == true)
                            {
                                UkProhibitionTbl("tbl_HomeAndKitchenNotPrime");
                            }
                            DateTime dt = DateTime.Now.AddHours(-24.00);
                            IEnumerable<tbl_HomeAndKitchenNotPrime> data = ukdb.tbl_HomeAndKitchenNotPrime.Where(x => x.Account1_AmazonActive == 0 && x.Account2_AmazonActive == 0 && x.Reviewed == 1 && x.Account4_Status != 1 && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(ea.rownumber.ToString())).ToList();
                            ExportHomeAndKitchenNotPrime(ea.PriceValue, ea.marketplace, data);
                        }


                    }


                }
            }

            ViewBag.SellingVenue = ukdb.SellingPlaces.OrderBy(x => x.SellingPlace1);
            ModelState.Clear();
            return View();

        }



        private void ExportJewelryCanadaNotPrime(double PriceValue, string shortcode, IEnumerable<tbl_JewelleryNotPrime> data)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Suppress))
            {


                string Exportpath = "";
                ((IObjectContextAdapter)ukdb).ObjectContext.CommandTimeout = 1800;

                StringWriter st = new StringWriter();
                string header1 = "TemplateType=Jewelry	Version=2015.1204	The top 3 rows are for Amazon.com use only. Do not modify or delete the top 3 rows.								Offer - Offer Information - These attributes are required to make your item buyable for customers on the site.																	Dimensions - Product Dimensions - These attributes specify the size and weight of a product.							Discovery - Item discovery information - These attributes have an effect on how customers can find your product on the site using browse or search.																				Images - Image Information - See Image Instructions tab for details.					Fulfillment - Use these columns to provide fulfillment-related information for either Amazon-fulfilled (FBA) or seller-fulfilled orders.	Variation - Variation information - Populate these attributes if your product is available in different variations (for example colour or wattage).				Compliance - Compliance Information - Attributes used to comply with consumer laws in the country or region where the item is sold.						Ungrouped - These attributes create rich product listings for your buyers.";
                string header2 = "Seller SKU	Title	Manufacturer	Model Number	Product Type	Brand Name	Product ID	Product ID Type	Product Description	Update Delete	Standard Price	Quantity	Launch Date	Product Tax Code	Manufacturer's Suggested Retail Price	Sale Price	Sale Start Date	Sale End Date	Release Date	Package Quantity	Fulfillment Latency	Restock Date	Max Aggregate Ship Quantity	Offering Can Be Gift Messaged	Is Gift Wrap Available	Is Discontinued by Manufacturer	Shipping-Template	Shipping Weight	Website Shipping Weight Unit Of Measure	Display Dimensions Unit Of Measure	Diameter	Display Height	Width	Item Display Length	recommended-browse-nodes1-2	Key Product Features1	Key Product Features2	Key Product Features3	Key Product Features4	Key Product Features5	Target Audience1	Target Audience2	Target Audience3	Intended Use1	Intended Use2	Intended Use3	Intended Use4	Intended Use5	Subject Matter1	Subject Matter2	Subject Matter3	Subject Matter4	Subject Matter5	Search Terms	Main Image URL	Swatch Image URL	Other Image Url1	Other Image Url2	Other Image Url3	Fulfillment Center ID	Parentage	Parent SKU	Relationship Type	Variation Theme	Country of Publication	Cpsia Warning1	Cpsia Warning2	Cpsia Warning3	Cpsia Warning4	CPSIA Warning Description	Other Attributes1	Other Attributes2	Other Attributes3	Other Attributes4	Other Attributes5	Total Metal Weight	Total Metal Weight Unit Of Measure	Total Diamond Weight	Total Diamond Weight Unit Of Measure	Total Gem Weight	Total Gem Weight Unit Of Measure	Material Type	Metal Type	Metal Stamp	Setting Type	Number Of Stones	Clasp Type	Chain Type	Gem Type1	Gem Type2	Gem Type3	Stone Color	Stone Clarity	Stone Shape	Stone Treatment Method	Stone Weight	Pearl Type	Color	Style	Pearl Minimum Color	Pearl Shape	Pearl Uniformity	Pearl Surface Blemishes	Pearl Stringing Method	Size Per Pearl	Gender	Ring Size	Ring Sizing Upper Range	Back Finding";
                string header3 = "item_sku	item_name	manufacturer	model	feed_product_type	brand_name	external_product_id	external_product_id_type	product_description	update_delete	standard_price	quantity	product_site_launch_date	product_tax_code	list_price	sale_price	sale_from_date	sale_end_date	merchant_release_date	item_package_quantity	fulfillment_latency	restock_date	max_aggregate_ship_quantity	offering_can_be_gift_messaged	offering_can_be_giftwrapped	is_discontinued_by_manufacturer	merchant_shipping_group_name	website_shipping_weight	website_shipping_weight_unit_of_measure	display_dimensions_unit_of_measure	item_display_diameter	item_display_height	item_display_width	item_display_length	recommended_browse_nodes	bullet_point1	bullet_point2	bullet_point3	bullet_point4	bullet_point5	target_audience_keywords1	target_audience_keywords2	target_audience_keywords3	specific_uses_keywords1	specific_uses_keywords2	specific_uses_keywords3	specific_uses_keywords4	specific_uses_keywords5	thesaurus_subject_keywords1	thesaurus_subject_keywords2	thesaurus_subject_keywords3	thesaurus_subject_keywords4	thesaurus_subject_keywords5	generic_keywords	main_image_url	swatch_image_url	other_image_url1	other_image_url2	other_image_url3	fulfillment_center_id	parent_child	parent_sku	relationship_type	variation_theme	country_of_origin	cpsia_cautionary_statement1	cpsia_cautionary_statement2	cpsia_cautionary_statement3	cpsia_cautionary_statement4	cpsia_cautionary_description	thesaurus_attribute_keywords1	thesaurus_attribute_keywords2	thesaurus_attribute_keywords3	thesaurus_attribute_keywords4	thesaurus_attribute_keywords5	total_metal_weight	total_metal_weight_unit_of_measure	total_diamond_weight	total_diamond_weight_unit_of_measure	total_gem_weight	total_gem_weight_unit_of_measure	material_type	metal_type	metal_stamp	setting_type	number_of_stones	clasp_type	chain_type	gem_type1	gem_type2	gem_type3	stone_color	stone_clarity	stone_shape	stone_treatment_method	stone_weight	pearl_type	color_name	style_name	pearl_minimum_color	pearl_shape	pearl_uniformity	pearl_surface_blemishes	pearl_stringing_method	size_per_pearl	department_name	ring_size	ring_sizing_upper_range	back_finding";

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(header1);
                sb.AppendLine(header2);
                sb.AppendLine(header3);
                //IEnumerable<tbl_Beauty> data = ukdb.tbl_Beauty.Where(x => x.UPC != "null" && x.Status == 0 && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(count.ToString())).ToList();
                if (data != null)
                {
                    foreach (var d in data)
                    {
                        try
                        {


                            double price = 0;
                            double pricecal;
                            double pricemin = 0.00;
                            double finalprice;
                            string UPC;
                            if (d.UPC == null)
                            {
                                upc_codes uc = ukdb.upc_codes.Where(x => x.AssignedTo == null).FirstOrDefault();
                                UPC = uc.Upc;
                                UpdateUPC(uc.Upc, "NP" + d.ASIN);
                            }
                            else
                            {
                                UPC = d.UPC;
                            }
                            if (d.UpdatedSalesPrice != null)
                            {
                                price = double.Parse(d.UpdatedSalesPrice);
                            }
                            else if (d.SalesPrice != null)
                            {
                                price = double.Parse(d.SalesPrice);
                            }
                            if (price != 0)
                            {


                                if (price < 200.01)
                                {


                                    if (price > 0 && price <= 19.99)
                                    {
                                        pricemin = 92.99;
                                    }
                                    else if (price > 19.99 && price <= 29.99)
                                    {
                                        pricemin = 120.99;
                                    }
                                    else if (price > 29.99 && price <= 39.99)
                                    {
                                        pricemin = 142.99;
                                    }
                                    else if (price > 39.99 && price <= 49.99)
                                    {
                                        pricemin = 168.99;
                                    }
                                    else if (price > 49.99 && price <= 59.99)
                                    {
                                        pricemin = 192.99;
                                    }
                                    else if (price > 59.99 && price <= 69.99)
                                    {
                                        pricemin = 222.99;
                                    }
                                    else if (price > 69.99 && price <= 79.99)
                                    {
                                        pricemin = 255.99;
                                    }
                                    else if (price > 79.99 && price <= 89.99)
                                    {
                                        pricemin = 285.99;
                                    }
                                    else if (price > 89.99 && price <= 99.99)
                                    {
                                        pricemin = 313.99;
                                    }
                                    else if (price > 99.99 && price <= 109.99)
                                    {
                                        pricemin = 327.99;
                                    }
                                    else if (price > 109.99 && price <= 119.99)
                                    {
                                        pricemin = 356.99;
                                    }
                                    else if (price > 119.99 && price <= 129.99)
                                    {
                                        pricemin = 385.99;
                                    }
                                    else
                                        if (price > 129.99 && price <= 139.99)
                                        {
                                            pricemin = 420.99;
                                        }
                                        else if (price > 139.99 && price <= 149.99)
                                        {
                                            pricemin = 442.99;
                                        }
                                        else if (price > 149.99 && price <= 159.99)
                                        {
                                            pricemin = 463.99;
                                        }
                                        else if (price > 159.99 && price <= 169.99)
                                        {
                                            pricemin = 485.99;
                                        }
                                        else
                                            if (price > 169.99 && price <= 179.99)
                                            {
                                                pricemin = 513.99;
                                            }
                                            else if (price > 179.99 && price <= 189.99)
                                            {
                                                pricemin = 542.99;
                                            }
                                            else if (price > 189.99 && price <= 199.99)
                                            {
                                                pricemin = 569.99;
                                            }
                                    pricecal = price * PriceValue;
                                    if (pricemin >= pricecal)
                                    {
                                        finalprice = pricemin;
                                    }
                                    else
                                    {
                                        finalprice = pricecal;
                                    }



                                    string ItemName;
                                    if (d.Title != null)
                                    {
                                        // ItemName = new string(d.Title.Take(490).ToArray());
                                        ItemName = d.Title.Substring(0, d.Title.Length > 499 ? 499 : d.Title.Length);
                                    }
                                    else
                                    {
                                        ItemName = null;
                                    }
                                    string description;

                                    if (d.Description != null)
                                    {
                                        string desc = Regex.Replace(d.Description, "<.*?>", String.Empty);
                                        //description = new string(desc.Take(1990).ToArray());
                                        description = desc.Substring(0, desc.Length > 1990 ? 1990 : desc.Length);
                                    }
                                    else
                                    {
                                        description = null;
                                    }

                                    string manufacturer;
                                    string brand;
                                    if (d.Manufacturer == null && d.Brand == null)
                                    {
                                        manufacturer = "Unknown";
                                        brand = "Unknown";
                                    }
                                    else
                                    {
                                        if (d.Manufacturer == null)
                                        {
                                            manufacturer = d.Brand;
                                        }
                                        else
                                        {
                                            manufacturer = d.Manufacturer;
                                        }

                                        if (d.Brand == null)
                                        {
                                            brand = d.Manufacturer;
                                        }
                                        else
                                        {
                                            brand = d.Brand;
                                        }

                                    }
                                    string feature1, feature2, feature3, feature4;
                                    if (d.Features1 != null)
                                    {
                                        // feature1 = new string(d.Features1.Take(500).ToArray());
                                        feature1 = d.Features1.Substring(0, d.Features1.Length > 500 ? 500 : d.Features1.Length);

                                    }
                                    else
                                        feature1 = d.Features1;
                                    if (d.Features2 != null)
                                    {
                                        // feature2 = new string(d.Features2.Take(500).ToArray());
                                        feature2 = d.Features2.Substring(0, d.Features2.Length > 500 ? 500 : d.Features2.Length);
                                    }
                                    else
                                        feature2 = d.Features2;
                                    if (d.Features3 != null)
                                    {
                                        //feature3 = new string(d.Features3.Take(500).ToArray());
                                        feature3 = d.Features3.Substring(0, d.Features3.Length > 500 ? 500 : d.Features3.Length);
                                    }
                                    else
                                        feature3 = d.Features3;
                                    if (d.Features4 != null)
                                    {
                                        // feature4 = new string(d.Features4.Take(500).ToArray());
                                        feature4 = d.Features4.Substring(0, d.Features4.Length > 500 ? 500 : d.Features4.Length);
                                    }
                                    else
                                        feature4 = d.Features4;

                                    sb.AppendLine(string.Join("\t",
                                                   string.Format(@"""{0}""", "NP-" + d.ASIN.Trim()),
                                                   string.Format(@"""{0}""", ItemName),
                                                   string.Format(@"""{0}""", manufacturer),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", "FashionOther"),
                                                   string.Format(@"""{0}""", brand),
                                                   string.Format(@"""{0}""", UPC),
                                                   string.Format(@"""{0}""", "UPC"),
                                                   string.Format(@"""{0}""", description),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", String.Format("{0:0.00}", finalprice)),
                                                   string.Format(@"""{0}""", Quantity),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", FulfillmentLatency),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", feature1),
                                                   string.Format(@"""{0}""", feature2),
                                                   string.Format(@"""{0}""", feature3),
                                                   string.Format(@"""{0}""", feature4),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", d.LargeImageUrl),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", "")));

                                    d.FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_Not_Prime_Jewelry_Canada_" + fcountWatches;

                                    d.ExportDate = DateTime.Now;
                                    d.Instock = 1;

                                    if (shortcode == ConstantData.DC)
                                    {
                                        d.Account3_Status = 1;
                                        d.Account3_ExportDate = DateTime.Now;
                                        Exportpath = d.Account3_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_DC_Not_Prime_Jewelry_Canada_" + fcountJewelry;
                                    }
                                    d.Status = 1;


                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    ukdb.ObjectContext().SaveChanges(SaveOptions.DetectChangesBeforeSave);

                    // ukdb.SaveChanges();
                    fcountJewelry++;
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=" + Exportpath + ".txt");
                    //Response.AddHeader("content-disposition", "attachment;filename="+name +"_"+ DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
                    Response.ContentType = "application/text";
                    Response.ContentEncoding = Encoding.GetEncoding(1252);
                    Response.Write(sb.ToString());
                    Response.End();
                }
                transaction.Complete();
                ukdb.ObjectContext().AcceptAllChanges();
            }
        }

        private void ExportWatchesCanadaNotPrime(double PriceValue, string shortcode, IEnumerable<tbl_WatchesNotPrime> data)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Suppress))
            {


                string Exportpath = "";
                ((IObjectContextAdapter)ukdb).ObjectContext.CommandTimeout = 1800;

                StringWriter st = new StringWriter();
                string header1 = "TemplateType=Watches	Version=2015.1204	The top 3 rows are for Amazon.com use only. Do not modify or delete the top 3 rows.							Offer - Offer Information - These attributes are required to make your item buyable for customers on the site.																			Dimensions - Product Dimensions - These attributes specify the size and weight of a product.				Discovery - Item discovery information - These attributes have an effect on how customers can find your product on the site using browse or search.								Images - Image Information - See Image Instructions tab for details.				Fulfillment - Use these columns to provide fulfillment-related information for either Amazon-fulfilled (FBA) or seller-fulfilled orders.							Variation - Variation information - Populate these attributes if your product is available in different variations (for example colour or wattage).				Compliance - Compliance Information - Attributes used to comply with consumer laws in the country or region where the item is sold.	Ungrouped - These attributes create rich product listings for your buyers.";
                string header2 = "Seller SKU	Product Name	Product ID	Product ID Type	Manufacturer	Manufacturer Part Number	Brand	Description	Update Delete	Standard Price	Item Condition	Condition Note	Quantity	Manufacturer's Suggested Retail Price	Launch Date	Release Date	Restock Date	Fulfillment Latency	Max Aggregate Ship Quantity	Sale Price	Sale Start Date	Sale End Date	Product Tax Code	Item Package Quantity	Offering Can Be Gift Messaged	Is Gift Wrap Available	Is Discontinued By Manufacturer	Shipping-Template	Item Weight	Item Weight Unit Of Measure	Website Shipping Weight Unit Of Measure	Shipping Weight	Recommended Browse Node	Search Terms	Target Audience	Key Product Features1	Key Product Features2	Key Product Features3	Key Product Features4	Key Product Features5	Main Image URL	Other Image URL1	Other Image URL2	Other Image URL3	Fulfillment Center ID	Package Height	Package Width	Package Length	Package Weight	Package Weight Unit Of Measure	Package Dimensions Unit Of Measure	Parentage	Parent SKU	Relationship Type	Variation Theme	Country Of Origin	Gender	Warranty Type	Band Material	Bezel Material	Band Width Unit Of Measure	Calendar Type	Case Material	Clasp Type	Dial Color	Dial Color Map	Case Diameter Unit Of Measure	Display Type	Style Name	Model Year	Movement Type	Special Features	Water Resistant Depth	Water Resistance Depth Unit Of Measure	Band Color	Band Length	Band Width	Case Size Diameter	Are Batteries Included	Battery Type	Number of Batteries Required";
                string header3 = "item_sku	item_name	external_product_id	external_product_id_type	manufacturer	part_number	brand_name	product_description	update_delete	standard_price	condition_type	condition_note	quantity	list_price	product_site_launch_date	merchant_release_date	restock_date	fulfillment_latency	max_aggregate_ship_quantity	sale_price	sale_from_date	sale_end_date	product_tax_code	item_package_quantity	offering_can_be_gift_messaged	offering_can_be_giftwrapped	is_discontinued_by_manufacturer	merchant_shipping_group_name	item_weight	item_weight_unit_of_measure	website_shipping_weight_unit_of_measure	website_shipping_weight	recommended_browse_nodes	generic_keywords	target_audience_keywords	bullet_point1	bullet_point2	bullet_point3	bullet_point4	bullet_point5	main_image_url	other_image_url1	other_image_url2	other_image_url3	fulfillment_center_id	package_height	package_width	package_length	package_weight	package_weight_unit_of_measure	package_dimensions_unit_of_measure	parent_child	parent_sku	relationship_type	variation_theme	country_of_origin	department_name	warranty_type	band_material_type	bezel_material_type	band_width_unit_of_measure	calendar_type	case_material_type	clasp_type	dial_color	color_name	case_diameter_unit_of_measure	display_type	style_name	model_year	watch_movement_type	special_features	water_resistance_depth	water_resistance_depth_unit_of_measure	band_color	band_size	band_width	case_diameter	are_batteries_included	battery_type	number_of_batteries";

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(header1);
                sb.AppendLine(header2);
                sb.AppendLine(header3);
                //IEnumerable<tbl_Beauty> data = ukdb.tbl_Beauty.Where(x => x.UPC != "null" && x.Status == 0 && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(count.ToString())).ToList();
                if (data != null)
                {
                    foreach (var d in data)
                    {
                        try
                        {


                            double price = 0;
                            double pricecal;
                            double pricemin = 0.00;
                            double finalprice;
                            string UPC;
                            if (d.UPC == null)
                            {
                                upc_codes uc = ukdb.upc_codes.Where(x => x.AssignedTo == null).FirstOrDefault();
                                UPC = uc.Upc;
                                UpdateUPC(uc.Upc, "NP" + d.ASIN);
                            }
                            else
                            {
                                UPC = d.UPC;
                            }
                            if (d.UpdatedSalesPrice != null)
                            {
                                price = double.Parse(d.UpdatedSalesPrice);
                            }
                            else if (d.SalesPrice != null)
                            {
                                price = double.Parse(d.SalesPrice);
                            }
                            if (price != 0)
                            {


                                if (price < 200.01)
                                {
                                    double minval = 49.99;


                                    if (price > 0 && price <= 19.99)
                                    {
                                        pricemin = 92.99;
                                    }
                                    else if (price > 19.99 && price <= 29.99)
                                    {
                                        pricemin = 120.99;
                                    }
                                    else if (price > 29.99 && price <= 39.99)
                                    {
                                        pricemin = 142.99;
                                    }
                                    else if (price > 39.99 && price <= 49.99)
                                    {
                                        pricemin = 168.99;
                                    }
                                    else if (price > 49.99 && price <= 59.99)
                                    {
                                        pricemin = 192.99;
                                    }
                                    else
                                        if (price > 59.99 && price <= 69.99)
                                        {
                                            pricemin = 222.99;
                                        }
                                        else if (price > 69.99 && price <= 79.99)
                                        {
                                            pricemin = 255.99;
                                        }
                                        else if (price > 79.99 && price <= 89.99)
                                        {
                                            pricemin = 285.99;
                                        }
                                        else if (price > 89.99 && price <= 99.99)
                                        {
                                            pricemin = 313.99;
                                        }
                                        else if (price > 99.99 && price <= 109.99)
                                        {
                                            pricemin = 327.99;
                                        }
                                        else if (price > 109.99 && price <= 119.99)
                                        {
                                            pricemin = 356.99;
                                        }
                                        else if (price > 119.99 && price <= 129.99)
                                        {
                                            pricemin = 385.99;
                                        }
                                        else if (price > 129.99 && price <= 139.99)
                                        {
                                            pricemin = 420.99;
                                        }
                                        else if (price > 139.99 && price <= 149.99)
                                        {
                                            pricemin = 442.99;
                                        }
                                        else
                                            if (price > 149.99 && price <= 159.99)
                                            {
                                                pricemin = 463.99;
                                            }
                                            else if (price > 159.99 && price <= 169.99)
                                            {
                                                pricemin = 485.99;
                                            }
                                            else if (price > 169.99 && price <= 179.99)
                                            {
                                                pricemin = 513.99;
                                            }
                                            else if (price > 179.99 && price <= 189.99)
                                            {
                                                pricemin = 542.99;
                                            }
                                            else if (price > 189.99 && price <= 199.99)
                                            {
                                                pricemin = 569.99;
                                            }
                                    pricecal = price * PriceValue;
                                    if (pricemin >= pricecal)
                                    {
                                        finalprice = pricemin;
                                    }
                                    else
                                    {
                                        finalprice = pricecal;
                                    }


                                    string ItemName;
                                    if (d.Title != null)
                                    {
                                        // ItemName = new string(d.Title.Take(490).ToArray());
                                        ItemName = d.Title.Substring(0, d.Title.Length > 499 ? 499 : d.Title.Length);
                                    }
                                    else
                                    {
                                        ItemName = null;
                                    }
                                    string description;

                                    if (d.Description != null)
                                    {
                                        string desc = Regex.Replace(d.Description, "<.*?>", String.Empty);
                                        //description = new string(desc.Take(1990).ToArray());
                                        description = desc.Substring(0, desc.Length > 1990 ? 1990 : desc.Length);
                                    }
                                    else
                                    {
                                        description = null;
                                    }

                                    string manufacturer;
                                    string brand;
                                    if (d.Manufacturer == null && d.Brand == null)
                                    {
                                        manufacturer = "Unknown";
                                        brand = "Unknown";
                                    }
                                    else
                                    {
                                        if (d.Manufacturer == null)
                                        {
                                            manufacturer = d.Brand;
                                        }
                                        else
                                        {
                                            manufacturer = d.Manufacturer;
                                        }

                                        if (d.Brand == null)
                                        {
                                            brand = d.Manufacturer;
                                        }
                                        else
                                        {
                                            brand = d.Brand;
                                        }

                                    }
                                    string feature1, feature2, feature3, feature4;
                                    if (d.Features1 != null)
                                    {
                                        // feature1 = new string(d.Features1.Take(500).ToArray());
                                        feature1 = d.Features1.Substring(0, d.Features1.Length > 500 ? 500 : d.Features1.Length);

                                    }
                                    else
                                        feature1 = d.Features1;
                                    if (d.Features2 != null)
                                    {
                                        // feature2 = new string(d.Features2.Take(500).ToArray());
                                        feature2 = d.Features2.Substring(0, d.Features2.Length > 500 ? 500 : d.Features2.Length);
                                    }
                                    else
                                        feature2 = d.Features2;
                                    if (d.Features3 != null)
                                    {
                                        //feature3 = new string(d.Features3.Take(500).ToArray());
                                        feature3 = d.Features3.Substring(0, d.Features3.Length > 500 ? 500 : d.Features3.Length);
                                    }
                                    else
                                        feature3 = d.Features3;
                                    if (d.Features4 != null)
                                    {
                                        // feature4 = new string(d.Features4.Take(500).ToArray());
                                        feature4 = d.Features4.Substring(0, d.Features4.Length > 500 ? 500 : d.Features4.Length);
                                    }
                                    else
                                        feature4 = d.Features4;

                                    sb.AppendLine(string.Join("\t",
                                                   string.Format(@"""{0}""", "NP-" + d.ASIN.Trim()),
                                                   string.Format(@"""{0}""", ItemName),
                                                   string.Format(@"""{0}""", UPC),
                                                   string.Format(@"""{0}""", "UPC"),
                                                   string.Format(@"""{0}""", manufacturer),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", brand),
                                                   string.Format(@"""{0}""", description),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", String.Format("{0:0.00}", finalprice)),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", Quantity),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", FulfillmentLatency),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", feature1),
                                                   string.Format(@"""{0}""", feature2),
                                                   string.Format(@"""{0}""", feature3),
                                                   string.Format(@"""{0}""", feature4),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", d.LargeImageUrl),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", "")));

                                    d.FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_Not_Prime_Watches_Canada_" + fcountWatches;

                                    d.ExportDate = DateTime.Now;
                                    d.Instock = 1;

                                    if (shortcode == ConstantData.DC)
                                    {
                                        d.Account3_Status = 1;
                                        d.Account3_ExportDate = DateTime.Now;
                                        Exportpath = d.Account3_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_DC_Not_Prime_Watches_Canada_" + fcountWatches;
                                    }
                                    d.Status = 1;


                                }
                            }
                        }
                        catch (Exception ex)
                        {


                        }
                    }
                    ukdb.ObjectContext().SaveChanges(SaveOptions.DetectChangesBeforeSave);//  ukdb.SaveChanges();

                    fcountWatches++;
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=" + Exportpath + ".txt");
                    //Response.AddHeader("content-disposition", "attachment;filename="+name +"_"+ DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
                    Response.ContentType = "application/text";
                    Response.ContentEncoding = Encoding.GetEncoding(1252);
                    Response.Write(sb.ToString());
                    Response.End();
                }
                transaction.Complete();
                ukdb.ObjectContext().AcceptAllChanges();
            }
        }

        private void ExportBabyCanadaNotPrime(double PriceValue, string shortcode, IEnumerable<tbl_BabyNotPrime> data)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Suppress))
            {



                string Exportpath = "";
                ((IObjectContextAdapter)ukdb).ObjectContext.CommandTimeout = 1800;

                StringWriter st = new StringWriter();
                string header1 = "TemplateType=Baby	Version=2015.1204	The top 3 rows are for Amazon.com use only. Do not modify or delete the top 3 rows.								Offer - Offer Information - These attributes are required to make your item buyable for customers on the site.																			Dimensions - Product Dimensions - These attributes specify the size and weight of a product.																		Discovery - Item discovery information - These attributes have an effect on how customers can find your product on the site using browse or search.									Images - Image Information - See Image Instructions tab for details.					Fulfillment - Use these columns to provide fulfillment-related information for either Amazon-fulfilled (FBA) or seller-fulfilled orders.									Variation - Variation information - Populate these attributes if your product is available in different variations (for example colour or wattage).				Compliance - Compliance Information - Attributes used to comply with consumer laws in the country or region where the item is sold.				Ungrouped - These attributes create rich product listings for your buyers.";
                string header2 = "Title	Description	Brand	Manufacturer	Part Number	Seller SKU	Update Delete	Product ID	Product ID Type	Product Type	Fulfillment Latency	Number of Items	Item Condition	Offer Condition Note	Is Gift Wrap Available	Offering Can Be Gift Messaged	Is Discontinued by Manufacturer	Release Date	Launch Date	Item Package Quantity	Sale End Date	Product Tax Code	Sale Price	Standard Price	Launch Date	Restock Date	Sale Start Date	Quantity	Shipping-Template	Item Height Unit Of Measure	Item Height	Item Width	Item Length Unit Of Measure	Item Width Unit Of Measure	Item Length	Item Weight	Item Weight Unit Of Measure	Item Display Height Unit Of Measure	Display Height	Display Width	Item Display Length Unit Of Measure	Item Display Width Unit Of Measure	Display Length	Item Display Weight Unit Of Measure	Display Weight	Shipping Weight	Website Shipping Weight Unit Of Measure	Bullet Point1	Bullet Point2	Bullet Point3	Bullet Point4	Bullet Point5	Recommended Browse Nodes	Intended Use	Target Audience	Subject Matter	Swatch Image URL	Other Image URL1	Other Image URL2	Other Image URL3	Main Image URL	Package Height Unit Of Measure	Package Height	Package Width	Package Length Unit Of Measure	Package Width Unit Of Measure	Package Length	Package Weight	Package Weight Unit Of Measure	Fulfillment Center ID	Variation Theme	Parentage	Parent SKU	Relationship Type	Country Of Origin	Region Of Origin	Legal Disclaimer	Safety Warning	Recommended Uses	Specific Uses For Product	Target Gender	Batteries are Included	Battery Type	Lithium Battery Packaging	Lithium Battery Voltage Unit Of Measure	Lithium Battery Voltage	Lithium Battery Weight Unit Of Measure	Lithium Battery Weight	Size	Size Map	Color Map	Color	Material Type	Maximum Manufacturer Age Recommended	Minimum Manufacturer Age  Recommended	Minimum Weight Recommended	Number Of Pieces	Unit Count Unit Of Measure	Maximum Manufacturer Weight Recommended	weight_recommendation_unit_of_measure";
                string header3 = "item_name	product_description	brand_name	manufacturer	part_number	item_sku	update_delete	external_product_id	external_product_id_type	feed_product_type	fulfillment_latency	number_of_items	condition_type	condition_note	offering_can_be_giftwrapped	offering_can_be_gift_messaged	is_discontinued_by_manufacturer	merchant_release_date	product_site_launch_date	item_package_quantity	sale_end_date	product_tax_code	sale_price	standard_price	offering_start_date	restock_date	sale_from_date	quantity	merchant_shipping_group_name	item_height_unit_of_measure	item_height	item_width	item_length_unit_of_measure	item_width_unit_of_measure	item_length	item_weight	item_weight_unit_of_measure	item_display_height_unit_of_measure	item_display_height	item_display_width	item_display_length_unit_of_measure	item_display_width_unit_of_measure	item_display_length	item_display_weight_unit_of_measure	item_display_weight	website_shipping_weight	website_shipping_weight_unit_of_measure	bullet_point1	bullet_point2	bullet_point3	bullet_point4	bullet_point5	recommended_browse_nodes	specific_uses_keywords	target_audience_keywords	thesaurus_subject_keywords	swatch_image_url	other_image_url1	other_image_url2	other_image_url3	main_image_url	package_height_unit_of_measure	package_height	package_width	package_length_unit_of_measure	package_width_unit_of_measure	package_length	package_weight	package_weight_unit_of_measure	fulfillment_center_id	variation_theme	parent_child	parent_sku	relationship_type	country_of_origin	region_of_origin	legal_disclaimer_description	safety_warning	recommended_uses_for_product	specific_uses_for_product	target_gender	are_batteries_included	battery_cell_composition	lithium_battery_packaging	lithium_battery_voltage_unit_of_measure	lithium_battery_voltage	lithium_battery_weight_unit_of_measure	lithium_battery_weight	size_name	size_map	color_name	color_map	material_type	mfg_maximum	mfg_minimum	minimum_weight_recommendation	number_of_pieces	unit_count_type	maximum_weight_recommendation	weight_recommendation_unit_of_measure";

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(header1);
                sb.AppendLine(header2);
                sb.AppendLine(header3);
                //IEnumerable<tbl_Beauty> data = ukdb.tbl_Beauty.Where(x => x.UPC != "null" && x.Status == 0 && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(count.ToString())).ToList();
                if (data != null)
                {
                    foreach (var d in data)
                    {
                        try
                        {


                            double price = 0;
                            double pricecal;
                            double pricemin = 0.00;
                            double finalprice;
                            string UPC;
                            if (d.UPC == null)
                            {
                                upc_codes uc = ukdb.upc_codes.Where(x => x.AssignedTo == null).FirstOrDefault();
                                UPC = uc.Upc;
                                UpdateUPC(uc.Upc, "NP" + d.ASIN);
                            }
                            else
                            {
                                UPC = d.UPC;
                            }
                            if (d.UpdatedSalesPrice != null)
                            {
                                price = double.Parse(d.UpdatedSalesPrice);
                            }
                            else if (d.SalesPrice != null)
                            {
                                price = double.Parse(d.SalesPrice);
                            }
                            if (price != 0)
                            {
                                double minval = 49.99;


                                if (price > 0 && price <= 19.99)
                                {
                                    pricemin = 92.99;
                                }
                                else if (price > 19.99 && price <= 29.99)
                                {
                                    pricemin = 120.99;
                                }
                                else if (price > 29.99 && price <= 39.99)
                                {
                                    pricemin = 142.99;
                                }
                                else if (price > 39.99 && price <= 49.99)
                                {
                                    pricemin = 168.99;
                                }
                                else
                                    if (price > 49.99 && price <= 59.99)
                                    {
                                        pricemin = 192.99;
                                    }
                                    else if (price > 59.99 && price <= 69.99)
                                    {
                                        pricemin = 222.99;
                                    }
                                    else if (price > 69.99 && price <= 79.99)
                                    {
                                        pricemin = 255.99;
                                    }
                                    else if (price > 79.99 && price <= 89.99)
                                    {
                                        pricemin = 285.99;
                                    }
                                    else if (price > 89.99 && price <= 99.99)
                                    {
                                        pricemin = 313.99;
                                    }
                                    else if (price > 99.99 && price <= 109.99)
                                    {
                                        pricemin = 327.99;
                                    }
                                    else if (price > 109.99 && price <= 119.99)
                                    {
                                        pricemin = 356.99;
                                    }
                                    else if (price > 119.99 && price <= 129.99)
                                    {
                                        pricemin = 385.99;
                                    }
                                    else if (price > 129.99 && price <= 139.99)
                                    {
                                        pricemin = 420.99;
                                    }
                                    else if (price > 139.99 && price <= 149.99)
                                    {
                                        pricemin = 442.99;
                                    }
                                    else if (price > 149.99 && price <= 159.99)
                                    {
                                        pricemin = 463.99;
                                    }
                                    else if (price > 159.99 && price <= 169.99)
                                    {
                                        pricemin = 485.99;
                                    }
                                    else if (price > 169.99 && price <= 179.99)
                                    {
                                        pricemin = 513.99;
                                    }
                                    else if (price > 179.99 && price <= 189.99)
                                    {
                                        pricemin = 542.99;
                                    }
                                    else if (price > 189.99 && price <= 199.99)
                                    {
                                        pricemin = 569.99;
                                    }
                                pricecal = price * PriceValue;
                                if (pricemin >= pricecal)
                                {
                                    finalprice = pricemin;
                                }
                                else
                                {
                                    finalprice = pricecal;
                                }



                                string ItemName;
                                if (d.Title != null)
                                {
                                    // ItemName = new string(d.Title.Take(490).ToArray());
                                    ItemName = d.Title.Substring(0, d.Title.Length > 499 ? 499 : d.Title.Length);
                                }
                                else
                                {
                                    ItemName = null;
                                }
                                string description;

                                if (d.Description != null)
                                {
                                    string desc = Regex.Replace(d.Description, "<.*?>", String.Empty);
                                    //description = new string(desc.Take(1990).ToArray());
                                    description = desc.Substring(0, desc.Length > 1990 ? 1990 : desc.Length);
                                }
                                else
                                {
                                    description = null;
                                }

                                string manufacturer;
                                string brand;
                                if (d.Manufacturer == null && d.Brand == null)
                                {
                                    manufacturer = "Unknown";
                                    brand = "Unknown";
                                }
                                else
                                {
                                    if (d.Manufacturer == null)
                                    {
                                        manufacturer = d.Brand;
                                    }
                                    else
                                    {
                                        manufacturer = d.Manufacturer;
                                    }

                                    if (d.Brand == null)
                                    {
                                        brand = d.Manufacturer;
                                    }
                                    else
                                    {
                                        brand = d.Brand;
                                    }

                                }
                                string feature1, feature2, feature3, feature4;
                                if (d.Features1 != null)
                                {
                                    // feature1 = new string(d.Features1.Take(500).ToArray());
                                    feature1 = d.Features1.Substring(0, d.Features1.Length > 500 ? 500 : d.Features1.Length);

                                }
                                else
                                    feature1 = d.Features1;
                                if (d.Features2 != null)
                                {
                                    // feature2 = new string(d.Features2.Take(500).ToArray());
                                    feature2 = d.Features2.Substring(0, d.Features2.Length > 500 ? 500 : d.Features2.Length);
                                }
                                else
                                    feature2 = d.Features2;
                                if (d.Features3 != null)
                                {
                                    //feature3 = new string(d.Features3.Take(500).ToArray());
                                    feature3 = d.Features3.Substring(0, d.Features3.Length > 500 ? 500 : d.Features3.Length);
                                }
                                else
                                    feature3 = d.Features3;
                                if (d.Features4 != null)
                                {
                                    // feature4 = new string(d.Features4.Take(500).ToArray());
                                    feature4 = d.Features4.Substring(0, d.Features4.Length > 500 ? 500 : d.Features4.Length);
                                }
                                else
                                    feature4 = d.Features4;

                                sb.AppendLine(string.Join("\t",
                                               string.Format(@"""{0}""", ItemName),
                                               string.Format(@"""{0}""", description),
                                               string.Format(@"""{0}""", brand),
                                               string.Format(@"""{0}""", manufacturer),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", "NP-" + d.ASIN.Trim()),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", UPC),
                                               string.Format(@"""{0}""", "UPC"),
                                               string.Format(@"""{0}""", "BabyProducts"),
                                               string.Format(@"""{0}""", FulfillmentLatency),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", String.Format("{0:0.00}", finalprice)),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", Quantity),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", feature1),
                                               string.Format(@"""{0}""", feature2),
                                               string.Format(@"""{0}""", feature3),
                                               string.Format(@"""{0}""", feature4),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", d.LargeImageUrl),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", "")));

                                d.FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_Not_Prime_Baby_Canada_" + fcountBaby;

                                d.ExportDate = DateTime.Now;
                                d.Instock = 1;

                                if (shortcode == ConstantData.DC)
                                {
                                    d.Account3_Status = 1;
                                    d.Account3_ExportDate = DateTime.Now;
                                    Exportpath = d.Account3_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_DC_Not_Prime_Baby_Canada_" + fcountBaby;
                                }
                                d.Status = 1;


                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    // ukdb.SaveChanges();
                    ukdb.ObjectContext().SaveChanges(SaveOptions.DetectChangesBeforeSave);

                    fcountBaby++;
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=" + Exportpath + ".txt");
                    //Response.AddHeader("content-disposition", "attachment;filename="+name +"_"+ DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
                    Response.ContentType = "application/text";
                    Response.ContentEncoding = Encoding.GetEncoding(1252);
                    Response.Write(sb.ToString());
                    Response.End();
                }
                transaction.Complete();
                ukdb.ObjectContext().AcceptAllChanges();
            }
        }

        private void ExportToysCanadaNotPrime(double PriceValue, string shortcode, IEnumerable<tbl_ToysNotPrime> data)
        {

            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Suppress))
            {
                string Exportpath = "";
                StringWriter st = new StringWriter();

                string header1 = "TemplateType=Toys	Version=2016.0108	The top 3 rows are for Amazon.com use only. Do not modify or delete the top 3 rows.									Offer - Offer Information - These attributes are required to make your item buyable for customers on the site.																					Dimensions - Product Dimensions - These attributes specify the size and weight of a product.								Discovery - Item discovery information - These attributes have an effect on how customers can find your product on the site using browse or search.														Images - Image Information - See Image Instructions tab for details.					Fulfillment - Use these columns to provide fulfillment-related information for either Amazon-fulfilled (FBA) or seller-fulfilled orders.							Compliance - Compliance Information - Attributes used to comply with consumer laws in the country or region where the item is sold.					Ungrouped - These attributes create rich product listings for your buyers.";
                string header2 = "Seller SKU	Product ID	Product ID Type	Product Type	Product Name	Brand Name	Manufacturer	Manufacturer Part Number	Product Description	Edition	Update Delete	Product Tax Code	Launch Date	Stop Selling Date	Item Condition	Offer Condition Note	Is Gift Wrap Available	Offering Can Be Gift Messaged	Minimum Advertised Price	Manufacturer's Suggested Retail Price	Standard Price	Quantity	Release Date	Fulfillment Latency	Restock Date	Sale Price	Sale Start Date	Sale End Date	Package Quantity	Max Aggregate Ship Quantity	Is Discontinued by Manufacturer	Shipping-Template	Item Weight	Item Weight Unit Of Measure	Item Height	Item Length	Item Width	Item Length Unit Of Measure	Shipping Weight	Website Shipping Weight Unit Of Measure	recommended-browse-nodes	Key Product Features1	Key Product Features2	Key Product Features3	Key Product Features4	Key Product Features5	Intended Use1	Intended Use2	Intended Use3	Intended Use4	Intended Use5	Target Audience	Search Terms	Style-specific Terms	Swatch Image URL	Main Image URL	Other Image URL1	Other Image URL2	Other Image URL3	Fulfillment Center ID	Package Length	Package Width	Package Height	Package Length Unit Of Measure	Package Weight	Package Weight Unit Of Measure	Cpsia Warning	CPSIA Warning Description	Legal Disclaimer	Safety Warning	Country of Publication	Minimum Manufacturer Age Recommended	Minimum Manufacturer Age Recommended Unit Of Measure	Maximum Manufacturer Age Recommended	Maximum Manufacturer Age Recommended Unit Of Measure	Other Attributes	Theme	Character	Educational Objective	Size	Size Map	Specific Uses For Product	Genre	Material Type	Assembly Time	Assembly Time Unit Of Measure	Manufacturer Warranty Description	Number of Pieces	Batteries are Included	Battery Type	Number of Batteries Required	Weight Supported	Maximum Weight Recommendation Unit Of Measure";
                string header3 = "item_sku	external_product_id	external_product_id_type	feed_product_type	item_name	brand_name	manufacturer	part_number	product_description	edition	update_delete	product_tax_code	product_site_launch_date	offering_end_date	condition_type	condition_note	offering_can_be_giftwrapped	offering_can_be_gift_messaged	map_price	list_price	standard_price	quantity	merchant_release_date	fulfillment_latency	restock_date	sale_price	sale_from_date	sale_end_date	item_package_quantity	max_aggregate_ship_quantity	is_discontinued_by_manufacturer	merchant_shipping_group_name	item_weight	item_weight_unit_of_measure	item_height	item_length	item_width	item_length_unit_of_measure	website_shipping_weight	website_shipping_weight_unit_of_measure	recommended_browse_nodes	bullet_point1	bullet_point2	bullet_point3	bullet_point4	bullet_point5	specific_uses_keywords1	specific_uses_keywords2	specific_uses_keywords3	specific_uses_keywords4	specific_uses_keywords5	target_audience_keywords	generic_keywords	style_keywords	swatch_image_url	main_image_url	other_image_url1	other_image_url2	other_image_url3	fulfillment_center_id	package_length	package_width	package_height	package_length_unit_of_measure	package_weight	package_weight_unit_of_measure	cpsia_cautionary_statement	cpsia_cautionary_description	legal_disclaimer_description	safety_warning	country_of_origin	mfg_minimum	mfg_minimum_unit_of_measure	mfg_maximum	mfg_maximum_unit_of_measure	thesaurus_attribute_keywords	theme	subject_character	educational_objective	size_name	size_map	specific_uses_for_product	genre	material_type	assembly_time	assembly_time_unit_of_measure	warranty_description	number_of_pieces	are_batteries_included	battery_type	number_of_batteries	maximum_weight_recommendation	maximum_weight_recommendation_unit_of_measure";

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(header1);
                sb.AppendLine(header2);
                sb.AppendLine(header3);
                ((IObjectContextAdapter)ukdb).ObjectContext.CommandTimeout = 1800;
                if (data != null)
                {
                    foreach (var d in data)
                    {
                        try
                        {


                            double price = 0;
                            double pricecal;
                            double pricemin = 0.00;
                            double finalprice;

                            string UPC;
                            if (d.UPC == null)
                            {
                                upc_codes uc = ukdb.upc_codes.Where(x => x.AssignedTo == null).FirstOrDefault();
                                UPC = uc.Upc;
                                
                                UpdateUPC(uc.Upc, d.ASIN);
                            }
                            else
                            {
                                UPC = d.UPC;
                            }
                            if (d.UpdatedSalesPrice != null)
                            {
                                price = double.Parse(d.UpdatedSalesPrice);
                            }
                            else if (d.SalesPrice != null)
                            {
                                price = double.Parse(d.SalesPrice);
                            }

                            if (price != 0)
                            {

                                if (price < 200.01)
                                {

                                    double minval = 0.00;

                                    if (price > 0 && price <= 19.99)
                                    {
                                        pricemin = 92.99;
                                    }
                                    else if (price > 19.99 && price <= 29.99)
                                    {
                                        pricemin = 120.99;
                                    }
                                    else if (price > 29.99 && price <= 39.99)
                                    {
                                        pricemin = 142.99;
                                    }
                                    else if (price > 39.99 && price <= 49.99)
                                    {
                                        pricemin = 168.99;
                                    }
                                    else if (price > 49.99 && price <= 59.99)
                                    {
                                        pricemin = 192.99;
                                    }
                                    else if (price > 59.99 && price <= 69.99)
                                    {
                                        pricemin = 222.99;
                                    }
                                    else if (price > 69.99 && price <= 79.99)
                                    {
                                        pricemin = 255.99;
                                    }
                                    else if (price > 79.99 && price <= 89.99)
                                    {
                                        pricemin = 285.99;
                                    }
                                    else if (price > 89.99 && price <= 99.99)
                                    {
                                        pricemin = 313.99;
                                    }
                                    else if (price > 99.99 && price <= 109.99)
                                    {
                                        pricemin = 327.99;
                                    }
                                    else if (price > 109.99 && price <= 119.99)
                                    {
                                        pricemin = 356.99;
                                    }
                                    else if (price > 119.99 && price <= 129.99)
                                    {
                                        pricemin = 385.99;
                                    }
                                    else if (price > 129.99 && price <= 139.99)
                                    {
                                        pricemin = 420.99;
                                    }
                                    else if (price > 139.99 && price <= 149.99)
                                    {
                                        pricemin = 442.99;
                                    }
                                    else if (price > 149.99 && price <= 159.99)
                                    {
                                        pricemin = 463.99;
                                    }
                                    else if (price > 159.99 && price <= 169.99)
                                    {
                                        pricemin = 485.99;
                                    }
                                    else if (price > 169.99 && price <= 179.99)
                                    {
                                        pricemin = 513.99;
                                    }
                                    else if (price > 179.99 && price <= 189.99)
                                    {
                                        pricemin = 542.99;
                                    }
                                    else if (price > 189.99 && price <= 199.99)
                                    {
                                        pricemin = 569.99;
                                    }


                                    pricecal = price * PriceValue;
                                    if (pricemin >= pricecal)
                                    {
                                        finalprice = pricemin;
                                    }
                                    else
                                    {
                                        finalprice = pricecal;
                                    }



                                    string ItemName;
                                    if (d.Title != null)
                                    {
                                        // ItemName = new string(d.Title.Take(490).ToArray());
                                        ItemName = d.Title.Substring(0, d.Title.Length > 499 ? 499 : d.Title.Length);
                                    }
                                    else
                                    {
                                        ItemName = null;
                                    }
                                    string description;

                                    if (d.Description != null)
                                    {
                                        string desc = Regex.Replace(d.Description, "<.*?>", String.Empty);
                                        //description = new string(desc.Take(1990).ToArray());
                                        description = desc.Substring(0, desc.Length > 1990 ? 1990 : desc.Length);
                                    }
                                    else
                                    {
                                        description = null;
                                    }

                                    string manufacturer;
                                    string brand;
                                    if (d.Manufacturer == null && d.Brand == null)
                                    {
                                        manufacturer = "Unknown";
                                        brand = "Unknown";
                                    }
                                    else
                                    {
                                        if (d.Manufacturer == null)
                                        {
                                            manufacturer = d.Brand;
                                        }
                                        else
                                        {
                                            manufacturer = d.Manufacturer;
                                        }

                                        if (d.Brand == null)
                                        {
                                            brand = d.Manufacturer;
                                        }
                                        else
                                        {
                                            brand = d.Brand;
                                        }

                                    }
                                    string feature1, feature2, feature3, feature4;
                                    if (d.Features1 != null)
                                    {
                                        // feature1 = new string(d.Features1.Take(500).ToArray());
                                        feature1 = d.Features1.Substring(0, d.Features1.Length > 500 ? 500 : d.Features1.Length);

                                    }
                                    else
                                        feature1 = d.Features1;
                                    if (d.Features2 != null)
                                    {
                                        // feature2 = new string(d.Features2.Take(500).ToArray());
                                        feature2 = d.Features2.Substring(0, d.Features2.Length > 500 ? 500 : d.Features2.Length);
                                    }
                                    else
                                        feature2 = d.Features2;
                                    if (d.Features3 != null)
                                    {
                                        //feature3 = new string(d.Features3.Take(500).ToArray());
                                        feature3 = d.Features3.Substring(0, d.Features3.Length > 500 ? 500 : d.Features3.Length);
                                    }
                                    else
                                        feature3 = d.Features3;
                                    if (d.Features4 != null)
                                    {
                                        // feature4 = new string(d.Features4.Take(500).ToArray());
                                        feature4 = d.Features4.Substring(0, d.Features4.Length > 500 ? 500 : d.Features4.Length);
                                    }
                                    else
                                        feature4 = d.Features4;
                                    sb.AppendLine(string.Join("\t",
                                                 string.Format(@"""{0}""", d.ASIN.Trim()),
                                                 string.Format(@"""{0}""", UPC),
                                                 string.Format(@"""{0}""", "UPC"),
                                                 string.Format(@"""{0}""", "ToysAndGames"),
                                                 string.Format(@"""{0}""", !string.IsNullOrEmpty(ItemName) ? ItemName.Contains('"') ? ItemName.Replace('"', ' ') : ItemName : string.Empty),
                                                 string.Format(@"""{0}""", brand),
                                                 string.Format(@"""{0}""", manufacturer),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", !string.IsNullOrEmpty(description) ? description.Contains('"') ? description.Replace('"', ' ') : description : string.Empty),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", String.Format("{0:0.00}", finalprice)),
                                                 string.Format(@"""{0}""", Quantity),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", FulfillmentLatency),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", !string.IsNullOrEmpty(feature1) ? feature1.Contains('"') ? feature1.Replace('"', ' ') : feature1 : string.Empty),
                                                 string.Format(@"""{0}""", !string.IsNullOrEmpty(feature2) ? feature2.Contains('"') ? feature2.Replace('"', ' ') : feature2 : string.Empty),
                                                 string.Format(@"""{0}""", !string.IsNullOrEmpty(feature3) ? feature3.Contains('"') ? feature3.Replace('"', ' ') : feature3 : string.Empty),
                                                 string.Format(@"""{0}""", !string.IsNullOrEmpty(feature4) ? feature4.Contains('"') ? feature4.Replace('"', ' ') : feature4 : string.Empty),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", d.LargeImageURL),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", ""),
                                                 string.Format(@"""{0}""", "")));

                                    d.FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_Not_Prime_Toys_" + fcountToys;

                                    d.ExportDate = DateTime.Now;
                                    if (shortcode == ConstantData.DC)
                                    {
                                        d.Account3_Status = 1;
                                        d.Account3_ExportDate = DateTime.Now;
                                        Exportpath = d.Account3_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_DC_Not_Prime_Toys_" + fcountToys;
                                    }

                                    d.Instock = 1;
                                    d.Status = 1;

                                }
                            }
                            //  ukdb.SaveChanges();
                        }
                        catch (Exception ex)
                        {


                        }
                    }
                    ukdb.ObjectContext().SaveChanges(SaveOptions.DetectChangesBeforeSave);

                    fcountToys++;
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=" + Exportpath + ".txt");
                    //Response.AddHeader("content-disposition", "attachment;filename="+name +"_"+ DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
                    Response.ContentType = "application/text";
                    Response.ContentEncoding = Encoding.GetEncoding(1252);
                    Response.Write(sb.ToString());
                    Response.End();



                    transaction.Complete();
                    ukdb.ObjectContext().AcceptAllChanges();
                }

                //throw new NotImplementedException();
            }
        }

        private void ExportSportsCanadaNotPrime(string shortcode, double PriceValue, IEnumerable<tbl_SportsNotPrime> data)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Suppress))
            {
                string Exportpath = "";

                ((IObjectContextAdapter)ukdb).ObjectContext.CommandTimeout = 1800;



                string header1 = "TemplateType=Sports	Version=2015.1204	The top 3 rows are for Amazon.com use only. Do not modify or delete the top 3 rows.								Offer - Offer Information - These attributes are required to make your item buyable for customers on the site.																		Dimensions - Product Dimensions - These attributes specify the size and weight of a product.										Discovery - Item discovery information - These attributes have an effect on how customers can find your product on the site using browse or search.							Images - Image Information - See Image Instructions tab for details.					Variation - Variation information - Populate these attributes if your product is available in different variations (for example colour or wattage).				Compliance - Compliance Information - Attributes used to comply with consumer laws in the country or region where the item is sold.		Ungrouped - These attributes create rich product listings for your buyers.";
                string header2 = "Seller SKU	Product ID	Product ID Type	Product Name	Product Description	Manufacturer	Manufacturer Part Number	Product Type	Brand	Update Delete	Standard Price	Item Condition	Condition Note	Quantity	Manufacturer's Suggested Retail Price	Launch Date	Release Date	Restock Date	Fulfillment Latency	Sale Price	Sale Start Date	Sale End Date	Offering Can Be Gift Messaged	Is Gift Wrap Available	Is Discontinued By Manufacturer	Number of Items	Product Tax Code	Shipping-Template	Shipping Weight	Website Shipping Weight Unit Of Measure	Item Display Height	Item Display Height Unit Of Measure	Item Display Length	Item Display Length Unit Of Measure	Item Display Width	Item Display Width Unit Of Measure	Item Display Weight	Item Display Weight Unit Of Measure	Bullet Point1	Bullet Point2	Bullet Point3	Bullet Point4	Bullet Point5	Recommended Browse Node	Search Terms	Main Image URL	Swatch Image URL	Other Image URL1	Other Image URL2	Other Image URL3	Parentage	Parent SKU	Relationship Type	Variation Theme	Safety Warning	Legal Disclaimer	Color	Grip Size	Grip Type	Hand	Lens Color	Shape	Size	Style	Tension Level	Golf Flex	Golf Loft	Shaft Length	Shaft Length Unit Of Measure	Shaft Material";
                string header3 = "item_sku	external_product_id	external_product_id_type	item_name	product_description	manufacturer	part_number	feed_product_type	brand_name	update_delete	standard_price	condition_type	condition_note	quantity	list_price	product_site_launch_date	merchant_release_date	restock_date	fulfillment_latency	sale_price	sale_from_date	sale_end_date	offering_can_be_gift_messaged	offering_can_be_giftwrapped	is_discontinued_by_manufacturer	number_of_items	product_tax_code	merchant_shipping_group_name	website_shipping_weight	website_shipping_weight_unit_of_measure	item_display_height	item_display_height_unit_of_measure	item_display_length	item_display_length_unit_of_measure	item_display_width	item_display_width_unit_of_measure	item_display_weight	item_display_weight_unit_of_measure	bullet_point1	bullet_point2	bullet_point3	bullet_point4	bullet_point5	recommended_browse_nodes	generic_keywords	main_image_url	swatch_image_url	other_image_url1	other_image_url2	other_image_url3	parent_child	parent_sku	relationship_type	variation_theme	safety_warning	legal_disclaimer_description	color_name	grip_size	grip_type	hand_orientation	lens_color	item_shape	size_name	style_name	tension_level	golf_club_flex	golf_club_loft	shaft_length	shaft_length_unit_of_measure	shaft_material";

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(header1);
                sb.AppendLine(header2);
                sb.AppendLine(header3);

                if (data != null)
                {
                    foreach (var d in data)
                    {
                        try
                        {


                            //Nullable<double> price;
                            double price = 0;
                            double pricecal;
                            double pricemin = 0.00;
                            double finalprice;
                            string UPC;
                            if (d.UPC == null)
                            {
                                upc_codes uc = ukdb.upc_codes.Where(x => x.AssignedTo == null).FirstOrDefault();
                                UPC = uc.Upc;
                                UpdateUPC(uc.Upc, "NP" + d.ASIN);
                            }
                            else
                            {
                                UPC = d.UPC;
                            }
                            if (d.UpdatedSalesPrice != null)
                            {
                                price = double.Parse(d.UpdatedSalesPrice);
                            }
                            else
                                if (d.SalesPrice != null)
                                {
                                    price = double.Parse(d.SalesPrice);
                                }

                            if (price != 0)
                            {
                                price = double.Parse(d.SalesPrice);

                                if (price < 300.01)
                                {

                                    double minval = 0.00;

                                    if (price > 0 && price <= 19.99)
                                    {
                                        pricemin = 92.99;
                                    }
                                    else if (price > 19.99 && price <= 29.99)
                                    {
                                        pricemin = 120.99;
                                    }
                                    else if (price > 29.99 && price <= 39.99)
                                    {
                                        pricemin = 142.99;
                                    }
                                    else if (price > 39.99 && price <= 49.99)
                                    {
                                        pricemin = 168.99;
                                    }
                                    else if (price > 49.99 && price <= 59.99)
                                    {
                                        pricemin = 192.99;
                                    }
                                    else if (price > 59.99 && price <= 69.99)
                                    {
                                        pricemin = 222.99;
                                    }
                                    else if (price > 69.99 && price <= 79.99)
                                    {
                                        pricemin = 255.99;
                                    }
                                    else if (price > 79.99 && price <= 89.99)
                                    {
                                        pricemin = 285.99;
                                    }
                                    else if (price > 89.99 && price <= 99.99)
                                    {
                                        pricemin = 313.99;
                                    }
                                    else if (price > 99.99 && price <= 109.99)
                                    {
                                        pricemin = 327.99;
                                    }
                                    else if (price > 109.99 && price <= 119.99)
                                    {
                                        pricemin = 356.99;
                                    }
                                    else if (price > 119.99 && price <= 129.99)
                                    {
                                        pricemin = 385.99;
                                    }
                                    else if (price > 129.99 && price <= 139.99)
                                    {
                                        pricemin = 420.99;
                                    }
                                    else if (price > 139.99 && price <= 149.99)
                                    {
                                        pricemin = 442.99;
                                    }
                                    else if (price > 149.99 && price <= 159.99)
                                    {
                                        pricemin = 463.99;
                                    }
                                    else if (price > 159.99 && price <= 169.99)
                                    {
                                        pricemin = 485.99;
                                    }
                                    else if (price > 169.99 && price <= 179.99)
                                    {
                                        pricemin = 513.99;
                                    }
                                    else if (price > 179.99 && price <= 189.99)
                                    {
                                        pricemin = 542.99;
                                    }
                                    else if (price > 189.99 && price <= 199.99)
                                    {
                                        pricemin = 569.99;
                                    }



                                    pricecal = price * PriceValue;
                                    if (pricemin >= pricecal)
                                    {
                                        finalprice = pricemin;
                                    }
                                    else
                                    {
                                        finalprice = pricecal;
                                    }


                                    string ItemName;
                                    if (d.Title != null)
                                    {
                                        // ItemName = new string(d.Title.Take(490).ToArray());
                                        ItemName = d.Title.Substring(0, d.Title.Length > 499 ? 499 : d.Title.Length);
                                    }
                                    else
                                    {
                                        ItemName = null;
                                    }
                                    string description;

                                    if (d.Description != null)
                                    {
                                        string desc = Regex.Replace(d.Description, "<.*?>", String.Empty);
                                        //description = new string(desc.Take(1990).ToArray());
                                        description = desc.Substring(0, desc.Length > 1990 ? 1990 : desc.Length);
                                    }
                                    else
                                    {
                                        description = null;
                                    }

                                    string manufacturer;
                                    string brand;
                                    if (d.Manufacturer == null && d.Brand == null)
                                    {
                                        manufacturer = "Unknown";
                                        brand = "Unknown";
                                    }
                                    else
                                    {
                                        if (d.Manufacturer == null)
                                        {
                                            manufacturer = d.Brand;
                                        }
                                        else
                                        {
                                            manufacturer = d.Manufacturer;
                                        }

                                        if (d.Brand == null)
                                        {
                                            brand = d.Manufacturer;
                                        }
                                        else
                                        {
                                            brand = d.Brand;
                                        }

                                    }
                                    string feature1, feature2, feature3, feature4;
                                    if (d.Features1 != null)
                                    {
                                        // feature1 = new string(d.Features1.Take(500).ToArray());
                                        feature1 = d.Features1.Substring(0, d.Features1.Length > 500 ? 500 : d.Features1.Length);

                                    }
                                    else
                                        feature1 = d.Features1;
                                    if (d.Features2 != null)
                                    {
                                        // feature2 = new string(d.Features2.Take(500).ToArray());
                                        feature2 = d.Features2.Substring(0, d.Features2.Length > 500 ? 500 : d.Features2.Length);
                                    }
                                    else
                                        feature2 = d.Features2;
                                    if (d.Features3 != null)
                                    {
                                        //feature3 = new string(d.Features3.Take(500).ToArray());
                                        feature3 = d.Features3.Substring(0, d.Features3.Length > 500 ? 500 : d.Features3.Length);
                                    }
                                    else
                                        feature3 = d.Features3;
                                    if (d.Features4 != null)
                                    {
                                        // feature4 = new string(d.Features4.Take(500).ToArray());
                                        feature4 = d.Features4.Substring(0, d.Features4.Length > 500 ? 500 : d.Features4.Length);
                                    }
                                    else
                                        feature4 = d.Features4;


                                    sb.AppendLine(string.Join("\t",
                                                   string.Format(@"""{0}""", "NP-" + d.ASIN.Trim()),
                                                   string.Format(@"""{0}""", UPC),
                                                   string.Format(@"""{0}""", "UPC"),
                                                   string.Format(@"""{0}""", ItemName),
                                                   string.Format(@"""{0}""", description),
                                                   string.Format(@"""{0}""", manufacturer),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", "SportingGoods"),
                                                   string.Format(@"""{0}""", brand),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", String.Format("{0:0.00}", finalprice)),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", Quantity),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", FulfillmentLatency),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", feature1),
                                                   string.Format(@"""{0}""", feature2),
                                                   string.Format(@"""{0}""", feature3),
                                                   string.Format(@"""{0}""", feature4),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", d.LargeImageUrl),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", "")));

                                    d.FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_Not_Prime_Canada_Sports_" + canadaSport;

                                    d.ExportDate = DateTime.Now;

                                    if (shortcode == ConstantData.DC)
                                    {
                                        d.Account3_Status = 1;
                                        d.Account3_ExportDate = DateTime.Now;
                                        Exportpath = d.Account3_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_DC_Not_Prime_Canada_Sports_" + canadaSport;
                                    }

                                    d.Status = 1;
                                    d.Instock = 1;

                                }


                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    ukdb.ObjectContext().SaveChanges(SaveOptions.DetectChangesBeforeSave);

                    //ukdb.SaveChanges();
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=" + Exportpath + ".txt");
                    //Response.AddHeader("content-disposition", "attachment;filename="+name +"_"+ DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
                    Response.ContentType = "application/text";
                    Response.ContentEncoding = Encoding.GetEncoding(1252);

                    canadaSport++;

                    Response.Write(sb.ToString());
                    Response.End();

                }
                transaction.Complete();
                ukdb.ObjectContext().AcceptAllChanges();
            }

        }

        private void ExportBeautyCanadaNotPrime(double PriceValue, string shortcode, IEnumerable<tbl_BeautyNotPrime> data)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Suppress))
            {


                string Exportpath = "";
                ((IObjectContextAdapter)ukdb).ObjectContext.CommandTimeout = 1800;

                StringWriter st = new StringWriter();
                string header1 = "TemplateType=Beauty	Version=2015.1204	The top 3 rows are for Amazon.com use only. Do not modify or delete the top 3 rows.								Offer - Offer Information - These attributes are required to make your item buyable for customers on the site.																		Dimensions - Product Dimensions - These attributes specify the size and weight of a product.								Discovery - Item discovery information - These attributes have an effect on how customers can find your product on the site using browse or search.								Images - Image Information - See Image Instructions tab for details.					Fulfillment - Use these columns to provide fulfillment-related information for either Amazon-fulfilled (FBA) or seller-fulfilled orders.	Variation - Variation information - Populate these attributes if your product is available in different variations (for example colour or wattage).				Compliance - Compliance Information - Attributes used to comply with consumer laws in the country or region where the item is sold.		Ungrouped - These attributes create rich product listings for your buyers.";
                string header2 = "SKU	Product ID	Product ID Type	Product Name	Brand Name	Manufacturer	Manufacturer Part Number	Product Description	Product Type	Update Delete	Quantity	Standard Price	Max Order Quantity	Fulfillment Latency	Restock Date	Is Discontinued by Manufacturer	Max Aggregate Ship Quantity	Product Tax Code	Launch Date	Release Date	Manufacturer's Suggested Retail Price	Sale Price	Sale Start Date	Sale End Date	Package Quantity	Offering Can Be Gift Messaged	Is Gift Wrap Available	Shipping-Template	Website Shipping Weight Unit Of Measure	Shipping Weight	Item Weight Unit Of Measure	Item Weight	Item Length Unit Of Measure	Item Length	Item Width	Item Height	Key Product Features1	Key Product Features2	Key Product Features3	Key Product Features4	Key Product Features5	recommended-browse-nodes	Target Audience	Search Terms	Main Image URL	Swatch Image URL	Other Image URL1	Other Image URL2	Other Image URL3	Fulfillment Center ID	Parentage	Parent SKU	Relationship Type	Variation Theme	Legal Disclaimer	Safety Warning	Size	Color	Scent Name	Skin Type	Coverage	Material Type	Hair Type	Target Gender	Item Form	Specialty	Unit Count Type	Batteries are Included	Battery Type	Number of Batteries Required	Lithium Battery Packaging	Lithium Battery Voltage	Lithium Battery Weight	Colour Map";
                string header3 = "item_sku	external_product_id	external_product_id_type	item_name	brand_name	manufacturer	part_number	product_description	feed_product_type	update_delete	quantity	standard_price	max_order_quantity	fulfillment_latency	restock_date	is_discontinued_by_manufacturer	max_aggregate_ship_quantity	product_tax_code	product_site_launch_date	merchant_release_date	list_price	sale_price	sale_from_date	sale_end_date	item_package_quantity	offering_can_be_gift_messaged	offering_can_be_giftwrapped	merchant_shipping_group_name	website_shipping_weight_unit_of_measure	website_shipping_weight	item_weight_unit_of_measure	item_weight	item_length_unit_of_measure	item_length	item_width	item_height	bullet_point1	bullet_point2	bullet_point3	bullet_point4	bullet_point5	recommended_browse_nodes	target_audience_keywords	generic_keywords	main_image_url	swatch_image_url	other_image_url1	other_image_url2	other_image_url3	fulfillment_center_id	parent_child	parent_sku	relationship_type	variation_theme	legal_disclaimer_description	safety_warning	size_name	color_name	scent_name	skin_type	coverage	material_type	hair_type	target_gender	item_form	specialty	unit_count_type	are_batteries_included	battery_type	number_of_batteries	lithium_battery_packaging	lithium_battery_voltage	lithium_battery_weight	color_map";

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(header1);
                sb.AppendLine(header2);
                sb.AppendLine(header3);

                if (data != null)
                {
                    foreach (var d in data)
                    {
                        try
                        {


                            double price = 0;
                            double pricecal;
                            double pricemin = 0.00;
                            double finalprice;
                            string UPC;
                            if (d.UPC == null)
                            {
                                upc_codes uc = ukdb.upc_codes.Where(x => x.AssignedTo == null).FirstOrDefault();
                                UPC = uc.Upc;
                                UpdateUPC(uc.Upc, "NP" + d.ASIN);
                            }
                            else
                            {
                                UPC = d.UPC;
                            }
                            if (d.UpdatedSalesPrice != null)
                            {
                                price = double.Parse(d.UpdatedSalesPrice);
                            }
                            else if (d.SalesPrice != null)
                            {
                                price = double.Parse(d.SalesPrice);
                            }
                            if (price != 0)
                            {


                                if (price < 200.01)
                                {

                                    double minval = 0.00;

                                    if (price > 0 && price <= 19.99)
                                    {
                                        pricemin = 92.99;
                                    }
                                    else if (price > 19.99 && price <= 29.99)
                                    {
                                        pricemin = 120.99;
                                    }
                                    else if (price > 29.99 && price <= 39.99)
                                    {
                                        pricemin = 142.99;
                                    }
                                    else if (price > 39.99 && price <= 49.99)
                                    {
                                        pricemin = 168.99;
                                    }
                                    else if (price > 49.99 && price <= 59.99)
                                    {
                                        pricemin = 192.99;
                                    }
                                    else if (price > 59.99 && price <= 69.99)
                                    {
                                        pricemin = 222.99;
                                    }
                                    else if (price > 69.99 && price <= 79.99)
                                    {
                                        pricemin = 255.99;
                                    }
                                    else if (price > 79.99 && price <= 89.99)
                                    {
                                        pricemin = 285.99;
                                    }
                                    else if (price > 89.99 && price <= 99.99)
                                    {
                                        pricemin = 313.99;
                                    }
                                    else
                                        if (price > 99.99 && price <= 109.99)
                                        {
                                            pricemin = 327.99;
                                        }
                                        else if (price > 109.99 && price <= 119.99)
                                        {
                                            pricemin = 356.99;
                                        }
                                        else if (price > 119.99 && price <= 129.99)
                                        {
                                            pricemin = 385.99;
                                        }
                                        else if (price > 129.99 && price <= 139.99)
                                        {
                                            pricemin = 420.99;
                                        }
                                        else if (price > 139.99 && price <= 149.99)
                                        {
                                            pricemin = 442.99;
                                        }
                                        else if (price > 149.99 && price <= 159.99)
                                        {
                                            pricemin = 463.99;
                                        }
                                        else if (price > 159.99 && price <= 169.99)
                                        {
                                            pricemin = 485.99;
                                        }
                                        else if (price > 169.99 && price <= 179.99)
                                        {
                                            pricemin = 513.99;
                                        }
                                        else if (price > 179.99 && price <= 189.99)
                                        {
                                            pricemin = 542.99;
                                        }
                                        else if (price > 189.99 && price <= 199.99)
                                        {
                                            pricemin = 569.99;
                                        }

                                    pricecal = price * PriceValue;
                                    if (pricemin >= pricecal)
                                    {
                                        finalprice = pricemin;
                                    }
                                    else
                                    {
                                        finalprice = pricecal;
                                    }



                                    string ItemName;
                                    if (d.Title != null)
                                    {
                                        // ItemName = new string(d.Title.Take(490).ToArray());
                                        ItemName = d.Title.Substring(0, d.Title.Length > 499 ? 499 : d.Title.Length);
                                    }
                                    else
                                    {
                                        ItemName = null;
                                    }
                                    string description;

                                    if (d.Description != null)
                                    {
                                        string desc = Regex.Replace(d.Description, "<.*?>", String.Empty);
                                        //description = new string(desc.Take(1990).ToArray());
                                        description = desc.Substring(0, desc.Length > 1990 ? 1990 : desc.Length);
                                    }
                                    else
                                    {
                                        description = null;
                                    }

                                    string manufacturer;
                                    string brand;
                                    if (d.Manufacturer == null && d.Brand == null)
                                    {
                                        manufacturer = "Unknown";
                                        brand = "Unknown";
                                    }
                                    else
                                    {
                                        if (d.Manufacturer == null)
                                        {
                                            manufacturer = d.Brand;
                                        }
                                        else
                                        {
                                            manufacturer = d.Manufacturer;
                                        }

                                        if (d.Brand == null)
                                        {
                                            brand = d.Manufacturer;
                                        }
                                        else
                                        {
                                            brand = d.Brand;
                                        }

                                    }
                                    string feature1, feature2, feature3, feature4;
                                    if (d.Features1 != null)
                                    {
                                        // feature1 = new string(d.Features1.Take(500).ToArray());
                                        feature1 = d.Features1.Substring(0, d.Features1.Length > 500 ? 500 : d.Features1.Length);

                                    }
                                    else
                                        feature1 = d.Features1;
                                    if (d.Features2 != null)
                                    {
                                        // feature2 = new string(d.Features2.Take(500).ToArray());
                                        feature2 = d.Features2.Substring(0, d.Features2.Length > 500 ? 500 : d.Features2.Length);
                                    }
                                    else
                                        feature2 = d.Features2;
                                    if (d.Features3 != null)
                                    {
                                        //feature3 = new string(d.Features3.Take(500).ToArray());
                                        feature3 = d.Features3.Substring(0, d.Features3.Length > 500 ? 500 : d.Features3.Length);
                                    }
                                    else
                                        feature3 = d.Features3;
                                    if (d.Features4 != null)
                                    {
                                        // feature4 = new string(d.Features4.Take(500).ToArray());
                                        feature4 = d.Features4.Substring(0, d.Features4.Length > 500 ? 500 : d.Features4.Length);
                                    }
                                    else
                                        feature4 = d.Features4;

                                    sb.AppendLine(string.Join("\t",
                                                   string.Format(@"""{0}""", "NP-" + d.ASIN.Trim()),
                                                   string.Format(@"""{0}""", UPC),
                                                   string.Format(@"""{0}""", "UPC"),
                                                   string.Format(@"""{0}""", ItemName),
                                                   string.Format(@"""{0}""", brand),
                                                   string.Format(@"""{0}""", manufacturer),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", description),
                                                   string.Format(@"""{0}""", "BeautyMisc"),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", Quantity),
                                                   string.Format(@"""{0}""", String.Format("{0:0.00}", finalprice)),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", FulfillmentLatency),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", feature1),
                                                   string.Format(@"""{0}""", feature2),
                                                   string.Format(@"""{0}""", feature3),
                                                   string.Format(@"""{0}""", feature4),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", d.LargeImageUrl),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", "")));

                                    d.FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_Not_Prime_Canada_Beauty_" + canadabeauty;

                                    d.ExportDate = DateTime.Now;
                                    d.Instock = 1;

                                    if (shortcode == ConstantData.DC)
                                    {
                                        d.Account3_Status = 1;
                                        d.Account3_ExportDate = DateTime.Now;
                                        Exportpath = d.Account3_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_DC_Not_Prime_Canada_Beauty_" + canadabeauty;
                                    }
                                    d.Status = 1;


                                }
                            }
                        }
                        catch (Exception ex)
                        {


                        }
                    }
                    ukdb.ObjectContext().SaveChanges(SaveOptions.DetectChangesBeforeSave);

                    //ukdb.SaveChanges();
                    canadabeauty++;
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=" + Exportpath + ".txt");
                    //Response.AddHeader("content-disposition", "attachment;filename="+name +"_"+ DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
                    Response.ContentType = "application/text";
                    Response.ContentEncoding = Encoding.GetEncoding(1252);
                    Response.Write(sb.ToString());
                    Response.End();
                }
                transaction.Complete();
                ukdb.ObjectContext().AcceptAllChanges();
            }
        }

        private void ExportJewelryNotPrime(double PriceValue, string shortcode, IEnumerable<tbl_JewelleryNotPrime> data)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Suppress))
            {


                string Exportpath = "";
                ((IObjectContextAdapter)ukdb).ObjectContext.CommandTimeout = 1800;

                StringWriter st = new StringWriter();
                string header1 = "TemplateType=jewelry	Version=2015.1216	The top 3 rows are for Amazon.com use only. Do not modify or delete the top 3 rows.									Offer-Offer Information - These attributes are required to make your item buyable for customers on the site.																Dimensions-Product Dimensions - These attributes specify the size and weight of a product.					Discovery-Item discovery information - These attributes have an effect on how customers can find your product on the site using browse or search.					Images-Image Information - See Image Instructions tab for details.					Fulfillment-Use these columns to provide fulfilment-related information for orders fulfilled either by Amazon (FBA) or by the Seller.									Variation-Variation information - Populate these attributes if your product is available in different variations (for example colour or wattage).				Ungrouped - These attributes create rich product listings for your buyers.";
                string header2 = "Seller SKU	Item Name	Manufacturer	Model	Feed Product Type	Brand Name	Product ID	Product ID Type	Product Description	Update Delete	Manufacturer Part Number	Standard Price	Quantity	Launch Date	Release Date	Condition Type	Condition Note	Product Tax Code	Sale Price	Sale From Date	Sale End Date	Fulfillment Latency	Max Aggregate Ship Quantity	Can Be Gift Messaged	Is Gift Wrap Available?	Is Discontinued By Manufacturer	Merchant Shipping Group	Website Shipping Weight	Website Shipping Weight Unit Of Measure	Display Dimensions Unit Of Measure	Item Display Width	Display Length	Recommended Browse Nodes	Bullet Point	Bullet Point	Bullet Point	Search Terms	Main Image URL	Swatch Image Url	Other Image Url	Other Image Url	Other Image Url	Fulfillment Centre ID	Package Width Unit Of Measure	Package Width	Package Weight Unit Of Measure	Package weight	Package Length Unit Of Measure	Package Length	Package Height Unit of Measure	Package Height	Parentage	Parent SKU	Relationship Type	Variation Theme	Total Diamond Weight	Metal Type	Metal Stamp	Ring Size	Ring Sizing Lower Range	Ring Sizing Upper Range	Gem Type	Gem Type	Stone Colour	Stone Colour	Stone Shape	Stone Shape	Size Per Pearl	Material	Theme	Occasion Type	Earring Type	Colour Map	Clasp Type	Chain Type	Color	Back Finding";
                string header3 = "item_sku	item_name	manufacturer	model	feed_product_type	brand_name	external_product_id	external_product_id_type	product_description	update_delete	part_number	standard_price	quantity	product_site_launch_date	merchant_release_date	condition_type	condition_note	product_tax_code	sale_price	sale_from_date	sale_end_date	fulfillment_latency	max_aggregate_ship_quantity	offering_can_be_gift_messaged	offering_can_be_giftwrapped	is_discontinued_by_manufacturer	merchant_shipping_group_name	website_shipping_weight	website_shipping_weight_unit_of_measure	display_dimensions_unit_of_measure	item_display_width	item_display_length	recommended_browse_nodes	bullet_point1	bullet_point2	bullet_point3	generic_keywords	main_image_url	swatch_image_url	other_image_url1	other_image_url2	other_image_url3	fulfillment_center_id	package_width_unit_of_measure	package_width	package_weight_unit_of_measure	package_weight	package_length_unit_of_measure	package_length	package_height_unit_of_measure	package_height	parent_child	parent_sku	relationship_type	variation_theme	total_diamond_weight	metal_type	metal_stamp	ring_size	ring_sizing_lower_range	ring_sizing_upper_range	gem_type1	gem_type2	stone_color1	stone_color2	stone_shape1	stone_shape2	size_per_pearl	material_type	theme	occasion_type	item_shape	color_map	clasp_type	chain_type	color_name	back_finding";

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(header1);
                sb.AppendLine(header2);
                sb.AppendLine(header3);
                double addToMinVal = double.Parse(ConfigurationManager.AppSettings["ProdPriceAddToMinValUK"]);// 27-June-2016 making is configurable. It adds vaue accoding to range define in loop.

                //IEnumerable<tbl_Beauty> data = ukdb.tbl_Beauty.Where(x => x.UPC != "null" && x.Status == 0 && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(count.ToString())).ToList();
                if (data != null)
                {
                    foreach (var d in data)
                    {
                        try
                        {


                            double price = 0;
                            double pricecal;
                            double pricemin = 0.00;
                            double finalprice;
                            string UPC;
                            if (d.UPC == null)
                            {
                                upc_codes uc = ukdb.upc_codes.Where(x => x.AssignedTo == null).FirstOrDefault();
                                UPC = uc.Upc;
                                UpdateUPC(uc.Upc, "NP" + d.ASIN);
                            }
                            else
                            {
                                UPC = d.UPC;
                            }
                            if (d.UpdatedSalesPrice != null)
                            {
                                price = double.Parse(d.UpdatedSalesPrice);
                            }
                            else
                                if (d.SalesPrice != null)
                                {
                                    price = double.Parse(d.SalesPrice);
                                }

                            if (price != 0)
                            {

                                double minval = double.Parse(ConfigurationManager.AppSettings["ProdPriceMinValUK"]);  // 27-June-2016 making it configurable. Minimum Product Price can Change from configuration file. 


                                if (price > 0 && price <= 19.99)
                                {
                                    pricemin = minval;
                                }
                                else
                                {
                                    for (double i = 20; i < 500; i += 10)
                                    {
                                        minval = minval + addToMinVal;
                                        double temp = i + 10;
                                        if (price >= i && price < temp)
                                        {
                                            pricemin = minval;
                                            break;
                                        }
                                    }
                                }
                                pricecal = price * PriceValue;
                                if (pricemin >= pricecal)
                                {
                                    finalprice = pricemin;
                                }
                                else
                                {
                                    finalprice = pricecal;
                                }





                                string ItemName;
                                if (d.Title != null)
                                {
                                    // ItemName = new string(d.Title.Take(490).ToArray());
                                    ItemName = d.Title.Substring(0, d.Title.Length > 499 ? 499 : d.Title.Length);
                                }
                                else
                                {
                                    ItemName = null;
                                }
                                string description;

                                if (d.Description != null)
                                {
                                    string desc = Regex.Replace(d.Description, "<.*?>", String.Empty);
                                    //description = new string(desc.Take(1990).ToArray());
                                    description = desc.Substring(0, desc.Length > 1990 ? 1990 : desc.Length);
                                }
                                else
                                {
                                    description = null;
                                }

                                string manufacturer;
                                string brand;
                                if (d.Manufacturer == null && d.Brand == null)
                                {
                                    manufacturer = "Unknown";
                                    brand = "Unknown";
                                }
                                else
                                {
                                    if (d.Manufacturer == null)
                                    {
                                        manufacturer = d.Brand;
                                    }
                                    else
                                    {
                                        manufacturer = d.Manufacturer;
                                    }

                                    if (d.Brand == null)
                                    {
                                        brand = d.Manufacturer;
                                    }
                                    else
                                    {
                                        brand = d.Brand;
                                    }

                                }
                                string feature1, feature2, feature3, feature4;
                                if (d.Features1 != null)
                                {
                                    // feature1 = new string(d.Features1.Take(500).ToArray());
                                    feature1 = d.Features1.Substring(0, d.Features1.Length > 500 ? 500 : d.Features1.Length);

                                }
                                else
                                    feature1 = d.Features1;
                                if (d.Features2 != null)
                                {
                                    // feature2 = new string(d.Features2.Take(500).ToArray());
                                    feature2 = d.Features2.Substring(0, d.Features2.Length > 500 ? 500 : d.Features2.Length);
                                }
                                else
                                    feature2 = d.Features2;
                                if (d.Features3 != null)
                                {
                                    //feature3 = new string(d.Features3.Take(500).ToArray());
                                    feature3 = d.Features3.Substring(0, d.Features3.Length > 500 ? 500 : d.Features3.Length);
                                }
                                else
                                    feature3 = d.Features3;
                                if (d.Features4 != null)
                                {
                                    // feature4 = new string(d.Features4.Take(500).ToArray());
                                    feature4 = d.Features4.Substring(0, d.Features4.Length > 500 ? 500 : d.Features4.Length);
                                }
                                else
                                    feature4 = d.Features4;

                                sb.AppendLine(string.Join("\t",
                                               string.Format(@"""{0}""", "NP-" + d.ASIN.Trim()),
                                               string.Format(@"""{0}""", ItemName),
                                               string.Format(@"""{0}""", manufacturer),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", "FashionOther"),
                                               string.Format(@"""{0}""", brand),
                                               string.Format(@"""{0}""", UPC),
                                               string.Format(@"""{0}""", "UPC"),
                                               string.Format(@"""{0}""", description),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", d.UPC),
                                               string.Format(@"""{0}""", String.Format("{0:0.00}", finalprice)),
                                               string.Format(@"""{0}""", Quantity),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", FulfillmentLatency),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", "193717031"),
                                               string.Format(@"""{0}""", feature1),
                                               string.Format(@"""{0}""", feature2),
                                               string.Format(@"""{0}""", feature3),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", d.LargeImageUrl),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", "")));

                                d.FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_Not_Prime_Jewelry_" + fcountWatches;

                                d.ExportDate = DateTime.Now;
                                d.Instock = 1;
                                if (shortcode == ConstantData.ED)
                                {
                                    d.Account1_Status = 1;
                                    d.Account1_AmazonActive = 1;
                                    d.Account1_ExportDate = DateTime.Now;
                                    Exportpath = d.Account1_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_ED_Not_Prime_Jewelry_" + fcountJewelry;
                                }
                                else if (shortcode == ConstantData.EM)
                                {
                                    d.Account2_Status = 1;
                                    d.Account2_AmazonActive = 1;
                                    d.Account2_ExportDate = DateTime.Now;
                                    Exportpath = d.Account2_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_EM_Not_Prime_Jewelry_" + fcountJewelry;
                                }

                                else if (shortcode == ConstantData.DI)
                                {
                                    d.Account4_Status = 1;
                                    d.Account4_AmazonActive = 1;
                                    d.Account4_ExportDate = DateTime.Now;
                                    Exportpath = d.Account4_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_DI_Not_Prime_Jewelry_" + fcountJewelry;
                                }
                                d.Status = 1;



                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    ukdb.ObjectContext().SaveChanges(SaveOptions.DetectChangesBeforeSave);

                    //ukdb.SaveChanges();
                    fcountJewelry++;
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=" + Exportpath + ".txt");
                    //Response.AddHeader("content-disposition", "attachment;filename="+name +"_"+ DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
                    Response.ContentType = "application/text";
                    Response.ContentEncoding = Encoding.GetEncoding(1252);
                    Response.Write(sb.ToString());
                    Response.End();
                }
                transaction.Complete();
                ukdb.ObjectContext().AcceptAllChanges();
            }

        }

        private void ExportWatchesNotPrime(double PriceValue, string shortcode, IEnumerable<tbl_WatchesNotPrime> data)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Suppress))
            {



                string Exportpath = "";
                ((IObjectContextAdapter)ukdb).ObjectContext.CommandTimeout = 1800;

                StringWriter st = new StringWriter();
                string header1 = "TemplateType=Watches	Version=2015.1216	The top 3 rows are for Amazon.com use only. Do not modify or delete the top 3 rows.							Offer - Offer Information - These attributes are required to make your item buyable for customers on the site.												Dimensions - Product Dimensions - These attributes specify the size and weight of a product.		Discovery - Item discovery information - These attributes have an effect on how customers can find your product on the site using browse or search.								Images - Image Information - See Image Instructions tab for details.				Fulfillment - Use these columns to provide fulfilment-related information for orders fulfilled either by Amazon (FBA) or by the Seller.							Ungrouped - These attributes create rich product listings for your buyers.";
                string header2 = "Seller SKU	Product ID	Product ID Type	Item Name (aka Title)	Manufacturer	Manufacturer Part Number	Brand Name	Product Description	Update Delete	Standard Price	Offer Condition Note	Launch Date	Sale Price	Sale From Date	Sale End Date	Quantity	Fulfillment Latency	Can Be Gift Messaged	Is Gift Wrap Available?	Is Discontinued by Manufacturer	Merchant Shipping Group	Website Shipping Weight Unit Of Measure	Shipping Weight	Target Audience	Recommended Browse Nodes	Search Terms	Key Product Features1	Key Product Features2	Key Product Features3	Key Product Features4	Key Product Features5	Main Image URL	Other Image URL1	Other Image URL2	Other Image URL3	Fulfillment Centre ID	package-width	Package Weight Unit Of Measure	Package Weight	Package Length	Package Height	Package Dimensions Unit Of Measure	Display Type	Band Material Type	Watch Movement Type	Water Resistance Depth	Water Resistance Depth Unit Of Measure	Lifestyle	Item Shape	Band Width	Case Thickness	Case Diameter	Crystal	Dial Colour	Band Colour	Warranty Type	Bezel Material Type	Clasp Type	Sport Type";
                string header3 = "item_sku	external_product_id	external_product_id_type	item_name	manufacturer	part_number	brand_name	product_description	update_delete	standard_price	condition_note	product_site_launch_date	sale_price	sale_from_date	sale_end_date	quantity	fulfillment_latency	offering_can_be_gift_messaged	offering_can_be_giftwrapped	is_discontinued_by_manufacturer	merchant_shipping_group_name	website_shipping_weight_unit_of_measure	website_shipping_weight	target_audience_keywords	recommended_browse_nodes	generic_keywords	bullet_point1	bullet_point2	bullet_point3	bullet_point4	bullet_point5	main_image_url	other_image_url1	other_image_url2	other_image_url3	fulfillment_center_id	package_width	package_weight_unit_of_measure	package_weight	package_length	package_height	package_dimensions_unit_of_measure	display_type	band_material_type	watch_movement_type	water_resistance_depth	water_resistance_depth_unit_of_measure	lifestyle	item_shape	band_width	case_thickness	case_diameter	dial_window_material_type	dial_color	band_color	warranty_type	bezel_material_type	clasp_type	sport_type";

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(header1);
                sb.AppendLine(header2);
                sb.AppendLine(header3);
                double addToMinVal = double.Parse(ConfigurationManager.AppSettings["ProdPriceAddToMinValUK"]);// 27-June-2016 making is configurable. It adds vaue accoding to range define in loop.

                //IEnumerable<tbl_Beauty> data = ukdb.tbl_Beauty.Where(x => x.UPC != "null" && x.Status == 0 && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(count.ToString())).ToList();
                if (data != null)
                {
                    foreach (var d in data)
                    {
                        try
                        {


                            double price = 0;
                            double pricecal;
                            double pricemin = 0.00;
                            double finalprice;
                            string UPC;
                            if (d.UPC == null)
                            {
                                upc_codes uc = ukdb.upc_codes.Where(x => x.AssignedTo == null).FirstOrDefault();
                                UPC = uc.Upc;
                                UpdateUPC(uc.Upc, "NP" + d.ASIN);
                            }
                            else
                            {
                                UPC = d.UPC;
                            }
                            if (d.UpdatedSalesPrice != null)
                            {
                                price = double.Parse(d.UpdatedSalesPrice);
                            }
                            else
                                if (d.SalesPrice != null)
                                {
                                    price = double.Parse(d.SalesPrice);
                                }

                            if (price != 0)
                            {

                                double minval = double.Parse(ConfigurationManager.AppSettings["ProdPriceMinValUK"]);  // 27-June-2016 making it configurable. Minimum Product Price can Change from configuration file. 


                                if (price > 0 && price <= 19.99)
                                {
                                    pricemin = minval;
                                }
                                else
                                {
                                    for (double i = 20; i < 500; i += 10)
                                    {
                                        minval = minval + addToMinVal;
                                        double temp = i + 10;
                                        if (price >= i && price < temp)
                                        {
                                            pricemin = minval;
                                            break;
                                        }
                                    }
                                }
                                pricecal = price * PriceValue;
                                if (pricemin >= pricecal)
                                {
                                    finalprice = pricemin;
                                }
                                else
                                {
                                    finalprice = pricecal;
                                }




                                string ItemName;
                                if (d.Title != null)
                                {
                                    // ItemName = new string(d.Title.Take(490).ToArray());
                                    ItemName = d.Title.Substring(0, d.Title.Length > 499 ? 499 : d.Title.Length);
                                }
                                else
                                {
                                    ItemName = null;
                                }
                                string description;

                                if (d.Description != null)
                                {
                                    string desc = Regex.Replace(d.Description, "<.*?>", String.Empty);
                                    //description = new string(desc.Take(1990).ToArray());
                                    description = desc.Substring(0, desc.Length > 1990 ? 1990 : desc.Length);
                                }
                                else
                                {
                                    description = null;
                                }

                                string manufacturer;
                                string brand;
                                if (d.Manufacturer == null && d.Brand == null)
                                {
                                    manufacturer = "Unknown";
                                    brand = "Unknown";
                                }
                                else
                                {
                                    if (d.Manufacturer == null)
                                    {
                                        manufacturer = d.Brand;
                                    }
                                    else
                                    {
                                        manufacturer = d.Manufacturer;
                                    }

                                    if (d.Brand == null)
                                    {
                                        brand = d.Manufacturer;
                                    }
                                    else
                                    {
                                        brand = d.Brand;
                                    }

                                }
                                string feature1, feature2, feature3, feature4;
                                if (d.Features1 != null)
                                {
                                    // feature1 = new string(d.Features1.Take(500).ToArray());
                                    feature1 = d.Features1.Substring(0, d.Features1.Length > 500 ? 500 : d.Features1.Length);

                                }
                                else
                                    feature1 = d.Features1;
                                if (d.Features2 != null)
                                {
                                    // feature2 = new string(d.Features2.Take(500).ToArray());
                                    feature2 = d.Features2.Substring(0, d.Features2.Length > 500 ? 500 : d.Features2.Length);
                                }
                                else
                                    feature2 = d.Features2;
                                if (d.Features3 != null)
                                {
                                    //feature3 = new string(d.Features3.Take(500).ToArray());
                                    feature3 = d.Features3.Substring(0, d.Features3.Length > 500 ? 500 : d.Features3.Length);
                                }
                                else
                                    feature3 = d.Features3;
                                if (d.Features4 != null)
                                {
                                    // feature4 = new string(d.Features4.Take(500).ToArray());
                                    feature4 = d.Features4.Substring(0, d.Features4.Length > 500 ? 500 : d.Features4.Length);
                                }
                                else
                                    feature4 = d.Features4;

                                sb.AppendLine(string.Join("\t",
                                               string.Format(@"""{0}""", "NP-" + d.ASIN.Trim()),
                                               string.Format(@"""{0}""", UPC),
                                               string.Format(@"""{0}""", "UPC"),
                                               string.Format(@"""{0}""", ItemName),
                                               string.Format(@"""{0}""", manufacturer),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", brand),
                                               string.Format(@"""{0}""", description),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", String.Format("{0:0.00}", finalprice)),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", Quantity),
                                               string.Format(@"""{0}""", FulfillmentLatency),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", feature1),
                                               string.Format(@"""{0}""", feature2),
                                               string.Format(@"""{0}""", feature3),
                                               string.Format(@"""{0}""", feature4),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", d.LargeImageUrl),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", "")));

                                d.FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_Not_Prime_Watches_" + fcountWatches;

                                d.ExportDate = DateTime.Now;
                                d.Instock = 1;
                                if (shortcode == ConstantData.ED)
                                {
                                    d.Account1_Status = 1;
                                    d.Account1_AmazonActive = 1;
                                    d.Account1_ExportDate = DateTime.Now;
                                    Exportpath = d.Account1_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_ED_Not_Prime_Watches_" + fcountWatches;
                                }
                                else if (shortcode == ConstantData.EM)
                                {
                                    d.Account2_Status = 1;
                                    d.Account2_AmazonActive = 1;
                                    d.Account2_ExportDate = DateTime.Now;
                                    Exportpath = d.Account2_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_EM_Not_Prime_Watches_" + fcountWatches;
                                }

                                else if (shortcode == ConstantData.DI)
                                {
                                    d.Account4_Status = 1;
                                    d.Account4_AmazonActive = 1;
                                    d.Account4_ExportDate = DateTime.Now;
                                    Exportpath = d.Account4_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_DI_Not_Prime_Watches_" + fcountWatches;
                                }
                                d.Status = 1;


                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    ukdb.ObjectContext().SaveChanges(SaveOptions.DetectChangesBeforeSave);

                    //ukdb.SaveChanges();
                    fcountWatches++;
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=" + Exportpath + ".txt");
                    //Response.AddHeader("content-disposition", "attachment;filename="+name +"_"+ DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
                    Response.ContentType = "application/text";
                    Response.ContentEncoding = Encoding.GetEncoding(1252);
                    Response.Write(sb.ToString());
                    Response.End();
                }
                transaction.Complete();
                ukdb.ObjectContext().AcceptAllChanges();
            }
        }

        private void ExportBabyNotPrime(double PriceValue, string shortcode, IEnumerable<tbl_BabyNotPrime> data)
        {

            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Suppress))
            {


                string Exportpath = "";
                ((IObjectContextAdapter)ukdb).ObjectContext.CommandTimeout = 1800;

                StringWriter st = new StringWriter();
                string header1 = "TemplateType=Baby	Version=2015.1217	The top 3 rows are for Amazon.com use only. Do not modify or delete the top 3 rows.								Offer - Offer Information - These attributes are required to make your item buyable for customers on the site.																		Dimensions - Product Dimensions - These attributes specify the size and weight of a product.														Discovery - Item discovery information - These attributes have an effect on how customers can find your product on the site using browse or search.							Images - Image Information - See Image Instructions tab for details.					Fulfillment - Use these columns to provide fulfilment-related information for orders fulfilled either by Amazon (FBA) or by the Seller.							Variation - Variation information - Populate these attributes if your product is available in different variations (for example colour or wattage).				Ungrouped - These attributes create rich product listings for your buyers.";
                string header2 = "Seller SKU	Item Name (aka Title)	Product ID	Product ID Type	Feed Product Type	Brand Name	Manufacturer	Manufacturer Part Number	Product Description	Update Delete	Quantity	Standard Price	Condition Type	Offer Condition Note	Launch Date	Fulfillment Latency	Release Date	Sale Price	Sale From Date	Sale End Date	Number of Items	Stop Selling Date	Max Aggregate Ship Quantity	Product Tax Code	Can Be Gift Messaged	Is Gift Wrap Available?	Is Discontinued by Manufacturer	Merchant Shipping Group	Shipping Weight	Website Shipping Weight Unit Of Measure	Item Height	Item Length	Item Width	Item Length Unit Of Measure	Item Weight	Unit of measure of item weight	Display Weight	Item Display Weight Unit Of Measure	Display Volume	Item Display Volume Unit Of Measure	Display Length	Item Display Length Unit Of Measure	Key Product Features1	Key Product Features2	Key Product Features3	Key Product Features4	Key Product Features5	Recommended Browse Nodes	Search Terms	Main Image URL	Swatch Image Url	Other Image URL1	Other Image URL2	Other Image URL3	Fulfillment Centre ID	Package Height	Package Width	Package Length	Package Length Unit Of Measure	Package Weight	Package Weight Unit Of Measure	Parentage	Relationship Type	Parent SKU	Variation Theme	Colour	Colour Map	Size	Size Map	Manufacturer Warranty Description	Material Type	Product Care Instructions	Is Assembly Required	Assembly Instructions	Minimum Age Recommendation	Mfg Minimum Unit Of Measure	Maximum Age Recommendation	Mfg Maximum Unit Of Measure	Minimum Weight Recommendation	Minimum Weight Recommendation Unit Of Measure	Maximum Weight Recommendation	Maximum Weight Recommendation Unit Of Measure	Target Gender	Special Features	Material Composition	Language	Batteries are Included	BatteryType	Number of Batteries	Lithium Battery Voltage	Lithium Battery Weight	Lithium Battery Packaging";
                string header3 = "item_sku	item_name	external_product_id	external_product_id_type	feed_product_type	brand_name	manufacturer	part_number	product_description	update_delete	quantity	standard_price	condition_type	condition_note	product_site_launch_date	fulfillment_latency	merchant_release_date	sale_price	sale_from_date	sale_end_date	number_of_items	offering_end_date	max_aggregate_ship_quantity	product_tax_code	offering_can_be_gift_messaged	offering_can_be_giftwrapped	is_discontinued_by_manufacturer	merchant_shipping_group_name	website_shipping_weight	website_shipping_weight_unit_of_measure	item_height	item_length	item_width	item_length_unit_of_measure	item_weight	item_weight_unit_of_measure	item_display_weight	item_display_weight_unit_of_measure	item_display_volume	item_display_volume_unit_of_measure	item_display_length	item_display_length_unit_of_measure	bullet_point1	bullet_point2	bullet_point3	bullet_point4	bullet_point5	recommended_browse_nodes	generic_keywords	main_image_url	swatch_image_url	other_image_url1	other_image_url2	other_image_url3	fulfillment_center_id	package_height	package_width	package_length	package_length_unit_of_measure	package_weight	package_weight_unit_of_measure	parent_child	relationship_type	parent_sku	variation_theme	color_name	color_map	size_name	size_map	warranty_description	material_type	care_instructions	is_assembly_required	assembly_instructions	mfg_minimum	mfg_minimum_unit_of_measure	mfg_maximum	mfg_maximum_unit_of_measure	minimum_weight_recommendation	minimum_weight_recommendation_unit_of_measure	maximum_weight_recommendation	maximum_weight_recommendation_unit_of_measure	target_gender	special_features	material_composition	language_value	are_batteries_included	battery_type	number_of_batteries	lithium_battery_voltage	lithium_battery_weight	lithium_battery_packaging";

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(header1);
                sb.AppendLine(header2);
                sb.AppendLine(header3);
                double addToMinVal = double.Parse(ConfigurationManager.AppSettings["ProdPriceAddToMinValUK"]);// 27-June-2016 making is configurable. It adds vaue accoding to range define in loop.

                //IEnumerable<tbl_Beauty> data = ukdb.tbl_Beauty.Where(x => x.UPC != "null" && x.Status == 0 && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(count.ToString())).ToList();
                if (data != null)
                {
                    foreach (var d in data)
                    {
                        try
                        {


                            double price = 0;
                            double pricecal;
                            double pricemin = 0.00;
                            double finalprice;
                            string UPC;
                            if (d.UPC == null)
                            {
                                upc_codes uc = ukdb.upc_codes.Where(x => x.AssignedTo == null).FirstOrDefault();
                                UPC = uc.Upc;
                                UpdateUPC(uc.Upc, "NP" + d.ASIN);
                            }
                            else
                            {
                                UPC = d.UPC;
                            }
                            if (d.UpdatedSalesPrice != null)
                            {
                                price = double.Parse(d.UpdatedSalesPrice);
                            }
                            else
                                if (d.SalesPrice != null)
                                {
                                    price = double.Parse(d.SalesPrice);
                                }
                            if (price != 0)
                            {

                                double minval = double.Parse(ConfigurationManager.AppSettings["ProdPriceMinValUK"]);  // 27-June-2016 making it configurable. Minimum Product Price can Change from configuration file. 


                                if (price > 0 && price <= 19.99)
                                {
                                    pricemin = minval;
                                }
                                else
                                {
                                    for (double i = 20; i < 500; i += 10)
                                    {
                                        minval = minval + addToMinVal;
                                        double temp = i + 10;
                                        if (price >= i && price < temp)
                                        {
                                            pricemin = minval;
                                            break;
                                        }
                                    }
                                }
                                pricecal = price * PriceValue;
                                if (pricemin >= pricecal)
                                {
                                    finalprice = pricemin;
                                }
                                else
                                {
                                    finalprice = pricecal;
                                }



                                string ItemName;
                                if (d.Title != null)
                                {
                                    // ItemName = new string(d.Title.Take(490).ToArray());
                                    ItemName = d.Title.Substring(0, d.Title.Length > 499 ? 499 : d.Title.Length);
                                }
                                else
                                {
                                    ItemName = null;
                                }
                                string description;

                                if (d.Description != null)
                                {
                                    string desc = Regex.Replace(d.Description, "<.*?>", String.Empty);
                                    //description = new string(desc.Take(1990).ToArray());
                                    description = desc.Substring(0, desc.Length > 1990 ? 1990 : desc.Length);
                                }
                                else
                                {
                                    description = null;
                                }

                                string manufacturer;
                                string brand;
                                if (d.Manufacturer == null && d.Brand == null)
                                {
                                    manufacturer = "Unknown";
                                    brand = "Unknown";
                                }
                                else
                                {
                                    if (d.Manufacturer == null)
                                    {
                                        manufacturer = d.Brand;
                                    }
                                    else
                                    {
                                        manufacturer = d.Manufacturer;
                                    }

                                    if (d.Brand == null)
                                    {
                                        brand = d.Manufacturer;
                                    }
                                    else
                                    {
                                        brand = d.Brand;
                                    }

                                }
                                string feature1, feature2, feature3, feature4;
                                if (d.Features1 != null)
                                {
                                    // feature1 = new string(d.Features1.Take(500).ToArray());
                                    feature1 = d.Features1.Substring(0, d.Features1.Length > 500 ? 500 : d.Features1.Length);

                                }
                                else
                                    feature1 = d.Features1;
                                if (d.Features2 != null)
                                {
                                    // feature2 = new string(d.Features2.Take(500).ToArray());
                                    feature2 = d.Features2.Substring(0, d.Features2.Length > 500 ? 500 : d.Features2.Length);
                                }
                                else
                                    feature2 = d.Features2;
                                if (d.Features3 != null)
                                {
                                    //feature3 = new string(d.Features3.Take(500).ToArray());
                                    feature3 = d.Features3.Substring(0, d.Features3.Length > 500 ? 500 : d.Features3.Length);
                                }
                                else
                                    feature3 = d.Features3;
                                if (d.Features4 != null)
                                {
                                    // feature4 = new string(d.Features4.Take(500).ToArray());
                                    feature4 = d.Features4.Substring(0, d.Features4.Length > 500 ? 500 : d.Features4.Length);
                                }
                                else
                                    feature4 = d.Features4;

                                sb.AppendLine(string.Join("\t",
                                               string.Format(@"""{0}""", "NP-" + d.ASIN.Trim()),
                                               string.Format(@"""{0}""", ItemName),
                                               string.Format(@"""{0}""", UPC),
                                               string.Format(@"""{0}""", "UPC"),
                                               string.Format(@"""{0}""", "BabyProducts"),
                                               string.Format(@"""{0}""", brand),
                                               string.Format(@"""{0}""", manufacturer),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", description),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", Quantity),
                                               string.Format(@"""{0}""", String.Format("{0:0.00}", finalprice)),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", FulfillmentLatency),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", feature1),
                                               string.Format(@"""{0}""", feature2),
                                               string.Format(@"""{0}""", feature3),
                                               string.Format(@"""{0}""", feature4),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", d.LargeImageUrl),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", "")));

                                d.FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_Not_Prime_Baby_" + fcountBaby;

                                d.ExportDate = DateTime.Now;
                                d.Instock = 1;
                                if (shortcode == ConstantData.ED)
                                {
                                    d.Account1_Status = 1;
                                    d.Account1_AmazonActive = 1;
                                    d.Account1_ExportDate = DateTime.Now;
                                    Exportpath = d.Account1_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_ED_Not_Prime_Baby_" + fcountBaby;
                                }
                                else if (shortcode == ConstantData.EM)
                                {
                                    d.Account2_Status = 1;
                                    d.Account2_AmazonActive = 1;
                                    d.Account2_ExportDate = DateTime.Now;
                                    Exportpath = d.Account2_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_EM_Not_Prime_Baby_" + fcountBaby;
                                }

                                else if (shortcode == ConstantData.DI)
                                {
                                    d.Account4_Status = 1;
                                    d.Account4_AmazonActive = 1;
                                    d.Account4_ExportDate = DateTime.Now;
                                    Exportpath = d.Account4_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_DI_Not_Prime_Baby_" + fcountBaby;
                                }
                                d.Status = 1;


                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    // ukdb.SaveChanges();
                    ukdb.ObjectContext().SaveChanges(SaveOptions.DetectChangesBeforeSave);

                    fcountBaby++;
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=" + Exportpath + ".txt");
                    //Response.AddHeader("content-disposition", "attachment;filename="+name +"_"+ DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
                    Response.ContentType = "application/text";
                    Response.ContentEncoding = Encoding.GetEncoding(1252);
                    Response.Write(sb.ToString());
                    Response.End();
                }
                transaction.Complete();
                ukdb.ObjectContext().AcceptAllChanges();
            }
        }

        private void ExportForUKNotPrime(double PriceValue, string shortcode, IEnumerable<tbl_SportsNotPrime> data)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Suppress))
            {
                string Exportpath = "";

                ((IObjectContextAdapter)ukdb).ObjectContext.CommandTimeout = 1800;


                string header1 = "TemplateType=sports	Version=2016.0505	The top 3 rows are for Amazon.com use only. Do not modify or delete the top 3 rows.								Offer-Offer Information - These attributes are required to make your item buyable for customers on the site.																			Dimensions-Product Dimensions - These attributes specify the size and weight of a product.												Discovery-Item discovery information - These attributes have an effect on how customers can find your product on the site using browse or search.							Images-Image Information - See Image Instructions tab for details.										Fulfillment-Use these columns to provide fulfilment-related information for orders fulfilled either by Amazon (FBA) or by the Seller.							Variation-Variation information - Populate these attributes if your product is available in different variations (for example colour or wattage).				Ungrouped - These attributes create rich product listings for your buyers.";
                string header2 = "Seller SKU	Product ID	Product ID Type	Item Name	Product Description	Manufacturer	Manufacturer Part Number	Product Type	Brand Name	Update Delete	Standard Price	Item Condition	Offer Condition Note	Quantity	Recommended Retail Price	Launch Date	Release Date	Fulfillment Latency	Product Tax Code	Sale Price	Sale From Date	Sale End Date	Package Quantity	Number of Items	Max Aggregate Ship Quantity	Can Be Gift Messaged	Is Gift Wrap Available?	Is Discontinued by Manufacturer	Merchant Shipping Group	Shipping Weight	Website Shipping Weight Unit Of Measure	Display Length	Item Display Length Unit Of Measure	Display Weight	Item Display Weight Unit Of Measure	Item Height	Item Length	Item Width	Item Weight	Item Weight Unit Of Measure	Item Dimensions Unit Of Measure	Key Product Features	Key Product Features	Key Product Features	Key Product Features	Key Product Features	Recommended Browse Nodes	Search Terms	Main Image URL	Swatch Image URL	Other Image URL	Other Image URL	Other Image URL	Other Image URL	Other Image URL	Other Image URL	Other Image URL	Other Image URL	Fulfillment Centre ID	Package Height	Package Width	Package Length	Package Weight	Package Weight Unit Of Measure	Package Dimensions Unit Of Measure	Parentage	Parent Sku	Relationship Type	Variation Theme	Age restriction  bladed products	Colour	Colour Map	Closure Type	Fabric Type	Material Type	Season	Size Map	Size	Sport	Speed Rating	Bottom Style	Cup Size	Department	Glove Type	Top Style	UV Protection	Waist Size	Waist Size Unit Of Measure	Golf Flex	Golf Loft	Grip Size	Wheel Size	Wheel Size Unit Of Measure	Model Name	Lens Colour	Blade Length	Blade Length Unit Of Measure	Outer Material Type	Inner Material Type	Sleeping Capacity	League Name	Hand	Head Size	Shaft Length	Shaft Length Unit Of Measure	Shaft Material	JerseyType	Team Name	Specific Uses For Product	PPU Count	PPU Count Type	Batteries Are Included	BatteryType	Number of Batteries	Lithium Battery Energy Content	Lithium Battery Packaging	Lithium Battery Voltage	Lithium Battery Weight	Model Year	Lithium Battery Weight Unit of Measure	Season and collection year	Battery Cell Composition	Bra band size unit	Bra band size";
                string header3 = "item_sku	external_product_id	external_product_id_type	item_name	product_description	manufacturer	part_number	feed_product_type	brand_name	update_delete	standard_price	condition_type	condition_note	quantity	list_price	product_site_launch_date	merchant_release_date	fulfillment_latency	product_tax_code	sale_price	sale_from_date	sale_end_date	item_package_quantity	number_of_items	max_aggregate_ship_quantity	offering_can_be_gift_messaged	offering_can_be_giftwrapped	is_discontinued_by_manufacturer	merchant_shipping_group_name	website_shipping_weight	website_shipping_weight_unit_of_measure	item_display_length	item_display_length_unit_of_measure	item_display_weight	item_display_weight_unit_of_measure	item_height	item_length	item_width	item_weight	item_weight_unit_of_measure	item_dimensions_unit_of_measure	bullet_point1	bullet_point2	bullet_point3	bullet_point4	bullet_point5	recommended_browse_nodes	generic_keywords	main_image_url	swatch_image_url	other_image_url1	other_image_url2	other_image_url3	other_image_url4	other_image_url5	other_image_url6	other_image_url7	other_image_url8	fulfillment_center_id	package_height	package_width	package_length	package_weight	package_weight_unit_of_measure	package_dimensions_unit_of_measure	parent_child	parent_sku	relationship_type	variation_theme	customer_restriction_type	color_name	color_map	closure_type	fabric_type	material_type	seasons	size_map	size_name	sport_type	speed_rating	bottom_style	cup_size	department_name	glove_type	top_style	ultraviolet_light_protection	waist_size	waist_size_unit_of_measure	golf_club_flex	golf_club_loft	grip_size	wheel_size	wheel_size_unit_of_measure	model_name	lens_color	blade_length	blade_length_unit_of_measure	outer_material_type	inner_material_type	occupancy	league_name	hand_orientation	head_size	shaft_length	shaft_length_unit_of_measure	shaft_material	style_name	team_name	specific_uses_for_product	unit_count	unit_count_type	are_batteries_included	battery_type	number_of_batteries	lithium_battery_energy_content	lithium_battery_packaging	lithium_battery_voltage	lithium_battery_weight	model_year	lithium_battery_weight_unit_of_measure	collection_name	battery_cell_composition	band_size_num_unit_of_measure	band_size_num";

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(header1);
                sb.AppendLine(header2);
                sb.AppendLine(header3);

                double addToMinVal = double.Parse(ConfigurationManager.AppSettings["ProdPriceAddToMinValUK"]);// 27-June-2016 making is configurable. It adds vaue accoding to range define in loop.

                if (data != null)
                {
                    foreach (var d in data)
                    {
                        try
                        {


                            //Nullable<double> price;
                            double price = 0;
                            double pricecal;
                            double pricemin = 0.00;
                            double finalprice;
                            string UPC;
                            if (d.UPC == null)
                            {
                                upc_codes uc = ukdb.upc_codes.Where(x => x.AssignedTo == null).FirstOrDefault();
                                UPC = uc.Upc;
                                UpdateUPC(uc.Upc, "NP" + d.ASIN);
                            }
                            else
                            {
                                UPC = d.UPC;
                            }
                            if (d.UpdatedSalesPrice != null)
                            {
                                price = double.Parse(d.UpdatedSalesPrice);
                            }
                            else if (d.SalesPrice != null)
                            {
                                price = double.Parse(d.SalesPrice);
                            }

                            if (price != 0)
                            {
                                double minval = double.Parse(ConfigurationManager.AppSettings["ProdPriceMinValUK"]);  // 27-June-2016 making it configurable. Minimum Product Price can Change from configuration file. 
                                
                                if (price > 0 && price <= 19.99)
                                {
                                    pricemin = minval;
                                }
                                else
                                {
                                    for (double i = 20; i < 500; i += 10)
                                    {
                                        minval = minval + addToMinVal;
                                        double temp = i + 10;
                                        if (price >= i && price < temp)
                                        {
                                            pricemin = minval;
                                            break;
                                        }
                                    }
                                }
                                pricecal = price * PriceValue;
                                if (pricemin >= pricecal)
                                {
                                    finalprice = pricemin;
                                }
                                else
                                {
                                    finalprice = pricecal;
                                }

                                string ItemName;
                                if (d.Title != null)
                                {
                                    // ItemName = new string(d.Title.Take(490).ToArray());
                                    ItemName = d.Title.Substring(0, d.Title.Length > 499 ? 499 : d.Title.Length);
                                }
                                else
                                {
                                    ItemName = null;
                                }
                                string description;

                                if (d.Description != null)
                                {
                                    string desc = Regex.Replace(d.Description, "<.*?>", String.Empty);
                                    //description = new string(desc.Take(1990).ToArray());
                                    description = desc.Substring(0, desc.Length > 1990 ? 1990 : desc.Length);
                                }
                                else
                                {
                                    description = null;
                                }

                                string manufacturer;
                                string brand;
                                if (d.Manufacturer == null && d.Brand == null)
                                {
                                    manufacturer = "Unknown";
                                    brand = "Unknown";
                                }
                                else
                                {
                                    if (d.Manufacturer == null)
                                    {
                                        manufacturer = d.Brand;
                                    }
                                    else
                                    {
                                        manufacturer = d.Manufacturer;
                                    }

                                    if (d.Brand == null)
                                    {
                                        brand = d.Manufacturer;
                                    }
                                    else
                                    {
                                        brand = d.Brand;
                                    }

                                }
                                string feature1, feature2, feature3, feature4;
                                if (d.Features1 != null)
                                {
                                    // feature1 = new string(d.Features1.Take(500).ToArray());
                                    feature1 = d.Features1.Substring(0, d.Features1.Length > 500 ? 500 : d.Features1.Length);

                                }
                                else
                                    feature1 = d.Features1;
                                if (d.Features2 != null)
                                {
                                    // feature2 = new string(d.Features2.Take(500).ToArray());
                                    feature2 = d.Features2.Substring(0, d.Features2.Length > 500 ? 500 : d.Features2.Length);
                                }
                                else
                                    feature2 = d.Features2;
                                if (d.Features3 != null)
                                {
                                    //feature3 = new string(d.Features3.Take(500).ToArray());
                                    feature3 = d.Features3.Substring(0, d.Features3.Length > 500 ? 500 : d.Features3.Length);
                                }
                                else
                                    feature3 = d.Features3;
                                if (d.Features4 != null)
                                {
                                    // feature4 = new string(d.Features4.Take(500).ToArray());
                                    feature4 = d.Features4.Substring(0, d.Features4.Length > 500 ? 500 : d.Features4.Length);
                                }
                                else
                                    feature4 = d.Features4;

                                sb.AppendLine(string.Join("\t",
                                              string.Format(@"""{0}""", "NP-" + d.ASIN.Trim()),
                                              string.Format(@"""{0}""", UPC),
                                              string.Format(@"""{0}""", "UPC"),
                                              string.Format(@"""{0}""", !string.IsNullOrEmpty(ItemName) ? ItemName.Contains('"') ? ItemName.Replace('"', ' ') : ItemName : string.Empty),
                                              string.Format(@"""{0}""", !string.IsNullOrEmpty(description) ? description.Contains('"') ? description.Replace('"', ' ') : description : string.Empty),
                                              string.Format(@"""{0}""", manufacturer),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", "SportingGoods"),
                                              string.Format(@"""{0}""", brand),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", String.Format("{0:0.00}", finalprice)),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", Quantity),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", FulfillmentLatency),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),/*package qty*/
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", !string.IsNullOrEmpty(feature1) ? feature1.Contains('"') ? feature1.Replace('"', ' ') : feature1 : string.Empty),
                                              string.Format(@"""{0}""", !string.IsNullOrEmpty(feature2) ? feature2.Contains('"') ? feature2.Replace('"', ' ') : feature2 : string.Empty),
                                              string.Format(@"""{0}""", !string.IsNullOrEmpty(feature3) ? feature3.Contains('"') ? feature3.Replace('"', ' ') : feature3 : string.Empty),
                                              string.Format(@"""{0}""", !string.IsNullOrEmpty(feature4) ? feature4.Contains('"') ? feature4.Replace('"', ' ') : feature4 : string.Empty),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", d.LargeImageUrl),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", "")));


                                d.FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_Not_Prime_Sports_" + fcountSport;

                                d.ExportDate = DateTime.Now;
                                if (shortcode == ConstantData.ED)
                                {
                                    d.Account1_Status = 1;
                                    d.Account1_AmazonActive = 1;
                                    d.Account1_ExportDate = DateTime.Now;
                                    Exportpath = d.Account1_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_ED_Not_Prime_Sports_" + fcountSport;
                                }
                                else if (shortcode == ConstantData.EM)
                                {
                                    d.Account2_Status = 1;
                                    d.Account2_AmazonActive = 1;
                                    d.Account2_ExportDate = DateTime.Now;
                                    Exportpath = d.Account2_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_EM_Not_Prime_Sports_" + fcountSport;
                                }

                                else if (shortcode == ConstantData.DI)
                                {
                                    d.Account4_Status = 1;
                                    d.Account4_AmazonActive = 1;
                                    d.Account4_ExportDate = DateTime.Now;
                                    Exportpath = d.Account4_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_DI_Not_Prime_Sports_" + fcountSport;
                                }


                                d.Status = 1;
                                d.Instock = 1;

                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    // ukdb.SaveChanges();

                    ukdb.ObjectContext().SaveChanges(SaveOptions.DetectChangesBeforeSave);

                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=" + Exportpath + ".txt");
                    //Response.AddHeader("content-disposition", "attachment;filename="+name +"_"+ DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
                    Response.ContentType = "application/text";
                    Response.ContentEncoding = Encoding.GetEncoding(1252);

                    fcountSport++;

                    Response.Write(sb.ToString());
                    Response.End();
                }



                transaction.Complete();
                ukdb.ObjectContext().AcceptAllChanges();
            }

        }

        private void ExportToysNotPrime(double PriceValue, string shortcode, IEnumerable<tbl_ToysNotPrime> data)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Suppress))
            {


                string Exportpath = "";

                StringWriter st = new StringWriter();

                string header1 = "TemplateType=toys	Version=2016.0505	The top 3 rows are for Amazon.com use only. Do not modify or delete the top 3 rows.								Offer-Offer Information - These attributes are required to make your item buyable for customers on the site.																		Dimensions-Product Dimensions - These attributes specify the size and weight of a product.														Discovery-Item discovery information - These attributes have an effect on how customers can find your product on the site using browse or search.								Images-Image Information - See Image Instructions tab for details.										Fulfillment-Use these columns to provide fulfilment-related information for orders fulfilled either by Amazon (FBA) or by the Seller.							Compliance-Compliance Information - Attributes used to comply with consumer laws in the country or region where the item is sold.			Ungrouped - These attributes create rich product listings for your buyers.";
                string header2 = "Seller SKU	Item Name (aka Title)	Product ID	Product ID Type	Feed Product Type	Brand Name	Manufacturer	Manufacturer Part Number	Product Description	Update Delete	Quantity	Standard Price	Condition Type	Offer Condition Note	Launch Date	Fulfillment Latency	Release Date	Sale Price	Sale From Date	Sale End Date	Number of Items	Stop Selling Date	Max Aggregate Ship Quantity	Product Tax Code	Can Be Gift Messaged	Is Gift Wrap Available?	Is Discontinued by Manufacturer	Merchant Shipping Group	Shipping Weight	Website Shipping Weight Unit Of Measure	Item Height	Item Length	Item Width	Item Length Unit Of Measure	Item Weight	Unit of measure of item weight	Display Weight	Item Display Weight Unit Of Measure	Display Volume	Item Display Volume Unit Of Measure	Display Length	Item Display Length Unit Of Measure	Key Product Features	Key Product Features	Key Product Features	Key Product Features	Key Product Features	Recommended Browse Nodes	Search Terms	Catalog Number	Main Image URL	Swatch Image Url	Other Image URL	Other Image URL	Other Image URL	Other Image URL	Other Image URL	Other Image URL	Other Image URL	Other Image URL	Fulfillment Centre ID	Package Height	Package Width	Package Length	Package Length Unit Of Measure	Package Weight	Package Weight Unit Of Measure	EU Toys Safety Directive Age-specific warning	EU Toys Safety Directive Non-Age-specific warning	EU Toys Safety Directive language warning	Parentage	Relationship Type	Parent SKU	Is Adult Product	Variation Theme	Colour	Colour Map	Size	Size Map	Manufacturer Warranty Description	Material Type	Product Care Instructions	Assembly Instructions	Minimum Age Recommendation	Mfg Minimum Unit Of Measure	Maximum Age Recommendation	Mfg Maximum Unit Of Measure	Target Gender	Special Features	Seller Warranty Description	Subject Character	Material Composition	Scale	Rail Gauge	Batteries are Included	BatteryType	Number of Batteries	Lithium Battery Voltage	Lithium Battery Weight	Lithium Battery Packaging	unit_count_type	unit_count";
                string header3 = "item_sku	item_name	external_product_id	external_product_id_type	feed_product_type	brand_name	manufacturer	part_number	product_description	update_delete	quantity	standard_price	condition_type	condition_note	product_site_launch_date	fulfillment_latency	merchant_release_date	sale_price	sale_from_date	sale_end_date	number_of_items	offering_end_date	max_aggregate_ship_quantity	product_tax_code	offering_can_be_gift_messaged	offering_can_be_giftwrapped	is_discontinued_by_manufacturer	merchant_shipping_group_name	website_shipping_weight	website_shipping_weight_unit_of_measure	item_height	item_length	item_width	item_length_unit_of_measure	item_weight	item_weight_unit_of_measure	item_display_weight	item_display_weight_unit_of_measure	item_display_volume	item_display_volume_unit_of_measure	item_display_length	item_display_length_unit_of_measure	bullet_point1	bullet_point2	bullet_point3	bullet_point4	bullet_point5	recommended_browse_nodes	generic_keywords	catalog_number	main_image_url	swatch_image_url	other_image_url1	other_image_url2	other_image_url3	other_image_url4	other_image_url5	other_image_url6	other_image_url7	other_image_url8	fulfillment_center_id	package_height	package_width	package_length	package_length_unit_of_measure	package_weight	package_weight_unit_of_measure	eu_toys_safety_directive_age_warning	eu_toys_safety_directive_warning	eu_toys_safety_directive_language	parent_child	relationship_type	parent_sku	is_adult_product	variation_theme	color_name	color_map	size_name	size_map	warranty_description	material_type	care_instructions	assembly_instructions	mfg_minimum	mfg_minimum_unit_of_measure	mfg_maximum	mfg_maximum_unit_of_measure	target_gender	special_features	seller_warranty_description	subject_character	material_composition	scale_name	rail_gauge	are_batteries_included	battery_type	number_of_batteries	lithium_battery_voltage	lithium_battery_weight	lithium_battery_packaging	unit_count_type	unit_count";


                StringBuilder sb = new StringBuilder();
                sb.AppendLine(header1);
                sb.AppendLine(header2);
                sb.AppendLine(header3);
                ((IObjectContextAdapter)ukdb).ObjectContext.CommandTimeout = 1800;
                double addToMinVal = double.Parse(ConfigurationManager.AppSettings["ProdPriceAddToMinValUK"]);// 27-June-2016 making is configurable. It adds vaue accoding to range define in loop.

                if (data != null)
                {
                    foreach (var d in data)
                    {
                        try
                        {


                            double price = 0;
                            double pricecal;
                            double pricemin = 0.00;
                            double finalprice;
                            string UPC;
                            if (d.UPC == null)
                            {
                                upc_codes uc = ukdb.upc_codes.Where(x => x.AssignedTo == null).FirstOrDefault();
                                UPC = uc.Upc;
                                UpdateUPC(uc.Upc, "NP" + d.ASIN);
                            }
                            else
                            {
                                UPC = d.UPC;
                            }
                            if (d.UpdatedSalesPrice != null)
                            {
                                price = double.Parse(d.UpdatedSalesPrice);
                            }
                            else
                                if (d.SalesPrice != null)
                                {
                                    price = double.Parse(d.SalesPrice);
                                }

                            if (price != 0)
                            {

                                double minval = double.Parse(ConfigurationManager.AppSettings["ProdPriceMinValUK"]);  // 27-June-2016 making it configurable. Minimum Product Price can Change from configuration file. 

                                if (price > 0 && price <= 19.99)
                                {
                                    pricemin = minval;
                                }
                                else
                                {
                                    for (double i = 20; i < 500; i += 10)
                                    {
                                        minval = minval + addToMinVal;
                                        double temp = i + 10;
                                        if (price >= i && price < temp)
                                        {
                                            pricemin = minval;
                                            break;
                                        }
                                    }
                                }
                                pricecal = price * PriceValue;
                                if (pricemin >= pricecal)
                                {
                                    finalprice = pricemin;
                                }
                                else
                                {
                                    finalprice = pricecal;
                                }



                                string ItemName;
                                if (d.Title != null)
                                {
                                    // ItemName = new string(d.Title.Take(490).ToArray());
                                    ItemName = d.Title.Substring(0, d.Title.Length > 499 ? 499 : d.Title.Length);
                                }
                                else
                                {
                                    ItemName = null;
                                }
                                string description;

                                if (d.Description != null)
                                {
                                    string desc = Regex.Replace(d.Description, "<.*?>", String.Empty);
                                    //description = new string(desc.Take(1990).ToArray());
                                    description = desc.Substring(0, desc.Length > 1990 ? 1990 : desc.Length);
                                }
                                else
                                {
                                    description = null;
                                }

                                string manufacturer;
                                string brand;
                                if (d.Manufacturer == null && d.Brand == null)
                                {
                                    manufacturer = "Unknown";
                                    brand = "Unknown";
                                }
                                else
                                {
                                    if (d.Manufacturer == null)
                                    {
                                        manufacturer = d.Brand;
                                    }
                                    else
                                    {
                                        manufacturer = d.Manufacturer;
                                    }

                                    if (d.Brand == null)
                                    {
                                        brand = d.Manufacturer;
                                    }
                                    else
                                    {
                                        brand = d.Brand;
                                    }

                                }
                                string feature1, feature2, feature3, feature4;
                                if (d.Features1 != null)
                                {
                                    // feature1 = new string(d.Features1.Take(500).ToArray());
                                    feature1 = d.Features1.Substring(0, d.Features1.Length > 500 ? 500 : d.Features1.Length);

                                }
                                else
                                    feature1 = d.Features1;
                                if (d.Features2 != null)
                                {
                                    // feature2 = new string(d.Features2.Take(500).ToArray());
                                    feature2 = d.Features2.Substring(0, d.Features2.Length > 500 ? 500 : d.Features2.Length);
                                }
                                else
                                    feature2 = d.Features2;
                                if (d.Features3 != null)
                                {
                                    //feature3 = new string(d.Features3.Take(500).ToArray());
                                    feature3 = d.Features3.Substring(0, d.Features3.Length > 500 ? 500 : d.Features3.Length);
                                }
                                else
                                    feature3 = d.Features3;
                                if (d.Features4 != null)
                                {
                                    // feature4 = new string(d.Features4.Take(500).ToArray());
                                    feature4 = d.Features4.Substring(0, d.Features4.Length > 500 ? 500 : d.Features4.Length);
                                }
                                else
                                    feature4 = d.Features4;
                                sb.AppendLine(string.Join("\t",
                                              string.Format(@"""{0}""", "NP-" + d.ASIN.Trim()),
                                              string.Format(@"""{0}""", !string.IsNullOrEmpty(ItemName) ? ItemName.Contains('"') ? ItemName.Replace('"', ' ') : ItemName : string.Empty),
                                              string.Format(@"""{0}""", UPC),
                                              string.Format(@"""{0}""", "UPC"),
                                              string.Format(@"""{0}""", "ToysAndGames"),
                                              string.Format(@"""{0}""", brand),
                                              string.Format(@"""{0}""", manufacturer),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", !string.IsNullOrEmpty(description) ? description.Contains('"') ? description.Replace('"', ' ') : description : string.Empty),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", Quantity),
                                              string.Format(@"""{0}""", String.Format("{0:0.00}", finalprice)),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", FulfillmentLatency),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", !string.IsNullOrEmpty(feature1) ? feature1.Contains('"') ? feature1.Replace('"', ' ') : feature1 : string.Empty),
                                              string.Format(@"""{0}""", !string.IsNullOrEmpty(feature2) ? feature2.Contains('"') ? feature2.Replace('"', ' ') : feature2 : string.Empty),
                                              string.Format(@"""{0}""", !string.IsNullOrEmpty(feature3) ? feature3.Contains('"') ? feature3.Replace('"', ' ') : feature3 : string.Empty),
                                              string.Format(@"""{0}""", !string.IsNullOrEmpty(feature4) ? feature4.Contains('"') ? feature4.Replace('"', ' ') : feature4 : string.Empty),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", d.LargeImageURL),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", "")));  //Fulfillment latency changes to 4 from 10 anf file templete version too..


                                d.FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_Not_Prime_Toys_" + fcountToys;

                                d.ExportDate = DateTime.Now;
                                if (shortcode == ConstantData.ED)
                                {
                                    d.Account1_Status = 1;
                                    d.Account1_AmazonActive = 1;
                                    d.Account1_ExportDate = DateTime.Now;
                                    Exportpath = d.Account1_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_ED_Not_Prime_Toys_" + fcountToys;
                                }
                                else if (shortcode == ConstantData.EM)
                                {
                                    d.Account2_Status = 1;
                                    d.Account2_AmazonActive = 1;
                                    d.Account2_ExportDate = DateTime.Now;
                                    Exportpath = d.Account2_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_EM_Not_Prime_Toys_" + fcountToys;
                                }
                                else if (shortcode == ConstantData.DI)
                                {
                                    d.Account4_Status = 1;
                                    d.Account4_AmazonActive = 1;
                                    d.Account4_ExportDate = DateTime.Now;
                                    Exportpath = d.Account4_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_DI_Not_Prime_Toys_" + fcountToys;
                                }

                                d.Instock = 1;
                                d.Status = 1;

                            }
                           
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    //  ukdb.SaveChanges();
                    ukdb.ObjectContext().SaveChanges(SaveOptions.DetectChangesBeforeSave);
                    fcountToys++;
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=" + Exportpath + ".txt");
                    //Response.AddHeader("content-disposition", "attachment;filename="+name +"_"+ DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
                    Response.ContentType = "application/text";
                    Response.ContentEncoding = Encoding.GetEncoding(1252);
                    Response.Write(sb.ToString());
                    Response.End();

                }

                transaction.Complete();
                ukdb.ObjectContext().AcceptAllChanges();
            }
        }

        private void ExportBeautyNotPrime(double PriceValue, string shortcode, IEnumerable<tbl_BeautyNotPrime> data)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Suppress))
            {


                string Exportpath = "";
                ((IObjectContextAdapter)ukdb).ObjectContext.CommandTimeout = 1800;

                StringWriter st = new StringWriter();
                string header1 = "TemplateType=Beauty	Version=2015.1217	The top 3 rows are for Amazon.com use only. Do not modify or delete the top 3 rows.								Offer - Offer Information - These attributes are required to make your item buyable for customers on the site.																	Dimensions - Product Dimensions - These attributes specify the size and weight of a product.														Discovery - Item discovery information - These attributes have an effect on how customers can find your product on the site using browse or search.							Images - Image Information - See Image Instructions tab for details.					Fulfillment - Use these columns to provide fulfilment-related information for orders fulfilled either by Amazon (FBA) or by the Seller.	Variation - Variation information - Populate these attributes if your product is available in different variations (for example colour or wattage).				Ungrouped - These attributes create rich product listings for your buyers.";
                string header2 = "SKU	Item Name (aka Title)	Product Type	Product ID	Product ID Type	Brand Name	Manufacturer	Manufacturer Part Number	Product Description	Update Delete	Standard Price	Quantity	Fulfillment Latency	Package Quantity	Number of Items	Launch Date	Release Date	Is Discontinued by Manufacturer	Sale Price	Sale From Date	Sale End Date	Max Order Quantity	Max Aggregate Ship Quantity	Can Be Gift Messaged	Is Gift Wrap Available?	Product Tax Code	Merchant Shipping Group	Item Display Weight Unit Of Measure	Display Weight	Item Display Volume Unit Of Measure	Display Volume	Display Length	Item Display Length Unit Of Measure	Item Weight Unit Of Measure	Item Weight	Item Length Unit Of Measure	Item Length	Item Width	Item Height	Website Shipping Weight Unit Of Measure	Shipping Weight	Recommended Browse Nodes	Key Product Features1	Key Product Features2	Key Product Features3	Key Product Features4	Key Product Features5	Search Terms	Main Image URL	Swatch Image URL	Other Image URL1	Other Image URL2	Other Image URL3	Fulfillment Centre ID	Parentage	Parent SKU	Relationship Type	Variation Theme	Ingredients	Material Type	Item Form	Is Adult Product	Target Gender	Skin Type	Hair Type	Indications	Directions	Size	Colour	Colour Map	Scent	Sun Protection Factor";
                string header3 = "item_sku	item_name	feed_product_type	external_product_id	external_product_id_type	brand_name	manufacturer	part_number	product_description	update_delete	standard_price	quantity	fulfillment_latency	item_package_quantity	number_of_items	product_site_launch_date	merchant_release_date	is_discontinued_by_manufacturer	sale_price	sale_from_date	sale_end_date	max_order_quantity	max_aggregate_ship_quantity	offering_can_be_gift_messaged	offering_can_be_giftwrapped	product_tax_code	merchant_shipping_group_name	item_display_weight_unit_of_measure	item_display_weight	item_display_volume_unit_of_measure	item_display_volume	item_display_length	item_display_length_unit_of_measure	item_weight_unit_of_measure	item_weight	item_length_unit_of_measure	item_length	item_width	item_height	website_shipping_weight_unit_of_measure	website_shipping_weight	recommended_browse_nodes	bullet_point1	bullet_point2	bullet_point3	bullet_point4	bullet_point5	generic_keywords	main_image_url	swatch_image_url	other_image_url1	other_image_url2	other_image_url3	fulfillment_center_id	parent_child	parent_sku	relationship_type	variation_theme	ingredients	material_type	item_form	is_adult_product	target_gender	skin_type	hair_type	indications	directions	size_name	color_name	color_map	scent_name	sun_protection";

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(header1);
                sb.AppendLine(header2);
                sb.AppendLine(header3);
                double addToMinVal = double.Parse(ConfigurationManager.AppSettings["ProdPriceAddToMinValUK"]);// 27-June-2016 making is configurable. It adds vaue accoding to range define in loop.

                //IEnumerable<tbl_Beauty> data = ukdb.tbl_Beauty.Where(x => x.UPC != "null" && x.Status == 0 && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(count.ToString())).ToList();
                if (data != null)
                {
                    foreach (var d in data)
                    {
                        try
                        {


                            double price = 0;
                            double pricecal;
                            double pricemin = 0.00;
                            double finalprice;
                            string UPC;
                            if (d.UPC == null)
                            {
                                upc_codes uc = ukdb.upc_codes.Where(x => x.AssignedTo == null).FirstOrDefault();
                                UPC = uc.Upc;
                                UpdateUPC(uc.Upc, "NP" + d.ASIN);
                            }
                            else
                            {
                                UPC = d.UPC;
                            }
                            if (d.UpdatedSalesPrice != null)
                            {
                                price = double.Parse(d.UpdatedSalesPrice);
                            }
                            else
                                if (d.SalesPrice != null)
                                {
                                    price = double.Parse(d.SalesPrice);
                                }

                            if (price != 0)
                            {
                                double minval = double.Parse(ConfigurationManager.AppSettings["ProdPriceMinValUK"]);  // 27-June-2016 making it configurable. Minimum Product Price can Change from configuration file. 
                                

                                if (price > 0 && price <= 19.99)
                                {
                                    pricemin = minval;
                                }
                                else
                                {
                                    for (double i = 20; i < 500; i += 10)
                                    {
                                        minval = minval + addToMinVal;
                                        double temp = i + 10;
                                        if (price >= i && price < temp)
                                        {
                                            pricemin = minval;
                                            break;
                                        }
                                    }
                                }
                                pricecal = price * PriceValue;
                                if (pricemin >= pricecal)
                                {
                                    finalprice = pricemin;
                                }
                                else
                                {
                                    finalprice = pricecal;
                                }


                                string ItemName;
                                if (d.Title != null)
                                {
                                    // ItemName = new string(d.Title.Take(490).ToArray());
                                    ItemName = d.Title.Substring(0, d.Title.Length > 499 ? 499 : d.Title.Length);
                                }
                                else
                                {
                                    ItemName = null;
                                }
                                string description;

                                if (d.Description != null)
                                {
                                    string desc = Regex.Replace(d.Description, "<.*?>", String.Empty);
                                    //description = new string(desc.Take(1990).ToArray());
                                    description = desc.Substring(0, desc.Length > 1990 ? 1990 : desc.Length);
                                }
                                else
                                {
                                    description = null;
                                }

                                string manufacturer;
                                string brand;
                                if (d.Manufacturer == null && d.Brand == null)
                                {
                                    manufacturer = "Unknown";
                                    brand = "Unknown";
                                }
                                else
                                {
                                    if (d.Manufacturer == null)
                                    {
                                        manufacturer = d.Brand;
                                    }
                                    else
                                    {
                                        manufacturer = d.Manufacturer;
                                    }

                                    if (d.Brand == null)
                                    {
                                        brand = d.Manufacturer;
                                    }
                                    else
                                    {
                                        brand = d.Brand;
                                    }

                                }
                                string feature1, feature2, feature3, feature4;
                                if (d.Features1 != null)
                                {
                                    // feature1 = new string(d.Features1.Take(500).ToArray());
                                    feature1 = d.Features1.Substring(0, d.Features1.Length > 500 ? 500 : d.Features1.Length);

                                }
                                else
                                    feature1 = d.Features1;
                                if (d.Features2 != null)
                                {
                                    // feature2 = new string(d.Features2.Take(500).ToArray());
                                    feature2 = d.Features2.Substring(0, d.Features2.Length > 500 ? 500 : d.Features2.Length);
                                }
                                else
                                    feature2 = d.Features2;
                                if (d.Features3 != null)
                                {
                                    //feature3 = new string(d.Features3.Take(500).ToArray());
                                    feature3 = d.Features3.Substring(0, d.Features3.Length > 500 ? 500 : d.Features3.Length);
                                }
                                else
                                    feature3 = d.Features3;
                                if (d.Features4 != null)
                                {
                                    // feature4 = new string(d.Features4.Take(500).ToArray());
                                    feature4 = d.Features4.Substring(0, d.Features4.Length > 500 ? 500 : d.Features4.Length);
                                }
                                else
                                    feature4 = d.Features4;

                                sb.AppendLine(string.Join("\t",
                                               string.Format(@"""{0}""", "NP-" + d.ASIN.Trim()),
                                               string.Format(@"""{0}""", ItemName),
                                               string.Format(@"""{0}""", "BeautyMisc"),
                                               string.Format(@"""{0}""", UPC),
                                               string.Format(@"""{0}""", "UPC"),
                                               string.Format(@"""{0}""", brand),
                                               string.Format(@"""{0}""", manufacturer),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", description),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", String.Format("{0:0.00}", finalprice)),
                                               string.Format(@"""{0}""", Quantity),
                                               string.Format(@"""{0}""", FulfillmentLatency),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", feature1),
                                               string.Format(@"""{0}""", feature2),
                                               string.Format(@"""{0}""", feature3),
                                               string.Format(@"""{0}""", feature4),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", d.LargeImageUrl),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", "")));

                                d.FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_Not_Prime_Beauty_" + fcountBeauty;

                                d.ExportDate = DateTime.Now;
                                d.Instock = 1;
                                if (shortcode == ConstantData.ED)
                                {
                                    d.Account1_Status = 1;
                                    d.Account1_AmazonActive = 1;
                                    d.Account1_ExportDate = DateTime.Now;
                                    Exportpath = d.Account1_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_ED_Not_Prime_Beauty_" + fcountBeauty;
                                }
                                else if (shortcode == ConstantData.EM)
                                {
                                    d.Account2_Status = 1;
                                    d.Account2_AmazonActive = 1;
                                    d.Account2_ExportDate = DateTime.Now;
                                    Exportpath = d.Account2_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_EM_Not_Prime_Beauty_" + fcountBeauty;
                                }
                                else if (shortcode == ConstantData.DI)
                                {
                                    d.Account4_Status = 1;
                                    d.Account4_AmazonActive = 1;
                                    d.Account4_ExportDate = DateTime.Now;
                                    Exportpath = d.Account4_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_DI_Not_Prime_Beauty_" + fcountBeauty;
                                }
                                d.Status = 1;


                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    ukdb.ObjectContext().SaveChanges(SaveOptions.DetectChangesBeforeSave);


                    fcountBeauty++;
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=" + Exportpath + ".txt");
                    Response.ContentType = "application/text";
                    Response.ContentEncoding = Encoding.GetEncoding(1252);
                    Response.Write(sb.ToString());
                    Response.End();
                }
                transaction.Complete();
                ukdb.ObjectContext().AcceptAllChanges();
            }
        }
        private void ExportHomeAndKitchenNotPrime(double PriceValue, string shortcode, IEnumerable<tbl_HomeAndKitchenNotPrime> data)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Suppress))
            {
                string ExportPath = "";
                ((IObjectContextAdapter)ukdb).ObjectContext.CommandTimeout = 1800;
                StringWriter st = new StringWriter();
                string header1 = "TemplateType=home	Version=2016.0505	The top 3 rows are for Amazon.com use only. Do not modify or delete the top 3 rows.								Offer-Offer Information - These attributes are required to make your item buyable for customers on the site.																	Dimensions-Product Dimensions - These attributes specify the size and weight of a product.														Discovery-Item discovery information - These attributes have an effect on how customers can find your product on the site using browse or search.								Images-Image Information - See Image Instructions tab for details.										Fulfillment-Use these columns to provide fulfilment-related information for orders fulfilled either by Amazon (FBA) or by the Seller.					Variation-Variation information - Populate these attributes if your product is available in different variations (for example colour or wattage).				Compliance-Compliance Information - Attributes used to comply with consumer laws in the country or region where the item is sold.	Ungrouped - These attributes create rich product listings for your buyers.";
                string header2 = "Seller SKU	Product ID	Product ID Type	Item Name (aka Title)	Brand Name	Manufacturer	Product Type	Manufacturer Part Number	Product Description	Update Delete	Launch Date	Standard Price	Quantity	Package Quantity	Product Tax Code	Release Date	Sale Price	Sale From Date	Sale End Date	Item Condition	Offer Condition Note	Fulfillment Latency	Maximum Aggregate Ship Quantity	Can Be Gift Messaged	Is Gift Wrap Available?	Is Discontinued by Manufacturer	Merchant Shipping Group	Shipping Weight	Website Shipping Weight Unit Of Measure	Display Length	Item Display Length Unit Of Measure	Display Width	Item Display Width Unit Of Measure	Display Height	Item Display Height Unit Of Measure	Display Diameter	Item Display Diameter Unit Of Measure	Display Weight	Item Display Weight Unit Of Measure	Volume Capacity of the appliance	Item Display Volume Unit Of Measure	Recommended Browse Nodes	Key Product Features	Key Product Features	Key Product Features	Key Product Features	Key Product Features	Search Terms	Home & Furniture Target Audience	Main Image URL	Swatch Image URL	Other Image Url	Other Image Url	Other Image Url	Other Image Url	Other Image Url	Other Image Url	Other Image Url	Other Image Url	Package Length	Package Width	Package Height	Package Length Unit Of Measure	Fulfillment Centre ID	Parentage	Parent SKU	Relationship Type	Variation Theme	Country Produced In	Age restriction bladed products	Colour	Size	Unit of Measure (Per Unit Pricing)	Unit Count (Per Unit Pricing)	Subject	Style Name	Special Features	Paint Type	Material Type	Shape	Color Map	Capacity	Capacity Unit Of Measure	Energy efficiency class	Scent	Thread Count	Wattage	Usage	Design	Occasion Type	Number of doors	Material Composition	Bed and Mattress/Sofa type	Mattress Firmness	Indoor Fountain Installation Type	Bed Frame Type	Batteries are Included	BatteryType	Number of Batteries	Outer Material Type	Minimum Age Recommendation";
                string header3 = "item_sku	external_product_id	external_product_id_type	item_name	brand_name	manufacturer	feed_product_type	part_number	product_description	update_delete	product_site_launch_date	standard_price	quantity	item_package_quantity	product_tax_code	merchant_release_date	sale_price	sale_from_date	sale_end_date	condition_type	condition_note	fulfillment_latency	max_aggregate_ship_quantity	offering_can_be_gift_messaged	offering_can_be_giftwrapped	is_discontinued_by_manufacturer	merchant_shipping_group_name	website_shipping_weight	website_shipping_weight_unit_of_measure	item_display_length	item_display_length_unit_of_measure	item_display_width	item_display_width_unit_of_measure	item_display_height	item_display_height_unit_of_measure	item_display_diameter	item_display_diameter_unit_of_measure	item_display_weight	item_display_weight_unit_of_measure	item_display_volume	item_display_volume_unit_of_measure	recommended_browse_nodes	bullet_point1	bullet_point2	bullet_point3	bullet_point4	bullet_point5	generic_keywords	target_audience_keywords	main_image_url	swatch_image_url	other_image_url1	other_image_url2	other_image_url3	other_image_url4	other_image_url5	other_image_url6	other_image_url7	other_image_url8	package_length	package_width	package_height	package_length_unit_of_measure	fulfillment_center_id	parent_child	parent_sku	relationship_type	variation_theme	country_string	customer_restriction_type	color_name	size_name	unit_count_type	unit_count	theme	style_name	special_features	paint_type	material_type	item_shape	color_map	capacity	capacity_unit_of_measure	efficiency	scent_name	thread_count	wattage	specific_uses_for_product	pattern_name	occasion_type	number_of_doors	material_composition	item_type_name	item_hardness	installation_type	frame_type	are_batteries_included	battery_type	number_of_batteries	outer_material_type	mfg_minimum";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(header1);
                sb.AppendLine(header2);
                sb.AppendLine(header3);
                double addToMinVal = double.Parse(ConfigurationManager.AppSettings["ProdPriceAddToMinValUK"]);// 27-June-2016 making is configurable. It adds vaue accoding to range define in loop.

                //IEnumerable<tbl_HomeandKitchen> data = ukdb.tbl_HomeandKitchen.Where(x => x.UPC != "null" && x.Status == 0 && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(count.ToString())).ToList();
                if (data != null)
                {
                    foreach (var d in data)
                    {
                        try
                        {


                            double price = 0;
                            double pricecal;
                            double pricemin = 0.0;
                            double finalprice;
                            string UPC;
                            if (d.UPC == null)
                            {
                                upc_codes uc = ukdb.upc_codes.Where(x => x.AssignedTo == null).FirstOrDefault();
                                UPC = uc.Upc;
                                UpdateUPC(uc.Upc, "NP" + d.ASIN);
                            }
                            else
                            {
                                UPC = d.UPC;
                            }
                            if (d.UpdatedSalesPrice != null)
                            {
                                price = Convert.ToDouble(d.UpdatedSalesPrice);
                            }
                            else
                                if (d.SalesPrice != null)
                                {
                                    price = Convert.ToDouble(d.SalesPrice);
                                }
                            if (price != 0)
                            {

                                double minval = double.Parse(ConfigurationManager.AppSettings["ProdPriceMinValUK"]);  // 27-June-2016 making it configurable. Minimum Product Price can Change from configuration file. 

                                if (price > 0 && price <= 19.99)
                                {
                                    pricemin = minval;
                                }
                                else
                                {
                                    for (double i = 20; i < 500; i += 10)
                                    {
                                        minval = minval + addToMinVal;
                                        double temp = i + 10;
                                        if (price >= i && price < temp)
                                        {
                                            pricemin = minval;
                                            break;
                                        }
                                    }
                                }
                                pricecal = price * PriceValue;
                                if (pricemin >= pricecal)
                                {
                                    finalprice = pricemin;
                                }
                                else
                                {
                                    finalprice = pricecal;
                                }
                                string ItemName;
                                if (d.Title != null)
                                {
                                    // ItemName = new string(d.Title.Take(490).ToArray());
                                    ItemName = d.Title.Substring(0, d.Title.Length > 499 ? 499 : d.Title.Length);
                                }
                                else
                                {
                                    ItemName = null;
                                }
                                string description;

                                if (d.Description != null)
                                {
                                    string desc = Regex.Replace(d.Description, "<.*?>", String.Empty);
                                    //description = new string(desc.Take(1990).ToArray());
                                    description = desc.Substring(0, desc.Length > 1990 ? 1990 : desc.Length);
                                }
                                else
                                {
                                    description = null;
                                }

                                string manufacturer;
                                string brand;
                                if (d.Manufacturer == null && d.Brand == null)
                                {
                                    manufacturer = "Unknown";
                                    brand = "Unknown";
                                }
                                else
                                {
                                    if (d.Manufacturer == null)
                                    {
                                        manufacturer = d.Brand;
                                    }
                                    else
                                    {
                                        manufacturer = d.Manufacturer;
                                    }

                                    if (d.Brand == null)
                                    {
                                        brand = d.Manufacturer;
                                    }
                                    else
                                    {
                                        brand = d.Brand;
                                    }

                                }
                                string feature1, feature2, feature3, feature4;
                                if (d.Features1 != null)
                                {
                                    // feature1 = new string(d.Features1.Take(500).ToArray());
                                    feature1 = d.Features1.Substring(0, d.Features1.Length > 500 ? 500 : d.Features1.Length);

                                }
                                else
                                    feature1 = d.Features1;
                                if (d.Features2 != null)
                                {
                                    // feature2 = new string(d.Features2.Take(500).ToArray());
                                    feature2 = d.Features2.Substring(0, d.Features2.Length > 500 ? 500 : d.Features2.Length);
                                }
                                else
                                    feature2 = d.Features2;
                                if (d.Features3 != null)
                                {
                                    //feature3 = new string(d.Features3.Take(500).ToArray());
                                    feature3 = d.Features3.Substring(0, d.Features3.Length > 500 ? 500 : d.Features3.Length);
                                }
                                else
                                    feature3 = d.Features3;
                                if (d.Features4 != null)
                                {
                                    // feature4 = new string(d.Features4.Take(500).ToArray());
                                    feature4 = d.Features4.Substring(0, d.Features4.Length > 500 ? 500 : d.Features4.Length);
                                }
                                else
                                    feature4 = d.Features4;

                                #region
                                sb.AppendLine(string.Join("\t",
                                    string.Format(@"""{0}""", "NP-" + d.ASIN.Trim()),
                                    string.Format(@"""{0}""", UPC),
                                    string.Format(@"""{0}""", "UPC"),
                                    string.Format(@"""{0}""", !string.IsNullOrEmpty(ItemName) ? ItemName.Contains('"') ? ItemName.Replace('"', ' ') : ItemName : string.Empty),
                                    string.Format(@"""{0}""", brand),
                                    string.Format(@"""{0}""", manufacturer),
                                    string.Format(@"""{0}""", "Home"),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", !string.IsNullOrEmpty(description) ? description.Contains('"') ? description.Replace('"', ' ') : description : string.Empty),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", String.Format("{0:0.00}", finalprice)),
                                    string.Format(@"""{0}""", Quantity),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", FulfillmentLatency),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", "1063498"),
                                    string.Format(@"""{0}""", !string.IsNullOrEmpty(feature1) ? feature1.Contains('"') ? feature1.Replace('"', ' ') : feature1 : string.Empty),
                                    string.Format(@"""{0}""", !string.IsNullOrEmpty(feature2) ? feature2.Contains('"') ? feature2.Replace('"', ' ') : feature2 : string.Empty),
                                    string.Format(@"""{0}""", !string.IsNullOrEmpty(feature3) ? feature3.Contains('"') ? feature3.Replace('"', ' ') : feature3 : string.Empty),
                                    string.Format(@"""{0}""", !string.IsNullOrEmpty(feature4) ? feature4.Contains('"') ? feature4.Replace('"', ' ') : feature4 : string.Empty),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", d.LargeImageURL),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", "")));
                                #endregion

                                d.FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_NotPrime_HomeandKitchen_" + fcountHomeandKitchen;
                                d.ExportDate = DateTime.Now;
                                d.Instock = 1;
                                if (shortcode == "ED")
                                {
                                    d.Account1_Status = 1;
                                    d.Account1_ExportDate = DateTime.Now;
                                    ExportPath = d.Account1_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_ED_NotPrime_HomeAndKitchen_" + fcountHomeandKitchen;

                                }
                                else if (shortcode == "EM")
                                {
                                    d.Account2_Status = 1;
                                    d.Account2_ExportDate = DateTime.Now;
                                    ExportPath = d.Account2_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_EM_NotPrime_HomeandKitchen_" + fcountHomeandKitchen;
                                }
                                else if (shortcode == "DC")
                                {
                                    d.Account3_Status = 1;
                                    d.Account3_ExportDate = DateTime.Now;
                                    ExportPath = d.Account3_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_DC_NotPrime_HomeandKitchen_" + fcountHomeandKitchen;
                                }
                                else if (shortcode == "DI")
                                {
                                    d.Account4_Status = 1;
                                    d.Account4_ExportDate = DateTime.Now;
                                    ExportPath = d.Account4_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_DI_NotPrime_HomeandKitchen_" + fcountHomeandKitchen;
                                }
                                d.Status = 1;


                            }
                        }
                        catch (Exception ex)
                        {


                        }
                    }
                    // ukdb.SaveChanges();
                    ukdb.ObjectContext().SaveChanges(SaveOptions.DetectChangesBeforeSave);

                    fcountHomeandKitchen++;
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=" + ExportPath + ".txt");
                    //Response.AddHeader("content-disposition", "attachment;filename="+name +"_"+ DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
                    Response.ContentType = "application/text";
                    Response.ContentEncoding = Encoding.GetEncoding(1252);
                    Response.Write(sb.ToString());
                    Response.End();
                }
                transaction.Complete();
                ukdb.ObjectContext().AcceptAllChanges();
            }
        }

        public void ExportHomeAndKitchenCanadaNotPrime(double PriceValue, string shortcode, IEnumerable<tbl_HomeAndKitchenNotPrime> data)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Suppress))
            {
                string ExportPath = "";
                ((IObjectContextAdapter)ukdb).ObjectContext.CommandTimeout = 1800;
                StringWriter st = new StringWriter();
                string header1 = "TemplateType=Home	Version=2015.1204	The top 3 rows are for Amazon.com use only. Do not modify or delete the top 3 rows.								Offer - Offer Information - These attributes are required to make your item buyable for customers on the site.																				Dimensions - Product Dimensions - These attributes specify the size and weight of a product.								Discovery - Item discovery information - These attributes have an effect on how customers can find your product on the site using browse or search.												Images - Image Information - See Image Instructions tab for details.					Fulfillment - Use these columns to provide fulfillment-related information for either Amazon-fulfilled (FBA) or seller-fulfilled orders.							Variation - Variation information - Populate these attributes if your product is available in different variations (for example colour or wattage).				Compliance - Compliance Information - Attributes used to comply with consumer laws in the country or region where the item is sold.			Ungrouped - These attributes create rich product listings for your buyers.";
                string header2 = "Seller SKU	Product ID	Product ID Type	Product Name	Product Description	Product Type	Brand Name	Manufacturer	Manufacturer Part Number	Update Delete	Standard Price	Item Condition	Offer Condition Note	Quantity	Manufacturer's Suggested Retail Price	Minimum Advertised Price	Launch Date	Release Date	Restock Date	Fulfillment Latency	Product Tax Code	Sale Price	Sale Start Date	Sale End Date	Package Quantity	Max Aggregate Ship Quantity	Offering Can Be Gift Messaged	Is Gift Wrap Available	Is Discontinued by Manufacturer	Shipping-Template	Shipping Weight	Website Shipping Weight Unit Of Measure	Item Display Length	Item Display Width	Item Display Height	Display Dimensions Unit Of Measure	Item Display Weight	Item Display Weight Unit Of Measure	Key Product Features1	Key Product Features2	Key Product Features3	Key Product Features4	Key Product Features5	recommended-browse-nodes	Intended Use	Target Audience1	Target Audience2	Target Audience3	Subject Matter	Search Terms	Main Image URL	Swatch Image URL	Other Image Url1	Other Image Url2	Other Image Url3	Fulfillment Center ID	Package Height	Package Width	Package Length	Package Dimensions Unit Of Measure	Package Weight	Package Weight Unit Of Measure	Parentage	Parent SKU	Relationship Type	Variation Theme	Cpsia Warning	CPSIA Warning Description	Country of Origin	Manufacturer Warranty Description	Other Attributes	Fabric Type	Import Designation	Country as Labeled	Fur Description	Paint Type	Number of Sets	Occasion	Thread Count	Number of Pieces	Shape	Scent Name	Pattern	Included Components	Color	Color Map	Size	Material Type	Style Name	PowerSource	Wattage	Specific Uses For Product	Additional Features	Batteries are Included	Battery Type	Number of Batteries Required	Lithium Battery Packaging	Lithium Battery Voltage	Lithium Battery Weight	Voltage	Blade Material Type";
                string header3 = "item_sku	external_product_id	external_product_id_type	item_name	product_description	feed_product_type	brand_name	manufacturer	part_number	update_delete	standard_price	condition_type	condition_note	quantity	list_price	map_price	product_site_launch_date	merchant_release_date	restock_date	fulfillment_latency	product_tax_code	sale_price	sale_from_date	sale_end_date	item_package_quantity	max_aggregate_ship_quantity	offering_can_be_gift_messaged	offering_can_be_giftwrapped	is_discontinued_by_manufacturer	merchant_shipping_group_name	website_shipping_weight	website_shipping_weight_unit_of_measure	item_display_length	item_display_width	item_display_height	display_dimensions_unit_of_measure	item_display_weight	item_display_weight_unit_of_measure	bullet_point1	bullet_point2	bullet_point3	bullet_point4	bullet_point5	recommended_browse_nodes	specific_uses_keywords	target_audience_keywords1	target_audience_keywords2	target_audience_keywords3	thesaurus_subject_keywords	generic_keywords	main_image_url	swatch_image_url	other_image_url1	other_image_url2	other_image_url3	fulfillment_center_id	package_height	package_width	package_length	package_dimensions_unit_of_measure	package_weight	package_weight_unit_of_measure	parent_child	parent_sku	relationship_type	variation_theme	cpsia_cautionary_statement	cpsia_cautionary_description	country_string	warranty_description	thesaurus_attribute_keywords	fabric_type	import_designation	country_as_labeled	fur_description	paint_type	number_of_sets	occasion	thread_count	number_of_pieces	item_shape	scent_name	pattern_name	included_components	color_name	color_map	size_name	material_type	style_name	power_source_type	wattage	specific_uses_for_product	special_features	are_batteries_included	battery_type	number_of_batteries	lithium_battery_packaging	lithium_battery_voltage	lithium_battery_weight	voltage	blade_material_type";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(header1);
                sb.AppendLine(header2);
                sb.AppendLine(header3);
                //IEnumerable<tbl_HomeandKitchen> data = ukdb.tbl_HomeandKitchen.Where(x => x.UPC != "null" && x.Status == 0 && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(count.ToString())).ToList();
                if (data != null)
                {
                    foreach (var d in data)
                    {

                        try
                        {


                            double price = 0;
                            double pricecal;
                            double pricemin = 0.0;
                            double finalprice;
                            string UPC;
                            if (d.UPC == null)
                            {
                                upc_codes uc = ukdb.upc_codes.Where(x => x.AssignedTo == null).FirstOrDefault();
                                UPC = uc.Upc;

                                UpdateUPC(uc.Upc, d.ASIN);
                            }
                            else
                            {
                                UPC = d.UPC;
                            }
                            if (d.UpdatedSalesPrice != null)
                            {
                                price = Convert.ToDouble(d.UpdatedSalesPrice);
                            }
                            else
                                if (d.SalesPrice != null)
                                {
                                    price = Convert.ToDouble(d.SalesPrice);
                                }
                            if (price != 0)
                            {
                                if (price < 200.01)
                                {

                                    double minval = 49.99;

                                    if (price > 0 && price <= 19.99)
                                    {
                                        pricemin = 92.99;
                                    }
                                    else if (price > 19.99 && price <= 29.99)
                                    {
                                        pricemin = 120.99;
                                    }
                                    else if (price > 29.99 && price <= 39.99)
                                    {
                                        pricemin = 142.99;
                                    }
                                    else if (price > 39.99 && price <= 49.99)
                                    {
                                        pricemin = 168.99;
                                    }
                                    else if (price > 49.99 && price <= 59.99)
                                    {
                                        pricemin = 192.99;
                                    }
                                    else if (price > 59.99 && price <= 69.99)
                                    {
                                        pricemin = 222.99;
                                    }
                                    else if (price > 69.99 && price <= 79.99)
                                    {
                                        pricemin = 255.99;
                                    }
                                    else if (price > 79.99 && price <= 89.99)
                                    {
                                        pricemin = 285.99;
                                    }
                                    else if (price > 89.99 && price <= 99.99)
                                    {
                                        pricemin = 313.99;
                                    }
                                    else if (price > 99.99 && price <= 109.99)
                                    {
                                        pricemin = 327.99;
                                    }
                                    else if (price > 109.99 && price <= 119.99)
                                    {
                                        pricemin = 356.99;
                                    }
                                    else if (price > 119.99 && price <= 129.99)
                                    {
                                        pricemin = 385.99;
                                    }
                                    else if (price > 129.99 && price <= 139.99)
                                    {
                                        pricemin = 420.99;
                                    }
                                    else if (price > 139.99 && price <= 149.99)
                                    {
                                        pricemin = 442.99;
                                    }
                                    else if (price > 149.99 && price <= 159.99)
                                    {
                                        pricemin = 463.99;
                                    }
                                    else if (price > 159.99 && price <= 169.99)
                                    {
                                        pricemin = 485.99;
                                    }
                                    else if (price > 169.99 && price <= 179.99)
                                    {
                                        pricemin = 513.99;
                                    }
                                    else if (price > 179.99 && price <= 189.99)
                                    {
                                        pricemin = 542.99;
                                    }
                                    else if (price > 189.99 && price <= 199.99)
                                    {
                                        pricemin = 569.99;
                                    }
                                    pricecal = price * PriceValue;
                                    if (pricemin >= pricecal)
                                    {
                                        finalprice = pricemin;
                                    }
                                    else
                                    {
                                        finalprice = pricecal;
                                    }
                                    string ItemName;
                                    if (d.Title != null)
                                    {
                                        // ItemName = new string(d.Title.Take(490).ToArray());
                                        ItemName = d.Title.Substring(0, d.Title.Length > 499 ? 499 : d.Title.Length);
                                    }
                                    else
                                    {
                                        ItemName = null;
                                    }
                                    string description;

                                    if (d.Description != null)
                                    {
                                        string desc = Regex.Replace(d.Description, "<.*?>", String.Empty);
                                        //description = new string(desc.Take(1990).ToArray());
                                        description = desc.Substring(0, desc.Length > 1990 ? 1990 : desc.Length);
                                    }
                                    else
                                    {
                                        description = null;
                                    }

                                    string manufacturer;
                                    string brand;
                                    if (d.Manufacturer == null && d.Brand == null)
                                    {
                                        manufacturer = "Unknown";
                                        brand = "Unknown";
                                    }
                                    else
                                    {
                                        if (d.Manufacturer == null)
                                        {
                                            manufacturer = d.Brand;
                                        }
                                        else
                                        {
                                            manufacturer = d.Manufacturer;
                                        }

                                        if (d.Brand == null)
                                        {
                                            brand = d.Manufacturer;
                                        }
                                        else
                                        {
                                            brand = d.Brand;
                                        }

                                    }
                                    string feature1, feature2, feature3, feature4;
                                    if (d.Features1 != null)
                                    {
                                        // feature1 = new string(d.Features1.Take(500).ToArray());
                                        feature1 = d.Features1.Substring(0, d.Features1.Length > 500 ? 500 : d.Features1.Length);

                                    }
                                    else
                                        feature1 = d.Features1;
                                    if (d.Features2 != null)
                                    {
                                        // feature2 = new string(d.Features2.Take(500).ToArray());
                                        feature2 = d.Features2.Substring(0, d.Features2.Length > 500 ? 500 : d.Features2.Length);
                                    }
                                    else
                                        feature2 = d.Features2;
                                    if (d.Features3 != null)
                                    {
                                        //feature3 = new string(d.Features3.Take(500).ToArray());
                                        feature3 = d.Features3.Substring(0, d.Features3.Length > 500 ? 500 : d.Features3.Length);
                                    }
                                    else
                                        feature3 = d.Features3;
                                    if (d.Features4 != null)
                                    {
                                        // feature4 = new string(d.Features4.Take(500).ToArray());
                                        feature4 = d.Features4.Substring(0, d.Features4.Length > 500 ? 500 : d.Features4.Length);
                                    }
                                    else
                                        feature4 = d.Features4;
                                    sb.AppendLine(string.Join("\t",
                                        string.Format(@"""{0}""","NP"+ d.ASIN.Trim()),
                                        string.Format(@"""{0}""", UPC),
                                        string.Format(@"""{0}""", "UPC"),
                                        string.Format(@"""{0}""", !string.IsNullOrEmpty(ItemName) ? ItemName.Contains('"') ? ItemName.Replace('"', ' ') : ItemName : string.Empty),
                                         string.Format(@"""{0}""", !string.IsNullOrEmpty(description) ? description.Contains('"') ? description.Replace('"', ' ') : description : string.Empty),
                                         string.Format(@"""{0}""", "Home"),
                                        string.Format(@"""{0}""", brand),
                                        string.Format(@"""{0}""", manufacturer),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", String.Format("{0:0.00}", finalprice)),
                                         string.Format(@"""{0}""", ""),
                                         string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", Quantity),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", FulfillmentLatency),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", !string.IsNullOrEmpty(feature1) ? feature1.Contains('"') ? feature1.Replace('"', ' ') : feature1 : string.Empty),
                                        string.Format(@"""{0}""", !string.IsNullOrEmpty(feature2) ? feature2.Contains('"') ? feature2.Replace('"', ' ') : feature2 : string.Empty),
                                        string.Format(@"""{0}""", !string.IsNullOrEmpty(feature3) ? feature3.Contains('"') ? feature3.Replace('"', ' ') : feature3 : string.Empty),
                                        string.Format(@"""{0}""", !string.IsNullOrEmpty(feature4) ? feature4.Contains('"') ? feature4.Replace('"', ' ') : feature4 : string.Empty),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", "1063498"),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", d.LargeImageURL),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", "")));

                                    d.FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_Prime_HomeandKitchen_" + fcountHomeandKitchen;
                                    d.ExportDate = DateTime.Now;
                                    d.Instock = 1;
                                    if (shortcode == "DC")
                                    {
                                        d.Account3_Status = 1;
                                        d.Account3_ExportDate = DateTime.Now;
                                        ExportPath = d.Account3_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_DC_Prime_HomeandKitchen_" + fcountHomeandKitchen;
                                    }

                                    d.Status = 1;


                                }

                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    ukdb.ObjectContext().SaveChanges(SaveOptions.DetectChangesBeforeSave);

                    // ukdb.SaveChanges();
                    fcountHomeandKitchen++;
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=" + ExportPath + ".txt");
                    //Response.AddHeader("content-disposition", "attachment;filename="+name +"_"+ DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
                    Response.ContentType = "application/text";
                    Response.ContentEncoding = Encoding.GetEncoding(1252);
                    Response.Write(sb.ToString());
                    Response.End();

                    transaction.Complete();
                    ukdb.ObjectContext().AcceptAllChanges();
                }
            }
        }


        public void CreateCSV(string name)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Main"].ToString());
            SqlCommand cmd = new SqlCommand("CSVReport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            StringWriter st = new StringWriter();

            st.WriteLine("\"AMAZONSKU\",\"ASIN\",\"SEARCHTERM\",\"TITLE\",\"OURMOSTRECENTUSLISTINGPRICE\",\"PRODUCTNAME\",\"PRODUCTCOST\",\"INTERNALPRODUCTID\",\"UPC\",\"MANUFACTURER\",\"IMAGEURL\",\"IMPORTANTINFORMATIONTEXT\",\"AMAZONDESCRIPTION\",\"PRODUCTDETAILSITEMWEIGHT\",\"PRODUCTDETAILS_SHIPPING WEIGHT\",\"PRODUCTFEATURES1\",\"PRODUCTFEATURES2\",\"PRODUCTFEATURES3\",\"PRODUCTFEATURES4\",\"PRODUCTFEATURES5\"");
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=" + name + DateTime.Now.ToString("yyyy-MM-dd") + ".csv");
            Response.ContentType = "text/csv";
            StringBuilder sb = new StringBuilder();

            foreach (DataRow rw in dt.Rows)
            {
                st.WriteLine(rw[0].ToString());
            }

            Response.Write(st.ToString());
            Response.End();

        }
        public void UkProhibitionTbl(string table)
        {

            //Old Version OF UK Prohibition
            #region
             //UKOmnimarkEntities dbcontext = new UKOmnimarkEntities();
//            //try
//            //{
//            //    ((IObjectContextAdapter)dbcontext).ObjectContext.CommandTimeout = 1800;
//            //    var prohibitedkeys = dbcontext.tbl_Prohibited_Keywords.Select(x => x.ProhibitedKeys).ToList();
//            //    Parallel.ForEach(prohibitedkeys, (inputString) =>
//            //    {
//            //        try
//            //        {
//            //            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UKMain"].ToString()))
//            //            {
//            //                Regex re = new Regex("'");
//            //                string pk = re.Replace(inputString, " ");
//            //                using (SqlCommand Cmd = new SqlCommand("ExecuteKeyWords", con))
//            //                {

//            //                    Cmd.CommandType = CommandType.StoredProcedure;
//            //                    Cmd.Parameters.AddWithValue("likeWord", pk);
//            //                    Cmd.Parameters.AddWithValue("tableName", table);
//            //                    Cmd.CommandTimeout = 2200;
//            //                    con.Open();
//            //                    Cmd.ExecuteNonQuery();
//            //                }

//            //            }
//            //        }
//            //        catch (Exception ex)
//            //        {
//            //            ex.ToString();
//            //        }
//            //    });
//            //}
//            //catch (Exception)
//            //{

//            //    throw;
//            //}
//            //finally
//            //{
//            //    dbcontext.Dispose();
            //}
            #endregion      // Old Version of UKprohibition
            
            // New Version of UK Prohibition (2-July-2016) 
            UKProhibition ukp = new UKProhibition();
            if (MSSqlMng.GetInstance().ConnectDBServer())
            {
                //InitProcess();
                ukp.DoProcess(table);

                MSSqlMng.GetInstance().DisconnectDBServer();
            }

        }

        public void CanadaProhibitionTbl(string table)
        {
            UKOmnimarkEntities dbcontext = new UKOmnimarkEntities();
            try
            {
                ((IObjectContextAdapter)dbcontext).ObjectContext.CommandTimeout = 1800;
                var prohibitedkeys = dbcontext.Canada_Prohibited_Keywords.Select(x => x.Keywords).ToList();
                Parallel.ForEach(prohibitedkeys, (inputString) =>
                {
                    try
                    {
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UKMain"].ToString()))
                        {
                            Regex re = new Regex("'");
                            string pk = re.Replace(inputString, " ");
                            using (SqlCommand Cmd = new SqlCommand("ExecuteKeyWordsForCanada", con))
                            {

                                Cmd.CommandType = CommandType.StoredProcedure;
                                Cmd.Parameters.AddWithValue("likeWord", pk);
                                Cmd.Parameters.AddWithValue("tableName", table);
                                Cmd.CommandTimeout = 2200;
                                con.Open();
                                Cmd.ExecuteNonQuery();
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }
                });
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                dbcontext.Dispose();
            }

        }

        public void ExportForUK(double PriceValue, string shortcode, IEnumerable<tbl_Sports> data)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Suppress))
            {
                string Exportpath = "";

                ((IObjectContextAdapter)ukdb).ObjectContext.CommandTimeout = 1800;


                string header1 = "TemplateType=sports	Version=2016.0505	The top 3 rows are for Amazon.com use only. Do not modify or delete the top 3 rows.								Offer-Offer Information - These attributes are required to make your item buyable for customers on the site.																			Dimensions-Product Dimensions - These attributes specify the size and weight of a product.												Discovery-Item discovery information - These attributes have an effect on how customers can find your product on the site using browse or search.							Images-Image Information - See Image Instructions tab for details.										Fulfillment-Use these columns to provide fulfilment-related information for orders fulfilled either by Amazon (FBA) or by the Seller.							Variation-Variation information - Populate these attributes if your product is available in different variations (for example colour or wattage).				Ungrouped - These attributes create rich product listings for your buyers.";
                string header2 = "Seller SKU	Product ID	Product ID Type	Item Name	Product Description	Manufacturer	Manufacturer Part Number	Product Type	Brand Name	Update Delete	Standard Price	Item Condition	Offer Condition Note	Quantity	Recommended Retail Price	Launch Date	Release Date	Fulfillment Latency	Product Tax Code	Sale Price	Sale From Date	Sale End Date	Package Quantity	Number of Items	Max Aggregate Ship Quantity	Can Be Gift Messaged	Is Gift Wrap Available?	Is Discontinued by Manufacturer	Merchant Shipping Group	Shipping Weight	Website Shipping Weight Unit Of Measure	Display Length	Item Display Length Unit Of Measure	Display Weight	Item Display Weight Unit Of Measure	Item Height	Item Length	Item Width	Item Weight	Item Weight Unit Of Measure	Item Dimensions Unit Of Measure	Key Product Features	Key Product Features	Key Product Features	Key Product Features	Key Product Features	Recommended Browse Nodes	Search Terms	Main Image URL	Swatch Image URL	Other Image URL	Other Image URL	Other Image URL	Other Image URL	Other Image URL	Other Image URL	Other Image URL	Other Image URL	Fulfillment Centre ID	Package Height	Package Width	Package Length	Package Weight	Package Weight Unit Of Measure	Package Dimensions Unit Of Measure	Parentage	Parent Sku	Relationship Type	Variation Theme	Age restriction  bladed products	Colour	Colour Map	Closure Type	Fabric Type	Material Type	Season	Size Map	Size	Sport	Speed Rating	Bottom Style	Cup Size	Department	Glove Type	Top Style	UV Protection	Waist Size	Waist Size Unit Of Measure	Golf Flex	Golf Loft	Grip Size	Wheel Size	Wheel Size Unit Of Measure	Model Name	Lens Colour	Blade Length	Blade Length Unit Of Measure	Outer Material Type	Inner Material Type	Sleeping Capacity	League Name	Hand	Head Size	Shaft Length	Shaft Length Unit Of Measure	Shaft Material	JerseyType	Team Name	Specific Uses For Product	PPU Count	PPU Count Type	Batteries Are Included	BatteryType	Number of Batteries	Lithium Battery Energy Content	Lithium Battery Packaging	Lithium Battery Voltage	Lithium Battery Weight	Model Year	Lithium Battery Weight Unit of Measure	Season and collection year	Battery Cell Composition	Bra band size unit	Bra band size";
                string header3 = "item_sku	external_product_id	external_product_id_type	item_name	product_description	manufacturer	part_number	feed_product_type	brand_name	update_delete	standard_price	condition_type	condition_note	quantity	list_price	product_site_launch_date	merchant_release_date	fulfillment_latency	product_tax_code	sale_price	sale_from_date	sale_end_date	item_package_quantity	number_of_items	max_aggregate_ship_quantity	offering_can_be_gift_messaged	offering_can_be_giftwrapped	is_discontinued_by_manufacturer	merchant_shipping_group_name	website_shipping_weight	website_shipping_weight_unit_of_measure	item_display_length	item_display_length_unit_of_measure	item_display_weight	item_display_weight_unit_of_measure	item_height	item_length	item_width	item_weight	item_weight_unit_of_measure	item_dimensions_unit_of_measure	bullet_point1	bullet_point2	bullet_point3	bullet_point4	bullet_point5	recommended_browse_nodes	generic_keywords	main_image_url	swatch_image_url	other_image_url1	other_image_url2	other_image_url3	other_image_url4	other_image_url5	other_image_url6	other_image_url7	other_image_url8	fulfillment_center_id	package_height	package_width	package_length	package_weight	package_weight_unit_of_measure	package_dimensions_unit_of_measure	parent_child	parent_sku	relationship_type	variation_theme	customer_restriction_type	color_name	color_map	closure_type	fabric_type	material_type	seasons	size_map	size_name	sport_type	speed_rating	bottom_style	cup_size	department_name	glove_type	top_style	ultraviolet_light_protection	waist_size	waist_size_unit_of_measure	golf_club_flex	golf_club_loft	grip_size	wheel_size	wheel_size_unit_of_measure	model_name	lens_color	blade_length	blade_length_unit_of_measure	outer_material_type	inner_material_type	occupancy	league_name	hand_orientation	head_size	shaft_length	shaft_length_unit_of_measure	shaft_material	style_name	team_name	specific_uses_for_product	unit_count	unit_count_type	are_batteries_included	battery_type	number_of_batteries	lithium_battery_energy_content	lithium_battery_packaging	lithium_battery_voltage	lithium_battery_weight	model_year	lithium_battery_weight_unit_of_measure	collection_name	battery_cell_composition	band_size_num_unit_of_measure	band_size_num";

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(header1);
                sb.AppendLine(header2);
                sb.AppendLine(header3);
                
                double addToMinVal = double.Parse(ConfigurationManager.AppSettings["ProdPriceAddToMinValUK"]);// 27-June-2016 making is configurable. It adds vaue accoding to range define in loop.

                if (data != null)
                {
                    foreach (var d in data)
                    {
                        try
                        {
                            //Nullable<double> price;
                            double price = 0;
                            double pricecal;
                            double pricemin = 0.00;
                            double finalprice;
                            string UPC;
                            if (d.UPC == null)
                            {
                                upc_codes uc = ukdb.upc_codes.Where(x => x.AssignedTo == null).FirstOrDefault();
                                UPC = uc.Upc;
                                
                                UpdateUPC(uc.Upc, d.ASIN);

                            }
                            else
                            {
                                UPC = d.UPC;
                            }
                            if (d.UpdatedSalesPrice != null)
                            {
                                price = double.Parse(d.UpdatedSalesPrice);
                            }
                            else if (d.SalesPrice != null)
                            {
                                price = double.Parse(d.SalesPrice);
                            }

                            if (price != 0)
                            {
                                double minval = double.Parse(ConfigurationManager.AppSettings["ProdPriceMinValUK"]);  // 27-June-2016 making it configurable. Minimum Product Price can Change from configuration file. 
                                if (price > 0 && price <= 19.99)
                                {
                                    pricemin = minval;
                                }
                                else
                                {
                                    for (double i = 20; i < 500; i += 10)
                                    {
                                        minval = minval + addToMinVal;
                                        double temp = i + 10;
                                        if (price >= i && price < temp)
                                        {
                                            pricemin = minval;
                                            break;
                                        }
                                    }
                                }
                                pricecal = price * PriceValue;
                                if (pricemin >= pricecal)
                                {
                                    finalprice = pricemin;
                                }
                                else
                                {
                                    finalprice = pricecal;
                                }

                                string ItemName;
                                if (d.Title != null)
                                {
                                    // ItemName = new string(d.Title.Take(490).ToArray());
                                    ItemName = d.Title.Substring(0, d.Title.Length > 499 ? 499 : d.Title.Length);
                                }
                                else
                                {
                                    ItemName = null;
                                }
                                string description;

                                if (d.Description != null)
                                {
                                    string desc = Regex.Replace(d.Description, "<.*?>", String.Empty);
                                    //description = new string(desc.Take(1990).ToArray());
                                    description = desc.Substring(0, desc.Length > 1990 ? 1990 : desc.Length);
                                }
                                else
                                {
                                    description = null;
                                }

                                string manufacturer;
                                string brand;
                                if (d.Manufacturer == null && d.Brand == null)
                                {
                                    manufacturer = "Unknown";
                                    brand = "Unknown";
                                }
                                else
                                {
                                    if (d.Manufacturer == null)
                                    {
                                        manufacturer = d.Brand;
                                    }
                                    else
                                    {
                                        manufacturer = d.Manufacturer;
                                    }

                                    if (d.Brand == null)
                                    {
                                        brand = d.Manufacturer;
                                    }
                                    else
                                    {
                                        brand = d.Brand;
                                    }

                                }
                                string feature1, feature2, feature3, feature4;
                                if (d.Features1 != null)
                                {
                                    // feature1 = new string(d.Features1.Take(500).ToArray());
                                    feature1 = d.Features1.Substring(0, d.Features1.Length > 500 ? 500 : d.Features1.Length);

                                }
                                else
                                    feature1 = d.Features1;
                                if (d.Features2 != null)
                                {
                                    // feature2 = new string(d.Features2.Take(500).ToArray());
                                    feature2 = d.Features2.Substring(0, d.Features2.Length > 500 ? 500 : d.Features2.Length);
                                }
                                else
                                    feature2 = d.Features2;
                                if (d.Features3 != null)
                                {
                                    //feature3 = new string(d.Features3.Take(500).ToArray());
                                    feature3 = d.Features3.Substring(0, d.Features3.Length > 500 ? 500 : d.Features3.Length);
                                }
                                else
                                    feature3 = d.Features3;
                                if (d.Features4 != null)
                                {
                                    // feature4 = new string(d.Features4.Take(500).ToArray());
                                    feature4 = d.Features4.Substring(0, d.Features4.Length > 500 ? 500 : d.Features4.Length);
                                }
                                else
                                    feature4 = d.Features4;


                                sb.AppendLine(string.Join("\t",
                                               string.Format(@"""{0}""", d.ASIN.Trim()),
                                               string.Format(@"""{0}""", UPC),
                                               string.Format(@"""{0}""", "UPC"),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(ItemName) ? ItemName.Contains('"') ? ItemName.Replace('"', ' ') : ItemName : string.Empty),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(description) ? description.Contains('"') ? description.Replace('"', ' ') : description : string.Empty),
                                               string.Format(@"""{0}""", manufacturer),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", "SportingGoods"),
                                               string.Format(@"""{0}""", brand),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", String.Format("{0:0.00}", finalprice)),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", Quantity),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", FulfillmentLatency),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),/*package qty*/
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(feature1) ? feature1.Contains('"') ? feature1.Replace('"', ' ') : feature1 : string.Empty),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(feature2) ? feature2.Contains('"') ? feature2.Replace('"', ' ') : feature2 : string.Empty),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(feature3) ? feature3.Contains('"') ? feature3.Replace('"', ' ') : feature3 : string.Empty),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(feature4) ? feature4.Contains('"') ? feature4.Replace('"', ' ') : feature4 : string.Empty),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", d.LargeImageUrl),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", "")));

                                d.FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_Prime_Sports_" + fcountSport;

                                d.ExportDate = DateTime.Now;
                                if (shortcode == ConstantData.ED)
                                {
                                    d.Account1_Status = 1;
                                    d.Account1_AmazonActive = 1;
                                    d.Account1_ExportDate = DateTime.Now;
                                    Exportpath = d.Account1_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_ED_Prime_Sports_" + fcountSport;
                                }
                                else if (shortcode == ConstantData.EM)
                                {
                                    d.Account2_Status = 1;
                                    d.Account2_AmazonActive = 1;
                                    d.Account2_ExportDate = DateTime.Now;
                                    Exportpath = d.Account2_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_EM_Prime_Sports_" + fcountSport;
                                }
                                else if (shortcode == ConstantData.DI)
                                {
                                    d.Account4_Status = 1;
                                    d.Account4_AmazonActive = 1;
                                    d.Account4_ExportDate = DateTime.Now;
                                    Exportpath = d.Account4_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_DI_Prime_Sports_" + fcountSport;
                                }


                                d.Status = 1;
                                d.Instock = 1;

                            }

                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    // ukdb.SaveChanges();

                    ukdb.ObjectContext().SaveChanges(SaveOptions.DetectChangesBeforeSave);
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=" + Exportpath + ".txt");
                    //Response.AddHeader("content-disposition", "attachment;filename="+name +"_"+ DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
                    Response.ContentType = "application/text";
                    Response.ContentEncoding = Encoding.GetEncoding(1252);

                    fcountSport++;

                    Response.Write(sb.ToString());
                    Response.End();
                }



                transaction.Complete();
                ukdb.ObjectContext().AcceptAllChanges();
            }


        }
        public void ExportSportsCanada(double PriceValue, string shortcode, IEnumerable<tbl_Sports> data)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Suppress))
            {
                string Exportpath = "";

                ((IObjectContextAdapter)ukdb).ObjectContext.CommandTimeout = 1800;



                string header1 = "TemplateType=Sports	Version=2015.1204	The top 3 rows are for Amazon.com use only. Do not modify or delete the top 3 rows.								Offer - Offer Information - These attributes are required to make your item buyable for customers on the site.																		Dimensions - Product Dimensions - These attributes specify the size and weight of a product.										Discovery - Item discovery information - These attributes have an effect on how customers can find your product on the site using browse or search.							Images - Image Information - See Image Instructions tab for details.					Variation - Variation information - Populate these attributes if your product is available in different variations (for example colour or wattage).				Compliance - Compliance Information - Attributes used to comply with consumer laws in the country or region where the item is sold.		Ungrouped - These attributes create rich product listings for your buyers.";
                string header2 = "Seller SKU	Product ID	Product ID Type	Product Name	Product Description	Manufacturer	Manufacturer Part Number	Product Type	Brand	Update Delete	Standard Price	Item Condition	Condition Note	Quantity	Manufacturer's Suggested Retail Price	Launch Date	Release Date	Restock Date	Fulfillment Latency	Sale Price	Sale Start Date	Sale End Date	Offering Can Be Gift Messaged	Is Gift Wrap Available	Is Discontinued By Manufacturer	Number of Items	Product Tax Code	Shipping-Template	Shipping Weight	Website Shipping Weight Unit Of Measure	Item Display Height	Item Display Height Unit Of Measure	Item Display Length	Item Display Length Unit Of Measure	Item Display Width	Item Display Width Unit Of Measure	Item Display Weight	Item Display Weight Unit Of Measure	Bullet Point1	Bullet Point2	Bullet Point3	Bullet Point4	Bullet Point5	Recommended Browse Node	Search Terms	Main Image URL	Swatch Image URL	Other Image URL1	Other Image URL2	Other Image URL3	Parentage	Parent SKU	Relationship Type	Variation Theme	Safety Warning	Legal Disclaimer	Color	Grip Size	Grip Type	Hand	Lens Color	Shape	Size	Style	Tension Level	Golf Flex	Golf Loft	Shaft Length	Shaft Length Unit Of Measure	Shaft Material";
                string header3 = "item_sku	external_product_id	external_product_id_type	item_name	product_description	manufacturer	part_number	feed_product_type	brand_name	update_delete	standard_price	condition_type	condition_note	quantity	list_price	product_site_launch_date	merchant_release_date	restock_date	fulfillment_latency	sale_price	sale_from_date	sale_end_date	offering_can_be_gift_messaged	offering_can_be_giftwrapped	is_discontinued_by_manufacturer	number_of_items	product_tax_code	merchant_shipping_group_name	website_shipping_weight	website_shipping_weight_unit_of_measure	item_display_height	item_display_height_unit_of_measure	item_display_length	item_display_length_unit_of_measure	item_display_width	item_display_width_unit_of_measure	item_display_weight	item_display_weight_unit_of_measure	bullet_point1	bullet_point2	bullet_point3	bullet_point4	bullet_point5	recommended_browse_nodes	generic_keywords	main_image_url	swatch_image_url	other_image_url1	other_image_url2	other_image_url3	parent_child	parent_sku	relationship_type	variation_theme	safety_warning	legal_disclaimer_description	color_name	grip_size	grip_type	hand_orientation	lens_color	item_shape	size_name	style_name	tension_level	golf_club_flex	golf_club_loft	shaft_length	shaft_length_unit_of_measure	shaft_material";

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(header1);
                sb.AppendLine(header2);
                sb.AppendLine(header3);
               

                if (data != null)
                {
                    foreach (var d in data)
                    {
                        try
                        {


                            //Nullable<double> price;
                            double price = 0;
                            double pricecal;
                            double pricemin = 0.00;
                            double finalprice;
                            string UPC;
                            if (d.UPC == null)
                            {
                                upc_codes uc = ukdb.upc_codes.Where(x => x.AssignedTo == null).FirstOrDefault();
                                UPC = uc.Upc;
                                
                                UpdateUPC(uc.Upc, d.ASIN);
                            }
                            else
                            {
                                UPC = d.UPC;
                            }
                            if (d.UpdatedSalesPrice != null)
                            {
                                price = double.Parse(d.UpdatedSalesPrice);
                            }
                            else if (d.SalesPrice != null)
                            {
                                price = double.Parse(d.SalesPrice);
                            }

                            if (price != 0)
                            {
                                price = double.Parse(d.SalesPrice);

                                if (price < 300.01)
                                {

                                   

                                    if (price > 0 && price <= 19.99)
                                    {
                                        pricemin = 92.99;
                                    }
                                    else if (price > 19.99 && price <= 29.99)
                                    {
                                        pricemin = 120.99;
                                    }
                                    else if (price > 29.99 && price <= 39.99)
                                    {
                                        pricemin = 142.99;
                                    }
                                    else if (price > 39.99 && price <= 49.99)
                                    {
                                        pricemin = 168.99;
                                    }
                                    else if (price > 49.99 && price <= 59.99)
                                    {
                                        pricemin = 192.99;
                                    }
                                    else if (price > 59.99 && price <= 69.99)
                                    {
                                        pricemin = 222.99;
                                    }
                                    else if (price > 69.99 && price <= 79.99)
                                    {
                                        pricemin = 255.99;
                                    }
                                    else if (price > 79.99 && price <= 89.99)
                                    {
                                        pricemin = 285.99;
                                    }
                                    else if (price > 89.99 && price <= 99.99)
                                    {
                                        pricemin = 313.99;
                                    }
                                    else if (price > 99.99 && price <= 109.99)
                                    {
                                        pricemin = 327.99;
                                    }
                                    else if (price > 109.99 && price <= 119.99)
                                    {
                                        pricemin = 356.99;
                                    }
                                    else if (price > 119.99 && price <= 129.99)
                                    {
                                        pricemin = 385.99;
                                    }
                                    else if (price > 129.99 && price <= 139.99)
                                    {
                                        pricemin = 420.99;
                                    }
                                    else if (price > 139.99 && price <= 149.99)
                                    {
                                        pricemin = 442.99;
                                    }
                                    else if (price > 149.99 && price <= 159.99)
                                    {
                                        pricemin = 463.99;
                                    }
                                    else if (price > 159.99 && price <= 169.99)
                                    {
                                        pricemin = 485.99;
                                    }
                                    else if (price > 169.99 && price <= 179.99)
                                    {
                                        pricemin = 513.99;
                                    }
                                    else if (price > 179.99 && price <= 189.99)
                                    {
                                        pricemin = 542.99;
                                    }
                                    else if (price > 189.99 && price <= 199.99)
                                    {
                                        pricemin = 569.99;
                                    }



                                    pricecal = price * PriceValue;
                                    if (pricemin >= pricecal)
                                    {
                                        finalprice = pricemin;
                                    }
                                    else
                                    {
                                        finalprice = pricecal;
                                    }


                                    string ItemName;
                                    if (d.Title != null)
                                    {
                                        // ItemName = new string(d.Title.Take(490).ToArray());
                                        ItemName = d.Title.Substring(0, d.Title.Length > 499 ? 499 : d.Title.Length);
                                    }
                                    else
                                    {
                                        ItemName = null;
                                    }
                                    string description;

                                    if (d.Description != null)
                                    {
                                        string desc = Regex.Replace(d.Description, "<.*?>", String.Empty);
                                        //description = new string(desc.Take(1990).ToArray());
                                        description = desc.Substring(0, desc.Length > 1990 ? 1990 : desc.Length);
                                    }
                                    else
                                    {
                                        description = null;
                                    }

                                    string manufacturer;
                                    string brand;
                                    if (d.Manufacturer == null && d.Brand == null)
                                    {
                                        manufacturer = "Unknown";
                                        brand = "Unknown";
                                    }
                                    else
                                    {
                                        if (d.Manufacturer == null)
                                        {
                                            manufacturer = d.Brand;
                                        }
                                        else
                                        {
                                            manufacturer = d.Manufacturer;
                                        }

                                        if (d.Brand == null)
                                        {
                                            brand = d.Manufacturer;
                                        }
                                        else
                                        {
                                            brand = d.Brand;
                                        }

                                    }
                                    string feature1, feature2, feature3, feature4;
                                    if (d.Features1 != null)
                                    {
                                        // feature1 = new string(d.Features1.Take(500).ToArray());
                                        feature1 = d.Features1.Substring(0, d.Features1.Length > 500 ? 500 : d.Features1.Length);

                                    }
                                    else
                                        feature1 = d.Features1;
                                    if (d.Features2 != null)
                                    {
                                        // feature2 = new string(d.Features2.Take(500).ToArray());
                                        feature2 = d.Features2.Substring(0, d.Features2.Length > 500 ? 500 : d.Features2.Length);
                                    }
                                    else
                                        feature2 = d.Features2;
                                    if (d.Features3 != null)
                                    {
                                        //feature3 = new string(d.Features3.Take(500).ToArray());
                                        feature3 = d.Features3.Substring(0, d.Features3.Length > 500 ? 500 : d.Features3.Length);
                                    }
                                    else
                                        feature3 = d.Features3;
                                    if (d.Features4 != null)
                                    {
                                        // feature4 = new string(d.Features4.Take(500).ToArray());
                                        feature4 = d.Features4.Substring(0, d.Features4.Length > 500 ? 500 : d.Features4.Length);
                                    }
                                    else
                                        feature4 = d.Features4;


                                    sb.AppendLine(string.Join("\t",
                                                   string.Format(@"""{0}""", d.ASIN.Trim()),
                                                   string.Format(@"""{0}""", UPC),
                                                   string.Format(@"""{0}""", "UPC"),
                                                   string.Format(@"""{0}""", !string.IsNullOrEmpty(ItemName) ? ItemName.Contains('"') ? ItemName.Replace('"', ' ') : ItemName : string.Empty),
                                                   string.Format(@"""{0}""", !string.IsNullOrEmpty(description) ? description.Contains('"') ? description.Replace('"', ' ') : description : string.Empty),
                                                   string.Format(@"""{0}""", manufacturer),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", "SportingGoods"),
                                                   string.Format(@"""{0}""", brand),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", String.Format("{0:0.00}", finalprice)),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", Quantity),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", FulfillmentLatency),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", !string.IsNullOrEmpty(feature1) ? feature1.Contains('"') ? feature1.Replace('"', ' ') : feature1 : string.Empty),
                                                   string.Format(@"""{0}""", !string.IsNullOrEmpty(feature2) ? feature2.Contains('"') ? feature2.Replace('"', ' ') : feature2 : string.Empty),
                                                   string.Format(@"""{0}""", !string.IsNullOrEmpty(feature3) ? feature3.Contains('"') ? feature3.Replace('"', ' ') : feature3 : string.Empty),
                                                   string.Format(@"""{0}""", !string.IsNullOrEmpty(feature4) ? feature4.Contains('"') ? feature4.Replace('"', ' ') : feature4 : string.Empty),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", d.LargeImageUrl),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", "")));

                                    d.FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_Prime_Canada_Sports_" + canadaSport;

                                    d.ExportDate = DateTime.Now;
                                    if (shortcode == ConstantData.DC)
                                    {
                                        d.Account3_Status = 1;
                                        d.Account3_ExportDate = DateTime.Now;
                                        Exportpath = d.Account3_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_DC_Prime_Canada_Sports_" + canadaSport;
                                    }

                                    d.Status = 1;
                                    d.Instock = 1;

                                }


                            }


                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    //ukdb.SaveChanges();
                    ukdb.ObjectContext().SaveChanges(SaveOptions.DetectChangesBeforeSave);
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=" + Exportpath + ".txt");
                    //Response.AddHeader("content-disposition", "attachment;filename="+name +"_"+ DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
                    Response.ContentType = "application/text";
                    Response.ContentEncoding = Encoding.GetEncoding(1252);

                    canadaSport++;

                    Response.Write(sb.ToString());
                    Response.End();

                }
                transaction.Complete();
                ukdb.ObjectContext().AcceptAllChanges();
            }

        }
        public void ExportToys(double PriceValue, string shortcode, IEnumerable<tbl_Toys> data)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Suppress))
            {


                string Exportpath = "";

                StringWriter st = new StringWriter();

                string header1 = "TemplateType=toys	Version=2016.0505	The top 3 rows are for Amazon.com use only. Do not modify or delete the top 3 rows.								Offer-Offer Information - These attributes are required to make your item buyable for customers on the site.																		Dimensions-Product Dimensions - These attributes specify the size and weight of a product.														Discovery-Item discovery information - These attributes have an effect on how customers can find your product on the site using browse or search.								Images-Image Information - See Image Instructions tab for details.										Fulfillment-Use these columns to provide fulfilment-related information for orders fulfilled either by Amazon (FBA) or by the Seller.							Compliance-Compliance Information - Attributes used to comply with consumer laws in the country or region where the item is sold.			Ungrouped - These attributes create rich product listings for your buyers.";
                string header2 = "Seller SKU	Item Name (aka Title)	Product ID	Product ID Type	Feed Product Type	Brand Name	Manufacturer	Manufacturer Part Number	Product Description	Update Delete	Quantity	Standard Price	Condition Type	Offer Condition Note	Launch Date	Fulfillment Latency	Release Date	Sale Price	Sale From Date	Sale End Date	Number of Items	Stop Selling Date	Max Aggregate Ship Quantity	Product Tax Code	Can Be Gift Messaged	Is Gift Wrap Available?	Is Discontinued by Manufacturer	Merchant Shipping Group	Shipping Weight	Website Shipping Weight Unit Of Measure	Item Height	Item Length	Item Width	Item Length Unit Of Measure	Item Weight	Unit of measure of item weight	Display Weight	Item Display Weight Unit Of Measure	Display Volume	Item Display Volume Unit Of Measure	Display Length	Item Display Length Unit Of Measure	Key Product Features	Key Product Features	Key Product Features	Key Product Features	Key Product Features	Recommended Browse Nodes	Search Terms	Catalog Number	Main Image URL	Swatch Image Url	Other Image URL	Other Image URL	Other Image URL	Other Image URL	Other Image URL	Other Image URL	Other Image URL	Other Image URL	Fulfillment Centre ID	Package Height	Package Width	Package Length	Package Length Unit Of Measure	Package Weight	Package Weight Unit Of Measure	EU Toys Safety Directive Age-specific warning	EU Toys Safety Directive Non-Age-specific warning	EU Toys Safety Directive language warning	Parentage	Relationship Type	Parent SKU	Is Adult Product	Variation Theme	Colour	Colour Map	Size	Size Map	Manufacturer Warranty Description	Material Type	Product Care Instructions	Assembly Instructions	Minimum Age Recommendation	Mfg Minimum Unit Of Measure	Maximum Age Recommendation	Mfg Maximum Unit Of Measure	Target Gender	Special Features	Seller Warranty Description	Subject Character	Material Composition	Scale	Rail Gauge	Batteries are Included	BatteryType	Number of Batteries	Lithium Battery Voltage	Lithium Battery Weight	Lithium Battery Packaging	unit_count_type	unit_count";
                string header3 = "item_sku	item_name	external_product_id	external_product_id_type	feed_product_type	brand_name	manufacturer	part_number	product_description	update_delete	quantity	standard_price	condition_type	condition_note	product_site_launch_date	fulfillment_latency	merchant_release_date	sale_price	sale_from_date	sale_end_date	number_of_items	offering_end_date	max_aggregate_ship_quantity	product_tax_code	offering_can_be_gift_messaged	offering_can_be_giftwrapped	is_discontinued_by_manufacturer	merchant_shipping_group_name	website_shipping_weight	website_shipping_weight_unit_of_measure	item_height	item_length	item_width	item_length_unit_of_measure	item_weight	item_weight_unit_of_measure	item_display_weight	item_display_weight_unit_of_measure	item_display_volume	item_display_volume_unit_of_measure	item_display_length	item_display_length_unit_of_measure	bullet_point1	bullet_point2	bullet_point3	bullet_point4	bullet_point5	recommended_browse_nodes	generic_keywords	catalog_number	main_image_url	swatch_image_url	other_image_url1	other_image_url2	other_image_url3	other_image_url4	other_image_url5	other_image_url6	other_image_url7	other_image_url8	fulfillment_center_id	package_height	package_width	package_length	package_length_unit_of_measure	package_weight	package_weight_unit_of_measure	eu_toys_safety_directive_age_warning	eu_toys_safety_directive_warning	eu_toys_safety_directive_language	parent_child	relationship_type	parent_sku	is_adult_product	variation_theme	color_name	color_map	size_name	size_map	warranty_description	material_type	care_instructions	assembly_instructions	mfg_minimum	mfg_minimum_unit_of_measure	mfg_maximum	mfg_maximum_unit_of_measure	target_gender	special_features	seller_warranty_description	subject_character	material_composition	scale_name	rail_gauge	are_batteries_included	battery_type	number_of_batteries	lithium_battery_voltage	lithium_battery_weight	lithium_battery_packaging	unit_count_type	unit_count";


                StringBuilder sb = new StringBuilder();
                sb.AppendLine(header1);
                sb.AppendLine(header2);
                sb.AppendLine(header3);
                double addToMinVal = double.Parse(ConfigurationManager.AppSettings["ProdPriceAddToMinValUK"]);// 27-June-2016 making is configurable. It adds vaue accoding to range define in loop.

                ((IObjectContextAdapter)ukdb).ObjectContext.CommandTimeout = 1800;
                if (data != null)
                {
                    foreach (var d in data)
                    {
                        try
                        {

                            double price = 0;
                            double pricecal;
                            double pricemin = 0.00;
                            double finalprice;

                            string UPC;
                            if (d.UPC == null)
                            {
                                upc_codes uc = ukdb.upc_codes.Where(x => x.AssignedTo == null).FirstOrDefault();
                                UPC = uc.Upc;
                                
                                UpdateUPC(uc.Upc, d.ASIN);
                            }
                            else
                            {
                                UPC = d.UPC;
                            }

                            if (d.UpdatedSalesPrice != null)
                            {
                                price = double.Parse(d.UpdatedSalesPrice);
                            }
                            else
                                if (d.SalesPrice != null)
                                {
                                    price = double.Parse(d.SalesPrice);
                                }

                            if (price != 0)
                            {
                                double minval = double.Parse(ConfigurationManager.AppSettings["ProdPriceMinValUK"]);  // 27-June-2016 making it configurable. Minimum Product Price can Change from configuration file. 

                                if (price > 0 && price <= 19.99)
                                {
                                    pricemin = minval;
                                }
                                else
                                {
                                    for (double i = 20; i < 500; i += 10)
                                    {
                                        minval = minval + addToMinVal;
                                        double temp = i + 10;
                                        if (price >= i && price < temp)
                                        {
                                            pricemin = minval;
                                            break;
                                        }
                                    }
                                }
                                pricecal = price * PriceValue;
                                if (pricemin >= pricecal)
                                {
                                    finalprice = pricemin;
                                }
                                else
                                {
                                    finalprice = pricecal;
                                }



                                string ItemName;
                                if (d.Title != null)
                                {
                                    // ItemName = new string(d.Title.Take(490).ToArray());
                                    ItemName = d.Title.Substring(0, d.Title.Length > 499 ? 499 : d.Title.Length);
                                }
                                else
                                {
                                    ItemName = null;
                                }
                                string description;

                                if (d.Description != null)
                                {
                                    string desc = Regex.Replace(d.Description, "<.*?>", String.Empty);
                                    //description = new string(desc.Take(1990).ToArray());
                                    description = desc.Substring(0, desc.Length > 1990 ? 1990 : desc.Length);
                                }
                                else
                                {
                                    description = null;
                                }

                                string manufacturer;
                                string brand;
                                if (d.Manufacturer == null && d.Brand == null)
                                {
                                    manufacturer = "Unknown";
                                    brand = "Unknown";
                                }
                                else
                                {
                                    if (d.Manufacturer == null)
                                    {
                                        manufacturer = d.Brand;
                                    }
                                    else
                                    {
                                        manufacturer = d.Manufacturer;
                                    }

                                    if (d.Brand == null)
                                    {
                                        brand = d.Manufacturer;
                                    }
                                    else
                                    {
                                        brand = d.Brand;
                                    }

                                }
                                string feature1, feature2, feature3, feature4;
                                if (d.Features1 != null)
                                {
                                    // feature1 = new string(d.Features1.Take(500).ToArray());
                                    feature1 = d.Features1.Substring(0, d.Features1.Length > 500 ? 500 : d.Features1.Length);

                                }
                                else
                                    feature1 = d.Features1;
                                if (d.Features2 != null)
                                {
                                    // feature2 = new string(d.Features2.Take(500).ToArray());
                                    feature2 = d.Features2.Substring(0, d.Features2.Length > 500 ? 500 : d.Features2.Length);
                                }
                                else
                                    feature2 = d.Features2;
                                if (d.Features3 != null)
                                {
                                    //feature3 = new string(d.Features3.Take(500).ToArray());
                                    feature3 = d.Features3.Substring(0, d.Features3.Length > 500 ? 500 : d.Features3.Length);
                                }
                                else
                                    feature3 = d.Features3;
                                if (d.Features4 != null)
                                {
                                    // feature4 = new string(d.Features4.Take(500).ToArray());
                                    feature4 = d.Features4.Substring(0, d.Features4.Length > 500 ? 500 : d.Features4.Length);
                                }
                                else
                                    feature4 = d.Features4;

                                sb.AppendLine(string.Join("\t",
                                              string.Format(@"""{0}""", d.ASIN.Trim()),
                                              string.Format(@"""{0}""", !string.IsNullOrEmpty(ItemName) ? ItemName.Contains('"') ? ItemName.Replace('"', ' ') : ItemName : string.Empty),
                                              string.Format(@"""{0}""", UPC),
                                              string.Format(@"""{0}""", "UPC"),
                                              string.Format(@"""{0}""", "ToysAndGames"),
                                              string.Format(@"""{0}""", brand),
                                              string.Format(@"""{0}""", manufacturer),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", !string.IsNullOrEmpty(description) ? description.Contains('"') ? description.Replace('"', ' ') : description : string.Empty),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", Quantity),
                                              string.Format(@"""{0}""", String.Format("{0:0.00}", finalprice)),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", FulfillmentLatency),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", !string.IsNullOrEmpty(feature1) ? feature1.Contains('"') ? feature1.Replace('"', ' ') : feature1 : string.Empty),
                                              string.Format(@"""{0}""", !string.IsNullOrEmpty(feature2) ? feature2.Contains('"') ? feature2.Replace('"', ' ') : feature2 : string.Empty),
                                              string.Format(@"""{0}""", !string.IsNullOrEmpty(feature3) ? feature3.Contains('"') ? feature3.Replace('"', ' ') : feature3 : string.Empty),
                                              string.Format(@"""{0}""", !string.IsNullOrEmpty(feature4) ? feature4.Contains('"') ? feature4.Replace('"', ' ') : feature4 : string.Empty),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", d.LargeImageUrl),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", ""),
                                              string.Format(@"""{0}""", "")));

                                d.FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_Prime_Toys_" + fcountToys;

                                d.ExportDate = DateTime.Now;
                                if (shortcode == ConstantData.ED)
                                {
                                    d.Account1_Status = 1;
                                    d.Account1_AmazonActive = 1;
                                    d.Account1_ExportDate = DateTime.Now;
                                    Exportpath = d.Account1_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_ED_Prime_Toys_" + fcountToys;
                                }
                                else if (shortcode == ConstantData.EM)
                                {
                                    d.Account2_Status = 1;
                                    d.Account2_AmazonActive = 1;
                                    d.Account2_ExportDate = DateTime.Now;
                                    Exportpath = d.Account2_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_EM_Prime_Toys_" + fcountToys;
                                }
                                else if (shortcode == ConstantData.DI)
                                {
                                    d.Account4_Status = 1;
                                    d.Account4_AmazonActive = 1;
                                    d.Account4_ExportDate = DateTime.Now;
                                    Exportpath = d.Account4_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_DI_Prime_Toys_" + fcountToys;
                                }

                                d.Instock = 1;
                                d.Status = 1;

                            }

                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    ukdb.ObjectContext().SaveChanges(SaveOptions.DetectChangesBeforeSave); //  Temporarily saves all updated fields

                    fcountToys++;
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=" + Exportpath + ".txt");
                    Response.ContentType = "application/text";
                    Response.ContentEncoding = Encoding.GetEncoding(1252);
                    Response.Write(sb.ToString());
                    Response.End();

                }

                transaction.Complete();
                ukdb.ObjectContext().AcceptAllChanges(); //Saves completely if after transaction complete

            }
        }
        public void ExportToysCanada(double PriceValue, string shortcode, IEnumerable<tbl_Toys> data)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Suppress))
            {


                string Exportpath = "";



                StringWriter st = new StringWriter();

                string header1 = "TemplateType=Toys	Version=2016.0108	The top 3 rows are for Amazon.com use only. Do not modify or delete the top 3 rows.									Offer - Offer Information - These attributes are required to make your item buyable for customers on the site.																					Dimensions - Product Dimensions - These attributes specify the size and weight of a product.								Discovery - Item discovery information - These attributes have an effect on how customers can find your product on the site using browse or search.														Images - Image Information - See Image Instructions tab for details.					Fulfillment - Use these columns to provide fulfillment-related information for either Amazon-fulfilled (FBA) or seller-fulfilled orders.							Compliance - Compliance Information - Attributes used to comply with consumer laws in the country or region where the item is sold.					Ungrouped - These attributes create rich product listings for your buyers.";
                string header2 = "Seller SKU	Product ID	Product ID Type	Product Type	Product Name	Brand Name	Manufacturer	Manufacturer Part Number	Product Description	Edition	Update Delete	Product Tax Code	Launch Date	Stop Selling Date	Item Condition	Offer Condition Note	Is Gift Wrap Available	Offering Can Be Gift Messaged	Minimum Advertised Price	Manufacturer's Suggested Retail Price	Standard Price	Quantity	Release Date	Fulfillment Latency	Restock Date	Sale Price	Sale Start Date	Sale End Date	Package Quantity	Max Aggregate Ship Quantity	Is Discontinued by Manufacturer	Shipping-Template	Item Weight	Item Weight Unit Of Measure	Item Height	Item Length	Item Width	Item Length Unit Of Measure	Shipping Weight	Website Shipping Weight Unit Of Measure	recommended-browse-nodes	Key Product Features1	Key Product Features2	Key Product Features3	Key Product Features4	Key Product Features5	Intended Use1	Intended Use2	Intended Use3	Intended Use4	Intended Use5	Target Audience	Search Terms	Style-specific Terms	Swatch Image URL	Main Image URL	Other Image URL1	Other Image URL2	Other Image URL3	Fulfillment Center ID	Package Length	Package Width	Package Height	Package Length Unit Of Measure	Package Weight	Package Weight Unit Of Measure	Cpsia Warning	CPSIA Warning Description	Legal Disclaimer	Safety Warning	Country of Publication	Minimum Manufacturer Age Recommended	Minimum Manufacturer Age Recommended Unit Of Measure	Maximum Manufacturer Age Recommended	Maximum Manufacturer Age Recommended Unit Of Measure	Other Attributes	Theme	Character	Educational Objective	Size	Size Map	Specific Uses For Product	Genre	Material Type	Assembly Time	Assembly Time Unit Of Measure	Manufacturer Warranty Description	Number of Pieces	Batteries are Included	Battery Type	Number of Batteries Required	Weight Supported	Maximum Weight Recommendation Unit Of Measure";
                string header3 = "item_sku	external_product_id	external_product_id_type	feed_product_type	item_name	brand_name	manufacturer	part_number	product_description	edition	update_delete	product_tax_code	product_site_launch_date	offering_end_date	condition_type	condition_note	offering_can_be_giftwrapped	offering_can_be_gift_messaged	map_price	list_price	standard_price	quantity	merchant_release_date	fulfillment_latency	restock_date	sale_price	sale_from_date	sale_end_date	item_package_quantity	max_aggregate_ship_quantity	is_discontinued_by_manufacturer	merchant_shipping_group_name	item_weight	item_weight_unit_of_measure	item_height	item_length	item_width	item_length_unit_of_measure	website_shipping_weight	website_shipping_weight_unit_of_measure	recommended_browse_nodes	bullet_point1	bullet_point2	bullet_point3	bullet_point4	bullet_point5	specific_uses_keywords1	specific_uses_keywords2	specific_uses_keywords3	specific_uses_keywords4	specific_uses_keywords5	target_audience_keywords	generic_keywords	style_keywords	swatch_image_url	main_image_url	other_image_url1	other_image_url2	other_image_url3	fulfillment_center_id	package_length	package_width	package_height	package_length_unit_of_measure	package_weight	package_weight_unit_of_measure	cpsia_cautionary_statement	cpsia_cautionary_description	legal_disclaimer_description	safety_warning	country_of_origin	mfg_minimum	mfg_minimum_unit_of_measure	mfg_maximum	mfg_maximum_unit_of_measure	thesaurus_attribute_keywords	theme	subject_character	educational_objective	size_name	size_map	specific_uses_for_product	genre	material_type	assembly_time	assembly_time_unit_of_measure	warranty_description	number_of_pieces	are_batteries_included	battery_type	number_of_batteries	maximum_weight_recommendation	maximum_weight_recommendation_unit_of_measure";


                StringBuilder sb = new StringBuilder();
                sb.AppendLine(header1);
                sb.AppendLine(header2);
                sb.AppendLine(header3);
                ((IObjectContextAdapter)ukdb).ObjectContext.CommandTimeout = 1800;
                if (data != null)
                {
                    foreach (var d in data)
                    {
                        try
                        {


                            double price = 0;
                            double pricecal;
                            double pricemin = 0.00;
                            double finalprice;

                            string UPC;
                            if (d.UPC == null)
                            {
                                upc_codes uc = ukdb.upc_codes.Where(x => x.AssignedTo == null).FirstOrDefault();
                                UPC = uc.Upc;
                                
                                UpdateUPC(uc.Upc, d.ASIN);
                            }
                            else
                            {
                                UPC = d.UPC;
                            }
                            if (d.UpdatedSalesPrice != null)
                            {
                                price = double.Parse(d.UpdatedSalesPrice);
                            }
                            else if (d.SalesPrice != null)
                            {
                                price = double.Parse(d.SalesPrice);
                            }

                            if (price != 0)
                            {

                                if (price < 200.01)
                                {

                                    double minval = 0.00;

                                    if (price > 0 && price <= 19.99)
                                    {
                                        pricemin = 92.99;
                                    }
                                    else if (price > 19.99 && price <= 29.99)
                                    {
                                        pricemin = 120.99;
                                    }
                                    else if (price > 29.99 && price <= 39.99)
                                    {
                                        pricemin = 142.99;
                                    }
                                    else if (price > 39.99 && price <= 49.99)
                                    {
                                        pricemin = 168.99;
                                    }
                                    else if (price > 49.99 && price <= 59.99)
                                    {
                                        pricemin = 192.99;
                                    }
                                    else if (price > 59.99 && price <= 69.99)
                                    {
                                        pricemin = 222.99;
                                    }
                                    else if (price > 69.99 && price <= 79.99)
                                    {
                                        pricemin = 255.99;
                                    }
                                    else if (price > 79.99 && price <= 89.99)
                                    {
                                        pricemin = 285.99;
                                    }
                                    else if (price > 89.99 && price <= 99.99)
                                    {
                                        pricemin = 313.99;
                                    }
                                    else if (price > 99.99 && price <= 109.99)
                                    {
                                        pricemin = 327.99;
                                    }
                                    else if (price > 109.99 && price <= 119.99)
                                    {
                                        pricemin = 356.99;
                                    }
                                    else if (price > 119.99 && price <= 129.99)
                                    {
                                        pricemin = 385.99;
                                    }
                                    else if (price > 129.99 && price <= 139.99)
                                    {
                                        pricemin = 420.99;
                                    }
                                    else if (price > 139.99 && price <= 149.99)
                                    {
                                        pricemin = 442.99;
                                    }
                                    else if (price > 149.99 && price <= 159.99)
                                    {
                                        pricemin = 463.99;
                                    }
                                    else if (price > 159.99 && price <= 169.99)
                                    {
                                        pricemin = 485.99;
                                    }
                                    else if (price > 169.99 && price <= 179.99)
                                    {
                                        pricemin = 513.99;
                                    }
                                    else if (price > 179.99 && price <= 189.99)
                                    {
                                        pricemin = 542.99;
                                    }
                                    else if (price > 189.99 && price <= 199.99)
                                    {
                                        pricemin = 569.99;
                                    }


                                    pricecal = price * PriceValue;
                                    if (pricemin >= pricecal)
                                    {
                                        finalprice = pricemin;
                                    }
                                    else
                                    {
                                        finalprice = pricecal;
                                    }



                                    string ItemName;
                                    if (d.Title != null)
                                    {
                                        // ItemName = new string(d.Title.Take(490).ToArray());
                                        ItemName = d.Title.Substring(0, d.Title.Length > 499 ? 499 : d.Title.Length);
                                    }
                                    else
                                    {
                                        ItemName = null;
                                    }
                                    string description;

                                    if (d.Description != null)
                                    {
                                        string desc = Regex.Replace(d.Description, "<.*?>", String.Empty);
                                        //description = new string(desc.Take(1990).ToArray());
                                        description = desc.Substring(0, desc.Length > 1990 ? 1990 : desc.Length);
                                    }
                                    else
                                    {
                                        description = null;
                                    }

                                    string manufacturer;
                                    string brand;
                                    if (d.Manufacturer == null && d.Brand == null)
                                    {
                                        manufacturer = "Unknown";
                                        brand = "Unknown";
                                    }
                                    else
                                    {
                                        if (d.Manufacturer == null)
                                        {
                                            manufacturer = d.Brand;
                                        }
                                        else
                                        {
                                            manufacturer = d.Manufacturer;
                                        }

                                        if (d.Brand == null)
                                        {
                                            brand = d.Manufacturer;
                                        }
                                        else
                                        {
                                            brand = d.Brand;
                                        }

                                    }
                                    string feature1, feature2, feature3, feature4;
                                    if (d.Features1 != null)
                                    {
                                        // feature1 = new string(d.Features1.Take(500).ToArray());
                                        feature1 = d.Features1.Substring(0, d.Features1.Length > 500 ? 500 : d.Features1.Length);

                                    }
                                    else
                                        feature1 = d.Features1;
                                    if (d.Features2 != null)
                                    {
                                        // feature2 = new string(d.Features2.Take(500).ToArray());
                                        feature2 = d.Features2.Substring(0, d.Features2.Length > 500 ? 500 : d.Features2.Length);
                                    }
                                    else
                                        feature2 = d.Features2;
                                    if (d.Features3 != null)
                                    {
                                        //feature3 = new string(d.Features3.Take(500).ToArray());
                                        feature3 = d.Features3.Substring(0, d.Features3.Length > 500 ? 500 : d.Features3.Length);
                                    }
                                    else
                                        feature3 = d.Features3;
                                    if (d.Features4 != null)
                                    {
                                        // feature4 = new string(d.Features4.Take(500).ToArray());
                                        feature4 = d.Features4.Substring(0, d.Features4.Length > 500 ? 500 : d.Features4.Length);
                                    }
                                    else
                                        feature4 = d.Features4;

                                    sb.AppendLine(string.Join("\t",
                                                  string.Format(@"""{0}""", d.ASIN.Trim()),
                                                  string.Format(@"""{0}""", UPC),
                                                  string.Format(@"""{0}""", "UPC"),
                                                  string.Format(@"""{0}""", "ToysAndGames"),
                                                  string.Format(@"""{0}""", !string.IsNullOrEmpty(ItemName) ? ItemName.Contains('"') ? ItemName.Replace('"', ' ') : ItemName : string.Empty),
                                                  string.Format(@"""{0}""", brand),
                                                  string.Format(@"""{0}""", manufacturer),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", !string.IsNullOrEmpty(description) ? description.Contains('"') ? description.Replace('"', ' ') : description : string.Empty),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", String.Format("{0:0.00}", finalprice)),
                                                  string.Format(@"""{0}""", Quantity),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", FulfillmentLatency),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", !string.IsNullOrEmpty(feature1) ? feature1.Contains('"') ? feature1.Replace('"', ' ') : feature1 : string.Empty),
                                                  string.Format(@"""{0}""", !string.IsNullOrEmpty(feature2) ? feature2.Contains('"') ? feature2.Replace('"', ' ') : feature2 : string.Empty),
                                                  string.Format(@"""{0}""", !string.IsNullOrEmpty(feature3) ? feature3.Contains('"') ? feature3.Replace('"', ' ') : feature3 : string.Empty),
                                                  string.Format(@"""{0}""", !string.IsNullOrEmpty(feature4) ? feature4.Contains('"') ? feature4.Replace('"', ' ') : feature4 : string.Empty),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", d.LargeImageUrl),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", ""),
                                                  string.Format(@"""{0}""", "")));

                                    d.FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_Prime_Canada_Toys_" + canadatoys;

                                    d.ExportDate = DateTime.Now;
                                    if (shortcode == ConstantData.DC)
                                    {
                                        d.Account3_Status = 1;
                                        d.Account3_ExportDate = DateTime.Now;
                                        Exportpath = d.Account3_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_DC_Prime_Canada_Toys_" + canadatoys;
                                    }

                                    d.Instock = 1;
                                    d.Status = 1;

                                }
                            }
                        }
                        catch (Exception ex)
                        {


                        }
                    }
                    ukdb.ObjectContext().SaveChanges(SaveOptions.DetectChangesBeforeSave);

                    //  ukdb.SaveChanges();
                    canadatoys++;
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=" + Exportpath + ".txt");
                    //Response.AddHeader("content-disposition", "attachment;filename="+name +"_"+ DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
                    Response.ContentType = "application/text";
                    Response.ContentEncoding = Encoding.GetEncoding(1252);
                    Response.Write(sb.ToString());
                    Response.End();
                }

                transaction.Complete();
                ukdb.ObjectContext().AcceptAllChanges();
            }
        }
        public void ExportBeauty(double PriceValue, string shortcode, IEnumerable<tbl_Beauty> data)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Suppress))
            {


                string Exportpath = "";
                ((IObjectContextAdapter)ukdb).ObjectContext.CommandTimeout = 1800;

                StringWriter st = new StringWriter();
                //string header1 = "TemplateType=Beauty	Version=2015.1217	The top 3 rows are for Amazon.com use only. Do not modify or delete the top 3 rows.								Offer - Offer Information - These attributes are required to make your item buyable for customers on the site.																	Dimensions - Product Dimensions - These attributes specify the size and weight of a product.														Discovery - Item discovery information - These attributes have an effect on how customers can find your product on the site using browse or search.							Images - Image Information - See Image Instructions tab for details.					Fulfillment - Use these columns to provide fulfilment-related information for orders fulfilled either by Amazon (FBA) or by the Seller.	Variation - Variation information - Populate these attributes if your product is available in different variations (for example colour or wattage).				Ungrouped - These attributes create rich product listings for your buyers.";
                //string header2 = "SKU	Item Name (aka Title)	Product Type	Product ID	Product ID Type	Brand Name	Manufacturer	Manufacturer Part Number	Product Description	Update Delete	Standard Price	Quantity	Fulfillment Latency	Package Quantity	Number of Items	Launch Date	Release Date	Is Discontinued by Manufacturer	Sale Price	Sale From Date	Sale End Date	Max Order Quantity	Max Aggregate Ship Quantity	Can Be Gift Messaged	Is Gift Wrap Available?	Product Tax Code	Merchant Shipping Group	Item Display Weight Unit Of Measure	Display Weight	Item Display Volume Unit Of Measure	Display Volume	Display Length	Item Display Length Unit Of Measure	Item Weight Unit Of Measure	Item Weight	Item Length Unit Of Measure	Item Length	Item Width	Item Height	Website Shipping Weight Unit Of Measure	Shipping Weight	Recommended Browse Nodes	Key Product Features1	Key Product Features2	Key Product Features3	Key Product Features4	Key Product Features5	Search Terms	Main Image URL	Swatch Image URL	Other Image URL1	Other Image URL2	Other Image URL3	Fulfillment Centre ID	Parentage	Parent SKU	Relationship Type	Variation Theme	Ingredients	Material Type	Item Form	Is Adult Product	Target Gender	Skin Type	Hair Type	Indications	Directions	Size	Colour	Colour Map	Scent	Sun Protection Factor";
                //string header3 = "item_sku	item_name	feed_product_type	external_product_id	external_product_id_type	brand_name	manufacturer	part_number	product_description	update_delete	standard_price	quantity	fulfillment_latency	item_package_quantity	number_of_items	product_site_launch_date	merchant_release_date	is_discontinued_by_manufacturer	sale_price	sale_from_date	sale_end_date	max_order_quantity	max_aggregate_ship_quantity	offering_can_be_gift_messaged	offering_can_be_giftwrapped	product_tax_code	merchant_shipping_group_name	item_display_weight_unit_of_measure	item_display_weight	item_display_volume_unit_of_measure	item_display_volume	item_display_length	item_display_length_unit_of_measure	item_weight_unit_of_measure	item_weight	item_length_unit_of_measure	item_length	item_width	item_height	website_shipping_weight_unit_of_measure	website_shipping_weight	recommended_browse_nodes	bullet_point1	bullet_point2	bullet_point3	bullet_point4	bullet_point5	generic_keywords	main_image_url	swatch_image_url	other_image_url1	other_image_url2	other_image_url3	fulfillment_center_id	parent_child	parent_sku	relationship_type	variation_theme	ingredients	material_type	item_form	is_adult_product	target_gender	skin_type	hair_type	indications	directions	size_name	color_name	color_map	scent_name	sun_protection";

                string header1 = "TemplateType=beauty	Version=2016.0324	The top 3 rows are for Amazon.com use only. Do not modify or delete the top 3 rows.								Offer-Offer Information - These attributes are required to make your item buyable for customers on the site.																	Dimensions-Product Dimensions - These attributes specify the size and weight of a product.														Discovery-Item discovery information - These attributes have an effect on how customers can find your product on the site using browse or search.							Images-Image Information - See Image Instructions tab for details.					Fulfillment-Use these columns to provide fulfilment-related information for orders fulfilled either by Amazon (FBA) or by the Seller.	Variation-Variation information - Populate these attributes if your product is available in different variations (for example colour or wattage).				Ungrouped - These attributes create rich product listings for your buyers.";
                string header2 = "Seller SKU	Item Name (aka Title)	Product Type	Product ID	Product ID Type	Brand Name	Manufacturer	Manufacturer Part Number	Product Description	Update Delete	Standard Price	Quantity	Fulfillment Latency	Package Quantity	Number of Items	Launch Date	Release Date	Is Discontinued by Manufacturer	Sale Price	Sale From Date	Sale End Date	Max Order Quantity	Max Aggregate Ship Quantity	Can Be Gift Messaged	Is Gift Wrap Available?	Product Tax Code	Merchant Shipping Group	Item Display Weight Unit Of Measure	Display Weight	Item Display Volume Unit Of Measure	Display Volume	Display Length	Item Display Length Unit Of Measure	Item Weight Unit Of Measure	Item Weight	Item Length Unit Of Measure	Item Length	Item Width	Item Height	Website Shipping Weight Unit Of Measure	Shipping Weight	Recommended Browse Nodes	Key Product Features	Key Product Features	Key Product Features	Key Product Features	Key Product Features	Search Terms	Main Image URL	Swatch Image URL	Other Image URL	Other Image URL	Other Image URL	Fulfillment Centre ID	Parentage	Parent SKU	Relationship Type	Variation Theme	Ingredients	Material Type	Item Form	Is Adult Product	Target Gender	Skin Type	Hair Type	Indications	Directions	Size	Colour	Colour Map	Scent	Sun Protection Factor	Medicine Classification";
                string header3 = "item_sku	item_name	feed_product_type	external_product_id	external_product_id_type	brand_name	manufacturer	part_number	product_description	update_delete	standard_price	quantity	fulfillment_latency	item_package_quantity	number_of_items	product_site_launch_date	merchant_release_date	is_discontinued_by_manufacturer	sale_price	sale_from_date	sale_end_date	max_order_quantity	max_aggregate_ship_quantity	offering_can_be_gift_messaged	offering_can_be_giftwrapped	product_tax_code	merchant_shipping_group_name	item_display_weight_unit_of_measure	item_display_weight	item_display_volume_unit_of_measure	item_display_volume	item_display_length	item_display_length_unit_of_measure	item_weight_unit_of_measure	item_weight	item_length_unit_of_measure	item_length	item_width	item_height	website_shipping_weight_unit_of_measure	website_shipping_weight	recommended_browse_nodes	bullet_point1	bullet_point2	bullet_point3	bullet_point4	bullet_point5	generic_keywords	main_image_url	swatch_image_url	other_image_url1	other_image_url2	other_image_url3	fulfillment_center_id	parent_child	parent_sku	relationship_type	variation_theme	ingredients	material_type	item_form	is_adult_product	target_gender	skin_type	hair_type	indications	directions	size_name	color_name	color_map	scent_name	sun_protection	medicine_classification";


                StringBuilder sb = new StringBuilder();
                sb.AppendLine(header1);
                sb.AppendLine(header2);
                sb.AppendLine(header3);
                double addToMinVal = double.Parse(ConfigurationManager.AppSettings["ProdPriceAddToMinValUK"]);// 27-June-2016 making is configurable. It adds vaue accoding to range define in loop.

                //IEnumerable<tbl_Beauty> data = ukdb.tbl_Beauty.Where(x => x.UPC != "null" && x.Status == 0 && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(count.ToString())).ToList();
                if (data != null)
                {
                    foreach (var d in data)
                    {
                        try
                        {


                            double price = 0;
                            double pricecal;
                            double pricemin = 0.00;
                            double finalprice;
                            string UPC;
                            if (d.UPC == null)
                            {
                                upc_codes uc = ukdb.upc_codes.Where(x => x.AssignedTo == null).FirstOrDefault();
                                UPC = uc.Upc;
                                
                                UpdateUPC(uc.Upc, d.ASIN);
                            }
                            else
                            {
                                UPC = d.UPC;
                            }
                            if (d.UpdatedSalesPrice != null)
                            {
                                price = double.Parse(d.UpdatedSalesPrice);
                            }
                            else
                                if (d.SalesPrice != null)
                                {
                                    price = double.Parse(d.SalesPrice);
                                }

                            if (price != 0)
                            {
                                double minval = double.Parse(ConfigurationManager.AppSettings["ProdPriceMinValUK"]);  // 27-June-2016 making it configurable. Minimum Product Price can Change from configuration file. 
                               

                                if (price > 0 && price <= 19.99)
                                {
                                    pricemin = minval;
                                }
                                else
                                {
                                    for (double i = 20; i < 500; i += 10)
                                    {
                                        minval = minval + addToMinVal;
                                        double temp = i + 10;
                                        if (price >= i && price < temp)
                                        {
                                            pricemin = minval;
                                            break;
                                        }
                                    }
                                }
                                pricecal = price * PriceValue;
                                if (pricemin >= pricecal)
                                {
                                    finalprice = pricemin;
                                }
                                else
                                {
                                    finalprice = pricecal;
                                }


                                string ItemName;
                                if (d.Title != null)
                                {
                                    // ItemName = new string(d.Title.Take(490).ToArray());
                                    ItemName = d.Title.Substring(0, d.Title.Length > 499 ? 499 : d.Title.Length);
                                }
                                else
                                {
                                    ItemName = null;
                                }
                                string description;

                                if (d.Description != null)
                                {
                                    string desc = Regex.Replace(d.Description, "<.*?>", String.Empty);
                                    //description = new string(desc.Take(1990).ToArray());
                                    description = desc.Substring(0, desc.Length > 1990 ? 1990 : desc.Length);
                                }
                                else
                                {
                                    description = null;
                                }

                                string manufacturer;
                                string brand;
                                if (d.Manufacturer == null && d.Brand == null)
                                {
                                    manufacturer = "Unknown";
                                    brand = "Unknown";
                                }
                                else
                                {
                                    if (d.Manufacturer == null)
                                    {
                                        manufacturer = d.Brand;
                                    }
                                    else
                                    {
                                        manufacturer = d.Manufacturer;
                                    }

                                    if (d.Brand == null)
                                    {
                                        brand = d.Manufacturer;
                                    }
                                    else
                                    {
                                        brand = d.Brand;
                                    }

                                }
                                string feature1, feature2, feature3, feature4;
                                if (d.Features1 != null)
                                {
                                    // feature1 = new string(d.Features1.Take(500).ToArray());
                                    feature1 = d.Features1.Substring(0, d.Features1.Length > 500 ? 500 : d.Features1.Length);

                                }
                                else
                                    feature1 = d.Features1;
                                if (d.Features2 != null)
                                {
                                    // feature2 = new string(d.Features2.Take(500).ToArray());
                                    feature2 = d.Features2.Substring(0, d.Features2.Length > 500 ? 500 : d.Features2.Length);
                                }
                                else
                                    feature2 = d.Features2;
                                if (d.Features3 != null)
                                {
                                    //feature3 = new string(d.Features3.Take(500).ToArray());
                                    feature3 = d.Features3.Substring(0, d.Features3.Length > 500 ? 500 : d.Features3.Length);
                                }
                                else
                                    feature3 = d.Features3;
                                if (d.Features4 != null)
                                {
                                    // feature4 = new string(d.Features4.Take(500).ToArray());
                                    feature4 = d.Features4.Substring(0, d.Features4.Length > 500 ? 500 : d.Features4.Length);
                                }
                                else
                                    feature4 = d.Features4;

                                sb.AppendLine(string.Join("\t",
                                               string.Format(@"""{0}""", d.ASIN.Trim()),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(ItemName) ? ItemName.Contains('"') ? ItemName.Replace('"', ' ') : ItemName : string.Empty),
                                               string.Format(@"""{0}""", "BeautyMisc"),
                                               string.Format(@"""{0}""", UPC),
                                               string.Format(@"""{0}""", "UPC"),
                                               string.Format(@"""{0}""", brand),
                                               string.Format(@"""{0}""", manufacturer),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(description) ? description.Contains('"') ? description.Replace('"', ' ') : description : string.Empty),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", String.Format("{0:0.00}", finalprice)),
                                               string.Format(@"""{0}""", Quantity),
                                               string.Format(@"""{0}""", FulfillmentLatency),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(feature1) ? feature1.Contains('"') ? feature1.Replace('"', ' ') : feature1 : string.Empty),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(feature2) ? feature2.Contains('"') ? feature2.Replace('"', ' ') : feature2 : string.Empty),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(feature3) ? feature3.Contains('"') ? feature3.Replace('"', ' ') : feature3 : string.Empty),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(feature4) ? feature4.Contains('"') ? feature4.Replace('"', ' ') : feature4 : string.Empty),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", d.LargeImageUrl),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", "")));

                                d.FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_Prime_Beauty_" + fcountBeauty;

                                d.ExportDate = DateTime.Now;
                                d.Instock = 1;
                                if (shortcode == ConstantData.ED)
                                {
                                    d.Account1_Status = 1;
                                    d.Account1_AmazonActive = 1;
                                    d.Account1_ExportDate = DateTime.Now;
                                    Exportpath = d.Account1_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_ED_Prime_Beauty_" + fcountBeauty;
                                }
                                else if (shortcode == ConstantData.EM)
                                {
                                    d.Account2_Status = 1;
                                    d.Account2_AmazonActive = 1;
                                    d.Account2_ExportDate = DateTime.Now;
                                    Exportpath = d.Account2_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_EM_Prime_Beauty_" + fcountBeauty;
                                }
                                else if (shortcode == ConstantData.DI)
                                {
                                    d.Account4_Status = 1;
                                    d.Account4_AmazonActive = 1;
                                    d.Account4_ExportDate = DateTime.Now;
                                    Exportpath = d.Account4_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_DI_Prime_Beauty_" + fcountBeauty;
                                }
                                d.Status = 1;


                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    ukdb.ObjectContext().SaveChanges(SaveOptions.DetectChangesBeforeSave);

                    // ukdb.SaveChanges();
                    fcountBeauty++;
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=" + Exportpath + ".txt");
                    //Response.AddHeader("content-disposition", "attachment;filename="+name +"_"+ DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
                    Response.ContentType = "application/text";
                    Response.ContentEncoding = Encoding.GetEncoding(1252);
                    Response.Write(sb.ToString());
                    Response.End();
                }
                transaction.Complete();
                ukdb.ObjectContext().AcceptAllChanges();

            }
        }
        public void ExportBeautyCanada(double PriceValue, string shortcode, IEnumerable<tbl_Beauty> data)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Suppress))
            {


                string Exportpath = "";
                ((IObjectContextAdapter)ukdb).ObjectContext.CommandTimeout = 1800;

                StringWriter st = new StringWriter();
                string header1 = "TemplateType=Beauty	Version=2015.1204	The top 3 rows are for Amazon.com use only. Do not modify or delete the top 3 rows.								Offer - Offer Information - These attributes are required to make your item buyable for customers on the site.																		Dimensions - Product Dimensions - These attributes specify the size and weight of a product.								Discovery - Item discovery information - These attributes have an effect on how customers can find your product on the site using browse or search.								Images - Image Information - See Image Instructions tab for details.					Fulfillment - Use these columns to provide fulfillment-related information for either Amazon-fulfilled (FBA) or seller-fulfilled orders.	Variation - Variation information - Populate these attributes if your product is available in different variations (for example colour or wattage).				Compliance - Compliance Information - Attributes used to comply with consumer laws in the country or region where the item is sold.		Ungrouped - These attributes create rich product listings for your buyers.";
                string header2 = "SKU	Product ID	Product ID Type	Product Name	Brand Name	Manufacturer	Manufacturer Part Number	Product Description	Product Type	Update Delete	Quantity	Standard Price	Max Order Quantity	Fulfillment Latency	Restock Date	Is Discontinued by Manufacturer	Max Aggregate Ship Quantity	Product Tax Code	Launch Date	Release Date	Manufacturer's Suggested Retail Price	Sale Price	Sale Start Date	Sale End Date	Package Quantity	Offering Can Be Gift Messaged	Is Gift Wrap Available	Shipping-Template	Website Shipping Weight Unit Of Measure	Shipping Weight	Item Weight Unit Of Measure	Item Weight	Item Length Unit Of Measure	Item Length	Item Width	Item Height	Key Product Features1	Key Product Features2	Key Product Features3	Key Product Features4	Key Product Features5	recommended-browse-nodes	Target Audience	Search Terms	Main Image URL	Swatch Image URL	Other Image URL1	Other Image URL2	Other Image URL3	Fulfillment Center ID	Parentage	Parent SKU	Relationship Type	Variation Theme	Legal Disclaimer	Safety Warning	Size	Color	Scent Name	Skin Type	Coverage	Material Type	Hair Type	Target Gender	Item Form	Specialty	Unit Count Type	Batteries are Included	Battery Type	Number of Batteries Required	Lithium Battery Packaging	Lithium Battery Voltage	Lithium Battery Weight	Colour Map";
                string header3 = "item_sku	external_product_id	external_product_id_type	item_name	brand_name	manufacturer	part_number	product_description	feed_product_type	update_delete	quantity	standard_price	max_order_quantity	fulfillment_latency	restock_date	is_discontinued_by_manufacturer	max_aggregate_ship_quantity	product_tax_code	product_site_launch_date	merchant_release_date	list_price	sale_price	sale_from_date	sale_end_date	item_package_quantity	offering_can_be_gift_messaged	offering_can_be_giftwrapped	merchant_shipping_group_name	website_shipping_weight_unit_of_measure	website_shipping_weight	item_weight_unit_of_measure	item_weight	item_length_unit_of_measure	item_length	item_width	item_height	bullet_point1	bullet_point2	bullet_point3	bullet_point4	bullet_point5	recommended_browse_nodes	target_audience_keywords	generic_keywords	main_image_url	swatch_image_url	other_image_url1	other_image_url2	other_image_url3	fulfillment_center_id	parent_child	parent_sku	relationship_type	variation_theme	legal_disclaimer_description	safety_warning	size_name	color_name	scent_name	skin_type	coverage	material_type	hair_type	target_gender	item_form	specialty	unit_count_type	are_batteries_included	battery_type	number_of_batteries	lithium_battery_packaging	lithium_battery_voltage	lithium_battery_weight	color_map";

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(header1);
                sb.AppendLine(header2);
                sb.AppendLine(header3);

                if (data != null)
                {
                    foreach (var d in data)
                    {
                        try
                        {


                            double price = 0;
                            double pricecal;
                            double pricemin = 0.00;
                            double finalprice;
                            string UPC;
                            if (d.UPC == null)
                            {
                                upc_codes uc = ukdb.upc_codes.Where(x => x.AssignedTo == null).FirstOrDefault();
                                UPC = uc.Upc;
                                
                                UpdateUPC(uc.Upc, d.ASIN);
                            }
                            else
                            {
                                UPC = d.UPC;
                            }
                            if (d.UpdatedSalesPrice != null)
                            {
                                price = double.Parse(d.UpdatedSalesPrice);
                            }
                            else if (d.SalesPrice != null)
                            {
                                price = double.Parse(d.SalesPrice);
                            }
                            if (price != 0)
                            {


                                if (price < 200.01)
                                {

                                    double minval = 0.00;

                                    if (price > 0 && price <= 19.99)
                                    {
                                        pricemin = 92.99;
                                    }
                                    else if (price > 19.99 && price <= 29.99)
                                    {
                                        pricemin = 120.99;
                                    }
                                    else if (price > 29.99 && price <= 39.99)
                                    {
                                        pricemin = 142.99;
                                    }
                                    else if (price > 39.99 && price <= 49.99)
                                    {
                                        pricemin = 168.99;
                                    }
                                    else if (price > 49.99 && price <= 59.99)
                                    {
                                        pricemin = 192.99;
                                    }
                                    else if (price > 59.99 && price <= 69.99)
                                    {
                                        pricemin = 222.99;
                                    }
                                    else if (price > 69.99 && price <= 79.99)
                                    {
                                        pricemin = 255.99;
                                    }
                                    else if (price > 79.99 && price <= 89.99)
                                    {
                                        pricemin = 285.99;
                                    }
                                    else if (price > 89.99 && price <= 99.99)
                                    {
                                        pricemin = 313.99;
                                    }
                                    else if (price > 99.99 && price <= 109.99)
                                    {
                                        pricemin = 327.99;
                                    }
                                    else if (price > 109.99 && price <= 119.99)
                                    {
                                        pricemin = 356.99;
                                    }
                                    else if (price > 119.99 && price <= 129.99)
                                    {
                                        pricemin = 385.99;
                                    }
                                    else if (price > 129.99 && price <= 139.99)
                                    {
                                        pricemin = 420.99;
                                    }
                                    else if (price > 139.99 && price <= 149.99)
                                    {
                                        pricemin = 442.99;
                                    }
                                    else if (price > 149.99 && price <= 159.99)
                                    {
                                        pricemin = 463.99;
                                    }
                                    else if (price > 159.99 && price <= 169.99)
                                    {
                                        pricemin = 485.99;
                                    }
                                    else if (price > 169.99 && price <= 179.99)
                                    {
                                        pricemin = 513.99;
                                    }
                                    else if (price > 179.99 && price <= 189.99)
                                    {
                                        pricemin = 542.99;
                                    }
                                    else if (price > 189.99 && price <= 199.99)
                                    {
                                        pricemin = 569.99;
                                    }

                                    pricecal = price * PriceValue;
                                    if (pricemin >= pricecal)
                                    {
                                        finalprice = pricemin;
                                    }
                                    else
                                    {
                                        finalprice = pricecal;
                                    }



                                    string ItemName;
                                    if (d.Title != null)
                                    {
                                        // ItemName = new string(d.Title.Take(490).ToArray());
                                        ItemName = d.Title.Substring(0, d.Title.Length > 499 ? 499 : d.Title.Length);
                                    }
                                    else
                                    {
                                        ItemName = null;
                                    }
                                    string description;

                                    if (d.Description != null)
                                    {
                                        string desc = Regex.Replace(d.Description, "<.*?>", String.Empty);
                                        //description = new string(desc.Take(1990).ToArray());
                                        description = desc.Substring(0, desc.Length > 1990 ? 1990 : desc.Length);
                                    }
                                    else
                                    {
                                        description = null;
                                    }

                                    string manufacturer;
                                    string brand;
                                    if (d.Manufacturer == null && d.Brand == null)
                                    {
                                        manufacturer = "Unknown";
                                        brand = "Unknown";
                                    }
                                    else
                                    {
                                        if (d.Manufacturer == null)
                                        {
                                            manufacturer = d.Brand;
                                        }
                                        else
                                        {
                                            manufacturer = d.Manufacturer;
                                        }

                                        if (d.Brand == null)
                                        {
                                            brand = d.Manufacturer;
                                        }
                                        else
                                        {
                                            brand = d.Brand;
                                        }

                                    }
                                    string feature1, feature2, feature3, feature4;
                                    if (d.Features1 != null)
                                    {
                                        // feature1 = new string(d.Features1.Take(500).ToArray());
                                        feature1 = d.Features1.Substring(0, d.Features1.Length > 500 ? 500 : d.Features1.Length);

                                    }
                                    else
                                        feature1 = d.Features1;
                                    if (d.Features2 != null)
                                    {
                                        // feature2 = new string(d.Features2.Take(500).ToArray());
                                        feature2 = d.Features2.Substring(0, d.Features2.Length > 500 ? 500 : d.Features2.Length);
                                    }
                                    else
                                        feature2 = d.Features2;
                                    if (d.Features3 != null)
                                    {
                                        //feature3 = new string(d.Features3.Take(500).ToArray());
                                        feature3 = d.Features3.Substring(0, d.Features3.Length > 500 ? 500 : d.Features3.Length);
                                    }
                                    else
                                        feature3 = d.Features3;
                                    if (d.Features4 != null)
                                    {
                                        // feature4 = new string(d.Features4.Take(500).ToArray());
                                        feature4 = d.Features4.Substring(0, d.Features4.Length > 500 ? 500 : d.Features4.Length);
                                    }
                                    else
                                        feature4 = d.Features4;

                                    sb.AppendLine(string.Join("\t",
                                                   string.Format(@"""{0}""", d.ASIN.Trim()),
                                                   string.Format(@"""{0}""", UPC),
                                                   string.Format(@"""{0}""", "UPC"),
                                                   string.Format(@"""{0}""", !string.IsNullOrEmpty(ItemName) ? ItemName.Contains('"') ? ItemName.Replace('"', ' ') : ItemName : string.Empty),
                                                   string.Format(@"""{0}""", brand),
                                                   string.Format(@"""{0}""", manufacturer),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", !string.IsNullOrEmpty(description) ? description.Contains('"') ? description.Replace('"', ' ') : description : string.Empty),
                                                   string.Format(@"""{0}""", "BeautyMisc"),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", Quantity),
                                                   string.Format(@"""{0}""", String.Format("{0:0.00}", finalprice)),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", FulfillmentLatency),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", !string.IsNullOrEmpty(feature1) ? feature1.Contains('"') ? feature1.Replace('"', ' ') : feature1 : string.Empty),
                                                   string.Format(@"""{0}""", !string.IsNullOrEmpty(feature2) ? feature2.Contains('"') ? feature2.Replace('"', ' ') : feature2 : string.Empty),
                                                   string.Format(@"""{0}""", !string.IsNullOrEmpty(feature3) ? feature3.Contains('"') ? feature3.Replace('"', ' ') : feature3 : string.Empty),
                                                   string.Format(@"""{0}""", !string.IsNullOrEmpty(feature4) ? feature4.Contains('"') ? feature4.Replace('"', ' ') : feature4 : string.Empty),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", d.LargeImageUrl),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", "")));

                                    d.FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_Prime_Canada_Beauty_" + canadabeauty;

                                    d.ExportDate = DateTime.Now;
                                    d.Instock = 1;

                                    if (shortcode == ConstantData.DC)
                                    {
                                        d.Account3_Status = 1;
                                        d.Account3_ExportDate = DateTime.Now;
                                        Exportpath = d.Account3_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_DC_Prime_Canada_Beauty_" + canadabeauty;
                                    }
                                    d.Status = 1;


                                }
                            }
                        }
                        catch (Exception ex)
                        {


                        }

                    }
                    //ukdb.SaveChanges();
                    ukdb.ObjectContext().SaveChanges(SaveOptions.DetectChangesBeforeSave);

                    canadabeauty++;
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=" + Exportpath + ".txt");
                    //Response.AddHeader("content-disposition", "attachment;filename="+name +"_"+ DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
                    Response.ContentType = "application/text";
                    Response.ContentEncoding = Encoding.GetEncoding(1252);
                    Response.Write(sb.ToString());
                    Response.End();
                }
                transaction.Complete();
                ukdb.ObjectContext().AcceptAllChanges();
            }
        }
        public void ExportBaby(double PriceValue, string shortcode, IEnumerable<tbl_Baby> data)
        {

            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Suppress))
            {


                string Exportpath = "";
                ((IObjectContextAdapter)ukdb).ObjectContext.CommandTimeout = 1800;

                StringWriter st = new StringWriter();
                string header1 = "TemplateType=Baby	Version=2015.1217	The top 3 rows are for Amazon.com use only. Do not modify or delete the top 3 rows.								Offer - Offer Information - These attributes are required to make your item buyable for customers on the site.																		Dimensions - Product Dimensions - These attributes specify the size and weight of a product.														Discovery - Item discovery information - These attributes have an effect on how customers can find your product on the site using browse or search.							Images - Image Information - See Image Instructions tab for details.					Fulfillment - Use these columns to provide fulfilment-related information for orders fulfilled either by Amazon (FBA) or by the Seller.							Variation - Variation information - Populate these attributes if your product is available in different variations (for example colour or wattage).				Ungrouped - These attributes create rich product listings for your buyers.";
                string header2 = "Seller SKU	Item Name (aka Title)	Product ID	Product ID Type	Feed Product Type	Brand Name	Manufacturer	Manufacturer Part Number	Product Description	Update Delete	Quantity	Standard Price	Condition Type	Offer Condition Note	Launch Date	Fulfillment Latency	Release Date	Sale Price	Sale From Date	Sale End Date	Number of Items	Stop Selling Date	Max Aggregate Ship Quantity	Product Tax Code	Can Be Gift Messaged	Is Gift Wrap Available?	Is Discontinued by Manufacturer	Merchant Shipping Group	Shipping Weight	Website Shipping Weight Unit Of Measure	Item Height	Item Length	Item Width	Item Length Unit Of Measure	Item Weight	Unit of measure of item weight	Display Weight	Item Display Weight Unit Of Measure	Display Volume	Item Display Volume Unit Of Measure	Display Length	Item Display Length Unit Of Measure	Key Product Features1	Key Product Features2	Key Product Features3	Key Product Features4	Key Product Features5	Recommended Browse Nodes	Search Terms	Main Image URL	Swatch Image Url	Other Image URL1	Other Image URL2	Other Image URL3	Fulfillment Centre ID	Package Height	Package Width	Package Length	Package Length Unit Of Measure	Package Weight	Package Weight Unit Of Measure	Parentage	Relationship Type	Parent SKU	Variation Theme	Colour	Colour Map	Size	Size Map	Manufacturer Warranty Description	Material Type	Product Care Instructions	Is Assembly Required	Assembly Instructions	Minimum Age Recommendation	Mfg Minimum Unit Of Measure	Maximum Age Recommendation	Mfg Maximum Unit Of Measure	Minimum Weight Recommendation	Minimum Weight Recommendation Unit Of Measure	Maximum Weight Recommendation	Maximum Weight Recommendation Unit Of Measure	Target Gender	Special Features	Material Composition	Language	Batteries are Included	BatteryType	Number of Batteries	Lithium Battery Voltage	Lithium Battery Weight	Lithium Battery Packaging";
                string header3 = "item_sku	item_name	external_product_id	external_product_id_type	feed_product_type	brand_name	manufacturer	part_number	product_description	update_delete	quantity	standard_price	condition_type	condition_note	product_site_launch_date	fulfillment_latency	merchant_release_date	sale_price	sale_from_date	sale_end_date	number_of_items	offering_end_date	max_aggregate_ship_quantity	product_tax_code	offering_can_be_gift_messaged	offering_can_be_giftwrapped	is_discontinued_by_manufacturer	merchant_shipping_group_name	website_shipping_weight	website_shipping_weight_unit_of_measure	item_height	item_length	item_width	item_length_unit_of_measure	item_weight	item_weight_unit_of_measure	item_display_weight	item_display_weight_unit_of_measure	item_display_volume	item_display_volume_unit_of_measure	item_display_length	item_display_length_unit_of_measure	bullet_point1	bullet_point2	bullet_point3	bullet_point4	bullet_point5	recommended_browse_nodes	generic_keywords	main_image_url	swatch_image_url	other_image_url1	other_image_url2	other_image_url3	fulfillment_center_id	package_height	package_width	package_length	package_length_unit_of_measure	package_weight	package_weight_unit_of_measure	parent_child	relationship_type	parent_sku	variation_theme	color_name	color_map	size_name	size_map	warranty_description	material_type	care_instructions	is_assembly_required	assembly_instructions	mfg_minimum	mfg_minimum_unit_of_measure	mfg_maximum	mfg_maximum_unit_of_measure	minimum_weight_recommendation	minimum_weight_recommendation_unit_of_measure	maximum_weight_recommendation	maximum_weight_recommendation_unit_of_measure	target_gender	special_features	material_composition	language_value	are_batteries_included	battery_type	number_of_batteries	lithium_battery_voltage	lithium_battery_weight	lithium_battery_packaging";

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(header1);
                sb.AppendLine(header2);
                sb.AppendLine(header3);
                double addToMinVal = double.Parse(ConfigurationManager.AppSettings["ProdPriceAddToMinValUK"]);// 27-June-2016 making is configurable. It adds vaue accoding to range define in loop.

                //IEnumerable<tbl_Beauty> data = ukdb.tbl_Beauty.Where(x => x.UPC != "null" && x.Status == 0 && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(count.ToString())).ToList();
                if (data != null)
                {
                    foreach (var d in data)
                    {
                        try
                        {


                            double price = 0;
                            double pricecal;
                            double pricemin = 0.00;
                            double finalprice;
                            string UPC;
                            if (d.UPC == null)
                            {
                                upc_codes uc = ukdb.upc_codes.Where(x => x.AssignedTo == null).FirstOrDefault();
                                UPC = uc.Upc;
                                
                                UpdateUPC(uc.Upc, d.ASIN);
                            }
                            else
                            {
                                UPC = d.UPC;
                            }
                            if (d.UpdatedSalesPrice != null)
                            {
                                price = double.Parse(d.UpdatedSalesPrice);
                            }
                            else
                                if (d.SalesPrice != null)
                                {
                                    price = double.Parse(d.SalesPrice);
                                }
                            if (price != 0)
                            {

                                double minval = double.Parse(ConfigurationManager.AppSettings["ProdPriceMinValUK"]);  // 27-June-2016 making it configurable. Minimum Product Price can Change from configuration file. 


                                if (price > 0 && price <= 19.99)
                                {
                                    pricemin = minval;
                                }
                                else
                                {
                                    for (double i = 20; i < 500; i += 10)
                                    {
                                        minval = minval + addToMinVal;
                                        double temp = i + 10;
                                        if (price >= i && price < temp)
                                        {
                                            pricemin = minval;
                                            break;
                                        }
                                    }
                                }
                                pricecal = price * PriceValue;
                                if (pricemin >= pricecal)
                                {
                                    finalprice = pricemin;
                                }
                                else
                                {
                                    finalprice = pricecal;
                                }

                                string ItemName;
                                if (d.Title != null)
                                {
                                    // ItemName = new string(d.Title.Take(490).ToArray());
                                    ItemName = d.Title.Substring(0, d.Title.Length > 499 ? 499 : d.Title.Length);
                                }
                                else
                                {
                                    ItemName = null;
                                }
                                string description;

                                if (d.Description != null)
                                {
                                    string desc = Regex.Replace(d.Description, "<.*?>", String.Empty);
                                    //description = new string(desc.Take(1990).ToArray());
                                    description = desc.Substring(0, desc.Length > 1990 ? 1990 : desc.Length);
                                }
                                else
                                {
                                    description = null;
                                }

                                string manufacturer;
                                string brand;
                                if (d.Manufacturer == null && d.Brand == null)
                                {
                                    manufacturer = "Unknown";
                                    brand = "Unknown";
                                }
                                else
                                {
                                    if (d.Manufacturer == null)
                                    {
                                        manufacturer = d.Brand;
                                    }
                                    else
                                    {
                                        manufacturer = d.Manufacturer;
                                    }

                                    if (d.Brand == null)
                                    {
                                        brand = d.Manufacturer;
                                    }
                                    else
                                    {
                                        brand = d.Brand;
                                    }

                                }
                                string feature1, feature2, feature3, feature4;
                                if (d.Features1 != null)
                                {
                                    // feature1 = new string(d.Features1.Take(500).ToArray());
                                    feature1 = d.Features1.Substring(0, d.Features1.Length > 500 ? 500 : d.Features1.Length);

                                }
                                else
                                    feature1 = d.Features1;
                                if (d.Features2 != null)
                                {
                                    // feature2 = new string(d.Features2.Take(500).ToArray());
                                    feature2 = d.Features2.Substring(0, d.Features2.Length > 500 ? 500 : d.Features2.Length);
                                }
                                else
                                    feature2 = d.Features2;
                                if (d.Features3 != null)
                                {
                                    //feature3 = new string(d.Features3.Take(500).ToArray());
                                    feature3 = d.Features3.Substring(0, d.Features3.Length > 500 ? 500 : d.Features3.Length);
                                }
                                else
                                    feature3 = d.Features3;
                                if (d.Features4 != null)
                                {
                                    // feature4 = new string(d.Features4.Take(500).ToArray());
                                    feature4 = d.Features4.Substring(0, d.Features4.Length > 500 ? 500 : d.Features4.Length);
                                }
                                else
                                    feature4 = d.Features4;

                                sb.AppendLine(string.Join("\t",
                                               string.Format(@"""{0}""", d.ASIN.Trim()),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(ItemName) ? ItemName.Contains('"') ? ItemName.Replace('"', ' ') : ItemName : string.Empty),
                                               string.Format(@"""{0}""", UPC),
                                               string.Format(@"""{0}""", "UPC"),
                                               string.Format(@"""{0}""", "BabyProducts"),
                                               string.Format(@"""{0}""", brand),
                                               string.Format(@"""{0}""", manufacturer),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(description) ? description.Contains('"') ? description.Replace('"', ' ') : description : string.Empty),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", Quantity),
                                               string.Format(@"""{0}""", String.Format("{0:0.00}", finalprice)),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", FulfillmentLatency),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(feature1) ? feature1.Contains('"') ? feature1.Replace('"', ' ') : feature1 : string.Empty),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(feature2) ? feature2.Contains('"') ? feature2.Replace('"', ' ') : feature2 : string.Empty),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(feature3) ? feature3.Contains('"') ? feature3.Replace('"', ' ') : feature3 : string.Empty),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(feature4) ? feature4.Contains('"') ? feature4.Replace('"', ' ') : feature4 : string.Empty),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", d.LargeImageUrl),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", "")));

                                d.FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_Prime_Baby_" + fcountBaby;

                                d.ExportDate = DateTime.Now;
                                d.Instock = 1;
                                if (shortcode == ConstantData.ED)
                                {
                                    d.Account1_Status = 1;
                                    d.Account1_AmazonActive = 1;
                                    d.Account1_ExportDate = DateTime.Now;
                                    Exportpath = d.Account1_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_ED_Prime_Baby_" + fcountBaby;
                                }
                                else if (shortcode == ConstantData.EM)
                                {
                                    d.Account2_Status = 1;
                                    d.Account2_AmazonActive = 1;
                                    d.Account2_ExportDate = DateTime.Now;
                                    Exportpath = d.Account2_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_EM_Prime_Baby_" + fcountBaby;
                                }
                                else if (shortcode == ConstantData.DI)
                                {
                                    d.Account4_Status = 1;
                                    d.Account4_AmazonActive = 1;
                                    d.Account4_ExportDate = DateTime.Now;
                                    Exportpath = d.Account4_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_DI_Prime_Baby_" + fcountBaby;
                                }
                                d.Status = 1;


                            }
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                    ukdb.ObjectContext().SaveChanges(SaveOptions.DetectChangesBeforeSave);

                    // ukdb.SaveChanges();
                    fcountBaby++;
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=" + Exportpath + ".txt");
                    //Response.AddHeader("content-disposition", "attachment;filename="+name +"_"+ DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
                    Response.ContentType = "application/text";
                    Response.ContentEncoding = Encoding.GetEncoding(1252);
                    Response.Write(sb.ToString());
                    Response.End();
                }
                transaction.Complete();
                ukdb.ObjectContext().AcceptAllChanges();
            }
        }
        public void ExportBabyCanada(double PriceValue, string shortcode, IEnumerable<tbl_Baby> data)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Suppress))
            {



                string Exportpath = "";
                ((IObjectContextAdapter)ukdb).ObjectContext.CommandTimeout = 1800;

                StringWriter st = new StringWriter();
                string header1 = "TemplateType=Baby	Version=2015.1204	The top 3 rows are for Amazon.com use only. Do not modify or delete the top 3 rows.								Offer - Offer Information - These attributes are required to make your item buyable for customers on the site.																			Dimensions - Product Dimensions - These attributes specify the size and weight of a product.																		Discovery - Item discovery information - These attributes have an effect on how customers can find your product on the site using browse or search.									Images - Image Information - See Image Instructions tab for details.					Fulfillment - Use these columns to provide fulfillment-related information for either Amazon-fulfilled (FBA) or seller-fulfilled orders.									Variation - Variation information - Populate these attributes if your product is available in different variations (for example colour or wattage).				Compliance - Compliance Information - Attributes used to comply with consumer laws in the country or region where the item is sold.				Ungrouped - These attributes create rich product listings for your buyers.";
                string header2 = "Title	Description	Brand	Manufacturer	Part Number	Seller SKU	Update Delete	Product ID	Product ID Type	Product Type	Fulfillment Latency	Number of Items	Item Condition	Offer Condition Note	Is Gift Wrap Available	Offering Can Be Gift Messaged	Is Discontinued by Manufacturer	Release Date	Launch Date	Item Package Quantity	Sale End Date	Product Tax Code	Sale Price	Standard Price	Launch Date	Restock Date	Sale Start Date	Quantity	Shipping-Template	Item Height Unit Of Measure	Item Height	Item Width	Item Length Unit Of Measure	Item Width Unit Of Measure	Item Length	Item Weight	Item Weight Unit Of Measure	Item Display Height Unit Of Measure	Display Height	Display Width	Item Display Length Unit Of Measure	Item Display Width Unit Of Measure	Display Length	Item Display Weight Unit Of Measure	Display Weight	Shipping Weight	Website Shipping Weight Unit Of Measure	Bullet Point1	Bullet Point2	Bullet Point3	Bullet Point4	Bullet Point5	Recommended Browse Nodes	Intended Use	Target Audience	Subject Matter	Swatch Image URL	Other Image URL1	Other Image URL2	Other Image URL3	Main Image URL	Package Height Unit Of Measure	Package Height	Package Width	Package Length Unit Of Measure	Package Width Unit Of Measure	Package Length	Package Weight	Package Weight Unit Of Measure	Fulfillment Center ID	Variation Theme	Parentage	Parent SKU	Relationship Type	Country Of Origin	Region Of Origin	Legal Disclaimer	Safety Warning	Recommended Uses	Specific Uses For Product	Target Gender	Batteries are Included	Battery Type	Lithium Battery Packaging	Lithium Battery Voltage Unit Of Measure	Lithium Battery Voltage	Lithium Battery Weight Unit Of Measure	Lithium Battery Weight	Size	Size Map	Color Map	Color	Material Type	Maximum Manufacturer Age Recommended	Minimum Manufacturer Age  Recommended	Minimum Weight Recommended	Number Of Pieces	Unit Count Unit Of Measure	Maximum Manufacturer Weight Recommended	weight_recommendation_unit_of_measure";
                string header3 = "item_name	product_description	brand_name	manufacturer	part_number	item_sku	update_delete	external_product_id	external_product_id_type	feed_product_type	fulfillment_latency	number_of_items	condition_type	condition_note	offering_can_be_giftwrapped	offering_can_be_gift_messaged	is_discontinued_by_manufacturer	merchant_release_date	product_site_launch_date	item_package_quantity	sale_end_date	product_tax_code	sale_price	standard_price	offering_start_date	restock_date	sale_from_date	quantity	merchant_shipping_group_name	item_height_unit_of_measure	item_height	item_width	item_length_unit_of_measure	item_width_unit_of_measure	item_length	item_weight	item_weight_unit_of_measure	item_display_height_unit_of_measure	item_display_height	item_display_width	item_display_length_unit_of_measure	item_display_width_unit_of_measure	item_display_length	item_display_weight_unit_of_measure	item_display_weight	website_shipping_weight	website_shipping_weight_unit_of_measure	bullet_point1	bullet_point2	bullet_point3	bullet_point4	bullet_point5	recommended_browse_nodes	specific_uses_keywords	target_audience_keywords	thesaurus_subject_keywords	swatch_image_url	other_image_url1	other_image_url2	other_image_url3	main_image_url	package_height_unit_of_measure	package_height	package_width	package_length_unit_of_measure	package_width_unit_of_measure	package_length	package_weight	package_weight_unit_of_measure	fulfillment_center_id	variation_theme	parent_child	parent_sku	relationship_type	country_of_origin	region_of_origin	legal_disclaimer_description	safety_warning	recommended_uses_for_product	specific_uses_for_product	target_gender	are_batteries_included	battery_cell_composition	lithium_battery_packaging	lithium_battery_voltage_unit_of_measure	lithium_battery_voltage	lithium_battery_weight_unit_of_measure	lithium_battery_weight	size_name	size_map	color_name	color_map	material_type	mfg_maximum	mfg_minimum	minimum_weight_recommendation	number_of_pieces	unit_count_type	maximum_weight_recommendation	weight_recommendation_unit_of_measure";

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(header1);
                sb.AppendLine(header2);
                sb.AppendLine(header3);
                //IEnumerable<tbl_Beauty> data = ukdb.tbl_Beauty.Where(x => x.UPC != "null" && x.Status == 0 && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(count.ToString())).ToList();
                if (data != null)
                {
                    foreach (var d in data)
                    {
                        try
                        {


                            double price = 0;
                            double pricecal;
                            double pricemin = 0.00;
                            double finalprice;
                            string UPC;
                            if (d.UPC == null)
                            {
                                upc_codes uc = ukdb.upc_codes.Where(x => x.AssignedTo == null).FirstOrDefault();
                                UPC = uc.Upc;
                                
                                UpdateUPC(uc.Upc, d.ASIN);
                            }
                            else
                            {
                                UPC = d.UPC;
                            }
                            if (d.UpdatedSalesPrice != null)
                            {
                                price = double.Parse(d.UpdatedSalesPrice);
                            }
                            else if (d.SalesPrice != null)
                            {
                                price = double.Parse(d.SalesPrice);
                            }
                            if (price != 0)
                            {
                                double minval = 49.99;


                                if (price > 0 && price <= 19.99)
                                {
                                    pricemin = 92.99;
                                }
                                else if (price > 19.99 && price <= 29.99)
                                {
                                    pricemin = 120.99;
                                }
                                else if (price > 29.99 && price <= 39.99)
                                {
                                    pricemin = 142.99;
                                }
                                else if (price > 39.99 && price <= 49.99)
                                {
                                    pricemin = 168.99;
                                }
                                else if (price > 49.99 && price <= 59.99)
                                {
                                    pricemin = 192.99;
                                }
                                else if (price > 59.99 && price <= 69.99)
                                {
                                    pricemin = 222.99;
                                }
                                else if (price > 69.99 && price <= 79.99)
                                {
                                    pricemin = 255.99;
                                }
                                else if (price > 79.99 && price <= 89.99)
                                {
                                    pricemin = 285.99;
                                }
                                else if (price > 89.99 && price <= 99.99)
                                {
                                    pricemin = 313.99;
                                }
                                else if (price > 99.99 && price <= 109.99)
                                {
                                    pricemin = 327.99;
                                }
                                else if (price > 109.99 && price <= 119.99)
                                {
                                    pricemin = 356.99;
                                }
                                else if (price > 119.99 && price <= 129.99)
                                {
                                    pricemin = 385.99;
                                }
                                else if (price > 129.99 && price <= 139.99)
                                {
                                    pricemin = 420.99;
                                }
                                else if (price > 139.99 && price <= 149.99)
                                {
                                    pricemin = 442.99;
                                }
                                else if (price > 149.99 && price <= 159.99)
                                {
                                    pricemin = 463.99;
                                }
                                else if (price > 159.99 && price <= 169.99)
                                {
                                    pricemin = 485.99;
                                }
                                else if (price > 169.99 && price <= 179.99)
                                {
                                    pricemin = 513.99;
                                }
                                else if (price > 179.99 && price <= 189.99)
                                {
                                    pricemin = 542.99;
                                }
                                else if (price > 189.99 && price <= 199.99)
                                {
                                    pricemin = 569.99;
                                }
                                pricecal = price * PriceValue;
                                if (pricemin >= pricecal)
                                {
                                    finalprice = pricemin;
                                }
                                else
                                {
                                    finalprice = pricecal;
                                }



                                string ItemName;
                                if (d.Title != null)
                                {
                                    // ItemName = new string(d.Title.Take(490).ToArray());
                                    ItemName = d.Title.Substring(0, d.Title.Length > 499 ? 499 : d.Title.Length);
                                }
                                else
                                {
                                    ItemName = null;
                                }
                                string description;

                                if (d.Description != null)
                                {
                                    string desc = Regex.Replace(d.Description, "<.*?>", String.Empty);
                                    //description = new string(desc.Take(1990).ToArray());
                                    description = desc.Substring(0, desc.Length > 1990 ? 1990 : desc.Length);
                                }
                                else
                                {
                                    description = null;
                                }

                                string manufacturer;
                                string brand;
                                if (d.Manufacturer == null && d.Brand == null)
                                {
                                    manufacturer = "Unknown";
                                    brand = "Unknown";
                                }
                                else
                                {
                                    if (d.Manufacturer == null)
                                    {
                                        manufacturer = d.Brand;
                                    }
                                    else
                                    {
                                        manufacturer = d.Manufacturer;
                                    }

                                    if (d.Brand == null)
                                    {
                                        brand = d.Manufacturer;
                                    }
                                    else
                                    {
                                        brand = d.Brand;
                                    }

                                }
                                string feature1, feature2, feature3, feature4;
                                if (d.Features1 != null)
                                {
                                    // feature1 = new string(d.Features1.Take(500).ToArray());
                                    feature1 = d.Features1.Substring(0, d.Features1.Length > 500 ? 500 : d.Features1.Length);

                                }
                                else
                                    feature1 = d.Features1;
                                if (d.Features2 != null)
                                {
                                    // feature2 = new string(d.Features2.Take(500).ToArray());
                                    feature2 = d.Features2.Substring(0, d.Features2.Length > 500 ? 500 : d.Features2.Length);
                                }
                                else
                                    feature2 = d.Features2;
                                if (d.Features3 != null)
                                {
                                    //feature3 = new string(d.Features3.Take(500).ToArray());
                                    feature3 = d.Features3.Substring(0, d.Features3.Length > 500 ? 500 : d.Features3.Length);
                                }
                                else
                                    feature3 = d.Features3;
                                if (d.Features4 != null)
                                {
                                    // feature4 = new string(d.Features4.Take(500).ToArray());
                                    feature4 = d.Features4.Substring(0, d.Features4.Length > 500 ? 500 : d.Features4.Length);
                                }
                                else
                                    feature4 = d.Features4;
                                sb.AppendLine(string.Join("\t",
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(ItemName) ? ItemName.Contains('"') ? ItemName.Replace('"', ' ') : ItemName : string.Empty),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(description) ? description.Contains('"') ? description.Replace('"', ' ') : description : string.Empty),
                                               string.Format(@"""{0}""", brand),
                                               string.Format(@"""{0}""", manufacturer),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", d.ASIN.Trim()),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", UPC),
                                               string.Format(@"""{0}""", "UPC"),
                                               string.Format(@"""{0}""", "BabyProducts"),
                                               string.Format(@"""{0}""", FulfillmentLatency),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", String.Format("{0:0.00}", finalprice)),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", Quantity),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(feature1) ? feature1.Contains('"') ? feature1.Replace('"', ' ') : feature1 : string.Empty),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(feature2) ? feature2.Contains('"') ? feature2.Replace('"', ' ') : feature2 : string.Empty),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(feature3) ? feature3.Contains('"') ? feature3.Replace('"', ' ') : feature3 : string.Empty),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(feature4) ? feature4.Contains('"') ? feature4.Replace('"', ' ') : feature4 : string.Empty),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", d.LargeImageUrl),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", "")));

                                d.FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_Prime_Baby_Canada_" + fcountBaby;

                                d.ExportDate = DateTime.Now;
                                d.Instock = 1;

                                if (shortcode == ConstantData.DC)
                                {
                                    d.Account3_Status = 1;
                                    d.Account3_ExportDate = DateTime.Now;
                                    Exportpath = d.Account3_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_DC_Prime_Baby_Canada_" + fcountBaby;
                                }
                                d.Status = 1;


                            }
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                    // ukdb.SaveChanges();
                    ukdb.ObjectContext().SaveChanges(SaveOptions.DetectChangesBeforeSave);

                    fcountBaby++;
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=" + Exportpath + ".txt");
                    //Response.AddHeader("content-disposition", "attachment;filename="+name +"_"+ DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
                    Response.ContentType = "application/text";
                    Response.ContentEncoding = Encoding.GetEncoding(1252);
                    Response.Write(sb.ToString());
                    Response.End();
                }
                transaction.Complete();
                ukdb.ObjectContext().AcceptAllChanges();
            }
        }
        public void ExportWatches(double PriceValue, string shortcode, IEnumerable<tbl_Watches> data)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Suppress))
            {



                string Exportpath = "";
                ((IObjectContextAdapter)ukdb).ObjectContext.CommandTimeout = 1800;

                StringWriter st = new StringWriter();
                string header1 = "TemplateType=Watches	Version=2015.1216	The top 3 rows are for Amazon.com use only. Do not modify or delete the top 3 rows.							Offer - Offer Information - These attributes are required to make your item buyable for customers on the site.												Dimensions - Product Dimensions - These attributes specify the size and weight of a product.		Discovery - Item discovery information - These attributes have an effect on how customers can find your product on the site using browse or search.								Images - Image Information - See Image Instructions tab for details.				Fulfillment - Use these columns to provide fulfilment-related information for orders fulfilled either by Amazon (FBA) or by the Seller.							Ungrouped - These attributes create rich product listings for your buyers.";
                string header2 = "Seller SKU	Product ID	Product ID Type	Item Name (aka Title)	Manufacturer	Manufacturer Part Number	Brand Name	Product Description	Update Delete	Standard Price	Offer Condition Note	Launch Date	Sale Price	Sale From Date	Sale End Date	Quantity	Fulfillment Latency	Can Be Gift Messaged	Is Gift Wrap Available?	Is Discontinued by Manufacturer	Merchant Shipping Group	Website Shipping Weight Unit Of Measure	Shipping Weight	Target Audience	Recommended Browse Nodes	Search Terms	Key Product Features1	Key Product Features2	Key Product Features3	Key Product Features4	Key Product Features5	Main Image URL	Other Image URL1	Other Image URL2	Other Image URL3	Fulfillment Centre ID	package-width	Package Weight Unit Of Measure	Package Weight	Package Length	Package Height	Package Dimensions Unit Of Measure	Display Type	Band Material Type	Watch Movement Type	Water Resistance Depth	Water Resistance Depth Unit Of Measure	Lifestyle	Item Shape	Band Width	Case Thickness	Case Diameter	Crystal	Dial Colour	Band Colour	Warranty Type	Bezel Material Type	Clasp Type	Sport Type";
                string header3 = "item_sku	external_product_id	external_product_id_type	item_name	manufacturer	part_number	brand_name	product_description	update_delete	standard_price	condition_note	product_site_launch_date	sale_price	sale_from_date	sale_end_date	quantity	fulfillment_latency	offering_can_be_gift_messaged	offering_can_be_giftwrapped	is_discontinued_by_manufacturer	merchant_shipping_group_name	website_shipping_weight_unit_of_measure	website_shipping_weight	target_audience_keywords	recommended_browse_nodes	generic_keywords	bullet_point1	bullet_point2	bullet_point3	bullet_point4	bullet_point5	main_image_url	other_image_url1	other_image_url2	other_image_url3	fulfillment_center_id	package_width	package_weight_unit_of_measure	package_weight	package_length	package_height	package_dimensions_unit_of_measure	display_type	band_material_type	watch_movement_type	water_resistance_depth	water_resistance_depth_unit_of_measure	lifestyle	item_shape	band_width	case_thickness	case_diameter	dial_window_material_type	dial_color	band_color	warranty_type	bezel_material_type	clasp_type	sport_type";

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(header1);
                sb.AppendLine(header2);
                sb.AppendLine(header3);
                double addToMinVal = double.Parse(ConfigurationManager.AppSettings["ProdPriceAddToMinValUK"]);// 27-June-2016 making is configurable. It adds vaue accoding to range define in loop.

                //IEnumerable<tbl_Beauty> data = ukdb.tbl_Beauty.Where(x => x.UPC != "null" && x.Status == 0 && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(count.ToString())).ToList();
                if (data != null)
                {
                    foreach (var d in data)
                    {
                        try
                        {


                            double price = 0;
                            double pricecal;
                            double pricemin = 0.00;
                            double finalprice;
                            string UPC;
                            if (d.UPC == null)
                            {
                                upc_codes uc = ukdb.upc_codes.Where(x => x.AssignedTo == null).FirstOrDefault();
                                UPC = uc.Upc;
                                
                                UpdateUPC(uc.Upc, d.ASIN);
                            }
                            else
                            {
                                UPC = d.UPC;
                            }
                            if (d.UpdatedSalesPrice != null)
                            {
                                price = double.Parse(d.UpdatedSalesPrice);
                            }
                            else
                                if (d.SalesPrice != null)
                                {
                                    price = double.Parse(d.SalesPrice);
                                }

                            if (price != 0)
                            {

                                double minval = double.Parse(ConfigurationManager.AppSettings["ProdPriceMinValUK"]);  // 27-June-2016 making it configurable. Minimum Product Price can Change from configuration file. 


                                if (price > 0 && price <= 19.99)
                                {
                                    pricemin = minval;
                                }
                                else
                                {
                                    for (double i = 20; i < 500; i += 10)
                                    {
                                        minval = minval + addToMinVal;
                                        double temp = i + 10;
                                        if (price >= i && price < temp)
                                        {
                                            pricemin = minval;
                                            break;
                                        }
                                    }
                                }
                                pricecal = price * PriceValue;
                                if (pricemin >= pricecal)
                                {
                                    finalprice = pricemin;
                                }
                                else
                                {
                                    finalprice = pricecal;
                                }




                                string ItemName;
                                if (d.Title != null)
                                {
                                    // ItemName = new string(d.Title.Take(490).ToArray());
                                    ItemName = d.Title.Substring(0, d.Title.Length > 499 ? 499 : d.Title.Length);
                                }
                                else
                                {
                                    ItemName = null;
                                }
                                string description;

                                if (d.Description != null)
                                {
                                    string desc = Regex.Replace(d.Description, "<.*?>", String.Empty);
                                    //description = new string(desc.Take(1990).ToArray());
                                    description = desc.Substring(0, desc.Length > 1990 ? 1990 : desc.Length);
                                }
                                else
                                {
                                    description = null;
                                }

                                string manufacturer;
                                string brand;
                                if (d.Manufacturer == null && d.Brand == null)
                                {
                                    manufacturer = "Unknown";
                                    brand = "Unknown";
                                }
                                else
                                {
                                    if (d.Manufacturer == null)
                                    {
                                        manufacturer = d.Brand;
                                    }
                                    else
                                    {
                                        manufacturer = d.Manufacturer;
                                    }

                                    if (d.Brand == null)
                                    {
                                        brand = d.Manufacturer;
                                    }
                                    else
                                    {
                                        brand = d.Brand;
                                    }

                                }
                                string feature1, feature2, feature3, feature4;
                                if (d.Features1 != null)
                                {
                                    // feature1 = new string(d.Features1.Take(500).ToArray());
                                    feature1 = d.Features1.Substring(0, d.Features1.Length > 500 ? 500 : d.Features1.Length);

                                }
                                else
                                    feature1 = d.Features1;
                                if (d.Features2 != null)
                                {
                                    // feature2 = new string(d.Features2.Take(500).ToArray());
                                    feature2 = d.Features2.Substring(0, d.Features2.Length > 500 ? 500 : d.Features2.Length);
                                }
                                else
                                    feature2 = d.Features2;
                                if (d.Features3 != null)
                                {
                                    //feature3 = new string(d.Features3.Take(500).ToArray());
                                    feature3 = d.Features3.Substring(0, d.Features3.Length > 500 ? 500 : d.Features3.Length);
                                }
                                else
                                    feature3 = d.Features3;
                                if (d.Features4 != null)
                                {
                                    // feature4 = new string(d.Features4.Take(500).ToArray());
                                    feature4 = d.Features4.Substring(0, d.Features4.Length > 500 ? 500 : d.Features4.Length);
                                }
                                else
                                    feature4 = d.Features4;

                                sb.AppendLine(string.Join("\t",
                                               string.Format(@"""{0}""", d.ASIN.Trim()),
                                               string.Format(@"""{0}""", UPC),
                                               string.Format(@"""{0}""", "UPC"),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(ItemName) ? ItemName.Contains('"') ? ItemName.Replace('"', ' ') : ItemName : string.Empty),
                                               string.Format(@"""{0}""", manufacturer),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", brand),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(description) ? description.Contains('"') ? description.Replace('"', ' ') : description : string.Empty),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", String.Format("{0:0.00}", finalprice)),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", Quantity),
                                               string.Format(@"""{0}""", FulfillmentLatency),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(feature1) ? feature1.Contains('"') ? feature1.Replace('"', ' ') : feature1 : string.Empty),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(feature2) ? feature2.Contains('"') ? feature2.Replace('"', ' ') : feature2 : string.Empty),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(feature3) ? feature3.Contains('"') ? feature3.Replace('"', ' ') : feature3 : string.Empty),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(feature4) ? feature4.Contains('"') ? feature4.Replace('"', ' ') : feature4 : string.Empty),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", d.LargeImageUrl),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", "")));

                                d.FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_Prime_Watches_" + fcountWatches;

                                d.ExportDate = DateTime.Now;
                                d.Instock = 1;
                                if (shortcode == ConstantData.ED)
                                {
                                    d.Account1_Status = 1;
                                    d.Account1_AmazonActive = 1;
                                    d.Account1_ExportDate = DateTime.Now;
                                    Exportpath = d.Account1_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_ED_Prime_Watches_" + fcountWatches;
                                }
                                else if (shortcode == ConstantData.EM)
                                {
                                    d.Account2_Status = 1;
                                    d.Account2_AmazonActive = 1;
                                    d.Account2_ExportDate = DateTime.Now;
                                    Exportpath = d.Account2_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_EM_Prime_Watches_" + fcountWatches;
                                }
                                else if (shortcode == ConstantData.DI)
                                {
                                    d.Account4_Status = 1;
                                    d.Account4_AmazonActive = 1;
                                    d.Account4_ExportDate = DateTime.Now;
                                    Exportpath = d.Account4_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_DI_Prime_Watches_" + fcountWatches;
                                }
                                d.Status = 1;


                            }
                        }
                        catch (Exception ex)
                        {


                        }

                    }
                    //ukdb.SaveChanges();
                    ukdb.ObjectContext().SaveChanges(SaveOptions.DetectChangesBeforeSave);

                    fcountWatches++;
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=" + Exportpath + ".txt");
                    //Response.AddHeader("content-disposition", "attachment;filename="+name +"_"+ DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
                    Response.ContentType = "application/text";
                    Response.ContentEncoding = Encoding.GetEncoding(1252);
                    Response.Write(sb.ToString());
                    Response.End();
                }
                transaction.Complete();
                ukdb.ObjectContext().AcceptAllChanges();
            }
        }
        public void ExportWatchesCanada(double PriceValue, string shortcode, IEnumerable<tbl_Watches> data)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Suppress))
            {



                string Exportpath = "";
                ((IObjectContextAdapter)ukdb).ObjectContext.CommandTimeout = 1800;

                StringWriter st = new StringWriter();
                string header1 = "TemplateType=Watches	Version=2015.1204	The top 3 rows are for Amazon.com use only. Do not modify or delete the top 3 rows.							Offer - Offer Information - These attributes are required to make your item buyable for customers on the site.																			Dimensions - Product Dimensions - These attributes specify the size and weight of a product.				Discovery - Item discovery information - These attributes have an effect on how customers can find your product on the site using browse or search.								Images - Image Information - See Image Instructions tab for details.				Fulfillment - Use these columns to provide fulfillment-related information for either Amazon-fulfilled (FBA) or seller-fulfilled orders.							Variation - Variation information - Populate these attributes if your product is available in different variations (for example colour or wattage).				Compliance - Compliance Information - Attributes used to comply with consumer laws in the country or region where the item is sold.	Ungrouped - These attributes create rich product listings for your buyers.";
                string header2 = "Seller SKU	Product Name	Product ID	Product ID Type	Manufacturer	Manufacturer Part Number	Brand	Description	Update Delete	Standard Price	Item Condition	Condition Note	Quantity	Manufacturer's Suggested Retail Price	Launch Date	Release Date	Restock Date	Fulfillment Latency	Max Aggregate Ship Quantity	Sale Price	Sale Start Date	Sale End Date	Product Tax Code	Item Package Quantity	Offering Can Be Gift Messaged	Is Gift Wrap Available	Is Discontinued By Manufacturer	Shipping-Template	Item Weight	Item Weight Unit Of Measure	Website Shipping Weight Unit Of Measure	Shipping Weight	Recommended Browse Node	Search Terms	Target Audience	Key Product Features1	Key Product Features2	Key Product Features3	Key Product Features4	Key Product Features5	Main Image URL	Other Image URL1	Other Image URL2	Other Image URL3	Fulfillment Center ID	Package Height	Package Width	Package Length	Package Weight	Package Weight Unit Of Measure	Package Dimensions Unit Of Measure	Parentage	Parent SKU	Relationship Type	Variation Theme	Country Of Origin	Gender	Warranty Type	Band Material	Bezel Material	Band Width Unit Of Measure	Calendar Type	Case Material	Clasp Type	Dial Color	Dial Color Map	Case Diameter Unit Of Measure	Display Type	Style Name	Model Year	Movement Type	Special Features	Water Resistant Depth	Water Resistance Depth Unit Of Measure	Band Color	Band Length	Band Width	Case Size Diameter	Are Batteries Included	Battery Type	Number of Batteries Required";
                string header3 = "item_sku	item_name	external_product_id	external_product_id_type	manufacturer	part_number	brand_name	product_description	update_delete	standard_price	condition_type	condition_note	quantity	list_price	product_site_launch_date	merchant_release_date	restock_date	fulfillment_latency	max_aggregate_ship_quantity	sale_price	sale_from_date	sale_end_date	product_tax_code	item_package_quantity	offering_can_be_gift_messaged	offering_can_be_giftwrapped	is_discontinued_by_manufacturer	merchant_shipping_group_name	item_weight	item_weight_unit_of_measure	website_shipping_weight_unit_of_measure	website_shipping_weight	recommended_browse_nodes	generic_keywords	target_audience_keywords	bullet_point1	bullet_point2	bullet_point3	bullet_point4	bullet_point5	main_image_url	other_image_url1	other_image_url2	other_image_url3	fulfillment_center_id	package_height	package_width	package_length	package_weight	package_weight_unit_of_measure	package_dimensions_unit_of_measure	parent_child	parent_sku	relationship_type	variation_theme	country_of_origin	department_name	warranty_type	band_material_type	bezel_material_type	band_width_unit_of_measure	calendar_type	case_material_type	clasp_type	dial_color	color_name	case_diameter_unit_of_measure	display_type	style_name	model_year	watch_movement_type	special_features	water_resistance_depth	water_resistance_depth_unit_of_measure	band_color	band_size	band_width	case_diameter	are_batteries_included	battery_type	number_of_batteries";

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(header1);
                sb.AppendLine(header2);
                sb.AppendLine(header3);
                //IEnumerable<tbl_Beauty> data = ukdb.tbl_Beauty.Where(x => x.UPC != "null" && x.Status == 0 && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(count.ToString())).ToList();
                if (data != null)
                {
                    foreach (var d in data)
                    {
                        try
                        {


                            double price = 0;
                            double pricecal;
                            double pricemin = 0.00;
                            double finalprice;
                            string UPC;
                            if (d.UPC == null)
                            {
                                upc_codes uc = ukdb.upc_codes.Where(x => x.AssignedTo == null).FirstOrDefault();
                                UPC = uc.Upc;
                                
                                UpdateUPC(uc.Upc, d.ASIN);
                            }
                            else
                            {
                                UPC = d.UPC;
                            }
                            if (d.UpdatedSalesPrice != null)
                            {
                                price = double.Parse(d.UpdatedSalesPrice);
                            }
                            else if (d.SalesPrice != null)
                            {
                                price = double.Parse(d.SalesPrice);
                            }
                            if (price != 0)
                            {


                                if (price < 200.01)
                                {
                                    double minval = 49.99;


                                    if (price > 0 && price <= 19.99)
                                    {
                                        pricemin = 92.99;
                                    }
                                    else if (price > 19.99 && price <= 29.99)
                                    {
                                        pricemin = 120.99;
                                    }
                                    else if (price > 29.99 && price <= 39.99)
                                    {
                                        pricemin = 142.99;
                                    }
                                    else if (price > 39.99 && price <= 49.99)
                                    {
                                        pricemin = 168.99;
                                    }
                                    else if (price > 49.99 && price <= 59.99)
                                    {
                                        pricemin = 192.99;
                                    }
                                    else if (price > 59.99 && price <= 69.99)
                                    {
                                        pricemin = 222.99;
                                    }
                                    else if (price > 69.99 && price <= 79.99)
                                    {
                                        pricemin = 255.99;
                                    }
                                    else if (price > 79.99 && price <= 89.99)
                                    {
                                        pricemin = 285.99;
                                    }
                                    else if (price > 89.99 && price <= 99.99)
                                    {
                                        pricemin = 313.99;
                                    }
                                    else if (price > 99.99 && price <= 109.99)
                                    {
                                        pricemin = 327.99;
                                    }
                                    else if (price > 109.99 && price <= 119.99)
                                    {
                                        pricemin = 356.99;
                                    }
                                    else if (price > 119.99 && price <= 129.99)
                                    {
                                        pricemin = 385.99;
                                    }
                                    else if (price > 129.99 && price <= 139.99)
                                    {
                                        pricemin = 420.99;
                                    }
                                    else if (price > 139.99 && price <= 149.99)
                                    {
                                        pricemin = 442.99;
                                    }
                                    else if (price > 149.99 && price <= 159.99)
                                    {
                                        pricemin = 463.99;
                                    }
                                    else if (price > 159.99 && price <= 169.99)
                                    {
                                        pricemin = 485.99;
                                    }
                                    else if (price > 169.99 && price <= 179.99)
                                    {
                                        pricemin = 513.99;
                                    }
                                    else if (price > 179.99 && price <= 189.99)
                                    {
                                        pricemin = 542.99;
                                    }
                                    else if (price > 189.99 && price <= 199.99)
                                    {
                                        pricemin = 569.99;
                                    }
                                    pricecal = price * PriceValue;
                                    if (pricemin >= pricecal)
                                    {
                                        finalprice = pricemin;
                                    }
                                    else
                                    {
                                        finalprice = pricecal;
                                    }


                                    string ItemName;
                                    if (d.Title != null)
                                    {
                                        // ItemName = new string(d.Title.Take(490).ToArray());
                                        ItemName = d.Title.Substring(0, d.Title.Length > 499 ? 499 : d.Title.Length);
                                    }
                                    else
                                    {
                                        ItemName = null;
                                    }
                                    string description;

                                    if (d.Description != null)
                                    {
                                        string desc = Regex.Replace(d.Description, "<.*?>", String.Empty);
                                        //description = new string(desc.Take(1990).ToArray());
                                        description = desc.Substring(0, desc.Length > 1990 ? 1990 : desc.Length);
                                    }
                                    else
                                    {
                                        description = null;
                                    }

                                    string manufacturer;
                                    string brand;
                                    if (d.Manufacturer == null && d.Brand == null)
                                    {
                                        manufacturer = "Unknown";
                                        brand = "Unknown";
                                    }
                                    else
                                    {
                                        if (d.Manufacturer == null)
                                        {
                                            manufacturer = d.Brand;
                                        }
                                        else
                                        {
                                            manufacturer = d.Manufacturer;
                                        }

                                        if (d.Brand == null)
                                        {
                                            brand = d.Manufacturer;
                                        }
                                        else
                                        {
                                            brand = d.Brand;
                                        }

                                    }
                                    string feature1, feature2, feature3, feature4;
                                    if (d.Features1 != null)
                                    {
                                        // feature1 = new string(d.Features1.Take(500).ToArray());
                                        feature1 = d.Features1.Substring(0, d.Features1.Length > 500 ? 500 : d.Features1.Length);

                                    }
                                    else
                                        feature1 = d.Features1;
                                    if (d.Features2 != null)
                                    {
                                        // feature2 = new string(d.Features2.Take(500).ToArray());
                                        feature2 = d.Features2.Substring(0, d.Features2.Length > 500 ? 500 : d.Features2.Length);
                                    }
                                    else
                                        feature2 = d.Features2;
                                    if (d.Features3 != null)
                                    {
                                        //feature3 = new string(d.Features3.Take(500).ToArray());
                                        feature3 = d.Features3.Substring(0, d.Features3.Length > 500 ? 500 : d.Features3.Length);
                                    }
                                    else
                                        feature3 = d.Features3;
                                    if (d.Features4 != null)
                                    {
                                        // feature4 = new string(d.Features4.Take(500).ToArray());
                                        feature4 = d.Features4.Substring(0, d.Features4.Length > 500 ? 500 : d.Features4.Length);
                                    }
                                    else
                                        feature4 = d.Features4;

                                    sb.AppendLine(string.Join("\t",
                                                   string.Format(@"""{0}""", d.ASIN.Trim()),
                                                   string.Format(@"""{0}""", !string.IsNullOrEmpty(ItemName) ? ItemName.Contains('"') ? ItemName.Replace('"', ' ') : ItemName : string.Empty),
                                                   string.Format(@"""{0}""", UPC),
                                                   string.Format(@"""{0}""", "UPC"),
                                                   string.Format(@"""{0}""", manufacturer),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", brand),
                                                   string.Format(@"""{0}""", !string.IsNullOrEmpty(description) ? description.Contains('"') ? description.Replace('"', ' ') : description : string.Empty),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", String.Format("{0:0.00}", finalprice)),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", Quantity),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", FulfillmentLatency),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", !string.IsNullOrEmpty(feature1) ? feature1.Contains('"') ? feature1.Replace('"', ' ') : feature1 : string.Empty),
                                                   string.Format(@"""{0}""", !string.IsNullOrEmpty(feature2) ? feature2.Contains('"') ? feature2.Replace('"', ' ') : feature2 : string.Empty),
                                                   string.Format(@"""{0}""", !string.IsNullOrEmpty(feature3) ? feature3.Contains('"') ? feature3.Replace('"', ' ') : feature3 : string.Empty),
                                                   string.Format(@"""{0}""", !string.IsNullOrEmpty(feature4) ? feature4.Contains('"') ? feature4.Replace('"', ' ') : feature4 : string.Empty),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", d.LargeImageUrl),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", "")));

                                    d.FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_Prime_Watches_Canada_" + fcountWatches;

                                    d.ExportDate = DateTime.Now;
                                    d.Instock = 1;

                                    if (shortcode == ConstantData.DC)
                                    {
                                        d.Account3_Status = 1;
                                        d.Account3_ExportDate = DateTime.Now;
                                        Exportpath = d.Account3_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_DC_Prime_Watches_Canada_" + fcountWatches;
                                    }
                                    d.Status = 1;


                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    ukdb.ObjectContext().SaveChanges(SaveOptions.DetectChangesBeforeSave);//  ukdb.SaveChanges();

                    fcountWatches++;
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=" + Exportpath + ".txt");
                    //Response.AddHeader("content-disposition", "attachment;filename="+name +"_"+ DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
                    Response.ContentType = "application/text";
                    Response.ContentEncoding = Encoding.GetEncoding(1252);
                    Response.Write(sb.ToString());
                    Response.End();
                }
                transaction.Complete();
                ukdb.ObjectContext().AcceptAllChanges();
            }
        }
        public void ExportJewelry(double PriceValue, string shortcode, IEnumerable<tbl_Jewellery> data)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Suppress))
            {


                string Exportpath = "";
                ((IObjectContextAdapter)ukdb).ObjectContext.CommandTimeout = 1800;

                StringWriter st = new StringWriter();
                string header1 = "TemplateType=jewelry	Version=2015.1216	The top 3 rows are for Amazon.com use only. Do not modify or delete the top 3 rows.									Offer-Offer Information - These attributes are required to make your item buyable for customers on the site.																Dimensions-Product Dimensions - These attributes specify the size and weight of a product.					Discovery-Item discovery information - These attributes have an effect on how customers can find your product on the site using browse or search.					Images-Image Information - See Image Instructions tab for details.					Fulfillment-Use these columns to provide fulfilment-related information for orders fulfilled either by Amazon (FBA) or by the Seller.									Variation-Variation information - Populate these attributes if your product is available in different variations (for example colour or wattage).				Ungrouped - These attributes create rich product listings for your buyers.";
                string header2 = "Seller SKU	Item Name	Manufacturer	Model	Feed Product Type	Brand Name	Product ID	Product ID Type	Product Description	Update Delete	Manufacturer Part Number	Standard Price	Quantity	Launch Date	Release Date	Condition Type	Condition Note	Product Tax Code	Sale Price	Sale From Date	Sale End Date	Fulfillment Latency	Max Aggregate Ship Quantity	Can Be Gift Messaged	Is Gift Wrap Available?	Is Discontinued By Manufacturer	Merchant Shipping Group	Website Shipping Weight	Website Shipping Weight Unit Of Measure	Display Dimensions Unit Of Measure	Item Display Width	Display Length	Recommended Browse Nodes	Bullet Point	Bullet Point	Bullet Point	Search Terms	Main Image URL	Swatch Image Url	Other Image Url	Other Image Url	Other Image Url	Fulfillment Centre ID	Package Width Unit Of Measure	Package Width	Package Weight Unit Of Measure	Package weight	Package Length Unit Of Measure	Package Length	Package Height Unit of Measure	Package Height	Parentage	Parent SKU	Relationship Type	Variation Theme	Total Diamond Weight	Metal Type	Metal Stamp	Ring Size	Ring Sizing Lower Range	Ring Sizing Upper Range	Gem Type	Gem Type	Stone Colour	Stone Colour	Stone Shape	Stone Shape	Size Per Pearl	Material	Theme	Occasion Type	Earring Type	Colour Map	Clasp Type	Chain Type	Color	Back Finding";
                string header3 = "item_sku	item_name	manufacturer	model	feed_product_type	brand_name	external_product_id	external_product_id_type	product_description	update_delete	part_number	standard_price	quantity	product_site_launch_date	merchant_release_date	condition_type	condition_note	product_tax_code	sale_price	sale_from_date	sale_end_date	fulfillment_latency	max_aggregate_ship_quantity	offering_can_be_gift_messaged	offering_can_be_giftwrapped	is_discontinued_by_manufacturer	merchant_shipping_group_name	website_shipping_weight	website_shipping_weight_unit_of_measure	display_dimensions_unit_of_measure	item_display_width	item_display_length	recommended_browse_nodes	bullet_point1	bullet_point2	bullet_point3	generic_keywords	main_image_url	swatch_image_url	other_image_url1	other_image_url2	other_image_url3	fulfillment_center_id	package_width_unit_of_measure	package_width	package_weight_unit_of_measure	package_weight	package_length_unit_of_measure	package_length	package_height_unit_of_measure	package_height	parent_child	parent_sku	relationship_type	variation_theme	total_diamond_weight	metal_type	metal_stamp	ring_size	ring_sizing_lower_range	ring_sizing_upper_range	gem_type1	gem_type2	stone_color1	stone_color2	stone_shape1	stone_shape2	size_per_pearl	material_type	theme	occasion_type	item_shape	color_map	clasp_type	chain_type	color_name	back_finding";

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(header1);
                sb.AppendLine(header2);
                sb.AppendLine(header3);
                double addToMinVal = double.Parse(ConfigurationManager.AppSettings["ProdPriceAddToMinValUK"]);// 27-June-2016 making is configurable. It adds vaue accoding to range define in loop.

                //IEnumerable<tbl_Beauty> data = ukdb.tbl_Beauty.Where(x => x.UPC != "null" && x.Status == 0 && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(count.ToString())).ToList();
                if (data != null)
                {
                    foreach (var d in data)
                    {
                        try
                        {


                            double price = 0;
                            double pricecal;
                            double pricemin = 0.00;
                            double finalprice;
                            string UPC;
                            if (d.UPC == null)
                            {
                                upc_codes uc = ukdb.upc_codes.Where(x => x.AssignedTo == null).FirstOrDefault();
                                UPC = uc.Upc;
                                
                                UpdateUPC(uc.Upc, d.ASIN);
                            }
                            else
                            {
                                UPC = d.UPC;
                            }
                            if (d.UpdatedSalesPrice != null)
                            {
                                price = double.Parse(d.UpdatedSalesPrice);
                            }
                            else
                                if (d.SalesPrice != null)
                                {
                                    price = double.Parse(d.SalesPrice);
                                }

                            if (price != 0)
                            {

                                double minval = double.Parse(ConfigurationManager.AppSettings["ProdPriceMinValUK"]);  // 27-June-2016 making it configurable. Minimum Product Price can Change from configuration file. 


                                if (price > 0 && price <= 19.99)
                                {
                                    pricemin = minval;
                                }
                                else
                                {
                                    for (double i = 20; i < 500; i += 10)
                                    {
                                        minval = minval + addToMinVal;
                                        double temp = i + 10;
                                        if (price >= i && price < temp)
                                        {
                                            pricemin = minval;
                                            break;
                                        }
                                    }
                                }
                                pricecal = price * PriceValue;
                                if (pricemin >= pricecal)
                                {
                                    finalprice = pricemin;
                                }
                                else
                                {
                                    finalprice = pricecal;
                                }





                                string ItemName;
                                if (d.Title != null)
                                {
                                    // ItemName = new string(d.Title.Take(490).ToArray());
                                    ItemName = d.Title.Substring(0, d.Title.Length > 499 ? 499 : d.Title.Length);
                                }
                                else
                                {
                                    ItemName = null;
                                }
                                string description;

                                if (d.Description != null)
                                {
                                    string desc = Regex.Replace(d.Description, "<.*?>", String.Empty);
                                    //description = new string(desc.Take(1990).ToArray());
                                    description = desc.Substring(0, desc.Length > 1990 ? 1990 : desc.Length);
                                }
                                else
                                {
                                    description = null;
                                }

                                string manufacturer;
                                string brand;
                                if (d.Manufacturer == null && d.Brand == null)
                                {
                                    manufacturer = "Unknown";
                                    brand = "Unknown";
                                }
                                else
                                {
                                    if (d.Manufacturer == null)
                                    {
                                        manufacturer = d.Brand;
                                    }
                                    else
                                    {
                                        manufacturer = d.Manufacturer;
                                    }

                                    if (d.Brand == null)
                                    {
                                        brand = d.Manufacturer;
                                    }
                                    else
                                    {
                                        brand = d.Brand;
                                    }

                                }
                                string feature1, feature2, feature3, feature4;
                                if (d.Features1 != null)
                                {
                                    // feature1 = new string(d.Features1.Take(500).ToArray());
                                    feature1 = d.Features1.Substring(0, d.Features1.Length > 500 ? 500 : d.Features1.Length);

                                }
                                else
                                    feature1 = d.Features1;
                                if (d.Features2 != null)
                                {
                                    // feature2 = new string(d.Features2.Take(500).ToArray());
                                    feature2 = d.Features2.Substring(0, d.Features2.Length > 500 ? 500 : d.Features2.Length);
                                }
                                else
                                    feature2 = d.Features2;
                                if (d.Features3 != null)
                                {
                                    //feature3 = new string(d.Features3.Take(500).ToArray());
                                    feature3 = d.Features3.Substring(0, d.Features3.Length > 500 ? 500 : d.Features3.Length);
                                }
                                else
                                    feature3 = d.Features3;
                                if (d.Features4 != null)
                                {
                                    // feature4 = new string(d.Features4.Take(500).ToArray());
                                    feature4 = d.Features4.Substring(0, d.Features4.Length > 500 ? 500 : d.Features4.Length);
                                }
                                else
                                    feature4 = d.Features4;

                                sb.AppendLine(string.Join("\t",
                                               string.Format(@"""{0}""", d.ASIN.Trim()),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(ItemName) ? ItemName.Contains('"') ? ItemName.Replace('"', ' ') : ItemName : string.Empty),
                                               string.Format(@"""{0}""", manufacturer),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", "FashionOther"),
                                               string.Format(@"""{0}""", brand),
                                               string.Format(@"""{0}""", UPC),
                                               string.Format(@"""{0}""", "UPC"),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(description) ? description.Contains('"') ? description.Replace('"', ' ') : description : string.Empty),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", d.UPC),
                                               string.Format(@"""{0}""", String.Format("{0:0.00}", finalprice)),
                                               string.Format(@"""{0}""", Quantity),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", FulfillmentLatency),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", "193717031"),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(feature1) ? feature1.Contains('"') ? feature1.Replace('"', ' ') : feature1 : string.Empty),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(feature2) ? feature2.Contains('"') ? feature2.Replace('"', ' ') : feature2 : string.Empty),
                                               string.Format(@"""{0}""", !string.IsNullOrEmpty(feature3) ? feature3.Contains('"') ? feature3.Replace('"', ' ') : feature3 : string.Empty),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", d.LargeImageUrl),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", ""),
                                               string.Format(@"""{0}""", "")));

                                d.FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_Prime_Jewelry_" + fcountWatches;

                                d.ExportDate = DateTime.Now;
                                d.Instock = 1;
                                if (shortcode == ConstantData.ED)
                                {
                                    d.Account1_Status = 1;
                                    d.Account1_AmazonActive = 1;
                                    d.Account1_ExportDate = DateTime.Now;
                                    Exportpath = d.Account1_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_ED_Prime_Jewelry_" + fcountJewelry;
                                }
                                else if (shortcode == ConstantData.EM)
                                {
                                    d.Account2_Status = 1;
                                    d.Account2_AmazonActive = 1;
                                    d.Account2_ExportDate = DateTime.Now;
                                    Exportpath = d.Account2_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_EM_Prime_Jewelry_" + fcountJewelry;
                                }
                                else if (shortcode == ConstantData.DI)
                                {
                                    d.Account4_Status = 1;
                                    d.Account4_AmazonActive = 1;
                                    d.Account4_ExportDate = DateTime.Now;
                                    Exportpath = d.Account4_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_DI_Prime_Jewelry_" + fcountJewelry;
                                }
                                d.Status = 1;



                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    ukdb.ObjectContext().SaveChanges(SaveOptions.DetectChangesBeforeSave);

                    //ukdb.SaveChanges();
                    fcountJewelry++;
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=" + Exportpath + ".txt");
                    //Response.AddHeader("content-disposition", "attachment;filename="+name +"_"+ DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
                    Response.ContentType = "application/text";
                    Response.ContentEncoding = Encoding.GetEncoding(1252);
                    Response.Write(sb.ToString());
                    Response.End();
                }
                transaction.Complete();
                ukdb.ObjectContext().AcceptAllChanges();
            }
        }
        public void ExportJewelryCanada(double PriceValue, string shortcode, IEnumerable<tbl_Jewellery> data)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Suppress))
            {


                string Exportpath = "";
                ((IObjectContextAdapter)ukdb).ObjectContext.CommandTimeout = 1800;

                StringWriter st = new StringWriter();
                string header1 = "TemplateType=Jewelry	Version=2015.1204	The top 3 rows are for Amazon.com use only. Do not modify or delete the top 3 rows.								Offer - Offer Information - These attributes are required to make your item buyable for customers on the site.																	Dimensions - Product Dimensions - These attributes specify the size and weight of a product.							Discovery - Item discovery information - These attributes have an effect on how customers can find your product on the site using browse or search.																				Images - Image Information - See Image Instructions tab for details.					Fulfillment - Use these columns to provide fulfillment-related information for either Amazon-fulfilled (FBA) or seller-fulfilled orders.	Variation - Variation information - Populate these attributes if your product is available in different variations (for example colour or wattage).				Compliance - Compliance Information - Attributes used to comply with consumer laws in the country or region where the item is sold.						Ungrouped - These attributes create rich product listings for your buyers.";
                string header2 = "Seller SKU	Title	Manufacturer	Model Number	Product Type	Brand Name	Product ID	Product ID Type	Product Description	Update Delete	Standard Price	Quantity	Launch Date	Product Tax Code	Manufacturer's Suggested Retail Price	Sale Price	Sale Start Date	Sale End Date	Release Date	Package Quantity	Fulfillment Latency	Restock Date	Max Aggregate Ship Quantity	Offering Can Be Gift Messaged	Is Gift Wrap Available	Is Discontinued by Manufacturer	Shipping-Template	Shipping Weight	Website Shipping Weight Unit Of Measure	Display Dimensions Unit Of Measure	Diameter	Display Height	Width	Item Display Length	recommended-browse-nodes1-2	Key Product Features1	Key Product Features2	Key Product Features3	Key Product Features4	Key Product Features5	Target Audience1	Target Audience2	Target Audience3	Intended Use1	Intended Use2	Intended Use3	Intended Use4	Intended Use5	Subject Matter1	Subject Matter2	Subject Matter3	Subject Matter4	Subject Matter5	Search Terms	Main Image URL	Swatch Image URL	Other Image Url1	Other Image Url2	Other Image Url3	Fulfillment Center ID	Parentage	Parent SKU	Relationship Type	Variation Theme	Country of Publication	Cpsia Warning1	Cpsia Warning2	Cpsia Warning3	Cpsia Warning4	CPSIA Warning Description	Other Attributes1	Other Attributes2	Other Attributes3	Other Attributes4	Other Attributes5	Total Metal Weight	Total Metal Weight Unit Of Measure	Total Diamond Weight	Total Diamond Weight Unit Of Measure	Total Gem Weight	Total Gem Weight Unit Of Measure	Material Type	Metal Type	Metal Stamp	Setting Type	Number Of Stones	Clasp Type	Chain Type	Gem Type1	Gem Type2	Gem Type3	Stone Color	Stone Clarity	Stone Shape	Stone Treatment Method	Stone Weight	Pearl Type	Color	Style	Pearl Minimum Color	Pearl Shape	Pearl Uniformity	Pearl Surface Blemishes	Pearl Stringing Method	Size Per Pearl	Gender	Ring Size	Ring Sizing Upper Range	Back Finding";
                string header3 = "item_sku	item_name	manufacturer	model	feed_product_type	brand_name	external_product_id	external_product_id_type	product_description	update_delete	standard_price	quantity	product_site_launch_date	product_tax_code	list_price	sale_price	sale_from_date	sale_end_date	merchant_release_date	item_package_quantity	fulfillment_latency	restock_date	max_aggregate_ship_quantity	offering_can_be_gift_messaged	offering_can_be_giftwrapped	is_discontinued_by_manufacturer	merchant_shipping_group_name	website_shipping_weight	website_shipping_weight_unit_of_measure	display_dimensions_unit_of_measure	item_display_diameter	item_display_height	item_display_width	item_display_length	recommended_browse_nodes	bullet_point1	bullet_point2	bullet_point3	bullet_point4	bullet_point5	target_audience_keywords1	target_audience_keywords2	target_audience_keywords3	specific_uses_keywords1	specific_uses_keywords2	specific_uses_keywords3	specific_uses_keywords4	specific_uses_keywords5	thesaurus_subject_keywords1	thesaurus_subject_keywords2	thesaurus_subject_keywords3	thesaurus_subject_keywords4	thesaurus_subject_keywords5	generic_keywords	main_image_url	swatch_image_url	other_image_url1	other_image_url2	other_image_url3	fulfillment_center_id	parent_child	parent_sku	relationship_type	variation_theme	country_of_origin	cpsia_cautionary_statement1	cpsia_cautionary_statement2	cpsia_cautionary_statement3	cpsia_cautionary_statement4	cpsia_cautionary_description	thesaurus_attribute_keywords1	thesaurus_attribute_keywords2	thesaurus_attribute_keywords3	thesaurus_attribute_keywords4	thesaurus_attribute_keywords5	total_metal_weight	total_metal_weight_unit_of_measure	total_diamond_weight	total_diamond_weight_unit_of_measure	total_gem_weight	total_gem_weight_unit_of_measure	material_type	metal_type	metal_stamp	setting_type	number_of_stones	clasp_type	chain_type	gem_type1	gem_type2	gem_type3	stone_color	stone_clarity	stone_shape	stone_treatment_method	stone_weight	pearl_type	color_name	style_name	pearl_minimum_color	pearl_shape	pearl_uniformity	pearl_surface_blemishes	pearl_stringing_method	size_per_pearl	department_name	ring_size	ring_sizing_upper_range	back_finding";

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(header1);
                sb.AppendLine(header2);
                sb.AppendLine(header3);

                if (data != null)
                {
                    foreach (var d in data)
                    {
                        try
                        {


                            double price = 0;
                            double pricecal;
                            double pricemin = 0.00;
                            double finalprice;
                            string UPC;
                            if (d.UPC == null)
                            {
                                upc_codes uc = ukdb.upc_codes.Where(x => x.AssignedTo == null).FirstOrDefault();
                                UPC = uc.Upc;
                                
                                UpdateUPC(uc.Upc, d.ASIN);
                            }
                            else
                            {
                                UPC = d.UPC;
                            }
                            if (d.UpdatedSalesPrice != null)
                            {
                                price = double.Parse(d.UpdatedSalesPrice);
                            }
                            else
                                if (d.SalesPrice != null)
                                {
                                    price = double.Parse(d.SalesPrice);
                                }
                            if (price != 0)
                            {


                                if (price < 200.01)
                                {

                                    double minval = 49.99;

                                    if (price > 0 && price <= 19.99)
                                    {
                                        pricemin = 92.99;
                                    }
                                    else if (price > 19.99 && price <= 29.99)
                                    {
                                        pricemin = 120.99;
                                    }
                                    else if (price > 29.99 && price <= 39.99)
                                    {
                                        pricemin = 142.99;
                                    }
                                    else if (price > 39.99 && price <= 49.99)
                                    {
                                        pricemin = 168.99;
                                    }
                                    else if (price > 49.99 && price <= 59.99)
                                    {
                                        pricemin = 192.99;
                                    }
                                    else if (price > 59.99 && price <= 69.99)
                                    {
                                        pricemin = 222.99;
                                    }
                                    else if (price > 69.99 && price <= 79.99)
                                    {
                                        pricemin = 255.99;
                                    }
                                    else if (price > 79.99 && price <= 89.99)
                                    {
                                        pricemin = 285.99;
                                    }
                                    else if (price > 89.99 && price <= 99.99)
                                    {
                                        pricemin = 313.99;
                                    }
                                    else if (price > 99.99 && price <= 109.99)
                                    {
                                        pricemin = 327.99;
                                    }
                                    else if (price > 109.99 && price <= 119.99)
                                    {
                                        pricemin = 356.99;
                                    }
                                    else if (price > 119.99 && price <= 129.99)
                                    {
                                        pricemin = 385.99;
                                    }
                                    else if (price > 129.99 && price <= 139.99)
                                    {
                                        pricemin = 420.99;
                                    }
                                    else if (price > 139.99 && price <= 149.99)
                                    {
                                        pricemin = 442.99;
                                    }
                                    else if (price > 149.99 && price <= 159.99)
                                    {
                                        pricemin = 463.99;
                                    }
                                    else if (price > 159.99 && price <= 169.99)
                                    {
                                        pricemin = 485.99;
                                    }
                                    else if (price > 169.99 && price <= 179.99)
                                    {
                                        pricemin = 513.99;
                                    }
                                    else if (price > 179.99 && price <= 189.99)
                                    {
                                        pricemin = 542.99;
                                    }
                                    else if (price > 189.99 && price <= 199.99)
                                    {
                                        pricemin = 569.99;
                                    }
                                    pricecal = price * PriceValue;
                                    if (pricemin >= pricecal)
                                    {
                                        finalprice = pricemin;
                                    }
                                    else
                                    {
                                        finalprice = pricecal;
                                    }



                                    string ItemName;
                                    if (d.Title != null)
                                    {
                                        // ItemName = new string(d.Title.Take(490).ToArray());
                                        ItemName = d.Title.Substring(0, d.Title.Length > 499 ? 499 : d.Title.Length);
                                    }
                                    else
                                    {
                                        ItemName = null;
                                    }
                                    string description;

                                    if (d.Description != null)
                                    {
                                        string desc = Regex.Replace(d.Description, "<.*?>", String.Empty);
                                        //description = new string(desc.Take(1990).ToArray());
                                        description = desc.Substring(0, desc.Length > 1990 ? 1990 : desc.Length);
                                    }
                                    else
                                    {
                                        description = null;
                                    }

                                    string manufacturer;
                                    string brand;
                                    if (d.Manufacturer == null && d.Brand == null)
                                    {
                                        manufacturer = "Unknown";
                                        brand = "Unknown";
                                    }
                                    else
                                    {
                                        if (d.Manufacturer == null)
                                        {
                                            manufacturer = d.Brand;
                                        }
                                        else
                                        {
                                            manufacturer = d.Manufacturer;
                                        }

                                        if (d.Brand == null)
                                        {
                                            brand = d.Manufacturer;
                                        }
                                        else
                                        {
                                            brand = d.Brand;
                                        }

                                    }
                                    string feature1, feature2, feature3, feature4;
                                    if (d.Features1 != null)
                                    {
                                        // feature1 = new string(d.Features1.Take(500).ToArray());
                                        feature1 = d.Features1.Substring(0, d.Features1.Length > 500 ? 500 : d.Features1.Length);

                                    }
                                    else
                                        feature1 = d.Features1;
                                    if (d.Features2 != null)
                                    {
                                        // feature2 = new string(d.Features2.Take(500).ToArray());
                                        feature2 = d.Features2.Substring(0, d.Features2.Length > 500 ? 500 : d.Features2.Length);
                                    }
                                    else
                                        feature2 = d.Features2;
                                    if (d.Features3 != null)
                                    {
                                        //feature3 = new string(d.Features3.Take(500).ToArray());
                                        feature3 = d.Features3.Substring(0, d.Features3.Length > 500 ? 500 : d.Features3.Length);
                                    }
                                    else
                                        feature3 = d.Features3;
                                    if (d.Features4 != null)
                                    {
                                        // feature4 = new string(d.Features4.Take(500).ToArray());
                                        feature4 = d.Features4.Substring(0, d.Features4.Length > 500 ? 500 : d.Features4.Length);
                                    }
                                    else
                                        feature4 = d.Features4;
                                    sb.AppendLine(string.Join("\t",
                                                   string.Format(@"""{0}""", d.ASIN.Trim()),
                                                   string.Format(@"""{0}""", !string.IsNullOrEmpty(ItemName) ? ItemName.Contains('"') ? ItemName.Replace('"', ' ') : ItemName : string.Empty),
                                                   string.Format(@"""{0}""", manufacturer),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", "FashionOther"),
                                                   string.Format(@"""{0}""", brand),
                                                   string.Format(@"""{0}""", UPC),
                                                   string.Format(@"""{0}""", "UPC"),
                                                   string.Format(@"""{0}""", !string.IsNullOrEmpty(description) ? description.Contains('"') ? description.Replace('"', ' ') : description : string.Empty),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", String.Format("{0:0.00}", finalprice)),
                                                   string.Format(@"""{0}""", Quantity),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", FulfillmentLatency),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", !string.IsNullOrEmpty(feature1) ? feature1.Contains('"') ? feature1.Replace('"', ' ') : feature1 : string.Empty),
                                                   string.Format(@"""{0}""", !string.IsNullOrEmpty(feature2) ? feature2.Contains('"') ? feature2.Replace('"', ' ') : feature2 : string.Empty),
                                                   string.Format(@"""{0}""", !string.IsNullOrEmpty(feature3) ? feature3.Contains('"') ? feature3.Replace('"', ' ') : feature3 : string.Empty),
                                                   string.Format(@"""{0}""", !string.IsNullOrEmpty(feature4) ? feature4.Contains('"') ? feature4.Replace('"', ' ') : feature4 : string.Empty),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", d.LargeImageUrl),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", ""),
                                                   string.Format(@"""{0}""", "")));

                                    d.FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_Prime_Jewelry_Canada_" + fcountWatches;

                                    d.ExportDate = DateTime.Now;
                                    d.Instock = 1;

                                    if (shortcode == ConstantData.DC)
                                    {
                                        d.Account3_Status = 1;
                                        d.Account3_ExportDate = DateTime.Now;
                                        Exportpath = d.Account3_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_DC_Prime_Jewelry_Canada_" + fcountJewelry;
                                    }
                                    d.Status = 1;


                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                    ukdb.ObjectContext().SaveChanges(SaveOptions.DetectChangesBeforeSave);

                    // ukdb.SaveChanges();
                    fcountJewelry++;
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=" + Exportpath + ".txt");
                    //Response.AddHeader("content-disposition", "attachment;filename="+name +"_"+ DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
                    Response.ContentType = "application/text";
                    Response.ContentEncoding = Encoding.GetEncoding(1252);
                    Response.Write(sb.ToString());
                    Response.End();
                }
                transaction.Complete();
                ukdb.ObjectContext().AcceptAllChanges();
            }
        }
        private void ExportHomeAndKitchen(double PriceValue, string shortcode, IEnumerable<tbl_HomeandKitchen> data)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Suppress))
            {
                string ExportPath = "";
                ((IObjectContextAdapter)ukdb).ObjectContext.CommandTimeout = 1800;
                StringWriter st = new StringWriter();
                string header1 = "TemplateType=home	Version=2016.0505	The top 3 rows are for Amazon.com use only. Do not modify or delete the top 3 rows.								Offer-Offer Information - These attributes are required to make your item buyable for customers on the site.																	Dimensions-Product Dimensions - These attributes specify the size and weight of a product.														Discovery-Item discovery information - These attributes have an effect on how customers can find your product on the site using browse or search.								Images-Image Information - See Image Instructions tab for details.										Fulfillment-Use these columns to provide fulfilment-related information for orders fulfilled either by Amazon (FBA) or by the Seller.					Variation-Variation information - Populate these attributes if your product is available in different variations (for example colour or wattage).				Compliance-Compliance Information - Attributes used to comply with consumer laws in the country or region where the item is sold.	Ungrouped - These attributes create rich product listings for your buyers.";
                string header2 = "Seller SKU	Product ID	Product ID Type	Item Name (aka Title)	Brand Name	Manufacturer	Product Type	Manufacturer Part Number	Product Description	Update Delete	Launch Date	Standard Price	Quantity	Package Quantity	Product Tax Code	Release Date	Sale Price	Sale From Date	Sale End Date	Item Condition	Offer Condition Note	Fulfillment Latency	Maximum Aggregate Ship Quantity	Can Be Gift Messaged	Is Gift Wrap Available?	Is Discontinued by Manufacturer	Merchant Shipping Group	Shipping Weight	Website Shipping Weight Unit Of Measure	Display Length	Item Display Length Unit Of Measure	Display Width	Item Display Width Unit Of Measure	Display Height	Item Display Height Unit Of Measure	Display Diameter	Item Display Diameter Unit Of Measure	Display Weight	Item Display Weight Unit Of Measure	Volume Capacity of the appliance	Item Display Volume Unit Of Measure	Recommended Browse Nodes	Key Product Features	Key Product Features	Key Product Features	Key Product Features	Key Product Features	Search Terms	Home & Furniture Target Audience	Main Image URL	Swatch Image URL	Other Image Url	Other Image Url	Other Image Url	Other Image Url	Other Image Url	Other Image Url	Other Image Url	Other Image Url	Package Length	Package Width	Package Height	Package Length Unit Of Measure	Fulfillment Centre ID	Parentage	Parent SKU	Relationship Type	Variation Theme	Country Produced In	Age restriction bladed products	Colour	Size	Unit of Measure (Per Unit Pricing)	Unit Count (Per Unit Pricing)	Subject	Style Name	Special Features	Paint Type	Material Type	Shape	Color Map	Capacity	Capacity Unit Of Measure	Energy efficiency class	Scent	Thread Count	Wattage	Usage	Design	Occasion Type	Number of doors	Material Composition	Bed and Mattress/Sofa type	Mattress Firmness	Indoor Fountain Installation Type	Bed Frame Type	Batteries are Included	BatteryType	Number of Batteries	Outer Material Type	Minimum Age Recommendation";
                string header3 = "item_sku	external_product_id	external_product_id_type	item_name	brand_name	manufacturer	feed_product_type	part_number	product_description	update_delete	product_site_launch_date	standard_price	quantity	item_package_quantity	product_tax_code	merchant_release_date	sale_price	sale_from_date	sale_end_date	condition_type	condition_note	fulfillment_latency	max_aggregate_ship_quantity	offering_can_be_gift_messaged	offering_can_be_giftwrapped	is_discontinued_by_manufacturer	merchant_shipping_group_name	website_shipping_weight	website_shipping_weight_unit_of_measure	item_display_length	item_display_length_unit_of_measure	item_display_width	item_display_width_unit_of_measure	item_display_height	item_display_height_unit_of_measure	item_display_diameter	item_display_diameter_unit_of_measure	item_display_weight	item_display_weight_unit_of_measure	item_display_volume	item_display_volume_unit_of_measure	recommended_browse_nodes	bullet_point1	bullet_point2	bullet_point3	bullet_point4	bullet_point5	generic_keywords	target_audience_keywords	main_image_url	swatch_image_url	other_image_url1	other_image_url2	other_image_url3	other_image_url4	other_image_url5	other_image_url6	other_image_url7	other_image_url8	package_length	package_width	package_height	package_length_unit_of_measure	fulfillment_center_id	parent_child	parent_sku	relationship_type	variation_theme	country_string	customer_restriction_type	color_name	size_name	unit_count_type	unit_count	theme	style_name	special_features	paint_type	material_type	item_shape	color_map	capacity	capacity_unit_of_measure	efficiency	scent_name	thread_count	wattage	specific_uses_for_product	pattern_name	occasion_type	number_of_doors	material_composition	item_type_name	item_hardness	installation_type	frame_type	are_batteries_included	battery_type	number_of_batteries	outer_material_type	mfg_minimum";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(header1);
                sb.AppendLine(header2);
                sb.AppendLine(header3);
                double addToMinVal = double.Parse(ConfigurationManager.AppSettings["ProdPriceAddToMinValUK"]);// 27-June-2016 making is configurable. It adds vaue accoding to range define in loop.

                //IEnumerable<tbl_HomeandKitchen> data = ukdb.tbl_HomeandKitchen.Where(x => x.UPC != "null" && x.Status == 0 && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(count.ToString())).ToList();
                if (data != null)
                {
                    foreach (var d in data)
                    {
                        try
                        {


                            double price = 0;
                            double pricecal;
                            double pricemin = 0.0;
                            double finalprice;
                            string UPC;
                            if (d.UPC == null)
                            {
                                upc_codes uc = ukdb.upc_codes.Where(x => x.AssignedTo == null).FirstOrDefault();
                                UPC = uc.Upc;
                                
                                UpdateUPC(uc.Upc, d.ASIN);
                            }
                            else
                            {
                                UPC = d.UPC;
                            }
                            if (d.UpdatedSalesPrice != null)
                            {
                                price = Convert.ToDouble(d.UpdatedSalesPrice);
                            }
                            else
                                if (d.SalesPrice != null)
                                {
                                    price = Convert.ToDouble(d.SalesPrice);
                                }
                            if (price != 0)
                            {
                                double minval = double.Parse(ConfigurationManager.AppSettings["ProdPriceMinValUK"]);  // 27-June-2016 making it configurable. Minimum Product Price can Change from configuration file. 

                                if (price > 0 && price <= 19.99)
                                {
                                    pricemin = minval;
                                }
                                else
                                {
                                    for (double i = 20; i < 500; i += 10)
                                    {
                                        minval = minval + addToMinVal;
                                        double temp = i + 10;
                                        if (price >= i && price < temp)
                                        {
                                            pricemin = minval;
                                            break;
                                        }
                                    }
                                }
                                pricecal = price * PriceValue;
                                if (pricemin >= pricecal)
                                {
                                    finalprice = pricemin;
                                }
                                else
                                {
                                    finalprice = pricecal;
                                }
                                string ItemName;
                                if (d.Title != null)
                                {
                                    // ItemName = new string(d.Title.Take(490).ToArray());
                                    ItemName = d.Title.Substring(0, d.Title.Length > 499 ? 499 : d.Title.Length);
                                }
                                else
                                {
                                    ItemName = null;
                                }
                                string description;

                                if (d.Description != null)
                                {
                                    string desc = Regex.Replace(d.Description, "<.*?>", String.Empty);
                                    //description = new string(desc.Take(1990).ToArray());
                                    description = desc.Substring(0, desc.Length > 1990 ? 1990 : desc.Length);
                                }
                                else
                                {
                                    description = null;
                                }

                                string manufacturer;
                                string brand;
                                if (d.Manufacturer == null && d.Brand == null)
                                {
                                    manufacturer = "Unknown";
                                    brand = "Unknown";
                                }
                                else
                                {
                                    if (d.Manufacturer == null)
                                    {
                                        manufacturer = d.Brand;
                                    }
                                    else
                                    {
                                        manufacturer = d.Manufacturer;
                                    }

                                    if (d.Brand == null)
                                    {
                                        brand = d.Manufacturer;
                                    }
                                    else
                                    {
                                        brand = d.Brand;
                                    }

                                }
                                string feature1, feature2, feature3, feature4;
                                if (d.Features1 != null)
                                {
                                    // feature1 = new string(d.Features1.Take(500).ToArray());
                                    feature1 = d.Features1.Substring(0, d.Features1.Length > 500 ? 500 : d.Features1.Length);

                                }
                                else
                                    feature1 = d.Features1;
                                if (d.Features2 != null)
                                {
                                    // feature2 = new string(d.Features2.Take(500).ToArray());
                                    feature2 = d.Features2.Substring(0, d.Features2.Length > 500 ? 500 : d.Features2.Length);
                                }
                                else
                                    feature2 = d.Features2;
                                if (d.Features3 != null)
                                {
                                    //feature3 = new string(d.Features3.Take(500).ToArray());
                                    feature3 = d.Features3.Substring(0, d.Features3.Length > 500 ? 500 : d.Features3.Length);
                                }
                                else
                                    feature3 = d.Features3;
                                if (d.Features4 != null)
                                {
                                    // feature4 = new string(d.Features4.Take(500).ToArray());
                                    feature4 = d.Features4.Substring(0, d.Features4.Length > 500 ? 500 : d.Features4.Length);
                                }
                                else
                                    feature4 = d.Features4;
                                #region
                                sb.AppendLine(string.Join("\t",
                                    string.Format(@"""{0}""", d.ASIN.Trim()),
                                    string.Format(@"""{0}""", UPC),
                                    string.Format(@"""{0}""", "UPC"),
                                    string.Format(@"""{0}""", !string.IsNullOrEmpty(ItemName) ? ItemName.Contains('"') ? ItemName.Replace('"', ' ') : ItemName : string.Empty),
                                    string.Format(@"""{0}""", brand),
                                    string.Format(@"""{0}""", manufacturer),
                                    string.Format(@"""{0}""", "Home"),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", !string.IsNullOrEmpty(description) ? description.Contains('"') ? description.Replace('"', ' ') : description : string.Empty),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", String.Format("{0:0.00}", finalprice)),
                                    string.Format(@"""{0}""", Quantity),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", FulfillmentLatency),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", "1063498"),
                                    string.Format(@"""{0}""", !string.IsNullOrEmpty(feature1) ? feature1.Contains('"') ? feature1.Replace('"', ' ') : feature1 : string.Empty),
                                    string.Format(@"""{0}""", !string.IsNullOrEmpty(feature2) ? feature2.Contains('"') ? feature2.Replace('"', ' ') : feature2 : string.Empty),
                                    string.Format(@"""{0}""", !string.IsNullOrEmpty(feature3) ? feature3.Contains('"') ? feature3.Replace('"', ' ') : feature3 : string.Empty),
                                    string.Format(@"""{0}""", !string.IsNullOrEmpty(feature4) ? feature4.Contains('"') ? feature4.Replace('"', ' ') : feature4 : string.Empty),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", d.LargeImageURL),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", ""),
                                    string.Format(@"""{0}""", "")));
                                #endregion
                                d.FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_Prime_HomeandKitchen_" + fcountHomeandKitchen;
                                d.ExportDate = DateTime.Now;
                                d.Instock = 1;
                                if (shortcode == "ED")
                                {
                                    d.Account1_Status = 1;
                                    d.Account1_ExportDate = DateTime.Now;
                                    ExportPath = d.Account1_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_ED_Prime_HomeAndKitchen_" + fcountHomeandKitchen;

                                }
                                else if (shortcode == "EM")
                                {
                                    d.Account2_Status = 1;
                                    d.Account2_ExportDate = DateTime.Now;
                                    ExportPath = d.Account2_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_EM_Prime_HomeandKitchen_" + fcountHomeandKitchen;
                                }
                                else if (shortcode == "DC")
                                {
                                    d.Account3_Status = 1;
                                    d.Account3_ExportDate = DateTime.Now;
                                    ExportPath = d.Account3_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_DC_Prime_HomeandKitchen_" + fcountHomeandKitchen;
                                }
                                else if (shortcode == "DI")
                                {
                                    d.Account4_Status = 1;
                                    d.Account4_ExportDate = DateTime.Now;
                                    ExportPath = d.Account4_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_DI_Prime_HomeandKitchen_" + fcountHomeandKitchen;
                                }
                                d.Status = 1;


                            }
                        }
                        catch (Exception ex)
                        {


                        }

                    }
                    ukdb.ObjectContext().SaveChanges(SaveOptions.DetectChangesBeforeSave);

                    // ukdb.SaveChanges();
                    fcountHomeandKitchen++;
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=" + ExportPath + ".txt");
                    //Response.AddHeader("content-disposition", "attachment;filename="+name +"_"+ DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
                    Response.ContentType = "application/text";
                    Response.ContentEncoding = Encoding.GetEncoding(1252);
                    Response.Write(sb.ToString());
                    Response.End();
                }
                transaction.Complete();
                ukdb.ObjectContext().AcceptAllChanges();
            }
        }
        public void ExportHomeAndKitchenCanada(double PriceValue, string shortcode, IEnumerable<tbl_HomeandKitchen> data)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Suppress))
            {
                string ExportPath = "";
                ((IObjectContextAdapter)ukdb).ObjectContext.CommandTimeout = 1800;
                StringWriter st = new StringWriter();
                string header1 = "TemplateType=Home	Version=2015.1204	The top 3 rows are for Amazon.com use only. Do not modify or delete the top 3 rows.								Offer - Offer Information - These attributes are required to make your item buyable for customers on the site.																				Dimensions - Product Dimensions - These attributes specify the size and weight of a product.								Discovery - Item discovery information - These attributes have an effect on how customers can find your product on the site using browse or search.												Images - Image Information - See Image Instructions tab for details.					Fulfillment - Use these columns to provide fulfillment-related information for either Amazon-fulfilled (FBA) or seller-fulfilled orders.							Variation - Variation information - Populate these attributes if your product is available in different variations (for example colour or wattage).				Compliance - Compliance Information - Attributes used to comply with consumer laws in the country or region where the item is sold.			Ungrouped - These attributes create rich product listings for your buyers.";
                string header2 = "Seller SKU	Product ID	Product ID Type	Product Name	Product Description	Product Type	Brand Name	Manufacturer	Manufacturer Part Number	Update Delete	Standard Price	Item Condition	Offer Condition Note	Quantity	Manufacturer's Suggested Retail Price	Minimum Advertised Price	Launch Date	Release Date	Restock Date	Fulfillment Latency	Product Tax Code	Sale Price	Sale Start Date	Sale End Date	Package Quantity	Max Aggregate Ship Quantity	Offering Can Be Gift Messaged	Is Gift Wrap Available	Is Discontinued by Manufacturer	Shipping-Template	Shipping Weight	Website Shipping Weight Unit Of Measure	Item Display Length	Item Display Width	Item Display Height	Display Dimensions Unit Of Measure	Item Display Weight	Item Display Weight Unit Of Measure	Key Product Features1	Key Product Features2	Key Product Features3	Key Product Features4	Key Product Features5	recommended-browse-nodes	Intended Use	Target Audience1	Target Audience2	Target Audience3	Subject Matter	Search Terms	Main Image URL	Swatch Image URL	Other Image Url1	Other Image Url2	Other Image Url3	Fulfillment Center ID	Package Height	Package Width	Package Length	Package Dimensions Unit Of Measure	Package Weight	Package Weight Unit Of Measure	Parentage	Parent SKU	Relationship Type	Variation Theme	Cpsia Warning	CPSIA Warning Description	Country of Origin	Manufacturer Warranty Description	Other Attributes	Fabric Type	Import Designation	Country as Labeled	Fur Description	Paint Type	Number of Sets	Occasion	Thread Count	Number of Pieces	Shape	Scent Name	Pattern	Included Components	Color	Color Map	Size	Material Type	Style Name	PowerSource	Wattage	Specific Uses For Product	Additional Features	Batteries are Included	Battery Type	Number of Batteries Required	Lithium Battery Packaging	Lithium Battery Voltage	Lithium Battery Weight	Voltage	Blade Material Type";
                string header3 = "item_sku	external_product_id	external_product_id_type	item_name	product_description	feed_product_type	brand_name	manufacturer	part_number	update_delete	standard_price	condition_type	condition_note	quantity	list_price	map_price	product_site_launch_date	merchant_release_date	restock_date	fulfillment_latency	product_tax_code	sale_price	sale_from_date	sale_end_date	item_package_quantity	max_aggregate_ship_quantity	offering_can_be_gift_messaged	offering_can_be_giftwrapped	is_discontinued_by_manufacturer	merchant_shipping_group_name	website_shipping_weight	website_shipping_weight_unit_of_measure	item_display_length	item_display_width	item_display_height	display_dimensions_unit_of_measure	item_display_weight	item_display_weight_unit_of_measure	bullet_point1	bullet_point2	bullet_point3	bullet_point4	bullet_point5	recommended_browse_nodes	specific_uses_keywords	target_audience_keywords1	target_audience_keywords2	target_audience_keywords3	thesaurus_subject_keywords	generic_keywords	main_image_url	swatch_image_url	other_image_url1	other_image_url2	other_image_url3	fulfillment_center_id	package_height	package_width	package_length	package_dimensions_unit_of_measure	package_weight	package_weight_unit_of_measure	parent_child	parent_sku	relationship_type	variation_theme	cpsia_cautionary_statement	cpsia_cautionary_description	country_string	warranty_description	thesaurus_attribute_keywords	fabric_type	import_designation	country_as_labeled	fur_description	paint_type	number_of_sets	occasion	thread_count	number_of_pieces	item_shape	scent_name	pattern_name	included_components	color_name	color_map	size_name	material_type	style_name	power_source_type	wattage	specific_uses_for_product	special_features	are_batteries_included	battery_type	number_of_batteries	lithium_battery_packaging	lithium_battery_voltage	lithium_battery_weight	voltage	blade_material_type";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(header1);
                sb.AppendLine(header2);
                sb.AppendLine(header3);
                //IEnumerable<tbl_HomeandKitchen> data = ukdb.tbl_HomeandKitchen.Where(x => x.UPC != "null" && x.Status == 0 && x.Prime == "1" && x.SalesPrice != "Too low to display" && x.WeightUnits < 501 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000).Take(int.Parse(count.ToString())).ToList();
                if (data != null)
                {
                    foreach (var d in data)
                    {

                        try
                        {


                            double price = 0;
                            double pricecal;
                            double pricemin = 0.0;
                            double finalprice;
                            string UPC;
                            if (d.UPC == null)
                            {
                                upc_codes uc = ukdb.upc_codes.Where(x => x.AssignedTo == null).FirstOrDefault();
                                UPC = uc.Upc;
                                
                                UpdateUPC(uc.Upc, d.ASIN);
                            }
                            else
                            {
                                UPC = d.UPC;
                            }
                            if (d.UpdatedSalesPrice != null)
                            {
                                price = Convert.ToDouble(d.UpdatedSalesPrice);
                            }
                            else
                                if (d.SalesPrice != null)
                                {
                                    price = Convert.ToDouble(d.SalesPrice);
                                }
                            if (price != 0)
                            {
                                if (price < 200.01)
                                {

                                    double minval = 49.99;

                                    if (price > 0 && price <= 19.99)
                                    {
                                        pricemin = 92.99;
                                    }
                                    else if (price > 19.99 && price <= 29.99)
                                    {
                                        pricemin = 120.99;
                                    }
                                    else if (price > 29.99 && price <= 39.99)
                                    {
                                        pricemin = 142.99;
                                    }
                                    else if (price > 39.99 && price <= 49.99)
                                    {
                                        pricemin = 168.99;
                                    }
                                    else if (price > 49.99 && price <= 59.99)
                                    {
                                        pricemin = 192.99;
                                    }
                                    else if (price > 59.99 && price <= 69.99)
                                    {
                                        pricemin = 222.99;
                                    }
                                    else if (price > 69.99 && price <= 79.99)
                                    {
                                        pricemin = 255.99;
                                    }
                                    else if (price > 79.99 && price <= 89.99)
                                    {
                                        pricemin = 285.99;
                                    }
                                    else if (price > 89.99 && price <= 99.99)
                                    {
                                        pricemin = 313.99;
                                    }
                                    else if (price > 99.99 && price <= 109.99)
                                    {
                                        pricemin = 327.99;
                                    }
                                    else if (price > 109.99 && price <= 119.99)
                                    {
                                        pricemin = 356.99;
                                    }
                                    else if (price > 119.99 && price <= 129.99)
                                    {
                                        pricemin = 385.99;
                                    }
                                    else if (price > 129.99 && price <= 139.99)
                                    {
                                        pricemin = 420.99;
                                    }
                                    else if (price > 139.99 && price <= 149.99)
                                    {
                                        pricemin = 442.99;
                                    }
                                    else if (price > 149.99 && price <= 159.99)
                                    {
                                        pricemin = 463.99;
                                    }
                                    else if (price > 159.99 && price <= 169.99)
                                    {
                                        pricemin = 485.99;
                                    }
                                    else if (price > 169.99 && price <= 179.99)
                                    {
                                        pricemin = 513.99;
                                    }
                                    else if (price > 179.99 && price <= 189.99)
                                    {
                                        pricemin = 542.99;
                                    }
                                    else if (price > 189.99 && price <= 199.99)
                                    {
                                        pricemin = 569.99;
                                    }
                                    pricecal = price * PriceValue;
                                    if (pricemin >= pricecal)
                                    {
                                        finalprice = pricemin;
                                    }
                                    else
                                    {
                                        finalprice = pricecal;
                                    }
                                    string ItemName;
                                    if (d.Title != null)
                                    {
                                        // ItemName = new string(d.Title.Take(490).ToArray());
                                        ItemName = d.Title.Substring(0, d.Title.Length > 499 ? 499 : d.Title.Length);
                                    }
                                    else
                                    {
                                        ItemName = null;
                                    }
                                    string description;

                                    if (d.Description != null)
                                    {
                                        string desc = Regex.Replace(d.Description, "<.*?>", String.Empty);
                                        //description = new string(desc.Take(1990).ToArray());
                                        description = desc.Substring(0, desc.Length > 1990 ? 1990 : desc.Length);
                                    }
                                    else
                                    {
                                        description = null;
                                    }

                                    string manufacturer;
                                    string brand;
                                    if (d.Manufacturer == null && d.Brand == null)
                                    {
                                        manufacturer = "Unknown";
                                        brand = "Unknown";
                                    }
                                    else
                                    {
                                        if (d.Manufacturer == null)
                                        {
                                            manufacturer = d.Brand;
                                        }
                                        else
                                        {
                                            manufacturer = d.Manufacturer;
                                        }

                                        if (d.Brand == null)
                                        {
                                            brand = d.Manufacturer;
                                        }
                                        else
                                        {
                                            brand = d.Brand;
                                        }

                                    }
                                    string feature1, feature2, feature3, feature4;
                                    if (d.Features1 != null)
                                    {
                                        // feature1 = new string(d.Features1.Take(500).ToArray());
                                        feature1 = d.Features1.Substring(0, d.Features1.Length > 500 ? 500 : d.Features1.Length);

                                    }
                                    else
                                        feature1 = d.Features1;
                                    if (d.Features2 != null)
                                    {
                                        // feature2 = new string(d.Features2.Take(500).ToArray());
                                        feature2 = d.Features2.Substring(0, d.Features2.Length > 500 ? 500 : d.Features2.Length);
                                    }
                                    else
                                        feature2 = d.Features2;
                                    if (d.Features3 != null)
                                    {
                                        //feature3 = new string(d.Features3.Take(500).ToArray());
                                        feature3 = d.Features3.Substring(0, d.Features3.Length > 500 ? 500 : d.Features3.Length);
                                    }
                                    else
                                        feature3 = d.Features3;
                                    if (d.Features4 != null)
                                    {
                                        // feature4 = new string(d.Features4.Take(500).ToArray());
                                        feature4 = d.Features4.Substring(0, d.Features4.Length > 500 ? 500 : d.Features4.Length);
                                    }
                                    else
                                        feature4 = d.Features4;
                                    sb.AppendLine(string.Join("\t",
                                        string.Format(@"""{0}""", d.ASIN.Trim()),
                                        string.Format(@"""{0}""", UPC),
                                        string.Format(@"""{0}""", "UPC"),
                                        string.Format(@"""{0}""", !string.IsNullOrEmpty(ItemName) ? ItemName.Contains('"') ? ItemName.Replace('"', ' ') : ItemName : string.Empty),
                                         string.Format(@"""{0}""", !string.IsNullOrEmpty(description) ? description.Contains('"') ? description.Replace('"', ' ') : description : string.Empty),
                                         string.Format(@"""{0}""", "Home"),
                                        string.Format(@"""{0}""", brand),
                                        string.Format(@"""{0}""", manufacturer),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", String.Format("{0:0.00}", finalprice)),
                                         string.Format(@"""{0}""", ""),
                                         string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", Quantity),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", FulfillmentLatency),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", !string.IsNullOrEmpty(feature1) ? feature1.Contains('"') ? feature1.Replace('"', ' ') : feature1 : string.Empty),
                                        string.Format(@"""{0}""", !string.IsNullOrEmpty(feature2) ? feature2.Contains('"') ? feature2.Replace('"', ' ') : feature2 : string.Empty),
                                        string.Format(@"""{0}""", !string.IsNullOrEmpty(feature3) ? feature3.Contains('"') ? feature3.Replace('"', ' ') : feature3 : string.Empty),
                                        string.Format(@"""{0}""", !string.IsNullOrEmpty(feature4) ? feature4.Contains('"') ? feature4.Replace('"', ' ') : feature4 : string.Empty),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", "1063498"),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", d.LargeImageURL),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", ""),
                                        string.Format(@"""{0}""", "")));

                                    d.FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_Prime_HomeandKitchen_" + fcountHomeandKitchen;
                                    d.ExportDate = DateTime.Now;
                                    d.Instock = 1;
                                    if (shortcode == "DC")
                                    {
                                        d.Account3_Status = 1;
                                        d.Account3_ExportDate = DateTime.Now;
                                        ExportPath = d.Account3_FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_DC_Prime_HomeandKitchen_" + fcountHomeandKitchen;
                                    }

                                    d.Status = 1;


                                }

                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    ukdb.ObjectContext().SaveChanges(SaveOptions.DetectChangesBeforeSave);

                    // ukdb.SaveChanges();
                    fcountHomeandKitchen++;
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=" + ExportPath + ".txt");
                    //Response.AddHeader("content-disposition", "attachment;filename="+name +"_"+ DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
                    Response.ContentType = "application/text";
                    Response.ContentEncoding = Encoding.GetEncoding(1252);
                    Response.Write(sb.ToString());
                    Response.End();

                    transaction.Complete();
                    ukdb.ObjectContext().AcceptAllChanges();
                }
            }
        }

        private void UpdateUPC(string upc, string asin)
        {
            using(SqlConnection con=new SqlConnection(ConfigurationManager.ConnectionStrings["UKMain"].ToString()))
            {

                using (SqlCommand cmd = new SqlCommand("UpdateFakeUPC", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ASIN", asin);
                    cmd.Parameters.Add("@UPC", upc);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    
                }
            }
        }
        public partial class InboundProductInventoryAdjustmentAudits
        {
            public Guid AmazonAccountID { get; set; }
            public string AmazonAccountName { get; set; }
            public string ShipmentID { get; set; }
            public Guid? ProductID { get; set; }
            public string ProductName { get; set; }
            public decimal ShouldBe { get; set; }
            public decimal ActuallyIs { get; set; }
            public bool MissingProductAssociations { get; set; }
            public string SKU { get; set; }
            public Nullable<decimal> QuantityShipped { get; set; }
            public Nullable<decimal> QuantityReceived { get; set; }
        }
        public ActionResult ServiceStatus()
        {

            ServiceController[] services = ServiceController.GetServices();

            ViewBag.Service = services;



            return View();
        }
        public ActionResult Serviceinfo(string sc)
        {
            ServiceController st = new ServiceController(sc);

            st.Start();

            return RedirectToAction("ServiceStatus");
        }
        public ActionResult AllowStores()
        {
            //ViewBag.AccountStore = new SelectList(ukdb.tbl_Account.Select(x => x.Name));
            ViewBag.AccountStore = ukdb.tbl_Account.OrderBy(x => x.Name);

            return View();
        }
        public JsonResult Store(string acc)
        {

            var acountname = ukdb.tbl_Account.Where(x => x.Name == acc).Select(x => x.ReExportAllow).ToList();
            return Json(acountname, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult AllowStores(AllowStores als, string submit, string name)
        {
            if (submit == "Start")
            {

                tbl_Account ta;
                ta = ukdb.tbl_Account.Where(x => x.Name == als.Selling).SingleOrDefault();
                ta.ReExportAllow = 1;
                ukdb.SaveChanges();
                TempData["msg"] = "Service Started Successfully.";

            }
            else
                if (submit == "Stop")
                {
                    tbl_Account ta;
                    ta = ukdb.tbl_Account.Where(x => x.Name == als.Selling).SingleOrDefault();
                    ta.ReExportAllow = 0;
                    ukdb.SaveChanges();
                    TempData["msg"] = "Service Stopped Successfully.";
                }
            //ViewBag.AccountStore = new SelectList(ukdb.tbl_Account.Select(x => x.Name));
            ViewBag.AccountStore = ukdb.tbl_Account.OrderBy(x => x.Name);
            return View();
        }
        public ActionResult AddKeywords()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddKeywords(AddKeywords ak)
        {
            if (ModelState.IsValid)
            {

                tbl_Keywords tk = new tbl_Keywords();
                tk.Keywords = ak.keyword;
                tk.Priority = 1;
                ukdb.tbl_Keywords.Add(tk);
                ukdb.SaveChanges();
            }
            ModelState.Clear();
            return View();
        }
    }
}
