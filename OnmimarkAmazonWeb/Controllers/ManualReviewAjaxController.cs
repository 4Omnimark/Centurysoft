using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;

namespace OmnimarkAmazonWeb.Controllers
{
    public class ManualReviewAjaxController : ApiController
    {
        // GET api/ajax
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/ajax/5
        public string GetSetValue(string table, string asin, string state)
        {
            String[] tables = { "tbl_baby", "tbl_beauty", "tbl_homeandkitchen", "tbl_jewellery", "tbl_sports", "tbl_toys", "tbl_watches" };
            if (tables.Contains(table))
            {
                while (true)
                {
                    try
                    {
                        string sql = "";
                        if (state.Equals("1"))
                        {
                            sql = "update [UKOmnimarkNew].[dbo].[" + table + "] set Reviewed=1, UK_Prohibited=0 where ASIN='" + asin + "'";
                        }
                        else
                        {
                            sql = "update [UKOmnimarkNew].[dbo].[" + table + "] set Reviewed=1, UK_Prohibited=1 where ASIN='" + asin + "'";
                        }
                        string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["UKMain"].ConnectionString;
                        db_update(sql, connStr);
                    }
                    catch (SqlException e)
                    {
                        if (e.Number != 1205 && e.Number != -2)
                            throw;
                        continue;
                    }
                    break;
                }
                return "true";
            }
            return "false";
        }

        private static void db_update(string queryString, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(
                       connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        // POST api/ajax
        public void Post([FromBody]string value)
        {
        }

        // PUT api/ajax/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/ajax/5
        public void Delete(int id)
        {
        }
    }
}
