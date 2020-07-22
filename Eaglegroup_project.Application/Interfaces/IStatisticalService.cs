using System;
using System.Collections.Generic;
using System.Text;

namespace Eaglegroup_project.Application.Interfaces
{ 
    public interface IStatisticalService
    {
        decimal GetDealRate(DateTime fromDate, DateTime toDate, Guid userName, int option, string[] role); //GetStatiscalBySpecifyDate
        decimal GetDealRateBySpecifyDate(DateTime specDate, Guid userName, int option, string[] role);
        decimal GetStatiscal(DateTime fromDate, DateTime toDate, Guid userName, int option, string[] role); //GetStatiscalBySpecifyDate
        decimal GetStatiscalBySpecifyDate(DateTime specDate,Guid userName, int option,string[] role);
    }
}
