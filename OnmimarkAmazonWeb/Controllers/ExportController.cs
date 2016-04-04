using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OmnimarkAmazon.Models;
using System.Reflection;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;

namespace OmnimarkAmazonWeb.Controllers
{
    public class ExportController : _BaseController
    {

        public ActionResult Index()
        {
            var model = db.ExportSpecs.OrderBy(e => e.Name);
            return View("Index", model);
        }

        void LoadSalesVenues()
        {
            ViewBag.SalesVenues = new SelectList(db.SalesVenues.OrderBy(s => s.Name), "ID", "Name");
        }

        [HttpPost]
        public ActionResult UploadBuyResponse()
        {
            string RtnMsg = "";

            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = (HttpPostedFileBase)Request.Files[file];

                StreamReader sr = new StreamReader(hpf.InputStream);

                string line = null;
                int cnt = 0;

                while ((line = sr.ReadLine()) != null)
                {
                    string[] cols = line.Split('\t');

                    if (cols[1] == "1")
                    {
                        ASINsSuccessfullyExported ase = new ASINsSuccessfullyExported();
                        string SKU = cols[0];
                        ase.ASIN = db.AmazonInventorySKUs.Single(ais => ais.SKU == SKU).ASIN;
                        ase.SalesVenueID = OmnimarkAmazon.Library.SalesVenueIDs.Buy_Dot_Com;
                        ase.TimeStamp = DateTime.Now;

                        db.ASINsSuccessfullyExporteds.Add(ase);

                        cnt++;
                    }
                }

                sr.Close();
                db.SaveChanges();

                RtnMsg += " Successfully processed " + cnt.ToString() + " record(s) from " + hpf.FileName + "<br />";
            }

            ViewBag.Message = (RtnMsg == "" ? "No file(s) uploaded." : RtnMsg);

            return Index();


        }

        public ActionResult Create()
        {
            OmnimarkAmazon.Library.RecreateKnownASINsForExportView(db);
            InitViewBag(ActionType.Create, "Export Spec");
            LoadSalesVenues();
            return View("ExportSpec", new ExportSpec());
        }

        [HttpPost]
        public ActionResult Create(ExportSpec Rec)
        {
            InitViewBag(ActionType.Create, "Export Spec");

            if (ModelState.IsValid)
            {
                Rec.ID = Guid.NewGuid();
                Rec.TimeStamp = DateTime.Now;

                db.ExportSpecs.Add(Rec);

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                LoadSalesVenues();
                return View("ExportSpec", Rec);
            }
        }

        public ActionResult Edit(Guid id)
        {
            OmnimarkAmazon.Library.RecreateKnownASINsForExportView(db);
            InitViewBag(ActionType.Edit, "Export Spec");
            LoadSalesVenues();
            return View("ExportSpec", db.ExportSpecs.Single(e => e.ID == id));
        }

        [HttpPost]
        public ActionResult Edit(Guid id, ExportSpec Rec)
        {
            InitViewBag(ActionType.Edit, "Export Spec");

            if (ModelState.IsValid)
            {
                ExportSpec es = db.ExportSpecs.Single(e => e.ID == id);
                es.UpdateTimeStamp = DateTime.Now;

                UpdateModel(es);

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                LoadSalesVenues();
                return View("ExportSpec", Rec);
            }

        }

        public ActionResult SaveExport(Guid id)
        {
            ExportSpec spec = null;

            string data = Startbutton.Library.StringToAscii(GetExportString(id, false, ref spec));

            string FileExtension = spec.FileExtension == null ? "csv" : spec.FileExtension;

            System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition();

            cd.FileName = spec.Name.Replace(' ', '_') + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "." + FileExtension;
            cd.Inline = false;

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return Content(data, "text/" + FileExtension + "; charset=utf-8");
        }

        public ActionResult GetSQL(Guid id)
        {
            ExportSpec spec = null;
            Dictionary<string, string> ColumnNames = new Dictionary<string, string>();
            string SQL = OmnimarkAmazon.Library.GetGetASINsWithAttributesSQLFromExportSpec(db, id, ref spec, ref ColumnNames);

            return Content(SQL);
        }

        public ActionResult ViewExport(Guid id)
        {
            ExportSpec spec = null;

            ViewBag.DataString = Startbutton.Library.StringToAscii(GetExportString(id, false, ref spec));

            return View(spec);
        }

        public ActionResult ViewData(Guid id)
        {

            List<string> FieldList = new List<string>();
            ExportSpec spec = null;
            Dictionary<string, string> ColumnNames = new Dictionary<string, string>();

            List<Dictionary<string, object>> data = OmnimarkAmazon.Library.GetASINsWithAttributesFromExportSpec(db, id, ref spec, ref FieldList, ref ColumnNames);

            StringBuilder TableHTML = new StringBuilder();

            TableHTML.Append("<table id='Export'><tr>");

            foreach (string f in FieldList)
            {
                TableHTML.Append("<th>");
                TableHTML.Append(ColumnNames[f]);
                TableHTML.Append("</th>");
            }

            TableHTML.Append("</tr>");

            foreach (Dictionary<string, object> row in data)
            {
                TableHTML.Append("<tr>");

                foreach (string f in FieldList)
                {
                    TableHTML.Append("<td valign='top' nowrap='nowrap'>");
                    TableHTML.Append(System.Net.WebUtility.HtmlEncode(row[f].ToString()));
                    TableHTML.Append("</td>");
                }

                TableHTML.Append("</tr>");

            }

            ViewBag.DataTable = TableHTML.ToString();

            return View(spec);
        }

