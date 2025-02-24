using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using FeeManagementSystem.Fees;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FeeManagementSystem.FeeTypes
{
    [Audited]
    public class FeeType : Entity<int>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        [Key]
        public int FeeTypeId { get; internal set; }

        [Required]
        public string FeeTypeName { get; set; }
        public string NormalizedFeeTypeName { get; set; }

        public virtual ICollection<Fee> Fees { get; set; }
    }
}
