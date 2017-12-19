using System;
using System.Configuration;
using System.IO;
using NUnit.Framework;

namespace NUnitTestDemo
{
    [ExpectPass]
    public class ConfigFileTests
    {
        [Test]
        public static void ProperConfigFileIsUsed()
        {
            var expectedPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "CSharpTestDemo.dll.config");
            Assert.That(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, Is.EqualTo(expectedPath));
        }

        [Test]
        public static void CanReadConfigFile()
        {
            Assert.That(ConfigurationManager.AppSettings.Get("test.setting"), Is.EqualTo("54321"));
        }

    }
}
