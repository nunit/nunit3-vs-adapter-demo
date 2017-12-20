#include "ExpectedOutcomeAttributes.h"

using namespace System;
using namespace System::Configuration;
using namespace System::IO;
using namespace NUnit::Framework;

namespace NUnitTestDemo
{
    [ExpectPass]
    public class ConfigFileTests
    {
	public:
        [Test]
        static void ProperConfigFileIsUsed()
        {
            String^ expectedPath = Path::Combine(TestContext::CurrentContext->TestDirectory, "CppTestDemo.dll.config");
            Assert::That(AppDomain::CurrentDomain->SetupInformation->ConfigurationFile, Is::EqualTo(expectedPath));
        }

        [Test]
        static void CanReadConfigFile()
        {
            Assert::That(ConfigurationManager::AppSettings->Get("test.setting"), Is::EqualTo("54321"));
        }

	};
}