        string GetExportString(Guid id, bool HtmlEncode, ref ExportSpec spec)
        {

            List<string> FieldList = new List<string>();
            Dictionary<string, string> ColumnNames = new Dictionary<string, string>();

            List<Dictionary<string, object>> data = OmnimarkAmazon.Library.GetASINsWithAttributesFromExportSpec(db, id, ref spec, ref FieldList, ref ColumnNames);

            StringBuilder ExportData = new StringBuilder();

            if (spec.FileHeaderText != null)
                ExportData.Append(Startbutton.Library.FromCString(spec.FileHeaderText.Replace("\n", "\\n").Replace("\r", "\\r")));

            string Delimeter = Startbutton.Library.FromCString(spec.FieldDelimiter);

            bool FirstField = true;
            foreach (string f in FieldList)
            {
                if (FirstField)
                    FirstField = false;
                else
                    ExportData.Append(Delimeter);

                ExportData.Append(ColumnNames[f]);
            }

            ExportData.Append('\n');

            foreach (Dictionary<string, object> row in data)
            {

                FirstField = true;

                foreach (string f in FieldList)
                {
                    string dat = row[f].ToString();

                    bool Numeric = Startbutton.Library.IsNumericType(row[f].GetType());

                    if (FirstField)
                        FirstField = false;
                    else
                        ExportData.Append(Delimeter);

                    if (!Numeric && spec.EncloseInQuotes)
                    {
                        ExportData.Append('"');
                        dat = dat.Replace("\"", "\"\"");
                    }

                    dat = dat.Replace("\n", "\\n").Replace(Delimeter, "");

                    if (HtmlEncode)
                        ExportData.Append(System.Net.WebUtility.HtmlEncode(dat));
                    else
                        ExportData.Append(dat);

                    if (!Numeric && spec.EncloseInQuotes)
                        ExportData.Append('"');

                }

                ExportData.Append('\n');

            }

            return ExportData.ToString();
        }

        List<string> FieldList
        {
            get
            {
                HttpContext.Application.Lock();
                HttpContext.Application["lock"] = "ok";

                bool doit = false; 

                #region only run once per minute max
                if (HttpContext.Application["ExportFieldList"] == null)
                    doit = true;
                else
                    if (((DateTime)HttpContext.Application["ExportFieldListTimeStamp"]).AddMinutes(1) < DateTime.Now)
                        doit = true;
                #endregion

                if (doit)
                {
                    #region Build Field List

                    OmnimarkAmazon.Library.RecreateKnownASINsForExportView(db);

                    string SQL = OmnimarkAmazon.Library.GetGetASINsWithAttributesSQL(db, null, null, null, 0);

                    List<string> rtn = Startbutton.Library.GetQueryColumns(SQL).Where(r => !r.StartsWith("_ignore_")).ToList();

                    rtn.Sort((a, b) => String.Compare(a, b));

                    HttpContext.Application["ExportFieldList"] = rtn;
                    HttpContext.Application["ExportFieldListTimeStamp"] = DateTime.Now;


                    #endregion
                }

                HttpContext.Application.UnLock();

                return (List<string>)HttpContext.Application["ExportFieldList"];

            }

        }

        public ActionResult GetFieldList()
        {
            return Json(FieldList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetExtraFieldList(string SelectClauseExtension, string FieldList)
        {

            List<string> ExtraFields = new List<string>();
            string ErrMsg = null;

            string SQL = OmnimarkAmazon.Library.GetGetASINsWithAttributesSQL(db, null, SelectClauseExtension, null, 0);
            
            List<string> rtn = null;
            
            try
            {
                rtn = Startbutton.Library.GetQueryColumns(SQL).Where(r => !r.StartsWith("_ignore_")).ToList();
            }
            catch (Exception Ex)
            {
                ErrMsg = Ex.Message;
            }

            if (ErrMsg == null)
            {
                #region any field that is returned in the row that was not part of the field list is an "extra"
                foreach(string c in rtn)
                    if (!this.FieldList.Contains(c))
                        if (!c.StartsWith("_ignore_"))
                            ExtraFields.Add(c);
                #endregion

            }

            return Json(new { Error = ErrMsg, Fields = ExtraFields }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Delete(Guid id)
        {
            InitViewBag(ActionType.Delete, "Export Spec");

            return View(db.ExportSpecs.Single(es => es.ID == id));

        }

        [HttpPost]
        public ActionResult Delete(Guid id, object stuff)
        {
            db.ExportSpecs.Remove(db.ExportSpecs.Single(es => es.ID == id));
            db.SaveChanges();

            return RedirectToAction("Index");

        }

        //public ActionResult Copy(Guid id)
        //{
        //    ExportSpec CopyFrom = db.ExportSpecs.Single(es => es.ID == id);
        //    ExportSpec CopyTo = new ExportSpec();

        //    Startbutton.Library.SetMatchingMembers(CopyTo, CopyFrom);

        //    CopyTo.ID = Guid.NewGuid();
        //    CopyTo.EntityKey = null;
        //    CopyTo.TimeStamp = DateTime.Now;
        //    CopyTo.UpdateTimeStamp = null;
        //    CopyTo.Name = "Copy of " + CopyTo.Name;

        //    db.ExportSpecs.Add(CopyTo);

        //    db.SaveChanges();

        //    return RedirectToAction("Index");
        //}

    }

}
