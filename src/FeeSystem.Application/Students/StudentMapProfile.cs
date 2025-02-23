using AutoMapper;
using FeeManagementSystem.Classes;
using FeeManagementSystem.Classes.Dto;
using FeeManagementSystem.Fees;
using FeeManagementSystem.FeeTypes;
using FeeManagementSystem.PaymentPlans;
using FeeManagementSystem.Payments;
using FeeManagementSystem.StudentFees;
using FeeManagementSystem.Students;
using FeeSystem.Fees.Dto;
using FeeSystem.Students.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeeSystem.Students
{
    public class StudentMapProfile : Profile
    {
        public StudentMapProfile()
        {
            // Student mappings
            CreateMap<CreateStudentDto, Student>();
            CreateMap<Student, StudentDto>()
                .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.StudentId))
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class.ClassName));

            CreateMap<StudentDto, Student>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
            CreateMap<UpdateStudentDto, Student>();

            // Class mappings
            // Map from Class to ClassDto
            CreateMap<Class, ClassDto>() // Map Entity to DTO
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ClassId))
               .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.ClassName));

            CreateMap<ClassDto, Class>() // Map DTO to Entity
                .ForMember(dest => dest.TenantId, opt => opt.Ignore())
                .ForMember(dest => dest.ClassId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.ClassName));

            CreateMap<CreateClassDto, Class>() // For CreateClassAsync
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.ClassName));

            CreateMap<Class, CreateClassDto>()
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.ClassName));

            // ✅ Added Mapping: UpdateClassDto to Class for UpdateClassAsync
            CreateMap<UpdateClassDto, Class>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.ClassName));


            CreateMap<StudentFee, StudentFeeDto>()
            .ForMember(dest => dest.AmountPerPeriod, opt => opt.MapFrom(src => src.AmountPerPeriod))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
            .ForMember(dest => dest.PaymentPlanName, opt => opt.MapFrom(src => src.PaymentPlan.PlanName))
            .ForMember(dest => dest.FeeTypeName, opt => opt.MapFrom(src => src.Fee.FeeType.FeeTypeName));

            //// Fee mappings
            //CreateMap<Fee, FeeDto>();
            //CreateMap<CreateUpdateFeeDto, Fee>();

            //// Payment mappings
            //CreateMap<Payment, PaymentDto>();
            //CreateMap<CreateUpdatePaymentDto, Payment>();

            //// FeeType mappings
            //CreateMap<FeeType, FeeTypeDto>();
            //CreateMap<CreateUpdateFeeTypeDto, FeeType>();

            //// PaymentPlan mappings
            //CreateMap<PaymentPlan, PaymentPlanDto>();
            //CreateMap<CreateUpdatePaymentPlanDto, PaymentPlan>();
        }
    }
}
