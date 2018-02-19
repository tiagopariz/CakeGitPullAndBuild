#addin nuget:?package=Cake.Git

var target = Argument("target", "building-projects");
var directory = "c:/GitHub/CakeAutomation";
var username = "tiagopariz";
var email = "tiagopariz@gmail.com";
var path = new FilePath[] { directory + "/test.txt" };

Task("git-checkout-master")
    .Does(() =>
    {
        GitCheckout(directory, "master", path);
    });

Task("git-pull")
    .IsDependentOn("git-checkout-master")
    .Does(() =>
    {
        GitPull(directory,  username, email);
    });

Task("building-projects")
    .IsDependentOn("git-pull")
    .Does(() =>
    {
        foreach(var project in GetFiles("././CakeAutomation/src/**/*.csproj"))
        {
            MSBuild(project.GetDirectory().FullPath,
                new MSBuildSettings {
                    Verbosity = Verbosity.Minimal,
                    Configuration = "Debug"
                }
            );
        }
    });

RunTarget(target);