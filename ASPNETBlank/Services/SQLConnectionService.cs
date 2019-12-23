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
            UrlInfo toDelete = await GetUrlInfo(hash);
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

        private async Task<bool> CheckExist(string hash)
        {
            return await _dbContext.UrlInfos.FindAsync(hash) == null ? false : true;
        }

        public async Task<string> GetShortUrl(string fullUrl)
        {
            UrlInfo result = await _dbContext.UrlInfos.Where(ui => ui.Url == fullUrl).FirstOrDefaultAsync();
            if (result == null)
            {
                string hash = string.Empty;
                bool a;
                do
                {
                    hash = _hashGenerator.GenerateHash(fullUrl);
                }
                while (await CheckExist(hash));
                result = new UrlInfo()
                {
                    Hash = hash,
                    Url = fullUrl,
                    CreatonTime = DateTime.Now
                };
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
