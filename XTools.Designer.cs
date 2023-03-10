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
            this.ToolBrowser = new CefSharp.WinForms.ChromiumWebBrowser();
            this.FunctionsMenu = new System.Windows.Forms.MenuStrip();
            this.ChooseToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RefreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HomePageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ReloadToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FunctionsMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // ToolBrowser
            // 
            this.ToolBrowser.ActivateBrowserOnCreation = false;
            this.ToolBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ToolBrowser.Location = new System.Drawing.Point(0, 25);
            this.ToolBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.ToolBrowser.Name = "ToolBrowser";
            this.ToolBrowser.Size = new System.Drawing.Size(937, 499);
            this.ToolBrowser.TabIndex = 0;
            // 
            // FunctionsMenu
            // 
            this.FunctionsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ChooseToolToolStripMenuItem,
            this.ReloadToolsToolStripMenuItem,
            this.RefreshToolStripMenuItem,
            this.HomePageToolStripMenuItem,
            this.PluginsToolStripMenuItem});
            this.FunctionsMenu.Location = new System.Drawing.Point(0, 0);
            this.FunctionsMenu.Name = "FunctionsMenu";
            this.FunctionsMenu.Size = new System.Drawing.Size(937, 25);
            this.FunctionsMenu.TabIndex = 1;
            this.FunctionsMenu.Text = "menuStrip1";
            // 
            // ChooseToolToolStripMenuItem
            // 
            this.ChooseToolToolStripMenuItem.Name = "ChooseToolToolStripMenuItem";
            this.ChooseToolToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.ChooseToolToolStripMenuItem.Text = "选择工具";
            // 
            // RefreshToolStripMenuItem
            // 
            this.RefreshToolStripMenuItem.Name = "RefreshToolStripMenuItem";
            this.RefreshToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.RefreshToolStripMenuItem.Text = "刷新";
            this.RefreshToolStripMenuItem.Click += new System.EventHandler(this.RefreshToolStripMenuItem_Click);
            // 
            // HomePageToolStripMenuItem
            // 
            this.HomePageToolStripMenuItem.Name = "HomePageToolStripMenuItem";
            this.HomePageToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.HomePageToolStripMenuItem.Text = "返回主页";
            this.HomePageToolStripMenuItem.Click += new System.EventHandler(this.HomePageToolStripMenuItem_Click);
            // 
            // PluginsToolStripMenuItem
            // 
            this.PluginsToolStripMenuItem.Name = "PluginsToolStripMenuItem";
            this.PluginsToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.PluginsToolStripMenuItem.Text = "插件";
            // 
            // ReloadToolsToolStripMenuItem
            // 
            this.ReloadToolsToolStripMenuItem.Name = "ReloadToolsToolStripMenuItem";
            this.ReloadToolsToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.ReloadToolsToolStripMenuItem.Text = "重载工具";
            this.ReloadToolsToolStripMenuItem.Click += new System.EventHandler(this.ReloadToolsToolStripMenuItem_Click);
            // 
            // XTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(937, 524);
            this.Controls.Add(this.ToolBrowser);
            this.Controls.Add(this.FunctionsMenu);
            this.MainMenuStrip = this.FunctionsMenu;
            this.Name = "XTools";
            this.Text = "XTools";
            this.FunctionsMenu.ResumeLayout(false);
            this.FunctionsMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public ChromiumWebBrowser ToolBrowser;
        public System.Windows.Forms.MenuStrip FunctionsMenu;
        public System.Windows.Forms.ToolStripMenuItem ChooseToolToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem RefreshToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem HomePageToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem PluginsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ReloadToolsToolStripMenuItem;
    }
}
