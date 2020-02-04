using System.IO;

namespace FastBuild.Net.Config
{
    public interface IConfigBlock
    {
        void Serialize(StreamWriter outputStream);
        void Deserialize(StreamReader inputStream);
    }
}
