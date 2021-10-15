using System;
using NServiceBus;

namespace SagaScatterGather.Shared.Commands
{
    public class GetQuote : ICommand
    {
        public Guid QuoteId { get; set; }
    }
}
