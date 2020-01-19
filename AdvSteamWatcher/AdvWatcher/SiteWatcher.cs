using System;

namespace AdvWatcher
{
    public class SiteWatcher
    {
        public event EventHandler OnAdvAvaiable;
        public string Site { get; }
        public double Interval { get; }
        public string WantedText { get; }
        public bool IsWorking
        {
            get
            {
                return _timer.Enabled;
            }
        }
        private System.Timers.Timer _timer;
        private SiteParser _siteParser;
        public SiteWatcher(string site, double intervalInSeconds, string wantedText)
        {
            Site = site;
            Interval = intervalInSeconds * 1000;
            WantedText = wantedText;

            _timer = new System.Timers.Timer();
            _timer.Elapsed += OnTimerElapsed;
            _timer.Interval = Interval;
            _timer.Enabled = true;

            _siteParser = new SiteParser(Site, wantedText);
        }

        public void StartWatcher()
        {
            _timer.Start();
        }

        private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_siteParser.IsAdvertismentAvaiable())
            {
                OnAdvAvaiable(this, new EventArgs());
            }
        }

        public void StopWatcher()
        {
            _timer.Stop();
        }
    }
}
