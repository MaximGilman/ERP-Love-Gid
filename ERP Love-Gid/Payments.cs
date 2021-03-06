//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Payments
    {
        public int Id { get; set; }
        public int Receipt { get; set; }
        public string Comment { get; set; }
        public System.DateTime Date { get; set; }
        public int Sum { get; set; }
        public int EmployeeId { get; set; }
        public int ContractId { get; set; }
        public Nullable<bool> StatusForPeers { get; set; }
        public Nullable<bool> StatusForSalary { get; set; }
        public Nullable<System.DateTime> DateForPayment { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual Contract Contract { get; set; }
        public virtual Pay_min Pay_min { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Event Event { get; set; }
        public virtual Employee EmployeeTo { get; set; }
    }
}
