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
        private SiteParser _siteParser;
        public SiteWatcher(string site, uint interval)
        {
            Site = site;
            Interval = interval;

            _timer = new System.Timers.Timer();
            _timer.Elapsed += OnTimerElapsed;
            _timer.Interval = Interval;
            _timer.Enabled = true;

            _siteParser = new SiteParser(Site);
        }

        public void StartWatcher()
        {
            _timer.Start();
        }

        private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void StopWatcher()
        {
            _timer.Stop();
        }
    }
}
