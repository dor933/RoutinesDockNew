using routines.package.dockerpackage.Application;
using routines.package.dockerpackage.classes;

namespace routines.package.dockerprojtes
    
{
    internal class Program 
    {
        static async Task Main(string[] args)
        {
            var test = OptionFilesBuilder.Test();
            Console.WriteLine(test);

            var dockerHandler = new DockerHandler("dor93", "docker.io", "Vad62669!");
            var login = await dockerHandler.Login("dor93", "docker.io", "Vad62669!");
            // var pull = await dockerHandler.PullImage("myfirstnodeimg", "latest");
            var create = await dockerHandler.CreateContainer("myfirstnodeimg", "latest", "myfirstnodecontainer");
            //show the whole create result
            string containerid= create.StandardOutput.Trim();            



            var start= await dockerHandler.StartContainer(containerid);
            
           

        }
    }
}