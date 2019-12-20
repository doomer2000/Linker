using ASPNETBlank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETBlank.Services
{
    public interface IDbConnectionService
    {
        Task<UrlInfo> GetUrlInfo(string hash);
        Task DeleteUrlInfo(string hash);
        Task<string> GetFullUrl(string hash, bool isUsed);
        Task<string> GetShortUrl(string fullUrl);
        Task<IEnumerable<UrlInfo>> GetUrlInfos();
    }
}
