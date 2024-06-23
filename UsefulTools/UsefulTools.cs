using PluginFrame;
using System.Windows.Forms;

namespace UsefulTools
{
    public class UsefulTools : IPlugin
    {
        public string Name => "实用工具";

        public string Version => "1.0.1";

        public string Author => "XTools开发组";

        public string Description => "增加了一些外部网站的实用工具";

        public void AddTool(XTools.XTools xTools, ToolStripMenuItem usefulTools, string name, string url)
        {
            ToolStripMenuItem tool = new ToolStripMenuItem();
            tool.Name = tool.Text = name;
            tool.Click += (s, e) =>
            {
                xTools.ToolBrowser.LoadUrl(url);
            };
            usefulTools.DropDownItems.Add(tool);
        }

        public void Main(XTools.XTools xTools)
        {
            ToolStripMenuItem usefulTools = new ToolStripMenuItem();
            usefulTools.Name = "UsefulTools";
            usefulTools.Text = Name;
            xTools.ChooseToolToolStripMenuItem.DropDownItems.Add(usefulTools);

            AddTool(xTools, usefulTools, "MikuTools", "https://tools.miku.ac");
            AddTool(xTools, usefulTools, "知乎", "https://www.zhihu.com/");
            AddTool(xTools, usefulTools, "菜鸟教程", "https://www.runoob.com/");
            AddTool(xTools, usefulTools, "百度一下，你就知道", "https://www.baidu.com/");
            AddTool(xTools, usefulTools, "文心一言", "https://yiyan.baidu.com/");
        }
    }
}
