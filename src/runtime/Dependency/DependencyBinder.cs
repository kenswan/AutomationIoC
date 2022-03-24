using System.Reflection;

namespace AutomationIoC.Runtime.Dependency
{
    internal class DependencyBinder : IDependencyBinder
    {
        private readonly IServiceProvider serviceProvider;

        public DependencyBinder(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ??
                throw new ArgumentNullException(nameof(serviceProvider), "ServiceProvider has not been registered");
        }

        public void LoadFieldsByAttribute<TAttribute>(object instance) where TAttribute : Attribute
        {
            FieldInfo[] fields = instance.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (FieldInfo field in fields)
            {
                TAttribute attribute =
                    Attribute.GetCustomAttribute(field, typeof(TAttribute)) as TAttribute;

                if (attribute is not null)
                {
                    var service = serviceProvider.GetService(field.FieldType);
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

        public void LoadPropertiesByAttribute<TAttribute>(object instance) where TAttribute : Attribute
        {
            PropertyInfo[] properties = instance.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                TAttribute attribute =
                    Attribute.GetCustomAttribute(property, typeof(TAttribute)) as TAttribute;

                if (attribute is not null)
                {
                    var service = serviceProvider.GetService(property.PropertyType);
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
}
