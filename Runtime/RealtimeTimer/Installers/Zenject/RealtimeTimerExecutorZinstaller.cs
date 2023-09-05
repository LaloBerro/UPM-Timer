using ServiceLocatorPattern;
using Timer.Runtime.Realtime.Domain;
using ZenjectExtensions.Zinstallers;

namespace Timer.Runtime.Realtime.Installers
{
    public class RealtimeTimerExecutorZinstaller : Zinstaller
    {
        public override void Install()
        {
            if (ServiceLocator.Instance.IsContained<IRealtimeTimerExecutor>())
                return;
            
            IRealtimeTimerExecutor realtimeTimerExecutor = new RealtimeTimerExecutor();
            ServiceLocator.Instance.Register(realtimeTimerExecutor);
        }
    }
}