using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeeSystem.Students.Dto
{
    public class ApiResponseDto<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ApiResponseDto(bool success, string message, T data = default)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}
