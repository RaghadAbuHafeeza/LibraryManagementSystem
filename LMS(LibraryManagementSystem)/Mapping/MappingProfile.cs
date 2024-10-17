using AutoMapper;
using LMS_LibraryManagementSystem_.Models;
using LMS_LibraryManagementSystem_.ViewModel;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace LMS_LibraryManagementSystem_.Mapping
{
    public class MappingProfile : Profile
    {
        // Constructor
        public MappingProfile()
        {
            // we want to convert from model to ViewModel or reverse
            // CreateMap<IdentityRole, RoleVM>().ReverseMap();
            CreateMap<IdentityRole, RoleVM>();
            CreateMap<ApplicationUser, ApplicationUserVM>();
        }
    }
}
