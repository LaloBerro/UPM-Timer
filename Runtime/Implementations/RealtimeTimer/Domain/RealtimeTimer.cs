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
        private bool _isPaused;
        
        public Action OnTimerFinished { get; set; }
        public Action<float> OnTimeUpdated { get; set; }
        
        public string Id => _id;
        public float Duration => _duration;
        public float ElapsedTime => _elapsedTime;
        public bool IsPaused => _isPaused;

        public RealtimeTimer(IRealtimeTimerExecutor realtimeTimerExecutor)
        {
            _realtimeTimerExecutor = realtimeTimerExecutor;
            _id = DateTime.Now.ToLongTimeString();

            OnTimerFinished = () => { };
        }

        public void SetElapsedTime(float elapsedTime)
        {
            _elapsedTime = elapsedTime;

            OnTimeUpdated?.Invoke(_elapsedTime);
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
            _isPaused = true;
        }

        public void Resume()
        {
            _isPaused = false;
        }
    }
}