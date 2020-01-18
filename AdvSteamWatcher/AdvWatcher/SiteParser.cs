using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Diagnostics;

namespace AdvWatcher
{
    internal class SiteParser
    {
        private string _site;
        private string _wantedText;
        private Process _pythonProcess;
        public SiteParser(string site, string wantedText)
        {
            _site = site;
            _wantedText = wantedText;

            _pythonProcess = new Process 
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "py",
                    Arguments = $"CloudflareBanger.py { _site }",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
        }

        public bool IsAdvertismentAvaiable()
        {
            _pythonProcess.Start();

            string pythonOutput = _pythonProcess.StandardOutput.ReadToEnd();
            return !pythonOutput.Contains(_wantedText);
        }
    }
}
