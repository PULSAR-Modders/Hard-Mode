using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PulsarPluginLoader;

namespace Hard_Mode
{
    class Options //For all future options
    {
        public static bool FogOfWar = false;
        public static bool DangerousReactor = false;  
    }

    class ReciveOptions : ModMessage //Recive the options from the master client
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            Options.FogOfWar = (bool)arguments[0];
            Options.DangerousReactor = (bool)arguments[1];
        }
    }
}
