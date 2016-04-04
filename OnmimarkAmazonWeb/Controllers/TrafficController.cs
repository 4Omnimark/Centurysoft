using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OmnimarkAmazon.Models;
using System.IO;
using System.Data;

namespace OmnimarkAmazonWeb.Controllers
{
    public class TrafficController : _BaseController
    {
        public ActionResult UploadData()
        {
            ViewBag.Title = "Traffic Data Import";

            var model = db.Database.SqlQuery<ImportedTrafficRec>("GetImportedTraffic").ToList();

            ViewBag.AccountNames = db.AmazonAccounts.Where(aa => aa.DoTrafficImport).OrderBy(aa => aa.DisplaySeq).ToList();

            return View(model);
        }

        class CheckAccountResult
        {
            public Guid AmazonAccountID { get; set; }
            public int Count { get; set; }
        }

        [HttpPost]
        public ActionResult DoUpload(DateTime Date, Guid AmazonAccountID)
        {

            if (Request.Files.Count == 1)
            {
                int rowcnt = 0;

                try
                {

                    DataTable dt = Startbutton.Library.TextStreamToDataTable(((HttpPostedFileBase)Request.Files[0]).InputStream, ",", true, true);

                    #region Check for right account

                    //string SKUs = "'";

                    //foreach (DataRow dr in dt.Rows)
                    //    SKUs += (SKUs == "'" ? "" : "','") + (string)dr["SKU"];

                    //SKUs += "'";

                    //List<CheckAccountResult> CheckAccounts = db.ExecuteStoreQuery<CheckAccountResult>("SELECT AmazonAccountID,count(*) as [Count] from AmazonInventorySKUs where SKU in (" + SKUs + ") group by AmazonAccountID").ToList();

                    //if (CheckAccounts.Count != 1 || CheckAccounts.First().AmazonAccountID != AmazonAccountID)
                    //{
                    //    string StoreList = "";

                    //    foreach (CheckAccountResult acct in CheckAccounts)
                    //    {
                    //        Guid AcctID = (Guid)acct.AmazonAccountID;
                    //        StoreList += (StoreList == "" ? "" : ", ") + db.AmazonAccounts.Single(aa => aa.ID == AcctID).Name;
                    //    }

                    //    return Json(new { Success = false, Error = "Data is not for selected store! Found SKUs for: " + StoreList });
                    //}

                    #endregion

                    bool UsesGrossProductSales = false;

                    if (dt.Columns.Contains("Gross Product Sales"))
                        UsesGrossProductSales = true;

                    foreach (DataRow dr in dt.Rows)
                    {
                        rowcnt++;

                        DailyTraffic rec = new DailyTraffic();
                        rec.Date = Date;
                        rec.AmazonAccountID = AmazonAccountID;
                        rec.ASIN = (string)dr["(Child) ASIN"];
                        rec.SKU = (string)dr["SKU"];
                        rec.Sessions = int.Parse(((string)dr["Sessions"]).Replace(",", ""));
                        rec.SessionPercentage = decimal.Parse(((string)dr["Session Percentage"]).Replace("%", "").Replace(",", ""));
                        rec.PageViews = int.Parse(((string)dr["Page Views"]).Replace(",", ""));
                        rec.PageViewPercentage = decimal.Parse(((string)dr["Page Views Percentage"]).Replace("%", "").Replace(",", ""));
                        rec.BuyBoxPercentage = decimal.Parse(((string)dr["Buy Box Percentage"]).Replace("%", "").Replace(",", ""));
                        rec.UnitsOrdered = int.Parse(((string)dr["Units Ordered"]).Replace(",", ""));
                        rec.UnitSessionPercentage = decimal.Parse(((string)dr["Unit Session Percentage"]).Replace("%", "").Replace(",", ""));
                        
                        if (UsesGrossProductSales)
                            rec.OrderedProductSales = decimal.Parse(((string)dr["Gross Product Sales"]).Replace("$", "").Replace(",", ""));
                        else
                            rec.OrderedProductSales = decimal.Parse(((string)dr["Ordered Product Sales"]).Replace("$", "").Replace(",", ""));

                        rec.TotalOrderItems = int.Parse(((string)dr["Total Order Items"]).Replace(",", ""));
                        rec.TimeStamp = DateTime.Now;

                        db.DailyTraffics.Add(rec);
                    }

                    db.SaveChanges();

                    return Json(new { Success = true });
                }
                catch (Exception Ex)
                {
                    return Json(new { Success = false, Error = "ROW " + (rowcnt + 1).ToString() + ": " + Ex.Message + "\n\n" + Ex.StackTrace });
                }
            }
            else
                return Json(new { Success = false, Error = "No file received!" });
        }

    }
}
