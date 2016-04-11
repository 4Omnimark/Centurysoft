using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarketplaceWebService;
using MarketplaceWebService.Model;
using OmnimarkAmazon.Models;
using System.IO;
using System.Threading;

namespace OmnimarkAmazon
{
    public static partial class Library
    {
        public enum FeedType
        {
            _POST_PRODUCT_DATA_,
            _POST_PRODUCT_RELATIONSHIP_DATA_,
            _POST_ITEM_DATA_,
            _POST_PRODUCT_OVERRIDES_DATA_,
            _POST_PRODUCT_IMAGE_DATA_,
            _POST_PRODUCT_PRICING_DATA_,
            _POST_INVENTORY_AVAILABILITY_DATA_,
            _POST_ORDER_ACKNOWLEDGEMENT_DATA_,
            _POST_ORDER_FULFILLMENT_DATA_,
            _POST_FULFILLMENT_ORDER_REQUEST_DATA_,
            _POST_FULFILLMENT_ORDER_CANCELLATION_REQUEST_DATA_,
            _POST_PAYMENT_ADJUSTMENT_DATA_,
            _POST_INVOICE_CONFIRMATION_DATA_
        }

        public class AmazonFeedMessageList : Dictionary<int, Tuple<Nullable<Amazon.XML.AmazonEnvelopeMessageOperationType>, object>>
        {

            public void Add(int MessageID, Nullable<Amazon.XML.AmazonEnvelopeMessageOperationType> OperationType, object ItemData)
            {
                base.Add(MessageID, new Tuple<Nullable<Amazon.XML.AmazonEnvelopeMessageOperationType>, object>(OperationType, ItemData));
            }

            public void Add(int MessageID, object ItemData)
            {
                base.Add(MessageID, new Tuple<Nullable<Amazon.XML.AmazonEnvelopeMessageOperationType>, object>(null, ItemData));
            }

            public void AddUpdate(int MessageID, object ItemData)
            {
                base.Add(MessageID, new Tuple<Nullable<Amazon.XML.AmazonEnvelopeMessageOperationType>, object>(Amazon.XML.AmazonEnvelopeMessageOperationType.Update, ItemData));
            }

            public void AddDelete(int MessageID, object ItemData)
            {
                base.Add(MessageID, new Tuple<Nullable<Amazon.XML.AmazonEnvelopeMessageOperationType>, object>(Amazon.XML.AmazonEnvelopeMessageOperationType.Delete, ItemData));
            }

            public void AddPartialUpdate(int MessageID, object ItemData)
            {
                base.Add(MessageID, new Tuple<Nullable<Amazon.XML.AmazonEnvelopeMessageOperationType>, object>(Amazon.XML.AmazonEnvelopeMessageOperationType.PartialUpdate, ItemData));
            }

        }

        public static SubmitFeedResult SubmitPriceChangeFeed(Entities db, int AmazonAccountShortID, Dictionary<int, object> MessageItems)
        {
            return SubmitFeed(db, AmazonAccountShortID, Library.FeedType._POST_PRODUCT_PRICING_DATA_, Amazon.XML.AmazonEnvelopeMessageType.Price, MessageItems);
        }

        public static SubmitFeedResult SubmitPriceChangeFeed(Entities db, Guid AmazonAccountID, Dictionary<int, object> MessageItems)
        {
            return SubmitFeed(db, AmazonAccountID, Library.FeedType._POST_PRODUCT_PRICING_DATA_, Amazon.XML.AmazonEnvelopeMessageType.Price, MessageItems);
        }

        public static SubmitFeedResult SubmitPriceChangeFeed(AmazonAccount AmazonAccount, Dictionary<int, object> MessageItems)
        {
            return SubmitFeed(AmazonAccount, Library.FeedType._POST_PRODUCT_PRICING_DATA_, Amazon.XML.AmazonEnvelopeMessageType.Price, MessageItems);
        }

        public static SubmitFeedResult SubmitFeed(Entities db, int AmazonAccountShortID, FeedType feedType, Amazon.XML.AmazonEnvelopeMessageType MessageType, Dictionary<int, object> MessageItems)
        {
            return SubmitFeed(db.AmazonAccounts.Single(aa => aa.ShortID == AmazonAccountShortID), feedType, MessageType, MessageItems);
        }

