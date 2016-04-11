using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace OmnimarkAmazon.Models
{
    public class ProductCategoryListing
    {
        public Guid ID;
        public string Name;
        public bool Value;

        public ProductCategoryListing(Guid ID, string Name, bool Value)
        {
            this.ID = ID;
            this.Name = Name;
            this.Value = Value;
        }

        public ProductCategoryListing(string Data)
        {
            string[] a = Data.Split(',');

            this.ID = Guid.Parse(a[0]);
            this.Name = a[1];
            this.Value = a[2] == "0" ? false : true;
        }

        public static List<ProductCategoryListing> GetList(string Data, NameValueCollection Form = null)
        {
            List<ProductCategoryListing> rtn = new List<ProductCategoryListing>();

            string[] a = Data.Split('|');

            for (int x = 0; x < a.Length; x++)
                rtn.Add(new ProductCategoryListing(a[x]));

            if (Form != null)
                SetValuesFromForm(rtn, Form);

            return rtn;
        }

        public static List<ProductCategoryListing> GetList(Entities db, NameValueCollection Form = null)
        {
            List<ProductCategoryListing> rtn = new List<ProductCategoryListing>();

            foreach(ProductCategory pc in db.ProductCategories.OrderBy(pcx => pcx.DisplaySeq))
                rtn.Add(new ProductCategoryListing(pc.ID, pc.Name, false));

            if (Form != null)
                SetValuesFromForm(rtn, Form);

            return rtn;
        }

        static void SetValuesFromForm(List<ProductCategoryListing> List, NameValueCollection Form)
        {
            foreach (ProductCategoryListing pcl in List)
            {
                if (Form["Category_" + pcl.ID.ToString()] != null)
                    if (Form["Category_" + pcl.ID.ToString()] != "")
                        pcl.Value = true;
            }
        }

        public static void SaveFormValuesToDB(Entities db, Product Product, NameValueCollection Form)
        {
            if (Product.Categories == null)
                Product.Categories = new List<ProductCategory>();
            else
                Product.Categories.Clear();

            foreach (string key in Form.Keys)
            {
                if (key.StartsWith("Category_"))
                {
                    if (Form[key] != null)
                        if (Form[key] != "")
                        {
                            Guid pcid = Guid.Parse(key.Substring(9));

                            Product.Categories.Add(db.ProductCategories.Single(pc => pc.ID == pcid));
                        }
                }
            }

        }

    }

}
