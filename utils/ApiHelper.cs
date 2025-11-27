using System.Text;
using System.Net;
using Newtonsoft.Json;

namespace FileUpload.utils
{
    /// <summary>
    /// API调用统一管理类
    /// </summary>
    public class ApiHelper
    {
        private static readonly HttpClient _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(30)
        };

        /// <summary>
        /// 设置HTTP客户端超时时间
        /// </summary>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        public static void SetTimeout(int timeoutSeconds)
        {
            _httpClient.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
        }

        /// <summary>
        /// API响应结果
        /// </summary>
        public class ApiResult
        {
            /// <summary>
            /// 是否成功
            /// </summary>
            public bool Success { get; set; }

            /// <summary>
            /// HTTP状态码
            /// </summary>
            public HttpStatusCode StatusCode { get; set; }

            /// <summary>
            /// 响应内容
            /// </summary>
            public string? Content { get; set; }

            /// <summary>
            /// 错误消息
            /// </summary>
            public string? ErrorMessage { get; set; }

            /// <summary>
            /// 响应数据（泛型）
            /// </summary>
            public DataResult? Data { get; set; }
        }

        public class DataResult
        {
            public int Code { get; set; }

            public string Msg { get; set; } 

            public object? Data { get; set; }

            public object? Extra { get; set; }

        }


        private static async Task<ApiResult> ApiRequest(string apiUrl, HttpMethod method,object body)
        {            
            using var request = new HttpRequestMessage(method, apiUrl);
            if (body != null)
            {
                var json = JsonConvert.SerializeObject(body);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            using HttpResponseMessage response = await _httpClient.SendAsync(request).ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return new ApiResult()
                {
                    Success = false,
                    StatusCode = response.StatusCode,
                    ErrorMessage = "接口请求失败"
                };
            }
            else
            {
                var rsp = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var data = JsonConvert.DeserializeObject<DataResult>(rsp);

                return new ApiResult()
                {
                    Success = true,
                    StatusCode = response.StatusCode,
                    Content = rsp,
                    Data = data
                };
            }
        }

        /// <summary>
        /// 服务发现注册（带端点）
        /// </summary>
        /// <param name="apiUrl">服务中心API地址</param>
        /// <param name="serviceName">服务名称</param>
        /// <param name="version">版本号</param>
        /// <param name="endpoint">服务端点</param>
        /// <param name="ttl">生存时间</param>
        /// <returns>API调用结果</returns>
        public static async Task<ApiResult> RegisterServiceDiscoveryAsync(
            string apiUrl,
            string serviceName,
            string version = "V1.0.0",
            string ttl = "60")
        {
            try
            {
                var registrationData = new
                {
                    serviceName,
                    version,
                    endpoint = ToolHelper.GetLocalIPAddress(),
                    ttl
                };

                return await ApiRequest($"{apiUrl.TrimEnd('/')}/sc/open/register", HttpMethod.Post, registrationData);

            }
            catch (Exception ex)
            {
                return new ApiResult
                {
                    Success = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// 发送心跳
        /// </summary>
        /// <param name="apiUrl">服务中心API地址</param>
        /// <param name="deviceId">设备ID</param>
        /// <param name="serviceName">服务名称</param>
        /// <param name="status">状态</param>
        /// <returns>API调用结果</returns>
        public static async Task<ApiResult> SendHeartbeatAsync(string apiUrl,string serviceName)
        {
            try
            {
                return await ApiRequest($"{apiUrl.TrimEnd('/')}/sc/open/heart/{serviceName}", HttpMethod.Get, null);
            }
            catch (Exception ex)
            {
                return new ApiResult
                {
                    Success = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// 上传配置
        /// </summary>
        /// <param name="apiUrl">服务中心API地址</param>
        /// <param name="serviceName">服务名称</param>
        /// <param name="configContent">配置内容</param>
        /// <param name="version">版本号</param>
        /// <param name="environment">环境</param>
        /// <returns>API调用结果</returns>
        public static async Task<ApiResult> UploadConfigAsync(
            string apiUrl,
            string serviceName,
            object configContent,
            string version = "V1.0.0",
            string environment = "prod")
        {
            try
            {
                var uploadData = new
                {
                    serviceName,
                    version,
                    environment,
                    content = JsonConvert.SerializeObject(configContent)
                };

                return await ApiRequest($"{apiUrl.TrimEnd('/')}/sc/open/upConfig", HttpMethod.Post, uploadData);
            }
            catch (Exception ex)
            {
                return new ApiResult
                {
                    Success = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// 下载配置
        /// </summary>
        /// <param name="apiUrl">服务中心API地址</param>
        /// <param name="serviceName">服务名称</param>
        /// <returns>API调用结果</returns>
        public static async Task<ApiResult> DownloadConfigAsync(string apiUrl,string serviceName)
        {
            try
            {
                return await ApiRequest($"{apiUrl.TrimEnd('/')}/sc/open/latest?serviceName={serviceName}", HttpMethod.Get, null);
            }
            catch (Exception ex)
            {
                return new ApiResult
                {
                    Success = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
