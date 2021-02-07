using Abstractions;
using Infrastructure.Serialization;

namespace Function.Domain
{
    internal sealed class ReplaceThisStream : Stream
    {
        public static ReplaceThisStream NewFrom(ReplaceThisCommand c)
        {
            var stream = new ReplaceThisStream();
            stream.ApplyChange(c.ToReplaceThisEvent().ToEventEnvelope());
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