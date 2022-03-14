using System.Diagnostics;

namespace Uninstaller;

public static class Files
{
    public static bool GetDirectories(
        ref DirectoryInfo app,
        ref DirectoryInfo cars,
        ref DirectoryInfo liveries
    )
    {
        try
        {
            cars = app.GetDirectories("?ars", SearchOption.TopDirectoryOnly)[0];
            liveries = app.GetDirectories("?iveries", SearchOption.TopDirectoryOnly)[0];
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }


    }

    public static bool GetFiles(
        ref DirectoryInfo carDi,
        ref DirectoryInfo liveriesDi,
        ref FileInfo[] json,
        ref DirectoryInfo[] liveryFolders
    )
    {
        try
        {
            json = carDi.GetFiles("*.json", SearchOption.TopDirectoryOnly);
            liveryFolders = liveriesDi.GetDirectories("*", SearchOption.TopDirectoryOnly);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

    }

    public static string ExePath()
    {
        string path = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
        if (path == null)
        {
            return "";
        }
        return path;
    }

    public static void NewFolder(ref DirectoryInfo rootDi, string folderName)
    {
        string newPath = rootDi.FullName + @"\" + folderName;
        try { Directory.CreateDirectory(newPath); }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}