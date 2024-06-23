using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CefSharp;
using System.IO;
using CefSharp.WinForms;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace XTools
{
    public partial class XTools : Form
    {
        public IRequestHandler requestHandler;
        public string Title;

        public XTools()
        {
            InitializeComponent();
            ReloadToolsToolStripMenuItem.Visible = false;
            // ToolBrowser.RequestHandler = new RequestHandler();
            requestHandler = new RequestHandler(this);
            ToolBrowser.RequestHandler = requestHandler;
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
            foreach (var control in ToolsViewer.SelectedTab.Controls)
            {
                if (control is ChromiumWebBrowser browser)
                {
                    browser.Refresh();
                }
            }
        }

        private void HomePageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolsViewer.SelectTab(0);
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
                MessageBox.Show("请不要关闭主页", "XTools", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

    public class RequestHandler : IRequestHandler
    {
        public XTools xTools;
        public Thread thread;
        public bool isInternalNavigation = false;

        public RequestHandler(XTools xTools) 
        {
            this.xTools = xTools;
        }

        public bool GetAuthCredentials(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
        {
            return true;
        }

        public IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            return null;
        }

        public bool OnBeforeBrowse(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool userGesture, bool isRedirect)
        {
            if (request.Url == "file:///index.html" || isInternalNavigation)
            {
                return false;
            }
            browser = null;
            isInternalNavigation = true;
            TabPage tabPage = new TabPage(request.Url);
            ChromiumWebBrowser NewTool = new ChromiumWebBrowser();
            NewTool.ActivateBrowserOnCreation = false;
            NewTool.Dock = DockStyle.Fill;
            NewTool.Location = new System.Drawing.Point(3, 3);
            NewTool.Margin = new Padding(4);
            NewTool.MinimumSize = new System.Drawing.Size(27, 25);
            NewTool.Name = "ToolBrowser";
            NewTool.Size = new System.Drawing.Size(1253, 596);
            NewTool.TabIndex = 0;
            NewTool.RequestHandler = this;
            NewTool.TitleChanged += new EventHandler<TitleChangedEventArgs>(xTools.ToolBrowser_TitleChanged);
            NewTool.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(xTools.ToolBrowser_FrameLoadEnd);
            NewTool.FrameLoadEnd += (sender, e) =>
            {
                if (e.Frame.IsMain)
                {
                    isInternalNavigation = false;
                }
            };
            NewTool.LoadUrl(request.Url);
            tabPage.Controls.Add(NewTool);
            xTools.Invoke(new MethodInvoker(delegate
            {
                xTools.ToolsViewer.Controls.Add(tabPage);
                xTools.ToolsViewer.SelectedIndex = xTools.ToolsViewer.TabPages.Count - 1;
            }));
            return true;
        }

        public bool OnCertificateError(IWebBrowser chromiumWebBrowser, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
        {
            return false;
        }

        public void OnDocumentAvailableInMainFrame(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            
        }

        public bool OnOpenUrlFromTab(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
        {
            return false;
        }

        public bool OnQuotaRequest(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, long newSize, IRequestCallback callback)
        {
            return false;
        }

        public void OnRenderProcessTerminated(IWebBrowser chromiumWebBrowser, IBrowser browser, CefTerminationStatus status)
        {
            
        }

        public void OnRenderViewReady(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            
        }

        public bool OnSelectClientCertificate(IWebBrowser chromiumWebBrowser, IBrowser browser, bool isProxy, string host, int port, X509Certificate2Collection certificates, ISelectClientCertificateCallback callback)
        {
            return true;
        }
    }
}
