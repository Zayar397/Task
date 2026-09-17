using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task.Domain.Models
{
    public class Result<T>
    {
        public ResultStatus Status { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsValidationError { get { return Status == ResultStatus.ValidationError; } }
        public bool IsSystemError { get { return Status == ResultStatus.SystemError; } }
        public static Result<T> Success(string message = null, T data = default)
        {
            return new Result<T>
            {
                Status = ResultStatus.Success,
                Data = data,
                Message = message,
                IsSuccess = true
            };
        }
        public static Result<T> ValidationError(string message = null, T data = default)
        {
            return new Result<T>
            {
                Status = ResultStatus.ValidationError,
                Data = data,
                Message = message,
                IsSuccess = false
            };
        }
        public static Result<T> SystemError(string message = null, T data = default)
        {
            return new Result<T>
            {
                Status = ResultStatus.SystemError,
                Data = data,
                Message = message,
                IsSuccess = false
            };
        }
    }

    public enum ResultStatus
    {
        None,
        Success,
        ValidationError,
        SystemError
    }
}
