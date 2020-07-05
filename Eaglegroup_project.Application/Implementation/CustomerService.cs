using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eaglegroup_project.Application.Interfaces;
using Eaglegroup_project.Application.ViewModels.System;
using Eaglegroup_project.Data.Entities;
using Eaglegroup_project.Data.Enums;
using Eaglegroup_project.Data.IRepositories;
using Eaglegroup_project.Infrastructure.Interfaces;
using Eaglegroup_project.Utilities.DTO;
using Microsoft.AspNetCore.Identity;
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
        private readonly ICustomerRepository _customerRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(IMapper mapper, ICustomerRepository customerRepository, UserManager<AppUser> userManager,
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _customerRepository = customerRepository;
            _userManager = userManager;
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
                //customerVm.SaleId = userUpdate;
                var customerDb = _customerRepository.FindById(customerVm.Id.Value);
                //customerDb.SaleId = customerVm.SaleId;
                customerDb.Deal = customerVm.Deal;
                customerDb.SaleNote = customerVm.SaleNote;
                customerDb.DateSendByCustomer = customerVm.DateSendByCustomer;
                customerDb.Price = customerVm.Price;
                customerDb.Status = customerVm.Status;
                //customerDb.DateCreated = customerVm.DateCreated;
                //customerDb.CreatedBy = customerVm.CreatedBy;
                //customerDb.Status = customerVm.Status;
                //customerDb.isDelete = customerVm.isDelete;
                //customerDb.CreatorId = customerVm.CreatorId;
                //var customer = _mapper.Map<CustomerViewModel, Customer>(customerVm);
                _customerRepository.Update(customerDb); 
            }
            else if(checkR==1)
            {
                var customerDb = _customerRepository.FindById(customerVm.Id.Value);
                customerDb.CreatorNote = customerVm.CreatorNote;
                customerDb.DateSendByCustomer = customerVm.DateSendByCustomer;
                customerDb.PhoneNumber = customerVm.PhoneNumber;
                customerDb.FullName = customerVm.FullName;
                //var customer = _mapper.Map<CustomerViewModel, Customer>(customerVm);
                _customerRepository.Update(customerDb);
            }
            else
            {
                var customerDb = _customerRepository.FindById(customerVm.Id.Value);
                customerDb.CreatorNote = customerVm.CreatorNote;
                customerDb.DateSendByCustomer = customerVm.DateSendByCustomer;
                customerDb.PhoneNumber = customerVm.PhoneNumber;
                customerDb.FullName = customerVm.FullName;
                customerDb.Deal = customerVm.Deal;
                customerDb.SaleNote = customerVm.SaleNote;
                customerDb.Price = customerVm.Price;
                //var customer = _mapper.Map<CustomerViewModel, Customer>(customerVm);
                _customerRepository.Update(customerDb);
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
            var queryAppUser = _userManager.Users.ToList();

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
                CreatorName=queryAppUser.Where(s=>s.Id==x.CreatorId).SingleOrDefault().FullName,
                CreatorId = x.CreatorId,
                CreatorNote = x.CreatorNote,
                DateSendByCustomer = x.DateSendByCustomer,
                SaleId = x.SaleId.GetValueOrDefault(),
                SaleName = queryAppUser.Where(s => s.Id == x.SaleId).SingleOrDefault().FullName,
                SaleNote = x.SaleNote,
                DateModified = x.DateModified,
                Deal = x.Deal,
                ModifiedBy = x.ModifiedBy,
                Price = x.Price
            }).ToList();

            var countCustomer = _customerRepository.FindAllAsNoTracking().Where(x => (x.SaleId == Guid.Empty || x.SaleId == null) && x.isDelete==false).Count();

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
                var query = _customerRepository.FindAllAsNoTracking(x=>x.isDelete==false);
                var randomCustomer = query.Where(x => x.SaleId == Guid.Empty || x.SaleId == null).FirstOrDefault();
                if (randomCustomer != null)
                {
                    //randomCustomer.SaleId = userId;//chua duoc gan' luon
                    randomCustomer.Status = 3;//pending
                    randomCustomer.SaleId = userId;
                    //randomCustomer.SaleId
                    _customerRepository.Update(randomCustomer);
                    Save();
                    //CustomerViewModel resCus = _mapper.Map<Customer, CustomerViewModel>(randomCustomer);
                    //resCus.SaleName=User
                    //randomCustomer.FullName = string.Empty;
                    return _mapper.Map<Customer, CustomerViewModel>(randomCustomer);
                }

            }

            return null;
        }
    }
}
