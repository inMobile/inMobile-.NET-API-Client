#addin "Cake.FileHelpers"
#addin nuget:?package=Cake.Git

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var version = Argument("releaseVersion", "");
var githubUser = Argument("githubUser", "");
var githubPass = Argument("githubPass", "");

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
    // Executed BEFORE the first task.
    var cakeVersion = typeof(ICakeContext).Assembly.GetName().Version.ToString();
    Information($"Running tasks using version {cakeVersion} of Cake ...");

	if (string.IsNullOrWhiteSpace(version)) 
	{
		throw new ArgumentException("Cannot be null or empty", nameof(version));
	}
	if (string.IsNullOrWhiteSpace(githubUser))
	{
		throw new ArgumentException("Cannot be null or empty", nameof(githubUser));
	}
	if (string.IsNullOrWhiteSpace(githubPass))
	{
		throw new ArgumentException("Cannot be null or empty", nameof(githubPass));
	}
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
		GitAddAll("./");
		GitCommit("./", "inMobile BuildBot", "buildbot@inmobile.dk", "Updated version number");
		GitPush("./", githubUser, githubPass);
	});

Task("CreateGitTag")
	.IsDependentOn("GitCommitAndPush")
	.Does(() => 
	{
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
