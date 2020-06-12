﻿using Eaglegroup_project.Application.ViewModels.System;
using Eaglegroup_project.Utilities.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Eaglegroup_project.Application.Interfaces
{
    public interface IRoleService
    {
        Task<bool> AddAsync(AppRoleViewModel userVm);

        Task<bool> CheckNameExist(string name);

        Task DeleteAsync(Guid id);

        Task<List<AppRoleViewModel>> GetAllAsync();

        Task<List<AppRoleViewModel>> GetAllByModuleIdAsync(int moduleId);

        Task<List<AppRoleViewModel>> GetAllExcept_Async(string roleName);

        PagedResult<AppRoleViewModel> GetAllPagingAsync(int? moduleId, string keyword, int page, int pageSize);

        Task<AppRoleViewModel> GetById(Guid id);


        Task UpdateAsync(AppRoleViewModel userVm);

        List<PermissionViewModel> GetListFunctionWithRole(Guid roleId);

        void SavePermission(List<PermissionViewModel> permissions, Guid roleId);

        Task<bool> CheckPermission(string functionId, string action, string[] roles);
    }
}
