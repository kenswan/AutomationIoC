namespace AutomationIoC.Runtime
{
    public interface IAutomationEnvironment
    {
        T GetVariable<T>(string key);

        bool TryGetVariable<T>(string key, out T value);
    }
}
