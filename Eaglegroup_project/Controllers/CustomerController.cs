using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using Eaglegroup_project.Application.Interfaces;
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

        public IActionResult GetAll()
        {
            var model = _customerService.GetAll(null);
            return new OkObjectResult(model);
        }
    }
}