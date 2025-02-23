using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using FeeManagementSystem.Fees;
using FeeManagementSystem.StudentFees;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace FeeManagementSystem.PaymentPlans
{
    public class PaymentPlan : Entity<int>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        [Key]
        public int PaymentPlanId { get; set; }

        [Required]
        public string PlanName { get; set; }
        
        public string NormalizedPlanName { get; set; }

        [Required]
        public int IntervalInMonths { get; set; }

        public virtual ICollection<Fee> Fees { get; set; }
        public virtual ICollection<StudentFee> StudentFees { get; set; }
    }
}
