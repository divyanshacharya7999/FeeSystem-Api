using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeeSystem.Students.Dto
{
    public class StudentResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public StudentDto Data { get; set; }
    }

}
