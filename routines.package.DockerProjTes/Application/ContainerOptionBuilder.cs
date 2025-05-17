namespace routines.package.dockerpackage.Application;

public static class OptionFilesBuilder
{

   public static string Test()
   {
    return "Test";
   }

    public static List<string> BuildPortsBindings(List<KeyValuePair<int, int>> ports, List<string> arguments)
    {
        
            foreach (KeyValuePair<int,int> port in ports)
            {
                arguments.Add("-p");
                arguments.Add($"{port.Key}:{port.Value}");
            }

        return arguments;

    }

    public static List<string> AttachVolumes(List<KeyValuePair<string, string>> myvolumes, List<string> arguments)
    {
        foreach (KeyValuePair<string,string> volume in myvolumes)
        {
            arguments.Add("-v");
            var hostPath = volume.Key.Replace("\\", "/");
            arguments.Add($"{hostPath}:{volume.Value}");
        }
        return arguments;
    }
}
