﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csMQ
{
    public static class ConfigSettings
    {
        public static string Host { get; set; }
        public static string JobQueue { get; set; }
        public static string RunQueue { get; set; }
        public static string ErrorQueue { get; set; }
        public static string CompletedQueue { get; set; }
    }
}
