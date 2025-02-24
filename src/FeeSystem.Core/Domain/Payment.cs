using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using FeeManagementSystem.Fees;
using FeeManagementSystem.Students;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FeeManagementSystem.Payments
{
    [Audited]
    public class Payment : Entity<long>,IMustHaveTenant
    {
        public int TenantId { get; set; }
        [Key]
        public long PaymentId { get; set; }

        public string StudentId { get; set; }
        public int FeeId { get; set; }

        [Required]
        public double AmountPaid { get; set; }

        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public int Discount { get; set; }
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }
        [ForeignKey("FeeId")]
        public virtual Fee Fee { get; set; }
    }
}
