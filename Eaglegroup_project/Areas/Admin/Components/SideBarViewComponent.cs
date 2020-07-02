using Eaglegroup_project.Application.Interfaces;
using Eaglegroup_project.Application.ViewModels.System;
using Eaglegroup_project.Extensions;
using Eaglegroup_project.Utilities.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Eaglegroup_project.Areas.Admin.Components
{
    [Area("Admin")]
    [Authorize]
    public class SideBarViewComponent : ViewComponent
    {
        private IFunctionService _functionService;
        private readonly IRoleService _roleService;

        public SideBarViewComponent(IFunctionService functionService, IRoleService roleService)
        {
            _functionService = functionService;
            _roleService = roleService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var roles = ((ClaimsPrincipal)User).GetSpecificClaim("Roles");
            List<FunctionViewModel> functions;
            
            if (roles.Split(";").Contains(CommonConstants.AppRole.AdminRole))
            {
                functions = await _functionService.GetAll(string.Empty, Guid.Empty);
            }
            else
            {
                var roleId = await _roleService.GetRoleIdByName(roles);
                //TODO: Get by permission
                functions = await _functionService.GetAll(string.Empty, Guid.Empty);
            }
            return View(functions);
        }
    }
}
