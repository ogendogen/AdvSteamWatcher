using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace AdvWatcher
{
    internal class SiteParser
    {
        private string _site;
        private string _wantedText;
        private WebClient _webClient;
        public SiteParser(string site, string wantedText)
        {
            _site = site;
            _wantedText = wantedText;
        }

        public bool IsAdvertismentAvaiable()
        {
            string siteCode = _webClient.DownloadString(_site);
            return !siteCode.Contains(_wantedText);
        }
    }
}
