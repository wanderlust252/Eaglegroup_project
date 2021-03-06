﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eaglegroup_project.Application.Interfaces;
using Eaglegroup_project.Application.ViewModels.System;
using Eaglegroup_project.Data.Enums;
using Eaglegroup_project.Extensions;
using Eaglegroup_project.Utilities.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Eaglegroup_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, SaleStaff, Marketing")]
    public class CustomerController : Controller
    {
        ICustomerService _customerService;
        IUserService _userService;
        private int _checkR;
        private Guid _userId;

        public CustomerController(ICustomerService customerService, IUserService userService)
        {
            _customerService = customerService;
            _userService = userService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //Get user id   
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
            //model.SaleName = User.Identity.Name;
            //model.CreatorName = _userService.GetById(model.CreatorId).Result.UserName;
            return new OkObjectResult(model);
        }
        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            var model = _customerService.GetAllPaging(keyword, page, pageSize, _checkR, _userId);

            return new OkObjectResult(model);
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var model = _customerService.GetAll(null);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllForAppoint()
        {
            var model = _customerService.GetAllForAppoint();
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(CustomerViewModel cusVm)
        {
            var Id = cusVm.Id;
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                if (cusVm.Id == null || cusVm.Id==0)
                {
                    cusVm.CreatedBy = User.Identity.Name;
                    cusVm.CreatorId = Guid.Parse(User.GetSpecificClaim("UserId"));
                    cusVm.DateCreated = DateTime.Now;
                    cusVm.Status = 1;
                    _customerService.Add(cusVm, _checkR, _userId);
                }
                else
                {
                    cusVm.DateModified = DateTime.Now;
                    cusVm.ModifiedBy= User.Identity.Name;
                    _customerService.Update(cusVm, _checkR, _userId);
                }
                _customerService.Save();
            }
            return new OkObjectResult(cusVm);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _customerService.Delete(id, _checkR, _userId);
            _customerService.Save();
            return new OkObjectResult(id);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AppointCustomer(List<int> listId, Guid userId)
        {
            _customerService.AppointCustomer(listId, userId);
            return new ObjectResult(listId);
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
        [HttpGet]
        public int Statistical(DateTime fromDate, DateTime toDate)
        {//?fromDate=" + fromD + "&toDate=" + toD
            //_customerService.AppointCustomer(listId, userId);
            //call service
            return _customerService.statistical(fromDate,toDate);
        }



    }
}