namespace PluginFrame
{
    public interface IPlugin
    {
        string Name { get; }
        string Version { get; }
        string Author { get; }
        string Description { get; }
        void Main(XTools.XTools xTools);
    }
}
