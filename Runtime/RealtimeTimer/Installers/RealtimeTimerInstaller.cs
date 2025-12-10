using DependencyInjector.Core;
using DependencyInjector.Installers;
using Timer.Runtime.Core.Domain;
using Timer.Runtime.Realtime.Domain;

namespace Timer.Runtime.Realtime.Installers
{
    public class RealtimeTimerInstaller : SingleMonoInstaller<ITimer>
    {
        [Inject] private IRealtimeTimerExecutor _realtimeTimerExecutor;
        
        protected override ITimer GetData()
        {
            return new RealtimeTimer(_realtimeTimerExecutor);
        }
    }
}