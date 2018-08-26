using System;
using System.Linq;
using System.Threading.Tasks;
using Orleans;
using PoC.Blog.GrainContracts;

namespace PoC.Blog.GrainImplementation
{
    public class BlogRegistry : Grain<BlogRegistryState>, IBlogRegistry
    {
        public override Task OnActivateAsync()
        {
            long id = this.GetPrimaryKeyLong();
            Console.WriteLine($"Blog registry {id} activated;");
            return Task.CompletedTask;
        }

        public override Task OnDeactivateAsync()
        {
            long id = this.GetPrimaryKeyLong();
            Console.WriteLine($"Blog registry {id} deactivated;");
            return Task.CompletedTask;
        }

        public Task<BLogInfo[]> GetUserBlogs()
        {
            return Task.FromResult(State.BLogInfos.ToArray());
        }

        public async Task AddBlog(string name)
        {
            var blogInfo = new BLogInfo()
            {
                Id = ++State.IdCounter,
                Title = name
            };

            State.BLogInfos.Add(blogInfo);

            await this.WriteStateAsync();
        }
    }
}