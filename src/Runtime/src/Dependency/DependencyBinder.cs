// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.Reflection;

namespace AutomationIoC.Runtime.Dependency;

internal static class DependencyBinder
{
    public static void BindServicesByAttribute<TAttribute>(IServiceProvider serviceProvider, object instance)
        where TAttribute : Attribute
    {
#if NET8_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(instance);
#else
        if (serviceProvider is null)
        {
            throw new AutomationRuntimeException("Error binding service to object instance: Service Provider cannot be null");
        }

        if (instance is null)
        {
            throw new AutomationRuntimeException("Error binding service to object instance:Object Instance cannot be null");
        }
#endif

        LoadFieldsByAttribute<TAttribute>(serviceProvider, instance);

        LoadPropertiesByAttribute<TAttribute>(serviceProvider, instance);
    }

    private static void LoadFieldsByAttribute<TAttribute>(IServiceProvider serviceProvider, object instance)
        where TAttribute : Attribute
    {
        FieldInfo[] fields = instance.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (FieldInfo field in fields)
        {
            if (Attribute.GetCustomAttribute(field, typeof(TAttribute)) is not TAttribute _)
            {
                continue;
            }

            object service = serviceProvider.GetService(field.FieldType);
            if (service is not null)
            {
                field.SetValue(instance, service);
            }
            else
            {
                throw new ArgumentNullException(field.Name,
                    $"Injected Member {field.Name} does not have a registered type");
            }
        }
    }

    private static void LoadPropertiesByAttribute<TAttribute>(IServiceProvider serviceProvider, object instance)
        where TAttribute : Attribute
    {
        PropertyInfo[] properties = instance.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (PropertyInfo property in properties)
        {
            if (Attribute.GetCustomAttribute(property, typeof(TAttribute)) is not TAttribute _)
            {
                continue;
            }

            object service = serviceProvider.GetService(property.PropertyType);
            if (service is not null)
            {
                property.SetValue(instance, service, null);
            }
            else
            {
                throw new ArgumentNullException(property.Name,
                    $"Injected Member {property.Name} does not have a registered type");
            }
        }
    }
}
