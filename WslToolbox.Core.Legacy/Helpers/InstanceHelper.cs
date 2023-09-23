using System;
using System.Collections.Generic;
using System.Linq;
using static System.Boolean;

namespace WslToolbox.Core.Legacy.Helpers;

public static class InstanceHelper
{
    public static T Create<T>(IDictionary<string, string>? objects) where T : class
    {
        var newInstance = Activator.CreateInstance(typeof(T));
        if (newInstance == null)
        {
            throw new Exception($"Unable to create instance of {typeof(T)}");
        }

        Console.WriteLine($"Instance {typeof(T)} created");
        if (objects == null || !objects.Any())
        {
            return (T)newInstance;
        }

        foreach (var propertyInfo in newInstance.GetType().GetProperties())
        {
            Console.WriteLine($"Property {propertyInfo.PropertyType} {propertyInfo.Name}");
            var propertyObject = objects.FirstOrDefault(x => string.Equals(x.Key, propertyInfo.Name, StringComparison.CurrentCultureIgnoreCase));
            if (propertyObject.Value == null)
            {
                Console.WriteLine($"Property {propertyInfo.Name} not found in objects list");
                continue;
            }

            object propertyResult = null;
            if ((propertyInfo.PropertyType == typeof(bool) || Nullable.GetUnderlyingType(propertyInfo.PropertyType) != null) && TryParse(propertyObject.Value, out var boolResult))
            {
                propertyResult = boolResult;
            }
            else if (propertyInfo.PropertyType == typeof(string) && !string.IsNullOrWhiteSpace(propertyObject.Value))
            {
                propertyResult = propertyObject.Value;
            }
            else
            {
                Console.WriteLine($"No parser for type {propertyInfo.PropertyType} of property {propertyInfo.Name} ");
                continue;
            }

            if (propertyResult == null)
            {
                Console.WriteLine($"Property {newInstance.GetType().Name}.{propertyInfo.Name} is null");
                propertyInfo.SetValue(newInstance, null, null);
                continue;
            }

            Console.WriteLine($"Property {propertyInfo.Name} parsed as {propertyResult.GetType().Name} with value {propertyResult}");
            propertyInfo.SetValue(newInstance, propertyResult, null);
        }

        return (T)newInstance;
    }
}