using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eaglegroup_project.Application.Interfaces;
using Eaglegroup_project.Application.ViewModels.System;
using Eaglegroup_project.Data.Entities;
using Eaglegroup_project.Data.Enums;
using Eaglegroup_project.Data.IRepositories;
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
        private ICustomerRepository _customerRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(IMapper mapper, ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public void Add(CustomerViewModel customerVm, int checkR, Guid userAdd)
        {
            if (checkR == 0 || checkR == 1)
            {
                var customer = _mapper.Map<Customer>(customerVm);
                _customerRepository.Add(customer);
            }

        }

        public Task<List<CustomerViewModel>> GetAll(string filter)
        {
            var query = _customerRepository.FindAll();
            if (!string.IsNullOrEmpty(filter))
                query = query.Where(x => x.FullName.Contains(filter));
            return query.OrderBy(x => x.DateCreated).ProjectTo<CustomerViewModel>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public CustomerViewModel GetById(int id, int checkR, Guid userGet)
        {
            var query = (checkR == 1) ? _customerRepository.FindAllAsNoTracking(x => x.CreatorId.Equals(userGet))
                : (checkR == 2) ? _customerRepository.FindAllAsNoTracking(x => x.SaleId.Equals(userGet))
                : _customerRepository.FindAllAsNoTracking();

            var customer = _customerRepository.FindSingle(x => x.Id == id);
            return _mapper.Map<Customer, CustomerViewModel>(customer);
        }

        public void Update(CustomerViewModel customerVm, int checkR, Guid userUpdate)
        {
            if (checkR == 2)
            {
                customerVm.SaleId = userUpdate;
                var customer = _mapper.Map<CustomerViewModel, Customer>(customerVm);
                _customerRepository.Update(customer); 
            }
            else
            {
                //var customerDb = _customerRepository.FindById(customerVm.Id.Value);
                customerVm.SaleId = userUpdate;
                var customer = _mapper.Map<CustomerViewModel, Customer>(customerVm);
                _customerRepository.Update(customer);
            }
        }

        public void Delete(int id, int checkR, Guid userDelete)
        {
            var query = (checkR == 1) ? _customerRepository.FindAllAsNoTracking(x => x.CreatorId.Equals(userDelete))
               : (checkR == 2) ? _customerRepository.FindAllAsNoTracking(x => x.SaleId.Equals(userDelete))
               : _customerRepository.FindAllAsNoTracking();

            var customer = query.Where(x => x.Id == id).FirstOrDefault();
            customer.isDelete = true;
            _customerRepository.Update(customer);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public PagedResult<CustomerViewModel> GetAllPaging(string keyword, int page, int pageSize, int checkR, Guid userGet)
        {
            var query = (checkR == 1) ? _customerRepository.FindAllAsNoTracking(x => x.CreatorId.Equals(userGet) && x.isDelete == false)
                : (checkR == 2) ? _customerRepository.FindAllAsNoTracking(x => x.SaleId.Equals(userGet) && x.isDelete == false)
                : _customerRepository.FindAllAsNoTracking(x => x.isDelete == false);
            //neu marketing thi where theo marketing 
            //checkR 1 la marketing

            //neu sale thi where theo sale 
            //checkR 2 la sale

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.FullName.Contains(keyword)
                || x.FullName.Contains(keyword));

            int totalRow = query.Count();

            query = query.Skip((page - 1) * pageSize)
               .Take(pageSize);

            var data = new List<CustomerViewModel>();

            data = query.Select(x => new CustomerViewModel()
            {
                FullName = x.FullName,
                Id = x.Id,
                PhoneNumber = x.PhoneNumber,
                Status = x.Status,
                DateCreated = x.DateCreated,
                CreatedBy = x.CreatedBy,
                CreatorId = x.CreatorId,
                CreatorNote = x.CreatorNote,
                DateSendByCustomer = x.DateSendByCustomer,
                SaleId = x.SaleId.GetValueOrDefault(),
                SaleNote = x.SaleNote,
                DateModified = x.DateModified,
                Deal = x.Deal,
                ModifiedBy = x.ModifiedBy,
                Price = x.Price
            }).ToList();

            data = query.Select(x => new CustomerViewModel()
            {
                FullName = x.FullName,
                Id = x.Id,
                PhoneNumber = x.PhoneNumber,
                Status = x.Status,
                DateCreated = x.DateCreated,
                CreatedBy = x.CreatedBy,
                CreatorId = x.CreatorId,
                CreatorNote = x.CreatorNote,
                DateSendByCustomer = x.DateSendByCustomer,
                SaleId = x.SaleId.GetValueOrDefault(),
                SaleNote = x.SaleNote,
                DateModified = x.DateModified,
                Deal = x.Deal,
                ModifiedBy = x.ModifiedBy,
                Price = x.Price
            }).ToList();

            var countCustomer = _customerRepository.FindAllAsNoTracking().Where(x => x.SaleId == Guid.Empty).Count();

            var paginationSet = new PageResultCustomer<CustomerViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize,
                TotalCustomer = countCustomer
            };

            return paginationSet;
        }

        public CustomerViewModel GetCustomerForSale(int checkR, Guid userId)
        {
            if (checkR == 2)
            {
                var query = _customerRepository.FindAllAsNoTracking();
                var randomCustomer = query.Where(x => x.SaleId==Guid.Empty).FirstOrDefault();
                if (randomCustomer != null)
                {
                    randomCustomer.SaleId = userId;//chua duoc gan' luon
                    randomCustomer.Status = 3;//pending
                    randomCustomer.SaleId = userId;
                    _customerRepository.Update(randomCustomer);
                    Save();
                    //randomCustomer.FullName = string.Empty;
                    return _mapper.Map<Customer, CustomerViewModel>(randomCustomer);
                }

            }

            return null;
        }
    }
}
