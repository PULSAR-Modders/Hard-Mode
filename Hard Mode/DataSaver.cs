using PulsarModLoader.SaveData;
using System.IO;

namespace Hard_Mode
{
    internal class DataSaver : PMLSaveData
    {
        public override uint VersionID => 160;

        public override string Identifier()
        {
            return "modders.hardmode";
        }

        public override void LoadData(byte[] Data, uint VersionID)
        {
            using (MemoryStream dataStream = new MemoryStream(Data))
            {
                using (BinaryReader binaryReader = new BinaryReader(dataStream))
                {
                    if (VersionID <= 140)
                    {
                        Options.FogOfWar = binaryReader.ReadBoolean();
                        Options.DangerousReactor = binaryReader.ReadBoolean();
                        Options.WeakReactor = binaryReader.ReadBoolean();
                        Options.SpinningCycpher = binaryReader.ReadBoolean();
                    }
                    if (VersionID >= 160)
                    {
                        Options.AdvancedCloak = binaryReader.ReadBoolean();
                    }
                }
            }
        }

        public override byte[] SaveData()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(stream))
                {
                    binaryWriter.Write(Options.FogOfWar);
                    binaryWriter.Write(Options.DangerousReactor);
                    binaryWriter.Write(Options.WeakReactor);
                    binaryWriter.Write(Options.SpinningCycpher);
                    binaryWriter.Write(Options.AdvancedCloak);
                }
                return stream.ToArray();
            }
        }
    }
}
