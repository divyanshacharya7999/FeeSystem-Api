using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeeSystem.Fees.Dto
{
    public class UpdateFeeTypeDto : CreateFeeTypeDto
    {
        public int FeeTypeId { get; set; }
    }
}
