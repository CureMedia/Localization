// Copied from https://raw.githubusercontent.com/cake-build/example/master/build.cake

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

var projects = new string[] { "Localization.Routing", "Localization.Routing.Mvc" };

// Define directories.
var buildDir = Directory("src/Localization.Routing/bin") + Directory(configuration);
var artifacts = Directory("artificats");
var solution = "Localization.sln";

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
	CleanDirectory(artifacts);
    CleanDirectory(buildDir);
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreRestore();
});

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
      // Use MSBuild
      MSBuild(solution, settings =>
        settings.SetConfiguration(configuration));
    }
    else
    {
      // Use XBuild
      XBuild(solution, settings =>
        settings.SetConfiguration(configuration));
    }
});

Task("Test")
    .IsDependentOn("Build")
  .Does(() =>
    {
		var settings = new DotNetCoreTestSettings
                {
                    Configuration = configuration,
                    NoBuild = true
                };
        var projects = GetFiles("test/**/*.csproj");
        foreach(var project in projects)
        {
            DotNetCoreTest(project.FullPath, settings);
        }
    });

Task("Pack")
	.IsDependentOn("Test")
	.Does(() =>
		{
			 var settings = new DotNetCorePackSettings
		     {
				Configuration = "Release",
				OutputDirectory = artifacts,
				NoBuild = true
			};
			var projects = GetFiles("src/**/*.csproj");
			foreach(var project in projects)
			{
				DotNetCorePack(project.FullPath, settings);
			}			
		});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Pack");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);