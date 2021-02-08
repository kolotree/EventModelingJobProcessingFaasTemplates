using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using JobProcessing.Abstractions;
using Function.Domain;

namespace Function
{
    public sealed class FunctionHandler
    {
        private readonly IStore _store;

        public FunctionHandler(IStore store)
        {
            _store = store;
        }
        
        public async Task<FunctionResult> Handle(HttpRequest request)
        {
            var command = await request.Read<ReplaceThisCommand>();
            var commandHandler = new CommandHandler(_store);

            try
            {
                await commandHandler.Handle(command);
                return FunctionResult.Success;
            }
            catch (VersionMismatchException)
            {
                return FunctionResult.FailureWith("Item with the same ID already in store");
            }
        }
    }
}