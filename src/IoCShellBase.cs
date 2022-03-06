using AutomationIoC.Context;
using System.Management.Automation;
using System.Reflection;

namespace AutomationIoC
{
    public abstract class IoCShellBase : PSCmdlet
    {
        internal IAutomationContext Context { get; set; }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            if (Context is null)
                Context = new AutomationContext(SessionState);
        }

        internal void RunInstance()
        {
            BeginProcessing();
            ProcessRecord();
            EndProcessing();
        }

        internal void LoadDependencies()
        {
            PropertyInfo[] properties = this.GetType().GetProperties();

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
                        throw new ArgumentNullException(property.Name, "Injected Member is not registered in container");
                    }
                }
            }
        }
    }
}
