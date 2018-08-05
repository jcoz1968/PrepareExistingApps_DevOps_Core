using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;

namespace CoreFlogger
{
    public class TrackUsageFilter : IActionFilter
    {
        string _product, _layer;

        public TrackUsageFilter(string product, string layer)
        {
            _product = product;
            _layer = layer;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var request = context.HttpContext.Request;
            var activity = $"{request.Path}-{request.Method}";

            var dict = new Dictionary<string, object>();
            foreach (var key in context.RouteData.Values?.Keys)
                dict.Add($"RouteData-{key}", (string)context.RouteData.Values[key]);                            

            WebHelper.LogWebUsage(_product, _layer, activity, context.HttpContext, dict);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {            
        }
    }
}
