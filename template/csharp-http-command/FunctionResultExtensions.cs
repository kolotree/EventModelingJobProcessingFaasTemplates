using Function;

internal static class FunctionResultExtensions
{
    public static int StatusCode(this FunctionResult functionResult) =>
        functionResult.IsSuccess ? 200 : 400;

    public static string ResponseJson(this FunctionResult functionResult) =>
        functionResult.IsSuccess
            ? functionResult.Value!
            : functionResult.Error!;
}