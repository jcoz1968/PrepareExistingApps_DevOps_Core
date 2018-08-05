using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace TodoServiceLibrary.WcfLogging
{
    // Nuget: Newtonsoft.Json, Microsoft.Net.Http
    // Approach: -> globally track performance for 
    //                      every service call (app.config)
    //           -> globally handle errors (service class attribute)
    public static class WcfLogger
    {
        public static void LogDiagnostic(string message, object additionalInfo = null)
        {
            var logEntry = GetWcfLogEntry(message, additionalInfo);
            LogIt(logEntry, "Diagnostic");
        }
        public static void LogError(string message, Exception ex,
            object additionalInfo = null)
        {
            var logEntry = GetWcfLogEntry(message, additionalInfo, ex: ex);

            if (ex.Data.Contains("ErrorId"))
                logEntry.CorrelationId = ex.Data["ErrorId"].ToString();

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

        internal static FlogDetail GetWcfLogEntry(string message,
            object additionalInfo, string serviceName = null,
            Exception ex = null)
        {
            var logEntry = new FlogDetail
            {
                Timestamp = DateTime.Now,
                UserId = "",
                UserName = "",
                Hostname = Environment.MachineName,
                Product = "Todos",
                Layer = "WcfService",
                Message = message,
                Location = serviceName,
                Exception = ex,
                CustomException = ex.ToCustomException()
            };
            if (string.IsNullOrEmpty(logEntry.Location))
                logEntry.Location = GetLocationFromExceptionOrStackTrace(logEntry);

            logEntry.AdditionalInfo = new Dictionary<string, object>();
            if (additionalInfo != null)
                SetPropertiesFromAdditionalInfo(logEntry, additionalInfo);

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
                if (method.DeclaringType == typeof(WcfLogger) ||
                    method.DeclaringType == typeof(FloggingParameterInspector))
                    continue;

                //assert - class is not this one
                return $"{method.DeclaringType.FullName}->{method.Name}";
            }
            return "Unable to determine location from StackTrace";
        }

        private static void SetPropertiesFromAdditionalInfo(FlogDetail logEntry, object additionalInfo)
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
