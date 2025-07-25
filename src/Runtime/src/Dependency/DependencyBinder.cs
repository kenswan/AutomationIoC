// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.Reflection;

namespace AutomationIoC.Runtime.Dependency;

internal static class DependencyBinder
{
    public static void BindServicesByAttribute<TAttribute>(IServiceProvider serviceProvider, object instance) where TAttribute : Attribute
    {
        if (serviceProvider is null)
        {
            throw new ArgumentNullException(nameof(serviceProvider));
        }

        LoadFieldsByAttribute<TAttribute>(serviceProvider, instance);

        LoadPropertiesByAttribute<TAttribute>(serviceProvider, instance);
    }

    private static void LoadFieldsByAttribute<TAttribute>(IServiceProvider serviceProvider, object instance) where TAttribute : Attribute
    {
        FieldInfo[] fields = instance.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (FieldInfo field in fields)
        {
            var attribute =
                Attribute.GetCustomAttribute(field, typeof(TAttribute)) as TAttribute;

            if (attribute is not null)
            {
                object service = serviceProvider.GetService(field.FieldType);
                if (service is not null)
                {
                    field.SetValue(instance, service);
                }
                else
                {
                    throw new ArgumentNullException(field.Name, $"Injected Member {field.Name} does not have a registered type");
                }
            }
        }
    }

    private static void LoadPropertiesByAttribute<TAttribute>(IServiceProvider serviceProvider, object instance) where TAttribute : Attribute
    {
        PropertyInfo[] properties = instance.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (PropertyInfo property in properties)
        {
            var attribute =
                Attribute.GetCustomAttribute(property, typeof(TAttribute)) as TAttribute;

            if (attribute is not null)
            {
                object service = serviceProvider.GetService(property.PropertyType);
                if (service is not null)
                {
                    property.SetValue(instance, service, null);
                }
                else
                {
                    throw new ArgumentNullException(property.Name, $"Injected Member {property.Name} does not have a registered type");
                }
            }
        }
    }
}
