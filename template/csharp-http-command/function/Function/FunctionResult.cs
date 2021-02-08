using System.Collections.Generic;
using JobProcessing.Abstractions;

namespace Function
{
    public sealed class FunctionResult : ValueObject
    {
        public bool IsSuccess { get; }
        public string Value { get; }
        public string Error { get; }

        private FunctionResult(
            bool isSuccess,
            string value,
            string error)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }
        
        public static readonly FunctionResult Success = new(true, string.Empty, string.Empty);

        public static FunctionResult SuccessWith(string value) => new(true, value, string.Empty);

        public static FunctionResult FailureWith(string error) => new(false, string.Empty, error);
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return IsSuccess;
            yield return Value;
            yield return Error;
        }

        public override string ToString()
        {
            return $"{nameof(IsSuccess)}: {IsSuccess}, {nameof(Value)}: {Value}, {nameof(Error)}: {Error}";
        }
    }
}