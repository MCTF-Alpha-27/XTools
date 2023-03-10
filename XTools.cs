using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CefSharp;
using System.IO;

namespace XTools
{
    public partial class XTools : Form
    {
        public XTools()
        {
            InitializeComponent();
            ReloadToolsToolStripMenuItem.Visible = false;
            LoadTools();
            ToolBrowser.LoadUrl("file:///index.html");
        }
        public void LoadTools()
        {
            string[] paths = Directory.GetDirectories(Environment.CurrentDirectory + "/tools");
            foreach (string path in paths)
            {
                string[] tools = Directory.GetFiles(path, "*.html");
                Regex re_title = new Regex(@"<title>([\s\S]*?)</title>");
                foreach (string tool in tools)
                {
                    ToolStripMenuItem function = new ToolStripMenuItem();
                    function.Name = tool.Replace("tools", "").Remove(0, 1);
                    function.Text = re_title.Match(File.ReadAllText(tool)).Value
                        .Replace("<title>", "").Replace("</title>", "");
                    function.Click += new EventHandler((s, e) =>
                    {
                        string[] split = tool.Split('\\');
                        ToolBrowser.LoadUrl("file:///tools/" + split[split.Length - 2] + "/" + split[split.Length - 1]);
                    });
                    ChooseToolToolStripMenuItem.DropDownItems.Add(function);
                }
            }
        }

        private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolBrowser.Reload();
        }

        private void HomePageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolBrowser.LoadUrl("file:///index.html");
        }

        private void ReloadToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // FIXME: 以下语句会导致插件添加的菜单也被移除
            // ChooseToolToolStripMenuItem.DropDownItems.Clear();
            // LoadTools();
        }
    }
}
