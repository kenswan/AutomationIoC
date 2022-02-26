using System.Management.Automation;

namespace PowerShellFocused.SDK
{
    public class TestService : PSCmdlet
    {
        public void CallTestMethod()
        {
            Console.WriteLine("Test Method Call");
        }
    }
}
