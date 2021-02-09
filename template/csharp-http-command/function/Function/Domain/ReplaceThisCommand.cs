using System;
using JobProcessing.Abstractions;

namespace Function.Domain
{
    internal sealed class ReplaceThisCommand : ICommand
    {
        public string Id { get; }

        public ReplaceThisCommand(string? id)
        {
            Id = id ?? throw new ArgumentException(nameof(id));
        }
        
        public StreamId StreamId => StreamId.AssembleFor<ReplaceThisStream>(Id);
        
        public ReplaceThisEvent ToReplaceThisEvent() => new(Id);
    }
}