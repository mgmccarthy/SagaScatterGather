using System;
using NServiceBus;

namespace SagaScatterGather.Shared.Messages
{
    public class Vendor1QuoteRequest : IMessage
    {
        public Guid QuoteId { get; set; }
    }
}
