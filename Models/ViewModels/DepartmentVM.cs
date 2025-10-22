using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NamLao206.Models.ViewModels
{
    public class DepartmentVM
    {
        public int Id { get; set; }
        public string TenDonVi { get; set; }
        public string MaDonVi { get; set; }
        public bool IsActive { get; set; }
    }
}