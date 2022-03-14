using System;
using System.IO;
using System.Diagnostics;

namespace Uninstaller;
public static class Uninstaller
{
    static DirectoryInfo appDi;
    static DirectoryInfo carDi;
    static DirectoryInfo liveriesDi;
    static FileInfo[] carJsons;
    static DirectoryInfo[] liveryFolders;


    public static void Main()
    {
        // string appPath = ExePath();
        string appPath = @"C:\Users\Tom\Documents\Assetto Corsa Competizione\Customs";
        DirectoryInfo appDi = new DirectoryInfo(appPath);

        if (!Files.GetDirectories(ref appDi, ref carDi, ref liveriesDi)) { return; }
        if (!Files.GetFiles()) { return; }


        //     FileInfo[] carJsons = carsDi.GetFiles("*.json", SearchOption.TopDirectoryOnly);
        //     DirectoryInfo[] liveryFolders = liveriesDi.GetDirectories("*", SearchOption.TopDirectoryOnly);
        //     Console.WriteLine(carJsons.Length);
        //     Console.WriteLine(liveryFolders.Length);
        // }
        // catch (Exception e)
        // {
        //     Console.WriteLine(e);
        // }



        // Console.ReadLine();

    }
}