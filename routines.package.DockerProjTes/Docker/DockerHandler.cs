using package.sdk;
using Docker.DotNet;
using Docker.DotNet.Models;
using CliWrap;
using routines.package.dockerpackage.Application;

namespace routines.package.dockerpackage.classes;

public class DockerHandler
{
    private readonly string username;
    private readonly string registry;
    private readonly string password;

    public DockerHandler(string username, string registry, string password)
    {
     this.username = username;
     this.registry = registry;
     this.password = password;

     var dockerUri = Environment.OSVersion.Platform == PlatformID.Win32NT? 
     new Uri("npipe://./pipe/docker_engine"): 
     new Uri("unix:///var/run/docker.sock");


    }

    public async Task<CliWrap.CommandResult> Login(string username, string registry, string password)
    {
        var result = await Cli.Wrap("docker")
        .WithArguments(new[] { "login", registry, "-u", username, "-p", password })
        .ExecuteAsync();
        return result;
   
    }


    public async Task<CliWrap.CommandResult> PullImage(string imageName, string tag)
    {
        var result = await Cli.Wrap("docker")
        .WithArguments(new[] { "pull", $"{registry}/{imageName}:{tag}" })
        .ExecuteAsync();
        return result;

    }


    public async Task<CliWrap.CommandResult> CreateContainer(string imageName, string containerName, string imageTag, List<KeyValuePair<string,string>> ports, List<KeyValuePair<string,string>> volumes)
    {
        var myports = OptionFilesBuilder.BuildPortsBindings(ports);
        var myvolumes = OptionFilesBuilder.AttachVolumes(volumes);
        var result = await Cli.Wrap("docker")
        .WithArguments(new[] { "create", $"{registry}/{imageName}:{imageTag}", "-n", containerName, myports, myvolumes })
        .ExecuteAsync();
        return result;
    }

    public async Task<CliWrap.CommandResult> StartContainer(string containerId)
    {
        var result = await Cli.Wrap("docker")
        .WithArguments(new[] { "start", containerId })
        .ExecuteAsync();
        return result;
    }
    
    public async Task<CliWrap.CommandResult> StopContainer(string containerId)
    {
        var result = await Cli.Wrap("docker")
        .WithArguments(new[] { "stop", containerId })
        .ExecuteAsync();
        return result;
    }
    
    public async Task<CliWrap.CommandResult> RemoveContainer(string containerId)
    {
        var result = await Cli.Wrap("docker")
        .WithArguments(new[] { "rm", containerId })
        .ExecuteAsync();
        return result;
    }
    

}   
    
