using AutomationIoC.Context;
using System.Management.Automation;
using System.Reflection;

namespace AutomationIoC
{
    public abstract class IoCShellBase : PSCmdlet
    {
        internal IAutomationContext Context { get; set; }

        protected sealed override void BeginProcessing()
        {
            base.BeginProcessing();

            if (Context is null)
                Context = new AutomationContext(SessionState);

            LoadDependencies();
        }

        internal void RunInstance()
        {
            BeginProcessing();
            ProcessRecord();
            EndProcessing();
        }

        internal void LoadDependencies()
        {
            PropertyInfo[] properties = this.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                AutomationDependencyAttribute attribute =
                    Attribute.GetCustomAttribute(property, typeof(AutomationDependencyAttribute)) as AutomationDependencyAttribute;

                if (attribute is not null)
                {
                    var service = Context.GetDependency(property.PropertyType);
                    if (service is not null)
                    {
                        property.SetValue(this, service, null);
                    }
                    else
                    {
                        throw new ArgumentNullException(property.Name, $"Injected Member {property.Name} does not have a registered type");
                    }
                }
            }

            FieldInfo[] fields = this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (FieldInfo field in fields)
            {
                AutomationDependencyAttribute attribute =
                    Attribute.GetCustomAttribute(field, typeof(AutomationDependencyAttribute)) as AutomationDependencyAttribute;

                if (attribute is not null)
                {
                    var service = Context.GetDependency(field.FieldType);

                    if (service is not null)
                    {
                        field.SetValue(this, service);
                    }
                    else
                    {
                        throw new ArgumentNullException(field.Name, $"Injected Member {field.Name} does not have a registered type");
                    }
                }
            }
        }
    }
}
