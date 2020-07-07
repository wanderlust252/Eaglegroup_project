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
        void Add(CustomerViewModel function, int checkR, Guid userAdd);

        Task<List<CustomerViewModel>> GetAll(string filter);
        CustomerViewModel GetCustomerForSale(int checkR, Guid userGet);
        Task<List<CustomerViewModel>> GetAllForAppoint();

        PagedResult<CustomerViewModel> GetAllPaging(string keyword, int page, int pageSize,int checkR, Guid userGet);

        CustomerViewModel GetById(int id, int checkR, Guid userGet);
        void AppointCustomer(List<int> listId, Guid userId);
        int statistical(DateTime fromDate, DateTime toDate);
        void Update(CustomerViewModel function, int checkR, Guid userUpdate);

        void Delete(int id, int checkR, Guid userDelete);

        void Save();

    }
}
