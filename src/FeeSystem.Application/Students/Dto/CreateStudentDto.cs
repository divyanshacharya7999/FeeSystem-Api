using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeeSystem.Students.Dto
{
    public class CreateStudentDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ClassId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
    }
}
