using package.sdk;
using Docker.DotNet;
using Docker.DotNet.Models;
using CliWrap;
using routines.package.dockerpackage.Application;
using CliWrap.Buffered;

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

    public string PrintCredentials()
    {
        return $"Username: {username}, Registry: {registry}, Password: {password}";
    }
    

    public async Task<CliWrap.Buffered.BufferedCommandResult> Login(string username, string registry, string password)
    {
        var result = await Cli.Wrap("docker")
        .WithArguments(new[] { "login", registry, "-u", username, "-p", password })
        .ExecuteBufferedAsync();
        Console.WriteLine(result.StandardOutput);
        Console.WriteLine(result.StandardError);
        Console.WriteLine("Type is: " + result);
        return result;
   
    }


    public async Task<CliWrap.Buffered.BufferedCommandResult> PullImage(string imageName, string tag)
    {
        var result = await Cli.Wrap("docker")
        .WithArguments(new[] { "pull", $"{registry}/{username}/{imageName}:{tag}" })
        .ExecuteBufferedAsync();

        return result;

    }


    public async Task<CliWrap.Buffered.BufferedCommandResult> CreateContainer(string imageName,string imageTag, string containerName, List<KeyValuePair<string,string>> ports = null, List<KeyValuePair<string,string>> volumes = null)
    {
        var myports = ports == null ? null : OptionFilesBuilder.BuildPortsBindings(ports);
        var myvolumes = volumes == null ? null : OptionFilesBuilder.AttachVolumes(volumes);
        var result = await Cli.Wrap("docker")
        .WithArguments(new[] { "create", $"{registry}/{username}/{imageName}:{imageTag}", "-n", containerName, myports ?? "", myvolumes ?? "" })
        .ExecuteBufferedAsync();
        return result;
    }

    public async Task<CliWrap.Buffered.BufferedCommandResult> StartContainer(string containerId)
    {

        var result = await Cli.Wrap("docker")
        .WithArguments(new[] { "start", containerId })
        .ExecuteBufferedAsync();
        return result;
    }


    public async Task<CliWrap.Buffered.BufferedCommandResult> StopContainer(string containerId)
    {
        var result = await Cli.Wrap("docker")
        .WithArguments(new[] { "stop", containerId })
        .ExecuteBufferedAsync();
        return result;
    }
    
    public async Task<CliWrap.Buffered.BufferedCommandResult> RemoveContainer(string containerId)
    {
        var result = await Cli.Wrap("docker")
        .WithArguments(new[] { "rm", containerId })
        .ExecuteBufferedAsync();
        return result;
    }
    

}   
    
