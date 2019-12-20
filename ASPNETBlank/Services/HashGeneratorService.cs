using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETBlank.Services
{
    public class HashGeneratorService : IHashGeneratorService
    {
        public string GenerateHash()
        {

            return new Guid().ToString()
                   .Substring(0, 8);
        }
    }
}
