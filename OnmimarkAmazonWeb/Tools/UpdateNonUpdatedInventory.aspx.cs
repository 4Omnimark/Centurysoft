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
    public partial class UpdateNonUpdatedInventory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write("<script src='/Scripts/jquery-1.9.1.min.js' type='text/javascript'></script>");
            Response.Flush();

            Entities db = new Entities();
           bool isError = false;

            foreach (AmazonAccount Account in db.AmazonAccounts.Where(aa => aa.Enabled || aa.AccessKeyID == "AKIAJJE7HWBEDEJ7FXLQ").ToList())
            {
                try
                {


                    Log(true, "Getting List of ASINs...");

                    List<string> ASINs = db.Database.SqlQuery<string>(@"
	                select distinct ASIN from AmazonInventory ai
	                join AmazonAccounts aa on aa.ID = ai.AmazonAccountID
	                where LastInventoryUpdateStart > AmazonStockTimeStamp 
	                and AmazonAccountID='" + Account.ID.ToString() + @"'
                ").ToList();

                    Log(true, "Getting List of SKUs...");

                    List<string> SKUs = db.Database.SqlQuery<string>(@"
                    select distinct SKU from AmazonInventorySKUs where ASIN in (
	                    select distinct ASIN from AmazonInventory ai
	                    join AmazonAccounts aa on aa.ID = ai.AmazonAccountID
	                    where LastInventoryUpdateStart > AmazonStockTimeStamp 
	                    and AmazonAccountID='" + Account.ID.ToString() + @"'
                    )  and AmazonAccountID='" + Account.ID.ToString() + @"'
                ").ToList();

                    Log(false, "Setting ASIN Qtys to 0");

                    foreach (string ASIN in ASINs)
                    {
                        var rec = db.AmazonInventories.Single(ai => ai.AmazonAccountID == Account.ID && ai.ASIN == ASIN);
                        rec.AmazonInStockQty = 0;
                        rec.AmazonStockQty = 0;
                        rec.AmazonStockTimeStamp = DateTime.Now;

                        Log(false, ".");
                    }

                    Log(true, "");

                    Log(false, "Setting SKU Qtys to 0");

                    var ToDelete = db.AmazonInventorySKUs.Where(ais => ASINs.Contains(ais.ASIN) && ais.AmazonAccountID == Account.ID).ToList();

                    foreach (AmazonInventorySKU ais in ToDelete)
                    {
                        ais.TotalQty = 0;
                        ais.InStockQty = 0;
                        Log(false, ".");
                    }

                    Log(true, "");
                    Log(true, "Saving...");

                    db.SaveChanges();

                    if (SKUs.Count > 0)
                    {
                        for (int x = 0; x < SKUs.Count; x += 50)
                        {

                            //AmazonAccount Account = db.AmazonAccounts.First();
                            IEnumerable<InventorySupplySummary> inventory = Library.GetInventory(null, Account, Log, SKUs.Skip(x).Take(50).ToList());
                            db.SaveChanges();
                            db = new Entities();

                            Log(true, "Updating KnownASINs");
                            int ChangeCount = Library.AddASINsToDBFromInventory(db, Account, inventory, Log);
                            Log(true, ChangeCount.ToString() + " items updated/added.");

                            db.SaveChanges();
                            db = new Entities();

                            Log(true, "Updating Inventory");
                            ChangeCount = Library.UpdateInventory(db, Account, inventory, Log, false);
                            Log(true, ChangeCount.ToString() + " items updated/added.");

                            db.SaveChanges();
                        }
                    }
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