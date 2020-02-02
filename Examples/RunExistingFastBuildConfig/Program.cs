using System;
using System.Threading.Tasks;
using FastBuild.Net;

namespace RunExistingFastBuildConfig
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var startInfo = new FastBuildStartInfo
            {
                FastBuildExecutableLocation = "../../../../../External/FastBuild/0.99/FBuild.exe",
                FastBuildConfigLocation = "FastBuild/FBuild.bff",
                ShowCommands = true,
                Verbose = true
            };

            try
            {
                using (var fastBuild = new FastBuildRunner(startInfo))
                {
                    await fastBuild.Run();
                }
                Console.WriteLine("FastBuild run has completed.");
            }
            catch (FastBuildRunnerException e)
            {
                Console.WriteLine($"Message: {e.Message}");
                Console.WriteLine($"Output:\n{e.ProcessStandardOutput}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }
    }
}
