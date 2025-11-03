using System.Globalization;
using System.Resources;

namespace FileUpload.Services
{
    /// <summary>
    /// 简单的本地化服务：通过 Resources/Strings(.culture).resx 提供多语言字符串
    /// </summary>
    public static class LocalizationService
    {
        private static readonly ResourceManager _rm = new ResourceManager("FileUpload.Resources.Strings", typeof(LocalizationService).Assembly);

        /// <summary>
        /// 获取本地化字符串，未命中时返回 key 本身，便于发现缺失
        /// </summary>
        public static string T(string key)
        {
            try
            {
                var value = _rm.GetString(key, CultureInfo.CurrentUICulture);
                return string.IsNullOrEmpty(value) ? key : value!;
            }
            catch
            {
                return key;
            }
        }

        /// <summary>
        /// 设置应用的当前文化（同时设置 UI 与格式化文化）
        /// </summary>
        public static void SetCulture(string culture)
        {
            var ci = new CultureInfo(culture);
            CultureInfo.CurrentUICulture = ci;
            CultureInfo.CurrentCulture = ci;
            CultureInfo.DefaultThreadCurrentUICulture = ci;
            CultureInfo.DefaultThreadCurrentCulture = ci;
        }
    }
}

