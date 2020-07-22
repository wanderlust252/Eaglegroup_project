using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eaglegroup_project.Application.Interfaces;
using Eaglegroup_project.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Eaglegroup_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class StatisticalController : Controller
    {
        ICustomerService _customerService;
        IUserService _userService;
        IStatisticalService _statisticalService;
        private Guid _userId;
        public StatisticalController(ICustomerService customerService, IUserService userService, IStatisticalService statisticalService)
        {
            _customerService = customerService;
            _userService = userService;
            _statisticalService = statisticalService;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //Get user id   
            _userId = User.GetUserId();
        }
        [HttpGet]
        public decimal GetDealRate(DateTime fromDate, DateTime toDate, int status)
        {//?fromDate=" + fromD + "&toDate=" + toD
            //_customerService.AppointCustomer(listId, userId);
            //call service
            return _statisticalService.GetDealRate(fromDate, toDate, _userId, status, User.GetRoles().Split(";"));
        }
        [HttpGet]
        public decimal GetDealRateBySpecifyDate(DateTime specDate, int status)
        {//?fromDate=" + fromD + "&toDate=" + toD
            //_customerService.AppointCustomer(listId, userId);
            //call service
            //string userSpec = User.Identity.Name;
            return _statisticalService.GetDealRateBySpecifyDate(specDate, _userId, status, User.GetRoles().Split(";"));
        }
        [HttpGet]
        public decimal GetStatiscal(DateTime fromDate, DateTime toDate, int status)
        {//?fromDate=" + fromD + "&toDate=" + toD
            //_customerService.AppointCustomer(listId, userId);
            //call service
            return _statisticalService.GetStatiscal(fromDate, toDate, _userId,status, User.GetRoles().Split(";"));
        }
        [HttpGet]
        public decimal GetStatiscalBySpecifyDate(DateTime specDate,int status )
        {//?fromDate=" + fromD + "&toDate=" + toD
            //_customerService.AppointCustomer(listId, userId);
            //call service
            //string userSpec = User.Identity.Name;
            return _statisticalService.GetStatiscalBySpecifyDate(specDate, _userId, status, User.GetRoles().Split(";"));
        }
        public IActionResult Index()
        {
            //var model = _statisticalService.GetStatiscal();
            return View();
        }
    }
}