namespace Timer.Runtime.Realtime.Domain
{
    public interface IRealtimeTimerExecutor
    {
        void Initialize();
        void StartTimer(RealtimeTimer realtimeTimer);
        void Pause(string id);
        void Resume(string id);
    }
}