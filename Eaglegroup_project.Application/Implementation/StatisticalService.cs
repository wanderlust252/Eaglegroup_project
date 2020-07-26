using AutoMapper;
using Eaglegroup_project.Application.Interfaces;
using Eaglegroup_project.Data.Entities;
using Eaglegroup_project.Data.Enums;
using Eaglegroup_project.Data.IRepositories;
using Eaglegroup_project.Infrastructure.Interfaces;
using Eaglegroup_project.Utilities.Constants;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eaglegroup_project.Application.Implementation
{ 
    public class StatisticalService : IStatisticalService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StatisticalService(IMapper mapper, ICustomerRepository customerRepository, UserManager<AppUser> userManager,
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _customerRepository = customerRepository;
            _userManager = userManager;
            _mapper = mapper;
        }
        public decimal GetDealRateBySpecifyDate(DateTime specDate, Guid userName, int option, string[] role)
        {

            DateTime tempDate = new DateTime();
            if (specDate != DateTime.MinValue)
            {
                tempDate = specDate;
            }
            //if (option <= 0)
            //{
            //    return _customerRepository.FindAllAsNoTracking(x => x.isDelete != true  
            //    && x.DateCreated.Date == specDate.Date 
            //    ).Sum(e => e.Price);
            //}
            var query = _customerRepository.FindAllAsNoTracking(x => x.isDelete != true
                      && x.DateCreated.Date == specDate.Date);

            if (role.Contains(CommonConstants.AppRole.MarketingRole))
            {
                var allMar = query.Where(x => x.CreatorId == userName).ToList();
                var chotMar = allMar.Count(x => x.Status == (int)CustomerStatus.Confirm);
                return allMar.Count() <= 0 ? -1 : chotMar / allMar.Count() * 100;
            }
            else if (role.Contains(CommonConstants.AppRole.SaleStaffRole))
            {
                var allSale = query.Where(x => x.SaleId == userName).ToList();
                var chotSale = allSale.Count(x => x.Status == (int)CustomerStatus.Confirm);
                return allSale.Count() <= 0 ? -1 : chotSale / allSale.Count() * 100;
            }
            var chotAdmin = query.Count(x => x.Status == (int)CustomerStatus.Confirm);

            return query.Count() <= 0 ? -1 : chotAdmin / query.Count() * 100;
        }
        public decimal GetDealRate(DateTime from, DateTime to, Guid userName, int option, string[] role)
        {
            DateTime fromDate = new DateTime();
            DateTime toDate = new DateTime();
            if (from != DateTime.MinValue && to != DateTime.MinValue)
            {
                fromDate = from;
                toDate = to;
            }
            else
            {
                var date = DateTime.Today;
                fromDate = new DateTime(date.Year, date.Month, 1);
                toDate = fromDate.AddMonths(1).AddDays(-1);
            }
            var query = _customerRepository.FindAllAsNoTracking(x => x.isDelete != true
                      && (x.DateCreated.Date <= toDate.Date && x.DateCreated.Date >= fromDate.Date));

            if (role.Contains(CommonConstants.AppRole.MarketingRole))
            {
                var allMar = query.Where(x => x.CreatorId == userName).ToList();
                var chotMar = allMar.Count(x => x.Status == (int)CustomerStatus.Confirm);
                return allMar.Count() <= 0 ? -1 : chotMar * 100 / allMar.Count();
            }
            else if (role.Contains(CommonConstants.AppRole.SaleStaffRole))
            {
                var allSale = query.Where(x => x.SaleId == userName).ToList();
                var chotSale = allSale.Count(x => x.Status == (int)CustomerStatus.Confirm);
                return allSale.Count() <= 0 ? -1 : chotSale * 100 / allSale.Count()  ;
            }
            var chotAdmin = query.Count(x => x.Status == (int)CustomerStatus.Confirm);

            return query.Count() <= 0 ? -1 : chotAdmin * 100 / query.Count()  ;
        }
        public decimal GetStatiscalBySpecifyDate(DateTime specDate,Guid userName,int option,string[] role)
        {
            //string role = _userManager.Users.FirstOrDefault(x=>x.UserName==userName);
            //if (role.Contains(CommonConstants.AppRole.MarketingRole))
            //{

            //}
            DateTime tempDate = new DateTime(); 
            if (specDate != DateTime.MinValue)
            {
                tempDate = specDate;
            } 
            decimal query;
            //if (option <= 0)
            //{
            //    return _customerRepository.FindAllAsNoTracking(x => x.isDelete != true  
            //    && x.DateCreated.Date == specDate.Date 
            //    ).Sum(e => e.Price);
            //}
            if (role.Contains(CommonConstants.AppRole.MarketingRole))
            {
                query = _customerRepository.FindAllAsNoTracking(x => x.isDelete != true
                && x.Status == (int)CustomerStatus.Confirm
                && x.DateCreated.Date == specDate.Date
                && x.CreatorId.Equals(userName)
                ).Sum(e => e.Price);
            }
            else if (role.Contains(CommonConstants.AppRole.SaleStaffRole)) {
                query = _customerRepository.FindAllAsNoTracking(x => x.isDelete != true
                && option <= 0 ? true : (x.Status == option)
                && x.DateCreated.Date == specDate.Date
                && x.SaleId.Equals(userName)
                ).Sum(e => e.Price);
            }
            else
            {
                query = _customerRepository.FindAllAsNoTracking(x => x.isDelete != true && option <= 0 ? true : (x.Status == option) && x.DateCreated.Date == specDate.Date ).Sum(e => e.Price);
            }


            return query;
        }
        public decimal GetStatiscal(DateTime from , DateTime to, Guid userName, int option, string[] role)
        {
            DateTime fromDate = new DateTime();
            DateTime toDate = new DateTime();
            if (from != DateTime.MinValue && to != DateTime.MinValue)
            {
                fromDate = from;
                toDate = to;
            }
            else
            {
                var date = DateTime.Today;
                fromDate = new DateTime(date.Year, date.Month, 1);
                toDate = fromDate.AddMonths(1).AddDays(-1);
            }
            decimal query; 
                if (role.Contains(CommonConstants.AppRole.MarketingRole))
                {
                    query = _customerRepository.FindAllAsNoTracking(x => x.isDelete != true 
                    && (x.DateCreated.Date <= toDate.Date && x.DateCreated.Date >= fromDate.Date) 
                    && x.Status == (int)CustomerStatus.Confirm
                    && x.CreatorId.Equals(userName)).Sum(e => e.Price);
                }
                else if (role.Contains(CommonConstants.AppRole.SaleStaffRole))
                {
                    query = _customerRepository.FindAllAsNoTracking(x => x.isDelete != true
                    && (x.DateCreated.Date <= toDate.Date && x.DateCreated.Date >= fromDate.Date)
                    && x.Status == (int)CustomerStatus.Confirm
                    && x.SaleId.Equals(userName)).Sum(e => e.Price);
                }
                    query = _customerRepository.FindAllAsNoTracking(x => x.isDelete != true
                    && (x.DateCreated.Date <= toDate.Date && x.DateCreated.Date >= fromDate.Date)
                    && x.Status == (int)CustomerStatus.Confirm).Sum(e => e.Price);

            return query;

            
        }
    }
}
