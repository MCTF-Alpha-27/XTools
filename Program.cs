using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.VisualBasic.Devices;
using PluginFrame;

namespace XTools
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            XTools xTools = new XTools();
            try
            {
                string[] pluginFiles = Directory.GetFiles("plugins", "*.dll");
                foreach (string pluginFile in pluginFiles)
                {
                    Assembly assembly = Assembly.LoadFrom(pluginFile);
                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        if (type.GetInterface("IPlugin") != null)
                        {
                            IPlugin plugin = Activator.CreateInstance(type) as IPlugin;
                            ToolStripMenuItem pluginItems = new ToolStripMenuItem();
                            pluginItems.Name = type.Name;
                            pluginItems.Text = plugin.Name;
                            ToolStripMenuItem pluginInfos = new ToolStripMenuItem();
                            pluginInfos.Name = type.Name + "_info";
                            pluginInfos.Text = "插件信息";
                            pluginInfos.Click += new EventHandler((sender, e) =>
                            {
                                MessageBox.Show(
                                    $"插件注册名：{type.Name}\n" +
                                    $"插件名称：{plugin.Name}\n" +
                                    $"插件版本：v{plugin.Version}\n" +
                                    $"插件作者：{plugin.Author}\n" +
                                    $"\n{plugin.Description}",
                                    "插件信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            });
                            ToolStripMenuItem disablePlugin = new ToolStripMenuItem();
                            disablePlugin.Name = type.Name;
                            disablePlugin.Text = "禁用此插件";
                            Computer computer = new Computer();
                            disablePlugin.Click += new EventHandler((sender, e) =>
                            {
                                computer.FileSystem.RenameFile(pluginFile, pluginFile.Split('\\')[1] + ".disabled");
                                MessageBox.Show("已禁用此插件，重启后生效", "需要重启",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            });
                            pluginItems.DropDownItems.Add(pluginInfos);
                            pluginItems.DropDownItems.Add(disablePlugin);
                            xTools.PluginsToolStripMenuItem.DropDownItems.Add(pluginItems);
                            plugin.Main(xTools);
                        }
                    }
                }
                string[] disabledPluginFiles = Directory.GetFiles("plugins", "*.disabled");
                foreach (string disabledPluginFile in disabledPluginFiles)
                {
                    ToolStripMenuItem disabledPlugin = new ToolStripMenuItem();
                    disabledPlugin.Name = disabledPluginFile;
                    disabledPlugin.Text = disabledPluginFile.Split('\\')[1].Replace(".disabled", "") + "（已禁用）";
                    ToolStripMenuItem enablePlugin = new ToolStripMenuItem();
                    enablePlugin.Name = disabledPluginFile;
                    enablePlugin.Text = "启用此插件";
                    Computer computer = new Computer();
                    enablePlugin.Click += new EventHandler((sender, e) =>
                    {
                        computer.FileSystem.RenameFile(disabledPluginFile, disabledPluginFile.Split('\\')[1].Replace(".disabled", ""));
                        MessageBox.Show("已启用此插件，重启后生效", "需要重启",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    });
                    disabledPlugin.DropDownItems.Add(enablePlugin);
                    xTools.PluginsToolStripMenuItem.DropDownItems.Add(disabledPlugin);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "加载插件时出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Application.Run(xTools);
        }
    }
}
