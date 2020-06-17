using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eaglegroup_project.Application.Interfaces;
using Eaglegroup_project.Application.ViewModels.System;
using Eaglegroup_project.Data.Entities;
using Eaglegroup_project.Data.Enums;
using Eaglegroup_project.Infrastructure.Interfaces;
using Eaglegroup_project.Utilities.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eaglegroup_project.Application.Implementation
{
    public class CustomerService : ICustomerService
    {
        private IRepository<Customer, int> _customerRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(IMapper mapper, IRepository<Customer, int> customerRepository,
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public void Add(CustomerViewModel customerVm)
        {
            var customer = _mapper.Map<Customer>(customerVm);
            _customerRepository.Add(customer);
        }

        public Task<List<CustomerViewModel>> GetAll(string filter)
        {
            var query = _customerRepository.FindAll();
            if (!string.IsNullOrEmpty(filter))
                query = query.Where(x => x.FullName.Contains(filter));
            return query.OrderBy(x => x.DateCreated).ProjectTo<CustomerViewModel>(_mapper.ConfigurationProvider).ToListAsync();
        }
        public List<CustomerViewModel> GetByMarketingId(Guid id)
        {
            var customer = _customerRepository.FindAllAsNoTracking(x => x.CreatorId.Equals(id)).ProjectTo<CustomerViewModel>(_mapper.ConfigurationProvider);
            return customer.ToList();
        }
        public List<CustomerViewModel> GetByStaffId(Guid id)
        {
            var customer = _customerRepository.FindAllAsNoTracking(x => x.CreatorId.Equals(id)).ProjectTo<CustomerViewModel>(_mapper.ConfigurationProvider);
            return customer.ToList();
        }
        public CustomerViewModel GetById(int id)
        {
            var customer = _customerRepository.FindSingle(x => x.Id == id);
            return _mapper.Map<Customer, CustomerViewModel>(customer);
        }

        public void Update(CustomerViewModel customerVm)
        {
            var customerDb = _customerRepository.FindById(customerVm.Id);
            var customer = _mapper.Map<CustomerViewModel,Customer>(customerVm);
            _customerRepository.Update(customer);
        }

        public void Delete(int id)
        {
            _customerRepository.Remove(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
