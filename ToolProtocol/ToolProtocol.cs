using CefSharp;
using CefSharp.SchemeHandler;
using CefSharp.WinForms;
using PluginFrame;
using System;
using System.IO;

namespace ToolProtocol
{
    public class ToolProtocol : IPlugin
    {
        public string Name => "工具协议";
        public string Version => "1.0.0";
        public string Author => "XTools开发组";
        public string Description => "新增tools协议，允许开发者通过tools://local/<某工具目录>访问工具的本地文件";

        public void Main(XTools.XTools xTools)
        {
            var settings = new CefSettings();
            settings.RegisterScheme(new CefCustomScheme
            {
                SchemeName = "tools",
                SchemeHandlerFactory = new FolderSchemeHandlerFactory(
                    rootFolder: Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tools"),
                    hostName: "local"
                )
            });
            Cef.Initialize(settings);
        }
    }
}
