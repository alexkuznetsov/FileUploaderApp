using System.IO;
using System.Linq;

namespace FileUploadApp.Storage.Filesystem;

internal static class Dir
{
    public static bool IsDirectoryEmpty(string path)
    {
        return !Directory.EnumerateFileSystemEntries(path).Any();
    }

    public static void Delete(string path) => Directory.Delete(path);
}