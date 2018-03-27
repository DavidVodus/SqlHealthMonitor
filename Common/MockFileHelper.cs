using Ionic.Zip;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace Common
{
  public  class MockFileHelper : IFileHelper
    {
        public  string[] GetFilesInDirectories(string targetDirectory)
        {

            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);

            //foreach (string fileName in fileEntries)
            //{
            //}
            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                fileEntries = GetFilesInDirectories(subdirectory).Concat(fileEntries).ToArray();
            return fileEntries;
        }

        public  void MoveFiles(string source,string replacedSourcePart,string insteadOfReplacedPart)
        {
            Random gen = new Random();
            int prob = gen.Next(100);
            bool isExists = Directory.Exists(insteadOfReplacedPart);
            isExists = (prob <= 20);
            string destinationFinall= source.Replace(replacedSourcePart,insteadOfReplacedPart);
            if (!isExists)
            { }
            Thread.Sleep(200);
        

        }

        public void ZipFiles(string source, string destination)
        {
            Random gen = new Random();
            int prob = gen.Next(100);
            bool isExists = Directory.Exists(destination) ;
            isExists = (prob <= 20);
            if (!isExists)
            { }
            if (File.Exists(destination))
            {
                using (ZipFile zip = new ZipFile())
                {
                    zip.UpdateFile(source,"");
                   // zip.Save();
                }
            }
            else
            {
                using (ZipFile zip = new ZipFile())
                {
                    zip.AddFile(source,"");
                 //   zip.Save(destination);
                }
            }
            Thread.Sleep(200);
           // File.Delete(source);

        }
    }
}
