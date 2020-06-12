using Eaglegroup_project.Data.Entities;
using Eaglegroup_project.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eaglegroup_project.Data.IRepositories
{
    public interface IPermissionRepository : IRepository<Permission, int>
    {
    }
}
