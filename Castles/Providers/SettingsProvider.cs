using Castles.Data;
using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Castles.Providers
{
    public interface ISettingsProvider
    {
        SettingsFile Settings { get; }
    }

    public class SettingsProvider : ISettingsProvider
    {
        private readonly SettingsFile settings;

        public SettingsFile Settings => settings;

        public SettingsProvider()
        {
            var settingsPath = Path.Combine(Environment.CurrentDirectory, @"Content/settings.json");

            if (File.Exists(settingsPath))
            {
                using var fs = File.OpenRead(settingsPath);
                using var sr = new StreamReader(fs, Encoding.UTF8);
                string content = sr.ReadToEnd();

                settings = JsonSerializer.Deserialize<SettingsFile>(content);
            }
            else
            {
                // TODO: Supply default
            }
        }

        public SettingsFile GetSettings()
        {
            return settings;
        }
    }
}
