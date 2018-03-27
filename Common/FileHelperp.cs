using System.IO;
using System.Linq;
using System.Threading;
using Ionic.Zip;

namespace Common
{
    /// <summary>
    ///     Helper method for working with files
    /// </summary>
    public class FileHelper : IFileHelper
    {
        public string[] GetFilesInDirectories(string targetDirectory)
        {
            // Process the list of files found in the directory.
            var fileEntries = Directory.GetFiles(targetDirectory);

            //foreach (string fileName in fileEntries)
            //{
            //}
            // Recurse into subdirectories of this directory.
            var subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (var subdirectory in subdirectoryEntries)
                fileEntries = GetFilesInDirectories(subdirectory).Concat(fileEntries).ToArray();
            return fileEntries;
        }

        public void MoveFiles(string source, string replacedSourcePart, string insteadOfReplacedPart)
        {
            var destinationFinall = source.Replace(replacedSourcePart, insteadOfReplacedPart);
            var isExists = Directory.Exists(destinationFinall);
            if (!isExists)
                Directory.CreateDirectory(Path.GetDirectoryName(destinationFinall));
            File.Copy(source, destinationFinall, true);
            Thread.Sleep(200);
            File.Delete(source);
        }

        public void ZipFiles(string source, string destination)
        {
            var isExists = Directory.Exists(destination);
            if (!isExists)
                Directory.CreateDirectory(Path.GetDirectoryName(destination));
            if (File.Exists(destination))
                using (var zip = ZipFile.Read(destination))
                {
                    zip.UpdateFile(source, "");
                    zip.Save();
                }
            else
                using (var zip = new ZipFile())
                {
                    zip.AddFile(source, "");
                    zip.Save(destination);
                }
            Thread.Sleep(200);
            File.Delete(source);
        }
    }
}