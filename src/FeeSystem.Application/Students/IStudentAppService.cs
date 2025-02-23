using Abp.Application.Services;
using FeeSystem.Students.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeeManagementSystem.Classes;
using FeeManagementSystem.Classes.Dto;
using Abp.Application.Services.Dto;

namespace FeeSystem.Students
{
    public interface IStudentAppService : IApplicationService
    {
        Task<List<StudentDto>> GetAllStudents();
        Task<StudentDto> GetStudentById(string id);
        Task<StudentResponseDto> CreateStudent(CreateStudentDto input);
        Task<ApiResponseDto<StudentDto>> UpdateStudent(UpdateStudentDto input);
        Task<ApiResponseDto<string>> DeleteStudent(string id);
        Task<List<StudentFeeDto>> GetFeesForStudent(string studentId);
        Task PayFee(string studentId, int feeId, double amountPaid, string paymentMethod, int discount = 0);

        // Class Management
        Task<ClassDto> GetClassAsync(int id);
        Task<List<ClassDto>> GetAllClassesAsync();
        Task<ApiResponseDto<ClassDto>> CreateClassAsync(CreateClassDto input);

        Task<ApiResponseDto<string>> DeleteClassAsync(EntityDto<int> input);

        Task<ApiResponseDto<ClassDto>> UpdateClassAsync(UpdateClassDto input);
    }
}