using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Microsoft.Extensions.Logging;
using ASPNETBlank.Models;
using ASPNETBlank.Services;
using Microsoft.EntityFrameworkCore.Design;
using System.Text.RegularExpressions;

namespace ASPNETBlank.Controllers
{
    public class HomeController : Controller
    {

        private readonly IDbConnectionService _dbConnectionService;
        private readonly IUrlManipulationService _urlManipulationService;

        public HomeController(IDbConnectionService dbConnectionService, IUrlManipulationService urlManipulationService)
        {
            _dbConnectionService = dbConnectionService;
            _urlManipulationService = urlManipulationService;
        }


        public async Task<IActionResult> Index()
        {
            IEnumerable<UrlInfo> urlInfos = await _dbConnectionService.GetUrlInfos();
            return View(urlInfos.OrderByDescending(x => x.CreatonTime));
        }

        [Route("{hash}")]
        public async Task<IActionResult> GoTo(string hash)
        {
            string fullUrl = await _dbConnectionService.GetFullUrl(hash, true);
            return fullUrl == null ? (IActionResult)RedirectToAction("Error") : Redirect(fullUrl);
        }

        [Route("del/{hash}")]
        [HttpGet]
        public async Task<IActionResult> Delete(string hash)
        {
            UrlInfo toDelete = await _dbConnectionService.GetUrlInfo(hash);
            return toDelete == null ? RedirectToAction("Error") : (IActionResult)View(toDelete);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string hash)
        {
            await _dbConnectionService.DeleteUrlInfo(hash);
            return RedirectToAction("Index");
        }

        [Route("/create")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [Route("/create")]
        public async Task<IActionResult> Create(UrlInfo urlInfo)
        {
            if (ModelState.IsValid)
            {
                UrlInfo toReturn = new UrlInfo();
                toReturn.Url = _urlManipulationService.HandleUrlStr(urlInfo.Url);
                if (urlInfo.Hash != null)
                {
                    toReturn = new UrlInfo()
                    {
                        Hash = await _dbConnectionService.GetShortUrl(toReturn.Url, urlInfo.Hash)
                    };
                    if(toReturn.Hash == null)
                    {
                        ModelState.AddModelError(nameof(urlInfo.Hash), "This hash already exist.");
                    }
                    return View(toReturn);
                }
                else
                {
                    toReturn = new UrlInfo()
                    {
                        Hash = await _dbConnectionService.GetShortUrl(toReturn.Url),
                    };
                    return View(toReturn);
                }
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(ErrorViewModel error = null)
        {
            if (error != null) return View(error);
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}