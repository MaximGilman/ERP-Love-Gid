﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ERP_Love_Gid
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ERPModelContainer : DbContext
    {
        public ERPModelContainer()
            : base("name=ERPModelContainer")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Account> AccountSet { get; set; }
        public virtual DbSet<Client> ClientSet { get; set; }
        public virtual DbSet<Contract> ContractSet { get; set; }
        public virtual DbSet<Employee> EmployeeSet { get; set; }
        public virtual DbSet<Event> EventSet { get; set; }
        public virtual DbSet<Payments> PaymentsSet { get; set; }
        public virtual DbSet<PaymentToPeers> PaymentToPeersSet { get; set; }
        public virtual DbSet<Pay_min> Pay_minSet { get; set; }
        public virtual DbSet<Salary> SalarySet { get; set; }
        public virtual DbSet<SalaryTypes> SalaryTypesSet { get; set; }
    }
}
