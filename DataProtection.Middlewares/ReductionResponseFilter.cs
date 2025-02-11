namespace DataProtection.Middlewares;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

public class ReductionResponseFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Do nothing before execution
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is ObjectResult objectResult && objectResult.Value != null)
        {
            ConvertStringsToUpperCase(objectResult.Value); // Modify response object before serialization
        }
    }

    private void ConvertStringsToUpperCase(object obj)
    {
        if (obj == null) return;

        var type = obj.GetType();
        if (type.IsPrimitive || obj is string) return;

        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!prop.CanRead || !prop.CanWrite) continue;

            if (prop.PropertyType == typeof(string))
            {
                var value = prop.GetValue(obj) as string;
                if (!string.IsNullOrEmpty(value))
                {
                    prop.SetValue(obj, value.ToUpper());
                }
            }
            else if (prop.PropertyType.IsClass)
            {
                var nestedObj = prop.GetValue(obj);
                ConvertStringsToUpperCase(nestedObj); // Recursively process nested objects
            }
        }
    }
}
