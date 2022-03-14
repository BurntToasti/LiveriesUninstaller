using System;
using System.IO;
using System.Diagnostics;

namespace Uninstaller;
public static class Uninstaller
{
    static DirectoryInfo _appDi;
    static DirectoryInfo _carDi;
    static DirectoryInfo _liveriesDi;
    static FileInfo[] _carJsons;
    static DirectoryInfo[] _liveryFolders;
    static DirectoryInfo _uninstalledCars;
    static DirectoryInfo _uninstalledLiveries;
    static string _eventCode;
    static List<Livery> _liveries;
    static List<Livery> _toUninstallList;


    public static void Main()
    {
        //string appPath = Files.ExePath();
        string appPath = @"C:\Users\Tom\Documents\Assetto Corsa Competizione\Customs";
        _appDi = new DirectoryInfo(appPath);

        // Find all files and directories
        if (!Files.GetDirectories(ref _appDi, ref _carDi, ref _liveriesDi)) { return; }
        if (!Files.GetFiles(ref _carDi, ref _liveriesDi, ref _carJsons, ref _liveryFolders)) { return; }

        // Make output folders if it don't exist bruh
        Files.NewFolder(ref _appDi, "UninstalledLiveries");
        DirectoryInfo uninstallDi = new DirectoryInfo(_appDi.FullName + @"\UninstalledLiveries");
        Files.NewFolder(ref _appDi, @"UninstalledLiveries\Cars");
        Files.NewFolder(ref _appDi, @"UninstalledLiveries\Liveries");

        _uninstalledCars = uninstallDi.GetDirectories("Cars")[0];
        _uninstalledLiveries = uninstallDi.GetDirectories("Liveries")[0];

        Console.WriteLine("Please enter the AOR event code to remove.");
        Console.WriteLine("E.g EC_S2");
        _eventCode = Console.ReadLine();

        // Add all car livery jsons to an list
        _liveries = new List<Livery>();
        foreach (var carJson in _carJsons)
        {
            _liveries.Add(new Livery(carJson));
        }

        Parallel.ForEach(_liveries, livery =>
        {
            livery.ReadJson();
            if (livery.CustomSkinName != "")
            {
                try
                {
                    livery.LiveryFolder = _liveriesDi.GetDirectories(livery.CustomSkinName)[0];
                }
                catch (Exception e)
                {

                }
                livery.GetLiveryFiles();
            }

        });

        foreach (var livery in _liveries)
        {
            livery.GenerateUninstallList(_eventCode, ref _toUninstallList);
        }

        int movedFiles = 0;
        int unmovedFiles = 0;
        foreach (var livery in _toUninstallList)
        {
            livery.MoveFiles(ref movedFiles, ref unmovedFiles);
        }


        Console.ReadLine();

    }
}