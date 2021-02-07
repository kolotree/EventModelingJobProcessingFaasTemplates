using System.Threading.Tasks;
using Abstractions;

namespace Function.Domain
{
    internal sealed class CommandHandler : ICommandHandler<ReplaceThisCommand>
    {
        private readonly IStore _store;

        public CommandHandler(IStore store)
        {
            _store = store;
        }
        
        public async Task Handle(ReplaceThisCommand c)
        {
            var newStream = ReplaceThisStream.NewFrom(c);
            await _store.SaveChanges(newStream);
        }
    }
}