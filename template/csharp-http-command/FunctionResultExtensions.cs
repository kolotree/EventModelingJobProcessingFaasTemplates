using Function;
using Newtonsoft.Json;

internal static class FunctionResultExtensions
{
    public static int StatusCode(this FunctionResult functionResult) =>
        functionResult.IsSuccess ? 200 : 400;

    public static string ResponseJson(this FunctionResult functionResult) =>
        functionResult.IsSuccess
            ? JsonConvert.SerializeObject(new {value = functionResult.Value})
            : JsonConvert.SerializeObject(new {error = functionResult.Error});
}