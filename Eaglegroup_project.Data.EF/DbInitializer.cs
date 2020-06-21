using Eaglegroup_project.Data.Entities;
using Eaglegroup_project.Data.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eaglegroup_project.Data.EF
{
    public class DbInitializer
    {
        private readonly AppDbContext _context;
        private UserManager<AppUser> _userManager;
        private RoleManager<AppRole> _roleManager;
        public DbInitializer(AppDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task Seed()
        {

            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new AppRole()
                {
                    Name = "Admin",
                    NormalizedName = "Admin",
                    Description = "Top manager"
                });
                await _roleManager.CreateAsync(new AppRole()
                {
                    Name = "SaleStaff",
                    NormalizedName = "SaleStaff",
                    Description = "Nhan vien Sale"
                });
                await _roleManager.CreateAsync(new AppRole()
                {
                    Name = "Marketing",
                    NormalizedName = "Marketing",
                    Description = "Nhan vien Marketing"
                });
            }

            if (!_userManager.Users.Any())
            {

                await _userManager.CreateAsync(new AppUser()
                {
                    UserName = "admin",
                    FullName = "Administrator",
                    Email = "admin@gmail.com",
                    CreatedBy = "admin",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    Status = Status.Active
                }, "12345678");
                var user = await _userManager.FindByNameAsync("admin");
                await _userManager.AddToRoleAsync(user, "Admin");
            }

            if (_context.Functions.Count() == 0)
            {
                _context.Functions.AddRange(new List<Function>()
                {
                    new Function() {Id = "SYSTEM", Name = "Hệ thống",ParentId = null,SortOrder = 1,Status = Status.Active,URL = "/",IconCss = "fa-desktop"  },
                    new Function() {Id = "ROLE", Name = "Nhóm",ParentId = "SYSTEM",SortOrder = 1,Status = Status.Active,URL = "/admin/role/index",IconCss = "fa-home"  },
                    new Function() {Id = "FUNCTION", Name = "Chức năng",ParentId = "SYSTEM",SortOrder = 2,Status = Status.Active,URL = "/admin/function/index",IconCss = "fa-home"  },
                    new Function() {Id = "USER", Name = "Người dùng",ParentId = "SYSTEM",SortOrder =3,Status = Status.Active,URL = "/admin/user/index",IconCss = "fa-home"  },
                    new Function() {Id = "ACTIVITY", Name = "Nhật ký",ParentId = "SYSTEM",SortOrder = 4,Status = Status.Active,URL = "/admin/activity/index",IconCss = "fa-home"  },
                    new Function() {Id = "ERROR", Name = "Lỗi",ParentId = "SYSTEM",SortOrder = 5,Status = Status.Active,URL = "/admin/error/index",IconCss = "fa-home"  },
                    new Function() {Id = "SETTING", Name = "Cấu hình",ParentId = "SYSTEM",SortOrder = 6,Status = Status.Active,URL = "/admin/setting/index",IconCss = "fa-home"  },
                    //new Function() {Id = "PRODUCT",Name = "Sản phẩm",ParentId = null,SorOrder = 2,Status = Status.Active,URL = "/",IconCss = "fa-chevron-down"  },
                    //new Function() {Id = "PRODUCT_CATEGORY",Name = "Danh mục",ParentId = "PRODUCT",SorOrder =1,Status = Status.Active,URL = "/admin/productcategory/index",IconCss = "fa-chevron-down"  },
                    //new Function() {Id = "PRODUCT_LIST",Name = "Sản phẩm",ParentId = "PRODUCT",SorOrder = 2,Status = Status.Active,URL = "/admin/product/index",IconCss = "fa-chevron-down"  },
                    //new Function() {Id = "BILL",Name = "Hóa đơn",ParentId = "PRODUCT",SorOrder = 3,Status = Status.Active,URL = "/admin/bill/index",IconCss = "fa-chevron-down"  },
                    new Function() {Id = "CONTENT",Name = "Nội dung",ParentId = null,SortOrder = 3,Status = Status.Active,URL = "/",IconCss = "fa-table"  },
                    new Function() {Id = "NEWS",Name = "Bài viết",ParentId = "CONTENT",SortOrder = 1,Status = Status.Active,URL = "/admin/news/index",IconCss = "fa-table"  },
                    new Function() {Id = "CATEGORY",Name = "Danh mục",ParentId = "CONTENT",SortOrder = 2,Status = Status.Active,URL = "/admin/category/index",IconCss = "fa-table"  },
                    new Function() {Id = "PAGE",Name = "Trang",ParentId = "CONTENT",SortOrder = 3,Status = Status.Active,URL = "/admin/page/index",IconCss = "fa-table"  },
                    new Function() {Id = "UTILITY",Name = "Tiện ích",ParentId = null,SortOrder = 4,Status = Status.Active,URL = "/",IconCss = "fa-clone"  },
                    new Function() {Id = "FOOTER",Name = "Footer",ParentId = "UTILITY",SortOrder = 1,Status = Status.Active,URL = "/admin/footer/index",IconCss = "fa-clone"  },
                    new Function() {Id = "FEEDBACK",Name = "Phản hồi",ParentId = "UTILITY",SortOrder = 2,Status = Status.Active,URL = "/admin/feedback/index",IconCss = "fa-clone"  },
                    new Function() {Id = "ANNOUNCEMENT",Name = "Thông báo",ParentId = "UTILITY",SortOrder = 3,Status = Status.Active,URL = "/admin/announcement/index",IconCss = "fa-clone"  },
                    new Function() {Id = "CONTACT",Name = "Liên hệ",ParentId = "UTILITY",SortOrder = 4,Status = Status.Active,URL = "/admin/contact/index",IconCss = "fa-clone"  },
                    new Function() {Id = "SLIDE",Name = "Slide",ParentId = "UTILITY",SortOrder = 5,Status = Status.Active,URL = "/admin/slide/index",IconCss = "fa-clone"  },
                    new Function() {Id = "ADVERTISMENT",Name = "Quảng cáo",ParentId = "UTILITY",SortOrder = 6,Status = Status.Active,URL = "/admin/advertistment/index",IconCss = "fa-clone"  },
                    new Function() {Id = "VIDEO",Name = "Video",ParentId = "UTILITY",SortOrder = 7,Status = Status.Active,URL = "/admin/video/index",IconCss = "fa-clone"  },

                    new Function() {Id = "REPORT",Name = "Báo cáo",ParentId = null,SortOrder = 5,Status = Status.Active,URL = "/",IconCss = "fa-bar-chart-o"  },
                    new Function() {Id = "REVENUES",Name = "Báo cáo doanh thu",ParentId = "REPORT",SortOrder = 1,Status = Status.Active,URL = "/admin/report/revenues",IconCss = "fa-bar-chart-o"  },
                    new Function() {Id = "ACCESS",Name = "Báo cáo truy cập",ParentId = "REPORT",SortOrder = 2,Status = Status.Active,URL = "/admin/report/visitor",IconCss = "fa-bar-chart-o"  },
                    new Function() {Id = "READER",Name = "Báo cáo độc giả",ParentId = "REPORT",SortOrder = 3,Status = Status.Active,URL = "/admin/report/reader",IconCss = "fa-bar-chart-o"  },
                });
            }
            if (_context.Customer.Count() == 0)
            {
                var user = await _userManager.FindByNameAsync("admin");
                _context.Customer.AddRange(new List<Customer>()
                {
                     new Customer(){FullName="Lê Đình Hiếu",CreatorId=user.Id,CreatorNote="Note cái nhẹ",Price=1000,DateSendByCustomer=DateTime.Now,PhoneNumber="039373309",CreatedBy="admin",DateCreated=DateTime.Now,Status=1},
                     new Customer(){FullName="Lê Đình 1",CreatorId=user.Id,CreatorNote="Note cái nhẹ0",Price=1000,DateSendByCustomer=DateTime.Now,PhoneNumber="039373309",CreatedBy="admin",DateCreated=DateTime.Now,Status=2},
                     new Customer(){FullName="Lê Đình 2",CreatorId=user.Id,CreatorNote="Note cái nhẹ1",Price=1000,DateSendByCustomer=DateTime.Now,PhoneNumber="039373309",CreatedBy="admin",DateCreated=DateTime.Now,Status=0},
                     new Customer(){FullName="Lê Đình 3",CreatorId=user.Id,CreatorNote="Note cái nhẹ2",Price=1000,DateSendByCustomer=DateTime.Now,PhoneNumber="039373309",CreatedBy="admin",DateCreated=DateTime.Now,Status=1},
                     new Customer(){FullName="Lê Đình 4",CreatorId=user.Id,CreatorNote="Note cái nhẹ3",Price=1000,DateSendByCustomer=DateTime.Now,PhoneNumber="039373309",CreatedBy="admin",DateCreated=DateTime.Now,Status=1},
                     new Customer(){FullName="Lê Đình 5",CreatorId=user.Id,CreatorNote="Note cái nhẹ4",Price=1000,DateSendByCustomer=DateTime.Now,PhoneNumber="039373309",CreatedBy="admin",DateCreated=DateTime.Now,Status=1},
                });

            }
        await _context.SaveChangesAsync();
        }
    }
}
