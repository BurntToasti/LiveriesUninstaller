using System.IO;
namespace Uninstaller;

public static class Debug
{

    public static void Directories(bool found)
    {
        if (found)
        {
            Console.WriteLine("Livery Directories Found");
        }
        else
        {
            Console.WriteLine("Livery Directories Not Found");
        }
    }

    public static void Files(bool found)
    {
        if (found)
        {
            Console.WriteLine("Car Files Found");
        }
        else
        {
            Console.WriteLine("Car Files Not Found");
        }
    }

    public static void FoldersCreated(bool created)
    {
        if (created)
        {
            Console.WriteLine("Uninstall Folders Created");
        }
    }

    public static void AddJson()
    {
        Console.WriteLine("Json Files Stored");
    }

    public static void LiveryFilesRead()
    {
        Console.WriteLine("Livery Files Read");
    }

    public static void FilesToRemove()
    {
        Console.WriteLine("Liveries to remove stored");
    }
}