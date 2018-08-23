using System;
using System.Threading.Tasks;
using GrainContracts;
using Microsoft.Extensions.Logging;

namespace GrainImpl
{
    public class HelloGrain : Orleans.Grain, IHello
    {
        private readonly ILogger logger;

        public HelloGrain(ILogger<HelloGrain> logger)
        {
            this.logger = logger;
        }

        Task<string> IHello.SayHello(string greeting)
        {
            Console.WriteLine($"SayHello message received: greeting = '{greeting}'");
            return Task.FromResult($"You said: '{greeting}', I say: Hello!");
        }
    }
}
