using System;

namespace FastBuild.Net
{
    public class FastBuildStartInfo
    {
        /// <summary>
        /// The absolute path to the Fbuild.exe
        /// </summary>
        public string FastBuildExecutableLocation { get; set; }

        /// <summary>
        /// The absolute path to the .bff Fast Build config to use.
        /// </summary>
        public string FastBuildConfigLocation { get; set; }

        /// <summary>
        /// Enable usage of the build cache.
        /// The cache can be enabled as read only or write only.
        /// This can be useful for automated build systems, where you might like one
        /// machine to populate the cache for read-only use by other users.
        /// </summary>
        public CacheModeEnum CacheMode { get; set; } = CacheModeEnum.None;

        /// <summary>
        /// Emit summary of objects in the cache.
        /// This can be used to understand the total size of the cache and how quickly it is growing.
        /// </summary>
        public bool CacheInfo { get; set; }

        /// <summary>
        /// Reduce the size of the cache to the specified size in MiB.
        /// This will delete items in the cache (oldest first) until under the requested size. 
        /// </summary>
        public uint CacheTrimSize { get; set; }

        /// <summary>
        /// Provide additional information about cache interactions, including cache keys,
        /// explicit hit/miss/store information and performance metrics.
        /// </summary>
        public bool VerboseCache { get; set; }

        /// <summary>
        /// Force a clean build.
        /// The build configuration file is re-parsed and all existing dependency information is discarded.
        /// </summary>
        public bool Clean { get; set; }

        /// <summary>
        /// Instead of building specified targets generate a JSON compilation database for them.
        /// Resulting compilation database will include entries for all source files from ObjectList
        /// and Library nodes that are dependencies of the specified targets.
        /// </summary>
        public bool CompareDatabase { get; set; }

        /// <summary>
        /// Enable distributed compilation.
        /// Requires some build configuration.
        /// </summary>
        public bool DistributedCompiling { get; set; }

        /// <summary>
        /// Print detailed information about distributed compilation.
        /// This can help when investigating connectivity issues.
        /// Activates 'DistributedCompiling' if not already specified.
        /// </summary>
        public bool VerboseDistribution { get; set; }

        /// <summary>
        /// Prevents all local compilation of distributable jobs.
        /// Additionally, this option disabled use of the cache.
        /// </summary>
        public bool ForceRemoteWorker { get; set; }

        /// <summary>
        /// Output a detailed report at the end of the build.
        /// The report is written to report.html in the current directory.
        /// </summary>
        public bool GenerateReport { get; set; }

        /// <summary>
        /// Displays the full command lines passed to external tools as they are invoked.
        /// </summary>
        public bool ShowCommands { get; set; }

        /// <summary>
        /// Displays the list of targets defined in the bff configuration file,
        /// excluding those which have the .Hidden property set.
        /// </summary>
        public bool ShowTargets { get; set; }

        /// <summary>
        /// Displays the list of targets defined in the bff configuration file,
        /// including those which have the .Hidden property set.
        /// </summary>
        public bool ShowAllTargets { get; set; }

        /// <summary>
        /// Displays a summary upon build completion.
        /// </summary>
        public bool Summary { get; set; }

        /// <summary>
        /// Show detailed diagnostic information for debugging.
        /// </summary>
        public bool Verbose { get; set; }
    }

    [Flags]
    public enum CacheModeEnum
    {
        None = 0,
        Read = 1,
        Write = 2,
        ReadWrite = Read | Write
    }
}
