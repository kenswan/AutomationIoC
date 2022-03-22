namespace AutomationIoC.Services
{
    internal interface IDependencyService
    {
        void LoadFieldsByAttribute<TAttribute>(object instance) where TAttribute : Attribute;

        void LoadPropertiesByAttribute<TAttribute>(object shellInstance) where TAttribute : Attribute;
    }
}
