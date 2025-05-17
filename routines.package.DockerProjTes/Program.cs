using CliWrap.Buffered;
using routines.package.dockerpackage.classes;
using routines.package.dockerprojtes.Utils;
using System.Text.Json.Nodes;



namespace routines.package.dockerprojtes
    
{
    internal class Program 
    {
        static async Task Main(string[] args)
        {
            try
            {

                DockerHandler dockerHandler = new DockerHandler("dor93", "docker.io", Environment.GetEnvironmentVariable("DockerHubPass")!);
                BufferedCommandResult login = await dockerHandler.Login();
                //var remove= await dockerHandler.RemoveContainer("myfirstnodecontainer",true);
                //var pushimage= await dockerHandler.PushImage("dockersample", "v1.0");
                //var pull = await dockerHandler.PullImage("dockersample", "v1.0");
                var ports = new List<KeyValuePair<int, int>>()
            {
                new KeyValuePair<int, int>(3000, 3000)            };
                var volumes = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("C:\\Program Files\\Files_For_Docker_Proj", "/app/test"),
            };
                BufferedCommandResult create = await dockerHandler.CreateContainer("dockersample", "v1.0", "myfirstnodecontainer", ports, volumes, true);
                //show the whole create result
                string containerid = create.StandardOutput.Trim();
                BufferedCommandResult start = await dockerHandler.StartContainer(containerid);
                BufferedCommandResult list = await dockerHandler.ListContainers(true);
                BufferedCommandResult logs = await dockerHandler.GetContainerLogs(containerid);
                string[] containerslist = InternalFuctions.NdJsonSplitter(list.StandardOutput);
                JsonArray containerslistnew =InternalFuctions.AddJsonsToArray(containerslist);
                JsonObject objnew = new JsonObject();
                objnew["logs"] = logs.StandardOutput;
                objnew["containers"] = containerslistnew;
                Console.ReadKey();
                
            
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }


        }
    }
}