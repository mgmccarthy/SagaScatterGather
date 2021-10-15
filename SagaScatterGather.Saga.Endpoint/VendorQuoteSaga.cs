using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using SagaScatterGather.Shared.Commands;
using SagaScatterGather.Shared.Messages;

namespace SagaScatterGather.Saga.Endpoint
{
    //TODO: mark if a vendor quote has been received in saga data
    //set a timeout for compensating action where only the ones that returned in the SLA timeframe get to have their quotes counted
    //will need to task.delay some vendor quote endpoints
    public class VendorQuoteSaga : Saga<VendorQuoteSaga.SagaData>,
        IAmStartedByMessages<GetQuote>,
        IHandleMessages<Vendor1QuoteResponse>,
        IHandleMessages<Vendor2QuoteResponse>,
        IHandleMessages<Vendor3QuoteResponse>
    {
        private static readonly ILog Log = LogManager.GetLogger<VendorQuoteSaga>();

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
            mapper.ConfigureMapping<GetQuote>(message => message.QuoteId).ToSaga(sagaData => sagaData.QuoteId);
        }

        public async Task Handle(GetQuote message, IMessageHandlerContext context)
        {
            Log.Info("Handling GetQuote");
            await context.Send(new Vendor1QuoteRequest { QuoteId = message.QuoteId });
            await context.Send(new Vendor2QuoteRequest { QuoteId = message.QuoteId });
            await context.Send(new Vendor3QuoteRequest { QuoteId = message.QuoteId });
        }

        public Task Handle(Vendor1QuoteResponse message, IMessageHandlerContext context)
        {
            Log.Info($"Handling VendorQuote1Response with quote value of: {message.QuoteAmount}");
            return Task.CompletedTask;
        }

        public Task Handle(Vendor2QuoteResponse message, IMessageHandlerContext context)
        {
            Log.Info($"Handling VendorQuote1Response with quote value of: {message.QuoteAmount}");
            return Task.CompletedTask;
        }

        public Task Handle(Vendor3QuoteResponse message, IMessageHandlerContext context)
        {
            Log.Info($"Handling VendorQuote1Response with quote value of: {message.QuoteAmount}");
            return Task.CompletedTask;
        }

        public class SagaData : ContainSagaData
        {
            public Guid QuoteId { get; set; }
        }
    }
}
