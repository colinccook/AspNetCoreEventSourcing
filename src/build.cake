// The MIT License (MIT)
// 
// Copyright (c) 2015-2017 Rasmus Mikkelsen
// Copyright (c) 2015-2017 eBay Software Foundation
// https://github.com/eventflow/EventFlow
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

#r "System.IO.Compression.FileSystem"
#r "System.Xml"

#tool "nuget:?package=NUnit.ConsoleRunner"
#tool "nuget:?package=OpenCover"

using System.IO.Compression;
using System.Net;
using System.Xml;

var VERSION = GetArgumentVersion();
var PROJECT_DIR = Context.Environment.WorkingDirectory.FullPath;
var CONFIGURATION = "Release";
var REGEX_NUGETPARSER = new System.Text.RegularExpressions.Regex(
    @"(?<group>[a-z]+)\s+(?<package>[a-z\.0-9]+)\s+\-\s+(?<version>[0-9\.]+)",
    System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled);

// IMPORTANT DIRECTORIES
var DIR_OUTPUT_PACKAGES = System.IO.Path.Combine(PROJECT_DIR, "Build", "Packages");
var DIR_OUTPUT_REPORTS = System.IO.Path.Combine(PROJECT_DIR, "Build", "Reports");
var DIR_OUTPUT_DOCUMENTATION = System.IO.Path.Combine(PROJECT_DIR, "Build", "Documentation");
var DIR_DOCUMENTATION = System.IO.Path.Combine(PROJECT_DIR, "Documentation");
var DIR_BUILT_DOCUMENTATION = System.IO.Path.Combine(DIR_DOCUMENTATION, "_build");
var DIR_BUILT_HTML_DOCUMENTATION = System.IO.Path.Combine(DIR_BUILT_DOCUMENTATION, "html");

// IMPORTANT FILES
var FILE_OPENCOVER_REPORT = System.IO.Path.Combine(DIR_OUTPUT_REPORTS, "opencover-results.xml");
var FILE_NUNIT_XML_REPORT = System.IO.Path.Combine(DIR_OUTPUT_REPORTS, "nunit-results.xml");
var FILE_NUNIT_TXT_REPORT = System.IO.Path.Combine(DIR_OUTPUT_REPORTS, "nunit-output.txt");
var FILE_DOCUMENTATION_MAKE = System.IO.Path.Combine(DIR_DOCUMENTATION, "make.bat");
var FILE_SOLUTION = System.IO.Path.Combine(PROJECT_DIR, "EventFlow.Documentation.sln");
var FILE_OUTPUT_DOCUMENTATION_ZIP = System.IO.Path.Combine(
    DIR_OUTPUT_DOCUMENTATION,
    string.Format("EventFlow-HtmlDocs.zip", VERSION));

// =====================================================================================================
Task("Default")
    .IsDependentOn("Documentation");

// =====================================================================================================
Task("Clean")
    .Does(() =>
        {
            CleanDirectories(new []
                {
                    DIR_OUTPUT_REPORTS,
                    DIR_OUTPUT_DOCUMENTATION,
                    DIR_BUILT_DOCUMENTATION,
                });
				
			DeleteDirectories(GetDirectories("**/bin"), true);
			DeleteDirectories(GetDirectories("**/obj"), true);
        });
	
// =====================================================================================================
Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
        {
			DotNetCoreRestore(
				".", 
				new DotNetCoreRestoreSettings()
				{
					ArgumentCustomization = aggs => aggs.Append(GetDotNetCoreArgsVersions())
				});
        });
		
// =====================================================================================================
Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
        {
            DotNetCoreBuild(
				".", 
				new DotNetCoreBuildSettings()
				{
					Configuration = CONFIGURATION,
					ArgumentCustomization = aggs => aggs
                        .Append(GetDotNetCoreArgsVersions())
                        .Append("/p:ci=true")
                        .Append("/p:SourceLinkEnabled=true")
				});
        });

// =====================================================================================================
Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
        {
            ExecuteTest("./Source/**/bin/" + CONFIGURATION + "/**/EventFlow.Documentation*.dll", FILE_NUNIT_XML_REPORT);
        })
	.Finally(() => 
        {
            UploadArtifact(FILE_NUNIT_TXT_REPORT);
            UploadTestResults(FILE_NUNIT_XML_REPORT);
        });

