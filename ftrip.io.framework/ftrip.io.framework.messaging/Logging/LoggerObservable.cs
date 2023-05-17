using MassTransit;
using Serilog;
using System;
using System.Threading.Tasks;

namespace ftrip.io.framework.messaging.Logging
{
    public class LoggerObservable : IConsumeObserver
    {
        public Task ConsumeFault<T>(ConsumeContext<T> context, Exception exception) where T : class
        {
            return Task.CompletedTask;
        }

        public Task PostConsume<T>(ConsumeContext<T> context) where T : class
        {
            return Task.CompletedTask;
        }

        public Task PreConsume<T>(ConsumeContext<T> context) where T : class
        {
            Log.Logger.Information("Consuming message - Type[{Type}]", context.Message.GetType().Name);

            return Task.CompletedTask;
        }
    }
}