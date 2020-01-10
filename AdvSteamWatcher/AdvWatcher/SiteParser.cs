using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace AdvWatcher
{
    internal class SiteParser
    {
        private string _site;
        private WebClient _webClient;
        public SiteParser(string site)
        {
            _site = site;
        }

        public bool IsAdvertismentAvaiable()
        {
            string siteCode = _webClient.DownloadString(_site);
            return !siteCode.Contains("Limit reached, week boost will be available on ");
        }
    }
}
