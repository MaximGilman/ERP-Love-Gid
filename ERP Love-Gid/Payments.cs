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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Payments()
        {
            this.PaymentEmployeeConnect = new HashSet<PaymentEmployeeConnect>();
        }
    
        public int Id { get; set; }
        public int Receipt { get; set; }
        public string Comment { get; set; }
        public System.DateTime Date { get; set; }
        public int Sum { get; set; }
        public int AccounId { get; set; }
        public int ContractId { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual Contract Contract { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaymentEmployeeConnect> PaymentEmployeeConnect { get; set; }
    }
}
