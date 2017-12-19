Imports System.Configuration
Imports System.IO
Imports NUnit.Framework

Namespace NUnitTestDemo

    <ExpectPass>
    Public Class ConfigFileTests
        <Test>
        Public Shared Sub ProperConfigFileIsUsed()
            Dim expectedPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "VbTestDemo.dll.config")
            Assert.That(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, Iz.EqualTo(expectedPath))
        End Sub

        <Test>
        Public Shared Sub CanReadConfigFile()
            Assert.That(ConfigurationManager.AppSettings.Get("test.setting"), Iz.EqualTo("54321"))
        End Sub

    End Class

End Namespace
