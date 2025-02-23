using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeeSystem.Students.Dto
{
    public class StudentFeeDto
    {
        public string StudentId { get; set; }
        public double AmountPerPeriod { get; set; }
        public double TotalAmount { get; set; }
        public double AdditionalFee { get; set; }
        public double Discount { get; set; }
        public double UpdatedTotalAmount { get; set; }
        public string PaymentPlanName { get; set; }

        public string FeeTypeName { get; set; } 
    }
}
