using System;
using System.Collections.Generic;
using System.Text;

namespace Steam
{
    public class Config
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public List<string> AdvMessageReceivers { get; set; }
    }
}
