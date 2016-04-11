using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OmnimarkAmazon.Models;

namespace OmnimarkAmazon
{
    namespace NewAmazonProduct
    {
        public class MessageBatch
        {
            public AmazonAccount Account;
            public OmnimarkAmazon.Library.AmazonFeedMessageList ProductMessages = new OmnimarkAmazon.Library.AmazonFeedMessageList();
            public OmnimarkAmazon.Library.AmazonFeedMessageList PricingMessages = new OmnimarkAmazon.Library.AmazonFeedMessageList();
            public OmnimarkAmazon.Library.AmazonFeedMessageList ImageMessages = new OmnimarkAmazon.Library.AmazonFeedMessageList();
            public OmnimarkAmazon.Library.AmazonFeedMessageList InventoryMessages = new OmnimarkAmazon.Library.AmazonFeedMessageList();
            public List<KeyValuePair<string, string>> Relationships = new List<KeyValuePair<string, string>>();
        }

        public class MessageBatches : List<MessageBatch>
        {
            MessageBatch GetBatch(AmazonAccount Account)
            {
                MessageBatch mb = this.Where(mbx => mbx.Account == Account).FirstOrDefault();

                if (mb == null)
                {
                    mb = new MessageBatch();
                    mb.Account = Account;
                    this.Add(mb);
                }

                return mb;

            }

            public void Add(AmazonAccount Account, NewAmazonProduct.ProductInfo pi)
            {
                MessageBatch mb = GetBatch(Account);
                mb.ProductMessages.AddUpdate(mb.ProductMessages.Count + 1, pi.Product);

                if (pi.Price != null)
                    mb.PricingMessages.AddUpdate(mb.PricingMessages.Count + 1, pi.Price);

                foreach(var im in pi.Images)
                    mb.ImageMessages.AddUpdate(mb.ImageMessages.Count + 1, im);

                if (pi.Inventory != null)
                    mb.InventoryMessages.AddUpdate(mb.InventoryMessages.Count + 1, pi.Inventory);

                if (pi.ParentSKU != null)
                    mb.Relationships.Add(new KeyValuePair<string,string>(pi.ParentSKU, pi.Product.SKU));

            }

            public void Submit(Action<bool, string> Log)
            {
                foreach (var mb in this)
                {
                    OmnimarkAmazon.Library.AmazonFeedMessageList RelationshipMessages = new Library.AmazonFeedMessageList();

                    foreach (string par in mb.Relationships.Select(rx => rx.Key).Distinct())
                    {
                        Amazon.XML.Relationship amzrel = new Amazon.XML.Relationship();
                        amzrel.ParentSKU = par;
                        amzrel.Relation = new Amazon.XML.RelationshipRelation[mb.Relationships.Where(rx => rx.Key == par).Count()];
                        int x = 0;

                        foreach (var rel in mb.Relationships.Where(rx => rx.Key == par))
                        {
                            amzrel.Relation[x] = new Amazon.XML.RelationshipRelation();
                            amzrel.Relation[x].SKU = rel.Value;
                            amzrel.Relation[x].Type = Amazon.XML.RelationshipRelationType.Variation;

                            x++;
                        }

                        RelationshipMessages.AddUpdate(RelationshipMessages.Count + 1, amzrel);
                    }

                    List<OmnimarkAmazon.Library.Throttler> Throttlers = new OmnimarkAmazon.Library.Throttler[] { new OmnimarkAmazon.Library.Throttler(3000) }.ToList();

                    MarketplaceWebService.Model.SubmitFeedResult r = Library.SubmitFeed(mb.Account, Library.FeedType._POST_PRODUCT_DATA_, Amazon.XML.AmazonEnvelopeMessageType.Product, mb.ProductMessages, Throttlers, Log);
                    Log(true, "PRODUCT FeedSubmissionId: " + r.FeedSubmissionInfo.FeedSubmissionId);

                    if (mb.PricingMessages.Count > 0)
                    {
                        r = Library.SubmitFeed(mb.Account, Library.FeedType._POST_PRODUCT_PRICING_DATA_, Amazon.XML.AmazonEnvelopeMessageType.Price, mb.PricingMessages, Throttlers, Log);
                        Log(true, "PRODUCT_PRICING FeedSubmissionId: " + r.FeedSubmissionInfo.FeedSubmissionId);
                    }

                    if (mb.ImageMessages.Count > 0)
                    {
                        r = Library.SubmitFeed(mb.Account, Library.FeedType._POST_PRODUCT_IMAGE_DATA_, Amazon.XML.AmazonEnvelopeMessageType.ProductImage, mb.ImageMessages, Throttlers, Log);
                        Log(true, "PRODUCT_IMAGE FeedSubmissionId: " + r.FeedSubmissionInfo.FeedSubmissionId);
                    }

                    if (mb.InventoryMessages.Count > 0)
                    {
                        r = Library.SubmitFeed(mb.Account, Library.FeedType._POST_INVENTORY_AVAILABILITY_DATA_, Amazon.XML.AmazonEnvelopeMessageType.Inventory, mb.InventoryMessages, Throttlers, Log);
                        Log(true, "INVENTORY_AVAILABILITY FeedSubmissionId: " + r.FeedSubmissionInfo.FeedSubmissionId);
                    }

                    if (RelationshipMessages.Count > 0)
                    {
                        r = Library.SubmitFeed(mb.Account, Library.FeedType._POST_PRODUCT_RELATIONSHIP_DATA_, Amazon.XML.AmazonEnvelopeMessageType.Relationship, RelationshipMessages, Throttlers, Log);
                        Log(true, "PRODUCT_RELATIONSHIP FeedSubmissionId: " + r.FeedSubmissionInfo.FeedSubmissionId);
                    }

                }
            }


        }

        public class ProductInfo
        {
            public Amazon.XML.Product Product;
            public Amazon.XML.Price Price;
            public List<Amazon.XML.ProductImage> Images = new List<Amazon.XML.ProductImage>();
            public Amazon.XML.Inventory Inventory;
            public string ParentSKU;
        }
    }

    public static partial class Library
    {

        public static string GetUPCFromSKU(Entities db, string SKU)
        {
            FakeUPC fu = db.FakeUPCs.Where(fux => fux.SKU == SKU).FirstOrDefault();

            if (fu == null)
            {
                fu = db.FakeUPCs.Where(fux => fux.SKU == null && fux.ProductComboCode == null).Take(1).First();
                fu.SKU = SKU;
                fu.TimeStamp = DateTime.Now;
                fu.ProductComboCode = "x";
                db.SaveChanges();
            }

            return fu.UPC;

        }
    }
}
