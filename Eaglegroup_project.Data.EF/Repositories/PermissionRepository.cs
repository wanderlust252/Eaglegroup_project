using Eaglegroup_project.Data.Entities;
using Eaglegroup_project.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eaglegroup_project.Data.EF.Repositories
{
    public class PermissionRepository : EFRepository<Permission, int>, IPermissionRepository
    {
        public PermissionRepository(AppDbContext context) : base(context)
        {
        }
    }
}
