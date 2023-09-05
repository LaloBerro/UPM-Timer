using System;

namespace Timer.Runtime.Core.Domain
{
    public interface ITimer : IPausable, IResumable
    {
        public Action OnTimerFinished { get; set;}

        void Start(float duration);
    }
}