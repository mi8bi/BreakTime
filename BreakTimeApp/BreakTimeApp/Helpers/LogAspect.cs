using AspectInjector.Broker;
using NLog;

namespace BreakTimeApp.Helpers
{
    [Aspect(Scope.Global)]
    [Injection(typeof(LogAspect))]
    public class LogAspect : Attribute
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        [Advice(Kind.Before, Targets = Target.Method)]
        public void OnEntry([Argument(Source.Name)] string name)
        {
            _logger.Info($"Entering method {name}");
        }

        [Advice(Kind.After, Targets = Target.Method)]
        public void OnExit([Argument(Source.Name)] string name)
        {
            _logger.Info($"Exiting method {name}");
        }

    }

}
