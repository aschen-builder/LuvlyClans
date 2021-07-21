using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuvlyClans.Server
{
    class FileManager
    {
        public static string bpxConfigPath = BepInEx.Paths.BepInExConfigPath;
        public static string lcConfigPath = System.IO.Path.Combine(bpxConfigPath, LuvlyClans.PluginGUID + ".cfg");
    }
}
