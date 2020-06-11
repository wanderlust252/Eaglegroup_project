﻿using Eaglegroup_project.Data.Enums;
using Eaglegroup_project.Data.Interfaces;
using Eaglegroup_project.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Eaglegroup_project.Data.Entities
{ 
    [Table("Customer")]
    public class Customer : DomainEntity<int>, IDateTracking
    {
        public Customer()
        {

        }

        public Customer(DateTime? dateModified, string modifiedBy, int status)
        {
            DateModified = dateModified;
            ModifiedBy = modifiedBy; 
            Status = status;
        }

        public string FullName { get; set; }
        public string CreatorNote { get; set; }//note cua ng tao
        public string StaffNote { get; set; }//note cua sale
        public Guid StaffId { get; set; }//id sale
        public DateTime? Deal { get; set; }//thoi gian chot deal
        [DefaultValue(0)]
        public decimal Price { get; set; }//don gia
        public DateTime? DateSendByCustomer { get; set; }//thoi gian khach hang gui sdt
        public string PhoneNumber { get; set; }//sdt
        [StringLength(50)]
        public string CreatedBy { get; set; }//nguoi tao ban ghi

        public DateTime DateCreated { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? DateModified { get; set; }

        public int Status { get; set; } //status chot don, chua chot,...
    }
}