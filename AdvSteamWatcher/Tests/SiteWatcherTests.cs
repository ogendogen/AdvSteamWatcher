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

        [TearDown]
        public void DisableEventFlag()
        {
            isEventCalled = false;
        }

        [Test]
        [TestCase("https://www.gametracker.rs/sms_boost/", 15.00, "PayPal boost")]
        public void GameTrackerPayPalBoostTest(string site, double interval, string wantedText)
        {
            CreateSiteWatcher(site, interval, wantedText);
            Assert.IsTrue(isEventCalled, "Watcher stopped and string not found");
        }

        private void CreateSiteWatcher(string site, double interval, string wantedText)
        {
            SiteWatcher siteWatcher = new SiteWatcher(site, interval, wantedText);
            siteWatcher.OnAdvAvaiable += SiteWatcher_OnAdvAvaiable;
            siteWatcher.StartWatcher();

            int steps = (Convert.ToInt32(interval) * 1000) * 4;
            Thread.Sleep(steps);

            siteWatcher.StopWatcher();
        }

        private void SiteWatcher_OnAdvAvaiable(object sender, System.EventArgs e)
        {
            isEventCalled = true;
        }
    }
}