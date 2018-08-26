using System;
using System.Collections.Generic;

namespace PoC.Blog.GrainImplementation
{
    [Serializable]
    public class UserRegistryState
    {
        public int UserIdSequence { get; set; }

        public IList<UserInfo> UserInfos { get; set; }
    }
}