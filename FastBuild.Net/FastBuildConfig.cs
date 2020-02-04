using System.Collections.Generic;
using System.IO;
using System.Text;
using FastBuild.Net.Config;

namespace FastBuild.Net
{
    public class FastBuildConfig
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly List<IConfigBlock> m_configBlocks = new List<IConfigBlock>();

        public void Save(string filePath)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new StreamWriter(memoryStream, Encoding.UTF8, 4096, true))
                {
                    writer.WriteLine("");
                    writer.WriteLine("// This file is auto-generated //");
                    writer.WriteLine("");
                    foreach (var configBlock in m_configBlocks)
                    {
                        configBlock.Serialize(writer);
                        writer.WriteLine("");
                    }
                }

                memoryStream.Position = 0;
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    memoryStream.CopyTo(fileStream);
                }
            }
        }

        public bool TryLoad(string filePath, out FastBuildConfig config)
        {
            config = null;
            return false;
        }

        public bool Add(IConfigBlock block)
        {
            // TODO: Check for duplicate blocks with the same alias and type
            m_configBlocks.Add(block);
            return true;
        }
    }
}
