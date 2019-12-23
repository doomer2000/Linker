using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETBlank.Services
{
    public interface IHashGeneratorService
    {
        string GenerateHash(string url);
    }
}
