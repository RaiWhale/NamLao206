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
    
    public partial class SubMenu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SubMenu()
        {
            this.News = new HashSet<News>();
        }
    
        public int Id { get; set; }
        public string subMenuName { get; set; }
        public Nullable<int> ParentId { get; set; }
        public int TopicId { get; set; }
        public Nullable<int> UserId { get; set; }
    
        public virtual Topic Topic { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<News> News { get; set; }
    }
}