        public static SubmitFeedResult SubmitFeed(Entities db, Guid AmazonAccountID, FeedType feedType, Amazon.XML.AmazonEnvelopeMessageType MessageType, Dictionary<int, object> MessageItems)
        {
            return SubmitFeed(db.AmazonAccounts.Single(aa => aa.ID == AmazonAccountID), feedType, MessageType, MessageItems);
        }

        public static SubmitFeedResult SubmitFeed(AmazonAccount AmazonAccount, FeedType feedType, Amazon.XML.AmazonEnvelopeMessageType MessageType, Dictionary<int, object> MessageItems)
        {
            AmazonFeedMessageList NewMessageItems = new AmazonFeedMessageList();

            foreach (var i in MessageItems)
                NewMessageItems.Add(i.Key, null, i.Value);

            return SubmitFeed(AmazonAccount, feedType, MessageType, NewMessageItems);
        }

        public static SubmitFeedResult SubmitFeed(AmazonAccount AmazonAccount, FeedType feedType, Amazon.XML.AmazonEnvelopeMessageType MessageType, AmazonFeedMessageList MessageItems, List<OmnimarkAmazon.Library.Throttler> Throttlers = null, Action<bool, string> Log = null)
        {
            var service = GetAmazonService<MarketplaceWebServiceClient>(AmazonAccount);

            SubmitFeedRequest sfrequest = new SubmitFeedRequest();
            sfrequest.Merchant = AmazonAccount.MerchantID;

            Amazon.XML.Header h = new Amazon.XML.Header();
            h.MerchantIdentifier = AmazonAccount.MerchantID;
            h.DocumentVersion = "1.01";

            Amazon.XML.AmazonEnvelope e = new Amazon.XML.AmazonEnvelope();
            e.Header = h;

            List<Amazon.XML.AmazonEnvelopeMessage> msgs = new List<Amazon.XML.AmazonEnvelopeMessage>();

            foreach (int mi in MessageItems.Keys)
            {

                Amazon.XML.AmazonEnvelopeMessage em = new Amazon.XML.AmazonEnvelopeMessage();
                em.Item = MessageItems[mi].Item2;
                em.MessageID = mi.ToString();

                if (MessageItems[mi].Item1 != null)
                    em.OperationType = (Amazon.XML.AmazonEnvelopeMessageOperationType)MessageItems[mi].Item1;
                
                msgs.Add(em);
            }
            
            e.Message = msgs.ToArray();
            e.MessageType = MessageType;

            sfrequest.FeedContent = Startbutton.Library.XmlSerializeToStream<Amazon.XML.AmazonEnvelope>(e);

            sfrequest.FeedContent.Position = 0;
            sfrequest.FeedType = feedType.ToString();

            string xml = Startbutton.Library.StreamToString(sfrequest.FeedContent, 0);

            sfrequest.ContentMD5 = System.Convert.ToBase64String(Startbutton.Library.MD5(xml));

            SubmitFeedResponse response = null;

            if (Throttlers == null)
                Throttlers = new OmnimarkAmazon.Library.Throttler[] { new OmnimarkAmazon.Library.Throttler(3000) }.ToList();

            bool ThrottlerSlowed = false;
            int WaitTime = 1000;

            while (response == null)
            {

                try
                {
                    response = service.SubmitFeed(sfrequest);
                }
                catch (Exception Ex)
                {
                    if (Ex.Message.StartsWith("Request is throttled"))
                    {
                        if (Log != null)
                            Log(false, "Throttled...");

                        foreach (Throttler Throttler in Throttlers)
                            if (Throttler != null)
                            {
                                Throttler.LastRequest = DateTime.Now;

                                if (!ThrottlerSlowed)
                                {
                                    Throttler.MillisecondsBetweenRequests = (int)((double)Throttler.MillisecondsBetweenRequests * 1.1);
                                    Log(false, "New Wait: " + Throttler.MillisecondsBetweenRequests.ToString() + ". ");
                                }

                                ThrottlerSlowed = true;

                                Thread.Sleep(WaitTime);

                                WaitTime *= (int)1.1;
                            }

                    }

                }

            }

            return response.SubmitFeedResult;

        }