// =====================================================================================================
Task("Documentation")
    .IsDependentOn("Clean")
    .Does(() =>
        {
            ExecuteCommand(FILE_DOCUMENTATION_MAKE, "html", DIR_DOCUMENTATION);

            ZipFile.CreateFromDirectory(DIR_BUILT_HTML_DOCUMENTATION, FILE_OUTPUT_DOCUMENTATION_ZIP);
        });

// =====================================================================================================
Task("All")
    .IsDependentOn("Documentation")
    .IsDependentOn("Test")
    .Does(() =>
        {

        });

// =====================================================================================================

Version GetArgumentVersion()
{
    return Version.Parse(EnvironmentVariable("APPVEYOR_BUILD_VERSION") ?? "0.0.1");
}

string GetDotNetCoreArgsVersions()
{
	var version = GetArgumentVersion().ToString();
	
	return string.Format(
		@"/p:Version={0} /p:AssemblyVersion={0} /p:FileVersion={0} /p:ProductVersion={0}",
		version);
}

void UploadArtifact(string filePath)
{
    if (!FileExists(filePath))
    {
        Information("Skipping uploading of artifact, does not exist: {0}", filePath);
        return;
    }

    if (AppVeyor.IsRunningOnAppVeyor)
    {
        Information("Uploading artifact: {0}", filePath);

        AppVeyor.UploadArtifact(filePath);
    }
    else
    {
        Information("Not on AppVeyor, skipping artifact upload of: {0}", filePath);
    }
}

void UploadTestResults(string filePath)
{
    if (!FileExists(filePath))
    {
        Information("Skipping uploading of test results, does not exist: {0}", filePath);
        return;
    }

    if (AppVeyor.IsRunningOnAppVeyor)
    {
        Information("Uploading test results: {0}", filePath);

        try
        {
            using (var webClient = new WebClient())
            {
                webClient.UploadFile(
                    string.Format(
                        "https://ci.appveyor.com/api/testresults/nunit3/{0}",
                        Environment.GetEnvironmentVariable("APPVEYOR_JOB_ID")),
                    filePath);
            }
        }
        catch (Exception e)
        {
            Error(
                "Failed to upload '{0}' due to {1} - {2}: {3}",
                filePath,
                e.Message,
                e.GetType().Name,
                e.ToString());
        }
    }    
    else
    {
        Information("Not on AppVeyor, skipping test result upload of: {0}", filePath);
    }
}

string ExecuteCommand(string exePath, string arguments = null, string workingDirectory = null)
{
    Information("Executing '{0}' {1}", exePath, arguments ?? string.Empty);

    using (var process = new System.Diagnostics.Process())
    {
        process.StartInfo = new System.Diagnostics.ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                FileName = exePath,
                Arguments = arguments,
                WorkingDirectory = workingDirectory,
            };
        process.Start();

        var output = process.StandardOutput.ReadToEnd();

        if (!process.WaitForExit(30000))
        {
            throw new Exception("Failed to stop process!");
        }

        Debug(output);

        if (process.ExitCode != 0)
        {
            throw new Exception(string.Format("Error code {0} was returned", process.ExitCode));
        }

        return output;
    }
}

void ExecuteTest(string files, string resultsFile)
{
	OpenCover(tool => 
		{
			tool.NUnit3(
				files,
				new NUnit3Settings
					{
						Framework = "net-4.5",
						Timeout = 600000,
						ShadowCopy = false,
						NoHeader = true,
						NoColor = true,
						DisposeRunners = true,
						OutputFile = FILE_NUNIT_TXT_REPORT,
						Results = new []
                            {
                                new NUnit3Result
                                    {
                                        FileName = resultsFile,
                                    }
                            }
					});
        },
    new FilePath(FILE_OPENCOVER_REPORT),
    new OpenCoverSettings
        {
            ArgumentCustomization = aggs => aggs.Append("-returntargetcode")
        }
        .WithFilter("+[EventFlow*]*")
        .WithFilter("-[*Tests]*")
        .WithFilter("-[*TestHelpers]*")
        .WithFilter("-[*Shipping*]*"));
}

RunTarget(Argument<string>("target", "Package"));
