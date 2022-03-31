namespace AutomationIoC.Sample.Models
{
    public interface IDeck
    {
        Card Draw();

        void Load();
    }
}
