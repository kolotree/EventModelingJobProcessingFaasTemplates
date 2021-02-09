using Function;
using Newtonsoft.Json;

internal static class FunctionResultExtensions
{
    public static string ResponseJson(this FunctionResult functionResult) => JsonConvert.SerializeObject(functionResult);
}