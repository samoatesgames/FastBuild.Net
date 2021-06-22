using System;
using System.IO;
using System.Linq;

namespace FastBuild.Net.Config
{
    public class Compiler : IConfigBlock
    {
        public enum CompilerFamilyEnum
        {
            Auto,
            MSVC,
            Clang,
            GCC,
            SNC,
            CodeWarriorWii,
            GreenHillsWiiU,
            CudaNvcc,
            QtRcc,
            VBCC,
            OrbisWavePsslc,
            Custom
        }

        private static readonly Compiler s_default = new Compiler(null);

        /// <summary>
        /// The name of the compiler instance
        /// </summary>
        public string Alias { get; }

        /// <summary>
        /// Required
        /// The primary compiler executable that will be invoked
        /// by FASTBuild when this Compiler() is used.
        /// </summary>
        public string Executable { get; set; } = string.Empty;

        /// <summary>
        /// For distributed compilation, the specified files will also be
        /// synchronized to the remote machine. The relative location of
        /// the source files controls how they will be mirrored on the remote
        /// machine. Files in 'ExtraFiles' in the same directory or in
        /// sub-directories under the primary 'Executable' will be placed
        /// in the same relative location on the remote machine. 'ExtraFiles'
        /// in other folders will be placed at the same level as the executable. 
        /// </summary>
        public string[] ExtraFiles { get; set; } = new string[0];

        /// <summary>
        /// By default, FASTBuild will detect the compiler type based on the executable name.
        /// The .CompilerFamily property allows you to explicitly control the compiler type instead.
        /// This can be useful for:
        ///  * custom variants of compilers with unique naming
        ///  * custom executables used as compilers
        /// </summary>
        public CompilerFamilyEnum CompilerFamily { get; set; } = CompilerFamilyEnum.Auto;

        /// <summary>
        /// For compilers where distributed compilation is supported, said feature can be disabled.
        /// </summary>
        public bool AllowDistribution { get; set; } = true;

        /// <summary>
        /// When a compiler is distributed the .Compiler and .ExtraFiles hierarchy is
        /// replicated on the remote machine as documented above (see .ExtraFiles).
        /// The base path for this replication can be overridden by setting the
        /// .ExecutableRootPath property, allowing more flexibility in how the file hierarchy
        /// is replicated on the remote host.
        /// </summary>
        public string ExecutableRootPath { get; set; } = string.Empty;

        /// <summary>
        /// FASTbuild supports distributed compilation for certain compilers that it explicitly
        /// understands how to interact with in order to obtain dependency information
        /// (in addition to the simple primary input file). By setting .SimpleDistributionMode,
        /// FASTBuild can be told that the single file input of a "compiler" is the only dependency
        /// and thus can be safely used with distributed compilation. This allows distribution of
        /// custom tools or other useful work like texture conversion.
        /// </summary>
        public bool SimpleDistributionMode { get; set; } = true;

        /// <summary>
        /// When compiling on a remote host, a clean environment is used. If needed,
        /// environment variables can be set.
        /// </summary>
        public string[] CustomEnvironmentVariables { get; set; } = new string[0];

        /// <summary>
        /// FASTBuild uses the -E preprocessor option when compiling with Clang to
        /// preprocess the source code. In order to improve consistency between this
        /// preprocessed source and the original source, FASTBuild also uses the
        /// -frewrite-includes option by default. An example of this improved
        /// consistency is that compiler errors originating from macros will have
        /// the caret point to the correct column location of the source code instead
        /// of the column location where the error would be in the expanded macro.
        /// </summary>
        public bool ClangRewriteIncludes { get; set; } = true;

        /// <summary>
        /// An issue exists in the Visual Studio 2012 compiler whereby enums in preprocessed
        /// code are sometimes incorrectly processed when they lie on specific buffer
        /// alignment boundaries. This issue is fixed in Visual Studio 2013 and later.
        /// </summary>
        public bool VS2012EnumBugFix { get; set; } = false;

