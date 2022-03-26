namespace AutomationIoC.Runtime
{
    public interface IAutomationEnvironment
    {
        T GetVariable<T>(string key);

        void SetVariable<T>(string key, T value);

        bool TryGetVariable<T>(string key, out T value);
    }
}
