namespace DataProtection.Middlewares;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Compliance.Classification;
using Microsoft.Extensions.Compliance.Redaction;
using System.Reflection;


// TODO: ⚠ support sensitive data within nested objects

public class ReductionResponseFilter : IActionFilter
{
    private readonly IRedactorProvider? _redactorProvider;

    public ReductionResponseFilter(IRedactorProvider? redactorProvider)
    {
        _redactorProvider = redactorProvider;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Do nothing before execution
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (_redactorProvider == null)
            return;

        if (context.Result is ObjectResult objectResult && objectResult.Value != null)
        {
            objectResult.Value = Reduct(objectResult.Value);
        }
    }

    private object Reduct(object obj)
    {
        if (obj == null) return null;

        var type = obj.GetType();

        // If it's a primitive, string, or struct, return as is
        if (type.IsPrimitive || obj is string || type.IsEnum)
            return obj is string str ? str.ToUpper() : obj;

        // If it's a record or class, clone it with modified string properties
        if (type.IsClass || type.IsValueType)
        {
            object? instance = Activator.CreateInstance(type, true); // Bypass constructor if needed

            if (instance == null)
                throw new ArgumentNullException(type.Name);

            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic))
            {
                if (!prop.CanRead) continue; // Skip properties without a getter

                var classifications = prop.GetCustomAttributes<DataClassificationAttribute>(true)
                                          .Select(clsAtt => clsAtt.Classification);

                if (!classifications.Any())
                {
                    classifications = type.GetConstructors()
                                .SelectMany(ctor => ctor.GetParameters()
                                                                     .Where(prm => prm.Name == prop.Name)
                                                                     .SelectMany(prm => prm.GetCustomAttributes<DataClassificationAttribute>())
                                                                     .Select(clsAtt => clsAtt.Classification));
                }

                if (!classifications.Any())
                    continue; // Check for redaction attribute

                object? value = prop.GetValue(obj);
                if (value == null)
                    continue;

                var classificationSets = new DataClassificationSet(classifications);
                Redactor redactor = _redactorProvider!.GetRedactor(classificationSets);
                if (redactor == null)
                    continue;

                value = redactor.Redact(value);
                WriteProp();

                void WriteProp()
                {
                    if (prop.CanWrite)
                    {
                        // Writable properties can be set normally
                        prop.SetValue(instance, value);
                    }
                    else
                    {
                        // Read-only properties need to be modified using reflection
                        var field = type.GetField($"<{prop.Name}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
                        if (field != null)
                        {
                            field.SetValue(instance, value);
                        }
                    }
                }
            }

            return instance;
        }

        return obj;
    }
}
