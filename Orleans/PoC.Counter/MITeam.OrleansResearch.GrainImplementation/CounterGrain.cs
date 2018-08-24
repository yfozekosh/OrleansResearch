using System.Threading.Tasks;
using MITeam.OrleansResearch.GrainContracts;

namespace MITeam.OrleansResearch.GrainImplementation
{
    public class CounterGrain : Orleans.Grain, ICounterGrain
    {
        private int _counter;

        public Task<int> GetCount()
        {
            return Task.FromResult(this._counter);
        }

        public Task Add()
        {
            return Task.FromResult(++this._counter);
        }
    }
}
