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
    public static bool debug;
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
                Console.WriteLine("Manually add Customs path");
                appPath = Console.ReadLine();
                // appPath = @"C:\Users\Tom\Desktop\testing\Customs";
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
            bool findDir = Files.GetDirectories(ref _appDi, ref _carDi, ref _liveriesDi);
            if (debug) { Debug.Directories(findDir); }
            if (!findDir) { return; }

            bool findFiles = Files.GetFiles(ref _carDi, ref _liveriesDi, ref _carJsons, ref _liveryFolders);
            if (debug) { Debug.Files(findDir); }
            if (!findFiles) { return; }

            // Make output folders if it don't exist bruh
            Files.NewFolder(ref _appDi, "UninstalledLiveries");
            DirectoryInfo uninstallDi = new DirectoryInfo(_appDi.FullName + @"\UninstalledLiveries");
            Files.NewFolder(ref _appDi, @"UninstalledLiveries\Cars");
            Files.NewFolder(ref _appDi, @"UninstalledLiveries\Liveries");
            if (debug) { Debug.FoldersCreated(true); }

            _uninstalledCars = uninstallDi.GetDirectories("Cars")[0];
            _uninstalledLiveries = uninstallDi.GetDirectories("Liveries")[0];

            Console.WriteLine("Please enter the AOR event code to remove.");
            Console.WriteLine("E.g SX_GT3");
            _eventCode = Console.ReadLine();

            // Add all car livery jsons to an list
            _liveries = new List<Livery>();
            int number = 0;
            foreach (var carJson in _carJsons)
            {
                number++;
                _liveries.Add(new Livery(carJson));
            }
            if (debug) { Debug.AddJson(number); }

            int livNumber = 0;
            foreach (var livery in _liveries)
            {
                livery.ReadJson();
                if (livery.CustomSkinName != "")
                {
                    try
                    {
                        livery.LiveryFolder = _liveriesDi.GetDirectories(livery.CustomSkinName)[0];
                        livery.GetLiveryFiles();
                        livNumber++;
                    }
                    catch (Exception e)
                    {
                        if (debug)
                        {
                            Console.WriteLine(e);
                        }
                    }

                }
            }
            if (debug) { Debug.LiveryFilesRead(livNumber); }

            _toUninstallList = new List<Livery>();
            foreach (var livery in _liveries)
            {
                livery.GenerateUninstallList(_eventCode, ref _toUninstallList);
            }
            if (debug) { Debug.FilesToRemove(_toUninstallList); }

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