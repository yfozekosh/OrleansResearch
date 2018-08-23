using System;
using System.Threading.Tasks;

namespace MITeam.OrleansResearch.GrainContracts
{
    public interface ICounterGrain : Orleans.IGrainWithIntegerKey
    {
        Task<int> GetCount();

        Task Add();
    }
}
