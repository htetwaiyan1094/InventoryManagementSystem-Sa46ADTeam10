//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LUSSISADTeam10API.Models.DBModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class purchaseorder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public purchaseorder()
        {
            this.purchaseorderdetails = new HashSet<purchaseorderdetail>();
        }
    
        public int poid { get; set; }
        public int purchasedby { get; set; }
        public int supid { get; set; }
        public Nullable<System.DateTime> podate { get; set; }
        public int status { get; set; }
    
        public virtual supplier supplier { get; set; }
        public virtual user user { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<purchaseorderdetail> purchaseorderdetails { get; set; }
    }
}
