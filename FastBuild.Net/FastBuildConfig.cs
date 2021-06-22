using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        public void SaveToFolder(string folderPath)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new StreamWriter(memoryStream, Encoding.UTF8, 4096, true))
                {
                    writer.WriteLine("//-----------------------------//");
                    writer.WriteLine("// This file is auto-generated //");
                    writer.WriteLine("//-----------------------------//");
                    writer.WriteLine("");
                    foreach (var configBlock in m_configBlocks)
                    {
                        configBlock.Serialize(writer);
                        writer.WriteLine("");
                    }
                }

                var filePath = Path.Combine(folderPath, "FBuild.bff");

                memoryStream.Position = 0;
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    memoryStream.CopyTo(fileStream);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public bool TryLoad(string filePath, out FastBuildConfig config)
        {
            config = null;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        public bool TryAdd(IConfigBlock block)
        {
            if (m_configBlocks.Any(b => b.Equals(block)))
            {
                return false;
            }

            // TODO: Check for duplicate blocks with the same alias and type
            m_configBlocks.Add(block);
            return true;
        }
    }
}
