using System;
using System.Collections.Generic;

namespace TodoServiceLibrary.WcfLogging
{
    public static class ExceptionPostingHelpers
    {
        public static CustomException ToCustomException(this Exception ex)
        {
            if (ex == null)
                return null;

            var error = new CustomException
            {
                Message = ex.Message,
                ExceptionName = ex.GetType().Name,
                ModuleName = ex.TargetSite?.Module.Name,
                DeclaringTypeName = ex.TargetSite?.DeclaringType?.Name,
                TargetSiteName = ex.TargetSite?.Name,
                StackTrace = ex.StackTrace,
                Data = new List<DictEntry>()
            };
            foreach (var dataKey in ex.Data.Keys)
            {
                error.Data.Add(new DictEntry
                {
                    Key = dataKey.ToString(),
                    Value = ex.Data[dataKey].ToString()
                });
            }

            if (ex.InnerException != null)
                error.InnerException = ex.InnerException.ToCustomException();

            return error;
        }
    }

    public class CustomException
    {
        public string ExceptionName { get; set; }
        public string ModuleName { get; set; }
        public string DeclaringTypeName { get; set; }
        public string TargetSiteName { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public List<DictEntry> Data { get; set; }
        public CustomException InnerException{ get; set; }

        public CustomException GetBaseError()
        {
            return InnerException != null ? InnerException.GetBaseError() : this;
        }
    }

    public class DictEntry
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
