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
    
    public partial class SalesVenue
    {
        public SalesVenue()
        {
            this.ASINsSuccessfullyExporteds = new HashSet<ASINsSuccessfullyExported>();
            this.ExportSpecs = new HashSet<ExportSpec>();
        }
    
        public System.Guid ID { get; set; }
        public string Name { get; set; }
        public System.DateTime TimeStamp { get; set; }
    
        public virtual ICollection<ASINsSuccessfullyExported> ASINsSuccessfullyExporteds { get; set; }
        public virtual ICollection<ExportSpec> ExportSpecs { get; set; }
    }
}