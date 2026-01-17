/*
 * Lone EFT DMA Radar
 * Brought to you by Lone (Lone DMA)
 * 
MIT License

Copyright (c) 2025 Lone DMA

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 *
*/

using LoneArenaDmaRadar.Misc.JSON;
using LoneArenaDmaRadar.UI;
using LoneArenaDmaRadar.UI.ColorPicker;
using VmmSharpEx.Extensions.Input;

namespace LoneArenaDmaRadar
{
    /// <summary>
    /// Global Program Configuration (Config.json)
    /// </summary>
    public sealed class ArenaDmaConfig
    {
        /// <summary>
        /// Public Constructor required for deserialization.
        /// DO NOT CALL - USE LOAD().
        /// </summary>
        public ArenaDmaConfig() { }

        /// <summary>
        /// DMA Config
        /// </summary>
        [JsonPropertyName("dma")]
        public DMAConfig DMA { get; set; } = new();

        /// <summary>
        /// Twitch API Config (for streamer lookup).
        /// </summary>
        [JsonPropertyName("twitchApi")]
        public TwitchApiConfig TwitchApi { get; set; } = new();

        /// <summary>
        /// UI/Radar Config
        /// </summary>
        [JsonPropertyName("ui")]
        public UIConfig UI { get; set; } = new();

        /// <summary>
        /// Hotkeys Dictionary for Radar.
        /// </summary>
        [JsonPropertyName("hotkeys_v2")]
        public ConcurrentDictionary<Win32VirtualKey, string> Hotkeys { get; set; } = new(); // Default entries

        /// <summary>
        /// All defined Radar Colors.
        /// </summary>
        [JsonPropertyName("radarColors")]
        [JsonConverter(typeof(ColorDictionaryConverter))]
        public ConcurrentDictionary<ColorPickerOption, string> RadarColors { get; set; } = new();

        /// <summary>
        /// Widgets Configuration.
        /// </summary>
        [JsonPropertyName("aimviewWidget")]
        public AimviewWidgetConfig AimviewWidget { get; set; } = new();

        #region Config Interface

        /// <summary>
        /// Filename of this Config File (not full path).
        /// </summary>
        [JsonIgnore]
        internal const string Filename = "Config-Arena.json";

        [JsonIgnore]
        private static readonly Lock _syncRoot = new();

        [JsonIgnore]
        private static readonly FileInfo _configFile = new(Path.Combine(Program.ConfigPath.FullName, Filename));

        [JsonIgnore]
        private static readonly FileInfo _tempFile = new(Path.Combine(Program.ConfigPath.FullName, Filename + ".tmp"));

        [JsonIgnore]
        private static readonly FileInfo _backupFile = new(Path.Combine(Program.ConfigPath.FullName, Filename + ".bak"));

        /// <summary>
        /// Loads the configuration from disk.
        /// Creates a new config if it does not exist.
        /// ** ONLY CALL ONCE PER MUTEX **
        /// </summary>
        /// <returns>Loaded Config.</returns>
        public static ArenaDmaConfig Load()
        {
            ArenaDmaConfig config;
            lock (_syncRoot)
            {
                Program.ConfigPath.Create();
                if (_configFile.Exists)
                {
                    config = TryLoad(_tempFile) ??
                        TryLoad(_configFile) ??
                        TryLoad(_backupFile);

                    if (config is null)
                    {
                        var dlg = MessageBox.Show(
                            RadarWindow.Handle,
                            "Config File Corruption Detected! If you backed up your config, you may attempt to restore it.\n" +
                            "Press OK to Reset Config and continue startup, or CANCEL to terminate program.",
                            Program.Name,
                            MessageBoxButton.OKCancel,
                            MessageBoxImage.Error);
                        if (dlg == MessageBoxResult.Cancel)
                            Environment.Exit(0); // Terminate program
                        config = new ArenaDmaConfig();
                        SaveInternal(config);
                    }
                }
                else
                {
                    config = new();
                    SaveInternal(config);
                }

                return config;
            }
        }

