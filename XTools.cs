using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CefSharp;
using System.IO;
using CefSharp.WinForms;
using System.Collections.Generic;

namespace XTools
{
    public partial class XTools : Form
    {
        public ILifeSpanHandler lifeSpanHandler;
        public string Title;
        public LinkedList<string> Histories = new LinkedList<string>();

        public static string HomePageUrl
        {
            get { return new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "index.html")).AbsoluteUri; }
        }

        public XTools()
        {
            InitializeComponent();
            ApplyTheme();
            lifeSpanHandler = new LifeSpanHandler(this);
            ToolBrowser.LifeSpanHandler = lifeSpanHandler;
            LoadTools();
            ToolBrowser.LoadUrl(HomePageUrl);
        }

        private void ApplyTheme()
        {
            BackColor = UITheme.Background;
            ForeColor = UITheme.Text;
            Font = UITheme.BodyFont;
            DoubleBuffered = true;

            UITheme.ApplyMenuTheme(FunctionsMenu);
            UITheme.EnableDarkTitleBar(this);

            ToolsViewer.BackColor = UITheme.Background;
            ModernTabControl modernTabs = ToolsViewer as ModernTabControl;
            if (modernTabs != null)
            {
                modernTabs.TabCloseRequested += ToolsViewer_TabCloseRequested;
            }
        }

        private void ToolsViewer_TabCloseRequested(object sender, TabCloseRequestedEventArgs e)
        {
            CloseTab(e.Page);
        }

        public void LoadTools()
        {
            string toolsRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tools");
            if (!Directory.Exists(toolsRoot))
            {
                return;
            }

            foreach (string path in Directory.GetDirectories(toolsRoot))
            {
                string[] tools = Directory.GetFiles(path, "*.html");
                Regex re_title = new Regex(@"<title>([\s\S]*?)</title>");
                foreach (string tool in tools)
                {
                    ToolStripMenuItem function = new ToolStripMenuItem();
                    function.Name = tool.Substring(toolsRoot.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                    function.Text = re_title.Match(File.ReadAllText(tool)).Value.Replace("<title>", "").Replace("</title>", "");
                    function.Click += new EventHandler((s, e) =>
                    {
                        string url = new Uri(tool).AbsoluteUri;
                        foreach (var control in ToolsViewer.SelectedTab.Controls)
                        {
                            if (control is ChromiumWebBrowser browser)
                            {
                                browser.LoadUrl(url);
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
                    browser.LoadUrl(HomePageUrl);
                }
            }
        }

        private void ReloadToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChooseToolToolStripMenuItem.DropDownItems.Clear();
            LoadTools();
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseTab(ToolsViewer.SelectedTab);
        }

        private void CloseTab(TabPage page)
        {
            if (page == null)
            {
                return;
            }

            if (ToolsViewer.TabPages.IndexOf(page) == 0)
            {
                MessageBox.Show("这是置顶标签页，请不要关闭", "XTools", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ToolsViewer.TabPages.Remove(page);
            page.Dispose();

            if (ToolsViewer.TabPages.Count > 0)
            {
                ToolsViewer.SelectedIndex = ToolsViewer.TabPages.Count - 1;
            }
        }

        public void ToolBrowser_TitleChanged(object sender, TitleChangedEventArgs e)
        {
            Title = e.Title;

            ChromiumWebBrowser browser = sender as ChromiumWebBrowser;
            TabPage page = browser == null ? null : browser.Parent as TabPage;
            if (page != null && !string.IsNullOrWhiteSpace(e.Title))
            {
                page.Text = e.Title;
            }
        }

        public void ToolBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            ChromiumWebBrowser browser = sender as ChromiumWebBrowser;
            if (browser == null)
            {
                return;
            }

            TabPage page = browser.Parent as TabPage;
            if (page != null && string.IsNullOrWhiteSpace(page.Text))
            {
                page.Text = "新标签页";
            }

            string finalUrl = browser.Address;
            if (string.IsNullOrEmpty(finalUrl))
            {
                return;
            }
            if (Histories.Contains(finalUrl) || string.Equals(finalUrl, HomePageUrl, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            Histories.AddLast(finalUrl);
            if (Histories.Count > 10)
            {
                Histories.RemoveFirst();
                HistoryToolStripMenuItem.DropDownItems.Remove(HistoryToolStripMenuItem.DropDownItems[0]);
            }
            ToolStripMenuItem history = new ToolStripMenuItem();
            history.Name = finalUrl;
            history.Text = Title;
            history.Click += new EventHandler((_s, _e) =>
            {
                foreach (var control in ToolsViewer.SelectedTab.Controls)
                {
                    if (control is ChromiumWebBrowser tabBrowser)
                    {
                        tabBrowser.LoadUrl(finalUrl);
                    }
                }
            });
            HistoryToolStripMenuItem.DropDownItems.Add(history);
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
            xTools.Invoke(new MethodInvoker(delegate
            {
                TabPage tabPage = new TabPage("加载中...");
                ChromiumWebBrowser newTool = new ChromiumWebBrowser();
                newTool.ActivateBrowserOnCreation = false;
                newTool.Dock = DockStyle.Fill;
                newTool.Location = new System.Drawing.Point(3, 3);
                newTool.Margin = new Padding(4);
                newTool.MinimumSize = new System.Drawing.Size(27, 25);
                newTool.Name = "ToolBrowser";
                newTool.Size = new System.Drawing.Size(1253, 596);
                newTool.TabIndex = 1;
                newTool.LifeSpanHandler = this;
                newTool.TitleChanged += xTools.ToolBrowser_TitleChanged;
                newTool.FrameLoadEnd += xTools.ToolBrowser_FrameLoadEnd;
                newTool.FrameLoadEnd += (sender, e) =>
                {
                    if (e.Frame.IsMain)
                    {
                        isInternalNavigation = false;
                    }
                };
                tabPage.Controls.Add(newTool);
                xTools.ToolsViewer.Controls.Add(tabPage);
                xTools.ToolsViewer.SelectedIndex = xTools.ToolsViewer.TabPages.Count - 1;
                newTool.LoadUrl(targetUrl);
            }));
            return true;
        }
    }
}
