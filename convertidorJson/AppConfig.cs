using System;
using System.IO;
using System.Reflection.Metadata.Ecma335;

class AppConfig
{
    public static string InputFolder
    {
        get
        {
            string basePath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;
            string fullPath = Path.Combine(basePath, "DATOS", "in");

            return Path.GetFullPath(fullPath);
        }
    }
}
