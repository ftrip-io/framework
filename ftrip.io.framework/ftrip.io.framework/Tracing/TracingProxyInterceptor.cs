using Castle.DynamicProxy;

namespace ftrip.io.framework.Tracing
{
    public class TracingProxyInterceptor : IInterceptor
    {
        private readonly ITracer _tracer;

        public TracingProxyInterceptor(ITracer tracer)
        {
            _tracer = tracer;
        }

        public void Intercept(IInvocation invocation)
        {
            using var activity = _tracer.ActivitySource.StartActivity($"{invocation.TargetType}.{invocation.Method.Name} called");

            invocation.Proceed();
        }
    }
}