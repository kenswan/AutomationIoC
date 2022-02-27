namespace AutomationIoC.Services
{
    public class TestService
    {
        public int CallCount { get; private set; }

        public string CallTestMethod()
        {
            Console.WriteLine("Test Method Call");

            CallCount += 1;

            return "This is a message Test";
        }
    }
}
