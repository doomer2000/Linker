using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNETBlank.Models;
using ASPNETBlank.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ASPNETBlank.Services
{
    public class SQLConnectionService : IDbConnectionService
    {
        private readonly UrlShorterDbContext _dbContext;
        private readonly IHashGeneratorService _hashGenerator;
        private readonly IDateTimeService _timeService;

        public SQLConnectionService(UrlShorterDbContext dbContext, IHashGeneratorService hashGenerator, IDateTimeService timeService)
        {
            _timeService = timeService;
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

        public async Task<string> GetFullUrl(string hash, bool isUsed = false)
        {
            UrlInfo url = await _dbContext.UrlInfos.FindAsync(hash);
            if (url != null && isUsed)
            {
                url.UsesCount++;
                await _dbContext.SaveChangesAsync();
            }
            return url.Url;
        }

        public async Task<bool> CheckExist(string hash)
        {
            return await _dbContext.UrlInfos.AnyAsync(x => x.Hash == hash);
        }

        public async Task<string> GetShortUrl(string fullUrl, string hash)
        {
            if (await CheckExist(hash)) return null;
            else
            {
                UrlInfo toAdd = new UrlInfo()
                {
                    Hash = hash,
                    Url = fullUrl,
                    CreatonTime = DateTime.Now
                };
                await AddUrlInfo(toAdd);
                return hash;
            }
        }
        public async Task<string> GetShortUrl(string fullUrl)
        {
            UrlInfo result = await _dbContext.UrlInfos.Where(ui => ui.Url == fullUrl).FirstOrDefaultAsync();
            if (result == null)
            {
                string hash = string.Empty;
                do
                {
                    hash = _hashGenerator.GenerateHash(fullUrl);
                }
                while (await CheckExist(hash));
                result = new UrlInfo()
                {
                    Hash = hash,
                    Url = fullUrl,
                    CreatonTime = _timeService.GetCurrentDateTime()
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
