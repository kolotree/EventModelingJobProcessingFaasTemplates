using JobProcessing.Abstractions;
using JobProcessing.Infrastructure.Serialization;

namespace Function.Domain
{
    internal sealed class ReplaceThisStream : Stream
    {
        public static ReplaceThisStream NewFrom(ReplaceThisCommand c)
        {
            var stream = new ReplaceThisStream();
            stream.ApplyChange(c.ToReplaceThisEvent().ToEventEnvelopeUsing(c.Metadata));
            return stream;
        }

        protected override void When(EventEnvelope eventEnvelope)
        {
            switch (eventEnvelope.Type)
            {
                case nameof(ReplaceThisEvent):
                    var replaceThisEvent = eventEnvelope.Deserialize<ReplaceThisEvent>();
                    SetIdentity(StreamId.AssembleFor<ReplaceThisStream>(replaceThisEvent.Id));
                    break;
            }
        }
    }
}