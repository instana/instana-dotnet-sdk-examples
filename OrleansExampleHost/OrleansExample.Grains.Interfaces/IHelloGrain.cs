using Orleans;
using System.Threading.Tasks;

namespace OrleansExample.Grains.Interfaces
{
    public interface IHelloGrain : IGrainWithIntegerKey
    {
        Task<string> GreetMe(GrainMessage<string> nameMessage);
    }
}
