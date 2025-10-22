using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NamLao206.Models.ViewModels
{
    public class UserPermission
    {
        public int AccountId { get; set; }
        public List<string> PermissionCodes { get; set; }
        public List<MenuItem> MenuItems { get; set; }
    }
}