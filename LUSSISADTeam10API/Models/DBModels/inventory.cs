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
    
    public partial class inventory
    {
        public int invid { get; set; }
        public int itemid { get; set; }
        public Nullable<int> stock { get; set; }
        public Nullable<int> reorderlevel { get; set; }
        public Nullable<int> reorderqty { get; set; }
    
        public virtual item item { get; set; }
    }
}
