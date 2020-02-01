using System;
using SamOatesGames.System;

namespace FastBuild.Net
{
    public class FastBuildRunnerException : Exception
    {
        public FastBuildStartInfo StartInfo { get; }

        public string ProcessStandardOutput { get; }
        public string ProcessStandardError { get; }

        public FastBuildRunnerException(string message, FastBuildStartInfo startInfo) 
            : this(message, startInfo, null)
        {
        }

        public FastBuildRunnerException(string message,
            FastBuildStartInfo startInfo,
            AsyncProcessResult processResult) 
            : base(message)
        {
            StartInfo = startInfo;
            ProcessStandardOutput = processResult?.StandardOutput;
            ProcessStandardError = processResult?.StandardError;
        }
    }
}
