using System;
using System.IO;
using System.Threading.Tasks;
using FastBuild.Net;

namespace RunExistingFastBuildConfig
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            if (Directory.Exists("FastBuild/output/"))
            {
                foreach (var file in Directory.EnumerateFiles("FastBuild/output/"))
                {
                    File.Delete(file);
                }
            }

            SetupTestData("FastBuild/input/logo.png", 1000);

            var startInfo = new FastBuildStartInfo
            {
                FastBuildExecutableLocation = "../../../../../External/FastBuild/0.99/FBuild.exe",
                FastBuildConfigLocation = "FastBuild/FBuild.bff",
                ShowCommands = true,
                Verbose = true,
                GenerateReport = true
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

        private static void SetupTestData(string sourceImageLocation, int imageCount)
        {
            var sourceImagePath = Path.GetFullPath(sourceImageLocation);
            if (!File.Exists(sourceImagePath))
            {
                Console.WriteLine($"Failed to find the source input image '{sourceImagePath}'");
                Console.ReadLine();
                return;
            }

            var sourceDirectory = Path.GetDirectoryName(sourceImagePath);
            var sourceNoExtension = Path.GetFileNameWithoutExtension(sourceImagePath);
            var sourceExtension = Path.GetExtension(sourceImagePath);

            if (sourceDirectory == null)
            {
                Console.WriteLine($"Failed to get the source directory of the image path '{sourceImagePath}'");
                Console.ReadLine();
                return;
            }

            for (var i = 0; i < imageCount; ++i)
            {
                var targetPath = Path.Combine(sourceDirectory, $"{sourceNoExtension}-{i}.{sourceExtension}");
                File.Copy(sourceImagePath, targetPath, true);
            }
        }
    }
}
