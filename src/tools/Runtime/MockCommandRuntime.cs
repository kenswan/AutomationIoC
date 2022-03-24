using System.Management.Automation;
using System.Management.Automation.Host;

namespace AutomationIoC.Tools.Runtime
{
    internal class MockCommandRuntime : ICommandRuntime
    {
        public List<object> WrittenObjects { get; } = new List<object>();

        public PSTransactionContext CurrentPSTransaction => throw new NotImplementedException();

        public PSHost Host => throw new NotImplementedException();

        public bool ShouldContinue(string query, string caption) => true;

        public bool ShouldContinue(string query, string caption, ref bool yesToAll, ref bool noToAll) => true;

        public bool ShouldProcess(string target) => true;

        public bool ShouldProcess(string target, string action) => true;

        public bool ShouldProcess(string verboseDescription, string verboseWarning, string caption) => true;

        public bool ShouldProcess(string verboseDescription, string verboseWarning, string caption, out ShouldProcessReason shouldProcessReason)
        {
            shouldProcessReason = ShouldProcessReason.None;

            return true;
        }

        public void ThrowTerminatingError(ErrorRecord errorRecord)
        {
            Console.WriteLine(errorRecord.ToString());

            throw errorRecord.Exception;
        }

        public bool TransactionAvailable() => true;

        public void WriteCommandDetail(string text)
        {
            Console.WriteLine(text);
        }

        public void WriteDebug(string text)
        {
            Console.WriteLine(text);
        }

        public void WriteError(ErrorRecord errorRecord)
        {
            Console.WriteLine(errorRecord.ToString());
        }

        public void WriteObject(object sendToPipeline)
        {
            WrittenObjects.Add(sendToPipeline);
        }

        public void WriteObject(object sendToPipeline, bool enumerateCollection)
        {
            WrittenObjects.Add(sendToPipeline);
        }

        public void WriteProgress(long sourceId, ProgressRecord progressRecord)
        {
            Console.WriteLine(progressRecord.ToString());
        }

        public void WriteProgress(ProgressRecord progressRecord)
        {
            Console.WriteLine(progressRecord.ToString());
        }

        public void WriteVerbose(string text)
        {
            Console.WriteLine(text);
        }

        public void WriteWarning(string text)
        {
            Console.WriteLine(text);
        }
    }
}
