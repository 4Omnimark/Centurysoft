using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OmnimarkAmazon.Models;
using Startbutton.ExtensionMethods;

namespace OmnimarkAmazon.BLL
{
    public static class FeedSubmissionSchedule
    {
        public static void Delete(Entities db, Guid id)
        {
            var rec = db.FeedSubmissionSchedule.Single(fss => fss.ID == id);

            if (rec.FeedSubmissionQueueRecs.Count == 0)
            {
                db.FeedSubmissionSchedule.Remove(rec);
            }
            else
            {
                rec.Deleted = db.GetServerNow();
                rec.Enabled = false;
            }
        }
    }
}
