//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OmnimarkAmazon.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class FeedSubmissionScheduleStatusRec
    {
        public System.Guid ID { get; set; }
        public System.Guid AmazonAccountID { get; set; }
        public string FeedType { get; set; }
        public string MessageType { get; set; }
        public string MessageXML { get; set; }
        public System.DateTime NextRun { get; set; }
        public string TimeSpan { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public bool MustRunAll { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public bool Enabled { get; set; }
        public Nullable<System.Guid> ScheduleID { get; set; }
        public Nullable<int> RunCount { get; set; }
        public Nullable<System.DateTime> LastRun { get; set; }
        public Nullable<System.DateTime> LastCompletion { get; set; }
        public Nullable<int> ErrorCount { get; set; }
        public string AmazonAccountName { get; set; }
        public Nullable<int> ActiveCount { get; set; }
        public Nullable<System.DateTime> Deleted { get; set; }
        public Nullable<System.Guid> AutomaticPriceChangeID { get; set; }
    }
}
