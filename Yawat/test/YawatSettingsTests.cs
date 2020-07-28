namespace Yawat.Tests
{
    using System;
    using System.IO;

    using NUnit.Framework;

    [TestFixture]
    public class YawatSettingsTests
    {
        [Test]
        public void ShouldLoadSettingsFromDirectoryTree()
        {
            Environment.SetEnvironmentVariable(YawatSettings.EnvironmentVariableName, string.Empty);
            var host = YawatSettings.Get("Host");

            Assert.AreEqual(host, "localhost");
        }

        [Test]
        public void ShouldLoadSettingsFromEnvironment()
        {
            YawatSettings.Reset();
            var jsonFileName = Path.Combine(Path.GetTempPath(), YawatSettings.YawatSettingsJsonFileName);
            Environment.SetEnvironmentVariable(YawatSettings.EnvironmentVariableName, jsonFileName);
            var jsonObject = new { test = new { user = "user" } };

            File.WriteAllText(jsonFileName, Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject));

            var testUser = YawatSettings.Get("test.user");
            Assert.AreEqual(testUser, "user");

            Assert.AreEqual(YawatSettings.SettingsFile, jsonFileName);

            Environment.SetEnvironmentVariable(YawatSettings.EnvironmentVariableName, string.Empty);
            File.Delete(jsonFileName);
        }
    }
}
