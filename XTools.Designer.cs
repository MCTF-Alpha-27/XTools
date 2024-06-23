using CefSharp.WinForms;

namespace XTools
{
    partial class XTools
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        public System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        public void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XTools));
            this.FunctionsMenu = new System.Windows.Forms.MenuStrip();
            this.ChooseToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ReloadToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RefreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HomePageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CloseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ToolBrowser = new CefSharp.WinForms.ChromiumWebBrowser();
            this.ToolsViewer = new System.Windows.Forms.TabControl();
            this.FunctionsMenu.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.ToolsViewer.SuspendLayout();
            this.SuspendLayout();
            // 
            // FunctionsMenu
            // 
            this.FunctionsMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.FunctionsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ChooseToolToolStripMenuItem,
            this.ReloadToolsToolStripMenuItem,
            this.RefreshToolStripMenuItem,
            this.HomePageToolStripMenuItem,
            this.CloseToolStripMenuItem,
            this.PluginsToolStripMenuItem});
            this.FunctionsMenu.Location = new System.Drawing.Point(0, 0);
            this.FunctionsMenu.Name = "FunctionsMenu";
            this.FunctionsMenu.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.FunctionsMenu.Size = new System.Drawing.Size(1249, 29);
            this.FunctionsMenu.TabIndex = 1;
            this.FunctionsMenu.Text = "menuStrip1";
            // 
            // ChooseToolToolStripMenuItem
            // 
            this.ChooseToolToolStripMenuItem.Name = "ChooseToolToolStripMenuItem";
            this.ChooseToolToolStripMenuItem.Size = new System.Drawing.Size(88, 25);
            this.ChooseToolToolStripMenuItem.Text = "选择工具";
            // 
            // ReloadToolsToolStripMenuItem
            // 
            this.ReloadToolsToolStripMenuItem.Name = "ReloadToolsToolStripMenuItem";
            this.ReloadToolsToolStripMenuItem.Size = new System.Drawing.Size(88, 25);
            this.ReloadToolsToolStripMenuItem.Text = "重载工具";
            this.ReloadToolsToolStripMenuItem.Click += new System.EventHandler(this.ReloadToolsToolStripMenuItem_Click);
            // 
            // RefreshToolStripMenuItem
            // 
            this.RefreshToolStripMenuItem.Name = "RefreshToolStripMenuItem";
            this.RefreshToolStripMenuItem.Size = new System.Drawing.Size(56, 25);
            this.RefreshToolStripMenuItem.Text = "刷新";
            this.RefreshToolStripMenuItem.Click += new System.EventHandler(this.RefreshToolStripMenuItem_Click);
            // 
            // HomePageToolStripMenuItem
            // 
            this.HomePageToolStripMenuItem.Name = "HomePageToolStripMenuItem";
            this.HomePageToolStripMenuItem.Size = new System.Drawing.Size(88, 25);
            this.HomePageToolStripMenuItem.Text = "返回主页";
            this.HomePageToolStripMenuItem.Click += new System.EventHandler(this.HomePageToolStripMenuItem_Click);
            // 
            // CloseToolStripMenuItem
            // 
            this.CloseToolStripMenuItem.Name = "CloseToolStripMenuItem";
            this.CloseToolStripMenuItem.Size = new System.Drawing.Size(104, 25);
            this.CloseToolStripMenuItem.Text = "关闭此页面";
            this.CloseToolStripMenuItem.Click += new System.EventHandler(this.CloseToolStripMenuItem_Click);
            // 
            // PluginsToolStripMenuItem
            // 
            this.PluginsToolStripMenuItem.Name = "PluginsToolStripMenuItem";
            this.PluginsToolStripMenuItem.Size = new System.Drawing.Size(56, 25);
            this.PluginsToolStripMenuItem.Text = "插件";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ToolBrowser);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1259, 602);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ToolBrowser
            // 
            this.ToolBrowser.ActivateBrowserOnCreation = false;
            this.ToolBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ToolBrowser.Location = new System.Drawing.Point(3, 3);
            this.ToolBrowser.Margin = new System.Windows.Forms.Padding(4);
            this.ToolBrowser.MinimumSize = new System.Drawing.Size(27, 25);
            this.ToolBrowser.Name = "ToolBrowser";
            this.ToolBrowser.Size = new System.Drawing.Size(1253, 596);
            this.ToolBrowser.TabIndex = 1;
            this.ToolBrowser.TitleChanged += new System.EventHandler<CefSharp.TitleChangedEventArgs>(this.ToolBrowser_TitleChanged);
            this.ToolBrowser.FrameLoadEnd += new System.EventHandler<CefSharp.FrameLoadEndEventArgs>(this.ToolBrowser_FrameLoadEnd);
            // 
            // ToolsViewer
            // 
            this.ToolsViewer.Controls.Add(this.tabPage1);
            this.ToolsViewer.Location = new System.Drawing.Point(-7, 32);
            this.ToolsViewer.Name = "ToolsViewer";
            this.ToolsViewer.SelectedIndex = 0;
            this.ToolsViewer.Size = new System.Drawing.Size(1267, 631);
            this.ToolsViewer.TabIndex = 2;
            // 
            // XTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1249, 655);
            this.Controls.Add(this.ToolsViewer);
            this.Controls.Add(this.FunctionsMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.FunctionsMenu;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "XTools";
            this.Text = "XTools";
            this.Resize += new System.EventHandler(this.XTools_Resize);
            this.FunctionsMenu.ResumeLayout(false);
            this.FunctionsMenu.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.ToolsViewer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.MenuStrip FunctionsMenu;
        public System.Windows.Forms.ToolStripMenuItem ChooseToolToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem RefreshToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem HomePageToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem PluginsToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem ReloadToolsToolStripMenuItem;
        public System.Windows.Forms.TabPage tabPage1;
        public ChromiumWebBrowser ToolBrowser;
        public System.Windows.Forms.TabControl ToolsViewer;
        private System.Windows.Forms.ToolStripMenuItem CloseToolStripMenuItem;
    }
}
