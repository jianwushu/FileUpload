# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a **.NET 6.0 Windows Forms** application for automatic file upload via HTTP interface. The system monitors configured folders, parses filenames to extract parameters, and uploads files to a backend API with comprehensive logging and retry mechanisms.

## Technology Stack

- **Framework**: .NET 6.0 (Target Framework: net6.0-windows)
- **UI Framework**: Windows Forms with Implicit Usings enabled
- **Configuration**: YamlDotNet 16.3.0 for YAML configuration
- **JSON Processing**: Newtonsoft.Json 13.0.4
- **Image Processing**: System.Drawing.Common 8.0.0
- **Architecture Pattern**: Service-oriented architecture with dependency injection via constructor

## Build and Run Commands

### Development
```bash
# Build the project
dotnet build

# Build in Release mode
dotnet build --configuration Release

# Run the application
dotnet run

# Run without debugging (faster startup)
dotnet run --no-build
```

### Testing
```bash
# Build first, then run
dotnet build && dotnet run
```

### Publishing
```bash
# Standard publish (framework-dependent)
dotnet publish --configuration Release

# Single file deployment
dotnet publish --configuration Release -p:PublishSingleFile=true

# Self-contained deployment for Windows x64
dotnet publish --configuration Release --runtime win-x64 --self-contained true -p:PublishSingleFile=true
```

**Output locations**:
- Debug build: `bin\Debug\net6.0-windows\`
- Release build: `bin\Release\net6.0-windows\`

## Code Architecture

### Directory Structure

```
FileUpload/
├── Models/                      # Data models and configuration
│   ├── AppConfig.cs             # Main configuration model (deserializes config.yml)
│   ├── ApiResponse.cs           # HTTP API response model
│   └── UploadLog.cs             # Upload log entry model
│
├── Services/                    # Business logic services
│   ├── ConfigManager.cs         # Configuration loading/saving (singleton pattern)
│   ├── FileUploadService.cs     # Core file upload orchestration
│   ├── FileNameParser.cs        # Filename parsing (3 modes: template, position, regex)
│   ├── ImageCompressionService.cs # Image compression before upload
│   ├── LocalizationService.cs   # Internationalization (i18n) service
│   ├── LogManager.cs            # Logging service (text + CSV formats)
│   ├── LogMonitor.cs            # UI log monitoring
│   └── ServiceHeartbeatManager.cs # Health check service
│
├── Utils/                       # Utility classes
│   ├── ApiHelper.cs             # HTTP API helper methods
│   └── ToolHelper.cs            # General utility methods
│
├── Resources/                   # Localization resources
│   ├── Strings.resx             # Chinese Simplified (default)
│   ├── Strings.en.resx          # English
│   ├── Strings.ja.resx          # Japanese
│   ├── Strings.vi.resx          # Vietnamese
│   ├── Strings.zzh-Hant.resx    # Traditional Chinese
│   └── Strings.Designer.cs      # Auto-generated designer code
│
├── Form1.*                      # Main application window
├── ConfigForm.*                 # Configuration editor dialog
├── AboutForm.*                  # About dialog
├── Program.cs                   # Application entry point
├── config.yml                   # Application configuration
└── FileUpload.{csproj,sln}      # Project and solution files
```

### Core Components

#### 1. **AppConfig (Models/AppConfig.cs)**
   - Central configuration model using YamlDotNet attributes
   - Properties map directly to `config.yml` keys via `[YamlMember(Alias = "key")]`
   - Includes nested classes: `JsonRequestBodyConfig`, `FileNameParseRules`, `SystemTrayConfig`
   - Contains helper methods like `GetWatchFolders()` to handle both single and multiple folder configurations

#### 2. **FileUploadService (Services/FileUploadService.cs)**
   - Primary orchestration service for file upload operations
   - Implements IDisposable for proper resource cleanup
   - Key features:
     - Monitors configured folders (supports multiple folders + recursion)
     - File occupation detection to avoid uploading incomplete files
     - SemaphoreSlim-based thread pool for concurrent processing
     - FileNameParser integration for parameter extraction
     - Image compression before upload
     - Moves successful uploads to `OK/` folder, failed to `NG/`
     - Optional directory structure preservation
   - CancellationToken-based graceful shutdown

#### 3. **ConfigManager (Services/ConfigManager.cs)**
   - Singleton pattern service for configuration management
   - Static methods: `LoadConfig()`, `SaveConfig()`
   - Caches configuration to avoid repeated file I/O
   - Auto-creates OK/NG folders if enabled

#### 4. **FileNameParser (Services/FileNameParser.cs)**
   - Three parsing modes for extracting parameters from filenames:
     1. **Template mode**: `{PARAM}_{PARAM}` format with placeholders
     2. **Position mapping**: Map parameters to positions/ranges (e.g., "2-3" for multiple segments)
     3. **Regex mode**: Custom regex patterns for complex scenarios
   - Handles empty value rejection via `rejectEmptyValues` flag
   - Used by FileUploadService to extract metadata before upload

#### 5. **LogManager (Services/LogManager.cs)**
   - Thread-safe logging service
   - Dual output: Text logs (`upload_YYYYMMDD.log`) + CSV logs (`upload_YYYYMM.csv`)
   - Auto-rotates logs (daily text, monthly CSV)
   - Auto-cleans logs (30-day retention)
   - Event-based log updates for UI (LogManager.LogUpdated)

#### 6. **ImageCompressionService (Services/ImageCompressionService.cs)**
   - Compresses images before upload to reduce bandwidth
   - Threshold-based: compresses only files > `compressionThresholdKB`
   - Configurable quality: `compressionQuality` (1-100)
   - Supports common formats: JPEG, PNG, BMP

#### 7. **LocalizationService (Services/LocalizationService.cs)**
   - Simple i18n service providing `T(key)` method
   - Integrates with .resx resource files
   - Set via `SetCulture(culture)` in Program.cs

#### 8. **Program.cs (Entry Point)**
   - Loads configuration on startup
   - Sets application culture based on config.language
   - Initializes Windows Forms application
   - Fallback to zh-CN if config loading fails

### Configuration System (config.yml)

The application uses **YAML configuration** with the following key sections:

```yaml
# Device and API configuration
deviceId: IMG-TEST-01
uploadUrl: http://10.18.53.14:8081/GwService/SendDataToMES
language: zh-CN

