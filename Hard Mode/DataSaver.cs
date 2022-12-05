using PulsarModLoader.SaveData;
using System.IO;

namespace Hard_Mode
{
    internal class DataSaver : PMLSaveData
    {
        public override uint VersionID => 140;

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
                    Options.FogOfWar = binaryReader.ReadBoolean();
                    Options.DangerousReactor = binaryReader.ReadBoolean();
                    Options.WeakReactor = binaryReader.ReadBoolean();
                    Options.SpinningCycpher = binaryReader.ReadBoolean();
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
                }
                return stream.ToArray();
            }
        }
    }
}
