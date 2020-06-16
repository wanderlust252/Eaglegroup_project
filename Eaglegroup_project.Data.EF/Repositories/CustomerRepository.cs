using Eaglegroup_project.Data.Entities;
using Eaglegroup_project.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eaglegroup_project.Data.EF.Repositories
{
    public class CustomerRepository : EFRepository<Customer, int> , ICustomerRepository
    {
        public CustomerRepository(AppDbContext context) : base(context)
        {

        } 
    }
}
