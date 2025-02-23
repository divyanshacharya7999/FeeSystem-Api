using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeeSystem.Students.Dto
{
    public class ClassDto : EntityDto<int>
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; }
    }
}