        /// <summary>
        /// When set, overrides the environment for local compiles
        /// This allows you to have a different environment per compiler
        /// </summary>
        public string[] Environment { get; set; } = new string[0];

        /// <summary>
        /// When set, activates "Light Caching" mode. Light Caching mode
        /// avoids using the compiler's preprocessor for cache lookups,
        /// instead allowing FASTBuild to parse the files itself to gather
        /// the required information. This parsing is significantly faster than
        /// for each file and additionally allows FASTBuild to eliminate
        /// redundant file parsing between object files, further accelerating cache lookups. 
        /// </summary>
        public bool UseLightCache_Experimental { get; set; } = false;

        /// <summary>
        /// A compiler block
        /// See: https://fastbuild.org/docs/functions/compiler.html
        /// </summary>
        /// <param name="alias">The name of the compiler block</param>
        public Compiler(string alias)
        {
            Alias = alias;
        }

        public void Serialize(StreamWriter outputStream)
        {
            outputStream.WriteLine($"Compiler( '{Alias}' )");
            outputStream.WriteLine("{");
            outputStream.WriteIfNoneDefault(v => $"  .Executable = '{v.Executable}'", this, s_default);
            outputStream.WriteIfNoneDefault(v => $"  .ExtraFiles = {{ {string.Join(", ", v.ExtraFiles.Select(x => $"'{x}'"))} }}", this, s_default);
            outputStream.WriteIfNoneDefault(v => $"  .CompilerFamily = '{FormatCompilerFamily(v.CompilerFamily)}'", this, s_default);
            outputStream.WriteIfNoneDefault(v => $"  .AllowDistribution = {v.AllowDistribution.ToString().ToLower()}", this, s_default);
            outputStream.WriteIfNoneDefault(v => $"  .ExecutableRootPath = '{v.ExecutableRootPath}'", this, s_default);
            outputStream.WriteIfNoneDefault(v => $"  .SimpleDistributionMode = {v.SimpleDistributionMode.ToString().ToLower()}", this, s_default);
            outputStream.WriteIfNoneDefault(v => $"  .CustomEnvironmentVariables = {{ {string.Join(", ", v.CustomEnvironmentVariables.Select(x => $"'{x}'"))} }}", this, s_default);
            outputStream.WriteIfNoneDefault(v => $"  .ClangRewriteIncludes = {v.ClangRewriteIncludes.ToString().ToLower()}", this, s_default);
            outputStream.WriteIfNoneDefault(v => $"  .VS2012EnumBugFix = {v.VS2012EnumBugFix.ToString().ToLower()}", this, s_default);
            outputStream.WriteIfNoneDefault(v => $"  .Environment = {{ {string.Join(", ", v.Environment.Select(x => $"'{x}'"))} }}", this, s_default);
            outputStream.WriteIfNoneDefault(v => $"  .UseLightCache_Experimental = {v.UseLightCache_Experimental.ToString().ToLower()}", this, s_default);
            outputStream.WriteLine("}");
        }

        public void Deserialize(StreamReader inputStream)
        {
            throw new System.NotImplementedException();
        }

        private string FormatCompilerFamily(CompilerFamilyEnum compilerFamily)
        {
            switch (compilerFamily)
            {
                case CompilerFamilyEnum.CodeWarriorWii:
                    return "codewarrior-wii";
                case CompilerFamilyEnum.GreenHillsWiiU:
                    return "greenhills-wiiu";
                case CompilerFamilyEnum.CudaNvcc:
                    return "cuda-nvcc";
                case CompilerFamilyEnum.QtRcc:
                    return "qt-rcc";
                case CompilerFamilyEnum.OrbisWavePsslc:
                    return "orbis-wave-psslc";
                default:
                    return compilerFamily.ToString().ToLower();
            }
        }
    }
}
