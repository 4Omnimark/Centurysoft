﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UkListing
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class UKOmnimarkEntities : DbContext
    {
        public UKOmnimarkEntities()
            : base("name=UKOmnimarkEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<tbl_Category> tbl_Category { get; set; }
        public DbSet<tbl_Sports> tbl_Sports { get; set; }
        public DbSet<tbl_SportsNotPrime> tbl_SportsNotPrime { get; set; }
        public DbSet<tbl_Toys> tbl_Toys { get; set; }
        public DbSet<tbl_ToysNotPrime> tbl_ToysNotPrime { get; set; }
        public DbSet<tbl_Beauty> tbl_Beauty { get; set; }
        public DbSet<tbl_BeautyNotPrime> tbl_BeautyNotPrime { get; set; }
        public DbSet<tbl_Prohibited_Keywords> tbl_Prohibited_Keywords { get; set; }
        public DbSet<tbl_Baby> tbl_Baby { get; set; }
        public DbSet<tbl_BabyNotPrime> tbl_BabyNotPrime { get; set; }
        public DbSet<tbl_Jewellery> tbl_Jewellery { get; set; }
        public DbSet<tbl_JewelleryNotPrime> tbl_JewelleryNotPrime { get; set; }
        public DbSet<tbl_Watches> tbl_Watches { get; set; }
        public DbSet<tbl_WatchesNotPrime> tbl_WatchesNotPrime { get; set; }
        public DbSet<SellingPlace> SellingPlaces { get; set; }
        public DbSet<tbl_Account> tbl_Account { get; set; }
        public DbSet<Canada_Prohibited_Keywords> Canada_Prohibited_Keywords { get; set; }
        public DbSet<tbl_Keywords> tbl_Keywords { get; set; }
        public DbSet<ServiceStatu> ServiceStatus { get; set; }
        public DbSet<AmazonResponse> AmazonResponses { get; set; }
        public DbSet<tbl_Sports_test> tbl_Sports_test { get; set; }
        public DbSet<VariousAsin> VariousAsins { get; set; }
        public DbSet<tbl_HomeandKitchen> tbl_HomeandKitchen { get; set; }
        public DbSet<tbl_HomeandKitchenNotPrime> tbl_HomeandKitchenNotPrime { get; set; }
        public DbSet<upc_codes> upc_codes { get; set; }
    
        public virtual int Update_UK_Prohibited(Nullable<int> flag)
        {
            var flagParameter = flag.HasValue ?
                new ObjectParameter("flag", flag) :
                new ObjectParameter("flag", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Update_UK_Prohibited", flagParameter);
        }
    }
}