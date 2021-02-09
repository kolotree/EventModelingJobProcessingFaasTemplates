using System.Collections.Generic;
using JobProcessing.Abstractions;

namespace Function
{
    public sealed class FunctionResult : ValueObject
    {
        public int HttpCode { get; }
        public string? Value { get; }
        public string? Error { get; }
        
        public bool IsSuccess => Value != null;
        public bool IsFailure => !IsSuccess;

        private FunctionResult(int httpCode, string? value, string? error)
        {
            HttpCode = httpCode;
            Value = value;
            Error = error;
        }
        
        public static readonly FunctionResult Success = new(200, string.Empty, null);

        public static FunctionResult SuccessWith(string value) => new(200, value, null);

        public static FunctionResult BadRequestFailureWith(string error) => new(400, null, error);
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value ?? "";
            yield return Error ?? "";
        }

        public override string ToString() =>
            IsSuccess
                ? $"{nameof(HttpCode)}: {HttpCode}, {nameof(Value)}: {Value}"
                : $"{nameof(HttpCode)}: {HttpCode}, {nameof(Error)}: {Error}";
    }
}