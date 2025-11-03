namespace FileUpload.Models
{
    /// <summary>
    /// 文件上传接口响应
    /// </summary>
    public class ApiResponse
    {
        public string? COMMAND { get; set; }
        public string? LINE { get; set; }
        public string? STATION_NAME { get; set; }
        public string? BARCODE { get; set; }
        public string? TOOLINGNO { get; set; }
        public string? CAVITYNO { get; set; }
        public string? MACHINENO { get; set; }
        public string? LOTNO { get; set; }
        public string? RESULT { get; set; }
        public string? RESULT_INFO { get; set; }
    }
}
