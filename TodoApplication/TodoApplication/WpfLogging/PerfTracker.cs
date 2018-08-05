using System;

namespace TodoApplication.WpfLogging
{
    public class PerfTracker
    {
        FlogDetail _logEntry;

        public PerfTracker(string message, object additionalInfo = null)
        {
            _logEntry = WpfLogger.GetWpfLogEntry(message, additionalInfo);
        }

        public void Stop()
        {
            _logEntry.ElapsedMilliseconds = 
                (DateTime.Now - _logEntry.Timestamp).Milliseconds;

            WpfLogger.LogIt(_logEntry, "Performance");
        }
    }
}
