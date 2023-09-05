using System;
using Timer.Runtime.Core.Domain;

namespace Timer.Runtime.Realtime.Domain
{
    public class RealtimeTimer : ITimer
    {
        private readonly IRealtimeTimerExecutor _realtimeTimerExecutor;
        private readonly string _id;
        
        private float _duration;
        private float _elapsedTime;
        
        public Action OnTimerFinished { get; set; }
        public string Id => _id;
        public float Duration => _duration;
        public float ElapsedTime => _elapsedTime;

        public RealtimeTimer(IRealtimeTimerExecutor realtimeTimerExecutor)
        {
            _realtimeTimerExecutor = realtimeTimerExecutor;
            _id = DateTime.Now.ToLongTimeString();
        }

        public void SetElapsedTime(float elapsedTime)
        {
            _elapsedTime = elapsedTime;
        }
        
        public void Start(float duration)
        {
            _duration = duration;
            _elapsedTime = 0;
            
            _realtimeTimerExecutor.StartTimer(this);
        }
        
        public void FinishTimer()
        {
            OnTimerFinished?.Invoke();
        }

        public void Pause()
        {
            _realtimeTimerExecutor.Pause(_id);
        }

        public void Resume()
        {
            _realtimeTimerExecutor.Resume(_id);
        }
    }
}