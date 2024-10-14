namespace SkillService.EventProcessing
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}