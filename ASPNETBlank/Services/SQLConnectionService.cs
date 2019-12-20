using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNETBlank.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPNETBlank.Services
{
    public class SQLConnectionService : IDbConnectionService
    {
        private readonly UrlShorterDbContext _dbContext;
        private readonly IHashGeneratorService _hashGenerator;

        public SQLConnectionService(UrlShorterDbContext dbContext,IHashGeneratorService hashGenerator)
        {
            _dbContext = dbContext;
            _hashGenerator = hashGenerator;
        }

        private async Task AddUrlInfo(UrlInfo info)
        {
            await _dbContext.UrlInfos.AddAsync(info);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<UrlInfo> GetUrlInfo(string hash)
        {
            UrlInfo url = await _dbContext.UrlInfos.FindAsync(hash);
            return url;
        }

        public async Task DeleteUrlInfo(string hash)
        {
            UrlInfo toDelete = await _dbContext.UrlInfos.FirstOrDefaultAsync(x => x.Hash == hash);
            if (toDelete == null)
            {
                return;
            }
            _dbContext.UrlInfos.Remove(toDelete);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<string> GetFullUrl(string hash,bool isUsed = false) //If isUsed == true - UsesCount increment
        {
            UrlInfo url = await _dbContext.UrlInfos.FindAsync(hash);
            if(url != null && isUsed)
            {
                url.UsesCount++;
                await _dbContext.SaveChangesAsync();
            }
            return url?.Url;
        }

        public async Task<string> GetShortUrl(string fullUrl)
        {
            UrlInfo result = await _dbContext.UrlInfos.Where(ui => ui.Url == fullUrl).FirstOrDefaultAsync();
            if (result == null)
            {
                result = new UrlInfo()
                {
                    Url = fullUrl,
                    CreatonTime = DateTime.Now
                };
                result.Hash = _hashGenerator.GenerateHash();
                await AddUrlInfo(result);
            }
            return result.Hash;
        }

        public async Task<IEnumerable<UrlInfo>> GetUrlInfos()
        {
            return await _dbContext.UrlInfos.ToListAsync();
        }
    }
}