        public static List<FeedSubmissionInfo> GetFeedSubmissionList(List<Throttler> Throttlers, AmazonAccount AmazonAccount, IEnumerable<string> FeedSubmissionIDs, Action<bool, string> Log)
        {

            var service = GetAmazonService<MarketplaceWebServiceClient>(AmazonAccount);
            
            if (Log != null)
                Log(false, "Retreiving Feed Submissions for " + AmazonAccount.Name + "... ");

            GetFeedSubmissionListRequest gfslrequest = new GetFeedSubmissionListRequest();
            gfslrequest.Merchant = AmazonAccount.MerchantID;

            if (FeedSubmissionIDs != null)
            {
                IdList il = new IdList();

                foreach (string SubID in FeedSubmissionIDs)
                    il.Id.Add(SubID);

                gfslrequest.FeedSubmissionIdList = il;
            }

            if (Throttlers != null)
                foreach (Throttler Throttler in Throttlers)
                    if (Throttler != null)
                        Throttler.DoWait(Log);

            GetFeedSubmissionListResponse gfslresponse = null;
            
            try
            {
                gfslresponse = service.GetFeedSubmissionList(gfslrequest);
            }
            catch (Exception Ex)
            {
                Log(true, "ERROR: " + Ex.Message);
                return new List<FeedSubmissionInfo>();
            }

            GetFeedSubmissionListResult GetFeedSubmissionListResult = gfslresponse.GetFeedSubmissionListResult;

            List<FeedSubmissionInfo> rtn = new List<FeedSubmissionInfo>();

            bool ResultIsSetReportInfo = GetFeedSubmissionListResult.IsSetFeedSubmissionInfo();
            bool ResultIsSetNextReportInfo = GetFeedSubmissionListResult.IsSetNextToken();
            List<FeedSubmissionInfo> FeedSubmissionList = GetFeedSubmissionListResult.FeedSubmissionInfo;
            string NextToken = GetFeedSubmissionListResult.NextToken;

            while (true)
            {
                if (ResultIsSetReportInfo)
                {
                    if (Log != null)
                        Log(true, "Got " + FeedSubmissionList.Count.ToString());

                    foreach (FeedSubmissionInfo ri in FeedSubmissionList)
                        rtn.Add(ri);
                }
                else
                    if (Log != null)
                        Log(true, "Got none");

                if (!ResultIsSetNextReportInfo)
                    break;

                if (NextToken == "")
                    break;

                GetFeedSubmissionListByNextTokenRequest request2 = new GetFeedSubmissionListByNextTokenRequest().WithMerchant(AmazonAccount.MerchantID);

                request2.NextToken = NextToken;

                if (Log != null)
                    Log(false, "Retreiving More Feed Submissions for " + AmazonAccount.Name + "... ");

                foreach (Throttler Throttler in Throttlers)
                    if (Throttler != null)
                        Throttler.DoWait(Log);

                GetFeedSubmissionListByNextTokenResponse response2 = service.GetFeedSubmissionListByNextToken(request2);
                GetFeedSubmissionListByNextTokenResult GetFeedSubmissionListResult2 = response2.GetFeedSubmissionListByNextTokenResult;

                ResultIsSetReportInfo = GetFeedSubmissionListResult2.IsSetFeedSubmissionInfo();
                ResultIsSetNextReportInfo = GetFeedSubmissionListResult2.IsSetNextToken();
                FeedSubmissionList = GetFeedSubmissionListResult2.FeedSubmissionInfo;
                NextToken = GetFeedSubmissionListResult2.NextToken;

            }

            return rtn;
        
        }

        public static string GetFeedSubmissionResult(List<Throttler> Throttlers, AmazonAccount AmazonAccount, string FeedSubmissionID, Action<bool, string> Log)
        {
            var service = GetAmazonService<MarketplaceWebServiceClient>(AmazonAccount);

            if (Log != null)
                Log(false, "Retreiving Feed Submissions Result " + FeedSubmissionID + " for " + AmazonAccount.Name + "... ");

            GetFeedSubmissionResultRequest request = new GetFeedSubmissionResultRequest();
            request.Merchant = AmazonAccount.MerchantID;
            request.FeedSubmissionId = FeedSubmissionID;

            foreach (Throttler Throttler in Throttlers)
                if (Throttler != null)
                    Throttler.DoWait(Log);

            var ms = new MemoryStream();

            request.FeedSubmissionResult = ms;

            GetFeedSubmissionResultResponse response = service.GetFeedSubmissionResult(request);

            GetFeedSubmissionResultResult result = response.GetFeedSubmissionResultResult;

            string rtn = Startbutton.Library.StreamToString(ms, 0);

            ms.Close();
            ms.Dispose();

            return rtn;

        }

    }
}
