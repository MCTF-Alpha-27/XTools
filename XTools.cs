using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CefSharp;
using System.IO;
using CefSharp.WinForms;

namespace XTools
{
    public partial class XTools : Form
    {
        public ILifeSpanHandler lifeSpanHandler;
        public string Title;

        public XTools()
        {
            InitializeComponent();
            ReloadToolsToolStripMenuItem.Visible = false;
            lifeSpanHandler = new LifeSpanHandler(this);
            ToolBrowser.LifeSpanHandler = lifeSpanHandler;
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
                        foreach (var control in ToolsViewer.SelectedTab.Controls)
                        {
                            if (control is ChromiumWebBrowser browser)
                            {
                                browser.LoadUrl("file:///tools/" + split[split.Length - 2] + "/" + split[split.Length - 1]);
                            }
                        }
                    });
                    ChooseToolToolStripMenuItem.DropDownItems.Add(function);
                }
            }
        }

        private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var control in ToolsViewer.SelectedTab.Controls)
            {
                if (control is ChromiumWebBrowser browser)
                {
                    browser.Reload();
                }
            }
        }

        private void HomePageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var control in ToolsViewer.SelectedTab.Controls)
            {
                if (control is ChromiumWebBrowser browser)
                {
                    browser.LoadUrl("file:///index.html");
                }
            }
        }

        private void ReloadToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // FIXME: 以下语句会导致插件添加的菜单也被移除
            // ChooseToolToolStripMenuItem.DropDownItems.Clear();
            // LoadTools();
        }

        private void XTools_Resize(object sender, EventArgs e)
        {
            ToolsViewer.Size = Size;
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ToolsViewer.SelectedIndex == 0)
            {
                MessageBox.Show("这是置顶标签页，请不要关闭", "XTools", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ToolsViewer.TabPages.Remove(ToolsViewer.SelectedTab);
            ToolsViewer.SelectedIndex = ToolsViewer.TabPages.Count - 1;
        }

        public void ToolBrowser_TitleChanged(object sender, TitleChangedEventArgs e)
        {
            Title = e.Title;
        }

        public void ToolBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            ToolsViewer.SelectedTab.Text = Title;
        }
    }

    public class LifeSpanHandler : ILifeSpanHandler
    {
        public XTools xTools;
        public bool isInternalNavigation = false;

        public LifeSpanHandler(XTools xTools)
        {
            this.xTools = xTools;
        }

        public bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            return false;
        }

        public void OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            return;
        }

        public void OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            return;
        }

        public bool OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            newBrowser = null;
            if (isInternalNavigation)
            {
                return false;
            }
            isInternalNavigation = true;
            TabPage tabPage = new TabPage(targetUrl);
            ChromiumWebBrowser NewTool = new ChromiumWebBrowser();
            NewTool.ActivateBrowserOnCreation = false;
            NewTool.Dock = DockStyle.Fill;
            NewTool.Location = new System.Drawing.Point(3, 3);
            NewTool.Margin = new Padding(4);
            NewTool.MinimumSize = new System.Drawing.Size(27, 25);
            NewTool.Name = "ToolBrowser";
            NewTool.Size = new System.Drawing.Size(1253, 596);
            NewTool.TabIndex = 0;
            NewTool.LifeSpanHandler = this;
            NewTool.TitleChanged += new EventHandler<TitleChangedEventArgs>(xTools.ToolBrowser_TitleChanged);
            NewTool.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(xTools.ToolBrowser_FrameLoadEnd);
            NewTool.FrameLoadEnd += (sender, e) =>
            {
                if (e.Frame.IsMain)
                {
                    isInternalNavigation = false;
                }
            };
            NewTool.LoadUrl(targetUrl);
            // xTools.ToolBrowser.LoadUrl(targetUrl);
            tabPage.Controls.Add(NewTool);
            xTools.Invoke(new MethodInvoker(delegate
            {
                xTools.ToolsViewer.Controls.Add(tabPage);
                xTools.ToolsViewer.SelectedIndex = xTools.ToolsViewer.TabPages.Count - 1;
            }));
            return true;
        }
    }
}
