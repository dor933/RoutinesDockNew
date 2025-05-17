using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace routines.package.dockerprojtes.Utils
{
    static internal class InternalFuctions
    {

        public static JsonArray AddJsonsToArray(string[] lines)
        {
         
            JsonArray jsonarray = new JsonArray();

            foreach (string line in lines)
            {

                jsonarray.Add(JsonNode.Parse(line));
            }

            return jsonarray;
        }

        public static string[] NdJsonSplitter(string input)
        {

            string[] lines = input
           .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            return lines;

        }
    }
}
