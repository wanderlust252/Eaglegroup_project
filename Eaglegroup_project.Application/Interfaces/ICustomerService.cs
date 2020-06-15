using Eaglegroup_project.Application.ViewModels.System;
using Eaglegroup_project.Utilities.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Eaglegroup_project.Application.Interfaces
{
    public interface ICustomerService
    {
        void Add(CustomerViewModel function);

        Task<List<CustomerViewModel>> GetAll(string filter);

        CustomerViewModel GetById(int id);

        void Update(CustomerViewModel function);

        void Delete(int id);

        void Save();

    }
}
