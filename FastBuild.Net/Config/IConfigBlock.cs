using System;
using System.IO;

namespace FastBuild.Net.Config
{
    public interface IConfigBlock
    {
        void Serialize(StreamWriter outputStream);
        void Deserialize(StreamReader inputStream);
    }

    public static class OutputWritterExtensions
    {
        public static void WriteIfNoneDefault<TType>(
            this StreamWriter outputStream,
            Func<TType, string> getOutput,
            TType currentValues,
            TType defaultValues) where TType : class, IConfigBlock
        {
            var currentOutput = getOutput(currentValues);
            var defaultOutput = getOutput(defaultValues);
            if (currentOutput.Equals(defaultOutput))
            {
                return;
            }

            outputStream.WriteLine(currentOutput);
        }
    }
}
