using System.Threading.Tasks;
using Orleans;

namespace PoC.Blog.GrainContracts
{
    public interface IBlog : IGrainWithIntegerCompoundKey
    {
        Task<string> GetTexts();
    }

    public interface IBlogRegistry : IGrainWithIntegerKey
    {
        Task<BLogInfo[]> GetUserBlogs();

        Task AddBlog(string name);
    }
}