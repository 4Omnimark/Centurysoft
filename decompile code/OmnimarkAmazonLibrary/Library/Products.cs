using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarketplaceWebServiceOrders;
using MarketplaceWebServiceOrders.Model;
using OmnimarkAmazon.Models;
using MarketplaceWebServiceProducts;
using MarketplaceWebServiceProducts.Model;

namespace OmnimarkAmazon
{
    public static partial class Library
    {
        public static List<GetMatchingProductResult> GetProduct(AmazonAccount AmazonAccount, List<string> ASINs)
        {
            MarketplaceWebServiceProductsConfig config = new MarketplaceWebServiceProductsConfig();
            config.ServiceURL = AmazonAccount.Country.ProductsServiceURL;

            MarketplaceWebServiceProducts.MarketplaceWebServiceProductsClient service = new MarketplaceWebServiceProductsClient(applicationName, applicationVersion, AmazonAccount.AccessKeyID, AmazonAccount.SecretAccessKey, config);

            GetMatchingProductRequest request = new GetMatchingProductRequest().WithSellerId(AmazonAccount.MerchantID);

            if (ASINs != null)
            {
                ASINListType ASINList = new ASINListType();

                foreach (string ASIN in ASINs)
                    ASINList.ASIN.Add(ASIN);

                request.ASINList = ASINList;
            }

            GetMatchingProductResponse response = service.GetMatchingProduct(request);

            List<string> rtn = new List<string>();

            return response.GetMatchingProductResult;

        }

    }
}
