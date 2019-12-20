﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public HomeController(IDbConnectionService dbConnectionService)
        {
            _dbConnectionService = dbConnectionService;
        }

        [Route("/short")]
        public async Task<IActionResult> ShowShortUrl(string url)
        {
            url = url.ToLower();
            try
            {
                if (url.Substring(new Uri(url).Scheme.Length + 3).StartsWith("www."))
                {
                    url = new Regex(Regex.Escape("www.")).Replace(url, "", 1);
                }
            }
            catch
            {
                return RedirectToAction("Error");
            }
            string hash = await _dbConnectionService.GetShortUrl(url);
            ViewBag.hash = hash;
            return View();
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            IEnumerable<UrlInfo> urlInfos = await _dbConnectionService.GetUrlInfos();
            return View(urlInfos.OrderByDescending(x=>x.CreatonTime));
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
            return RedirectToAction("Urls");
        }

        [Route("/create")]
        public IActionResult Create()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
