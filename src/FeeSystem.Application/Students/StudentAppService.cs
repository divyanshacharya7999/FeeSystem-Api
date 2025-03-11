using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using FeeManagementSystem.Classes;
using FeeManagementSystem.Fees;
using FeeManagementSystem.Payments;
using FeeManagementSystem.StudentFees;
using FeeManagementSystem.Students;
using FeeSystem.Students.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Abp.Domain.Entities;
using FeeManagementSystem.Classes.Dto;
using AutoMapper;
using FeeSystem.MultiTenancy.Dto;
using FeeSystem.MultiTenancy;
using Microsoft.AspNetCore.Authorization;

namespace FeeSystem.Students
{
    [AbpAuthorize]
    public class StudentAppService : ApplicationService, IStudentAppService
    {
        private readonly IRepository<Student, int> _studentRepository;
        private readonly IRepository<Class, int> _classRepository;
        private readonly IRepository<Fee, int> _feeRepository;
        private readonly IRepository<StudentFee, int> _studentFeeRepository;
        private readonly IRepository<Payment, long> _paymentRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<Tenant, int> _tenantRepository;

        public StudentAppService(
            IRepository<Student, int> studentRepository,
            IRepository<Class, int> classRepository,
            IRepository<Fee, int> feeRepository,
            IRepository<StudentFee, int> studentFeeRepository,
            IRepository<Payment, long> paymentRepository,
            IMapper mapper,
            IRepository<Tenant, int> tenantRepository)
        {
            _studentRepository = studentRepository;
            _classRepository = classRepository;
            _feeRepository = feeRepository;
            _studentFeeRepository = studentFeeRepository;
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _tenantRepository = tenantRepository;
        }

        public async Task<List<StudentDto>> GetAllStudents()
        {
            int currTenantId;
            if (AbpSession.TenantId.HasValue)
            {
                currTenantId = (int)AbpSession.TenantId;
            }
            else
            {
                currTenantId = 0;
            }

            var students = getstudentFunc(currTenantId);
            

            if (students == null || !students.Any())
            {
                Console.WriteLine("No students found in the database.");
                return new List<StudentDto>();
            }

            var studentDtos = ObjectMapper.Map<List<StudentDto>>(students);

            return studentDtos;
        }

        public async Task UpdateStudentFeeAsync(UpdateStudentFeeDto input)
        {
            // Retrieve the existing StudentFee
            var studentFee = _studentFeeRepository
                .GetAllIncluding(s=>s.Fee,s=>s.PaymentPlan)
                .Where(s=>s.Fee.FeeType.FeeTypeName == input.FeeHeading)
                .FirstOrDefault(s => s.StudentId == input.StudentId);
    
            if (studentFee == null)
            {
                throw new EntityNotFoundException(typeof(StudentFee), input.StudentId);
            }

            double totalPeriod;
            switch (studentFee.PaymentPlan.IntervalInMonths)
            {
                case 12:
                    totalPeriod = 1;
                    break;
                case 3:
                    totalPeriod =  4;
                    break;
                case 1:
                    totalPeriod = 12;
                    break;
                default:
                    throw new ArgumentException($"Unsupported interval: {studentFee.PaymentPlan.IntervalInMonths}");
            }

            studentFee.AdditionalFee = input.AdditionalFee;
            studentFee.Discount = input.Discount;

            studentFee.UpdatedTotalAmount = ((studentFee.TotalAmount) + studentFee.AdditionalFee) - studentFee.Discount;

            studentFee.AmountPerPeriod = (studentFee.UpdatedTotalAmount) / totalPeriod;

            await _studentFeeRepository.UpdateAsync(studentFee);
        }

        public async Task<StudentDto> GetStudentById(string id)
        {
            var student = await _studentRepository
        .GetAllIncluding(s => s.Class, s => s.StudentFees)
        .FirstOrDefaultAsync(s => s.StudentId == id);

            if (student == null)
            {
                throw new UserFriendlyException("Student not found!");
            }

            var studentDto = ObjectMapper.Map<StudentDto>(student);

            return studentDto;
        }

