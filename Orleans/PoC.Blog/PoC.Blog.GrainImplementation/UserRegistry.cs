using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orleans;
using PoC.Blog.GrainContracts;

namespace PoC.Blog.GrainImplementation
{
    public class UserRegistry: Grain<UserRegistryState>, IUserRegistry {
        public override async Task OnActivateAsync()
        {
            if (this.State.UserInfos == null)
            {

                this.State.UserInfos = new List<UserInfo>();
                await WriteStateAsync();

            }
            else
            {
                Console.WriteLine("WoW! Unexpected state.");
            }

            long id = this.GetPrimaryKeyLong();
            Console.WriteLine($"User registry {id} activated;");
        }

        public override Task OnDeactivateAsync()
        {
            long id = this.GetPrimaryKeyLong();
            Console.WriteLine($"User registry {id} deactivated;");
            return Task.CompletedTask;
        }

        public Task<int?> GetUser(string email, string password)
        {
            if (email == null) throw new ArgumentNullException(nameof(email));
            if (password == null) throw new ArgumentNullException(nameof(password));

            UserInfo user = GetUser(email);

            int? id = user?.Password == password ? (int?) user.Id : null;

            return Task.FromResult(id);
        }

        public async Task<bool> RegisterUser(string email, string password)
        {
            if (email == null) throw new ArgumentNullException(nameof(email));
            if (password == null) throw new ArgumentNullException(nameof(password));

            if (GetUser(email) != null)
            {
                return false;
            }

            UserInfo newUserInfo = new UserInfo()
            {
                Id = ++State.UserIdSequence,
                Password = password,
                Email = email
            };

            State.UserInfos.Add(newUserInfo);

            await WriteStateAsync();

            return true;
        }

        private UserInfo GetUser(string email)
        {
            return State.UserInfos.FirstOrDefault(u => u.Email == email);
        }
    }
}