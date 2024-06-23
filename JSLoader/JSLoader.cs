using System;
using System.IO;
using PluginFrame;
using CefSharp;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.Devices;

namespace JSLoader
{
    public class JSLoader : IPlugin
    {
        public string Name => "JavaScript加载器";

        public string Version => "1.2.2";

        public string Author => "XTools开发组";

        public string Description => "允许用户使用JavaScript脚本修改网页内容";

        public void Main(XTools.XTools xTools)
        {
            if (!Directory.Exists("scripts"))
            {
                Directory.CreateDirectory("scripts");
            }
            string[] scripts = Directory.GetFiles("scripts", "*.js");
            ToolStripMenuItem scriptItems = new ToolStripMenuItem();
            scriptItems.Name = "scripts";
            scriptItems.Text = "脚本";
            xTools.FunctionsMenu.Items.Add(scriptItems);
            foreach (string script in scripts)
            {
                xTools.ToolBrowser.AddressChanged += (s, e) =>
                {
                    xTools.ToolBrowser.ExecuteScriptAsyncWhenPageLoaded(File.ReadAllText(script));
                };
                ToolStripMenuItem jsScript = new ToolStripMenuItem();
                jsScript.Name = script;
                jsScript.Text = Regex.Match(File.ReadAllText(script), "@name\\s+(.*)").Value.Replace("@name ", "").Replace("\n", "").Replace("\r", "");
                scriptItems.DropDownItems.Add(jsScript);
                ToolStripMenuItem scriptInfo = new ToolStripMenuItem();
                scriptInfo.Name = script;
                scriptInfo.Text = "脚本信息";
                scriptInfo.Click += new EventHandler((s, e) =>
                {
                    MessageBox.Show(
                         $"脚本注册名：{script.Split('\\')[1].Replace(".js", "")}\n" +
                         $"脚本名称：{jsScript.Text}\n" +
                         $"脚本版本：v{Regex.Match(File.ReadAllText(script), "@version\\s+(.*)").Value.Replace("@version ", "")}\n" +
                         $"脚本作者：{Regex.Match(File.ReadAllText(script), "@author\\s+(.*)").Value.Replace("@author ", "")}\n" +
                         $"\n{Regex.Match(File.ReadAllText(script), "@description\\s+(.*)").Value.Replace("@description ", "")}",
                         "脚本信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                });
                jsScript.DropDownItems.Add(scriptInfo);
                ToolStripMenuItem disableScript = new ToolStripMenuItem();
                disableScript.Name = script;
                disableScript.Text = "禁用此脚本";
                Computer computer = new Computer();
                disableScript.Click += new EventHandler((sender, e) =>
                {
                    computer.FileSystem.RenameFile(script, script.Split('\\')[1] + ".disabled");
                    MessageBox.Show("已禁用此脚本，重启后生效", "需要重启",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                });
                jsScript.DropDownItems.Add(disableScript);
            }
            string[] disabledScripts = Directory.GetFiles("scripts", "*.disabled");
            foreach (string disabledJsScript in disabledScripts)
            {
                ToolStripMenuItem disabledScript = new ToolStripMenuItem();
                disabledScript.Name = disabledJsScript;
                disabledScript.Text = disabledJsScript.Split('\\')[1].Replace(".disabled", "") + "（已禁用）";
                ToolStripMenuItem enablePlugin = new ToolStripMenuItem();
                enablePlugin.Name = disabledJsScript;
                enablePlugin.Text = "启用此脚本";
                Computer computer = new Computer();
                enablePlugin.Click += new EventHandler((sender, e) =>
                {
                    computer.FileSystem.RenameFile(disabledJsScript, disabledJsScript.Split('\\')[1].Replace(".disabled", ""));
                    MessageBox.Show("已启用此脚本，重启后生效", "需要重启",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                });
                disabledScript.DropDownItems.Add(enablePlugin);
                scriptItems.DropDownItems.Add(disabledScript);
            }
        }
    }
}
