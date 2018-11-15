using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public sealed class CustomAuditBehavior
    {

        static CustomAuditBehavior()
        {
            if (!EventLog.SourceExists("Logger"))
            {
                EventLog.CreateEventSource("Logger", "LoggerIsLogging");
            }
            MyLogger = new EventLog("LoggerIsLogging", Environment.MachineName, "Logger");
        }
        
        private static EventLog MyLogger { get; set; }

        public static void AuthenticationSuccess(string user)
        {
            MyLogger.WriteEntry($"User {user} is successfully authenticated.", EventLogEntryType.Information);
        }

        public static void LogAuthenticationFailure(string user, string reason)
        {
            MyLogger.WriteEntry(string.Format("User {0} failed to authenticate. Reason: {1}.", user, reason));
        }

        public static void FunctionSuccsess(string user, string function, string file)
        {
            MyLogger.WriteEntry(string.Format("User {0} succsessfully called {1} on {2}.", user, function, file));
        }
        public static void FunctionUnsuccsess(string user, string function, string file)
        {
            MyLogger.WriteEntry(string.Format("User {0} unsuccsessfully called {1} on {2}.", user, function, file));
        }

    }

    //primer poziva
    //CustomAuditBehavior.AuthenticationSuccess("Korisnik1");
}
