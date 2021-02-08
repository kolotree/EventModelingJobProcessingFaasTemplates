using System.Collections.Generic;
using JobProcessing.Abstractions;

namespace Function.Domain
{
    public sealed class ReplaceThisEvent : ValueObject, IEvent
    {
        public string Id { get; }

        public ReplaceThisEvent(
            string id)
        {
            Id = id;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }
    }
}