using System;
using System.IO;
using System.Linq;

namespace FastBuild.Net.Config
{
    public class Alias : IConfigBlock
    {
        /// <summary>
        /// The name of the alias
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// One or more targets must be provided. Targets can be previously
        /// defined nodes, or files external to the build process. 
        /// </summary>
        public string[] Targets { get; set; } = new string[0];

        /// <summary>
        /// Hide a target from -showtargets
        /// </summary>
        public bool Hidden { get; set; } = false;

        /// <summary>
        /// Alias() can be used to create aliases to other nodes in the build. Aliases can be referenced
        /// within other Functions in the BFF config or from the command line. An Alias can refer
        /// to one or more nodes, and are typically used to simplify and de-duplicate configurations. 
        /// See: https://fastbuild.org/docs/functions/alias.html
        /// </summary>
        /// <param name="name">The name of the alias block</param>
        public Alias(string name)
        {
            Name = name;
        }

        public void Serialize(StreamWriter outputStream)
        {
            outputStream.WriteLine($"Alias( '{Name}' )");
            outputStream.WriteLine("{");
            outputStream.WriteLine($"  .Targets = {{ {string.Join(", ", Targets.Select(x => $"'{x}'"))} }}");
            outputStream.WriteLine($"  .Hidden = {Hidden.ToString().ToLower()}");
            outputStream.WriteLine("}");
        }

        public void Deserialize(StreamReader inputStream)
        {
            throw new System.NotImplementedException();
        }
    }
}
