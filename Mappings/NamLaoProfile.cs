using AutoMapper;
using NamLao206.Models;
using NamLao206.Models.ViewModels;

namespace NamLao206.Mappings
{
    public class NamLaoProfile : Profile
    {
        public NamLaoProfile()
        {
            CreateMap<RegisterVM, Account>(MemberList.None);
            CreateMap<Account, RegisterVM>(MemberList.None);
            CreateMap<UpdateProfileVM, Employee>(MemberList.None);
            CreateMap<Employee, UpdateProfileVM>(MemberList.None);
        }
    }
}
