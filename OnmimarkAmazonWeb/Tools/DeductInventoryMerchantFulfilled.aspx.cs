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
    public partial class DeductInventoryMerchantFulfilled : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write("<script src='/Scripts/jquery-1.9.1.min.js' type='text/javascript'></script>");
            Server.ScriptTimeout = 100000;
            Response.Flush();

            Entities db = new Entities();

            Library.ReconcileInventoryForAmazonOrdersShippedFromOrlando(db, Log);

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