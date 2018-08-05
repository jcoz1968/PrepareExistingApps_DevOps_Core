using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.ServiceModel;
using System.Text;

namespace TodoApplication.WpfLogging
{
    public static class WpfLogger
    {
        public static void LogUsage(string usageMessage, object additionalInfo = null)
        {
            var logEntry = GetWpfLogEntry(usageMessage, additionalInfo);
            LogIt(logEntry, "Usage");            
        }

        public static void LogDiagnostic(string message, object additionalInfo = null)
        {
            var logEntry = GetWpfLogEntry(message, additionalInfo);
            LogIt(logEntry, "Diagnostic");
        }
        public static void LogError(string message, Exception ex, 
            object additionalInfo = null)
        {
            var logEntry = GetWpfLogEntry(message, additionalInfo, ex);
            if (ex is FaultException)
            {
                var fe = ex as FaultException;
                logEntry.CorrelationId = fe.Code.Name;
            }
            
            LogIt(logEntry, "Error");
        }

        internal static void LogIt(FlogDetail logEntry, string endpoint)
        {
            var client = new HttpClient();
            var baseUrl = new Uri("https://loggingapi.knowyourtoolset.com/logging/");
            var objAsJson = JsonConvert.SerializeObject(logEntry);
            var objToPost = new StringContent(objAsJson, Encoding.UTF8, "application/json");
            client.PostAsync(baseUrl + endpoint, objToPost);
        }

        internal static FlogDetail GetWpfLogEntry(string message,
            object additionalInfo,
            Exception ex = null)
        {
            var logEntry = new FlogDetail
            {
                Timestamp = DateTime.Now,
                UserId = Environment.UserName,
                UserName = Environment.UserName,
                Hostname = Environment.MachineName,
                Product = "Todos",
                Layer = "WpfClient",
                Message = message,
                Exception = ex
            };
            logEntry.Location = GetLocationFromExceptionOrStackTrace(logEntry);

            logEntry.AdditionalInfo = new Dictionary<string, object>();
            if (additionalInfo != null)
                SetPropertiesFromAdditionalInfo(logEntry, additionalInfo);
            
            //Add any extra stuff you might want here...
            logEntry.AdditionalInfo["ClientOS"] = Environment.OSVersion.VersionString;

            return logEntry;
        }

        private static string GetLocationFromExceptionOrStackTrace(FlogDetail logEntry)
        {
            var st = logEntry.Exception != null
                ? new StackTrace(logEntry.Exception)
                : new StackTrace();

            var sf = st.GetFrames();
            foreach (var frame in sf)
            {
                var method = frame.GetMethod();
                if (method.DeclaringType == typeof(WpfLogger) ||
                    method.DeclaringType == typeof(PerfTracker))
                    continue;

                //assert - class is not this one
                return $"{method.DeclaringType.FullName}->{method.Name}";
            }
            return "Unable to determine location from StackTrace";
        }

        private static void SetPropertiesFromAdditionalInfo(FlogDetail logEntry, 
            object additionalInfo)
        {
            if (additionalInfo is Dictionary<string, object>)
            {
                var ai = additionalInfo as Dictionary<string, object>;

                foreach (var item in ai)
                {
                    if (!logEntry.AdditionalInfo.ContainsKey(item.Key))
                        logEntry.AdditionalInfo[item.Key] = item.Value;
                }
            }
            else  // not a dictionary
            {
                var props = additionalInfo.GetType().GetProperties();
                foreach (var prop in props)
                {
                    logEntry.AdditionalInfo[$"dtl-{prop.Name}"] = 
                        prop.GetValue(additionalInfo).ToString();
                }
            }
        }
    }
}
