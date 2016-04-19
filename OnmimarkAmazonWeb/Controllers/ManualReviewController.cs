using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using UkListing;
using PagedList;
namespace OmnimarkAmazonWeb.Controllers
{
    [Authorize]
    public class ManualReviewController : Controller
    {
        //
        // GET: /ManualReview/

        public ActionResult Index()
        {
            ViewBag.categories = getCategories("");
            return View("Menu");
        }

        public ActionResult Tables(string table, int? page)
        {
            UKOmnimarkEntities entity= new UKOmnimarkEntities();
            ViewBag.categories = getCategories(table);
            int pageNumber = (page ?? 1);
            //var products = from p in entities.tbl_Baby select p;
            //products = products.OrderByDescending(p => p.TimeStamp);

            if (table == null)
            {
                Response.Redirect("/ManualReview");
            }

            string tableName = table;

            int pageNum = page ?? 1;

            List<Dictionary<String, String>> products = getTableData(tableName, pageNum);

            ViewBag.page = pageNum;
            ViewBag.table = tableName;
            ViewBag.prevPage = pageNum - 1;
            ViewBag.nextPage = pageNum + 1;
            Int64 total = getRowCount(table);
            ViewBag.pageCnt = total / 25;
            if (total % 25 > 0)
                ViewBag.pageCnt = total / 25 + 1;

            ViewBag.title = getTitle(table);

            return View("Index", products);

        }

        private List<Dictionary<String, String>> getTableData(String table, int page)
        {
            List<Dictionary<String, String>> result = new List<Dictionary<String, String>>();

            int skp = (page - 1) * 25;

            string sql = "select MediumImageUrl, ASIN, HeightUnits, WidthUnits, LengthUnits, TimeStamp, Status, UK_Prohibited, Reviewed, Title from [UKOmnimarkNew].[dbo].[" + table + "]";
            sql += " where TimeStamp < DateAdd(d, -1, GetDate()) and WeightUnits < 251 and (UK_Prohibited is null or UK_Prohibited <> 1) and HeightUnits < 3000 and WidthUnits < 3000 and LengthUnits < 3000";
            sql += " order by Status desc, Reviewed";
            sql += " offset " + skp.ToString() + " rows fetch next 25 rows only";

            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["UKMain"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                while (true)
                {
                    try
                    {
                        SqlCommand command = new SqlCommand(sql, conn);
                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Dictionary<String, String> row = new Dictionary<String, String>();
                            row["ImageUrl"] = reader[0].ToString();
                            row["ASIN"] = reader[1].ToString();
                            row["HeightUnits"] = reader[2].ToString();
                            row["WidthUnits"] = reader[3].ToString();
                            row["LengthUnits"] = reader[4].ToString();
                            row["TimeStamp"] = reader[5].ToString();
                            row["Status"] = reader[6].ToString();
                            row["UK_Prohibited"] = reader[7].ToString();
                            row["Reviewed"] = reader[8].ToString();
                            row["Title"] = reader[9].ToString();
                            result.Add(row);
                        }
                        return result;
                    }
                    catch (SqlException e)
                    {
                        if (e.Number != 1205 && e.Number != -2)
                            throw;
                        conn.Close();
                        continue;
                    }

                }
            }
        }

        private Int64 getRowCount(String table)
        {
            List<Dictionary<String, String>> result = new List<Dictionary<String, String>>();



            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["UKMain"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                while (true)
                {
                    try
                    {
                        string countSql = "select count(*) from [UKOmnimarkNew].[dbo].[" + table + "]  where TimeStamp < DateAdd(d, -1, GetDate()) and WeightUnits < 251 and (UK_Prohibited is null or UK_Prohibited <> 1) and HeightUnits < 3000 and WidthUnits < 3000 and LengthUnits < 3000";
                        SqlCommand cntCmd = new SqlCommand(countSql, conn);
                        conn.Open();
                        Int32 cnt = (Int32)cntCmd.ExecuteScalar();
                        return cnt;
                    }
                    catch (SqlException e)
                    {
                        if (e.Number != 1205)
                            throw;
                        continue;
                    }

                }


            }
        }

        private List<SelectListItem> getCategories(String category)
        {
            String[] tables = { "", "tbl_baby", "tbl_beauty", "tbl_homeandkitchen", "tbl_jewellery", "tbl_sports", "tbl_toys", "tbl_watches" };
            String[] categories = { "", "Baby", "Beauty", "Home and Kitchen", "Jewellery", "Sports", "Toys", "Watches" };

            List<SelectListItem> items = new List<SelectListItem>();
            for (int i = 0; i < tables.Count(); i++)
            {
                SelectListItem it = new SelectListItem();
                if (category.Equals(tables[i]))
                    it.Selected = true;
                it.Text = categories[i];
                it.Value = tables[i];
                items.Add(it);
            }
            return items;
        }

        private String getTitle(String category)
        {
            String[] tables = { "", "tbl_baby", "tbl_beauty", "tbl_homeandkitchen", "tbl_jewellery", "tbl_sports", "tbl_toys", "tbl_watches" };
            String[] categories = { "", "Baby", "Beauty", "Home and Kitchen", "Jewellery", "Sports", "Toys", "Watches" };
            for (int i = 0; i < tables.Length; i++)
            {
                if (tables[i].Equals(category))
                    return categories[i];
            }
            return "";
        }

    }
}
