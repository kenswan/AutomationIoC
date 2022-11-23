namespace AutomationIoC.Runtime.Environment;

internal class AutomationEnvironment : IAutomationEnvironment
{
    private readonly IEnvironmentStorageProvider environmentStorageProvider;

    public AutomationEnvironment(IEnvironmentStorageProvider environmentStorageProvider)
    {
        this.environmentStorageProvider = environmentStorageProvider;
    }

    public T GetVariable<T>(string key)
    {
        T value = GetEnvironmentValue<T>(key);

        if (value is null)
        {
            throw new ArgumentException($"Environment Variable {key} does not exist");
        }

        return value;
    }

    public bool TryGetVariable<T>(string key, out T value)
    {
        value = GetEnvironmentValue<T>(key);

        return value is not null;
    }

    private T GetEnvironmentValue<T>(string key) =>
        environmentStorageProvider.GetEnvironmentVariable<T>(key);
}
