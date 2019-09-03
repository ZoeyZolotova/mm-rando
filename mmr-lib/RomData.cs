using MMRando.Models.Rom;
using System.Collections.Generic;

namespace MMRando
{
    public static class RomData
    {
        public static List<MMFile> MMFileList { get; set; }
        public static List<Scene> SceneList { get; set; }
        public static Dictionary<int, GetItemEntry> GetItemList { get; set; }
        public static Dictionary<int, BottleCatchEntry> BottleList { get; set; }
    }
}