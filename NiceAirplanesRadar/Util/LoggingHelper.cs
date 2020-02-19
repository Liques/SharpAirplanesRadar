using System;
using System.Collections.Generic;
using System.Linq;
using NiceAirplanesRadar;

namespace NiceAirplanesRadar.Util
{
    internal static class LoggingHelper {
        public static bool ShowBehaviorLog { get; set; }

        static LoggingHelper() {
        }

        public static void LogBehavior(string message){
            message = $"{DateTime.Now} - {message}";
            if(ShowBehaviorLog){
                Console.WriteLine(message);
            }
        }
    }
}