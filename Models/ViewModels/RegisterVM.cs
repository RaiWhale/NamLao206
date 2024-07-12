using System.ComponentModel.DataAnnotations;

namespace NamLao206.Models.ViewModels
{
    public class RegisterVM
    {
        [Key]
        public string LoginName { get; set; }
        public string Password { get; set; }

        public int? EmployeeId { get; set; }
        public int? AccountType { get; set; }
    }
}