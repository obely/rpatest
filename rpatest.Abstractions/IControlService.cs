namespace rpatest.Abstractions
{
    public interface IControlService
    {
        void Click(ControlInfo target);

        void TypeText(ControlInfo target, string text);

        void HotKey(int[] keys);
    }
}
