﻿using System;
using System.Collections.Generic;
using System.Text;
using SteamKit2;

namespace Steam
{
    public class Friend
    {
        public SteamID SteamID { get; set; }
        public bool IsReceivingAdvInfo { get; set; }
    }
}
