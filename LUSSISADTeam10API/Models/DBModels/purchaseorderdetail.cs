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
    
    public partial class purchaseorderdetail
    {
        public int poid { get; set; }
        public int itemid { get; set; }
        public int qty { get; set; }
        public int delivqty { get; set; }
    
        public virtual item item { get; set; }
        public virtual purchaseorder purchaseorder { get; set; }
    }
}
