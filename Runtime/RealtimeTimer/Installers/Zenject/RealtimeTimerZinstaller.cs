using ServiceLocatorPattern;
using Timer.Runtime.Core.Domain;
using Timer.Runtime.Realtime.Domain;
using ZenjectExtensions.Zinstallers;

namespace Timer.Runtime.Realtime.Installers
{
    public class RealtimeTimerZinstaller : InstanceZinstaller<ITimer>
    {
        protected override ITimer GetInitializedClass()
        {
            IRealtimeTimerExecutor realtimeTimerExecutor = ServiceLocator.Instance.Get<IRealtimeTimerExecutor>();
            return new RealtimeTimer(realtimeTimerExecutor);
        }
    }
}