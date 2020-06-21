using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using Eaglegroup_project.Application.Interfaces;
using Eaglegroup_project.Extensions;
using Eaglegroup_project.Utilities.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eaglegroup_project.Controllers
{
    [Authorize(Roles = "Admin, SaleStaff, Marketing")]
    public class CustomerController : Controller
    {
        ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            var roles = User.GetRoles();
            var listRole = roles.Split(";");
            Guid? userId = User.GetUserId();
            int checkR = 0;
            if (listRole.Contains(CommonConstants.AppRole.MarketingRole))
            {
                checkR = 1;
            }else if (listRole.Contains(CommonConstants.AppRole.SaleStaffRole))
            {
                checkR = 2;
            }
            var model = _customerService.GetAllPaging( keyword,  page, pageSize, checkR, userId.Value);
            return new OkObjectResult(model);
        }

        public IActionResult GetAll()
        {
            var model = _customerService.GetAll(null);
            return new OkObjectResult(model);
        }
    }
}