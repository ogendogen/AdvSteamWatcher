using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Diagnostics;
using System.IO;

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

            try
            {
                string pythonOutput = _pythonProcess.StandardOutput.ReadToEnd();
                if (pythonOutput.Contains("[ERROR]"))
                {
                    File.AppendAllText($"errors{ DateTime.Now.ToString("yyyyMMdd") }.log", DateTime.Now.ToString() + " " + pythonOutput);
                    return false;
                }
                return !pythonOutput.Contains(_wantedText);
            }
            catch (Exception e)
            {
                File.AppendAllText($"errors{ DateTime.Now.ToString("yyyyMMdd") }.log", DateTime.Now.ToString() + " " + e.Message);
                return false;
            }
        }
    }
}
