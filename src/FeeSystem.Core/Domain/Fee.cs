using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using FeeManagementSystem.Classes;
using FeeManagementSystem.FeeTypes;
using FeeManagementSystem.PaymentPlans;
using FeeManagementSystem.Payments;
using FeeManagementSystem.StudentFees;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FeeManagementSystem.Fees
{
    public class Fee : Entity<int>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        [Key]
        public int FeeId { get; set; }

        [Required]
        public double Amount { get; set; }

        public int ClassId { get; set; }
        public int FeeTypeId { get; set; }
        public int PaymentPlanId { get; set; }

        [ForeignKey("ClassId")]
        public virtual Class Class { get; set; }
        [ForeignKey("FeeTypeId")]
        public virtual FeeType FeeType { get; set; }
        [ForeignKey("PaymentPlanId")]
        public virtual PaymentPlan PaymentPlan { get; set; }
        public virtual ICollection<StudentFee> StudentFees { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
