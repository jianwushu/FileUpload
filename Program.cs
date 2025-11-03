using FileUpload.Services;
using System.Globalization;

namespace FileUpload
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 加载配置并设置语言
            try
            {
                var config = ConfigManager.LoadConfig();

                // 设置应用程序语言
                if (!string.IsNullOrEmpty(config.Language))
                {
                    var culture = new CultureInfo(config.Language);
                    Thread.CurrentThread.CurrentUICulture = culture;
                    Thread.CurrentThread.CurrentCulture = culture;
                    CultureInfo.DefaultThreadCurrentUICulture = culture;
                    CultureInfo.DefaultThreadCurrentCulture = culture;
                }
            }
            catch
            {
                // 如果加载配置失败，使用默认语言（中文简体）
                var culture = new CultureInfo("zh-CN");
                Thread.CurrentThread.CurrentUICulture = culture;
                Thread.CurrentThread.CurrentCulture = culture;
            }

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}