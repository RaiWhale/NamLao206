//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NamLao206.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Picture
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Picture()
        {
            this.DM_PhongBans = new HashSet<DM_PhongBans>();
        }
    
        public int Id { get; set; }
        public string Url { get; set; }
        public int KhoaphongId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DM_PhongBans> DM_PhongBans { get; set; }
        public virtual DM_PhongBans DM_PhongBans1 { get; set; }
    }
}