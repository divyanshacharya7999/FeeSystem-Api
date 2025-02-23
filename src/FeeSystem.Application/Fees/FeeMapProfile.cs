using AutoMapper;
using FeeManagementSystem.Fees;
using FeeManagementSystem.FeeTypes;
using FeeManagementSystem.PaymentPlans;
using FeeManagementSystem.Payments;
using FeeSystem.Fees.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeeSystem.Fees
{
  
    public class FeeMapProfile : Profile
    {
        public FeeMapProfile()
        {
            // Fee mappings
            CreateMap<Fee, FeeDto>()
            .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class.ClassName))
            .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.PaymentPlan.PlanName))
            .ForMember(dest => dest.FeeHeading, opt => opt.MapFrom(src => src.FeeType.FeeTypeName)); 


            // FeeType mappings
            CreateMap<CreateFeeTypeDto, FeeType>()
                .ForMember(dest => dest.TenantId, opt => opt.Ignore()) 
                .ForMember(dest => dest.FeeTypeName, opt => opt.MapFrom(src => src.FeeTypeName));
            CreateMap<FeeType, FeeTypeDto>();
            CreateMap<UpdateFeeTypeDto, FeeType>();

            // PaymentPlan mappings
            CreateMap<PaymentPlan, PaymentPlanDto>();
            CreateMap<CreatePaymentPlanDto, PaymentPlan>()
                .ForMember(dest => dest.TenantId, opt => opt.Ignore())
                .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.PlanName))
                .ForMember(dest => dest.IntervalInMonths, opt => opt.MapFrom(src => src.IntervalInMonths));
            CreateMap<UpdatePaymentPlanDto, PaymentPlan>();

            CreateMap<CreateOrUpdateFeeInput, Fee>()
    .ForMember(dest => dest.TenantId, opt => opt.Ignore()) // If TenantId is set elsewhere
    .ForMember(dest => dest.FeeId, opt => opt.Ignore());   // Ignore FeeId if auto-generated

        }
    }
}
