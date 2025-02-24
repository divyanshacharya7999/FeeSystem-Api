using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using FeeManagementSystem.Fees;
using FeeManagementSystem.PaymentPlans;
using FeeManagementSystem.Students;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FeeManagementSystem.StudentFees
{
    [Audited]
    public class StudentFee : Entity<int>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        [Key]
        public int StudentFeeId { get; set; }

        public int FeeId { get; set; }
        public string StudentId { get; set; }
        public int PaymentPlanId { get; set; }

        [Required]
        public double AmountPerPeriod { get; set; }
        [DefaultValue(0)]
        public double AdditionalFee { get; set; }
        
        [DefaultValue(0)]
        public double Discount { get; set; }
        [Required]
        public double TotalAmount { get; set; }
        
        [Required]
        public double UpdatedTotalAmount { get; set; }

        public DateTime EffectiveFrom { get; set; }

        [ForeignKey("FeeId")]
        public virtual Fee Fee { get; set; }
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }
        [ForeignKey("PaymentPlanId")]
        public virtual PaymentPlan PaymentPlan { get; set; }
    }
}
