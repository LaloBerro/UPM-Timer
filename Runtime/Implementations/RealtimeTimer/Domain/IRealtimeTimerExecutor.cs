namespace Timer.Runtime.Realtime.Domain
{
    public interface IRealtimeTimerExecutor
    {
        void Initialize();
        void StartTimer(RealtimeTimer realtimeTimer);
        void PauseAll();
        void ResumeAll();
        float GetElapsedTime(string id);
    }
}