using DependencyInjector.Installers;
using Timer.Runtime.Realtime.Domain;
using UnityEngine;

namespace Timer.Runtime.Realtime.Installers
{
    public class RealtimeTimerExecutorInstaller : SingleMonoInstaller<IRealtimeTimerExecutor>
    {
        [Header("References")] 
        [SerializeField] private RealtimeTimerExecutor _realtimeTimerExecutor;
        
        protected override IRealtimeTimerExecutor GetData()
        {
            _realtimeTimerExecutor.Initialize();

            return _realtimeTimerExecutor;
        }
    }
}