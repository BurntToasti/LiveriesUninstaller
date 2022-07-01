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
    static bool debug;
    static bool toasterMode;


    public static void Main(string[] args)
    {
        debug = false;
        toasterMode = false;

        foreach (var arg in args)
        {
            switch (arg)
            {
                case "/d":
                    debug = true;
                    Console.WriteLine("DEBUG MODE ON");
                    break;

                case "/t":
                    toasterMode = true;
                    Console.WriteLine("TOASTER MODE ON");
                    break;

            }
        }

        try
        {
            string appPath = new string("");
            if (toasterMode)
            {
                appPath = @"C:\Users\Tom\Desktop\testing\Customs";
            }
            else
            {
                appPath = Files.ExePath();
            }

            if (debug)
            {
                Console.WriteLine(appPath);
            }

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

            foreach (var livery in _liveries)
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

            }

            _toUninstallList = new List<Livery>();
            foreach (var livery in _liveries)
            {
                livery.GenerateUninstallList(_eventCode, ref _toUninstallList);
            }

            int movedJsonFiles = 0;
            int movedLiveryFolders = 0;
            int unmovedFiles = 0;
            int deletedLiveryFolders = 0;
            foreach (var livery in _toUninstallList)
            {
                livery.MoveFiles(
                    ref _uninstalledCars,
                    ref _uninstalledLiveries,
                    ref movedJsonFiles,
                    ref movedLiveryFolders,
                    ref deletedLiveryFolders,
                    ref unmovedFiles
                );
            }

            Console.WriteLine($"\nMoved {movedJsonFiles} car Json files.");
            Console.WriteLine($"Moved {movedLiveryFolders} livery folders.");
            Console.WriteLine($"Sucessfully deleted {deletedLiveryFolders} folders");
            Console.WriteLine("\nFinished, press ENTER to quit");

            Console.ReadLine();
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Console.ReadLine();
        }


    }
}