# Folder monitoring (supports multiple folders)
watchFolders:
  - D:\Upload\Watch
scanSubfolders: false
preserveDirectoryStructure: false

# Upload behavior
allowedExtensions: .jpg,.jpeg,.png
requestTimeout: 30
scanInterval: 5
maxRetryCount: 3
autoCreateFolders: true

# Request body configuration
jsonRequestBody:
  fieldName: strjson
  fixedParams: { LOTNO: "", UPLOADER: "" }
  extractParams: [BARCODE, TOOLINGNO, CAVITYNO]
  autoGenerateParams:
    FILENAME: FILENAME
    COMPUTERNO: COMPUTERNO
    LOCAL_PATH: LOCAL_PATH

# Filename parsing rules
fileNameParseRules:
  enabled: true
  separator: _
  rejectEmptyValues: false
  # Choose ONE of the three parsing modes:
  template: "{BARCODE}_{TOOLINGNO}_{CAVITYNO}"
  positionMapping:
    BARCODE: "1"
    CAVITYNO: "4"
  regexMapping:
    BARCODE: "^[^_]+_([^_]+)"

# Performance optimization
enableThreadPool: false
threadPoolSize: 3
enableImageCompression: false
compressionThresholdKB: 500
compressionQuality: 75

# System tray
systemTray:
  enabled: true
  minimizeToTray: true
  closeAction: ask
