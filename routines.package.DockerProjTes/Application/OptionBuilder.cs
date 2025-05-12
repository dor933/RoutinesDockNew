
using package.sdk;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace routines.package.dockerpackage.Application;

public static class OptionFilesBuilder
{
    public static string BuildPortsBindings(List<KeyValuePair<string,string>> ports)
    {
       
        var portBindings = "";
        foreach (var port in ports)
        {
            portBindings += $"-p {port.Key}:{port.Value},";
        }
        return portBindings.TrimEnd(',');
        
    }

    public static string AttachVolumes(List<KeyValuePair<string,string>> myvolumes)
    {
        var volumes = "";
        foreach (var volume in myvolumes)
        {
            volumes += $"-v {volume.Key}:{volume.Value},";
        }
        return volumes.TrimEnd(',');
     
     
    }
}
