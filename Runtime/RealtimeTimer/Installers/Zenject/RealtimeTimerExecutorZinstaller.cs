using ServiceLocatorPattern;
using Timer.Runtime.Realtime.Domain;
using UnityEngine;
using ZenjectExtensions.Zinstallers;

namespace Timer.Runtime.Realtime.Installers
{
    public class RealtimeTimerExecutorZinstaller : Zinstaller
    {
        [Header("References")] 
        [SerializeField] private RealtimeTimerExecutor _realtimeTimerExecutor;
        
        public override void Install()
        {
            if (ServiceLocator.Instance.IsContained<IRealtimeTimerExecutor>())
                return;
            
            _realtimeTimerExecutor.Initialize();
            
            ServiceLocator.Instance.Register<IRealtimeTimerExecutor>(_realtimeTimerExecutor);
        }
    }
}