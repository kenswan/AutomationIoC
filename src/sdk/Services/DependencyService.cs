using AutomationIoC.Providers;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AutomationIoC.Services
{
    internal class DependencyService : IDependencyService
    {
        private readonly ISessionStorageProvider sessionStorageProvider;

        public DependencyService(ISessionStorageProvider sessionStorageProvider)
        {
            this.sessionStorageProvider = sessionStorageProvider;
        }
        
        public void LoadFieldsByAttribute<TAttribute>(object shellInstance) where TAttribute : Attribute
        {
            FieldInfo[] fields = this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (FieldInfo field in fields)
            {
                TAttribute attribute =
                    Attribute.GetCustomAttribute(field, typeof(TAttribute)) as TAttribute;

                if (attribute is not null)
                {
                    var service = GetServiceObjectByType(field.FieldType);
                    if (service is not null)
                    {
                        field.SetValue(shellInstance, service);
                    }
                    else
                    {
                        throw new ArgumentNullException(field.Name, $"Injected Member {field.Name} does not have a registered type");
                    }
                }
            }
        }

        public void LoadPropertiesByAttribute<TAttribute>(object shellInstance) where TAttribute : Attribute
        {
            PropertyInfo[] properties = this.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                TAttribute attribute =
                    Attribute.GetCustomAttribute(property, typeof(TAttribute)) as TAttribute;

                if (attribute is not null)
                {
                    var service = GetServiceObjectByType(property.PropertyType);
                    if (service is not null)
                    {
                        property.SetValue(shellInstance, service, null);
                    }
                    else
                    {
                        throw new ArgumentNullException(property.Name, $"Injected Member {property.Name} does not have a registered type");
                    }
                }
            }
        }

        private object GetServiceObjectByType(Type type)
        {
            var method = this.GetType().GetMethod(nameof(this.GetRequiredService));
            var generic = method.MakeGenericMethod(type);
            return generic.Invoke(this, null);
        }

        private T GetRequiredService<T>()
        {
            IServiceProvider serviceProvider = sessionStorageProvider.GetServiceProvider();

            if (serviceProvider is null)
                throw new ArgumentNullException("ServiceProvider has not been registered");

            return serviceProvider.GetRequiredService<T>();
        }
    }
}
