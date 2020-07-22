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
            //string role = _userManager.Users.FirstOrDefault(x=>x.UserName==userName);
            //if (role.Contains(CommonConstants.AppRole.MarketingRole))
            //{

            //}
            DateTime tempDate = new DateTime();
            if (specDate != DateTime.MinValue)
            {
                tempDate = specDate;
            }
            var query=_customerRepository.FindAllAsNoTracking(x => x.isDelete != true
                && x.DateCreated.Date == specDate.Date
                && x.CreatorId.Equals(userName));
            
            if (role.Contains(CommonConstants.AppRole.MarketingRole))
            { 
                int sumDeal = query.Count(x=>x.Status= CustomerStatus.ClosedDeal)
                

            }
            else if (role.Contains(CommonConstants.AppRole.SaleStaffRole))
            { 
            }
            else
            { }


            return 0;
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
            decimal query;
            if (option > 0)
            {
                if (role.Contains(CommonConstants.AppRole.MarketingRole))
                {
                    query = _customerRepository.FindAllAsNoTracking(x => x.isDelete != true
                    && (x.DateCreated.Date <= toDate.Date && x.DateCreated.Date >= fromDate.Date)
                    && x.Status == option
                    && x.CreatorId.Equals(userName)).Sum(e => e.Price);
                }
                else if (role.Contains(CommonConstants.AppRole.SaleStaffRole))
                {
                    query = _customerRepository.FindAllAsNoTracking(x => x.isDelete != true
                    && (x.DateCreated.Date <= toDate.Date && x.DateCreated.Date >= fromDate.Date)
                    && x.Status == option
                    && x.SaleId.Equals(userName)).Sum(e => e.Price);
                }
                else
                {
                    query = _customerRepository.FindAllAsNoTracking(x => x.isDelete != true
                    && (x.DateCreated.Date <= toDate.Date && x.DateCreated.Date >= fromDate.Date)
                    && x.Status == option).Sum(e => e.Price);
                }


            }
            else
            {
                query = _customerRepository.FindAllAsNoTracking(x => x.isDelete != true
                && (x.DateCreated.Date <= toDate.Date && x.DateCreated.Date >= fromDate.Date)).Sum(e => e.Price);
            }

            return query;
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
            if (option <= 0)
            {
                return _customerRepository.FindAllAsNoTracking(x => x.isDelete != true  
                && x.DateCreated.Date == specDate.Date 
                ).Sum(e => e.Price);
            }
            if (role.Contains(CommonConstants.AppRole.MarketingRole))
            {
                query = _customerRepository.FindAllAsNoTracking(x => x.isDelete != true
                && x.Status == option
                && x.DateCreated.Date == specDate.Date
                && x.CreatorId.Equals(userName)
                ).Sum(e => e.Price);
            }
            else if (role.Contains(CommonConstants.AppRole.SaleStaffRole)) {
                query = _customerRepository.FindAllAsNoTracking(x => x.isDelete != true
                && x.Status == option
                && x.DateCreated.Date == specDate.Date
                && x.SaleId.Equals(userName)
                ).Sum(e => e.Price);
            }
            else
            {
                query = _customerRepository.FindAllAsNoTracking(x => x.isDelete != true && x.Status == option && x.DateCreated.Date == specDate.Date ).Sum(e => e.Price);
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
            if (option > 0)
            {
                if (role.Contains(CommonConstants.AppRole.MarketingRole))
                {
                    query = _customerRepository.FindAllAsNoTracking(x => x.isDelete != true 
                    && (x.DateCreated.Date <= toDate.Date && x.DateCreated.Date >= fromDate.Date) 
                    && x.Status == option
                    && x.CreatorId.Equals(userName)).Sum(e => e.Price);
                }
                else if (role.Contains(CommonConstants.AppRole.SaleStaffRole))
                {
                    query = _customerRepository.FindAllAsNoTracking(x => x.isDelete != true
                    && (x.DateCreated.Date <= toDate.Date && x.DateCreated.Date >= fromDate.Date)
                    && x.Status == option
                    && x.SaleId.Equals(userName)).Sum(e => e.Price);
                }
                else
                {
                    query = _customerRepository.FindAllAsNoTracking(x => x.isDelete != true
                    && (x.DateCreated.Date <= toDate.Date && x.DateCreated.Date >= fromDate.Date)
                    && x.Status == option).Sum(e => e.Price);
                }
                

            }
            else {
                query = _customerRepository.FindAllAsNoTracking(x => x.isDelete != true 
                && (x.DateCreated.Date <= toDate.Date && x.DateCreated.Date >= fromDate.Date)).Sum(e => e.Price);
            }

            return query;
        }
    }
}
