using Instana.ManagedTracing.Api;
using Instana.ManagedTracing.Sdk.Spans;
using Orleans;
using OrleansExample.Grains.Interfaces;
using System.Threading.Tasks;

namespace OrleansExample.Grains.Impl
{
    public class HelloGrain : Grain, IHelloGrain
    { 
        public Task<string> GreetMe(GrainMessage<string> nameMessage)
        {
            
                return Task.Run<string>(()=> {
                    using (var span = CustomSpan.CreateEntry(this, () => { return CorrelateFromMessage<string>(nameMessage); }))
                    {
                        span.SetData("name", nameMessage.Content);
                        span.SetTag("service", this.GetType().Name);

                        return $"Hello {nameMessage.Content}";
                    }
                });
        }

        private DistributedTraceInformation CorrelateFromMessage<T>(GrainMessage<T> message)
        {
            var dti = new DistributedTraceInformation();
            dti.ParentSpanId = long.Parse(message.Headers[TracingConstants.ExternalParentSpanIdHeader]);
            dti.TraceId = long.Parse(message.Headers[TracingConstants.ExternalTraceIdHeader]);
            return dti;
        }
    }
}
