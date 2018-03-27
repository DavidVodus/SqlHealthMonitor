namespace Common
{
    public interface IFileHelper
    {
        /// <summary>
        ///     Get all files in all directories and subdirectories
        /// </summary>
        /// <param name="targetDirectory"></param>
        /// <returns></returns>
        string[] GetFilesInDirectories(string targetDirectory);

        /// <summary>
        ///     Take source file path,replace part of source path with destination path then create directories if doesnt exist
        ///     then copy source file to modified source path base on destination ,then delete source file
        /// </summary>
        /// <param name="source">full file name path</param>
        /// <param name="replacedSourcePart">part of string from source string path</param>
        /// <param name="insteadOfReplacedPart">string which is placed instead of place replaceSourcePart</param>
        void MoveFiles(string source, string replacedSourcePart, string insteadOfReplacedPart);

        /// <summary>
        ///     Take destination file name zip archive,if directories doesnt exist,create them, then if zip archive exist
        ///     add source file to it,otherwise create one,finally delete source file
        /// </summary>
        /// <param name="source">full file name path</param>
        /// <param name="destination">full file name path of zip archive</param>
        void ZipFiles(string source, string destination);
    }
}