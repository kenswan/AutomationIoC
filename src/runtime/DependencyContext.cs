using System.Management.Automation;

namespace AutomationIoC.Runtime;

public class DependencyContext<TAttribute, TStartup>
    where TAttribute : Attribute
    where TStartup : IIoCStartup, new()
{
    public SessionState SessionState { get; set; }

    public object Instance { get; set; }
}
