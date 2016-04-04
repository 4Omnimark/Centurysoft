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
    public partial class DeductInventoryFBA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write("<script src='/Scripts/jquery-1.9.1.min.js' type='text/javascript'></script>");
            Server.ScriptTimeout = 100000;
            Response.Flush();

            Entities db = new Entities();

            if (Request.QueryString["ShipmentID"] == null)
            {
                Library.GetAllFBAShipments(db, Log);
                Library.SyncInboundFBAShipments(db, Log);
            }
            else
            {
                Guid aaid = Guid.Parse(Request.QueryString["ShipmentAccount"]);
                AmazonAccount aa = db.AmazonAccounts.Single(aax => aax.ID == aaid);
                var shp = Library.GetInboundShipments(new Library.Throttler[] { new Library.Throttler(2000) }.ToList(), aa, null, null, new string[] { Request.QueryString["ShipmentID"] }.ToList(), Log);
                Library.DoSyncInboundFBAShipments(db, aa, new Library.Throttler(2000), shp, true, Log);
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