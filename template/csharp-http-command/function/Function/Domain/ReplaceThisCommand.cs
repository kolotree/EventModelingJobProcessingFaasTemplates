using System;
using JobProcessing.Abstractions;

namespace Function.Domain
{
    internal sealed class ReplaceThisCommand : ICommand
    {
        public CommandMetadata Metadata { get; }
        public string Id { get; }

        public ReplaceThisCommand(CommandMetadata  metadata, string? id)
        {
            Metadata = metadata ?? throw new ArgumentException(nameof(metadata));
            Id = id ?? throw new ArgumentException(nameof(id));
        }

        public StreamId StreamId => StreamId.AssembleFor<ReplaceThisStream>(Id);

        public ReplaceThisEvent ToReplaceThisEvent() => new(Id);
    }
}