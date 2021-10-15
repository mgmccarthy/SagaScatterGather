using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using SagaScatterGather.Shared.Events;

namespace SagaScatterGather.Saga.Endpoint
{
    public class VendorQuoteSagaSlaBreachedHandler : IHandleMessages<VendorQuoteSlaBreached>
    {
        private static readonly ILog Log = LogManager.GetLogger<VendorQuoteSlaBreached>();

        public Task Handle(VendorQuoteSlaBreached message, IMessageHandlerContext context)
        {
            Log.Info($"VendorQuoteSaga SLA breached for Quote Id: {message.QuoteId}");
            return Task.CompletedTask;
        }
    }
}
