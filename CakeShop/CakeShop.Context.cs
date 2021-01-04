﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CakeShop
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class CakeShopEntities : DbContext
    {
        public CakeShopEntities()
            : base("name=CakeShopEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Cake> Cakes { get; set; }
        public virtual DbSet<Cake_Image> Cake_Image { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public virtual DbSet<StockReceiving> StockReceivings { get; set; }
    
        [DbFunction("CakeShopEntities", "CalcSumInvoicesInYear")]
        public virtual IQueryable<CalcSumInvoicesInYear_Result> CalcSumInvoicesInYear(Nullable<int> year)
        {
            var yearParameter = year.HasValue ?
                new ObjectParameter("year", year) :
                new ObjectParameter("year", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<CalcSumInvoicesInYear_Result>("[CakeShopEntities].[CalcSumInvoicesInYear](@year)", yearParameter);
        }
    
        [DbFunction("CakeShopEntities", "CalcSumRevenueInYear")]
        public virtual IQueryable<CalcSumRevenueInYear_Result> CalcSumRevenueInYear(Nullable<int> year)
        {
            var yearParameter = year.HasValue ?
                new ObjectParameter("year", year) :
                new ObjectParameter("year", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<CalcSumRevenueInYear_Result>("[CakeShopEntities].[CalcSumRevenueInYear](@year)", yearParameter);
        }
    
        [DbFunction("CakeShopEntities", "CalcSumStockReceivingCakesInYear")]
        public virtual IQueryable<CalcSumStockReceivingCakesInYear_Result> CalcSumStockReceivingCakesInYear(Nullable<int> year)
        {
            var yearParameter = year.HasValue ?
                new ObjectParameter("year", year) :
                new ObjectParameter("year", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<CalcSumStockReceivingCakesInYear_Result>("[CakeShopEntities].[CalcSumStockReceivingCakesInYear](@year)", yearParameter);
        }
    
        [DbFunction("CakeShopEntities", "CalcSumStockReceivingMoneyInYear")]
        public virtual IQueryable<CalcSumStockReceivingMoneyInYear_Result> CalcSumStockReceivingMoneyInYear(Nullable<int> year)
        {
            var yearParameter = year.HasValue ?
                new ObjectParameter("year", year) :
                new ObjectParameter("year", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<CalcSumStockReceivingMoneyInYear_Result>("[CakeShopEntities].[CalcSumStockReceivingMoneyInYear](@year)", yearParameter);
        }
    
        [DbFunction("CakeShopEntities", "GetInfCakeByID")]
        public virtual IQueryable<GetInfCakeByID_Result> GetInfCakeByID(Nullable<int> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<GetInfCakeByID_Result>("[CakeShopEntities].[GetInfCakeByID](@id)", idParameter);
        }
    
        [DbFunction("CakeShopEntities", "GetInfInvoiceByID")]
        public virtual IQueryable<GetInfInvoiceByID_Result> GetInfInvoiceByID(Nullable<int> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<GetInfInvoiceByID_Result>("[CakeShopEntities].[GetInfInvoiceByID](@id)", idParameter);
        }
    
        [DbFunction("CakeShopEntities", "GetInvoiceDetailsByIDInvoice")]
        public virtual IQueryable<GetInvoiceDetailsByIDInvoice_Result> GetInvoiceDetailsByIDInvoice(Nullable<int> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<GetInvoiceDetailsByIDInvoice_Result>("[CakeShopEntities].[GetInvoiceDetailsByIDInvoice](@id)", idParameter);
        }
    
        [DbFunction("CakeShopEntities", "SearchByName")]
        public virtual IQueryable<SearchByName_Result> SearchByName(string search_text)
        {
            var search_textParameter = search_text != null ?
                new ObjectParameter("search_text", search_text) :
                new ObjectParameter("search_text", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<SearchByName_Result>("[CakeShopEntities].[SearchByName](@search_text)", search_textParameter);
        }
    
        [DbFunction("CakeShopEntities", "StatisticByYear")]
        public virtual IQueryable<StatisticByYear_Result> StatisticByYear(Nullable<int> year)
        {
            var yearParameter = year.HasValue ?
                new ObjectParameter("year", year) :
                new ObjectParameter("year", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<StatisticByYear_Result>("[CakeShopEntities].[StatisticByYear](@year)", yearParameter);
        }
    
        [DbFunction("CakeShopEntities", "StatisticRevenueByMonthInYear")]
        public virtual IQueryable<StatisticRevenueByMonthInYear_Result> StatisticRevenueByMonthInYear(Nullable<int> mont, Nullable<int> year)
        {
            var montParameter = mont.HasValue ?
                new ObjectParameter("mont", mont) :
                new ObjectParameter("mont", typeof(int));
    
            var yearParameter = year.HasValue ?
                new ObjectParameter("year", year) :
                new ObjectParameter("year", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<StatisticRevenueByMonthInYear_Result>("[CakeShopEntities].[StatisticRevenueByMonthInYear](@mont, @year)", montParameter, yearParameter);
        }
    
        [DbFunction("CakeShopEntities", "StatisticRevenueByTypeOfCakeInYear")]
        public virtual IQueryable<StatisticRevenueByTypeOfCakeInYear_Result> StatisticRevenueByTypeOfCakeInYear(Nullable<int> year)
        {
            var yearParameter = year.HasValue ?
                new ObjectParameter("year", year) :
                new ObjectParameter("year", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<StatisticRevenueByTypeOfCakeInYear_Result>("[CakeShopEntities].[StatisticRevenueByTypeOfCakeInYear](@year)", yearParameter);
        }
    
        public virtual int AddCake(Nullable<int> id, string name, string des, string type, Nullable<decimal> orPrice, Nullable<decimal> sellPrice, Nullable<int> quantity, string link)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(int));
    
            var nameParameter = name != null ?
                new ObjectParameter("name", name) :
                new ObjectParameter("name", typeof(string));
    
            var desParameter = des != null ?
                new ObjectParameter("des", des) :
                new ObjectParameter("des", typeof(string));
    
            var typeParameter = type != null ?
                new ObjectParameter("type", type) :
                new ObjectParameter("type", typeof(string));
    
            var orPriceParameter = orPrice.HasValue ?
                new ObjectParameter("orPrice", orPrice) :
                new ObjectParameter("orPrice", typeof(decimal));
    
            var sellPriceParameter = sellPrice.HasValue ?
                new ObjectParameter("sellPrice", sellPrice) :
                new ObjectParameter("sellPrice", typeof(decimal));
    
            var quantityParameter = quantity.HasValue ?
                new ObjectParameter("quantity", quantity) :
                new ObjectParameter("quantity", typeof(int));
    
            var linkParameter = link != null ?
                new ObjectParameter("link", link) :
                new ObjectParameter("link", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AddCake", idParameter, nameParameter, desParameter, typeParameter, orPriceParameter, sellPriceParameter, quantityParameter, linkParameter);
        }
    
        public virtual int AddCakeImage(Nullable<int> id, Nullable<int> orNum, string link, Nullable<int> isActive)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(int));
    
            var orNumParameter = orNum.HasValue ?
                new ObjectParameter("orNum", orNum) :
                new ObjectParameter("orNum", typeof(int));
    
            var linkParameter = link != null ?
                new ObjectParameter("link", link) :
                new ObjectParameter("link", typeof(string));
    
            var isActiveParameter = isActive.HasValue ?
                new ObjectParameter("isActive", isActive) :
                new ObjectParameter("isActive", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AddCakeImage", idParameter, orNumParameter, linkParameter, isActiveParameter);
        }
    
        public virtual int AddInvoice(Nullable<int> id, Nullable<System.DateTime> date, string name, string add, string phone, Nullable<decimal> ship, Nullable<decimal> total)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(int));
    
            var dateParameter = date.HasValue ?
                new ObjectParameter("date", date) :
                new ObjectParameter("date", typeof(System.DateTime));
    
            var nameParameter = name != null ?
                new ObjectParameter("name", name) :
                new ObjectParameter("name", typeof(string));
    
            var addParameter = add != null ?
                new ObjectParameter("add", add) :
                new ObjectParameter("add", typeof(string));
    
            var phoneParameter = phone != null ?
                new ObjectParameter("phone", phone) :
                new ObjectParameter("phone", typeof(string));
    
            var shipParameter = ship.HasValue ?
                new ObjectParameter("ship", ship) :
                new ObjectParameter("ship", typeof(decimal));
    
            var totalParameter = total.HasValue ?
                new ObjectParameter("total", total) :
                new ObjectParameter("total", typeof(decimal));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AddInvoice", idParameter, dateParameter, nameParameter, addParameter, phoneParameter, shipParameter, totalParameter);
        }
    
        public virtual int AddInvoiceDetail(Nullable<int> id, Nullable<int> orNum, Nullable<int> idCake, Nullable<int> quantity)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(int));
    
            var orNumParameter = orNum.HasValue ?
                new ObjectParameter("orNum", orNum) :
                new ObjectParameter("orNum", typeof(int));
    
            var idCakeParameter = idCake.HasValue ?
                new ObjectParameter("idCake", idCake) :
                new ObjectParameter("idCake", typeof(int));
    
            var quantityParameter = quantity.HasValue ?
                new ObjectParameter("quantity", quantity) :
                new ObjectParameter("quantity", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AddInvoiceDetail", idParameter, orNumParameter, idCakeParameter, quantityParameter);
        }
    
        public virtual int AddStockReceiving(Nullable<int> id, Nullable<int> idCake, Nullable<int> quantity, Nullable<System.DateTime> date)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(int));
    
            var idCakeParameter = idCake.HasValue ?
                new ObjectParameter("idCake", idCake) :
                new ObjectParameter("idCake", typeof(int));
    
            var quantityParameter = quantity.HasValue ?
                new ObjectParameter("quantity", quantity) :
                new ObjectParameter("quantity", typeof(int));
    
            var dateParameter = date.HasValue ?
                new ObjectParameter("date", date) :
                new ObjectParameter("date", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AddStockReceiving", idParameter, idCakeParameter, quantityParameter, dateParameter);
        }
    
        public virtual int SetIsActive(Nullable<int> id, Nullable<int> orNum, Nullable<int> isActive)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(int));
    
            var orNumParameter = orNum.HasValue ?
                new ObjectParameter("orNum", orNum) :
                new ObjectParameter("orNum", typeof(int));
    
            var isActiveParameter = isActive.HasValue ?
                new ObjectParameter("isActive", isActive) :
                new ObjectParameter("isActive", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SetIsActive", idParameter, orNumParameter, isActiveParameter);
        }
    
        [DbFunction("CakeShopEntities", "getAllInvoices")]
        public virtual IQueryable<getAllInvoices_Result> getAllInvoices()
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<getAllInvoices_Result>("[CakeShopEntities].[getAllInvoices]()");
        }
    }
}