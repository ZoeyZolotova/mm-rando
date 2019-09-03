using System.Collections.ObjectModel;
using System.IO;

namespace MMRando.Constants
{
    public static class Values
    {
        public static string MusicDirectory = @"Resources\music\";
        public static string ModsDirectory = @"Resources\mods\";
        public static string AddrsDirectory = @"Resources\addresses\";
        public static string ObjsDirectory = @"Resources\models\";
        public static string VCDirectory = @"vc\";

        public const byte VanillaClockSpeed = 3;

        public static readonly uint[,] TatlColours = new uint[,] { // normal, npc, check, enemy, boss
            { 0xffffe6ff, 0xdca05000, 0x9696ffff, 0x9696ff00, 0x00ff00ff, 0x00ff0000, 0xffff00ff, 0xc89b0000, 0xffff00ff, 0xc89b0000 },
            { 0x200020ff, 0x80000000, 0x001080ff, 0x0080ff00, 0x104000ff, 0x80ff0000, 0x800000ff, 0x20002000, 0x800000ff, 0xff800000 },
            { 0xffc0e0ff, 0xff00ff00, 0xe040ffff, 0xff000000, 0xff80ffff, 0xff00ff00, 0xffe000ff, 0xff000000, 0xff0000ff, 0xff000000 },
            { 0xc0ffffff, 0x0000ff00, 0xffffffff, 0x00ffff00, 0x00ffffff, 0x00ffff00, 0xc080ffff, 0x0000ff00, 0x8080ffff, 0x0000ff00 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
        };

        public static readonly ReadOnlyCollection<int> OldEntrances
            = new ReadOnlyCollection<int>(new int[] {
            0x3000, 0x3C00, 0x2A00, 0x8C00
        });

        public static readonly ReadOnlyCollection<int> OldExits
            = new ReadOnlyCollection<int>(new int[] {
            0x8610, 0xB210, 0xAC10, 0x6A70
        });

        public static readonly ReadOnlyCollection<int> OldDCFlags
            = new ReadOnlyCollection<int>(new int[] {
                0x57C, 0x589, 0x59C, 0x59F
        });

        public static readonly ReadOnlyCollection<int> OldMaskFlags
            = new ReadOnlyCollection<int>(new int[] {
                0x02, 0x80, 0x20, 0x80
        });

    }
}
