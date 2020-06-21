using AutoMapper;
using Eaglegroup_project.Application.ViewModels.System;
using Eaglegroup_project.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eaglegroup_project.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<AppUserViewModel, AppUser>()
            .ConstructUsing(c => new AppUser(c.Id.GetValueOrDefault(Guid.Empty), c.FullName, c.UserName,
            c.Email, c.PhoneNumber, c.Avatar, c.Status, c.ModifiedBy, c.DateModified, c.DepartmentId, c.CompanyId));

            CreateMap<AppRoleViewModel, AppRole>()
       .ConstructUsing(c => new AppRole(c.Description, c.ModuleId, c.Name, c.Type, c.SortOrder));

            CreateMap<PermissionViewModel, Permission>()
            .ConstructUsing(c => new Permission(c.RoleId, c.FunctionId, c.CanCreate, c.CanRead, c.CanUpdate, c.CanDelete));

            CreateMap<FunctionViewModel, Function>()
          .ConstructUsing(c => new Function(c.Name, c.URL, c.ParentId, c.IconCss, c.SortOrder));

            CreateMap<CustomerViewModel, Customer>()
        .ConstructUsing(c => new Customer(c.FullName, c.CreatorNote, c.StaffNote, c.StaffId, c.Deal, c.Price, c.DateSendByCustomer, c.CreatorId,c.isDelete));
        }
    }
}
