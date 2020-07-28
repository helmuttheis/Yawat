namespace Yawat
{
    using System;
    using System.IO;
    using Newtonsoft.Json.Linq;

    public static class YawatSettings
    {
        // ReSharper disable once InconsistentNaming
        private static JToken settingsJToken;

        public const string EnvironmentVariableName = "yawatsettings";

        public const string YawatSettingsJsonFileName = "yawat.settings.json";

        public static string SettingsFile { get; private set; }

        public static string Get(string jsonPath, string defaultValue = "")
        {
            if (settingsJToken == null)
            {
                Load();
            }

            try
            {
                var jToken = settingsJToken?.SelectToken(jsonPath);
                if (jToken != null)
                {
                    return jToken.ToString();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }

            return defaultValue;
        }

        public static void Reset()
        {
            settingsJToken = null;
            SettingsFile = default;
        }

        private static void Load()
        {
            if (!FindFile())
            {
                return;
            }

            try
            {
                settingsJToken = JToken.Parse(File.ReadAllText(SettingsFile));
            }
            catch (Newtonsoft.Json.JsonException jsonException)
            {
                Console.WriteLine(jsonException);
                throw;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private static bool FindFile()
        {
            try
            {
                SettingsFile = Environment.GetEnvironmentVariable(EnvironmentVariableName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                SettingsFile = string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(SettingsFile) && File.Exists(SettingsFile))
            {
                return true;
            }

            try
            {
                var curDir =
                    System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ??
                    string.Empty;

                while (curDir.Length > 3)
                {
                    SettingsFile = System.IO.Path.Combine(curDir, YawatSettingsJsonFileName);
                    if (File.Exists(SettingsFile))
                    {
                        return true;
                    }

                    curDir = Path.GetFullPath(Path.Combine(curDir, ".."));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return false;
        }
    }
}
