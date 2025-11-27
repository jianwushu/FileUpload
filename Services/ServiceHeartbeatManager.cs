using FileUpload.Models;
using FileUpload.utils;

namespace FileUpload.Services
{
    /// <summary>
    /// 服务心跳管理器
    /// </summary>
    public class ServiceHeartbeatManager : IDisposable
    {
        private System.Threading.Timer? _heartbeatTimer;
        private AppConfig _config;
        private bool _isRegistered;
        private bool _disposed;

        /// <summary>
        /// 心跳状态变化事件
        /// </summary>
        public event EventHandler<HeartbeatStatusEventArgs>? HeartbeatStatusChanged;

        public ServiceHeartbeatManager()
        {
            _config = ConfigManager.GetConfig();
            _isRegistered = false;
        }

        /// <summary>
        /// 启动心跳
        /// </summary>
        public void StartHeartbeat()
        {
            if (!_config.ServiceRegistration.EnableHeartbeat)
            {
                LogManager.LogInfo("心跳功能未启用");
                return;
            }

            if (!_isRegistered)
            {
                LogManager.LogWarning("服务未注册，无法启动心跳");
                return;
            }

            StopHeartbeat();

            var interval = _config.ServiceRegistration.HeartbeatInterval;
            if (interval < 5)
            {
                interval = 5; // 最小5秒
                LogManager.LogWarning($"心跳间隔过小，已调整为{interval}秒");
            }

            _heartbeatTimer = new System.Threading.Timer(
                SendHeartbeat,
                null,
                TimeSpan.FromSeconds(interval),
                TimeSpan.FromSeconds(interval));

            LogManager.LogInfo($"心跳已启动，间隔: {interval}秒");
            OnHeartbeatStatusChanged(new HeartbeatStatusEventArgs
            {
                IsRunning = true,
                Message = $"心跳已启动，间隔: {interval}秒"
            });
        }

        /// <summary>
        /// 停止心跳
        /// </summary>
        public void StopHeartbeat()
        {
            if (_heartbeatTimer != null)
            {
                _heartbeatTimer.Dispose();
                _heartbeatTimer = null;
                LogManager.LogInfo("心跳已停止");
                OnHeartbeatStatusChanged(new HeartbeatStatusEventArgs
                {
                    IsRunning = false,
                    Message = "心跳已停止"
                });
            }
        }

        /// <summary>
        /// 设置注册状态
        /// </summary>
        public void SetRegistrationStatus(bool isRegistered)
        {
            _isRegistered = isRegistered;
            
            if (isRegistered && _config.ServiceRegistration.EnableHeartbeat)
            {
                StartHeartbeat();
            }
            else if (!isRegistered)
            {
                StopHeartbeat();
            }
        }

        /// <summary>
        /// 重新加载配置
        /// </summary>
        public void ReloadConfig()
        {
            _config = ConfigManager.GetConfig();
            
            // 如果心跳正在运行，重启以应用新配置
            if (_heartbeatTimer != null && _isRegistered)
            {
                StartHeartbeat();
            }
        }

        /// <summary>
        /// 发送心跳包
        /// </summary>
        private async void SendHeartbeat(object? state)
        {
            try
            {
                var result = await ApiHelper.SendHeartbeatAsync(
                    _config.ServiceRegistration.ServiceCenterApi,
                    _config.DeviceId);

                if (result.Success)
                {
                    _config.ServiceRegistration.LastHeartbeatTime = DateTime.Now;
                    LogManager.LogInfo($"心跳发送成功: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

                    OnHeartbeatStatusChanged(new HeartbeatStatusEventArgs
                    {
                        IsRunning = true,
                        IsSuccess = true,
                        Message = $"心跳发送成功: {DateTime.Now:HH:mm:ss}",
                        LastHeartbeatTime = DateTime.Now
                    });
                }
                else
                {
                    LogManager.LogError($"心跳发送失败: {result.StatusCode} - {result.ErrorMessage}");

                    OnHeartbeatStatusChanged(new HeartbeatStatusEventArgs
                    {
                        IsRunning = true,
                        IsSuccess = false,
                        Message = $"心跳发送失败: {result.StatusCode}",
                        ErrorMessage = result.ErrorMessage
                    });
                }
            }
            catch (Exception ex)
            {
                LogManager.LogError($"心跳发送异常: {ex.Message}");

                OnHeartbeatStatusChanged(new HeartbeatStatusEventArgs
                {
                    IsRunning = true,
                    IsSuccess = false,
                    Message = "心跳发送异常",
                    ErrorMessage = ex.Message
                });
            }
        }

        /// <summary>
        /// 触发心跳状态变化事件
        /// </summary>
        protected virtual void OnHeartbeatStatusChanged(HeartbeatStatusEventArgs e)
        {
            HeartbeatStatusChanged?.Invoke(this, e);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                StopHeartbeat();
                _disposed = true;
            }
        }
    }

    /// <summary>
    /// 心跳状态事件参数
    /// </summary>
    public class HeartbeatStatusEventArgs : EventArgs
    {
        /// <summary>
        /// 心跳是否正在运行
        /// </summary>
        public bool IsRunning { get; set; }

        /// <summary>
        /// 最后一次心跳是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 状态消息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 错误消息
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// 最后心跳时间
        /// </summary>
        public DateTime? LastHeartbeatTime { get; set; }
    }
}

