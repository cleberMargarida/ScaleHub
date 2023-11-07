namespace ScaleHub.Core.Abstract
{
    public interface ISetup
    {
        string Tag { get; set; }

        void ConfigureSubs(Action<IChannel> subscription);
    }
}