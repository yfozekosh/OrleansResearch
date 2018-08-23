using System;
using System.Threading.Tasks;

namespace GrainContracts
{
    public interface IHello : Orleans.IGrainWithIntegerKey
    {
        Task<string> SayHello(string greeting);
    }
}
