using System;

namespace Timer.Runtime.Core.Domain
{
    public interface ITimer : IPausable, IResumable
    {
        public Action OnTimerFinished { get; set;}
        public Action<float> OnTimeUpdated { get; set;}
        
        float ElapsedTime { get; }
        bool IsPaused { get; }

        void Start(float duration);
    }
}