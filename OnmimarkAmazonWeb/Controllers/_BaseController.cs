using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OmnimarkAmazon.Models;

namespace OmnimarkAmazonWeb.Controllers
{
    //[AuthorizeIPAddress]
[Authorize]
    public class _BaseController : Startbutton.Web._BaseController
    {

      
        Entities _db;
        List<AmazonAccount> _amazonaccountcache;
   
        public Entities db
        {
            
            get
            {
                if (_db == null)
                    _db = new Entities();

                return _db;
            }
        }
        public List<AmazonAccount> AmazonAccountCache
        {
            get
            {
                if (_amazonaccountcache == null)
                    _amazonaccountcache = db.AmazonAccounts.ToList();

                return _amazonaccountcache;
            }
        }

        public AmazonAccount GetAmazonAccount(string store)
        {
            string Store = store.ToUpper();

            AmazonAccount rtn = AmazonAccountCache.Where(aa => aa.CharID == Store).FirstOrDefault();

            if (rtn == null)
                throw (new Exception("Unknown store!"));
            else
                return rtn;

        }
    }
}
