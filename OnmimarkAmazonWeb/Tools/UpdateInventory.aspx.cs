using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OmnimarkAmazon;
using OmnimarkAmazon.Models;

namespace OmnimarkAmazonWeb.Tools
{
    public partial class UpdateInventory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write("<script src='/Scripts/jquery-1.9.1.min.js' type='text/javascript'></script>");
            Server.ScriptTimeout = 100000;
            Response.Flush();

            Entities db = new Entities();

            foreach (AmazonAccount Account in db.AmazonAccounts.Where(aa => aa.Enabled || aa.AccessKeyID == "AKIAJJE7HWBEDEJ7FXLQ").OrderBy(aa => aa.LastInventoryUpdate).ToList())
            {
                bool isError = false;
                try
                {

              
                //AmazonAccount Account = db.AmazonAccounts.First();
                IEnumerable<InventorySupplySummary> inventory = Library.GetInventory (null, Account, Log);
                db.SaveChanges();
                db = new Entities();

                Log(true, "Updating KnownASINs");
                int ChangeCount = Library.AddASINsToDBFromInventory(db, Account, inventory, Log);
                Log(true, ChangeCount.ToString() + " items updated/added.");

                db.SaveChanges();
                db = new Entities();

                Log(true, "Updating Inventory");
                ChangeCount = Library.UpdateInventory(db, Account, inventory, Log);
                Log(true, ChangeCount.ToString() + " items updated/added.");

                db.SaveChanges();
                }
                catch 
                {

                    isError = true;
                }
                if (isError) continue;

            }

                Response.Write(@"
                    <script type='text/javascript'>
                        $('#btnButton', parent.document)[0].disabled=false;
                        $('#btnButton', parent.document)[0].value='Done! Continue...';
                    </script>
                ");
                Response.Flush();
        }

        void Log(bool LineBreak, string Line)
        {
            Response.Write(Line);

            if (LineBreak)
                Response.Write("<br />");

            Response.Write("<script type='text/javascript'>window.scrollTo(0, document.body.scrollHeight);</script>");

             Response.Flush();
        }
    }
}