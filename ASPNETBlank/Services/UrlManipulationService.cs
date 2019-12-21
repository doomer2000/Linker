using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ASPNETBlank.Services
{
    public class UrlManipulationService : IUrlManipulationService
    {
        public string HandleUrlStr(string url)
        {
            try
            {
                url = url.ToLower();
                if (url.Substring(new Uri(url).Scheme.Length + 3).StartsWith("www."))
                {
                    url = new Regex(Regex.Escape("www.")).Replace(url, "", 1);
                }
                return url;
            }
            catch
            {
                return null;
            }
        }
    }
}
