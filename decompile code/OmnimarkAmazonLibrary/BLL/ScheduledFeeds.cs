using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OmnimarkAmazon.Models;
using Startbutton.ExtensionMethods;
using System.Data.Objects;
using System.Reflection;
using System.Transactions;

namespace OmnimarkAmazon.BLL
{
    public class ProcessScheduledFeedsResult
    {
        public List<FeedSubmissionQueueRec> Queued;
        public List<FeedSubmissionQueueRec> Submitted;
        public List<FeedSubmissionQueueRec> Processing;
        public List<FeedSubmissionQueueRec> Completed;
    }

    public static class ScheduledFeeds
    {
        public static ProcessScheduledFeedsResult ProcessScheduledFeeds(Entities db, Action<bool, string> Log)
        {
            ProcessScheduledFeedsResult rtn = new ProcessScheduledFeedsResult();

            rtn.Queued = QueueScheduledFeeds(db, Log);
            rtn.Submitted = SubmitQueuedScheduledFeeds(db, Log);
            rtn.Processing = GetStatusOfSubmittedScheduledFeeds(db, Log);
            rtn.Completed = ProcessCompletedScheduledFeeds(db, Log);

            return rtn;
        }

        public static List<FeedSubmissionQueueRec> QueueScheduledFeeds(Entities db2, Action<bool, string> Log)
        {
            Log(true, "Queuing Scheduled Items...");

            List<FeedSubmissionQueueRec> rtn = new List<FeedSubmissionQueueRec>();

            using (TransactionScope scope = new TransactionScope())
            {

                using (Entities db = new Entities())
                {

                    db.Database.Connection.Open();

                    db.LockTable("FeedSubmissionSchedule");

                    int ServerDateTimeOffset = db.GetServerDateTimeOffset();
                    DateTime ServerNow = db.GetServerNow(ServerDateTimeOffset);

                    while (true)
                    {

                        var RecsToProcess = db.FeedSubmissionSchedule.Where(fss => fss.NextRun <= ServerNow && fss.Enabled && fss.Deleted == null);

                        bool RecsProcessed = false;

                        foreach (var rec in RecsToProcess)
                        {
                            FeedSubmissionQueueRec fsq = new FeedSubmissionQueueRec();
                            fsq.ID = Guid.NewGuid();
                            fsq.ScheduleID = rec.ID;
                            fsq.AmazonAccountID = rec.AmazonAccountID;
                            fsq.FeedType = rec.FeedType;
                            fsq.MessageType = rec.MessageType;
                            fsq.MessageXML = rec.MessageXML;
                            fsq.RunAt = rec.NextRun;
                            fsq.TimeStamp = db.GetServerNow(ServerDateTimeOffset);

                            db.FeedSubmissionQueue.Add(fsq);

                            rtn.Add(fsq);

                            Log(true, "Queued: " + rec.Name + " for " + rec.NextRun.ToString("M/d/yyyy H:mm:ss"));

                            rec.NextRun = rec.NextRun.AddTimeSpan(rec.TimeSpan);

                            if (rec.MustRunAll == false)
                                while (rec.NextRun < fsq.TimeStamp)
                                    rec.NextRun = rec.NextRun.AddTimeSpan(rec.TimeSpan);

                            RecsProcessed = true;

                        }

                        db.SaveChanges();

                        if (!RecsProcessed)
                            break;
                    }
                }

                scope.Complete();
            }

            return rtn;

        }

        public static List<FeedSubmissionQueueRec> SubmitQueuedScheduledFeeds(Entities db, Action<bool, string> Log)
        {
            Log(true, "Submitting Feed(s)...");

            List<FeedSubmissionQueueRec> rtn = new List<FeedSubmissionQueueRec>();

            int ServerDateTimeOffset = db.GetServerDateTimeOffset();
            DateTime ServerNow = db.GetServerNow(ServerDateTimeOffset);

            var RecsToProcess = db.FeedSubmissionQueue.Where(fsq => fsq.RunAt < ServerNow && fsq.FeedSubmissionID == null);

            foreach (var rectype in RecsToProcess.Select(fsq => new { fsq.AmazonAccountID, fsq.MessageType, fsq.FeedType }).Distinct())
            {

                Dictionary<int, object> Messages = new Dictionary<int, object>();

                foreach (var rec in RecsToProcess.Where(r => r.AmazonAccountID == rectype.AmazonAccountID && r.MessageType == rectype.MessageType && r.FeedType == rectype.FeedType))
                {
                    Type TheType = Assembly.GetExecutingAssembly().GetType("Amazon.XML." + rec.MessageType);
                    Messages.Add(rec.AmazonMessageID, Startbutton.Library.InvokeStaticGenericMethod(typeof(Startbutton.Library), "XmlDeserialize", TheType, new object[] { rec.MessageXML }));
                }

                Log(false, "Submitting Feed... ");

                var sfr = Library.SubmitFeed(db, rectype.AmazonAccountID,  Startbutton.Library.StringToEnum<Library.FeedType>(rectype.FeedType), Startbutton.Library.StringToEnum<Amazon.XML.AmazonEnvelopeMessageType>(rectype.MessageType), Messages);
                string FeedSubmissionID = sfr.FeedSubmissionInfo.FeedSubmissionId;

                Log(true, FeedSubmissionID);

                DateTime SubmissionTimeStamp = db.GetServerNow(ServerDateTimeOffset); ;

                foreach (var rec in RecsToProcess.Where(r => r.AmazonAccountID == rectype.AmazonAccountID && r.MessageType == rectype.MessageType && r.FeedType == rectype.FeedType))
                {
                    Log(true, "Queue item " + rec.ID.ToString() + " included.");
                    rec.FeedSubmissionID = FeedSubmissionID;
                    rec.SubmissionTimeStamp = SubmissionTimeStamp;
                    rtn.Add(rec);
                }
            }

            db.SaveChanges();

            return rtn;

        }

