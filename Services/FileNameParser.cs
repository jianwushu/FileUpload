using FileUpload.Models;
using System.Text;
using System.Text.RegularExpressions;

namespace FileUpload.Services
{
    /// <summary>
    /// 文件名解析服务
    /// </summary>
    public class FileNameParser
    {
        private readonly FileNameParseRules _rules;

        public FileNameParser(FileNameParseRules rules)
        {
            _rules = rules;
        }

        /// <summary>
        /// 从文件名中提取参数
        /// </summary>
        /// <param name="fileName">文件名（不含扩展名）</param>
        /// <returns>提取的参数字典</returns>
        public Dictionary<string, string> ParseFileName(string fileName)
        {
            var result = new Dictionary<string, string>();

            if (!_rules.Enabled)
            {
                return result;
            }

            try
            {
                // 优先使用模板解析
                if (!string.IsNullOrWhiteSpace(_rules.Template))
                {
                    var templateResult = ParseByTemplate(fileName);
                    foreach (var kvp in templateResult)
                    {
                        result[kvp.Key] = kvp.Value;
                    }
                }
                else
                {
                    // 1. 使用位置映射解析（支持单个位置和范围）
                    if (_rules.PositionMapping != null && _rules.PositionMapping.Count > 0)
                    {
                        var segments = fileName.Split(_rules.Separator);
                        foreach (var mapping in _rules.PositionMapping)
                        {
                            var paramName = mapping.Key;
                            var positionOrRange = mapping.Value;

                            try
                            {
                                // 判断是单个位置还是范围
                                if (positionOrRange.Contains("-"))
                                {
                                    // 范围格式: "2-3"
                                    var parts = positionOrRange.Split('-');
                                    if (parts.Length == 2 && int.TryParse(parts[0], out int start) && int.TryParse(parts[1], out int end))
                                    {
                                        if (start >= 0 && end < segments.Length && start <= end)
                                        {
                                            var values = new List<string>();
                                            for (int i = start; i <= end; i++)
                                            {
                                                values.Add(segments[i]);
                                            }
                                            result[paramName] = string.Join(_rules.Separator, values);
                                        }
                                    }
                                    else
                                    {
                                        LogManager.LogWarning($"位置映射格式错误 [{paramName}]: {positionOrRange}");
                                    }
                                }
                                else
                                {
                                    // 单个位置格式: "0"
                                    if (int.TryParse(positionOrRange, out int position))
                                    {
                                        if (position >= 0 && position < segments.Length)
                                        {
                                            result[paramName] = segments[position];
                                        }
                                        else
                                        {
                                            LogManager.LogWarning($"位置映射下标越界 [{paramName}]: {positionOrRange}");
                                        }
                                    }
                                    else
                                    {
                                        LogManager.LogWarning($"位置映射格式错误 [{paramName}]: {positionOrRange}");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                LogManager.LogWarning($"位置映射解析失败 [{paramName}]: {positionOrRange}, 错误: {ex.Message}");
                            }
                        }
                    }

                    // 2. 使用正则表达式解析
                    if (_rules.RegexMapping != null && _rules.RegexMapping.Count > 0)
                    {
                        foreach (var mapping in _rules.RegexMapping)
                        {
                            var paramName = mapping.Key;
                            var pattern = mapping.Value;

                            try
                            {
                                var match = Regex.Match(fileName, pattern);
                                if (match.Success && match.Groups.Count > 1)
                                {
                                    // 使用第一个捕获组的值
                                    result[paramName] = match.Groups[1].Value;
                                }
                            }
                            catch (Exception ex)
                            {
                                LogManager.LogWarning($"正则表达式解析失败 [{paramName}]: {pattern}, 错误: {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.LogError($"文件名解析失败: {fileName}", ex);
            }

            return result;
        }

            /// <summary>
            /// 使用模板解析文件名
            /// </summary>
        private Dictionary<string, string> ParseByTemplate(string fileName)
        {
            var result = new Dictionary<string, string>();

            try
            {
                // 示例模板: {MACHINENO}_{BARCODE}_{TOOLINGNO}_{CAVITYNO}_{CREATETIME}_{}_{}_{FILETYPE}_{FOLDERTYPE}
                // 示例文件名: AOI-021_11S55HFF096X6_R05A_CR0395_A2_20250627215811_GW_1_AOI_OK.jpg
                // 假设 _rules.Separator 是 "_"

                var template = _rules.Template;
                // 如果文件名包含路径，只取文件名部分
                var shortFileName = Path.GetFileName(fileName);

                // 1. 提取所有参数名（占位符）
                var paramNames = new List<string>();
                var paramPattern = @"\{([^}]*)\}";
                var placeholderMatches = Regex.Matches(template, paramPattern);

                // 如果模板中没有占位符，则无法解析
                if (placeholderMatches.Count == 0)
                {
                    LogManager.LogWarning($"模板 \"{template}\" 中不包含任何有效的占位符 {{...}}。");
                    return result;
                }

                foreach (Match match in placeholderMatches)
                {
                    // Groups[0] 是整个匹配项, e.g., "{MACHINENO}"
                    // Groups[1] 是第一个捕获组, e.g., "MACHINENO"
                    paramNames.Add(match.Groups[1].Value);
                }

                // 2. 动态构建正则表达式
                var regexBuilder = new StringBuilder("^");
                var templateParts = Regex.Split(template, paramPattern); // 按占位符分割模板

                // templateParts 会是这样的数组: ["", "MACHINENO", "_", "BARCODE", "_", ... , ""]
                // 我们只需要处理非占位符部分（偶数索引）和占位符（奇数索引代表的内容，但我们用placeholderMatches来处理）
                int placeholderIndex = 0;
                for (int i = 0; i < templateParts.Length; i++)
                {
                    var part = templateParts[i];

                    if (i % 2 == 0) // 偶数索引是普通文本（分隔符或固定前缀/后缀）
                    {
                        if (!string.IsNullOrEmpty(part))
                        {
                            regexBuilder.Append(Regex.Escape(part));
                        }
                    }
                    else // 奇数索引是占位符的内容，我们将其替换为捕获组
                    {
                        // 判断是否是最后一个占位符
                        bool isLastPlaceholder = (placeholderIndex == placeholderMatches.Count - 1);

                        if (isLastPlaceholder && !template.EndsWith("}"))
                        {
                            // 如果最后一个占位符后面还有文本（比如文件扩展名），那么它的匹配不能太贪婪
                            // 匹配直到下一个分隔符出现
                            regexBuilder.Append($"([^{Regex.Escape(_rules.Separator)}]+)");
                        }
                        else if (isLastPlaceholder)
                        {
                            // 如果模板以占位符结尾，它需要匹配到字符串末尾，包括扩展名
                            regexBuilder.Append("(.+)");
                        }
                        else
                        {
                            // 对于中间的占位符，匹配所有不是分隔符的字符
                            regexBuilder.Append($"([^{Regex.Escape(_rules.Separator)}]+)");
                        }
                        placeholderIndex++;
                    }
                }

                regexBuilder.Append("$");
                var regexPattern = regexBuilder.ToString();

                // 3. 匹配文件名
                var fileMatch = Regex.Match(shortFileName, regexPattern);
                if (fileMatch.Success)
                {
                    // Groups[0] 是整个匹配，捕获组从索引 1 开始
                    for (int i = 0; i < paramNames.Count; i++)
                    {
                        var paramName = paramNames[i];
                        // 跳过空的参数名 (即模板中的 "{}")
                        if (!string.IsNullOrWhiteSpace(paramName))
                        {
                            // 确保索引在 Groups 范围内
                            if (i + 1 < fileMatch.Groups.Count)
                            {
                                result[paramName] = fileMatch.Groups[i + 1].Value;
                            }
                        }
                    }
                }
                else
                {
                    LogManager.LogWarning($"文件名与模板不匹配: {shortFileName}");
                    LogManager.LogWarning($"模板: {template}");
                    LogManager.LogWarning($"生成的正则表达式: {regexPattern}");
                }
            }
            catch (Exception ex)
            {
                LogManager.LogError($"模板解析失败: {fileName}", ex);
            }

            return result;
        }


        /// <summary>
        /// 验证解析规则配置
        /// </summary>
        public static (bool isValid, string errorMessage) ValidateRules(FileNameParseRules rules)
            {
                if (!rules.Enabled)
                {
                    return (true, string.Empty);
                }

                if (string.IsNullOrWhiteSpace(rules.Separator))
                {
                    return (false, "文件名分隔符不能为空");
                }

                // 验证正则表达式
                if (rules.RegexMapping != null)
                {
                    foreach (var mapping in rules.RegexMapping)
                    {
                        try
                        {
                            _ = new Regex(mapping.Value);
                        }
                        catch (Exception ex)
                        {
                            return (false, $"正则表达式无效 [{mapping.Key}]: {mapping.Value}, 错误: {ex.Message}");
                        }
                    }
                }

                return (true, string.Empty);
            }
    
    }
}

