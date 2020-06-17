using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using Eaglegroup_project.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Eaglegroup_project.Controllers
{
    public class CustomerController : Controller
    {
        IConfiguration _configuration;
        ICustomerService _customerService;

        public CustomerController(IConfiguration configuration, ICustomerService customerService)
        {
            _configuration = configuration;
            _customerService = customerService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}