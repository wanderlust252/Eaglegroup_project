using Eaglegroup_project.Data.Enums;
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

        public Customer(string fullName, string creatorNote, string saleNote, Guid? saleId, DateTime? deal, decimal price, DateTime? dateSendbycustomer, Guid creatorId,bool isDelete)
        {

            this.FullName = fullName;
            this.CreatorNote = creatorNote;
            this.SaleNote = saleNote;
            this.SaleId = saleId;
            this.Price = price;
            this.Deal = deal;
            this.DateSendByCustomer = dateSendbycustomer;
            this.CreatorId = creatorId;
            this.isDelete = isDelete;
        }

        public string FullName { get; set; }
        public string CreatorNote { get; set; }//note cua ng tao
        public Guid CreatorId { get; set; }//id cua nguoi tao
        public string SaleNote { get; set; }//note cua sale
        public Guid? SaleId { get; set; }//id sale
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
        [DefaultValue(false)]
        public bool isDelete { get; set; }
        public int Status { get; set; } //status chot don, chua chot,...
    }
}
