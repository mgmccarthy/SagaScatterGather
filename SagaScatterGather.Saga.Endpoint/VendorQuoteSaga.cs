using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using SagaScatterGather.Shared.Commands;
using SagaScatterGather.Shared.Messages;

namespace SagaScatterGather.Saga.Endpoint
{
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
            Log.Info($"Handling Vendor1QuoteResponse with quote value of: {message.QuoteAmount}");
            Data.Vendor1Quote = message.QuoteAmount;
            return Task.CompletedTask;
        }

        public Task Handle(Vendor2QuoteResponse message, IMessageHandlerContext context)
        {
            Log.Info($"Handling Vendor2QuoteResponse with quote value of: {message.QuoteAmount}");
            Data.Vendor2Quote = message.QuoteAmount;
            return Task.CompletedTask;
        }

        public Task Handle(Vendor3QuoteResponse message, IMessageHandlerContext context)
        {
            Log.Info($"Handling Vendor3QuoteResponse with quote value of: {message.QuoteAmount}");
            Data.Vendor3Quote = message.QuoteAmount;
            return Task.CompletedTask;
        }

        //TODO:
        //set SLA timeout for when the saga starts (aka, we guarntee x amount of competative quotes in 30 seconds!)
        //write check for all three quotes received so you can publish the results before the SLA expires on the saga

        public class SagaData : ContainSagaData
        {
            public Guid QuoteId { get; set; }
            public decimal Vendor1Quote { get; set; }
            public decimal Vendor2Quote { get; set; }
            public decimal Vendor3Quote { get; set; }
        }
    }
}
