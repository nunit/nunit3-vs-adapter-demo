//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// DEFINE RUN CONSTANTS
//////////////////////////////////////////////////////////////////////

// Directories
var PROJECT_DIR = Context.Environment.WorkingDirectory.FullPath + "/";
var VS2015_DIR = PROJECT_DIR + "solutions/vs2015/";
var VS2017_DIR = PROJECT_DIR + "solutions/vs2017/";
var TOOLS_DIR = PROJECT_DIR + "tools/";
var NET35_ADAPTER_PATH = TOOLS_DIR + "NUnit3TestAdapter/build/net35/";

// Version of the Adapter to Use
var ADAPTER_VERSION = "3.9.0";

// Specify all the demo projects
var DemoProjects = new DemoProject[] {
	new DemoProject()
	{
		Path = VS2015_DIR + "CSharpTestDemo/CSharpTestDemo.csproj", 
		OutputDir = VS2015_DIR + "CSharpTestDemo/bin/" + configuration + "/",
		ExpectedResult = "Total tests: 107. Passed: 58. Failed: 25. Skipped: 15."
	},
	new DemoProject()
	{
		Path = VS2015_DIR + "VbTestDemo/VbTestDemo.vbproj", 
		OutputDir = VS2015_DIR + "VbTestDemo/bin/" + configuration + "/",
		ExpectedResult = "Total tests: 107. Passed: 58. Failed: 25. Skipped: 15."
	},
	new DemoProject()
	{
		Path = VS2015_DIR + "CppTestDemo/CppTestDemo.vcxproj",
		OutputDir = VS2015_DIR + "CppTestDemo/" + configuration + "/",
		ExpectedResult = "Total tests: 29. Passed: 13. Failed: 6. Skipped: 8."
	},
	new DemoProject()
	{
		Path = VS2017_DIR + "NUnitTestDemo/NUnit3TestDemo.csproj",
		OutputDir = VS2017_DIR + "NUnitTestDemo/bin/" + configuration + "/",
		ExpectedResult = "Total tests: 107. Passed: 58. Failed: 25. Skipped: 15."
	},
	new DemoProject()
	{
		Path = VS2017_DIR + "NUnit3CoreTestDemo/NUnit3CoreTestDemo.csproj",
		OutputDir = VS2017_DIR + "NUnit3CoreTestDemo/bin/" + configuration + "/",
		ExpectedResult = "Total tests: 107. Passed: 58. Failed: 25. Skipped: 15."
	}
};

//////////////////////////////////////////////////////////////////////
// CLEAN
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
	foreach(var proj in DemoProjects)
		CleanDirectory(proj.OutputDir);
});

//////////////////////////////////////////////////////////////////////
// NUGET RESTORE
//////////////////////////////////////////////////////////////////////

Task("NuGetRestore")
	.Does(() =>
	{
		foreach (var proj in DemoProjects)
		{
			if (proj.SupportsRestore)
			{
				Information("Restoring NuGet Packages for " + proj.Name);

				if (proj.Name.Contains("Core"))
					DotNetCoreRestore(proj.Path);
				else
				{
					NuGetRestore(proj.Path,
						new NuGetRestoreSettings {
							PackagesDirectory = System.IO.Path.GetDirectoryName(proj.Path) + "/packages"
						});
				}
			}
		}
	});

//////////////////////////////////////////////////////////////////////
// BUILD
//////////////////////////////////////////////////////////////////////

Task("Build")
	.IsDependentOn("NugetRestore")
	.Does(() =>
	{
 		foreach (var proj in DemoProjects)
		{
			MSBuild(proj.Path, new MSBuildSettings
			{
				Configuration = configuration,
				EnvironmentVariables = new Dictionary<string, string>(),
				NodeReuse = false,
				PlatformTarget = PlatformTarget.MSIL,
				ToolVersion = proj.ToolVersion
			});
		}
    });

//////////////////////////////////////////////////////////////////////
// INSTALL ADAPTER
//////////////////////////////////////////////////////////////////////

Task("InstallAdapter")
	.Does(() =>
	{
		Information("Installing NUnit3TestAdapter");

		NuGetInstall("NUnit3TestAdapter",
			new NuGetInstallSettings()
			{
				OutputDirectory = TOOLS_DIR,
				Version = ADAPTER_VERSION,
				ExcludeVersion = true
			});
	});

//////////////////////////////////////////////////////////////////////
// RUN DEMOS
//////////////////////////////////////////////////////////////////////

Task("RunDemos")
	.IsDependentOn("Build")
	.IsDependentOn("InstallAdapter")
	.Does(() =>
	{
		foreach(var proj in DemoProjects)
		{
			Information("");
			Information("********************************************************************************************");
			Information("Demo: " + proj.TestAssembly);
			Information("********************************************************************************************");
			Information("");

			if (!proj.Name.Contains("Core"))
			{
				IEnumerable<string> redirectedStandardOutput;
				IEnumerable<string> redirectedErrorOutput;

				int result = StartProcess(
					"vstest.console.exe", 
					new ProcessSettings()
					{
						Arguments = $"{proj.TestAssembly} /TestAdapterPath:{NET35_ADAPTER_PATH}",
						RedirectStandardOutput = true
					},
					out redirectedStandardOutput,
					out redirectedErrorOutput);

				foreach(string line in redirectedStandardOutput)
				{
					Information(line);
					if (line.StartsWith("Total tests"))
						proj.ActualResult = line;
				}
			}
			else
			{
				Information("Skipping .NET Core demo for now");
				proj.SkipReason = ".NET Core Project";
			}
		}

		Information("");
		Information("******************************");
		Information("*  Test Demo Summary Report  *");
		Information("******************************");
		Information("");

		foreach (var proj in DemoProjects)
		{
			Information(proj.Path);
			if (proj.SkipReason != null)
			{
				Information(" Skipped: " + proj.SkipReason);
			}
			else
			if (proj.ActualResult == proj.ExpectedResult)
			{
			    Information("  Passed: " + proj.ExpectedResult);
			}
			else
			{
				Information("Expected: " + proj.ExpectedResult);
				Information(" But was: " + proj.ActualResult ?? "<null>");
			}
			Information("");
		}
	});

public class DemoProject
{
	public string Path { get; set; }
	public string OutputDir { get; set; }
	public string ExpectedResult { get; set; }
	public string ActualResult { get; set; }
	public string SkipReason { get; set; }

	public string Name
	{ 
		get { return System.IO.Path.GetFileNameWithoutExtension(Path); }
	}

	public string Type
	{
		get { return System.IO.Path.GetExtension(Path); }
	}

	public string TestAssembly
	{
		get { return OutputDir + Name + ".dll"; }
	}

	public bool SupportsRestore
	{
		get { return Type != ".vcxproj"; }
	}

	public MSBuildToolVersion ToolVersion
	{
		get { return Path.Contains("vs2015") ? MSBuildToolVersion.VS2015 : MSBuildToolVersion.VS2017; }
	}
}

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Appveyor")
	.IsDependentOn("RunDemos");

Task("Default")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
