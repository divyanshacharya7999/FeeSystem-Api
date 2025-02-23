namespace FeeSystem.Fees.Dto
{
    public class CreatePaymentPlanDto
    {
        public string PlanName { get; set; }
        public int IntervalInMonths { get; set; }
    }
}