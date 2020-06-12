using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eaglegroup_project.Data.Entities;
using Eaglegroup_project.Data.Enums;
using Eaglegroup_project.Models.AccountViewModels;
using Eaglegroup_project.Utilities.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Eaglegroup_project.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger _logger;
        private readonly IMemoryCache _cache;

        public LoginController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager
            , ILogger<LoginController> logger, IMemoryCache cache)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _cache = cache;
        }

        public async Task<IActionResult> Index()
        {

            //if (User.Identity.IsAuthenticated)
            //{
            //    await _signInManager.SignOutAsync();
            //}

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Authen(LoginViewModelAdmin model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(model.UserName))
                    {
                        return new ObjectResult(new GenericResult(false, "Chưa nhập tài khoản"));
                    }
                    if (string.IsNullOrEmpty(model.Password))
                    {
                        return new ObjectResult(new GenericResult(false, "Chưa nhập mật khẩu"));
                    }

                    var user = await _userManager.FindByNameAsync(model.UserName);

                    if (user == null)
                    {
                        _logger.LogWarning("Không tìm thấy tài khoản.");
                        return new ObjectResult(new GenericResult(false, "Không tìm thấy tài khoản"));
                    }

                    if (user.Status == Status.InActive)
                    {
                        _logger.LogWarning("Tài khoản đã bị khóa.");
                        return new ObjectResult(new GenericResult(false, "Tài khoản đã bị khoá"));
                    }


                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                    var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        //_cache.Remove("ListMenu");
                        _logger.LogInformation("User logged in.");
                        return new OkObjectResult(new GenericResult(true));
                    }
                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        return new ObjectResult(new GenericResult(false, "Tài khoản đã bị khoá"));
                    }
                    else
                    {
                        return new ObjectResult(new GenericResult(false, "Tên đăng nhập hoặc mật khẩu không đúng"));
                    }
                }

                // If we got this far, something failed, redisplay form
                return new ObjectResult(new GenericResult(false, model));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ObjectResult(new GenericResult(false, model));

            }

        }
    }
}