        public static List<FeedSubmissionQueueRec> GetStatusOfSubmittedScheduledFeeds(Entities db, Action<bool, string> Log)
        {
            Log(true, "Checking Statuses of Submitted Feeds...");

            List<FeedSubmissionQueueRec> rtn = new List<FeedSubmissionQueueRec>();

            int ServerDateTimeOffset = db.GetServerDateTimeOffset();

            var RecsToProcess = db.FeedSubmissionQueue.Where(IsActiveListing);

            foreach (Guid AmazonAccountID in RecsToProcess.Select(fsq => fsq.AmazonAccountID).Distinct())
            {
                List<string> SubmissionIDs = new List<string>();

                foreach(var rec in RecsToProcess.Where(r => r.AmazonAccountID == AmazonAccountID).Select(r => r.FeedSubmissionID).Distinct())
                    SubmissionIDs.Add(rec);

                var rslts = Library.GetFeedSubmissionList(null, db.AmazonAccounts.Single(aa => aa.ID == AmazonAccountID), SubmissionIDs, Log);

                foreach (var rslt in rslts)
                {
                    var recs = db.FeedSubmissionQueue.Where(fsq => fsq.FeedSubmissionID == rslt.FeedSubmissionId);

                    foreach (var rec in recs)
                    {
                        rec.SubmissionStatusTimeStamp = db.GetServerNow(ServerDateTimeOffset);
                        rec.SubmissionStatus = rslt.FeedProcessingStatus;
                        rtn.Add(rec);
                    }
                }

            }

            db.SaveChanges();

            return rtn;

        }

        public static List<FeedSubmissionQueueRec> ProcessCompletedScheduledFeeds(Entities db, Action<bool, string> Log)
        {
            Log(true, "Checking for Results of Completed Feeds...");

            List<FeedSubmissionQueueRec> rtn = new List<FeedSubmissionQueueRec>();

            int ServerDateTimeOffset = db.GetServerDateTimeOffset();
            var CompeltedFeeds = db.FeedSubmissionQueue.Where(fsq => fsq.FeedSubmissionID != null && fsq.SubmissionStatus == "_DONE_" && fsq.ResultTimeStamp == null).Select(r => new { r.AmazonAccountID, r.FeedSubmissionID }).Distinct();

            Library.Throttler throttler = new Library.Throttler(5000);

            foreach (var feed in CompeltedFeeds)
            {
                var QueueRecs = db.FeedSubmissionQueue.Where(fsq => fsq.FeedSubmissionID == feed.FeedSubmissionID);

                string ResultXML = Library.GetFeedSubmissionResult(new List<Library.Throttler>() { throttler }, db.AmazonAccounts.Single(aa => aa.ID == feed.AmazonAccountID), feed.FeedSubmissionID, Log);

                Amazon.XML.AmazonEnvelope Envelope = Startbutton.Library.XmlDeserialize<Amazon.XML.AmazonEnvelope>(ResultXML);

                var report = ((Amazon.XML.ProcessingReport)Envelope.Message[0].Item);
                
                if (report.Result != null)
                    foreach (var msg in report.Result)
                    {
                        int MessageID = int.Parse(msg.MessageID);
                        var rec = QueueRecs.Single(r => r.AmazonMessageID == MessageID);
                        rec.ErrorXML = Startbutton.Library.XmlSerialize(msg);
                        rec.ResultTimeStamp = db.GetServerNow(ServerDateTimeOffset);
                    }

                foreach(var rec in QueueRecs.Where(qr => qr.FeedSubmissionID == feed.FeedSubmissionID))
                    rec.ResultTimeStamp = db.GetServerNow(ServerDateTimeOffset);


            }

            db.SaveChanges();

            return rtn;

        }

        public static bool IsActiveListing(FeedSubmissionQueueRec fsq)
        {
            return fsq.FeedSubmissionID != null && (fsq.SubmissionStatus == null || fsq.SubmissionStatus != "_DONE_");
        }

    }

}
