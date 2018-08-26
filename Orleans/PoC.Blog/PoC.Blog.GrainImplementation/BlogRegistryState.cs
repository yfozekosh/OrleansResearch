using System;
using System.Collections.Generic;
using PoC.Blog.GrainContracts;

namespace PoC.Blog.GrainImplementation
{
    [Serializable]
    public class BlogRegistryState
    {
        public int IdCounter { get; set; } = 0;

        public IList<BLogInfo> BLogInfos { get; set; }
    }
}
