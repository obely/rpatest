namespace rpatest.Abstractions.Steps
{
    public class ClickStep : Step
    {
        public ClickStep(string name, ControlInfo target, IControlService controlService) : base(name, target, controlService) { }

        public override void Execute()
        {
            ControlService.Click(Target);
        }
    }
}
