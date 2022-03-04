namespace rpatest.Abstractions.Steps
{
    public abstract class Step
    {
        public string Name { get; }
        protected ControlInfo Target { get; }

        protected IControlService ControlService { get; }

        public Step(string name, ControlInfo target, IControlService controlService)
        {
            Name = name;
            Target = target;
            ControlService = controlService;
        }

        public abstract void Execute();
    }
}
