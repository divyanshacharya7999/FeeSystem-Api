using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using FeeManagementSystem.Classes;
using FeeManagementSystem.Payments;
using FeeManagementSystem.StudentFees;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FeeManagementSystem.Students
{
    [Audited]
    public class Student : Entity<int>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        [Key]
        [Required]
        public string StudentId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        public int ClassId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        
        public string ContactNumber { get; set; }

        [ForeignKey("ClassId")]
        public virtual Class Class { get; set; }
        public virtual ICollection<StudentFee> StudentFees { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
