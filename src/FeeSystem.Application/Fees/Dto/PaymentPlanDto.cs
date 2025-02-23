using Abp.Application.Services.Dto;

namespace FeeSystem.Fees.Dto
{
    public class PaymentPlanDto : EntityDto<int>
    {
        public int PaymentPlanId { get; set; }
        public string PlanName { get; set; }
        public int IntervalInMonths { get; set; }
    }
}