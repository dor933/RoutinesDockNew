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

    

    public async Task<BufferedCommandResult> Login()
    {
        BufferedCommandResult result = await Cli.Wrap("docker")
        .WithArguments(new[] { "login", registry, "-u", username, "-p", password  })
        .ExecuteBufferedAsync();
   
        return result;
   
    }


    public async Task<BufferedCommandResult> PullImage(string imageName, string tag)
    {
        BufferedCommandResult result = await Cli.Wrap("docker")
        .WithArguments(new[] { "pull", $"{registry}/{username}/{imageName}:{tag}" })
        .ExecuteBufferedAsync();

        return result;

    }

    public async Task<BufferedCommandResult> PushImage(string imageName, string tag)
    {
        await Cli.Wrap("docker").WithArguments(new[] { "tag", $"{imageName + ":" + tag}", $"{username+"/"+ imageName + ":" + tag}" } ).ExecuteBufferedAsync();

        BufferedCommandResult result = await Cli.Wrap("docker")
        .WithArguments(new[] { "push", $"{registry}/{username}/{imageName}:{tag}" })
        .ExecuteBufferedAsync();

        return result;
    }

    public async Task<BufferedCommandResult> CreateContainer(string imageName, string imageTag, string containerName, List<KeyValuePair<int, int>> ports = null, List<KeyValuePair<string, string>> volumes = null, bool isremovable = false)
    {
        List<string> arguments = new List<string> { "create" };

        if (isremovable)
        {
            arguments.Add("--rm");
        }

        arguments.Add("--name");
        arguments.Add(containerName);

       if(ports!=null)
        OptionFilesBuilder.BuildPortsBindings(ports, arguments);

  
       if (volumes != null)        
        OptionFilesBuilder.AttachVolumes(volumes, arguments);

        arguments.Add($"docker.io/dor93/{imageName}:{imageTag}");

        BufferedCommandResult result = await Cli.Wrap("docker")
            .WithArguments(args => args.Add(arguments))
            .ExecuteBufferedAsync();

       return result;
    }

    public async Task<BufferedCommandResult> StartContainer(string containerId)
    {

        BufferedCommandResult result = await Cli.Wrap("docker")
        .WithArguments(new[] {"start", containerId  })
        .ExecuteBufferedAsync();

        return result;
    }


    public async Task<BufferedCommandResult> StopContainer(string containerId)
    {
        BufferedCommandResult result = await Cli.Wrap("docker")
        .WithArguments(new[] { "stop", containerId })
        .ExecuteBufferedAsync();

        return result;
    }

    public async Task<BufferedCommandResult> KillContainer(string containerId)
    {
        BufferedCommandResult result = await Cli.Wrap("docker")
        .WithArguments(new[] { "kill", containerId })
        .ExecuteBufferedAsync();

        return result;
    }

    public async Task<BufferedCommandResult> RemoveContainer(string containerId, bool forcefully=false)
    {
        BufferedCommandResult result = await Cli.Wrap("docker")
        .WithArguments(new[] { "rm",forcefully? "-f":"" , containerId  })
        .ExecuteBufferedAsync();

        return result;
    }

    public async Task<BufferedCommandResult> ListContainers(bool all = false)
    {
        BufferedCommandResult result = await Cli.Wrap("docker")
        .WithArguments(new[] { "ps", all ? "-a" : "", "--format","{{json .}}" })
        .ExecuteBufferedAsync();

        return result;
    }

    public async Task<BufferedCommandResult> RunCommandOnContainer(string containerId, string command)
    {
        BufferedCommandResult result = await Cli.Wrap("docker")
            .WithArguments(new[] { "exec", containerId, command  })
            .ExecuteBufferedAsync();

        return result;
    }

    public async Task<BufferedCommandResult> GetContainerLogs(string containerId, string lines=null)
    {
        List<string> arguments = new List<string> { "logs", containerId };
        if (lines != null)
        {
            arguments.Add("--tail");
            arguments.Add(lines);
        }

        BufferedCommandResult result = await Cli.Wrap("docker")
        .WithArguments(args=> args.Add(arguments))
        .ExecuteBufferedAsync();

       return result;
    }

    public async Task<BufferedCommandResult> LogOut()
    {
        BufferedCommandResult result = await Cli.Wrap("docker")
        .WithArguments(new[] { "logout", registry })
        .ExecuteBufferedAsync();

       return result;
    }

}   
    
