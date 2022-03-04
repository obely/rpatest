
namespace rpatest.Abstractions.Steps
{
    public class HotKeyStep : Step
    {
        private int[] Keys { get; }

        public HotKeyStep(string name, int[] keys, IControlService controlService) : base(name, null, controlService)
        {
            Keys = keys;
        }

        public override void Execute()
        {
            ControlService.HotKey(Keys);
        }
    }
}
