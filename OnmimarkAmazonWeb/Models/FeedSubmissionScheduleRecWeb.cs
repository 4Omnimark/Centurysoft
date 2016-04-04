using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OmnimarkAmazon.Models
{
    [MetadataType(typeof(FeedSubmissionScheduleRecMetaData))]
    public class FeedSubmissionScheduleRecWeb : FeedSubmissionScheduleRec
    {

        [Required]
        public Nullable<int> TimeSpanCount { get; set; }

        [Required]
        public Nullable<int> TimeSpanSpanType { get; set; }

        [Required]
        public new Nullable<DateTime> NextRun { get; set; }

        class FeedSubmissionScheduleRecMetaData
        {
            [Required]
            public string Name { get; set; }

            [Required]
            public string FeedType { get; set; }

            [Required]
            public string MessageType { get; set; }

        }
    }
}