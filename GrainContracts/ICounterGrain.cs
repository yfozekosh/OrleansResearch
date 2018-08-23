using System;
using System.Threading.Tasks;

namespace GrainContracts
{
    public interface ICounterGrain : Orleans.IGrainWithIntegerKey
    {
        Task<int> GetCount();

        Task Add();
    }
}
