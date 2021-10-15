using System;
using NServiceBus;

namespace SagaScatterGather.Shared.Messages
{
    public class Vendor2QuoteRequest : IMessage
    {
        public Guid QuoteId { get; set; }
    }
}
