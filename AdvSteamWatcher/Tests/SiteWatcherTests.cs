using NUnit.Framework;
using AdvWatcher;
using System.Threading;
using System;

namespace Tests
{
    public class SiteWatcherTests
    {
        private bool isEventCalled;
        [SetUp]
        public void Setup()
        {
            isEventCalled = false;
        }

        [Test]
        [TestCase("https://www.gametracker.rs/sms_boost/", 20.00, "PayPal boost")]
        [TestCase("https://www.google.com", 10.00, "apis.google.com")]
        public void GeneralCloudflareBangerTest(string site, double interval, string wantedText)
        {
            CreateSiteWatcher(site, interval, wantedText);
            Assert.IsTrue(isEventCalled, "Watcher stopped and string not found");
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

                if (isEventCalled || counter > totalSteps)
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
            isEventCalled = true;
        }
    }
}