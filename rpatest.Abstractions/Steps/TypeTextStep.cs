namespace rpatest.Abstractions.Steps
{
    public class TypeTextStep : Step
    {
        private string Text { get; }

        public TypeTextStep(string name, ControlInfo target, string text, IControlService controlService) : base(name, target, controlService)
        {
            Text = text;
        }

        public override void Execute()
        {
            ControlService.TypeText(Target, Text);
        }
    }
}
