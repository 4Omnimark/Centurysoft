using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace OmnimarkAmazon.Models
{
    public class VendorListing
    {
        public Guid ID;
        public string Name;
        public bool Value;

        public VendorListing(Guid ID, string Name, bool Value)
        {
            this.ID = ID;
            this.Name = Name;
            this.Value = Value;
        }

        public VendorListing(string Data)
        {
            string[] a = Data.Split(',');

            this.ID = Guid.Parse(a[0]);
            this.Name = a[1];
            this.Value = a[2] == "0" ? false : true;
        }

        public static List<VendorListing> GetList(string Data, NameValueCollection Form = null)
        {
            List<VendorListing> rtn = new List<VendorListing>();

            string[] a = Data.Split('|');

            for (int x = 0; x < a.Length; x++)
                rtn.Add(new VendorListing(a[x]));

            if (Form != null)
                SetValuesFromForm(rtn, Form);

            return rtn;
        }

        public static List<VendorListing> GetList(Entities db, Nullable<Guid> ProductID, NameValueCollection Form = null)
        {
            List<VendorListing> rtn = new List<VendorListing>();

            if (ProductID == null)
                rtn = db.Vendors.OrderBy(v => v.Name).ToList().Select(v => new VendorListing(v.ID, v.Name, false)).ToList();
            else
                rtn = db.Vendors.ToList().GroupJoin(
                    db.Products.Single(p => p.ID == ProductID).Vendors,
                    p => p.ID,
                    c => c.ID,
                    (p, c) => new { Vendor = p, ProductVendors = c })
                    .OrderBy(x => x.Vendor.Name)
                    .SelectMany(c => c.ProductVendors.DefaultIfEmpty(),
                        (p, c) => new { Vendor = p.Vendor, ProductVendor = c })
                    .Select(x => new VendorListing(x.Vendor.ID, x.Vendor.Name, !(x.ProductVendor == null))).ToList();

            if (Form != null)
                SetValuesFromForm(rtn, Form);

            return rtn;
        }

        static void SetValuesFromForm(List<VendorListing> List, NameValueCollection Form)
        {
            foreach (VendorListing vl in List)
            {
                if (Form["Vendor_" + vl.ID.ToString()] != null)
                    if (Form["Vendor_" + vl.ID.ToString()] != "")
                        vl.Value = true;
            }
        }

        public static void SaveFormValuesToDB(Entities db, Product Product, NameValueCollection Form)
        {
            Product.Vendors.Clear();

            foreach (string key in Form.Keys)
            {
                if (key.StartsWith("Vendor_"))
                {
                    if (Form[key] != null)
                        if (Form[key] != "")
                        {
                            Guid vid = Guid.Parse(key.Substring(7));

                            Product.Vendors.Add(db.Vendors.Single(pc => pc.ID == vid));
                        }
                }
            }

        }

    }

}
