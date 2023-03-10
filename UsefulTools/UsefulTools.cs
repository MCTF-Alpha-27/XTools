using System;
using PluginFrame;
using System.Windows.Forms;

namespace UsefulTools
{
    public class UsefulTools : IPlugin
    {
        public string Name => "实用工具";

        public string Version => "1.0.0";

        public string Author => "XTools开发组";

        public string Description => "增加了一些外部网站的实用工具";

        public void Main(XTools.XTools xTools)
        {
            ToolStripMenuItem usefulTools = new ToolStripMenuItem();
            usefulTools.Name = "UsefulTools";
            usefulTools.Text = Name;
            xTools.ChooseToolToolStripMenuItem.DropDownItems.Add(usefulTools);

            ToolStripMenuItem mikuTools = new ToolStripMenuItem();
            mikuTools.Name = mikuTools.Text = "MikuTools";
            mikuTools.Click += new EventHandler((s, e) =>
            {
                xTools.ToolBrowser.LoadUrl("https://tools.miku.ac");
            });
            usefulTools.DropDownItems.Add(mikuTools);
        }
    }
}
