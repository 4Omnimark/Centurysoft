using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace OmnimarkAmazon.Models
{
    public class ProductTagListing
    {
        public Guid ID;
        public string Name;
        public bool Value;

        public ProductTagListing(Guid ID, string Name, bool Value)
        {
            this.ID = ID;
            this.Name = Name;
            this.Value = Value;
        }

        public ProductTagListing(string Data)
        {
            string[] a = Data.Split(',');

            this.ID = Guid.Parse(a[0]);
            this.Name = a[1];
            this.Value = a[2] == "0" ? false : true;
        }

        public static List<ProductTagListing> GetList(string Data, NameValueCollection Form = null)
        {
            List<ProductTagListing> rtn = new List<ProductTagListing>();

            string[] a = Data.Split('|');

            for (int x = 0; x < a.Length; x++)
                rtn.Add(new ProductTagListing(a[x]));

            if (Form != null)
                SetValuesFromForm(rtn, Form);

            return rtn;
        }

        public static List<ProductTagListing> GetList(Entities db, NameValueCollection Form = null)
        {
            List<ProductTagListing> rtn = new List<ProductTagListing>();

            foreach(ProductTag pc in db.ProductTags.OrderBy(pcx => pcx.Name))
                rtn.Add(new ProductTagListing(pc.ID, pc.Name, false));

            if (Form != null)
                SetValuesFromForm(rtn, Form);

            return rtn;
        }

        static void SetValuesFromForm(List<ProductTagListing> List, NameValueCollection Form)
        {
            foreach (ProductTagListing pcl in List)
            {
                if (Form["Tag_" + pcl.ID.ToString()] != null)
                    if (Form["Tag_" + pcl.ID.ToString()] != "")
                        pcl.Value = true;
            }
        }

        public static void SaveFormValuesToDB(Entities db, Product Product, NameValueCollection Form)
        {
            if (Product.Tags == null)
                Product.Tags = new List<ProductTag>();
            else
                Product.Tags.Clear();

            foreach (string key in Form.Keys)
            {
                if (key.StartsWith("Tag_"))
                {
                    if (Form[key] != null)
                        if (Form[key] != "")
                        {
                            Guid pcid = Guid.Parse(key.Substring(4));

                            Product.Tags.Add(db.ProductTags.Single(pc => pc.ID == pcid));
                        }
                }
            }

        }

    }

}
