using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace PoC.Blog.GrainContracts
{
    public enum UserRegistryId
    {
        ActiveUsers = 1,
        DeletedUsers = 2
    }

    public interface IUserRegistry : IGrainWithIntegerKey
    {
        Task<int?> GetUser(string email, string password);

        Task<bool> RegisterUser(string email, string password);
    }
}