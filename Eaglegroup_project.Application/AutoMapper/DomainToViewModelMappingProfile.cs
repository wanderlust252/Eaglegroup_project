using AutoMapper;
using Eaglegroup_project.Application.ViewModels.System;
using Eaglegroup_project.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eaglegroup_project.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        { 
            CreateMap<AppUser, AppUserViewModel>();
            CreateMap<AppRole, AppRoleViewModel>();
            CreateMap<Function, FunctionViewModel>();
            CreateMap<Customer, CustomerViewModel>();
        }
    }
}
