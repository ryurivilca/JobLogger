using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Belatrix.JobLogger.Helpers
{
    public interface IFileManager
    {
        bool DirectoryExist(string logDirectoryPath);
        void AppendTextToFile(string filePath, string text);
    }

    [ExcludeFromCodeCoverage]
    public class FileManager : IFileManager
    {
        public bool DirectoryExist(string logDirectoryPath)
        {
            return Directory.Exists(logDirectoryPath);
        }

        public void AppendTextToFile(string filePath, string text)
        {
            using (var streamWriter = File.AppendText(filePath))
            {
                streamWriter.WriteLine(text);
            }
        }
    }
}
