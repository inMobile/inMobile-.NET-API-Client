#addin nuget:?package=Cake.Git

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var version = Argument("releaseVersion", "1.0.0");

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
    // Executed BEFORE the first task.
    var cakeVersion = typeof(ICakeContext).Assembly.GetName().Version.ToString();
    Information($"Running tasks using version {cakeVersion} of Cake ...");
});

Teardown(ctx =>
{
   // Executed AFTER the last task.
   Information("Finished running tasks ...");
});

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("UpdateVersion")
    .Does(() => 
    {
		var assemplyInfoPattern = "**/Sms.ApiClient/Properties/AssemblyInfo.cs";

        ReplaceRegexInFiles(
            globberPattern: assemplyInfoPattern, 
            rxFindPattern: "(^\s*\[\s*assembly\s*:\s*((System\s*\.)?\s*Reflection\s*\.)?\s*AssemblyFileVersion(Attribute)?\s*\(\s*@?\")(([0-9\*])+\.?)+(\"\s*\)\s*\])", 
            replaceText: version);

		ReplaceRegexInFiles(
            globberPattern: assemplyInfoPattern, 
            rxFindPattern: "(^\s*\[\s*assembly\s*:\s*((System\s*\.)?\s*Reflection\s*\.)?\s*AssemblyVersion(Attribute)?\s*\(\s*@?\")(([0-9\*])+\.?)+(\"\s*\)\s*\])", 
            replaceText: version);
    });

Task("Build")
	.IsDependentOn("UpdateVersion")
	.Does(() => 
	{
		Information("Build");
		MsBuild("./Sms.ApiClient.Examples.sln", new MSBuildSettings {
			ToolVersion = MSBuildToolVersion.VS2017,
			Configuration = "Release",
			Targets = new HashSet { "Rebuild" },
			WarningsAsError = true
		});
	});

Task("GitCommitAndPush")
	.IsDependentOn("Build")
	.Does(() => 
	{
		Information("GitCommitAndPush");
		GitCommit("./", "Build Agent", "", "Updated version number");
		GitPush("./");
	});

Task("CreateGitTag")
	.IsDependentOn("GitCommitAndPush")
	.Does(() => 
	{
		Information("CreateGitTag");
		GitTag("./", version);
	});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("CreateGitTag");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
