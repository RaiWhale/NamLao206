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
    
    public partial class Feedback
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
        public System.DateTime DateUp { get; set; }
        public byte IsClosed { get; set; }
    }
}
