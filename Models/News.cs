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
    
    public partial class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Details { get; set; }
        public int TopicId { get; set; }
        public Nullable<int> SubMenuId { get; set; }
        public System.DateTime DateUp { get; set; }
        public Nullable<int> AdminId { get; set; }
        public int Views { get; set; }
        public string Picture { get; set; }
        public Nullable<System.DateTime> DateModified { get; set; }
        public Nullable<int> cosoId { get; set; }
        public Nullable<int> catid { get; set; }
        public bool uutien { get; set; }
        public Nullable<bool> Duyet { get; set; }
        public string TitleChange { get; set; }
        public string Author { get; set; }
    
        public virtual SubMenu SubMenu { get; set; }
        public virtual Topic Topic { get; set; }
        public virtual Transport Transport { get; set; }
    }
}