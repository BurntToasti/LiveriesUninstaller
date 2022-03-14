using System.Text;
using Newtonsoft.Json.Linq;

namespace Uninstaller;

public class Livery
{
    FileInfo LiveryJson;
    JObject _liveryJsonJobj;
    public DirectoryInfo LiveryFolder = null;
    public string CustomSkinName { get; private set; }
    FileInfo[] _liveryFiles;


    public Livery(FileInfo json)
    {
        LiveryJson = json;
    }


    private void ReadJsonFile(ref FileInfo jsonFile, ref JObject jsonJobj)

    {
        bool failed = true;
        string fileString;
        List<Encoding> encoders = new List<Encoding>()
            {
                Encoding.Unicode,
                Encoding.UTF8,
                Encoding.BigEndianUnicode,
                Encoding.UTF7,
                Encoding.ASCII,
                Encoding.UTF32,
                new UTF32Encoding(true, true)
            };
        foreach (var encodingType in encoders)
        {
            try
            {
                fileString = File.ReadAllText(jsonFile.FullName, encodingType);
                jsonJobj = JObject.Parse(fileString);
                failed = false;
                return;
            }
            catch { }

        }
        if (failed)
        {
            Console.WriteLine("ALL ENCODERS FAILED, CONTACT TOASTER.");
        }
    }

    public void ReadJson()
    {
        ReadJsonFile(ref LiveryJson, ref _liveryJsonJobj);
        CustomSkinName = (string)_liveryJsonJobj["customSkinName"];
    }

    private void AddToList(ref List<Livery> list)
    {
        list.Add(this);
    }

    public void GenerateUninstallList(string eventCode, ref List<Livery> list)
    {
        int length = eventCode.Length;
        if (CustomSkinName.Length < length) { return; }

        bool add = true;
        for (int i = 0; i < length; i++)
        {
            if (eventCode[i] != CustomSkinName[i])
            {
                add = false;
                return;
            }
        }
        if (add) { AddToList(ref list); }
    }

    public void GetLiveryFiles()
    {
        _liveryFiles = LiveryFolder.GetFiles();
    }

    public void MoveFiles(ref int movedFiles, ref int unmovedFiles)
    {
        try
        {
            //move json to cars
            //move all files to new folder in uninstall liveries
            //delete if applicable 
            movedFiles++;
        }
        catch (Exception e)
        {
            unmovedFiles++;
        }

    }
}