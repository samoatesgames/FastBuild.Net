using System;
using FastBuild.Net;
using FastBuild.Net.Config;

namespace GenerateFastBuildConfig
{
    class Program
    {
        static void Main(string[] args)
        {

            var config = new FastBuildConfig();
            config.Add(new Compiler("texconv")
            {
                Executable = "texconv.exe",
                CompilerFamily = Compiler.CompilerFamilyEnum.Custom,
                AllowDistribution = true,
                SimpleDistributionMode = true
            });

            config.Save("FBuild.bff");
        }
    }
}
