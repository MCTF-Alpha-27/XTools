using System;
using System.Collections.Generic;
using System.IO;
using PluginFrame;
using CefSharp;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.Devices;
using CefSharp.WinForms;

namespace JSLoader
{
    public class JSLoader : IPlugin
    {
        public string Name => "JavaScript加载器";
        public string Version => "1.3.0";
        public string Author => "XTools开发组";
        public string Description => "允许用户使用JavaScript脚本修改网页内容";

        public void Main(XTools.XTools xTools)
        {
            string scriptDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "scripts");
            if (!Directory.Exists(scriptDirectory))
            {
                Directory.CreateDirectory(scriptDirectory);
            }

            string[] scripts = Directory.GetFiles(scriptDirectory, "*.js");
            ToolStripMenuItem scriptItems = new ToolStripMenuItem();
            scriptItems.Name = "scripts";
            scriptItems.Text = "脚本";
            xTools.FunctionsMenu.Items.Add(scriptItems);

            HashSet<ChromiumWebBrowser> attachedBrowsers = new HashSet<ChromiumWebBrowser>();

            // 页面每次加载完成后执行全部脚本；同时覆盖插件加载前已经打开的首页。
            Action<ChromiumWebBrowser> attachScripts = browser =>
            {
                if (browser == null || !attachedBrowsers.Add(browser))
                {
                    return;
                }

                browser.FrameLoadEnd += (sender, e) =>
                {
                    if (!e.Frame.IsMain)
                    {
                        return;
                    }

                    foreach (string script in scripts)
                    {
                        try
                        {
                            browser.ExecuteScriptAsync(File.ReadAllText(script));
                        }
                        catch
                        {
                            // 页面正在销毁或脚本读取失败时跳过，不影响其它脚本。
                        }
                    }
                };

                if (!browser.IsLoading)
                {
                    foreach (string script in scripts)
                    {
                        try
                        {
                            browser.ExecuteScriptAsync(File.ReadAllText(script));
                        }
                        catch
                        {
                        }
                    }
                }
            };

            Action applyToOpenedTabs = () =>
            {
                foreach (TabPage tabPage in xTools.ToolsViewer.TabPages)
                {
                    foreach (Control control in tabPage.Controls)
                    {
                        ChromiumWebBrowser browser = control as ChromiumWebBrowser;
                        if (browser != null)
                        {
                            attachScripts(browser);
                        }
                    }
                }
            };

            xTools.ToolsViewer.ControlAdded += (sender, e) => applyToOpenedTabs();
            applyToOpenedTabs();
            attachScripts(xTools.ToolBrowser);

            foreach (string script in scripts)
            {
                string scriptFileName = Path.GetFileName(script);
                string scriptText = File.ReadAllText(script);
                string scriptName = Regex.Match(scriptText, "@name\\s+(.*)").Value
                    .Replace("@name ", string.Empty)
                    .Replace("\n", string.Empty)
                    .Replace("\r", string.Empty);

                ToolStripMenuItem jsScript = new ToolStripMenuItem();
                jsScript.Name = script;
                jsScript.Text = scriptName;
                scriptItems.DropDownItems.Add(jsScript);

                ToolStripMenuItem scriptInfo = new ToolStripMenuItem();
                scriptInfo.Name = script;
                scriptInfo.Text = "脚本信息";
                scriptInfo.Click += (sender, e) =>
                {
                    MessageBox.Show(
                        $"脚本注册名：{Path.GetFileNameWithoutExtension(scriptFileName)}\n" +
                        $"脚本名称：{scriptName}\n" +
                        $"脚本版本：v{Regex.Match(scriptText, "@version\\s+(.*)").Value.Replace("@version ", string.Empty)}\n" +
                        $"脚本作者：{Regex.Match(scriptText, "@author\\s+(.*)").Value.Replace("@author ", string.Empty)}\n" +
                        $"\n{Regex.Match(scriptText, "@description\\s+(.*)").Value.Replace("@description ", string.Empty)}",
                        "脚本信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                };
                jsScript.DropDownItems.Add(scriptInfo);

                ToolStripMenuItem disableScript = new ToolStripMenuItem();
                disableScript.Name = script;
                disableScript.Text = "禁用此脚本";
                Computer computer = new Computer();
                disableScript.Click += (sender, e) =>
                {
                    try
                    {
                        computer.FileSystem.RenameFile(script, scriptFileName + ".disabled");
                        MessageBox.Show("已禁用此脚本，重启后生效", "需要重启",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, "无法禁用脚本",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                jsScript.DropDownItems.Add(disableScript);
            }

            string[] disabledScripts = Directory.GetFiles(scriptDirectory, "*.disabled");
            foreach (string disabledScript in disabledScripts)
            {
                string disabledFileName = Path.GetFileName(disabledScript);
                string enabledFileName = disabledFileName.Replace(".disabled", string.Empty);

                ToolStripMenuItem disabledItem = new ToolStripMenuItem();
                disabledItem.Name = disabledScript;
                disabledItem.Text = enabledFileName + "（已禁用）";

                ToolStripMenuItem enableScript = new ToolStripMenuItem();
                enableScript.Name = disabledScript;
                enableScript.Text = "启用此脚本";
                Computer computer = new Computer();
                enableScript.Click += (sender, e) =>
                {
                    try
                    {
                        computer.FileSystem.RenameFile(disabledScript, enabledFileName);
                        MessageBox.Show("已启用此脚本，重启后生效", "需要重启",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, "无法启用脚本",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                disabledItem.DropDownItems.Add(enableScript);
                scriptItems.DropDownItems.Add(disabledItem);
            }
        }
    }
}