        public async Task<StudentResponseDto> CreateStudent(CreateStudentDto input)
        {
            int localTenantId = (int)AbpSession.TenantId;

            try
            {
                var existingStudent = await _studentRepository
                    .FirstOrDefaultAsync(s => s.ContactNumber == input.ContactNumber && s.TenantId == localTenantId);

                if (existingStudent != null)
                {
                    throw new UserFriendlyException("A student with this phone number already exists.");
                }

                var classExist = _classRepository
                    .GetAll()
                    .Where(cl => cl.ClassId == input.ClassId && cl.TenantId == localTenantId)
                    .ToList();

                if (classExist.IsNullOrEmpty())
                {
                    throw new UserFriendlyException("Class not found.");
                }

                string customStudentId = await GenerateCustomStudentId(localTenantId, input.ClassId);

                var student = ObjectMapper.Map<Student>(input);
                student.StudentId = customStudentId;
                await _studentRepository.InsertAsync(student);
                await CurrentUnitOfWork.SaveChangesAsync();

                var classFees = await _feeRepository
                    .GetAllIncluding(f => f.PaymentPlan)
                    .Where(f => f.ClassId == student.ClassId)
                    .ToListAsync();

                var studentId = _studentRepository
                    .GetAll()
                    .Where(s => s.TenantId == localTenantId)
                    .FirstOrDefault(s => s.ContactNumber == student.ContactNumber);

                double totalAmount = 0;
                double amountPerPeriod = 0;

                if (classFees.Any())
                {
                    foreach (var fee in classFees)
                    {
                        var paymentPlan = fee.PaymentPlan;
                        if (paymentPlan == null)
                        {
                            throw new UserFriendlyException("Payment plan is null for Fee ID: " + fee.Id);
                        }

                        double feeAmountPerPeriod = fee.Amount;
                        switch (paymentPlan.IntervalInMonths)
                        {
                            case 12:
                                feeAmountPerPeriod = fee.Amount / 1;
                                break;
                            case 3:
                                feeAmountPerPeriod = fee.Amount / 4;
                                break;
                            case 1:
                                feeAmountPerPeriod = fee.Amount / 12;
                                break;
                            default:
                                throw new UserFriendlyException($"Unsupported interval: {paymentPlan.IntervalInMonths}");
                        }

                        totalAmount += fee.Amount;
                        amountPerPeriod += feeAmountPerPeriod;

                        var studentFee = new StudentFee
                        {
                            StudentId = studentId.StudentId,
                            FeeId = fee.FeeId,
                            PaymentPlanId = fee.PaymentPlanId,
                            AmountPerPeriod = feeAmountPerPeriod,
                            TotalAmount = fee.Amount,
                            UpdatedTotalAmount = fee.Amount,
                            TenantId = localTenantId,
                            EffectiveFrom = DateTime.Now
                        };

                        if (studentFee.StudentId == null || studentFee.FeeId == 0 || studentFee.PaymentPlanId == 0)
                        {
                            Logger.Error("Invalid data for StudentFee. One or more IDs are missing.");
                            throw new UserFriendlyException("Invalid data for StudentFee. One or more IDs are missing.");
                        }
                        await _studentFeeRepository.InsertAsync(studentFee);
                    }
                }

                return new StudentResponseDto
                {
                    Success = true,
                    Message = "Student created successfully.",
                    Data = ObjectMapper.Map<StudentDto>(student)
                };
            }
            catch (UserFriendlyException ex)
            {
                return new StudentResponseDto
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponseDto<StudentDto>> UpdateStudent(UpdateStudentDto input)
        {
            var student = await _studentRepository.FirstOrDefaultAsync(s => s.StudentId == input.StudentId);
            if (student == null)
            {
                throw new UserFriendlyException("Student not found!");
            }

            var duplicatePhone = await _studentRepository.FirstOrDefaultAsync(s =>
                s.ContactNumber == input.ContactNumber && s.StudentId != input.StudentId);

            if (duplicatePhone != null)
            {
                throw new UserFriendlyException($"Phone number {input.ContactNumber} is already in use by another student.");
            }

            _mapper.Map(input, student);

            await _studentRepository.UpdateAsync(student);
            await CurrentUnitOfWork.SaveChangesAsync();

            return new ApiResponseDto<StudentDto>(
                success: true,
                message: "Student updated successfully!",
                data: _mapper.Map<StudentDto>(student)
            );
        }


        public async Task<ApiResponseDto<string>> DeleteStudent(string id)
        {
            var student = await _studentRepository.FirstOrDefaultAsync(s => s.StudentId == id);
            if (student == null)
            {
                throw new UserFriendlyException("Student not found!");
            }

            var studentFeeExists = await _studentFeeRepository.GetAll().AnyAsync(f => f.StudentId == id);
            if (studentFeeExists)
            {
                throw new UserFriendlyException("Cannot delete student because fees are associated with this student.");
            }

            await _studentRepository.DeleteAsync(s => s.StudentId == id);

            return new ApiResponseDto<string>(
                success: true,
                message: "Student deleted successfully!"
            );
        }


        public async Task<List<StudentFeeDto>> GetFeesForStudent(string studentId)
        {
            var student = await _studentRepository
                .GetAllIncluding(s => s.Class)
                .FirstOrDefaultAsync(s => s.StudentId == studentId);

            if (student == null)
            {
                throw new Exception("Student not found!");
            }

            var studentFees = await _studentFeeRepository
                .GetAll()
                .Where(sf => sf.StudentId == studentId)
                .Include(sf => sf.Fee)
                    .ThenInclude(f => f.FeeType)
                .Include(sf => sf.PaymentPlan)
                .ToListAsync();

            var studentFeeDtos = ObjectMapper.Map<List<StudentFeeDto>>(studentFees);

            return studentFeeDtos;
        }

        public async Task PayFee(string studentId, int feeId, double amountPaid, string paymentMethod, int discount = 0)
        {
            var studentFee = await _studentFeeRepository
                .FirstOrDefaultAsync(sf => sf.StudentId == studentId && sf.FeeId == feeId);

            if (studentFee == null)
            {
                throw new Exception("Fee not found for this student!");
            }

            if (studentFee.TotalAmount <= amountPaid)
            {
                await _studentFeeRepository.DeleteAsync(studentFee);
            }
            else
            {
                studentFee.TotalAmount -= amountPaid;
                await _studentFeeRepository.UpdateAsync(studentFee);
            }

            var payment = new Payment
            {
                StudentId = studentId,
                FeeId = feeId,
                AmountPaid = amountPaid,
                PaymentDate = DateTime.Now,
                PaymentMethod = paymentMethod,
                Discount = discount
            };

            await _paymentRepository.InsertAsync(payment);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public async Task<ClassDto> GetClassAsync(int id)
        {
            var classEntity = await _classRepository.GetAll()
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.ClassId == id);

            if (classEntity == null)
            {
                throw new UserFriendlyException("Class not found!");
            }

            var classDto = ObjectMapper.Map<ClassDto>(classEntity);
            return classDto;
        }

        public async Task<List<ClassDto>> GetAllClassesAsync()
        {
            var classes = await _classRepository.GetAllListAsync();
            return ObjectMapper.Map<List<ClassDto>>(classes);
        }

        public async Task<ApiResponseDto<ClassDto>> UpdateClassAsync(UpdateClassDto input)
        {
            var existingClass = await _classRepository.FirstOrDefaultAsync(x => x.ClassId == input.Id);
            if (existingClass == null)
            {
                throw new UserFriendlyException("Class not found.");
            }

            _mapper.Map(input, existingClass);
            await _classRepository.UpdateAsync(existingClass);

            return new ApiResponseDto<ClassDto>(
                success: true,
                message: "Class updated successfully!",
                data: _mapper.Map<ClassDto>(existingClass)
            );
        }

        public async Task<ApiResponseDto<ClassDto>> CreateClassAsync(CreateClassDto input)
        {
            var entity = _mapper.Map<Class>(input);
            await _classRepository.InsertAsync(entity);

            return new ApiResponseDto<ClassDto>(
                success: true,
                message: "Class created successfully!",
                data: _mapper.Map<ClassDto>(entity)
            );
        }

        public async Task<ApiResponseDto<string>> DeleteClassAsync(EntityDto<int> input)
        {
            var students = _studentRepository.GetAll().Where(x => x.ClassId == input.Id);
            if (students.Count() > 0)
            {
                throw new UserFriendlyException("Students Present in this class, Cannot Delete the Class");
            }

            var existingClass = await _classRepository.FirstOrDefaultAsync(x => x.ClassId == input.Id);
            if (existingClass == null)
            {
                throw new UserFriendlyException("Class not found.");
            }

            await _classRepository.DeleteAsync(existingClass);

            return new ApiResponseDto<string>(
                success: true,
                message: "Class deleted successfully!"
            );
        }

        private List<Student> getstudentFunc(int currTenantId)
        {
            if (currTenantId == 0){
                return  _studentRepository
                    .GetAll()
                    .Include(s => s.Class)
                    .Include(s => s.StudentFees)
                    .ToList();  
            }else
            {
                return _studentRepository
                    .GetAll()
                    .Include(s => s.Class)
                    .Where(S => S.TenantId == currTenantId)
                    .Include(s => s.StudentFees)
                    .ToList();
            }
        }
        
        private async Task<string> GenerateCustomStudentId(int tenantId, int classId)
        {
            string fixedRandomNumber = "0967";
            string tenantIdPart = tenantId.ToString("D3"); 
            string classIdPart = classId.ToString("D2");

            var lastStudentInClass = await _studentRepository
                .GetAll()
                .Where(s => s.TenantId == tenantId && s.ClassId == classId)
                .OrderByDescending(s => s.StudentId)
                .FirstOrDefaultAsync();

            int nextIncrementalNumber = 1;
            if (lastStudentInClass != null)
            {
                string lastStudentId = lastStudentInClass.StudentId;
                string lastIncrementalPart = lastStudentId.Substring(9);
                if (int.TryParse(lastIncrementalPart, out int lastIncrement))
                {
                    nextIncrementalNumber = lastIncrement + 1;
                }
            }

            string incrementalPart = nextIncrementalNumber.ToString("D3");

            string studentId = $"{fixedRandomNumber}{tenantIdPart}{classIdPart}{incrementalPart}";
            return studentId;
        }

        [AllowAnonymous]
        public async Task<List<TenantDto>> GetAllSchool()
        {
            var tenants = await _tenantRepository.GetAllListAsync();
            return ObjectMapper.Map<List<TenantDto>>(tenants);
        }

    }
}


