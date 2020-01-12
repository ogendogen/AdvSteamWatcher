using System;

namespace AdvWatcher
{
    public class SiteWatcher
    {
        public string Site { get; set; }
        public uint Interval { get; set; }
        public bool IsWorking
        {
            get
            {
                return _timer.Enabled;
            }
        }
        private System.Timers.Timer _timer;
        public SiteWatcher(string site, uint interval)
        {
            Site = site;
            Interval = interval;
        }

        public void StartWatcher()
        {
            SiteParser siteParser = new SiteParser(Site);
            _timer = new System.Timers.Timer();
            _timer.Elapsed += OnTimerElapsed;
            _timer.Interval = Interval;
            _timer.Enabled = true;
        }

        private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void StopWatcher()
        {

        }
    }
}
