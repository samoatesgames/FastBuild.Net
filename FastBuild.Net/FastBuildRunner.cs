using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SamOatesGames.System;

namespace FastBuild.Net
{
    public class FastBuildRunner : IDisposable
    {
        public enum OutputType
        {
            Information,
            Error
        }

        private readonly FastBuildStartInfo m_startInfo;
        private readonly Action<OutputType, string> m_outputCallbackAction;

        public FastBuildRunner(FastBuildStartInfo startInfo, Action<OutputType, string> outputCallbackAction)
        {
            m_startInfo = startInfo;
            m_outputCallbackAction = outputCallbackAction;
        }

        public FastBuildRunner(FastBuildStartInfo startInfo) : this(startInfo, null)
        {
        }

        public void Dispose()
        {
        }

        public async Task Run()
        {
            await Run(CancellationToken.None);
        }

        public async Task Run(CancellationToken cancellationToken)
        {
            var absoluteFastBuildLocation = Path.GetFullPath(m_startInfo.FastBuildExecutableLocation);
            if (!File.Exists(absoluteFastBuildLocation))
            {
                throw new FileNotFoundException($"The FastBuild application at '{absoluteFastBuildLocation}' does not exist.");
            }

            var absoluteBffLocation = Path.GetFullPath(m_startInfo.FastBuildConfigLocation);
            if (!File.Exists(absoluteBffLocation))
            {
                throw new FileNotFoundException($"The FastBuild config at '{absoluteBffLocation}' does not exist.");
            }

            var absoluteBffDirectory = Path.GetDirectoryName(absoluteBffLocation);
            if (absoluteBffDirectory == null)
            {
                throw new DirectoryNotFoundException($"The FastBuild config directory for '{absoluteBffLocation}' is invalid.");
            }

            var bffFileName = Path.GetFileName(absoluteBffLocation);
            if (!bffFileName.Equals("FBuild.bff", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"The FastBuild config at '{absoluteBffLocation}' is not named 'FBuild.bff'");
            }

            var arguments = GenerateCommandLineArguments(m_startInfo);

            var startInfo = new AsyncProcessStartInfo(absoluteFastBuildLocation, arguments)
            {
                CaptureOutputToProcessResult = ProcessOutputCaptureMode.Both,
                WorkingDirectory = absoluteBffDirectory
            };

            if (m_outputCallbackAction != null)
            {
                startInfo.OnStandardOutputReceived += x => m_outputCallbackAction(OutputType.Information, x);
                startInfo.OnStandardErrorReceived += x => m_outputCallbackAction(OutputType.Error, x);
            }

            using (var process = new AsyncProcess(startInfo, cancellationToken))
            {
                var result = await process.Run();
                if (result.ExitCode != 0)
                {
                    throw new FastBuildRunnerException($"FastBuild resulted in the exit code '{result.ExitCode}'",
                        m_startInfo,
                        result);
                }
            }
        }

        private string GenerateCommandLineArguments(FastBuildStartInfo startInfo)
        {
            var builder = new StringBuilder();

            switch (startInfo.CacheMode)
            {
                case CacheModeEnum.Read:
                    builder.Append("-cacheread ");
                    break;
                case CacheModeEnum.Write:
                    builder.Append("-cachewrite ");
                    break;
                case CacheModeEnum.ReadWrite:
                    builder.Append("-cache ");
                    break;
            }

            if (startInfo.CacheInfo)
            {
                builder.Append("-cacheinfo ");
            }

            if (startInfo.CacheTrimSize != 0)
            {
                builder.AppendFormat("-cachetrim {0} ", startInfo.CacheTrimSize);
            }

            if (startInfo.VerboseCache)
            {
                builder.Append("-cacheverbose ");
            }

            if (startInfo.Clean)
            {
                builder.Append("-clean ");
            }

            if (startInfo.CompareDatabase)
            {
                builder.Append("-compdb ");
            }

            if (startInfo.DistributedCompiling)
            {
                builder.Append("-dist ");
            }

            if (startInfo.VerboseDistribution)
            {
                builder.Append("-distverbose ");
            }

            if (startInfo.ForceRemoteWorker)
            {
                builder.Append("-forceremote ");
            }

            if (startInfo.GenerateReport)
            {
                builder.Append("-report ");
            }

            if (startInfo.ShowCommands)
            {
                builder.Append("-showcmds ");
            }

            if (startInfo.ShowTargets)
            {
                builder.Append("-showtargets ");
            }

            if (startInfo.ShowAllTargets)
            {
                builder.Append("-showalltargets ");
            }

            if (startInfo.Summary)
            {
                builder.Append("-summary ");
            }

            if (startInfo.Verbose)
            {
                builder.Append("-verbose ");
            }

            return builder.ToString();
        }
    }
}
