using NUnit.Framework;
using AdvWatcher;
using System.Threading;
using System;
using System.IO;

namespace Tests
{
    public class SiteWatcherTests
    {
        private bool _isEventCalled;
        private string _logName;
        [SetUp]
        public void Setup()
        {
            _isEventCalled = false;
            _logName = $"errors{ DateTime.Now.ToString("yyyyMMdd") }.log";
        }

        [Test]
        [TestCase("https://www.gametracker.rs/sms_boost/", 20.00, "PayPal boost")]
        [TestCase("https://www.google.com", 10.00, "apis.google.com")]
        public void GeneralCloudflareBangerTest(string site, double interval, string wantedText)
        {
            CreateSiteWatcher(site, interval, wantedText);
            Assert.IsTrue(_isEventCalled, "Watcher stopped and string not found");
        }

        [Test]
        [TestCase("ddddd")]
        [TestCase("https://")]
        [TestCase("http://google")]
        public void IncorrectURLs(string url)
        {
            ClearLogFile();
            CreateSiteWatcher(url, 5.00, " ");
            Assert.IsTrue(IsLogContainsErrors());
        }

        private void ClearLogFile()
        {
            File.WriteAllText(_logName, "");
        }

        private bool IsLogContainsErrors()
        {
            return File.ReadAllText(_logName).Contains("[ERROR]");
        }

        private void CreateSiteWatcher(string site, double interval, string wantedText)
        {
            SiteWatcher siteWatcher = new SiteWatcher(site, interval, wantedText);
            siteWatcher.OnAdvAvaiable += SiteWatcher_OnAdvAvaiable;
            siteWatcher.StartWatcher();

            int attempts = 4;
            int totalSteps = Convert.ToInt32(interval) * 1000 * attempts;
            int counter = 0;
            int step = 100;
            while (true)
            {
                counter += step;
                Thread.Sleep(step);

                if (_isEventCalled || counter > totalSteps)
                {
                    break;
                }
            }

            if (siteWatcher.IsWorking)
            {
                siteWatcher.StopWatcher();
            }
        }

        private void SiteWatcher_OnAdvAvaiable(object sender, System.EventArgs e)
        {
            _isEventCalled = true;
        }
    }
}