        private static ArenaDmaConfig TryLoad(FileInfo file)
        {
            try
            {
                if (!file.Exists)
                    return null;
                string json = File.ReadAllText(file.FullName);
                return JsonSerializer.Deserialize(json, AppJsonContext.Default.ArenaDmaConfig);
            }
            catch
            {
                return null; // Ignore errors, return null to indicate failure
            }
        }

        /// <summary>
        /// Save the current configuration to disk.
        /// </summary>
        /// <exception cref="IOException"></exception>
        public void Save()
        {
            lock (_syncRoot)
            {
                try
                {
                    SaveInternal(this);
                }
                catch (Exception ex)
                {
                    throw new IOException($"ERROR Saving Config: {ex.Message}", ex);
                }
            }
        }

        private static void SaveInternal(ArenaDmaConfig config)
        {
            var json = JsonSerializer.Serialize(config, AppConfigJsonContext.Default.ArenaDmaConfig);
            using (var fs = new FileStream(
                _tempFile.FullName,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 4096,
                options: FileOptions.WriteThrough))
            using (var sw = new StreamWriter(fs))
            {
                sw.Write(json);
                sw.Flush();
                fs.Flush(flushToDisk: true);
            }
            if (_configFile.Exists)
            {
                File.Replace(
                    sourceFileName: _tempFile.FullName,
                    destinationFileName: _configFile.FullName,
                    destinationBackupFileName: _backupFile.FullName,
                    ignoreMetadataErrors: true);
            }
            else
            {
                File.Copy(
                    sourceFileName: _tempFile.FullName,
                    destFileName: _backupFile.FullName);
                File.Move(
                    sourceFileName: _tempFile.FullName,
                    destFileName: _configFile.FullName);
            }
        }

        #endregion
    }

    public sealed class DMAConfig
    {
        /// <summary>
        /// FPGA Read Algorithm
        /// </summary>
        [JsonPropertyName("fpgaAlgo")]
        public FpgaAlgo FpgaAlgo { get; set; } = FpgaAlgo.Auto;

        /// <summary>
        /// Use a Memory Map for FPGA DMA Connection.
        /// </summary>
        [JsonPropertyName("enableMemMap")]
        public bool MemMapEnabled { get; set; } = true;
    }

    public sealed class UIConfig
    {
        /// <summary>
        /// Set FPS for the Radar Window (default: 60)
        /// </summary>
        [JsonPropertyName("fps")]
        public int FPS { get; set; } = 60;

        /// <summary>
        /// UI Scale Value (0.5-2.0 , default: 1.0)
        /// </summary>
        [JsonPropertyName("scale")]
        public float UIScale { get; set; } = 1.0f;

        /// <summary>
        /// Size of the Radar Window.
        /// </summary>
        [JsonPropertyName("windowSize")]
        public SKSize WindowSize { get; set; } = new(1280, 720);

        /// <summary>
        /// Window is maximized.
        /// </summary>
        [JsonPropertyName("windowMaximized")]
        public bool WindowMaximized { get; set; }

        /// <summary>
        /// Last used 'Zoom' level.
        /// </summary>
        [JsonPropertyName("zoom")]
        public int Zoom { get; set; } = 100;

        /// <summary>
        /// Player/Teammates Aimline Length (Max: 1500)
        /// </summary>
        [JsonPropertyName("aimLineLength")]
        public int AimLineLength { get; set; } = 1500;

        /// <summary>
        /// Show enemy aimlines when aiming at you.
        /// </summary>
        [JsonPropertyName("highAlert")]
        public bool HighAlert { get; set; } = true;
    }

    public sealed class TwitchApiConfig
    {
        [JsonPropertyName("clientId")]
        public string ClientId { get; set; } = null;
        [JsonPropertyName("clientSecret")]
        public string ClientSecret { get; set; } = null;
    }
    public sealed class AimviewWidgetConfig
    {
        /// <summary>
        /// True if the Aimview Widget is enabled.
        /// </summary>
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; } = true;
    }
}