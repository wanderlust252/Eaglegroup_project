using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eaglegroup_project.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Eaglegroup_project.Controllers
{
    public class ManagerController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly IAuthorizationService _authorizationService;

        public ManagerController(IAuthorizationService authorizationService, IMemoryCache memoryCache)
        {

            _authorizationService = authorizationService;
            _cache = memoryCache;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _authorizationService.AuthorizeAsync(User, "MANAGER", Operations.Read);
            if (result.Succeeded == false)  // Nếu không có quyền trả về trang Login
                return new RedirectResult("/Login/Index");

            ViewData["ViewHome"] = "";
            ViewData["ViewAction"] = "Eagle Group";
            return View();
        }

        public async Task<IActionResult> Home()
        {

            var result = await _authorizationService.AuthorizeAsync(User, "HOME_MANAGER", Operations.Read);
            if (result.Succeeded == false)  // Nếu không có quyền trả về trang Login
            {
                if ((await _authorizationService.AuthorizeAsync(User, "HOME", Operations.Read)).Succeeded)
                    return new RedirectResult("/Home/Index");
                else if ((await _authorizationService.AuthorizeAsync(User, "HOME_QLTC", Operations.Read)).Succeeded)
                    return new RedirectResult("/QuanLyThiCong/Home");
                else if ((await _authorizationService.AuthorizeAsync(User, "HOME_HSCN", Operations.Read)).Succeeded)
                    return new RedirectResult("/Worker/Home");
                else if ((await _authorizationService.AuthorizeAsync(User, "HOME_QLVT", Operations.Read)).Succeeded)
                    return new RedirectResult("/QuanLyVatTu/Home");
                else if ((await _authorizationService.AuthorizeAsync(User, "CV_HOME", Operations.Read)).Succeeded)
                    return new RedirectResult("/Task/Home");
                else
                    return new RedirectResult("/Login/index");
            }

            ViewData["ViewHome"] = "";
            ViewData["ViewAction"] = "Eagle Group";
            return View();
        }
    }
}