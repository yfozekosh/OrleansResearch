using System;

namespace PoC.Blog.GrainImplementation
{
    [Serializable]
    public class UserInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        // not hashed because it is only PoC
        public string Password { get; set; }
    }
}