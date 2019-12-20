using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETBlank.Services
{
    public class GuidHashGeneratorService : IHashGeneratorService
    {
        public string GenerateHash()
        {
            return Guid.NewGuid().ToString()
                   .Substring(0, 6);
        }
    }
}