```

### Key Design Patterns

1. **Singleton**: ConfigManager uses static methods with caching
2. **Strategy Pattern**: FileNameParser supports three different parsing strategies
3. **Observer Pattern**: LogManager uses events to notify UI of new log entries
4. **IDisposable Pattern**: FileUploadService implements proper resource cleanup
5. **Dependency Injection**: Services receive dependencies via constructor injection

### Thread Safety

- Logging uses `lock` statement for file write operations
- UI updates use `Invoke()` to marshal calls to UI thread
- Concurrent file processing uses `SemaphoreSlim` when thread pool enabled
- CancellationToken for graceful shutdown of background tasks

## Common Development Tasks

### Modifying Configuration
1. Edit `config.yml` manually, or
2. Use the GUI: Run the app → Click "配置" (Configuration) button

### Adding a New Service
1. Create service class in `Services/` directory
2. Add constructor with dependency injection pattern
3. Register in dependent classes (e.g., FileUploadService)
4. Follow existing naming conventions: `[ServiceName]Service.cs`

### Adding Localization Strings
1. Add string key to `Resources/Strings.resx` (default: Chinese Simplified)
2. Add translations to other language .resx files
3. Access via `LocalizationService.T("key")` in code

### Changing Filename Parsing Logic
- Modify `Services/FileNameParser.cs`
- Three parsing modes are mutually exclusive - configure one at a time
- Test with real filenames to ensure correct parameter extraction

### Debugging File Upload Issues
1. Check logs in application directory: `upload_YYYYMMDD.log`
2. Verify config.yml syntax (use YAML validator)
3. Test API endpoint with Postman/curl
4. Enable debug logging in LogManager if needed

## Important Implementation Notes

### Time Parameter Handling
Three ways to get time values:
- **From filename**: Add to `extractParams` + configure position/regex
- **From file properties**: Add to `autoGenerateParams` with `CREATETIME`/`MODIFYTIME` keys
- **Current system time**: Use `DATETIME` in `autoGenerateParams`

### File Processing Flow
1. Scan configured folders (respects `scanSubfolders` flag)
2. Filter by `allowedExtensions`
3. Check file occupation (skip if in use)
4. Parse filename (if `fileNameParseRules.enabled`)
5. Generate parameters (fixed + extracted + auto-generated)
6. Compress image if enabled and threshold exceeded
7. Upload via HTTP with retry logic
8. Move to OK/NG folder with optional directory structure preservation
9. Log result (both text and CSV)

### Thread Pool Configuration
- Enable via `enableThreadPool: true`
- Configure size via `threadPoolSize: N`
- Uses `SemaphoreSlim` for concurrency control
- Default: disabled (single-threaded sequential processing)

## Performance Characteristics

- **Default scan interval**: 5 seconds (configurable)
- **HTTP timeout**: 30 seconds (configurable)
- **Max retries**: 3 attempts per file
- **Log retention**: 30 days auto-cleanup
- **UI log buffer**: Last 1000 entries displayed

## Deployment

**Standard deployment**: Copy `bin\Release\net6.0-windows\` to target machine

**Single-file deployment**:
```bash
dotnet publish --configuration Release --runtime win-x64 --self-contained false -p:PublishSingleFile=true
```

The application is **Windows-only** (net6.0-windows target framework).

## Integration with External Systems

- **Upload API**: HTTP POST to configured URL with JSON body
- **Request format**: `{fieldName: "strjson", data: { ... extracted parameters ... }}`
- **Response handling**: Logs HTTP status codes, supports retry on failure
- **File system**: Monitors local folders, creates OK/NG subdirectories

## Critical Files to Understand

1. **Form1.cs**: Main UI loop, service lifecycle management
2. **FileUploadService.cs**: Core business logic and file processing pipeline
3. **AppConfig.cs**: All configuration options and their meanings
4. **config.yml**: Runtime configuration reference
5. **LogManager.cs**: Logging infrastructure and audit trail
6. **项目总结.md**: Comprehensive project documentation (Chinese)

## Current Version

- **Version**: v1.4
- **Last Updated**: 2025-10-31
- **Key Features**: Multi-folder monitoring, system tray, internationalization (5 languages)

The project summary (`项目总结.md`) contains extensive documentation about features, configuration, and usage in Chinese.
