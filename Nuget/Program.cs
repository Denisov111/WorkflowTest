using NuGet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nuget
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var myPackage = new ZipPackage(@"C:\Users\Maxxx\AppData\Local\Programs\UiPath\Studio\Packages\UiPath.Core.Activities.2018.3.6897.22553.nupkg");

            Console.WriteLine("Id: {0}", myPackage.Id);
            Console.WriteLine("Version: {0}", myPackage.Version);
            Console.WriteLine(
                "Assemblies: {0}",
                myPackage.AssemblyReferences.Select(a => a.Name).ToArray());
        }
    }
}
