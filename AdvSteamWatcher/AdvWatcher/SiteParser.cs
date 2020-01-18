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
        private StringBuilder _outputBuilder;
        public SiteParser(string site, string wantedText)
        {
            _outputBuilder = new StringBuilder();
            _site = site;
            _wantedText = wantedText;

            _pythonProcess = new Process 
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "CloudflareBanger.py",
                    Arguments = _site,
                    UseShellExecute = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
        }

        public bool IsAdvertismentAvaiable()
        {
            _outputBuilder.Clear();
            _pythonProcess.Start();
            while (!_pythonProcess.StandardOutput.EndOfStream)
            {
                _outputBuilder.Append(_pythonProcess.StandardOutput.ReadLine());
            }

            return !_outputBuilder.ToString().Contains(_wantedText);
        }
    }
}
