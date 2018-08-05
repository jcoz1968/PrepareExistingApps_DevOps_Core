using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Dispatcher;

namespace TodoServiceLibrary.WcfLogging
{
    // usage / performance are globally tracked here
    // WCF Power Topics: Custom Behaviors (Miguel Castro)
    // paramterinspector -> operationbehavior -> servicebehvior -> behavior extension
    public class FloggingParameterInspector : IParameterInspector
    {
        string _serviceName;
        public FloggingParameterInspector(string serviceName)
        {
            _serviceName = serviceName;
        }
        public object BeforeCall(string operationName, object[] inputs)
        {
            var details = new Dictionary<string, object>();
            if (inputs != null)
            {
                for (var i = 0; i < inputs.Count(); i++)
                    details.Add($"input-{i}", 
                        inputs[i] != null ? inputs[i].ToString() : "");
            }

            return WcfLogger.GetWcfLogEntry(operationName, details, 
                serviceName: _serviceName);
        }

        public void AfterCall(string operationName, object[] outputs, 
            object returnValue, object correlationState)
        {
            var logEntry = correlationState as FlogDetail;
            if (logEntry == null)
                return;

            logEntry.ElapsedMilliseconds = (DateTime.Now - logEntry.Timestamp).Milliseconds;
            WcfLogger.LogIt(logEntry, "Performance");
        }
    }
}
