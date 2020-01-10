using System;

namespace AdvWatcher
{
    public class SiteWatcher
    {
        public string Site { get; set; }
        public uint Interval { get; set; }
        public bool IsWorking { get; set; } = false;
        public SiteWatcher(string site, uint interval)
        {
            Site = site;
            Interval = interval;
        }

        public void StartWatcher()
        {
            IsWorking = true;
        }

        public void StopWatcher()
        {
            IsWorking = false;
        }
    }
}
