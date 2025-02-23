using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace FeeManagementSystem.Classes.Dto
{
    public class UpdateClassDto : EntityDto<int>
    {
        [Required]
        public string ClassName { get; set; }
    }
}
