using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MMRando.Utils
{

    public class VCInjectionUtils
    {

        private static void GetApp5(byte[] ROM, string VCDir)
        {
            BinaryReader a50 = new BinaryReader(File.Open(VCDir + "5-0", FileMode.Open));
            BinaryReader a51 = new BinaryReader(File.Open(VCDir + "5-1", FileMode.Open));
            BinaryWriter app5 = new BinaryWriter(File.Open(VCDir + "00000005.app", FileMode.Create));
            byte[] buffer = new byte[a50.BaseStream.Length];
            a50.Read(buffer, 0, buffer.Length);
            app5.Write(buffer);
            app5.Write(ROM);
            buffer = new byte[a51.BaseStream.Length];
            a51.Read(buffer, 0, buffer.Length);
            app5.Write(buffer);
            a50.Close();
            a51.Close();
            app5.Close();
        }

        private static byte[] AddVCHeader(byte[] ROM)
        {
            byte[] Header = new byte[] { 0x08, 0x00, 0x00, 0x00 };
            return Header.Concat(ROM).ToArray();
        }

        public static void BuildVC(byte[] ROM, string VCDir, string FileName)
        {
            ROM = AddVCHeader(ROM);
            GetApp5(ROM, VCDir);
            ProcessStartInfo p = new ProcessStartInfo
            {
                FileName = "wadpacker.exe",
                Arguments = "mm.tik mm.tmd mm.cert \"" + FileName + "\" -i NMRE",
                WorkingDirectory = VCDir
            };
            Process.Start(p);
        }

    }

}