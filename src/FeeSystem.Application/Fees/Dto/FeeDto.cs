using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeeSystem.Fees.Dto
{
    public class FeeDto : EntityDto<int>
    {
        public int FeeId { get; set; }
        public string FeeHeading { get; set; }
        public double Amount { get; set; }
        public string FeeTypeName { get; set; }
        public string ClassName { get; set; }
        public int ClassId { get; set; }
        public int FeeTypeId { get; set; }
        public string PlanName { get; set; }

    }
}
