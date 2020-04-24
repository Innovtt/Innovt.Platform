using System;
using System.Collections.Generic;
using Innovt.PublishToolbelt.AssetsDeploy;
using Innovt.PublishToolbelt.Minify;

namespace Innovt.PublishToolbelt
{
    class Program
    {
        static void ExecTool(string toolName, List<Argument>  parameters)
        {
            if ("Minify".Equals(toolName, StringComparison.CurrentCultureIgnoreCase))
            {
                var minify = new MinifyManager();

                minify.Minify(parameters);
            }
            else
            {
                if ("AssetsDeploy".Equals(toolName, StringComparison.CurrentCultureIgnoreCase))
                {
                    var dep = new AssetsDeployManager();

                    dep.Deploy(parameters);
                }
                else
                {
                    Console.WriteLine("Tool not Found. Available tools are Minify and AssetsDeploy");
                }
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Innovt Publish Toolbelt");

            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Helper");

                Console.WriteLine("You have to inform the Tool Name.");
                Console.WriteLine("Tool Name Availables: ");
                Console.WriteLine("1) Minify");
                Console.WriteLine("2) AssetsDeploy");
                Console.WriteLine("Example:  Innovt.PublishToolbelt.exe -T=minify");
                Console.ReadKey();
            }               
            else
            {
                var arguments = ArgumentHelper.Parse(args);

                var tool = ArgumentHelper.Find("-T", arguments);

                ExecTool(tool, arguments);
            }
        }
    }
}
