using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using Eaglegroup_project.Application.Interfaces;
using Eaglegroup_project.Application.ViewModels.System;
using Eaglegroup_project.Extensions;
using Eaglegroup_project.Utilities.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Eaglegroup_project.Controllers
{
    [Authorize(Roles = "Admin, SaleStaff, Marketing")]
    public class CustomerController : Controller
    {
        ICustomerService _customerService;
        private int _checkR;
        private Guid _userId;

        public CustomerController(ICustomerService customerService)
        {           
            _customerService = customerService;

            _userId = User.GetUserId();
            _checkR = RoleCheck(User.GetRoles().Split(";"));
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
    

            var model = _customerService.GetById(id, _checkR, _userId);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetCustomerForSale()
        {
            var model = _customerService.GetCustomerForSale(_checkR, _userId);
            return new OkObjectResult(model);
        }

        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            var model = _customerService.GetAllPaging(keyword, page, pageSize, _checkR, _userId);
            return new OkObjectResult(model);
        }

        public IActionResult GetAll()
        {
            var model = _customerService.GetAll(null);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(CustomerViewModel cusVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                if (cusVm.Id == null)
                {
                    _customerService.Add(cusVm, _checkR, _userId);
                }
                else
                {
                    _customerService.Update(cusVm, _checkR, _userId);
                }
            }
            return new OkObjectResult(cusVm);
        }

        [NonAction]
        public int RoleCheck(string[] listRole)
        {
            if (listRole.Contains(CommonConstants.AppRole.MarketingRole))
            {
                return 1;
            }
            else if (listRole.Contains(CommonConstants.AppRole.SaleStaffRole))
            {
                return 2;
            }
            return 0;
        }


    }
}