
namespace FileUpload.utils
{
    public class ToolHelper
    {
        public static string GetLocalIPAddress()
        {
            try
            {
                var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
            }
            catch
            {
                // 忽略错误
            }
            return "127.0.0.1";
        }
    }
}
