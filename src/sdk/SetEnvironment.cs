using AutomationIoC.Runtime;
using System.Management.Automation;

namespace AutomationIoC.SDK
{
    [Cmdlet(VerbsCommon.Set, "Environment")]
    public sealed class SetEnvironment : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        [Alias("k")]
        public string Key { get; set; }

        [Parameter(Mandatory = true)]
        [Alias("v")]
        public string Value { get; set; }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            AutomationIoCRuntime.SetEnvironment(SessionState, Key, Value);
        }
    }
}
