using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using FeeManagementSystem.Fees;
using FeeManagementSystem.Students;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FeeManagementSystem.Classes
{
    public class Class : Entity<int>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        [Key]
        public int ClassId { get; set; }

        [Required]
        public string ClassName { get; set; }

        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Fee> Fees { get; set; }
    }
}
