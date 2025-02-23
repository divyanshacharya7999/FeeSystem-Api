using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using FeeSystem.Authorization.Roles;
using FeeSystem.Authorization.Users;
using FeeSystem.MultiTenancy;
using FeeManagementSystem.Classes;
using FeeManagementSystem.Fees;
using FeeManagementSystem.FeeTypes;
using FeeManagementSystem.PaymentPlans;
using FeeManagementSystem.Payments;
using FeeManagementSystem.StudentFees;
using FeeManagementSystem.Students;
using System.Reflection;

namespace FeeSystem.EntityFrameworkCore
{
    public class FeeSystemDbContext : AbpZeroDbContext<Tenant, Role, User, FeeSystemDbContext>
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<FeeType> FeeTypes { get; set; }
        public DbSet<PaymentPlan> PaymentPlans { get; set; }
        public DbSet<Fee> Fees { get; set; }
        public DbSet<StudentFee> StudentFees { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public FeeSystemDbContext(DbContextOptions<FeeSystemDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            //setting contact number Unique
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.ContactNumber)
                .IsUnique();
            // Apply entity configurations
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Configure relationships to avoid multiple cascade paths
            modelBuilder.Entity<Class>(entity =>
            {
                // Ensure ClassId is the primary key and uses identity generation
                entity.HasKey(e => e.ClassId);
                entity.Property(e => e.ClassId).ValueGeneratedOnAdd(); // Identity generation

            });

            // Student to Class
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Class)
                .WithMany(c => c.Students)
                .HasForeignKey(s => s.ClassId);

            // Fee to Class (Cascade Delete)
            modelBuilder.Entity<Fee>()
                .HasOne(f => f.Class)
                .WithMany(c => c.Fees)
                .HasForeignKey(f => f.ClassId)
                .OnDelete(DeleteBehavior.Cascade);

            // StudentFee to Student (Cascade Delete)
            modelBuilder.Entity<StudentFee>()
                .HasOne(sf => sf.Student)
                .WithMany(s => s.StudentFees)
                .HasForeignKey(sf => sf.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            // StudentFee to Fee (Restrict Delete to avoid cascade path conflicts)
            modelBuilder.Entity<StudentFee>()
                .HasOne(sf => sf.Fee)
                .WithMany(f => f.StudentFees)
                .HasForeignKey(sf => sf.FeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // StudentFee to PaymentPlan (Restrict Delete to avoid cascade path conflicts)
            modelBuilder.Entity<StudentFee>()
                .HasOne(sf => sf.PaymentPlan)
                .WithMany(pp => pp.StudentFees)
                .HasForeignKey(sf => sf.PaymentPlanId)
                .OnDelete(DeleteBehavior.Restrict);

            // Payment to Student (Cascade Delete)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Student)
                .WithMany(s => s.Payments)
                .HasForeignKey(p => p.StudentId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            // Payment to Fee (Restrict Delete to avoid cascade path conflicts)
            modelBuilder.Entity<Payment>()
            .HasOne(p => p.Fee)
            .WithMany(f => f.Payments)
            .HasForeignKey(p => p.FeeId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
        }
    }
}