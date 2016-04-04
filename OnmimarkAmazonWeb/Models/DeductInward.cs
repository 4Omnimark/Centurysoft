using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace OmnimarkAmazonWeb.Models
{
    public class DeductInward
    {
        public string OrderID { get; set; }
        public string CustomerName { get; set; }
        public string SalesVenue { get; set; }
        [Required]
        [DisplayName("Enter Quantity")]
        
        public decimal Qty { get; set; }
        public string ReasonRef1 { get; set; }
        public string ReasonRef2 { get; set; }
        public string ReasonRef3 { get; set; }
    
        public Guid ProductID { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public string ProductName { get; set; }
        [Required]
        public Guid ReasonID { get; set; }
    }
}