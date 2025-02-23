using Abp.Application.Services.Dto;

namespace FeeSystem.Fees.Dto
{
    public class FeeTypeDto : EntityDto<int>
    {
        public int FeeTypeId { get; set; }
        public string FeeTypeName { get; set; }
    }
}