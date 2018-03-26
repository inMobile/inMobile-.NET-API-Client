#addin "Cake.FileHelpers"
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
            rxFindPattern: "(?<=AssemblyFileVersion\\(\")(.+?)(?=\"\\))", 
            replaceText: version);

		ReplaceRegexInFiles(
            globberPattern: assemplyInfoPattern, 
            rxFindPattern: "(?<=AssemblyVersion\\(\")(.+?)(?=\"\\))", 
            replaceText: version);
    });

Task("Build")
	.IsDependentOn("UpdateVersion")
	.Does(() => 
	{
		Information("Build");

		var buildSettings = new MSBuildSettings {
			ToolVersion = MSBuildToolVersion.VS2017,
			Configuration = "Release",
			WarningsAsError = true
		};
		buildSettings.Targets.Add("Rebuild");

		MSBuild("./Sms.ApiClient.Examples.sln", buildSettings);
	});

Task("GitCommitAndPush")
	.IsDependentOn("Build")
	.Does(() => 
	{
		Information("GitCommitAndPush");
		GitAddAll("./");
		GitCommit("./", "Build Agent", "buildbot@inmobile.dk", "Updated version number");
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
