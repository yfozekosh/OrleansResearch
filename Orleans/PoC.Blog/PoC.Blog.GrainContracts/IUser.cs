using System.Threading.Tasks;
using Orleans;

namespace PoC.Blog.GrainContracts
{
    public class BLogInfo
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }

    public interface IUser : IGrainWithIntegerKey
    {
        Task<BLogInfo> GetBlogs();
    }
}